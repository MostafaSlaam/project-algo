using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace autocomplete
{
    public class line
    {
        public int w { set; get; }
        public string s { set; get; }
        public line(int w,string s) 
        {
            this.w = w;
            this.s = s;
        }
    }
}
