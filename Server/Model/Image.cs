using ORM.ORMPostgres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model;

[Table("Images")]
public class Image : IEntity
{
    public int Id { get; set; }
    public int MessageId { get; set; }
    public string UrlImage { get; set; }
}
