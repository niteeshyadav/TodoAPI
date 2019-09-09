﻿using Microsoft.AspNetCore.Mvc;
using StackExchange.Profiling;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoProfilingController : ControllerBase
    {
        // GET: api/DemoProfiling
        // url to see last profile check: http://localhost:xxxxx/profiler/results
        // profile available in swagger page too
        [HttpGet]
        public IEnumerable<string> Get()
        {
            string url1 = string.Empty;
            string url2 = string.Empty;
            using (MiniProfiler.Current.Step("Get method"))
            {
                using (MiniProfiler.Current.Step("Prepare data"))
                {
                    using (MiniProfiler.Current.CustomTiming("SQL", "SELECT * FROM Config"))
                    {
                        // Simulate a SQL call
                        Thread.Sleep(500);
                        url1 = "https://google.com";
                        url2 = "https://stackoverflow.com/";
                    }
                }
                using (MiniProfiler.Current.Step("Use data for http call"))
                {
                    using (MiniProfiler.Current.CustomTiming("HTTP", "GET " + url1))
                    {
                        var client = new WebClient();
                        var reply = client.DownloadString(url1);
                    }

                    using (MiniProfiler.Current.CustomTiming("HTTP", "GET " + url2))
                    {
                        var client = new WebClient();
                        var reply = client.DownloadString(url2);
                    }
                }
            }
            return new string[] { "value1", "value2" };
        }
    }
}