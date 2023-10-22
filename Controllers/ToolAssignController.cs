using TMS.Models;
using TMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace TMS.Controllers
{
    [Authorize]
    public class ToolAssignController : Controller
    {
        private readonly dbToolsManagementContext _context;

        public ToolAssignController(dbToolsManagementContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            //TempData["msg"]="";
            TblToolAssign model=new TblToolAssign() ;
            var tsalist =  (from d in _context.TblTsasetup
                            select new
                            {
                                Tsacode = d.Tsacode,
                                Tsaname = d.Tsaname,
                            }).ToList();
            var toolList = (from c in _context.TblToolsSetup
                            join ta in _context.TblToolAssign
                            on c.ToolCode equals ta.ToolCode into gj
                            from subTa in gj.DefaultIfEmpty()
                            where subTa == null
                            select new
                            {
                                ToolCode = c.ToolCode,
                                ToolName = c.ToolName
                            }).ToList();
            var actionList= (from c in _context.TblAction
                             select new
                             {
                                 ActionCode = c.ActionCode,
                                 ActionName = c.ActionName
                             }).ToList();
            var actionTypeList= (from c in _context.TblActionType
                                 select new
                                 {
                                     ActionTypeCode = c.ActionTypeCode,
                                     ActionTypeName = c.ActionTypeName
                                 }).ToList();

            tsalist.Insert(0, new { Tsacode = "0", Tsaname = "-- Select TSA --"});
            toolList.Insert(0, new { ToolCode = "0", ToolName = "-- Select Tool --" });
            actionList.Insert(0, new { ActionCode = "0", ActionName = "-- Select Action --" });
            actionTypeList.Insert(0, new { ActionTypeCode = "0", ActionTypeName = "-- Select Action Type --" });

            ViewBag.ListOfTSA = tsalist.ToList();
            ViewBag.ToolList = toolList.ToList();
            ViewBag.ListOfAction = actionList.ToList();
            ViewBag.ListOfActionType = actionTypeList.ToList();
            return View(model);
        }
        public async Task<IActionResult> AssignTools(TblToolAssign model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    model.AssignCode = GenerateCode();
                    var toolCode = model.ToolCode;
                    var vTsaCode = model.Tsacode;
                    model.Iuser = User.Identity.Name;
                    model.Idate = DateTime.Now;
                    _context.TblToolAssign.AddAsync(model);

                    //UpdateToolQuantity(toolCode,model);

                    _context.SaveChanges();
                    //modelList = _context.TblToolAssign.Where(s => s.Tsacode == vTsaCode).ToList();

                  
                    var tsalist = (from d in _context.TblTsasetup
                                   select new
                                   {
                                       Tsacode = d.Tsacode,
                                       Tsaname = d.Tsaname,
                                   }).ToList();
                    var toolList = (from c in _context.TblToolsSetup
                                    select new
                                    {
                                        ToolCode = c.ToolCode,
                                        ToolName = c.ToolName
                                    }).ToList();
                    var actionList = (from c in _context.TblAction
                                      select new
                                      {
                                          ActionCode = c.ActionCode,
                                          ActionName = c.ActionName
                                      }).ToList();
                    var actionTypeList = (from c in _context.TblActionType
                                          select new
                                          {
                                              ActionTypeCode = c.ActionTypeCode,
                                              ActionTypeName = c.ActionTypeName
                                          }).ToList();

                    tsalist.Insert(0, new { Tsacode = "0", Tsaname = "-- Select TSA --" });
                    toolList.Insert(0, new { ToolCode = "0", ToolName = "-- Select Tool --" });
                    actionList.Insert(0, new { ActionCode = "0", ActionName = "-- Select Action --" });
                    actionTypeList.Insert(0, new { ActionTypeCode = "0", ActionTypeName = "-- Select Action Type --" });

                    ViewBag.ListOfTSA = tsalist.ToList();
                    ViewBag.ToolList = toolList.ToList();
                    ViewBag.ListOfAction = actionList.ToList();
                    ViewBag.ListOfActionType = actionTypeList.ToList();

                    return View("Index", model);
                }
            
            }
            catch (Exception)
            {
            
            }
            return RedirectToAction(nameof(Index));
        }
        public Boolean UpdateToolQuantity(string vToolcode,TblToolAssign model)
        {
            try
            {
             
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        public JsonResult GetTsaAssignedDetailInfo (string vTsaCode) {
            try
            {
                var sa = new JsonSerializerSettings();
                var Data = (
                            from c in _context.TblToolAssign
                            from t in _context.TblToolsSetup
                            from a in _context.TblAction
                            from at in _context.TblActionType
                            where c.ToolCode == t.ToolCode && c.ActionCode==a.ActionCode && at.ActionTypeCode==c.ActionTypeCode && c.Tsacode==vTsaCode
                            select new 
                            { 
                                Id=c.Id,
                                ToolCode=t.ToolCode,
                                ToolName=t.ToolName,
                                Unit=t.Unit,
                                Action=a.ActionName,
                                ActionType=at.ActionTypeName,
                                Quantity=c.Quantity,
                                ActionDate=c.ActionDate,
                                Description=c.Description
                            }
                            ).ToList();
                //TsaAssignedInfoViewModel model = new TsaAssignedInfoViewModel();
                StringBuilder tableHtml = new StringBuilder();
                foreach(var item in Data) 
                {
                    tableHtml.Append("<tr>");
                    tableHtml.Append("<td>" + item.ToolCode + "</td>");
                    tableHtml.Append("<td>" + item.ToolName + "</td>");
                    tableHtml.Append("<td>" + item.Action + "</td>");
                    tableHtml.Append("<td>" + item.ActionType + "</td>");
                    tableHtml.Append("<td>" + item.Quantity + "</td>");
                    tableHtml.Append("<td>" + item.Unit + "</td>");
                    tableHtml.Append("<td>" + item.ActionDate.ToString("dd-MMM-yyyy") + "</td>");
                    tableHtml.Append("<td>" + item.Description + "</td>");
                    tableHtml.Append("<td><input id='addRow' type='button' class='btn btn-sm btn-danger' value='X' onclick='Delete(" + item.Id + ");' /></td>");
                    tableHtml.Append("</tr>");
                }
                
                var model = from c in _context.TblTsasetup
                            from r in _context.TblRegion
                            from a in _context.TblArea
                            where a.AreaCode==c.AreaCode && c.RegionCode==r.RegionCode  && c.Tsacode == vTsaCode
                            select new TsaAssignedInfoViewModel
                            { 
                             Tsacode = c.Tsacode,
                             AreaName= a.AreaName,
                             RegionName= r.RegionName,
                             Designation=c.Designation,
                             htmlBuilder = tableHtml.ToString(),
                             assignedTool = _context.TblToolAssign.Where(s=> s.Tsacode == vTsaCode).ToList(),
                            };

                return new JsonResult(model);
            }
            catch (Exception ex)
            {

                throw;
            }
            
            
        }
        public bool Delete(int vId)
        {
            try
            {
                TblToolAssign model = _context.TblToolAssign.Where(s => s.Id == vId).First();
                if (model != null)
                {
                    _context.TblToolAssign.Remove(model);
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
        public JsonResult GetToolInfo (string vToolCode) 
        {
            try
            {
                var sa = new JsonSerializerSettings();
                var model = _context.TblToolsSetup.Where(s => s.ToolCode == vToolCode).ToList();
                return new JsonResult(model);
            }
            catch (Exception ex)
            {
                throw;
               
            }
            
        }
        #region Private classes
        private string GenerateCode()
        {

            var yearMonth = DateTime.Now.ToString("yyyyMM");
            var result = _context.TblToolAssign.OrderBy(x => x.Id).Select(x => x.Tsacode).LastOrDefault();
            var lastGrn = string.IsNullOrEmpty(result) ? "00000000000000" : result;


            var last5digits = "1";
            if (lastGrn.Length > 5)
            {
                last5digits = lastGrn.Substring(lastGrn.Length - 5);
            }

            int lastNumber = Int32.Parse(last5digits) + 1;
            string lastNumberString = lastNumber.ToString("D5");
            var generatedCode = $"TA-{yearMonth}{lastNumberString}";
            return generatedCode;
        }
        #endregion

    }
}
