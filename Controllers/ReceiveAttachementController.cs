using TMS.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace TMS.Controllers
{
    [Authorize]
    public class ReceiveAttachementController : Controller
    {
        private readonly dbToolsManagementContext _context;

        public ReceiveAttachementController(dbToolsManagementContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            TblTsatoolReceiveAttachment model = new TblTsatoolReceiveAttachment();
            model.AttachmentCode = GenerateCode();
            var tsalist = (from d in _context.TblTsasetup
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


            var actionList = (from c in _context.TblAction
                              select new
                              {
                                  ActionCode = c.ActionCode,
                                  ActionName = c.ActionName
                              }).ToList();
            var actionTypeList = (from c in _context.TblAttachmentType
                                  select new
                                  {
                                      ActionTypeCode = c.AttachmentTypeCode,
                                      ActionTypeName = c.AttachmentTypeName
                                  }).ToList();

            tsalist.Insert(0, new { Tsacode = "0", Tsaname = "-- Select TSA --" });
            toolList.Insert(0, new { ToolCode = "0", ToolName = "-- Select Tool --" });
            actionList.Insert(0, new { ActionCode = "0", ActionName = "-- Select Action --" });
            actionTypeList.Insert(0, new { ActionTypeCode = "0", ActionTypeName = "-- Select Action Type --" });

            ViewBag.ListOfTSA = tsalist.ToList();
            ViewBag.ToolList = toolList.ToList();
            ViewBag.ListOfAction = actionList.ToList();
            ViewBag.ListOfActionType = actionTypeList.ToList();
            return View(model);
        }
        public async Task<IActionResult> CreateReceiveAttachment(TblTsatoolReceiveAttachment model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (DoesCodeExists(model.AttachmentCode))
                    {
                        model.AttachmentCode = GenerateCode();
                    }
                    var (originalFileName, filename, fileLocation, extension) = await UploadFileAndReturnFileName(model.Attachment);
                 
                    model.OriginalFileName=originalFileName;
                    model.FileName=filename;
                    model.Location=fileLocation;
                    model.DocumentType = extension;

                    model.Iuser = User.Identity.Name;
                    model.Idate = DateTime.Now;

                    _context.TblTsatoolReceiveAttachment.AddAsync(model);
                    _context.SaveChanges();

                    if (DoesCodeExists(model.AttachmentCode))
                    {
                        model.AttachmentCode = GenerateCode();
                    }
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
                    var actionTypeList = (from c in _context.TblAttachmentType
                                          select new
                                          {
                                              ActionTypeCode = c.AttachmentTypeCode,
                                              ActionTypeName = c.AttachmentTypeName
                                          }).ToList();

                    tsalist.Insert(0, new { Tsacode = "0", Tsaname = "-- Select TSA --" });
                    toolList.Insert(0, new { ToolCode = "0", ToolName = "-- Select Tool --" });
                    actionList.Insert(0, new { ActionCode = "0", ActionName = "-- Select Action --" });
                    actionTypeList.Insert(0, new { ActionTypeCode = "0", ActionTypeName = "-- Select Action Type --" });

                    ViewBag.ListOfTSA = tsalist.ToList();
                    ViewBag.ToolList = toolList.ToList();
                    ViewBag.ListOfAction = actionList.ToList();
                    ViewBag.ListOfActionType = actionTypeList.ToList();
                    return View("Index",model);
                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        private async Task<(string, string, string, string)> UploadFileAndReturnFileName(IFormFile file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            try
            {

                string uniqueFileName = GenerateUniqueFileName(file.FileName);
                string extension = Path.GetExtension(file.FileName);

                var newFileName = String.Concat(uniqueFileName);

                string filePath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload")).Root + $@"{newFileName}";
                //string filePath = Path.Combine(_hostEnvironment.WebRootPath,"Upload"); ;
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }


                //FtpWebRequest request = (FtpWebRequest)WebRequest.Create(filePath);
                //request.Method = WebRequestMethods.Ftp.UploadFile;


                return (file.FileName, uniqueFileName, filePath, extension);
            }

            catch (WebException e)
            {
                string status = ((FtpWebResponse)e.Response).StatusDescription;
                throw new Exception(status);
            }
        }
        private static string GenerateUniqueFileName(string fileName)
        {
            var unixTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            fileName = Path.GetFileName(fileName);
            return $"{unixTimestamp}"
                   + "_"
                   + Guid.NewGuid()
                   + Path.GetExtension(fileName);
        }
        public bool Delete(int vId)
        {
            try
            {
                TblTsatoolReceiveAttachment model = _context.TblTsatoolReceiveAttachment.Where(s => s.Id == vId).First();
                if (model != null)
                {
                    _context.TblTsatoolReceiveAttachment.Remove(model);
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

        public JsonResult GetTsaAssignedDetailInfo(string vTsaCode)
        {
            try
            {
                var sa = new JsonSerializerSettings();
                var Data = (
                            from c in _context.TblTsatoolReceiveAttachment
                            from a in _context.TblAttachmentType
                            from t in _context.TblTsasetup
                            where c.Tsacode == t.Tsacode && c.AttachmentTypeCode==a.AttachmentTypeCode && c.Tsacode== vTsaCode
                            select new
                            {
                              Id=c.Id,
                              AttachmentCode= c.AttachmentCode,
                              AttachmenDate=c.AttachmentDate,
                              AttachmentType=a.AttachmentTypeName,
                              FileName= c.FileName,
                              Location =c.Location,
                              OriginalFileName= c.OriginalFileName,
                              Description=c.Description,
                            }
                            ).ToList();
                //TsaAssignedInfoViewModel model = new TsaAssignedInfoViewModel();
                StringBuilder tableHtml = new StringBuilder();
                foreach (var item in Data)
                {
                    tableHtml.Append("<tr>");
                    tableHtml.Append("<td>" + item.AttachmentCode + "</td>");
                    tableHtml.Append("<td>" + item.AttachmenDate.ToString("dd-MMM-yyyy") + "</td>");
                    tableHtml.Append("<td>" + item.AttachmentType + "</td>");
                    tableHtml.Append("<td> <a href='/Upload/"+ item.FileName +"';download='"+item.OriginalFileName+"'>"+item.OriginalFileName +"</a></td>");
                    tableHtml.Append("<td>" + item.Description + "</td>");
                    tableHtml.Append("<td><input id='addRow' type='button' class='btn btn-sm btn-danger' value='X' onclick='Delete(" + item.Id + ");' /></td>");
                    tableHtml.Append("</tr>");
                }

                var model = from c in _context.TblTsasetup
                            from r in _context.TblRegion
                            from a in _context.TblArea
                            where a.AreaCode == c.AreaCode && c.RegionCode == r.RegionCode && c.Tsacode == vTsaCode
                            select new TsaAssignedInfoViewModel
                            {
                                Tsacode = c.Tsacode,
                                AreaName = a.AreaName,
                                RegionName = r.RegionName,
                                Designation = c.Designation,
                                htmlBuilder = tableHtml.ToString(),
                                assignedTool = _context.TblToolAssign.Where(s => s.Tsacode == vTsaCode).ToList(),
                            };

                return new JsonResult(model);
            }
            catch (Exception ex)
            {

                throw;
            }


        }
        private string GenerateCode()
        {
            var yearMonth = DateTime.Now.ToString("yyyyMM");
            var result = _context.TblTsatoolReceiveAttachment.OrderBy(x => x.Id).Select(x => x.AttachmentCode).LastOrDefault();


            var lastGrn = string.IsNullOrEmpty(result) ? "00000000000000" : result;


            var last5digits = "1";
            if (lastGrn.Length > 5)
            {
                last5digits = lastGrn.Substring(lastGrn.Length - 5);
            }

            int lastNumber = Int32.Parse(last5digits) + 1;
            string lastNumberString = lastNumber.ToString("D5");
            var generatedCode = $"ATT-{yearMonth}{lastNumberString}";
            return generatedCode;
        }

        private bool DoesCodeExists(string vAttCode)
        {
             return _context.TblTsatoolReceiveAttachment.Any(e => e.AttachmentCode == vAttCode);
        }
        //[HttpPost]
        //public ActionResult DeleteAttachmentFile(int Id)
        //{
        //    var attachments = _context.TblExpenseBillAttachements.Where(x => x.Id == Id);
        //    eFTestContext.TblExpenseBillAttachements.RemoveRange(attachments);
        //    eFTestContext.SaveChangesAsync();
        //    return Ok();
        //}


    }



}
