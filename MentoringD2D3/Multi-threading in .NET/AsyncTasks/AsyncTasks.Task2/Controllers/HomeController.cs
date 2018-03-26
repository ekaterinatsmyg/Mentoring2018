using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using AsyncTasks.Task2.Interfaces;
using AsyncTasks.Task2.Models;

namespace AsyncTasks.Task2.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDownloadService downloadService;
        public HomeController(IDownloadService downloadService)
        {
            this.downloadService = downloadService;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult DownloadSite(UrlViewModel model)
        {
            downloadService.CreateNew(model);

            return PartialView(model);
        }

        public ActionResult Cancel(UrlViewModel model)
        {
            return PartialView(downloadService.CancelLoading(model));
        }

        public async Task<string> StartDownload(Guid Id)
        {
            return await downloadService.StartNewLoading(Id);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}