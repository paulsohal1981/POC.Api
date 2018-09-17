using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace POC.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        #region private members

        protected IMapper _mapper;

        #endregion

        public BaseController(IMapper mapper)
        {
            _mapper = mapper;
        }
    }
}
