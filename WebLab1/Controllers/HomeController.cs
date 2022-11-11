using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebLab1.Models;
using WebLab1.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System;

namespace WebLab1.Controllers
{
    public class HomeController : Controller
    {
        protected readonly IEmailSender _emailSender;
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _environment;

        public HomeController(ILogger<HomeController> logger, IEmailSender emailSender, IWebHostEnvironment environment)
        {
            _logger = logger;
            _emailSender = emailSender;
            _environment = environment;
        }
        [Route("home")]
        [Route("")]

        public IActionResult Index()
        {
            return View();
        }
        [Route("upload-files")]
        public IActionResult UploadFiles()
        {
            var files = Directory.GetFiles(_environment.WebRootPath + "/Files/");
            var list = new List<string>();
            foreach(var file in files)
            {
                list.Add(Path.GetFileName(file));
            }
            return View(list);
        }
        [Route("feed-back")]
        public IActionResult FeedBack()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpPost]
        [Route("send-email")]
        public async Task<IActionResult> SendEmail(MailModel mailModel)
        {
            await _emailSender.SendEmailAsync(mailModel.To, mailModel.ToName, "Feedback message", mailModel.Body);
            return RedirectToAction("Index");
        }
        [HttpPost]
        [Route("FileUpload")]
        public async Task<IActionResult> AddFile(IFormFile file)
        {
            try
            {
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString()+Path.GetExtension(file.FileName);
                    string path = Path.Combine(_environment.WebRootPath + "/Files/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
            }
            catch (Exception)
            {
            }
                return RedirectToAction("UploadFiles");
        }
    }
}
