using System;
using System.Collections.Generic;
using ORM.ORMPostgres;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model;

[Table("Topics")]
public class Topic : IEntity
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public string TopicName { get; set; }
    public DateTime DateDispatch { get; set; }
    public string Description { get; set; }
}
