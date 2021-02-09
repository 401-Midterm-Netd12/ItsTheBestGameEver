using System;
using System.Collections.Generic;
using System.Text;

namespace TheBestGameEver.Classes
{
  class CharacterClasses
  {
    public int ID { get; set; }
    public int statModifier { get; set; }
    public ClassNames classNames { get; set; }
    public List<ClassSkills> classSkills { get; set; }
  }
}
