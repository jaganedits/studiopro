using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Newtonsoft.Json.Linq;
using Service.DTO.Common;
using Service.Models;
using Service.Utils;
using Wkhtmltopdf.NetCore;

namespace Service.Services.Helper.MailService;

public class MailService : IMailService
    {
        private readonly DbContextHelper DbContext;
        private readonly IGeneratePdf _generatePdf;
        private readonly CustomResponse res = new CustomResponse();
        private readonly IHttpContextAccessor _httpContextAccessor;
        private MailDTO dto;
        private IConfiguration configuration;

        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly MailSettings _mailSettings;

        public MailService(DbContextHelper _dbContextHelper, IWebHostEnvironment webHostEnvironment, IOptions<MailSettings> mailSettings, IHttpContextAccessor httpContextAccessor, IConfiguration _configuration, IGeneratePdf generatePdf)
        {

            this._webHostEnvironment = webHostEnvironment;
            _generatePdf = generatePdf;
            DbContext = _dbContextHelper;
            _httpContextAccessor = httpContextAccessor;
            dto = new MailDTO();
            configuration = _configuration;
            this._mailSettings = mailSettings.Value;
        }
        public async Task SendEmailOTP(object obj)
        {
            try
            {
                Newtonsoft.Json.Linq.JObject emailObj = (Newtonsoft.Json.Linq.JObject)obj;
                Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                string fromid = serializer.Deserialize<string>(new JTokenReader(emailObj["FromID"]));
                string toId = serializer.Deserialize<string>(new JTokenReader(emailObj["ToID"]));
                string subject = serializer.Deserialize<string>(new JTokenReader(emailObj["Subject"]));
                string template = serializer.Deserialize<string>(new JTokenReader(emailObj["Template"]));

                var email = new MimeMessage();

                email.From.Add(MailboxAddress.Parse(_mailSettings.Mail));
                email.To.Add(MailboxAddress.Parse(toId));
                email.Subject = subject;

                var builder = new BodyBuilder();
                builder.HtmlBody = template;
                email.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();
                // SSL certificate validation enabled for security (remove bypass) // bypass SSL certificate validation

                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public async Task SendEmail(object obj)
        {
            try
            {
                Newtonsoft.Json.Linq.JObject emailObj = (Newtonsoft.Json.Linq.JObject)obj;
                Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
                string fromid = serializer.Deserialize<string>(new JTokenReader(emailObj["FromID"]));
                string toId = serializer.Deserialize<string>(new JTokenReader(emailObj["ToID"]));
                string subject = serializer.Deserialize<string>(new JTokenReader(emailObj["Subject"]));
                string template = serializer.Deserialize<string>(new JTokenReader(emailObj["Template"]));

                var email = new MimeMessage();

                email.From.Add(MailboxAddress.Parse(_mailSettings.Mail));
                email.To.Add(MailboxAddress.Parse(toId));
                email.Subject = subject;

                var builder = new BodyBuilder();
                builder.HtmlBody = template;
                email.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();
                // SSL certificate validation enabled for security (remove bypass) // bypass SSL certificate validation

                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<CustomResponse> SendEmailWithAttachment(MailRequest mailrequest, object obj, string htmlpath,string emailid,string emailpassword)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_mailSettings.Mail));
                //email.To.Add(MailboxAddress.Parse(mailrequest.ToEmail));
                foreach (var toEmail in mailrequest.ToEmailArr)
                {
                    email.To.Add(MailboxAddress.Parse(toEmail));
                }

                email.Subject = mailrequest.Subject;

                var builder = new BodyBuilder
                {
                    HtmlBody = mailrequest.Body
                };


                var path = htmlpath;

                byte[] pdf = await _generatePdf.GetByteArray(path, obj);

                var attachment = new MimePart("application", "pdf")
                {
                    FileName = mailrequest.Attachments[0].Filename,
                    Content = new MimeContent(new MemoryStream(pdf)),
                    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Base64
                };

                builder.Attachments.Add(attachment);

                email.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();
                // SSL certificate validation enabled for security (remove bypass)
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                // smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                smtp.Authenticate(emailid, emailpassword);
               var mailRes= await smtp.SendAsync(email);
                smtp.Disconnect(true);

                res.IsSuccess = true;
                res.Message = "Mail sent successfully";

            }
            catch (Exception ex)
            {

                res.IsSuccess = false;
                res.Message = $"Email sending failed: {ex.Message}";

            }

            return res;
        }
        public async Task SendEmailsingleWithAttachment(MailRequest mailrequest, object obj, string htmlpath,string emailid,string emailpassword)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_mailSettings.Mail));
            //email.To.Add(MailboxAddress.Parse(mailrequest.ToEmail));
            email.To.Add(MailboxAddress.Parse(mailrequest.ToEmail));

            email.Subject = mailrequest.Subject;

            var builder = new BodyBuilder
            {
                HtmlBody = mailrequest.Body
            };

            var path = htmlpath;

            byte[] pdf = await _generatePdf.GetByteArray(path, obj);

            var attachment = new MimePart("application", "pdf")
            {
                FileName = mailrequest.Attachments[0].Filename,
                Content = new MimeContent(new MemoryStream(pdf)),
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64
            };

            builder.Attachments.Add(attachment);

            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            // SSL certificate validation enabled for security (remove bypass)
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            // smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            smtp.Authenticate(emailid, emailpassword);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }


    }