using System;
using System.Collections.Generic;
using ORM.ORMPostgres;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model;

[Table("Messages")]
public class Message : IEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int PostId { get; set; }
    public DateTime DateDispatch { get; set; }
    public string TextMessage { get; set; }
}
