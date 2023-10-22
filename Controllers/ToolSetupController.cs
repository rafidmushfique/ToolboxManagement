using LILI_TTS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace LILI_TTS.Controllers
{
    [Authorize]
    public class ToolSetupController : Controller
    {
        private readonly dbToolsManagementContext _context;

        public ToolSetupController(dbToolsManagementContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;
            IQueryable<TblToolsSetup> model = _context.TblToolsSetup;


            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(s => s.ToolName.Contains(searchString) || s.ToolCode.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    model = model.OrderByDescending(s => s.ToolCode);
                    break;

                default:
                    model = model.OrderBy(s => s.Idate);
                    break;
            }
            int pageSize = 7;
            return View(await PaginatedList<TblToolsSetup>.CreateAsync(model.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public IActionResult Create()
        {

            TblToolsSetup model = new TblToolsSetup();
            //var newGeneratedCode = GenerateToolCode();
            model.ToolCode= GenerateToolCode();
            TempData["msg"] = "";
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateToolSetup(TblToolsSetup model)
        {
            try
            {
               //TempData["msg"] = "Data Save Unsuccessful";
                //return View("Create", model);

                if (ModelState.IsValid)
                {
                    if (DoesToolCodeExists(model.ToolCode))
                    {
                        model.ToolCode = GenerateToolCode().ToString();
                    }
                    model.Iuser = User.Identity.Name;
                    model.Idate = DateTime.Now;
                    _context.Add(model);
                    await _context.SaveChangesAsync();

                   
                }
                else 
                {
                    TempData["msg"] = "Data Save Unsuccessful";
                    return View("Create", model);
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Error Occured : "+ex.Message;
                return View("Create", model);

            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Update(int vId) {
          TblToolsSetup toolModel= new TblToolsSetup();
            var result = _context.TblToolsSetup.Where(s=>s.Id== vId).First();
            toolModel.Id = result.Id;
            toolModel.ToolCode = result.ToolCode;
            toolModel.ToolName = result.ToolName;
            toolModel.Brand = result.Brand;
            toolModel.Description = result.Description;
            toolModel.Unit = result.Unit;


         return View(toolModel);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateToolSetup(TblToolsSetup model) {
            var vId=   model.Id;
            try
            {
                var toolSetupToUpdate = await _context.TblToolsSetup.FirstOrDefaultAsync(s=>s.Id==vId);
                toolSetupToUpdate.Edate= DateTime.Now;
                toolSetupToUpdate.Euser= User.Identity.Name;
                if (await TryUpdateModelAsync<TblToolsSetup>(
                    toolSetupToUpdate,
                    "",
                    s => s.ToolName,
                    s => s.Unit,
                    s => s.Brand,
                    s => s.Description
                    
                    )) ;
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.");
            }
            return View(model);
        }
        public bool Delete(int vId)
        {
            try
            {
                TblToolsSetup model= _context.TblToolsSetup.Where(s=> s.Id == vId).First();
                if (model != null)
                {
                    _context.TblToolsSetup.Remove(model);
                    _context.SaveChanges();
                    return true;
                }
                else 
                {
                   
                    return false;
                }
            }
            catch (Exception ex)
            {
                TempData["msg"] = "Error Occurred while trying to Delete data.";
                return false;
            }
        }

        #region private classes
        private  string GenerateToolCode()
        {
         
            var yearMonth = DateTime.Now.ToString("yyyyMM");
            var result =  _context.TblToolsSetup.OrderBy(x => x.Id).Select(x=>x.ToolCode).LastOrDefault();
            var lastGrn = string.IsNullOrEmpty(result) ? "00000000000000" : result;


            var last5digits = "1";
            if (lastGrn.Length > 5)
            {
                last5digits = lastGrn.Substring(lastGrn.Length - 5);
            }

            int lastNumber = Int32.Parse(last5digits) + 1;
            string lastNumberString = lastNumber.ToString("D5");
            //             return $"{companyCode}{plantCode}gr{yearMonth}{lastNumberString}";
            var generatedCode = $"T-{yearMonth}{lastNumberString}";
            return generatedCode;
        }
        public bool DoesToolCodeExists(string vToolCode)
        {

            return _context.TblToolsSetup.Any(e => e.ToolCode == vToolCode);
        }
        #endregion
    }
}
