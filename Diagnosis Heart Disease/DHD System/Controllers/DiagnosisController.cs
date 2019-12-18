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
        public IActionResult Diagnosis()
        {
            DiagnosisViewModel options = new DiagnosisViewModel();
            options.Symptoms = new List<String>();
            ViewBag.ListOptions = new SelectList(_data.GetAllSymptoms());
            return View(options);
        }
        [HttpPost]
        public IActionResult Diagnosis(DiagnosisViewModel model)
        {
            DiagnosisViewModel newModel = new DiagnosisViewModel();
            newModel.Symptoms = _data.GetDiseases(model.Symptoms);

            return PartialView("_DiagnosisPartial", newModel);
        }
        [HttpGet]
        public IActionResult TreatmentFor(string disease)
        {
            List<string> model = new List<string>();
            model = _data.GetTreatmentFor(disease);

            return PartialView("_TreatmentPartial", model);
        }

    }
}