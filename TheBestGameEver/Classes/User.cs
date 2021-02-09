using System;
using System.Collections.Generic;
using System.Text;

namespace TheBestGameEver.Classes
{
  class User
  {
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public List<Roles> UserRole { get; set; }
  }
}
