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
        public char errorChar;

        public int indexLineError;
        public string trueQuery;

        public int positionError;
        public int positionLineError;
        // public string message;

        public InfoAboutError(bool er, string s, int positionError=0, char errorChar=';')
        {
            error = er;
            str = s;
            this.positionError = positionError;
            positionLineError = 0;
            this.errorChar = errorChar;

            this.indexLineError = 0;
            this.trueQuery = "";

            //indexLineError = 0;
        }

        public InfoAboutError()
        {
            this.error = false;
            this.str = "";
            this.positionError = 0;
            positionLineError = 0;
            this.errorChar = ';';
            this.indexLineError = 0;
            this.trueQuery = "";
        }


    }
}
