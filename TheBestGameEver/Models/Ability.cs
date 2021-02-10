using System;
using System.Collections.Generic;
using System.Text;

namespace TheBestGameEver.Models
{
  class Ability
  {
    public int ID { get; set; }
    public int RaceID { get; set; }
    public string Name { get; set; }
    public string Desc { get; set; }
    public List<RaceAbility> AbilityList { get; set; }
  }
}
