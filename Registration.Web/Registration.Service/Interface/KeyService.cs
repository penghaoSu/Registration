using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Registration.Service.Models;
using System.Threading.Tasks;
using Registration.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Registration.Service.Interface
{
    public class KeyService : ServiceBase, IKeyService
    {
        public KeyService(IMapper mapper, RegistrationContext context) : base(mapper, context)
        { }

        #region 註銷授權碼 -> 另外新增一組
        public async Task<Response> CancelAuthorize(AuthorizeModel model)
        {
            var result = new Response();

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                // 檢查訂單資料是否存在
                var order = await _context.Order.SingleOrDefaultAsync(o =>
                //o.Version == model.Version &&
                //o.SerialNumber == model.SerialNumber &&
                o.PurchaseDate == model.PurchaseDate);

                if (order != null)
                {
                    // 讓舊的產品金鑰失效
                    var oldKey = await _context.ProductKey.SingleOrDefaultAsync(p => p.OrderDetailId == order.Id && p.Status == 1);
                    oldKey.Status = 2;
                    _context.Update(oldKey);

                    // 新增產品金鑰
                    var productKey = new ProductKey()
                    {
                        //OrderId = oldKey.OrderId,
                        //Key = Guid.NewGuid().ToString(),
                        Date = DateTime.Now,
                        Status = 1
                    };
                    _context.ProductKey.Add(productKey);
                    await _context.SaveChangesAsync();

                    transaction.Commit();

                    // 回傳新的產品金鑰
                    result.Message = "success";
                    //result.ProductKey = productKey.Key;

                    return result;
                }

                result.Message = "無資料";

                return result;
            }

        }
        #endregion

        #region 取得授權碼
        public async Task<Response> GetAuthorize(AuthorizeModel model)
        {
            var result = new Response();

            // 檢查訂單資料是否存在
            var order = await _context.Order.AsNoTracking().SingleOrDefaultAsync(o =>
                //o.Version == model.Version &&
                //o.SerialNumber == model.SerialNumber &&
                o.PurchaseDate == model.PurchaseDate);

            if (order != null)
            {
                // 取得產品金鑰
                var key = await _context.ProductKey.AsNoTracking().SingleOrDefaultAsync(p => p.OrderDetailId == order.Id && p.Status == 1);

                result.Message = "success";

                //result.ProductKey = key.Key; ;

                return result;
            }

            result.Message = "無資料";

            return result;
        }
        #endregion

    }
}
