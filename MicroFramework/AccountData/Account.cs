using ORM.ORMPostgres;

namespace Framework.AccountData;


[Table("Accounts")]
public class Account : IEntity
{
    public int Id { get; set; }
    public string Login { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Salt { get; set; }
}