using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    public class InfoAboutError
    {
        public bool error;
        public string str;
        public int positionError;

        public InfoAboutError(bool er, string s, int positionError=-1)
        {
            error = er;
            str = s;
            this.positionError = positionError;
        }

        public InfoAboutError()
        {
            error = false;
            str = "";
            this.positionError = -1;
        }


    }
}
