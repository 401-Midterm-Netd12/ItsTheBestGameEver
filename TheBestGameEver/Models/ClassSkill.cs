using System;
using System.Collections.Generic;
using System.Text;

namespace TheBestGameEver.Models
{
  class ClassSkill
  {
    public int ClassID { get; set; }
    public int SkillID { get; set; }
    public Class ClassNav { get; set; }
    public Skill SkillNav { get; set; }
  }
}
