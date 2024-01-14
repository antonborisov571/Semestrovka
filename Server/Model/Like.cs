using System;
using System.Collections.Generic;
using ORM.ORMPostgres;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model;

[Table("Likes")]
public class Like : IEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int MessageId { get; set; }
    public DateTime DateDispatch { get; set; }
    public bool HasLike { get; set; }
}
