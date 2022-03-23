using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace Hangfire.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HangController : ControllerBase
    {
        private readonly ILogger _logger;

        public HangController(ILogger<HangController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                //_logger.LogInformation
                Console.WriteLine($"Request: {DateTime.Now}");
                var jobFireForget = BackgroundJob.Enqueue(() => Console.WriteLine($"Fire and forget: {DateTime.Now}"));
                var jobDelayed = BackgroundJob.Schedule(() => Console.WriteLine($"Delayed: {DateTime.Now}"), TimeSpan.FromSeconds(10));

                BackgroundJob.ContinueJobWith(jobDelayed, () => Console.WriteLine($"Continuation Emails: {DateTime.Now}"));
                RecurringJob.AddOrUpdate(() => Console.WriteLine($"Recurring: {DateTime.Now}"), Cron.Minutely);

                return Ok("Jobs criados com sucesso");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }
        }

    //    public void SendEmail()
    //    {
    //        Console.WriteLine($"Continuation Emails: {DateTime.Now}");
    //    }
    }
}
