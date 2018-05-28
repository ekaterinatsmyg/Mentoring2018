using System.IO;
using System.Linq;
using System.Web.Mvc;
using ProfileSample.DAL;
using ProfileSample.Models;

namespace ProfileSample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var context = new ProfileSampleEntities();

            var sources = context.ImgSources.AsNoTracking().Select(x => new ImageModel { Name = x.Name, Data = x.Data }).Take(20);

            //var model = new List<ImageModel>();

            ////foreach (var id in sources)
            ////{
            ////    var item = context.ImgSources.Find(id);

            ////    var obj = new ImageModel()
            ////    {
            ////        Name = item.Name,
            ////        Data = item.Data
            ////    };

            //model.AddRange(sources);

            return View(sources.ToList());
        }

        public ActionResult Convert()
        {
            using (var context = new ProfileSampleEntities())
            {
                foreach (var file in Directory.EnumerateFiles(Server.MapPath("~/Content/Img"), "*.jpg"))
                {
                    var buff = System.IO.File.ReadAllBytes(file);
                    
                    context.ImgSources.Add(new ImgSource()
                    {
                        Name = Path.GetFileName(file),
                        Data = buff,
                    });

                    context.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}