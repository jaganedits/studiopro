using System.Net;
using Service.Models;

namespace Service.Helper.FileUploadService;

public interface IFileUploadService
{
    Task UploadFile(IFormFile file, string folder);
    Task UploadFiles(List<IFormFile> files, string folder);
    Task<string> UploadFileWithUrl(IFormFile file, string folderPath);
    bool IsFileExists(string fileName, string folderName);
    Task<string> UploadFile(IFormFile file, string physicalPath, string folderPath, string subdomainpath, string userName, string password, bool isCloud);
}

public class FileUploadService : IFileUploadService
    {
        private readonly DbContextHelper dbContext;
        private IWebHostEnvironment _webHostEnvironment;
        private readonly IConfiguration _Configuration;
        private readonly IHttpContextAccessor httpContextAccessor;

        public FileUploadService(DbContextHelper DbContextHelper, IWebHostEnvironment webHostEnvironment, IConfiguration Configuration,
        IHttpContextAccessor _httpContextAccessor)
        {
            this.dbContext = DbContextHelper;
            httpContextAccessor = _httpContextAccessor;
            _webHostEnvironment = webHostEnvironment; // has ContentRootPath property
            _Configuration = Configuration;
        }

        /// <summary>
        /// Uploads file and returns full URL for web access
        /// </summary>
        public async Task<string> UploadFileWithUrl(IFormFile file, string folderPath)
        {
            if (file == null || file.Length == 0)
                return null;

            // Physical path for saving file - matches Program.cs static file config
            var physicalPath = Path.Combine(Directory.GetCurrentDirectory(), "Upload", folderPath);

            if (!Directory.Exists(physicalPath))
                Directory.CreateDirectory(physicalPath);

            var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(physicalPath, uniqueFileName);

            using (var stream = File.Create(filePath))
            {
                await file.CopyToAsync(stream);
            }

            // Build full URL for web access
            var request = httpContextAccessor.HttpContext?.Request;
            var baseUrl = $"{request?.Scheme}://{request?.Host}";
            var urlPath = $"Upload/{folderPath}/{uniqueFileName}".Replace("\\", "/");

            return $"{baseUrl}/{urlPath}";
        }

        public async Task UploadFile(IFormFile file, string folder)
        {
            IConfigurationSection section = _Configuration.GetSection("paths");
            string str = section.Value;
            var webRootPath = _webHostEnvironment.ContentRootPath + str + "\\" + folder;
            if (!Directory.Exists(webRootPath))
                Directory.CreateDirectory(webRootPath);
            var formFile = file;
            {
                if (formFile.Length > 0)
                {
                    var filePath = Path.Combine(webRootPath, formFile.FileName);
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }
        }

        public async Task UploadFiles(List<IFormFile> files, string folder)
        {
            IConfigurationSection section = _Configuration.GetSection("paths");
            string str = section.Value;
            var webRootPath = _webHostEnvironment.ContentRootPath + str + "\\" + folder;

            if (!Directory.Exists(webRootPath))
                Directory.CreateDirectory(webRootPath);

            //var formFile = file;
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {

                    var filePath = Path.Combine(webRootPath, formFile.FileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }
        }

        public bool IsFileExists(string fileName, string folderName)
        {
            IConfigurationSection section = _Configuration.GetSection("paths");
            string str = section.Value;
            var webRootPath = _webHostEnvironment.ContentRootPath + str + "\\" + folderName + "\\" + fileName;
            return File.Exists(webRootPath);
        }


        public async Task<string> UploadFile(IFormFile file, string physicalPath, string folderPath, string subdomainpath, string userName, string password, bool isCloud)
        {
            string absoluteUrl = null;
            if (isCloud == false)
            {
                string scheme = httpContextAccessor.HttpContext.Request.Scheme + "://" + httpContextAccessor.HttpContext.Request.Host.Value + "/upload/" + folderPath + "/";

                UploadFile(file, folderPath);

                string urlPath = scheme + file.FileName;
                absoluteUrl = $"{urlPath}";
            }
            else
            {
                using (new NetworkConnection(physicalPath, new NetworkCredential(userName, password)))
                {

                    var combinePath = Path.Combine(physicalPath, folderPath);

                    if (!Directory.Exists(combinePath))
                        Directory.CreateDirectory(combinePath);

                    var filePath = Path.Combine(combinePath, file.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }


                    string urlPath = Path.Combine(folderPath, file.FileName).Replace("\\", "/");
                    absoluteUrl = $"{subdomainpath}/{urlPath}";


                }
            }
            return absoluteUrl;
        }

        public async Task<string> QrCodeUploadFile(IFormFile file, string physicalPath, string folderPath, string subdomainpath, string userName, string password, bool isCloud)
        {

            // string userName = "administrator"; // Replace with your domain and username
            // string password = "leiten123*"; // Replace with your password

            // using (new NetworkConnection(physicalPath, new NetworkCredential(userName, password)))
            // {

            //     var combinePath = Path.Combine(physicalPath, folderPath);

            //     if (!Directory.Exists(combinePath))
            //         Directory.CreateDirectory(combinePath);

            //     var filePath = Path.Combine(combinePath, file.FileName);

            //     using (var stream = new FileStream(filePath, FileMode.Create))
            //     {
            //         await file.CopyToAsync(stream);
            //     }
            //     var returnpath = Path.Combine(subdomainpath, file.FileName);

            //     return returnpath;
            // }
            string absoluteUrl = null;
            if (isCloud == false)
            {
                string scheme = httpContextAccessor.HttpContext.Request.Scheme + "://" + httpContextAccessor.HttpContext.Request.Host.Value + "/upload/" + folderPath + "/";

                UploadFile(file, folderPath);

                string urlPath = scheme + file.FileName;
                absoluteUrl = $"{urlPath}";
            }
            else
            {
                using (new NetworkConnection(physicalPath, new NetworkCredential(userName, password)))
                {

                    var combinePath = Path.Combine(physicalPath, folderPath);

                    if (!Directory.Exists(combinePath))
                        Directory.CreateDirectory(combinePath);

                    var filePath = Path.Combine(combinePath, file.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }


                    string urlPath = Path.Combine(folderPath, file.FileName).Replace("\\", "/");
                    absoluteUrl = $"{subdomainpath}/{urlPath}";


                }
            }
            return absoluteUrl;
        }


        public class NetworkConnection : IDisposable
        {
            private string networkPath;

            public NetworkConnection(string networkPath, NetworkCredential credentials)
            {
                this.networkPath = networkPath;
                var processStartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "net",
                    Arguments = $"use {networkPath} {credentials.Password} /user:{credentials.UserName}",
                    CreateNoWindow = true,
                    UseShellExecute = false
                };
                System.Diagnostics.Process.Start(processStartInfo).WaitForExit();
            }

            public void Dispose()
            {
                var processStartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "net",
                    Arguments = $"use {networkPath} /delete",
                    CreateNoWindow = true,
                    UseShellExecute = false
                };
                System.Diagnostics.Process.Start(processStartInfo).WaitForExit();
            }
        }

    }