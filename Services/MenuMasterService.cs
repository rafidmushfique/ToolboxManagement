using LILI_TTS.Data;
using LILI_TTS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LILI_TTS.Services
{
    public class MenuMasterService:IMenuMasterService
    {
        private readonly dbToolsManagementContext _dbContext;
        //dbEFTestContext _dbContext = new dbEFTestContext();

        public MenuMasterService(dbToolsManagementContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<MenuMaster> GetMenuMaster()
        {
            return _dbContext.MenuMaster.AsEnumerable();
        }

        public IEnumerable<MenuMaster> GetMenuMaster(string UserRole)
        {
            //var result = _dbContext.MenuMaster.Where(m => m.UserRoll == UserRole).ToList();
            var result = _dbContext.MenuMaster.Where(m => m.UserRoll == UserRole).OrderBy(m=>m.MenuIdentity).ToList();
            return result;
        }
    }
}
