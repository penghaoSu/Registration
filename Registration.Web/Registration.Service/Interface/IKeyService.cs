using Registration.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Registration.Service.Interface
{
    public interface IKeyService
    {
        /// <summary>
        /// 取得授權碼
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Response> GetAuthorize(AuthorizeModel model);

        /// <summary>
        /// 註銷授權碼 -> 另新增授權碼
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<Response> CancelAuthorize(AuthorizeModel model);
    }
}
