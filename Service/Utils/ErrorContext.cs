namespace Service.Utils;

public class ErrorContext
{
    public bool result { get; set; }
    public List<ErrorItem> lstErrorItem { get; set; }

}
public class ErrorItem
{
    public string ErrorNo { get; set; }
    public string Message { get; set; }
}