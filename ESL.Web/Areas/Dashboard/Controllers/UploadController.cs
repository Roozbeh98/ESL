using ESL.Services.BaseRepository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ESL.Web.Areas.Dashboard.Controllers
{
    public class UploadController : Controller
    {
        [HttpPost]
        public JsonResult SaveFile(Guid guid)
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    file.SaveAs(Server.MapPath($"/App_Data/{Rep_CodeGroup.Get_CodeNameWithGUID(guid)}/{file.FileName}"));
                    return Json(file.FileName);
                }
            }

            return Json(false);
        }

        [HttpDelete]
        public JsonResult RevertFile(Guid guid)
        {
            string res;

            MemoryStream memstream = new MemoryStream();
            Request.InputStream.CopyTo(memstream);
            memstream.Position = 0;
            using (StreamReader reader = new StreamReader(memstream))
            {
                res = reader.ReadToEnd();
            }

            res = res.Remove(res.Length - 1);
            string filename = res.Substring(1);

            string source = Request.MapPath($"/App_Data/{Rep_CodeGroup.Get_CodeNameWithGUID(guid)}/{filename}");

            if (System.IO.File.Exists(source))
            {
                System.IO.File.Delete(source);
            }

            return Json(true);
        }
    }
}