using Service.Utils;

namespace Service.DTO.Common;

 public class MailDTO
    {
        public MailDTO()
        {
            tranStatus = new ErrorContext();
            tranStatus.result = false;
            tranStatus.lstErrorItem = new List<ErrorItem>();
        }

        public ErrorContext tranStatus { get; set; }
        public List<dynamic> Ecmaildetails { get; internal set; }
    }
    public class OrderConfirmationEmail
    {
        public string ToEmail { get; set; }
        public string UserName { get; set; }
        public string Logo { get; set; }
        public string Orderno { get; set; }
    }
    public class MailSettings
    {
        public string Mail { get; set; }
        public string DisplayName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string Website { get; set; }
        public string Service { get; set; }
        public string WhatsAppAPIKey { get; set; }
        public string WhatsAppAPIUrl { get; set; }
    }
    public class MailRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<Attachment> Attachments { get; set; }
        public List<Stream> AttachmentStream { get; set; }
        public List<String> ToEmailArr { get; set; }

    }

    public class SMSResponse
    {
        public string originator { get; set; }
        public string destination { get; set; }
        public string messageText { get; set; }
        public string messageId { get; set; }
        public string messageReference { get; set; }
        public string status { get; set; }
        public string messageDate { get; set; }
        public string charge { get; set; }
        public string scheduled { get; set; }
        public string messageValidity { get; set; }
        public string sendDateTime { get; set; }
    }
    public class Attachment
    {
        public string Filename { get; set; }
        public byte[] Content { get; set; }
        public string ContentType { get; set; }
    }
