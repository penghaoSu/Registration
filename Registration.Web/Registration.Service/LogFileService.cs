using AutoMapper;
using Registration.Data;
using Registration.Service.Interface;
using Registration.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Registration.Service
{
    public class LogFileService : ServiceBase, ILogFileService
    {
        public LogFileService(IMapper mapper, RegistrationContext context) : base(mapper, context)
        { }

        public async Task LogInformation(string msg , string userId)
        {
            var model = new LogFile()
            {
                Date = DateTime.Now,
                Item = msg,
                UserId = userId
            };

            _context.LogFile.Add(model);

            await _context.SaveChangesAsync();
        }
    }
}
