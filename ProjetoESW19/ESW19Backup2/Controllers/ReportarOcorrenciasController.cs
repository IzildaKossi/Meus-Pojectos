using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ESW19Backup2.Data;
using ESW19Backup2.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Extensions.FileProviders;
using ESW19Backup2.Models.Upload;
using Microsoft.Extensions.Logging;

namespace ESW19Backup2.Controllers
{
    public class ReportarOcorrenciasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileProvider _fileProvider;
        List<string> listaprioridades = new List<string>();


        public ReportarOcorrenciasController(ApplicationDbContext context, IFileProvider fileProvider)
        {
            _context = context;
            _fileProvider = fileProvider;
        }

        // GET: ReportarOcorrencias
        public IActionResult Index()
        {
            return PartialView(ProperView("Index"));
        }

        protected String ProperView(String viewName)
        {

            return viewName;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file, ReportarOcorrencia modelReportarOcorrencia)
        {
            if (file == null || file.Length == 0)
                return Content("file not selected");

            var path = Path.Combine(
                        Directory.GetCurrentDirectory(), "imagensReportadas",
                        file.GetFilename());

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            if (ModelState.IsValid)
            {
                var ocorrencia = new ReportarOcorrencia
                {
					ReportarOcorrenciaId = modelReportarOcorrencia.ReportarOcorrenciaId,
                    Propriedade = modelReportarOcorrencia.Propriedade,
                    Descricao = modelReportarOcorrencia.Descricao,
                    Localizacao = modelReportarOcorrencia.Localizacao,
                    Name = file.GetFilename().ToString()
                };
                _context.Add(ocorrencia);
                await _context.SaveChangesAsync();
                SetSuccessMessage("Ocorrênçia Reportada com sucesso. Muito Obrigado pela sua colaboração");
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Files()
        {
            var descricoes = this._context.ReportarOcorrencia
         .Where(x => x.ReportarOcorrenciaId > 0)
         .GroupBy(s => new { district = s.Descricao })
         .Select(x => new { count = x.Count(), district = x.Key.district })
         .ToList();

            var loc = this._context.ReportarOcorrencia
                .Where(x => x.ReportarOcorrenciaId > 0)
                .GroupBy(s => new { district = s.Localizacao })
         .Select(x => new { count = x.Count(), district = x.Key.district })
         .ToList();

            var pri = this._context.ReportarOcorrencia
                .Where(x => x.ReportarOcorrenciaId > 0)
                .GroupBy(s => new { district = s.Propriedade })
         .Select(x => new { count = x.Count(), district = x.Key.district })
         .ToList();


            int count = 0;
            var model = new FilesViewModel();
            foreach (var item in _fileProvider.GetDirectoryContents(""))
            {

                model.Files.Add(
                    new FileDetails { Name = item.Name, Path = item.PhysicalPath, Descricao = count >= descricoes.Count() ? "" : descricoes[count].district, Localizacao = count >= loc.Count() ? "" : loc[count].district, Prioridade = count >= pri.Count() ? "" : pri[count].district });
                count++;
            }
            return View(model);
        }


        public string Display()
        {
            var StreetCrimes = this._context.ReportarOcorrencia
   .Where(x => x.ReportarOcorrenciaId > 0)
   .GroupBy(s => new { district = s.Descricao })
   .Select(x => new { count = x.Count(), district = x.Key.district })
   .ToList();

            string toRet = "";
            foreach (var crime in StreetCrimes)
            {
                toRet += crime.district;

            }
            return toRet;

        }

        // GET: ReportarOcorrencias/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ReportarOcorrencias/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReportarOcorrenciaId,Propriedade,Descricao,Localizacao")] ReportarOcorrencia modelReportarOcorrencia)
        {
            if (ModelState.IsValid)
            {
                _context.Add(modelReportarOcorrencia);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(modelReportarOcorrencia);
        }



        public async Task<IActionResult> Download(string filename)
        {
            if (filename == null)
                return Content("filename not present");

            var path = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           "imgReportadas", filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }

        private string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }

        protected void SetSuccessMessage(String Message)
        {
            TempData["SucessoRO"] = Message;
        }

        //protected bool Image( IFormFile postedFile)
        //{
        //    return true;
        //}

        private bool IsImage(IFormFile postedFile)
        {
            if (Path.GetExtension(postedFile.FileName).ToLower() != ".jpg" && Path.GetExtension(postedFile.FileName).ToLower() != ".png"
                  && Path.GetExtension(postedFile.FileName).ToLower() != ".gif" && Path.GetExtension(postedFile.FileName).ToLower() != ".jpeg")
            {
                return true;
            }
            else
            {

                return false;
            }
        }
        }
}