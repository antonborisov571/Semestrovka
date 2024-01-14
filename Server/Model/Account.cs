using Framework.AccountData;
using System;
using System.Collections.Generic;
using ORM.ORMPostgres;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model;

[Table("Accounts")]
public class Account : Framework.AccountData.Account
{
    public string TractorName { get; set; }
    public string UrlImage { get; set; }
    public DateTime DateRegistration { get; set; }
    public DateTime LastOnline { get; set; }
    public int Reputation { get; set; }
}
