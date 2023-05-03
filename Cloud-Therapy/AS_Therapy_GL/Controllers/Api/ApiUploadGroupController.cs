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
    public class ApiUploadGroupController : ApiController
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

        public ApiUploadGroupController()
        {
            strHostName = System.Net.Dns.GetHostName();
            ipHostInfo = Dns.Resolve(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[0];
            td = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
        }


        //[Route("api/ApiUploadGroupController/GetGroupData")]
        [System.Web.Http.HttpGet]
        public IEnumerable<UploadGroupDTO> GetGroupData(string Compid)
        {
            Int64 compid = Convert.ToInt64(Compid);
            var find_GridData = (from t1 in db.UploadGroupDbSet
                                 where t1.COMPID == compid
                                 select new
                                 {
                                     t1.ID,
                                     t1.COMPID,
                                     t1.GROUPID,
                                     t1.GROUPNM,

                                     t1.INSIPNO,
                                     t1.INSLTUDE,
                                     t1.INSTIME,
                                     t1.INSUSERID,
                                 }).ToList();

            if (find_GridData.Count == 0)
            {
                yield return new UploadGroupDTO
                {
                    count = 1, //no data found
                };
            }
            else
            {
                foreach (var s in find_GridData)
                {
                    yield return new UploadGroupDTO
                    {
                        ID = s.ID,
                        COMPID = Convert.ToInt64(s.COMPID),
                        GROUPID = Convert.ToInt64(s.GROUPID),
                        GROUPNM = s.GROUPNM,
                        INSUSERID = s.INSUSERID,
                        INSLTUDE = s.INSLTUDE,
                        INSTIME = s.INSTIME,
                        INSIPNO = s.INSIPNO,
                    };
                }
            }
        }



        //[Route("api/ApiUploadGroupController/grid/addData")]
        [HttpPost]
        [ActionName("Add")]
        public HttpResponseMessage AddData(UploadGroupDTO model)
        {
            ASL_PGROUPS uploadGroup = new ASL_PGROUPS();

            var check_data = (from n in db.UploadGroupDbSet where n.COMPID == model.COMPID && n.GROUPNM == model.GROUPNM select n).ToList();
            if (check_data.Count == 0)
            {
                var find_data = (from n in db.UploadGroupDbSet where n.COMPID == model.COMPID select n.GROUPID).ToList();
                if (find_data.Count == 0)
                {
                    uploadGroup.GROUPID = Convert.ToInt64(Convert.ToString(model.COMPID) + "01");
                }
                else
                {
                    Int64 find_Max_MCATID = Convert.ToInt64((from n in db.UploadGroupDbSet where n.COMPID == model.COMPID select n.GROUPID).Max());
                    uploadGroup.GROUPID = find_Max_MCATID + 1;
                }

                uploadGroup.COMPID = model.COMPID;
                uploadGroup.GROUPNM = model.GROUPNM;
                uploadGroup.USERPC = strHostName;
                uploadGroup.INSIPNO = ipAddress.ToString();
                uploadGroup.INSTIME = Convert.ToDateTime(td);
                uploadGroup.INSUSERID = model.INSUSERID;
                uploadGroup.INSLTUDE = Convert.ToString(model.INSLTUDE);

                db.UploadGroupDbSet.Add(uploadGroup);
                db.SaveChanges();

                model.ID = uploadGroup.ID;
                model.COMPID = uploadGroup.COMPID;
                model.GROUPID = Convert.ToInt64(uploadGroup.GROUPID);
                model.GROUPNM = uploadGroup.GROUPNM;
                model.USERPC = uploadGroup.USERPC;
                model.INSIPNO = uploadGroup.INSIPNO;
                model.INSTIME = uploadGroup.INSTIME;
                model.INSUSERID = uploadGroup.INSUSERID;
                model.INSLTUDE = Convert.ToString(uploadGroup.INSLTUDE);

                //Log data save from UploadGroup Tabel
                UploadGroupController groupController = new UploadGroupController();
                groupController.Insert_UploadGroup_LogData(model);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                return response;
            }
            else
            {
                model.GROUPID = 0;
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                return response;
            }
        }




        //[Route("api/ApiUploadGroupController/grid/UpdateData")]
        [HttpPost]
        [ActionName("Update")]
        public HttpResponseMessage UpdateData(UploadGroupDTO model)
        {
            var check_data = (from n in db.UploadGroupDbSet where n.COMPID == model.COMPID && n.GROUPNM == model.GROUPNM select n).ToList();
            if (check_data.Count == 0)
            {
                var data_find = (from n in db.UploadGroupDbSet where n.ID == model.ID && n.COMPID == model.COMPID && n.GROUPID == model.GROUPID select n).ToList();

                foreach (var item in data_find)
                {
                    item.ID = model.ID;
                    item.COMPID = model.COMPID;
                    item.GROUPID = Convert.ToInt64(model.GROUPID);
                    item.GROUPNM = model.GROUPNM;

                    item.USERPC = strHostName;
                    item.UPDIPNO = ipAddress.ToString();
                    item.UPDLTUDE = Convert.ToString(model.INSLTUDE);
                    item.UPDTIME = Convert.ToDateTime(td);
                    item.UPDUSERID = Convert.ToInt16(model.INSUSERID);

                }
                db.SaveChanges();

                //Log data save from MediMst Tabel
                UploadGroupController groupController = new UploadGroupController();
                groupController.update_UploadGroup_LogData(model);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                return response;
            }
            else
            {
                model.GROUPID = 0;
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, model);
                return response;
            }
        }




        //[Route("api/ApiUploadGroupController/grid/DeleteData")]
        [HttpPost]
        [ActionName("Delete")]
        public HttpResponseMessage DeleteData(UploadGroupDTO model)
        {
            ASL_PGROUPS deleteModel = new ASL_PGROUPS();
            deleteModel.ID = model.ID;
            deleteModel.COMPID = model.COMPID;
            deleteModel.GROUPID = Convert.ToInt64(model.GROUPID);

            var findChildData = (from n in db.UploadContactDbSet
                                 where n.GROUPID == deleteModel.GROUPID && n.COMPID == deleteModel.COMPID
                                 select n).ToList();

            UploadGroupDTO GroupObj = new UploadGroupDTO();

            if (findChildData.Count != 0)
            {
                GroupObj.GetChildDataForDeleteMasterCategory = 1; // True (child data found)
            }
            else
            {
                deleteModel = db.UploadGroupDbSet.Find(deleteModel.ID, deleteModel.COMPID, deleteModel.GROUPID);
                db.UploadGroupDbSet.Remove(deleteModel);
                db.SaveChanges();

                //Log data save from MediMst Tabel
                UploadGroupController groupController = new UploadGroupController();
                groupController.Delete_UploadGroup_LogData(model);
                groupController.Delete_UploadGroup_LogDelete(model);
            }
            return Request.CreateResponse(HttpStatusCode.OK, GroupObj);
        }
    }
}
