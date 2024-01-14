using ORM.ORMPostgres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Model;

[Table("Jokes")]
public class Joke : IEntity
{
    public int Id { get; set; }
    public string TextJoke { get; set; }
    public DateTime DateShow { get; set; }
}