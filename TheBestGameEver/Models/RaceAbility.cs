using System;
using System.Collections.Generic;
using System.Text;

namespace TheBestGameEver.Models
{
  class RaceAbility
  {
    public int RaceID { get; set; }
    public int AbilityID { get; set; }
    public Race RaceInRace { get; set; }
    public Ability AbilityInRace { get; set; }
  }
}
