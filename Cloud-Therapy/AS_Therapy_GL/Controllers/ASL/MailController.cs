using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using AS_Therapy_GL.Models;
using AS_Therapy_GL.Models.ASL;
using AS_Therapy_GL.Models.DTO;

namespace AS_Therapy_GL.Controllers.ASL
{
    public class MailController : AppController
    {
        private Therapy_GL_DbContext db = new Therapy_GL_DbContext();

        //Datetime formet
        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
        public DateTime td;

        //Get Ip ADDRESS,Time & user PC Name
        public string strHostName;
        public IPHostEntry ipHostInfo;
        public IPAddress ipAddress;

        public MailController()
        {
            strHostName = System.Net.Dns.GetHostName();
            ipHostInfo = Dns.Resolve(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[0];
            td = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            ViewData["HighLight_Menu_PromotionForm"] = "High Light Menu";
        }




        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }







        public void Insert_ASL_PEMAIL_Table(ASL_PCONTACTS model, MailDTO currentMailBody)
        {
            ASL_PEMAIL emailLogData = new ASL_PEMAIL();
            emailLogData.COMPID = Convert.ToInt64(currentMailBody.COMPID);
            string currentDate = td.ToString("dd-MMM-yyyy");
            emailLogData.TRANSDT = Convert.ToDateTime(currentDate);
            emailLogData.TRANSYY = Convert.ToInt64(td.ToString("yyyy"));

            string year = td.ToString("yyyy");
            string last2Digit_inYEAR = year.Substring(2, 2);
            Int64 max = Convert.ToInt64(currentMailBody.COMPID + last2Digit_inYEAR + "9999");
            try
            {
                Int64 maxTransNO = Convert.ToInt64((from n in db.SendLogEmailDbSet where n.COMPID == currentMailBody.COMPID && n.TRANSYY == emailLogData.TRANSYY select n.TRANSNO).Max());
                if (maxTransNO == 0)
                {
                    emailLogData.TRANSNO = Convert.ToInt64(currentMailBody.COMPID + last2Digit_inYEAR + "0001");
                }
                else if (maxTransNO <= max)
                {
                    emailLogData.TRANSNO = maxTransNO + 1;
                }
            }
            catch
            {
                emailLogData.TRANSNO = Convert.ToInt64(currentMailBody.COMPID + last2Digit_inYEAR + "0001");
            }

            emailLogData.EMAILID = model.EMAIL;
            emailLogData.EMAILSUBJECT = currentMailBody.Subject;
            emailLogData.BODYMSG = currentMailBody.Body;
            emailLogData.STATUS = "PENDING";
            //emailLogData.SENTTM = null;

            emailLogData.USERPC = strHostName;
            emailLogData.INSIPNO = ipAddress.ToString();
            emailLogData.INSTIME = Convert.ToDateTime(td);
            emailLogData.INSUSERID = currentMailBody.INSUSERID;
            emailLogData.INSLTUDE = Convert.ToString(currentMailBody.INSLTUDE);
            db.SendLogEmailDbSet.Add(emailLogData);
            db.SaveChanges();
        }



        public void Update_ASL_PEMAIL_Table_SendingEmail(ASL_PEMAIL model, MailDTO currentMailBody)
        {
            var updateTable =
                (from m in db.SendLogEmailDbSet where m.ID == model.ID && m.COMPID == model.COMPID select m).ToList();
            if (updateTable.Count == 1)
            {
                foreach (ASL_PEMAIL emailLogData in updateTable)
                {
                    emailLogData.STATUS = "SENT";
                    emailLogData.SENTTM = Convert.ToDateTime(td);
                    emailLogData.USERPC = strHostName;
                    emailLogData.UPDIPNO = ipAddress.ToString();
                    emailLogData.UPDTIME = Convert.ToDateTime(td);
                    emailLogData.UPDUSERID = currentMailBody.INSUSERID;
                    emailLogData.UPDLTUDE = Convert.ToString(currentMailBody.INSLTUDE);
                }
                db.SaveChanges();
            }
        }






        // GET: /Mail/
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Index(MailDTO model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var getSMSUserNamePass = from m in db.AslCompanyDbSet
                                             where m.COMPID == model.COMPID
                                             select new { m.EMAILIDP, m.EMAILPWP };
                    string userName = "", usPss = "";
                    foreach (var VARIABLE in getSMSUserNamePass)
                    {
                        userName = VARIABLE.EMAILIDP;
                        usPss = VARIABLE.EMAILPWP;
                    }


                    if (userName == null || usPss == null)
                    {
                        ViewBag.MailMessage = "Your Email id not registered!!";
                        return View();
                    }

                    if (model.GROUPID == null && model.ToEmail == null)
                    {
                        ViewBag.MailMessage = "Select group name or To Email filed first!";
                        return View();
                    }



                    //Send user email to Alchemy Software email address.
                    MailMessage mail = new MailMessage();
                    //mail.From = new MailAddress("admin@alchemy-bd.com");
                    //mail.From = new MailAddress("admin@alchemy-utl.com");
                    mail.From = new MailAddress(userName);


                    //Current group contact list add in ASL_PEMAIL table. 
                    if (model.GROUPID != null)
                    {
                        var getUploadContactList = (from m in db.UploadContactDbSet where m.COMPID == model.COMPID && m.GROUPID == model.GROUPID select m).ToList();
                        foreach (var addList in getUploadContactList)
                        {
                            if (IsValidEmail(addList.EMAIL))
                            {
                                Insert_ASL_PEMAIL_Table(addList, model);
                            }
                        }
                    }


                    //model.ToEmail list add in ASL_PEMAIL Table.
                    if (model.ToEmail != null)
                    {
                        string[] multi = model.ToEmail.Split(',');
                        foreach (var multiEMail in multi)
                        {
                            if (multiEMail != "" && IsValidEmail(multiEMail))
                            {
                                ASL_PCONTACTS pcontacts = new ASL_PCONTACTS();
                                pcontacts.EMAIL = multiEMail;
                                Insert_ASL_PEMAIL_Table(pcontacts, model);

                            }
                        }
                    }


                    var findCompanyInfo = from m in db.AslCompanyDbSet
                                          where m.COMPID == model.COMPID
                                          select new { m.COMPNM, m.ADDRESS, m.ADDRESS2, m.CONTACTNO, m.EMAILID };
                    string companyName = "", address = "", address2 = "", contactNo = "", companyEmail = "";
                    foreach (var variable in findCompanyInfo)
                    {
                        companyName = variable.COMPNM.ToString();
                        address = variable.ADDRESS.ToString();
                        if (variable.ADDRESS2 != null)
                        {
                            address2 = variable.ADDRESS2.ToString();
                        }
                        if (variable.CONTACTNO != null)
                        {
                            contactNo = variable.CONTACTNO.ToString();
                        }
                        if (variable.EMAILID!=null)
                        {
                            companyEmail = variable.EMAILID.ToString();
                        }
                       
                    }


                    string currentDate = td.ToString("yyyy-MM-dd");
                    DateTime transdate = Convert.ToDateTime(currentDate.Substring(0, 10));
                    Int64 year = Convert.ToInt64(td.ToString("yyyy"));
                    var find_ASLPEMAIL = (from m in db.SendLogEmailDbSet where m.COMPID == model.COMPID && m.TRANSDT == transdate && m.TRANSYY == year && m.STATUS == "PENDING" select m).ToList();
                    Int64 count = 0;
                    foreach (var x in find_ASLPEMAIL)
                    {
                        count++;

                        mail.To.Add(x.EMAILID);
                        mail.Subject = model.Subject;
                        mail.Priority = MailPriority.Normal;
                        string backgroundColor = "";
                        if (model.Color == "Red")
                        {
                            backgroundColor = "AF5235";
                        }
                        else if (model.Color == "Green")
                        {
                            backgroundColor = "5E8A2B";
                        }
                        else if (model.Color == "Blue")
                        {
                            backgroundColor = "1C4E75";
                        }
                        else if (model.Color == "Black")
                        {
                            backgroundColor = "1f2836";
                        }

                        // Construct the alternate body as HTML.
                        string body = "<!DOCTYPE html>";
                        body += "<HTML><HEAD><META http-equiv=Content-Type content=\"text/html; charset=iso-8859-1\">";
                        body += "</HEAD><BODY><div marginwidth=0 marginheight=0>";

                        body += "<table style='width:100%'>";
                        body += "<tr>";
                        body += "<td style='background:#" + backgroundColor + ";height:40px;padding:10px 20px'>";
                        //body += "<h1 style='color:white; font-size:28px'><img src='https://ci5.googleusercontent.com/proxy/MqD4Lnz6VorenYZQghu3EO908D4CaPzL3pwXiRPFoDHdCS2DvfHLwD5ViKM-KLzG7qFqZC07Xg=s0-d-e1-ft#http://alchemy-bd.com/alchemy.png' alt='alchemy software' class='CToWUd' style='height: 29px;'>Alchemy Software</h1>";
                        body += "<h1 style='color:white; font-size:28px'><img alt='' class='CToWUd' style='height: 29px;'>" + companyName + "</h1>";

                        body += "<p style='line-height:18px;position:relative;top:-10px;color:white; font-size:12px'>";
                        body += "<a href='#' style='color:white;text-decoration:none'>Email : " + companyEmail + "</a>";
                        body += "<br>";
                        body += "<a href='#' style='color:white; text-decoration:none'>Hello : +" + contactNo + "</a>";
                        body += "<br>";
                        body += "<a href='#' style='color:white;text-decoration:none'>" + address + "</a>";
                        body += "<br>";
                        body += "<a href='#' style='color:white;text-decoration:none'>" + address2 + "</a>";
                        body += "</p>";
                        body += "</td>";
                        body += "</tr>";

                        body += "<tr>";
                        body += "<td style='padding:20px'>";
                        body += model.Body;
                        body += "</td>";
                        body += "<tr>";

                        body += "<tr>";
                        body += "<td style='background:#" + backgroundColor + ";height:30px;padding:4px 20px'>";
                        body += "<p style='color:white;'>erp | ready software | customized(cloud | mobile | web) application | website | sms integrated solutionsolution (cctv | access control | networking)<span style='padding-left: 5px;'><a style='color: #E7CBA3;' href='http://alchemy-bd.com/' target='_blank'>powered by alchemy sofyware</a></span></p>";
                        body += "</td>";
                        body += "<tr>";

                        body += "</table>";

                        body += "</div>";
                        body += "</BODY></HTML>";

                        //string body = "<!DOCTYPE html>";
                        //body += "<HTML><HEAD><META http-equiv=Content-Type content=\"text/html; charset=iso-8859-1\">";
                        //body += "</HEAD><BODY><div marginwidth=0 marginheight=0>";

                        //body += "<table width=100% align=center border=0 cellpadding=0 cellspacing=0 style=background-color:#" + backgroundColor + ";><tbody><tr><td width=100% align=center>";
                        //body += "<table width=650 border=0 cellpadding=0 cellspacing=0 align=center><tbody><tr><td width=100% height=20></td></tr><tr><td valign=middle align=center><a href=http://alchemy-bd.com/ target=_blank>";
                        //body += "<img class=CToWUd border=0 style=width: 150px;min-height:auto height=auto width=150 src='http://cloudcafebd.com/Images/MaliLogo1.png'></a>";
                        //body += "</td>";
                        //body += " </tr><tr><td width=100% height=40></td></tr><tr><td width=100% align=center><span style='font-size: 30px;line-height:48px;font-family:Helvetica,Arial,sans-serif;color:#ffffff;font-weight:900'>Artificier For Animating Your Aspiration</span></td></tr><tr><td width=100% height=20></td></tr><tr><td width=100% align=center>";
                        //body += "<table width=590 border=0 cellpadding=0 cellspacing=0 align=center style=background-color:#ffffff;border-radius:6px><tbody><tr><td width=100% height=60></td></tr><tr><td width=100% align=center>";
                        //body += "<table width=490 align=center border=0 cellspacing=0><tbody><tr><td width=100% align=center style=text-align:left>";
                        //body += "<span style=font-size:22.5px;line-height:33px;font-family:Helvetica,Arial,sans-serif;color:#1f2836;font-weight:bold>Hello,</span><br><br><span style=font-size:18px;line-height:27px;font-family:Helvetica,Arial,sans-serif;color:#1f2836>" + model.Body + "";
                        //body += "</span></td></tr></tbody></table></td>";
                        //body += "</tr><tr><td width=100% height=40></td></tr><tr><td width=100% align=center>";
                        //body += "<table width=490 border=0 cellpadding=0 cellspacing=0 align=center><tbody><tr><td align=center height=50 style=border-radius:4px;font-weight:800;font-family:Helvetica,Arial,sans-serif;color:#ffffff;background-color:#f77d0e>";
                        //body += "<span style=font-family:Helvetica,Arial,sans-serif;font-weight:800>";
                        //body += "<a href=http://alchemy-bd.com/portfolio/ style=color:#ffffff;font-size:18px;text-decoration:none;line-height:50px;display:block;width:100% target=_blank>Our portfolio</a>";
                        //body += "</span>";
                        //body += "</td>";
                        //body += "</tr></tbody></table><br><span style=font-size:12px;line-height:15px;color:#1f2836;font-family:Helvetica,Arial,sans-serif>Powered By : ALCHEMY SOFTWARE<br></span>";
                        //body += "</td>";
                        //body += "</tr><tr><td width=100% height=60></td></tr></tbody></table></td>";
                        //body += "</tr><tr><td width=100% height=80></td></tr></tbody></table></td>";
                        //body += "</tr></tbody></table><table width=100% align=center border=0 cellspacing=0 cellpadding=0 bgcolor=#ffffff style=background-color:#ffffff><tbody><tr><td width=100% align=center>";
                        //body += "<table width=640 align=center border=0 cellspacing=0 cellpadding=0 bgcolor=#ffffff style=font-family:Helvetica,Arial,sans-serif><tbody><tr><td colspan=2 height=40></td></tr><tr><td>";
                        //body += "<div style=color:#333333;font-size:12px;line-height:12px;padding:0;margin:0><a href=http://alchemy-bd.com/contact/ style=color:#333333;text-decoration:underline target=_blank>Contact</a> | <a href=http://alchemy-bd.com/faq/ style=color:#333333;text-decoration:underline target=_blank>Faq</a>";
                        //body += "         </div>";
                        //body += "<div style=line-height:10px;padding:0;margin:0>&nbsp;</div>";
                        //body += "<div style=color:#333333;font-size:12px;line-height:12px;padding:0;margin:0><a href=http://alchemy-bd.com/about/ style=color:#333333;text-decoration:underline target=_blank>About US</a> | <a href=mailto:info@alchemy-bd.com style=color:#333333;text-decoration:underline target=_blank>info@alchemy-bd.com</a>";
                        //body += "</div>";
                        //body += "</td>";
                        //body += "<td align=right>";
                        //body += "<span style=line-height:20px;font-size:10px><a href=https://www.facebook.com/AlchemySoftware target=_blank><img src=https://ci3.googleusercontent.com/proxy/DPEVDIl7zTJOuNLg0B_ewL3E0lz2b_reW6NcJHxwtYzW87NjV1HxSckdR97aBc03mSpFWIL47omxZxxCaae02tnxlF4j5ju-UfDyjXZ2r91__Pb6mUpd-iJ981I6Viepc-mU0ovHzbtrYUnDWmsv0Dn9wup-l7NCgA=s0-d-e1-ft#https://dtqn2osro0nhk.cloudfront.net/static/images/email/build/19589dd1df3811138bd6da9412853037.png alt=fb border=0 class=CToWUd></a>&nbsp;</span>";
                        //body += "<span style=line-height:20px;font-size:10px><a href=https://twitter.com/alchemybd target=_blank><img src=https://ci6.googleusercontent.com/proxy/BqKFs828FcMnGtkLEJO7at5YfplBcG4qBU1yAOfXZgIMboO0cvRRokooAhFjZ-Y4B_2vDK6x3OJ1TXBG1F_GKXEsih-2ox8Pg9W7evi9eZUAkZjxZ9RB13bXn46-J3LPZgLyIA9E2DJDGG7RfPSXOINWK8eQXuRncw=s0-d-e1-ft#https://dtqn2osro0nhk.cloudfront.net/static/images/email/build/90ba0b614307e571ccce5583caa5ff76.png alt=twit border=0 class=CToWUd></a>&nbsp;</span>";
                        //body += "<span style=line-height:20px;font-size:10px><a href=https://plus.google.com/100639053145643474788 target=_blank><img src=https://ci3.googleusercontent.com/proxy/dKnP_-OVPzDxCK2PQ-gU8C0ptKLgkv3H8QWM32lLapNy8g1lBQrdCAbMwzd1cBSQY87w-ncnRdwyn_--164LPsp1Gly_RXuunDRI9nuPdHrvBz0oW7V5X12WlqSf_strpv8YtvBH9oRTqW3DjzHuDHG7i2XRfwV7VQ=s0-d-e1-ft#https://dtqn2osro0nhk.cloudfront.net/static/images/email/build/11021554a4da865ca5900ac2dbc8e204.png alt=g border=0 class=CToWUd></a>&nbsp;</span>";
                        //body += "<div style=line-height:5px;padding:0;margin:0>&nbsp;</div>";
                        //body += "</td>";
                        //body += "</tr></tbody></table></td >";
                        //body += "</tr></tbody></table></div>";

                        //body += "</div></div>";
                        //body += "</BODY></HTML>";

                        ContentType mimeType = new System.Net.Mime.ContentType("text/html");

                        // Add the alternate body to the message.
                        AlternateView alternate = AlternateView.CreateAlternateViewFromString(body, mimeType);
                        mail.AlternateViews.Add(alternate);

                        SmtpClient client = new SmtpClient();
                        //client.Host = "mail.alchemy-bd.com";
                        //client.Credentials = new NetworkCredential("admin@alchemy-bd.com", "Asl.admin@&123%");
                        //client.Host = "mail.alchemy-utl.com";
                        //client.Credentials = new NetworkCredential("admin@alchemy-utl.com", "dTc4?}@2H(!x");
                        client.Host = "mail.alchemy-utl.com";
                        client.Credentials = new NetworkCredential(userName, usPss);

                        client.EnableSsl = false;
                        client.Send(mail);

                        Update_ASL_PEMAIL_Table_SendingEmail(x, model);

                    }

                    TempData["MailMessage"] = "Send Succesfully " + count + " emails.";
                    return RedirectToAction("Index");

                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                    ViewBag.MailMessage = "Sending Failed!!";
                    return View();
                }
            }
            return View("Index");
        }








        // GET: /Mail/
        public ActionResult getPendingMail()
        {
            return View();
        }

        [HttpPost]
        public ActionResult getPendingMail(PendingMailSmsDTO model, string command)
        {
            if (ModelState.IsValid)
            {
                DateTime transDate = Convert.ToDateTime(model.TRANSDT);
                if (command == "Search")
                {
                    List<ASL_PEMAIL> pendingList = new List<ASL_PEMAIL>();
                    var findPendingList =
                        (from m in db.SendLogEmailDbSet
                         where m.COMPID == model.COMPID && m.TRANSDT == transDate && m.STATUS == "PENDING"
                         select m).ToList();
                    foreach (var x in findPendingList)
                    {
                        pendingList.Add(new ASL_PEMAIL { EMAILID = x.EMAILID.ToString(), BODYMSG = x.BODYMSG.ToString(), STATUS = x.STATUS.ToString() });
                        ViewData["PendingList_EMAIL"] = pendingList;
                    }
                    return View();
                }
                else if (command == "Send")
                {
                    try
                    {
                        var getSMSUserNamePass = from m in db.AslCompanyDbSet
                                                 where m.COMPID == model.COMPID
                                                 select new { m.EMAILIDP, m.EMAILPWP };
                        string userName = "", usPss = "";
                        foreach (var VARIABLE in getSMSUserNamePass)
                        {
                            userName = VARIABLE.EMAILIDP;
                            usPss = VARIABLE.EMAILPWP;
                        }


                        if (userName == null || usPss == null)
                        {
                            ViewBag.PendingMailMessage = "Your Email id not registered!!";
                            return View();
                        }




                        var findCompanyInfo = from m in db.AslCompanyDbSet
                            where m.COMPID == model.COMPID
                            select new {m.COMPNM, m.ADDRESS, m.ADDRESS2, m.CONTACTNO,m.EMAILID};
                        string companyName = "", address = "", address2 = "", contactNo = "",companyEmail="";
                        foreach (var variable in findCompanyInfo)
                        {
                            companyName = variable.COMPNM.ToString();
                            address = variable.ADDRESS.ToString();
                            if (variable.ADDRESS2 != null)
                            {
                                address2 = variable.ADDRESS2.ToString();
                            }
                            if (variable.CONTACTNO != null)
                            {
                                contactNo = variable.CONTACTNO.ToString();
                            }
                            if (variable.EMAILID != null)
                            {
                                companyEmail = variable.EMAILID.ToString();
                            }
                        }



                        //Send user email to Alchemy Software email address.
                        MailMessage mail = new MailMessage();
                        //mail.From = new MailAddress("admin@alchemy-bd.com");
                        //mail.From = new MailAddress("admin@alchemy-utl.com");
                        mail.From = new MailAddress(userName);

                        var findPendingList = (from m in db.SendLogEmailDbSet where m.COMPID == model.COMPID && m.TRANSDT == transDate && m.STATUS == "PENDING" select m).ToList();
                        Int64 count = 0;
                        foreach (var x in findPendingList)
                        {
                            count++;

                            mail.To.Add(x.EMAILID);
                            mail.Subject = x.EMAILSUBJECT;
                            mail.Priority = MailPriority.Normal;
                            string backgroundColor = "";
                            if (model.Color == "Red")
                            {
                                backgroundColor = "AF5235";
                            }
                            else if (model.Color == "Green")
                            {
                                backgroundColor = "5E8A2B";
                            }
                            else if (model.Color == "Blue")
                            {
                                backgroundColor = "1C4E75";
                            }
                            else if (model.Color == "Black")
                            {
                                backgroundColor = "1f2836";
                            }

                            // Construct the alternate body as HTML.
                            string body = "<!DOCTYPE html>";
                            body += "<HTML><HEAD><META http-equiv=Content-Type content=\"text/html; charset=iso-8859-1\">";
                            body += "</HEAD><BODY><div marginwidth=0 marginheight=0>";

                            body += "<table style='width:100%'>";
                            body += "<tr>";
                            body += "<td style='background:#" + backgroundColor + ";height:40px;padding:10px 20px'>";
                            //body += "<h1 style='color:white; font-size:28px'><img src='https://ci5.googleusercontent.com/proxy/MqD4Lnz6VorenYZQghu3EO908D4CaPzL3pwXiRPFoDHdCS2DvfHLwD5ViKM-KLzG7qFqZC07Xg=s0-d-e1-ft#http://alchemy-bd.com/alchemy.png' alt='alchemy software' class='CToWUd' style='height: 29px;'>Alchemy Software</h1>";
                            body += "<h1 style='color:white; font-size:28px'><img alt='' class='CToWUd' style='height: 29px;'>" + companyName + "</h1>";
                           
                            body += "<p style='line-height:18px;position:relative;top:-10px;color:white; font-size:12px'>";
                            body += "<a href='#' style='color:white;text-decoration:none'>Email : "+companyEmail+"</a>";
                            body += "<br>";
                            body += "<a href='#' style='color:white; text-decoration:none'>Hello : +"+contactNo+"</a>";
                            body += "<br>";
                            body += "<a href='#' style='color:white;text-decoration:none'>" + address + "</a>";
                            body += "<br>";
                            body += "<a href='#' style='color:white;text-decoration:none'>" + address2 + "</a>";
                            body += "</p>";
                            body += "</td>";
                            body += "</tr>";

                            body += "<tr>";
                            body += "<td style='padding:20px'>";
                            body += x.BODYMSG;
                            body += "</td>";
                            body += "<tr>";

                            body += "<tr>";
                            body += "<td style='background:#" + backgroundColor + ";height:30px;padding:4px 20px'>";
                            body += "<p style='color:white;'>erp | ready software | customized(cloud | mobile | web) application | website | sms integrated solutionsolution (cctv | access control | networking)<span style='padding-left: 5px;'><a style='color: #E7CBA3;' href='http://alchemy-bd.com/' target='_blank'>powered by alchemy sofyware</a></span></p>";
                            body += "</td>";
                            body += "<tr>";

                            body += "</table>";

                            body += "</div>";
                            body += "</BODY></HTML>";


                            ContentType mimeType = new System.Net.Mime.ContentType("text/html");

                            // Add the alternate body to the message.
                            AlternateView alternate = AlternateView.CreateAlternateViewFromString(body, mimeType);
                            mail.AlternateViews.Add(alternate);

                            SmtpClient client = new SmtpClient();
                            //client.Host = "mail.alchemy-bd.com";
                            //client.Credentials = new NetworkCredential("admin@alchemy-bd.com", "Asl.admin@&123%");
                            //client.Host = "mail.alchemy-utl.com";
                            //client.Credentials = new NetworkCredential("admin@alchemy-utl.com", "dTc4?}@2H(!x");
                            client.Host = "mail.alchemy-utl.com";
                            client.Credentials = new NetworkCredential(userName, usPss);

                            client.EnableSsl = false;
                            client.Send(mail);

                            MailDTO pendingEmailBody = new MailDTO();
                            pendingEmailBody.INSUSERID = model.UPDUSERID;
                            pendingEmailBody.INSLTUDE = model.UPDLTUDE;
                            Update_ASL_PEMAIL_Table_SendingEmail(x, pendingEmailBody);
                        }
                        ViewBag.PendingMailMessage = "Send Succesfully " + count + " pending emails.";
                        return View();
                    }
                    catch (Exception ex)
                    {
                        Response.Write(ex.Message);
                        ViewBag.PendingMailMessage = "Sending Failed!!";
                        return View();
                    }
                }
            }
            return View("getPendingMail");
        }






        //tag-it autocomplete
        public JsonResult TagSearch_tagit(string term, string compid)
        {
            var companyid = Convert.ToInt16(compid);
            var tags = from p in db.UploadContactDbSet 
                       where p.COMPID == companyid && p.EMAIL!=null 
                       select p.EMAIL;

            return this.Json(tags.Where(t => t.StartsWith(term)),
                       JsonRequestBehavior.AllowGet);

        }







        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }


    }
}
