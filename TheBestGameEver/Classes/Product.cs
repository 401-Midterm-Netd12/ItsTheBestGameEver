using System;
using System.Collections.Generic;
using System.Text;

namespace TheBestGameEver.Classes
{
    public class Product
    {
        public int Count { get; set; }
        public string Next { get; set; }
        public string Previous { get; set; }
        public List<Results> Result { get; set; }
        
    }
}
