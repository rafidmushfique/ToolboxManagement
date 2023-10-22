using LILI_TTS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;

namespace LILI_TTS.Controllers
{
    [Authorize]
    public class TSAController : Controller
    {
        private readonly dbToolsManagementContext _context;

        public TSAController(dbToolsManagementContext context)
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
            IQueryable<TblTsasetup> model = _context.TblTsasetup;


            if (!String.IsNullOrEmpty(searchString))
            {
                model = model.Where(s => s.Tsaname.Contains(searchString) || s.Designation.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    model = model.OrderByDescending(s => s.Id);
                    break;

                default:
                    model = model.OrderBy(s => s.Id);
                    break;
            }
            int pageSize = 7;
            return View(await PaginatedList<TblTsasetup>.CreateAsync(model.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        public IActionResult Create()
        {

            TblTsasetup model = new TblTsasetup();
            //List<TblRegion> regionList = new List<TblRegion>();
            var regionList=(from c in  _context.TblRegion
                            select new 
                            { 
                             RegionCode=c.RegionCode,
                             RegionName=c.RegionName,
                            }
                            ).ToList();
            regionList.Insert(0, new { RegionCode = "", RegionName = "<Select Region>" });
            ViewBag.ListOfRegion = regionList;

            //List<TblArea> areaList = new List<TblArea>();
            var areaList = (from c in _context.TblArea 
                        select new 
                        {
                         AreaCode=c.AreaCode,
                         AreaName=c.AreaName
                        }
                        ).ToList();

            areaList.Insert(0, new { AreaCode = "", AreaName = "<Select Area>"});
            ViewBag.ListOfArea = areaList;

            model.Tsacode= GenerateTsaCode();
            TempData["msg"] = "";
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateTSA(TblTsasetup model)
        {
            try
            {
               //TempData["msg"] = "Data Save Unsuccessful";
                //return View("Create", model);

                if (ModelState.IsValid)
                {
                    if (DoesToolCodeExists(model.Tsacode))
                    {
                        model.Tsacode = GenerateTsaCode();
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
         
            TblTsasetup model = new TblTsasetup();
            List<TblRegion> regionList = new List<TblRegion>();
            regionList = _context.TblRegion.ToList();
            ViewBag.ListOfRegion = regionList;

            List<TblArea> areaList = new List<TblArea>();
            areaList = _context.TblArea.ToList();
            ViewBag.ListOfArea = areaList;

            var result = _context.TblTsasetup.Where(s=>s.Id== vId).First();
            model.Id = result.Id;
            model.Tsacode = result.Tsacode;
            model.Tsaname = result.Tsaname;
            model.Designation = result.Designation;
            model.AreaCode = result.AreaCode;
            model.RegionCode = result.RegionCode;
            model.Description = result.Description;
            model.MobileNo = result.MobileNo;
            TempData["msg"] = "";
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateTSAinfo(int id,TblTsasetup model) {
            if (id != model.Id)
            {
                return NotFound();
            }
            try
            {
               
                var tsaToUpdate = await _context.TblTsasetup.FirstOrDefaultAsync(s=>s.Id== id);
                tsaToUpdate.Edate= DateTime.Now;
                tsaToUpdate.Euser= User.Identity.Name;
                
                if (await TryUpdateModelAsync<TblTsasetup>(
                    tsaToUpdate,
                    "",
                    s => s.Tsaname,
                    s => s.Designation,
                    s => s.MobileNo,
                    s => s.Description, 
                    s => s.AreaCode,
                    s => s.RegionCode
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
                TblTsasetup model= _context.TblTsasetup.Where(s=> s.Id == vId).First();
                if (model != null)
                {
                    _context.TblTsasetup.Remove(model);
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
        private  string GenerateTsaCode()
        {
         
            var yearMonth = DateTime.Now.ToString("yyyyMM");
            var result =  _context.TblTsasetup.OrderBy(x => x.Id).Select(x=>x.Tsacode).LastOrDefault();
            var lastGrn = string.IsNullOrEmpty(result) ? "00000000000000" : result;


            var last5digits = "1";
            if (lastGrn.Length > 5)
            {
                last5digits = lastGrn.Substring(lastGrn.Length - 5);
            }

            int lastNumber = Int32.Parse(last5digits) + 1;
            string lastNumberString = lastNumber.ToString("D5");
            //             return $"{companyCode}{plantCode}gr{yearMonth}{lastNumberString}";
            var generatedCode = $"TSA-{yearMonth}{lastNumberString}";
            return generatedCode;
        }
        public bool DoesToolCodeExists(string vTsaCode)
        {

            return _context.TblTsasetup.Any(e => e.Tsacode == vTsaCode);
        }
        #endregion
    }
}
