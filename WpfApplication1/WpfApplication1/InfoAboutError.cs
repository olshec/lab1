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
        public string str;//for addition to result
        public char errorChar;

        public int indexLineError;
        public string trueQuery; //for get result from substing

        public int positionError;
        public int positionLineError;
        public string message;
        public string typeMessage;


        public InfoAboutError(bool er, string s, int positionError = 0, char errorChar = ';')
        {
            error = er;
            str = s;
            this.positionError = positionError;
            positionLineError = 0;
            this.errorChar = errorChar;

            this.indexLineError = 0;
            this.trueQuery = "";
            this.message = "";
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
            this.message = "";
        }

        public InfoAboutError Clone()
        {
            InfoAboutError iar = new InfoAboutError();
            iar.error = this.error;
            iar.str = this.str;
            iar.positionError = this.positionError;
            iar.positionLineError = this.positionLineError;
            iar.errorChar = this.errorChar;
            iar.indexLineError = this.indexLineError;
            iar.trueQuery = this.trueQuery;
            iar.message = this.message;
            return iar;
        }



    }
}
