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
        public char errorChar;


        public int indexLineError;
        public string trueQuery;

        // public string message;

        public InfoAboutError(bool er, string s, int positionError=-1, char errorChar='-')
        {
            error = er;
            str = s;
            this.positionError = positionError;
            this.errorChar = errorChar;

            this.indexLineError = -1;
            this.trueQuery = "";
        }

        public InfoAboutError()
        {
            this.error = false;
            this.str = "";
            this.positionError = -1;
            this.errorChar = '-';
            this.indexLineError = -1;
            this.trueQuery = "";
        }


    }
}
