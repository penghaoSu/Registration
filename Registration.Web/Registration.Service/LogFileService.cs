using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Registration.Data;
using Registration.Service.Interface;
using Registration.Service.Models;
using Sakura.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Registration.Service
{
    public class LogFileService : ServiceBase, ILogFileService
    {
        private int pageSize = 10;

        public LogFileService(IMapper mapper, RegistrationContext context) : base(mapper, context)
        { }

        public async Task<IEnumerable<LogModel>> GetAllLogAsync(int page)
        {
            int pageNumber = page < 1 ? 1 : page;

            var result = await _context.LogFile.AsNoTracking().ToPagedListAsync(pageSize, pageNumber);

            var xxx = await _context.LogFile.Select(l=>new LogModel {
                Date = l.Date,
                Item = l.Item,
                UserName = l.User.Name

            }).AsNoTracking().ToPagedListAsync(pageSize, pageNumber);

            return xxx;
        }

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
