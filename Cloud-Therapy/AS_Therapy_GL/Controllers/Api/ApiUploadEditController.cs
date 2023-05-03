using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AS_Therapy_GL.Controllers.ASL;
using AS_Therapy_GL.Models;
using AS_Therapy_GL.Models.ASL;
using AS_Therapy_GL.Models.DTO;

namespace AS_Therapy_GL.Controllers.Api
{
    public class ApiUploadEditController : ApiController
    {
        Therapy_GL_DbContext db = new Therapy_GL_DbContext();

        //Datetime formet
        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
        public DateTime td;

        //Get Ip ADDRESS,Time & user PC Name
        public string strHostName;
        public IPHostEntry ipHostInfo;
        public IPAddress ipAddress;

        public ApiUploadEditController()
        {
            strHostName = System.Net.Dns.GetHostName();
            ipHostInfo = Dns.Resolve(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[0];
            td = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
        }



        //[System.Web.Http.Route("api/ApiUploadExchangeContactController/GetContactData")]
        [System.Web.Http.HttpGet]
        public IEnumerable<UploadContactDTO> GetContactData(string compId, string groupId)
        {
            Int64 compid = Convert.ToInt64(compId);
            Int64 GroupId = Convert.ToInt64(groupId);
            var find_GridData = (from contact in db.UploadContactDbSet
                                 join groupNm in db.UploadGroupDbSet on contact.COMPID equals groupNm.COMPID
                                 where contact.COMPID == compid && contact.GROUPID == GroupId &&
                                 contact.GROUPID == groupNm.GROUPID //&& mediCare.GHEADID==ghead.GHEADID
                                 select new
                                 {
                                     contact.ID,
                                     contact.COMPID,
                                     contact.GROUPID,
                                     groupNm.GROUPNM,
                                     contact.NAME,
                                     contact.EMAIL,
                                     contact.MOBNO1,
                                     contact.MOBNO2,
                                     contact.ADDRESS,

                                     contact.INSIPNO,
                                     contact.INSLTUDE,
                                     contact.INSTIME,
                                     contact.INSUSERID,
                                 }).OrderBy(e => e.ID).ToList();

            if (find_GridData.Count == 0)
            {
                yield return new UploadContactDTO
                {
                    count = 1, //no data found
                };
            }
            else
            {
                foreach (var s in find_GridData)
                {
                    //String doseName = "";
                    //var findDoseName =
                    //    (from m in db.RxGheadDbSet
                    //     where m.COMPID == compid && m.GHEADID == s.GHEADID
                    //     select new { m.GHEADEN }).ToList();
                    //if (findDoseName.Count != 0)
                    //{
                    //    foreach (var x in findDoseName)
                    //    {
                    //        doseName = x.GHEADEN;
                    //    }
                    //}


                    yield return new UploadContactDTO
                    {
                        ID = s.ID,
                        COMPID = Convert.ToInt64(s.COMPID),
                        From_GROUPID = Convert.ToInt64(s.GROUPID),
                        TO_GROUPID = Convert.ToInt64(s.GROUPID),
                        TO_GROUPNM = Convert.ToString(s.GROUPNM),
                        NAME = s.NAME,
                        EMAIL = s.EMAIL,
                        MOBNO1 = s.MOBNO1,
                        MOBNO2 = s.MOBNO2,
                        ADDRESS = s.ADDRESS,
                        INSUSERID = s.INSUSERID,
                        INSLTUDE = s.INSLTUDE,
                        INSTIME = s.INSTIME,
                        INSIPNO = s.INSIPNO,
                    };
                }
            }
        }



        private bool IsValidEmail(string email)
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



        private bool BdNumberValidate(string number)
        {
            try
            {
                if (number.Length > 13 || number.Length < 13)
                {
                    return false;
                }
                else
                {
                    string operatorCode = number.Substring(0, 5);
                    switch (operatorCode)
                    {
                        case "88018":
                        case "88017":
                        case "88019":
                        case "88016":
                        case "88011":
                        case "88015":
                            return true; //all of the operator in case is return true
                            break;
                        default:
                            return false; //other operator code return false
                    }
                }
            }
            catch
            {
                return false;
            }
        }




        //[Route("api/ApiUploadExchangeContactController/grid/UpdateData")]
        [HttpPost]
        [ActionName("Update")]
        public HttpResponseMessage UpdateData(UploadContactDTO model)
        {
            if (IsValidEmail(model.EMAIL) || BdNumberValidate(model.MOBNO1) || BdNumberValidate(model.MOBNO2))
            {
                var check_EMAIL_data = (from n in db.UploadContactDbSet where n.ID != model.ID && n.COMPID == model.COMPID && n.GROUPID == model.TO_GROUPID && n.EMAIL == model.EMAIL select n).ToList();
                var check_MObileNo1 = (from n in db.UploadContactDbSet where n.ID != model.ID && n.COMPID == model.COMPID && n.GROUPID == model.TO_GROUPID && (n.MOBNO1 == model.MOBNO1 || n.MOBNO2 == model.MOBNO1) select n).ToList();
                //var check_MObileNo1_datainMOBNO2 = (from n in db.UploadContactDbSet where n.ID != model.ID && n.COMPID == model.COMPID && n.GROUPID == model.TO_GROUPID && n.MOBNO2 == model.MOBNO1 select n).ToList();
                var check_MObileNo2 = (from n in db.UploadContactDbSet where n.ID != model.ID && n.COMPID == model.COMPID && n.GROUPID == model.TO_GROUPID && (n.MOBNO1 == model.MOBNO2 || n.MOBNO2 == model.MOBNO2) select n).ToList();
                //var check_MObileNo2_datainMOBNO1 = (from n in db.UploadContactDbSet where n.ID != model.ID && n.COMPID == model.COMPID && n.GROUPID == model.TO_GROUPID && n.MOBNO1 == model.MOBNO2 select n).ToList();
                if (model.From_GROUPID == model.TO_GROUPID && check_EMAIL_data.Count == 0 && check_MObileNo1.Count == 0 && check_MObileNo2.Count == 0)
                {
                    //Update Logic
                    var data_find = (from n in db.UploadContactDbSet
                                     where n.ID == model.ID && n.COMPID == model.COMPID && n.GROUPID == model.TO_GROUPID
                                     select n).ToList();
                    foreach (var item in data_find)
                    {
                        item.ID = model.ID;
                        item.COMPID = model.COMPID;
                        item.GROUPID = Convert.ToInt64(model.TO_GROUPID);
                        item.NAME = model.NAME;
                        if (IsValidEmail(model.EMAIL))
                        {
                            item.EMAIL = model.EMAIL;
                        }
                        else
                        {
                            item.EMAIL = null;
                        }
                        if (model.MOBNO1 != null && BdNumberValidate(model.MOBNO1))
                        {
                            item.MOBNO1 = model.MOBNO1;
                        }
                        else
                        {
                            item.MOBNO1 = null;
                        }
                        if (model.MOBNO2 != null && BdNumberValidate(model.MOBNO2))
                        {
                            item.MOBNO2 = model.MOBNO2;
                        }
                        else
                        {
                            item.MOBNO2 = null;
                        }
                        item.ADDRESS = model.ADDRESS;

                        item.USERPC = strHostName;
                        item.UPDIPNO = ipAddress.ToString();
                        item.UPDLTUDE = Convert.ToString(model.INSLTUDE);
                        item.UPDTIME = Convert.ToDateTime(td);
                        item.UPDUSERID = Convert.ToInt16(model.INSUSERID);

                    }
                    db.SaveChanges();

                    //Log data saved from UploadContact Tabel (update)
                    UploadEditController controller = new UploadEditController();
                    controller.Update_LogData(model);

                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                    return response;
                }
                else if (model.From_GROUPID != model.TO_GROUPID && check_EMAIL_data.Count == 0 && check_MObileNo1.Count == 0 && check_MObileNo2.Count == 0)
                {
                    ASL_PCONTACTS contactsExchanged = new ASL_PCONTACTS();

                    contactsExchanged.COMPID = model.COMPID;
                    contactsExchanged.GROUPID = Convert.ToInt64(model.TO_GROUPID);
                    contactsExchanged.NAME = model.NAME;
                    if (IsValidEmail(model.EMAIL))
                    {
                        contactsExchanged.EMAIL = model.EMAIL;
                    }
                    else
                    {
                        contactsExchanged.EMAIL = null;
                    }
                    if (model.MOBNO1 != null && BdNumberValidate(model.MOBNO1))
                    {
                        contactsExchanged.MOBNO1 = model.MOBNO1;
                    }
                    else
                    {
                        contactsExchanged.MOBNO1 = null;
                    }
                    if (model.MOBNO2 != null && BdNumberValidate(model.MOBNO2))
                    {
                        contactsExchanged.MOBNO2 = model.MOBNO2;
                    }
                    else
                    {
                        contactsExchanged.MOBNO2 = null;
                    }
                    contactsExchanged.ADDRESS = model.ADDRESS;


                    contactsExchanged.USERPC = strHostName;
                    contactsExchanged.INSIPNO = ipAddress.ToString();
                    contactsExchanged.INSTIME = Convert.ToDateTime(td);
                    contactsExchanged.INSUSERID = model.INSUSERID;
                    contactsExchanged.INSLTUDE = Convert.ToString(model.INSLTUDE);

                    db.UploadContactDbSet.Add(contactsExchanged);
                    db.SaveChanges();

                    //Log data saved from UploadContact Tabel (exchange data)
                    UploadEditController controller = new UploadEditController();
                    controller.Insert_Exchange_LogData(model);

                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                    return response;
                }
                else
                {
                    model.CheckPreviousData = 1;
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                    return response;
                }
            }
            else
            {
                model.CheckValidation = 1;
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                return response;
            }
        }









        //[Route("api/ApiUploadExchangeContactController/grid/DeleteData")]
        [HttpPost]
        [ActionName("Delete")]
        public HttpResponseMessage DeleteData(UploadContactDTO model)
        {
            ASL_PCONTACTS deleteModel = new ASL_PCONTACTS();
            deleteModel.ID = model.ID;
            deleteModel.COMPID = model.COMPID;

            deleteModel = db.UploadContactDbSet.Find(deleteModel.ID, deleteModel.COMPID);
            db.UploadContactDbSet.Remove(deleteModel);
            db.SaveChanges();

            //Log data save from GheadMst Tabel
            UploadEditController controller = new UploadEditController();
            controller.Delete_Exchange_LogData(model);
            controller.Delete_Exchange_LogDelete(model);


            UploadContactDTO Obj = new UploadContactDTO();
            return Request.CreateResponse(HttpStatusCode.OK, Obj);
        }
    }
}
