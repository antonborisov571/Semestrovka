using ORM.ORMPostgres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model;

[Table("Posts")]
public class Post : IEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int TopicId { get; set; }
    public string PostName { get; set; }
    public DateTime DateDispatch { get; set; }
    public string Description { get; set; }
}
