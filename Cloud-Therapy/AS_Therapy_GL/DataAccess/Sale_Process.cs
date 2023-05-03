using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using AS_Therapy_GL.Models;

namespace AS_Store_GL.DataAccess
{
    public static class Sale_Process
    {
        public static string process(PageModel model, Int64 compid)
        {

            //Datetime formet
            IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime td;


            Therapy_GL_DbContext db = new Therapy_GL_DbContext();
            //Get Ip ADDRESS,Time & user PC Name
            string strHostName;
            IPHostEntry ipHostInfo;
            IPAddress ipAddress;

            strHostName = System.Net.Dns.GetHostName();
            ipHostInfo = Dns.Resolve(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[0];
            td = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);

            string date = Convert.ToString(model.AGlMaster.TRANSDT);
            DateTime MyDateTime = DateTime.Parse(date);
            string converttoString = MyDateTime.ToString("dd-MMM-yyyy"); ;
            string getYear = converttoString.Substring(9, 2);
            string getMonth = converttoString.Substring(3, 3);
            string Month_Year = getMonth + "-" + getYear;



            var forRemovedata = (from l in db.GlMasterDbSet
                                 where l.COMPID == compid && l.TRANSDT == model.AGlMaster.TRANSDT
                                 select l).ToList();

            foreach (var VARIABLE in forRemovedata)
            {
                if (VARIABLE.TABLEID == "PST_TRANS" && VARIABLE.TRANSDT == model.AGlMaster.TRANSDT)
                {
                    db.GlMasterDbSet.Remove(VARIABLE);
                }
            }
            db.SaveChanges();



            //GL Process for PST_TRANS
            var checkDate_PST_TRANS = (from n in db.PST_TransMstDbSet where n.TRANSDT == model.AGlMaster.TRANSDT && n.COMPID == compid && n.TRANSTP=="SALE" select n).OrderBy(x => x.TRANSNO).ToList();

            if (checkDate_PST_TRANS.Count != 0)
            {
                Int64 max_Sale = 50000;
                foreach (var check in checkDate_PST_TRANS)
                {
                    string patientID = Convert.ToString(check.PATIENTID);
                    string substring_patientID = Convert.ToString(patientID.Substring(4, 6));
                    //Sale
                    if (check.TRANSTP == "SALE")
                    {
                        max_Sale = max_Sale + 1;
                        model.AGlMaster.TRANSSL = max_Sale;
                        model.AGlMaster.TRANSDT = check.TRANSDT;
                        model.AGlMaster.COMPID = check.COMPID;
                        model.AGlMaster.TRANSTP = "JOUR";
                        model.AGlMaster.TRANSMY = Month_Year;
                        model.AGlMaster.TRANSNO = check.TRANSNO;
                        //model.AGlMaster.TRANSFOR = check.TRANSFOR;
                        //model.AGlMaster.TRANSMODE = check.TRANSMODE;
                        //model.AGlMaster.COSTPID = check.COSTPID;
                        model.AGlMaster.DEBITCD = check.PATIENTID;
                        model.AGlMaster.CREDITCD = Convert.ToInt64(compid + "3010001");
                        //model.AGlMaster.CHEQUENO = check.CHEQUENO;
                        //model.AGlMaster.CHEQUEDT = check.CHEQUEDT;
                        model.AGlMaster.REMARKS = Convert.ToString("Patient-"+ patientID.Substring(4,6) + check.REMARKS);

                        model.AGlMaster.DEBITAMT = check.TOTNET;
                        model.AGlMaster.CREDITAMT = 0;

                        model.AGlMaster.TRANSDRCR = "DEBIT";
                        model.AGlMaster.TABLEID = "PST_TRANS";

                        model.AGlMaster.USERPC = strHostName;
                        model.AGlMaster.INSIPNO = ipAddress.ToString();
                        model.AGlMaster.INSTIME = Convert.ToDateTime(td);
                        model.AGlMaster.INSUSERID =
                            Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                        model.AGlMaster.INSLTUDE = model.AGlMaster.INSLTUDE;

                        db.GlMasterDbSet.Add(model.AGlMaster);
                        db.SaveChanges();



                        max_Sale = max_Sale + 1;
                        model.AGlMaster.TRANSSL = max_Sale;
                        model.AGlMaster.TRANSDT = check.TRANSDT;
                        model.AGlMaster.COMPID = check.COMPID;
                        model.AGlMaster.TRANSTP = "JOUR";
                        model.AGlMaster.TRANSMY = Month_Year;
                        model.AGlMaster.TRANSNO = check.TRANSNO;
                        //model.AGlMaster.TRANSFOR = check.TRANSFOR;
                        //model.AGlMaster.TRANSMODE = check.TRANSMODE;
                        //model.AGlMaster.COSTPID = check.COSTPID;
                        model.AGlMaster.DEBITCD = Convert.ToInt64(compid + "3010001");
                        model.AGlMaster.CREDITCD = check.PATIENTID;
                        //model.AGlMaster.CHEQUENO = check.CHEQUENO;
                        //model.AGlMaster.CHEQUEDT = check.CHEQUEDT;
                        model.AGlMaster.REMARKS = Convert.ToString("Patient-" + patientID.Substring(4, 6) + check.REMARKS);

                        model.AGlMaster.DEBITAMT = 0;
                        model.AGlMaster.CREDITAMT = check.TOTNET;

                        model.AGlMaster.TRANSDRCR = "CREDIT";
                        model.AGlMaster.TABLEID = "PST_TRANS";

                        model.AGlMaster.USERPC = strHostName;
                        model.AGlMaster.INSIPNO = ipAddress.ToString();
                        model.AGlMaster.INSTIME = Convert.ToDateTime(td);
                        model.AGlMaster.INSUSERID =
                            Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                        model.AGlMaster.INSLTUDE = model.AGlMaster.INSLTUDE;

                        db.GlMasterDbSet.Add(model.AGlMaster);
                        db.SaveChanges();

                        if (check.AMTCASH != 0)
                        {
                            max_Sale = max_Sale + 1;
                            model.AGlMaster.TRANSSL = max_Sale;
                            model.AGlMaster.TRANSDT = check.TRANSDT;
                            model.AGlMaster.COMPID = check.COMPID;
                            model.AGlMaster.TRANSTP = "MREC";
                            model.AGlMaster.TRANSMY = Month_Year;
                            model.AGlMaster.TRANSNO = check.TRANSNO;
                            //model.AGlMaster.TRANSFOR = check.tra;
                            //model.AGlMaster.TRANSMODE = check.TRANSMODE;
                            //model.AGlMaster.COSTPID = check.COSTPID;
                            model.AGlMaster.DEBITCD = Convert.ToInt64(compid + "1010001");
                            model.AGlMaster.CREDITCD = check.PATIENTID;
                            //model.AGlMaster.CHEQUENO = check.CHEQUENO;
                            //model.AGlMaster.CHEQUEDT = check.CHEQUEDT;
                            model.AGlMaster.REMARKS = Convert.ToString("Patient-" + patientID.Substring(4, 6) + check.REMARKS);
                            model.AGlMaster.TRANSDRCR = "DEBIT";
                            model.AGlMaster.TABLEID = "PST_TRANS";

                            model.AGlMaster.DEBITAMT = check.AMTCASH;
                            model.AGlMaster.CREDITAMT = 0;

                            model.AGlMaster.USERPC = strHostName;
                            model.AGlMaster.INSIPNO = ipAddress.ToString();
                            model.AGlMaster.INSTIME = Convert.ToDateTime(td);
                            model.AGlMaster.INSUSERID =
                                Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                            model.AGlMaster.INSLTUDE = model.AGlMaster.INSLTUDE;

                            db.GlMasterDbSet.Add(model.AGlMaster);
                            db.SaveChanges();



                            max_Sale = max_Sale + 1;
                            model.AGlMaster.TRANSSL = max_Sale;
                            model.AGlMaster.TRANSDT = check.TRANSDT;
                            model.AGlMaster.COMPID = check.COMPID;
                            model.AGlMaster.TRANSTP = "MREC";
                            model.AGlMaster.TRANSMY = Month_Year;
                            model.AGlMaster.TRANSNO = check.TRANSNO;
                            //model.AGlMaster.TRANSFOR = check.TRANSFOR;
                            //model.AGlMaster.TRANSMODE = check.TRANSMODE;
                            //model.AGlMaster.COSTPID = check.COSTPID;
                            model.AGlMaster.DEBITCD = check.PATIENTID;
                            model.AGlMaster.CREDITCD = Convert.ToInt64(compid + "1010001");
                            //model.AGlMaster.CHEQUENO = check.CHEQUENO;
                            //model.AGlMaster.CHEQUEDT = check.CHEQUEDT;
                            model.AGlMaster.REMARKS = Convert.ToString("Patient-" + patientID.Substring(4, 6) + check.REMARKS);

                            model.AGlMaster.TRANSDRCR = "CREDIT";
                            model.AGlMaster.TABLEID = "PST_TRANS";

                            model.AGlMaster.DEBITAMT = 0;
                            model.AGlMaster.CREDITAMT = check.AMTCASH;

                            model.AGlMaster.USERPC = strHostName;
                            model.AGlMaster.INSIPNO = ipAddress.ToString();
                            model.AGlMaster.INSTIME = Convert.ToDateTime(td);
                            model.AGlMaster.INSUSERID =
                                Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                            model.AGlMaster.INSLTUDE = model.AGlMaster.INSLTUDE;

                            db.GlMasterDbSet.Add(model.AGlMaster);
                            db.SaveChanges();
                            //}
                        }



                    }
                }
                return "True";
            }
            else
            {
                return "False";
            }
        }


    }
}