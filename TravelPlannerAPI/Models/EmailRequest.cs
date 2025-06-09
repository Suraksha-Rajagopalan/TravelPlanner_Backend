public class EmailRequest
{
    public string AccessToken { get; set; }
    public string FromEmail { get; set; }
    public string ToEmail { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}
