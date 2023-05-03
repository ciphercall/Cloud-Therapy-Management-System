using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AS_Therapy_GL.DataAccess
{
    public static class CommainAmount
    {
        public static string AmountwithComma(string CVal)
        {

            string value = "", gotpoint = "", commainvalue = "", firstpart = "", secondpart = "", thirdpart = "", fourthpart = "", finalvalue = "";
            int lengthdebit = CVal.Length;
           

           
           
            for (int i = 0; i < lengthdebit; i++)
            {
                if (CVal[i] == '.')
                {
                    value = CVal.Substring(0, i);
                    commainvalue = CVal.Substring(i, (lengthdebit - i));
                    gotpoint = "y";
                    break;
                }
            }
            if (gotpoint == "y")
            {
                int valuelength = value.Length;
                if (valuelength > 3 && valuelength > 5 && valuelength > 7)
                {
                    value = new string(value.ToCharArray().Reverse().ToArray());
                    firstpart = value.Substring(0, 3);
                    secondpart = value.Substring(3, 2);
                    thirdpart = value.Substring(5, 2);
                    fourthpart = value.Substring(7, valuelength - 7);
                    finalvalue = firstpart + "," + secondpart + "," + thirdpart + "," + fourthpart;
                    finalvalue = new string(finalvalue.ToCharArray().Reverse().ToArray());
                    finalvalue = finalvalue + commainvalue;
                }
                else if (valuelength > 3 && valuelength > 5)
                {
                    value = new string(value.ToCharArray().Reverse().ToArray());
                    firstpart = value.Substring(0, 3);
                    secondpart = value.Substring(3, 2);

                    thirdpart = value.Substring(5, valuelength - 5);
                    finalvalue = firstpart + "," + secondpart + "," + thirdpart;
                    finalvalue = new string(finalvalue.ToCharArray().Reverse().ToArray());
                    finalvalue = finalvalue + commainvalue;
                }
                else if (valuelength > 3)
                {
                    value = new string(value.ToCharArray().Reverse().ToArray());
                    firstpart = value.Substring(0, 3);


                    secondpart = value.Substring(3, valuelength - 3);
                    finalvalue = firstpart + "," + secondpart;
                    finalvalue = new string(finalvalue.ToCharArray().Reverse().ToArray());
                    finalvalue = finalvalue + commainvalue;
                }
                else
                {
                    finalvalue = value;
                    finalvalue = finalvalue + commainvalue;
                }
            }
            else
            {

            }


            return finalvalue;

        }
    }
}