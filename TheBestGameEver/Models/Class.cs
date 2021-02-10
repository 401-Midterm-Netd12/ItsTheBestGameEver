using System;
using System.Collections.Generic;
using System.Text;

namespace TheBestGameEver.Models
{
  class Class
  {
    public int ID { get; set; }
    public int StatModifier { get; set; }
    public List<ClassSkill> ClassSkills { get; set; }
    public string ClassName { get; set; }
  }
}
