using Microsoft.AspNetCore.Mvc.Rendering;
using Registration.Data;
using Registration.Service.Models;
using Sakura.AspNetCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Registration.Service.Interface
{
    public interface ICustomerService
    {
        /// <summary>
        /// 取得單一筆資料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Customer> GetByIdAsync(int? id);

        /// <summary>
        /// 取得所有客戶(包括訂單)列表
        /// </summary>
        /// <param name="param"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        Task<IEnumerable<OrderDto>> GetAllCustomerAsync(CustomerSearch param, int page);

        /// <summary>
        /// 取得使用者列表
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<UserModel>> GetAllUserAsync();

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task CreateOrderAsync(CustomerCreateViewModel model);

        /// <summary>
        /// 新增擴充模組
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task CreateModuleAsync(CustomerCreateViewModel model);

        /// <summary>
        /// 編輯
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task EditAsync(int id, Order model);

        /// <summary>
        /// 刪除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(int? id);

        /// <summary>
        /// 取得客戶資料詳細內容
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<CustomerDto> GetCustomerByIdAsync(int id);

        /// <summary>
        /// 取得客戶所有訂單資料
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        Task<Customer> GetAllOrderByAsync(int cid);

        /// <summary>
        /// 取得客戶單筆訂單資料
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        Task<CustomerOrderViewModel> GetOrderByCustomerIdAsync(int cid);

        /// <summary>
        /// 取得所有訂單資料並作匯出
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Customer>> GetOrderExcelAsync();

        /// <summary>
        /// 取得訂單的產品金鑰
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<ProductKey>> GetProductKeyByOrderId(int id);


        /// <summary>
        /// 重製產品金鑰
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task ResetProductKey(int id);

        /// <summary>
        /// 取得縣市
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SelectListItem>> GetCityAsync();

        /// <summary>
        /// 取得地區
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SelectListItem>> GetCityAreaAsync(int cityId);

        /// <summary>
        /// 取得縣市名稱
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<string> GetCityNameAsync(int id);

        /// <summary>
        /// 取得地區名稱
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<string> GetCityAreaNameAsync(int id);




    }
}
