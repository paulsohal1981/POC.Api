using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POC.Data.Interface
{
    public interface ICityInfoRepository
    {
        IQueryable<string> GetCities();
    }
}
