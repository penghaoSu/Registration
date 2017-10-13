using AutoMapper;
using Registration.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Registration.Service
{
    public class ServiceBase
    {
        public IMapper _mapper;
        public RegistrationContext _context;

        public ServiceBase(IMapper mapper, RegistrationContext context)
        {
            _context = context;
            _mapper = mapper;
        }
    }
}
