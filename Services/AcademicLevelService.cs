using alumni.Data;
using alumni.Domain;
using alumni.IServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.Services
{
    public class AcademicLevelService : IAcademicLevelService
    {
        private readonly DataContext dataContext;

        public AcademicLevelService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<AcademicLevel> GetAsync(string id)
        {
            return await dataContext.AcademicLevels.SingleOrDefaultAsync(al => al.Id == id);
        }
    }
}
