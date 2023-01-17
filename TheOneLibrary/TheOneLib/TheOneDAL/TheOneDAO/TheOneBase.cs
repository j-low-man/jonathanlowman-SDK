using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheOneLib.TheOneDAL.TheOneDAO
{
    public class TheOneBase : TheOneInterface
    {
        public String _id { get; set; }

        public string getId()
        {
            return _id;
        }
    }
}
