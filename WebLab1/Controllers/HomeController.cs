﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebLab1.Models;
using WebLab1.Services.Interfaces;

namespace WebLab1.Controllers
{
    public class HomeController : Controller
    {
        protected readonly IEmailSender _emailSender;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IEmailSender emailSender)
        {
            _logger = logger;
            _emailSender = emailSender;
        }
        [Route("home")]
        [Route("")]

        public IActionResult Index()
        {
            return View();
        }
        [Route("about-us")]
        public IActionResult AboutUs()
        {
            return View();
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
    }
}
