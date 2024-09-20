using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TitanTechTask.Data.SeedData
{
    public interface IDataSeeder
    {
        void Seed(ApplicationDbContext _context);
    }
}
