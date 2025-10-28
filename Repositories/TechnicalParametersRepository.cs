using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PDSCalculatorDesktop.Data;
using PDSCalculatorDesktop.Models;
using PDSCalculatorDesktop.Repositories.Interfaces;

namespace PDSCalculatorDesktop.Repositories
{
    public class TechnicalParametersRepository : Repository<TechnicalParameters>, ITechnicalParametersRepository
    {
        public TechnicalParametersRepository(ApplicationDbContext contex) : base(contex) { }
    }
}
