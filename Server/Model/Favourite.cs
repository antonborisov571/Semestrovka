using ORM.ORMPostgres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model;

[Table("Favourites")]
public class Favourite : IEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int MessageId { get; set; }
    public DateTime DateDispatch { get; set; }
    public bool HasFavourite { get; set; }
}
