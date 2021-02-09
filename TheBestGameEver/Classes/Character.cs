using System;
using System.Collections.Generic;
using System.Text;

namespace TheBestGameEver.Classes
{
  class Character
  {
    public int ID { get; set; }
    public int UserID { get; set; }
    public int RaceID { get; set; }
    public int ClassID { get; set; }
    public string Name { get; set; }
    public int HP { get; set; }
    public int Dex { get; set; }
    public int Strength { get; set; }
  }
}
