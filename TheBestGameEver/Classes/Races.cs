using System;
using System.Collections.Generic;
using System.Text;

namespace TheBestGameEver.Classes
{
  class Races
  {
    public int ID { get; set; }
    public string RaceType { get; set; }
    public int StatModifier { get; set; }

    public List<RaceAbilities> RaceAbilities { get; set; }
  }
}
