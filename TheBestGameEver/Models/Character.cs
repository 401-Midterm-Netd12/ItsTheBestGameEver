using System;
using System.Collections.Generic;
using System.Text;
using TheBestGameEver.Models;

namespace TheBestGameEver.Classes
{
  class Character
  {
    public int ID { get; set; }
    public string UserID { get; set; }
    public int RaceID { get; set; }
    public int ClassID { get; set; }
    public string Name { get; set; }
    public int HP { get; set; }
    public int Dex { get; set; }
    public int Strength { get; set; }
    public AppUser CharAppUser { get; set; }
    public Race CharRace { get; set; }
    public Class CharClass { get; set; }
  }
}
