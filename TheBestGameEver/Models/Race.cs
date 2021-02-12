using System;
using System.Collections.Generic;
using System.Text;

namespace TheBestGameEver.Models
{
  class Race
  {
    public int ID { get; set; }
    public int StatModifer { get; set; }
    public string RaceType { get; set; }
    public List<RaceAbility> Abilities { get; set; }
  }
}
