using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebServicesLayerSG.Util;

namespace SG_Constancia_TSC.Controllers
{
    public class SubirArchivo : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> UploadFile(HttpPostedFileBase file, int idFile, string flexFields, string connectionString)
        {
            if (file == null || file.ContentLength == 0)
            {
                ViewBag.Message = "Please select a file.";
                return View("Index");
            }

            var result = await Utili.Utila.SubirArchivo(idFile, file, connectionString, flexFields);

            if (result.typeResult == WebServicesLayerSG.Util.UtilClass.codigoExitoso)
            {
                ViewBag.Message = "File uploaded successfully!";
            }
            else
            {
                ViewBag.Message = $"File upload failed: {result.message}";
            }

            return View("Index");
        }
    }
}