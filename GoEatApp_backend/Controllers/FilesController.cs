using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoEatApp_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IWebHostEnvironment _appEnvironment;

        public FilesController(IWebHostEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }

        [HttpGet("{filename}")]
        public async Task<ActionResult> GetBytes(string filename)
        {
            byte[] mas;
            try
            {
                mas = System.IO.File.ReadAllBytes(Path.Combine(_appEnvironment.ContentRootPath, "Files\\" + filename));
            }
            catch
            {
                return await Task.FromResult(NotFound());
            }
            return await Task.FromResult(File(mas, "application/jpg"));
        }
    }
}
