using Registration.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Registration.Service.Interface
{
    public interface ILogFileService
    {
        /// <summary>
        /// 紀錄動作
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task LogInformation(string msg , string userId);
    }
}
