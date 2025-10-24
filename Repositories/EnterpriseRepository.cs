using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDSCalculatorDesktop.Data;
using PDSCalculatorDesktop.Models;

namespace PDSCalculatorDesktop.Repositories
{
    public class EnterpriseRepository : Repository<Enterprise>
    {
        public EnterpriseRepository(ApplicationDbContext context) : base(context) { }
    }
}
