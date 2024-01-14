namespace Framework.Server.CookieAndSession;

public class Session
{
    public Guid Id { get; set; }
    public bool Unlimited { get; set; }
    public int AccountId { get; set; }
    public string Login { get; set; }
    public DateTime CreateDateTime { get; set; }
    public DateTime? Expires { get; set; }
}
