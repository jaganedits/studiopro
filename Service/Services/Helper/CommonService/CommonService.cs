using System.Text;
using Newtonsoft.Json.Linq;
using Service.ContextHelpers;
using Service.DTO.Common;
using Service.Helper.FileUploadService;
using Service.Services.Helper.MailService;
using Service.Utils;

namespace Service.Services.Helper.CommonService;

 public class CommonService : ICommonService
    {
        private readonly CustomResponse res = new CustomResponse();
        private readonly IDapperContext dapperContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly CommonDTO commonDTO = new CommonDTO();

        private readonly IFileUploadService fileUploadService;

        public CommonService
        (
            IHttpContextAccessor _httpContextAccessor,
            IDapperContext _dapperContext,
            IFileUploadService _fileUploadService
        )
        {
            httpContextAccessor = _httpContextAccessor;
            dapperContext = _dapperContext;
            fileUploadService = _fileUploadService;
        }

        public async Task<CustomResponse> GetState(JObject obj)
        {
            int? countryId = obj["CountryId"]?.ToObject<int>();
            string type = "GetState";

            try
            {
                if (countryId == null)
                {
                    // Return a validation error response
                    res.IsSuccess = false;
                    res.Title = "Warning Message";
                    res.Message = "Country Name is required.";
                    return res;
                }

                using (dapperContext)
                {
                    var spcall = await dapperContext.ExecuteStoredProcedureAsync(spName: "SP_Common", new
                    {
                        countryId,
                        type
                    });

                    // commonDTO.stateList = (await spcall.ReadAsync<State>()).ToList();

                    res.IsSuccess = true;
                    res.Message = "States retrieved successfully.";
                    res.Data = commonDTO;
                }
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.Title = "Error";
                res.Message = ex.Message;
            }

            return res;
        }
        public async Task<CustomResponse> GetCity(JObject obj)
        {
            int? stateId = obj["StateId"]?.ToObject<int>();
            string type = "GetCity";

            try
            {
                if (stateId == null)
                {
                    // Return a validation error response
                    res.IsSuccess = false;
                    res.Title = "Warning Message";
                    res.Message = "State Name is required.";
                    return res;
                }

                using (dapperContext)
                {
                    var spcall = await dapperContext.ExecuteStoredProcedureAsync(spName: "SP_Common", new
                    {
                        countryId = (object)null,
                        stateId,
                        type
                    });

                    // commonDTO.cityList = (await spcall.ReadAsync<City>()).ToList();

                    res.IsSuccess = true;
                    res.Message = "Cities retrieved successfully.";
                    res.Data = commonDTO;
                }
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.Title = "Error";
                res.Message = ex.Message;
            }

            return res;
        }
        public async Task<string> GenerateUniqueCode(int Documentid)
        {
            string documentno = "";
            int documentid = Documentid;
            CustomResponse res = new CustomResponse();
            using (dapperContext)
            {

                var spcall = await dapperContext.ExecuteStoredProcedureAsync(spName: "GetPrimaryKey", new
                {
                    documentid,
                });

                documentno = (await spcall.ReadAsync<string>()).SingleOrDefault();
            }
            return documentno;
        }
        public async Task<string> ProcessToken(JObject obj)
        {
            var resultValue = "";
            try
            {
                // Extract tokenType from the JObject
                var tokenType = obj["tokenType"]?.ToString();
                var encryptedToken = obj["processParam"]?.ToString(); // Assuming "processParam" contains the token

                if (tokenType == "ENCRYPT")
                {
                    // Call service to generate an encrypted token
                    resultValue = ApproveTokenService.GenerateToken(encryptedToken);
                }
                else if (tokenType == "DECRYPT")
                {
                    // Call service to decrypt the token
                    resultValue = ApproveTokenService.DecryptToken(encryptedToken);
                }
            }
            catch (Exception ex)
            {

            }
            return resultValue;
        }
        public static string TranslateCurrency(decimal currencyValue, string currencyCode)
        {
            string numericCurrency = currencyValue.ToString().Trim();

            // Check for the max capacity limit of the input
            if (numericCurrency.Length > 18)
                return "Invalid input format";

            string leftValue,
                decimalWord;
            // Right align the characters with padding of "0" to length the of 18 characters
            if (numericCurrency.IndexOf(".") >= 0)
            {
                leftValue = numericCurrency
                    .Substring(0, numericCurrency.IndexOf("."))
                    .PadLeft(18, '0');
                decimalWord = numericCurrency
                    .Substring(numericCurrency.IndexOf(".") + 1)
                    .PadRight(2, '0');
                decimalWord = (decimalWord.Length > 2 ? decimalWord.Substring(0, 2) : decimalWord);
            }
            else
            {
                leftValue = numericCurrency.PadLeft(18, '0');
                decimalWord = "0";
            }

            string quadrillionWord,
                trillionWord,
                billionWord,
                millionWord,
                thousandWord,
                hundredWord;
            quadrillionWord = TranslateHundreds(Convert.ToInt32(leftValue.Substring(0, 3))); // One Quadrillion - Number of zeros 15
            trillionWord = TranslateHundreds(Convert.ToInt32(leftValue.Substring(3, 3))); // One Trillion - Number of zeros 12
            billionWord = TranslateHundreds(Convert.ToInt32(leftValue.Substring(6, 3))); // One Billion - Number of zeros 9
            millionWord = TranslateHundreds(Convert.ToInt32(leftValue.Substring(9, 3))); // One Million - Number of zeros 6
            thousandWord = TranslateHundreds(Convert.ToInt32(leftValue.Substring(12, 3)));
            hundredWord = TranslateHundreds(Convert.ToInt32(leftValue.Substring(15, 3)));
            decimalWord = TranslateDecimal(decimalWord);

            // Start building the currency
            StringBuilder currencyInEnglish = new StringBuilder();
            currencyInEnglish.Append(
                (
                    quadrillionWord.Trim() == string.Empty
                        ? string.Empty
                        : quadrillionWord + " Quadrillion "
                )
            );
            currencyInEnglish.Append(
                (trillionWord.Trim() == string.Empty ? string.Empty : trillionWord + " Trillion ")
            );
            currencyInEnglish.Append(
                (billionWord.Trim() == string.Empty ? string.Empty : billionWord + " Billion ")
            );
            currencyInEnglish.Append(
                (millionWord.Trim() == string.Empty ? string.Empty : millionWord + " Million ")
            );
            currencyInEnglish.Append(
                (thousandWord.Trim() == string.Empty ? string.Empty : thousandWord + " Thousand ")
            );
            currencyInEnglish.Append(
                (hundredWord.Trim() == string.Empty ? string.Empty : hundredWord)
            );

            currencyInEnglish.Append(currencyInEnglish.ToString() == string.Empty ? "Zero" : "");

            if (currencyCode == "USD")
            {
                currencyInEnglish.Append(" Dollars");
                if (currencyInEnglish.ToString().StartsWith("One Dollars"))
                {
                    currencyInEnglish.Replace("One Dollars", "One Dollar");
                }
            }
            else if (currencyCode == "INR")
            {
                currencyInEnglish.Append(" Rupees");
                if (currencyInEnglish.ToString().StartsWith("One Rupees"))
                {
                    currencyInEnglish.Replace("One Rupees", "One Rupee");
                }
            }

            currencyInEnglish.Append(
                (decimalWord == string.Empty ? " and Zero Cents" : " and " + decimalWord + " Cents")
            );
            return currencyInEnglish.ToString();
        }
        private static string TranslateDecimal(string value)
        {
            string result = string.Empty;
            // Check whether the decimal part starts with a zero. Example : 12.05
            if (value != "0")
            {
                if (value.StartsWith("0"))
                {
                    result = "Zero ";
                    result += _arrayOfOnes[Convert.ToInt32(value.Substring(1, 1))];
                }
                else
                {
                    result = TranslateTens(Convert.ToInt32(value));
                }
            }
            return result;
        }

        private static string TranslateHundreds(int value)
        {
            string result;
            // If the value is less than hundred then convert it as tens
            if (value <= 99)
            {
                result = (TranslateTens(value));
            }
            else
            {
                result = _arrayOfOnes[Convert.ToInt32(Math.Floor(Convert.ToDecimal(value / 100)))];
                // Math.Floor method is used to get the largest integer from the decial value
                result += " Hundred ";
                // The rest of the decimal is converted into tens
                if (value - Math.Floor(Convert.ToDecimal((value / 100) * 100)) == 0)
                {
                    result += string.Empty;
                }
                else
                {
                    result +=
                        string.Empty
                        + TranslateTens(
                            Convert.ToInt32(
                                value - Math.Floor(Convert.ToDecimal((value / 100) * 100))
                            )
                        );
                }
            }
            return result;
        }

        private static string TranslateTens(int value)
        {
            string result;
            // If the value is less than 20 then get the word directly from the array
            if (value < 20)
            {
                result = _arrayOfOnes[value];
                value = 0;
            }
            else
            {
                result = _arrayOfTens[(int)Math.Floor(Convert.ToDecimal(value / 10))];
                value -= Convert.ToInt32(Math.Floor(Convert.ToDecimal((value / 10) * 10)));
            }
            result =
                result
                + (
                    _arrayOfOnes[value].Trim() == string.Empty
                        ? string.Empty
                        : "-" + _arrayOfOnes[value]
                );
            return result;
        }

        private static string[] _arrayOfOnes =
        {
            string.Empty,
            "One",
            "Two",
            "Three",
            "Four",
            "Five",
            "Six",
            "Seven",
            "Eight",
            "Nine",
            "Ten",
            "Eleven",
            "Twelve",
            "Thirteen",
            "Fourteen",
            "Fifteen",
            "Sixteen",
            "Seventeen",
            "Eighteen",
            "Nineteen",
        };

        // Array of string to hold the words of tens - 10, 20,.., 90
        private static string[] _arrayOfTens =
        {
            string.Empty,
            "Ten",
            "Twenty",
            "Thirty",
            "Forty",
            "Fifty",
            "Sixty",
            "Seventy",
            "Eighty",
            "Ninety",
        };

        internal static int ToFinancialYear(DateTime dateTime)
        {
            return (dateTime.Month >= 4 ? dateTime.Year + 1 : dateTime.Year);
        }

        internal static string ToFinancialYearShort(DateTime dateTime)
        {
            return (
                dateTime.Month >= 4 ? dateTime.AddYears(1).ToString("yy") : dateTime.ToString("yy")
            );
        }

        internal static string GetLast(string source, int tail_length)
        {
            if (tail_length >= source.Length)
                return source;
            return source.Substring(source.Length - tail_length);
        }
        public string ConvertToWords1(decimal amount, string currencyCode)
        {
            string result = string.Empty;
            currencyCode = currencyCode.ToUpper().Trim();

            if (currencyCode == "USD" || currencyCode == "INR")
            {
                result = TranslateCurrency(amount, currencyCode);
            }
            else
            {
                result = "Unsupported currency code.";
            }

            return result;
        }
        // public async Task<string> QrCodeCreation(IUnitOfWork uow, string encriptcodes, string qrUniqueCode)
        // {
        //     string uploadedFileName = "";
        //     var uploadDocument = (await uow.GetRepository<UploadDocument>().QueryAsync(x => x.Screenid == 0)).SingleOrDefault();
        //     QRCodeGenerator qrGenerator = new QRCodeGenerator();
        //     QRCodeData qrCodeData = qrGenerator.CreateQrCode(encriptcodes, QRCodeGenerator.ECCLevel.Q);
        //     QRCode qrCode = new QRCode(qrCodeData);

        //     var uploadDocumentRepo = uow.GetRepository<UploadDocument>();
        //     int screenQRId = 0;
        //     var uploadQRCode = (await uploadDocumentRepo.QueryAsync(x => x.Screenid == screenQRId)).SingleOrDefault();

        //     if (uploadQRCode == null)
        //     {
        //         {
        //             res.IsSuccess = false;
        //             res.Title = "Error";
        //             res.Message = "Upload configuration not found for the given screen.";
        //         };
        //     }

        //     using (var memoryStream = new MemoryStream())
        //     {
        //         using (Bitmap qrBitmap = qrCode.GetGraphic(5))
        //         {
        //             qrBitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
        //         }

        //         memoryStream.Position = 0;
        //         var file = new FormFile(memoryStream, 0, memoryStream.Length, null, $"{qrUniqueCode}_QRCode.png")
        //         {
        //             Headers = new HeaderDictionary(),
        //             ContentType = "image/png"
        //         };

        //         var physicalPath = uploadQRCode.Physicalpath;
        //         var folderPath = uploadQRCode.Folderpath;
        //         var subdomainpath = uploadQRCode.Subdomainpath;
        //         var userName = uploadQRCode.Username;
        //         var password = uploadQRCode.Keys;
        //         var isCloud = uploadQRCode.Iscloud;

        //         uploadedFileName = await fileUploadService.UploadFile(file, physicalPath, folderPath, subdomainpath, userName, password, isCloud);
        //     }

        //     return uploadedFileName;
        // }
    }