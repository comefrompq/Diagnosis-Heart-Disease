using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DHD_System.Models.DiagnosisViewModels;
using DHDSystem.Data.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Query;

namespace DHD_System.Controllers
{
    public class DiagnosisController : Controller
    {
        private Data _data;
        public DiagnosisController()
        {
            _data = new Data();
        }
        public IActionResult Index()
        {
            TestViewModel options = new TestViewModel();
            options.ListOptions = new List<String>();
            ViewBag.ListOptions = new SelectList(_data.GetAllSymptoms());
            return View(options);
        }
        [HttpPost]
        public IActionResult Index(TestViewModel model)
        {
            TestViewModel newModel = new TestViewModel();
            newModel.ListOptions = _data.GetDiseases(model.ListOptions);

            return PartialView("_DiagnosisPartial", newModel);
        }
        [HttpGet]
        public IActionResult TreatmentFor(string disease)
        {
            TestViewModel model = new TestViewModel();
            model.ListOptions = _data.GetTreatmentFor(disease);

            return PartialView("_TreatmentPartial", model);
        }

    }
}