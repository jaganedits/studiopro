using Service.DTO.Common;
using Service.Utils;

namespace Service.Services.Helper.MailService;

public interface IMailService
{
    Task SendEmailOTP(object obj);
    Task SendEmail(object obj);
    Task<CustomResponse> SendEmailWithAttachment(MailRequest mailrequest, object obj, string htmlpath,string emailid,string emailpassword);
    Task SendEmailsingleWithAttachment(MailRequest mailrequest, object obj, string htmlpath,string emailid,string emailpassword);

}