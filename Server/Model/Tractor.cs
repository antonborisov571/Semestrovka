using System;
using System.Collections.Generic;
using ORM.ORMPostgres;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model;

[Table("Tractors")]
public class Tractor : IEntity
{
    public int Id { get; set; }
    public string TractorName { get; set; }
    public DateTime YearRelease { get; set; }
}
