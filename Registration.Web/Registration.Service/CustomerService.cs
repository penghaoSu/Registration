using Registration.Service.Interface;
using System;
using System.Collections.Generic;
using Registration.Data;
using Registration.Service.Models;
using System.Threading.Tasks;
using AutoMapper;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Sakura.AspNetCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Registration.Service.Models.ViewModels;

namespace Registration.Service
{
    public class CustomerService : ServiceBase, ICustomerService
    {
        private int pageSize = 10;

        public CustomerService(IMapper mapper, RegistrationContext context) : base(mapper, context)
        { }

        #region 新增
        public async Task CreateAsync(CustomerCreateViewModel model)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                // 新增客戶資料
                var customer = _mapper.Map<CustomerDto, Customer>(model.CustomerNew);
                customer.IsDelete = false;
                customer.CreateDate = DateTime.Now;
                _context.Customer.Add(customer);

                // 新增訂單資料
                var orders = new List<Order>();
                foreach (var item in model.SerialNumber)
                {
                    var order = _mapper.Map<OrderDto, Order>(model.OrderDto);

                    order.CustomerId = customer.Id;
                    order.OrderType = 1;
                    order.Module = 0;
                    order.Status = 2;
                    order.SerialNumber = item.SN;
                    order.CreateDate = DateTime.Now;
                    orders.Add(order);
                }
                _context.Order.AddRange(orders);

                // 新增產品金鑰
                var productKey = new List<ProductKey>();
                foreach (var item in orders)
                {
                    productKey.Add(new ProductKey()
                    {
                        OrderId = item.Id,
                        Key = Guid.NewGuid().ToString(),
                        Date = DateTime.Now
                    });
                }
                _context.ProductKey.AddRange(productKey);
                await _context.SaveChangesAsync();

                transaction.Commit();
            }
        }
        #endregion

        #region 新增擴充模組
        public async Task CreateModuleAsync(CustomerCreateViewModel model)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                // 新增訂單資料
                var orders = new List<Order>();

                foreach (var item in model.SerialNumber)
                {
                    var order = _mapper.Map<OrderDto, Order>(model.OrderDto);

                    order.CustomerId = model.CustomerNew.Id;
                    order.OrderType = 2;
                    order.Module = model.OrderDto.Module;
                    order.Status = 2;
                    order.SerialNumber = item.SN;
                    order.CreateDate = DateTime.Now;
                    orders.Add(order);
                }

                _context.Order.AddRange(orders);

                // 新增產品金鑰
                var productKey = new List<ProductKey>();

                foreach (var item in orders)
                {
                    productKey.Add(new ProductKey()
                    {
                        OrderId = item.Id,
                        Key = Guid.NewGuid().ToString(),
                        Date = DateTime.Now
                    });
                }
                _context.ProductKey.AddRange(productKey);

                await _context.SaveChangesAsync();

                transaction.Commit();
            }
        }
        #endregion

        #region 刪除
        public async Task<bool> DeleteAsync(int? id)
        {
            var data = await GetByIdAsync(id);

            if (data == null)
                return false;

            _context.Customer.Remove(data);
            await _context.SaveChangesAsync();

            return true;
        }
        #endregion

        #region 編輯
        public async Task EditAsync(int id, CustomerDto dto)
        {
            var model = _mapper.Map<CustomerDto, Customer>(dto);

            _context.Update(model);

            await _context.SaveChangesAsync();

        }
        #endregion

        #region 取得列表
        public async Task<IEnumerable<CustomerModel>> GetAllAsync(CustomerSearch param, int page)
        {
            int pageNumber = page < 1 ? 1 : page;

            var query = _context.Customer.AsQueryable();

            if (param != null && param.Keyword != null)
            {
                if (param.Name == "1")
                {
                    query = query.Where(p => p.Number.Contains(param.Keyword));
                }
                else if (param.Name == "2")
                {
                    query = query.Where(p => p.Name.Contains(param.Keyword));
                }
                else
                {
                    query = query.Where(p => p.Area.ToString().Contains(param.Keyword));
                }
            }

            var data = query.Select(c => new CustomerModel
            {
                Id = c.Id,
                Industry = c.Industry,
                Area = c.Area,
                Attribute = c.Attribute,
                Name = c.Name,
                Number = c.Number,
                Order = c.Order.Select(o => new OrderDto
                {
                    Id = o.Id,
                    PurchaseDate = o.PurchaseDate,
                    Deliveryperson = o.Deliveryperson,
                    Salesperson = o.Salesperson,
                    IsReceipt = o.IsReceipt,
                    Version = o.Version,
                    IsAutoUpdate = o.IsAutoUpdate,
                    Status = o.Status

                }).ToList()

            }).OrderByDescending(o => o.Id).AsNoTracking().ToPagedList(pageSize, pageNumber);

            return data;
        }
        #endregion

        #region 取得使用者列表
        public async Task<IEnumerable<UserModel>> GetAllUserAsync()
        {
            var result = await _context.AspNetUsers.Select(u => new UserModel
            {
                Id = u.Id,
                Name = u.Name,
                UserName = u.UserName,
                IsLock = u.IsLock,
                IsDelete = u.IsDelete

            }).Where(u => !u.IsDelete).OrderBy(u => u.Id).AsNoTracking().ToListAsync();

            return result;
        }
        #endregion

        #region 取單一筆
        public async Task<Customer> GetByIdAsync(int? id)
        {
            var data = await _context.Customer.SingleOrDefaultAsync(m => m.Id == id);

            return data;
        }
        #endregion

        #region 取得縣市
        public async Task<IEnumerable<SelectListItem>> GetCityAsync()
        {
            var items = new List<SelectListItem>();

            var city = await _context.City.AsNoTracking().ToListAsync();

            foreach (var c in city)
            {
                var item = new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                };
                items.Add(item);
            }

            return items;
        }
        #endregion

        #region 取得地區
        public async Task<IEnumerable<SelectListItem>> GetCityAreaAsync(int cityId)
        {
            var items = new List<SelectListItem>();

            var area = await _context.CityArea.Where(ca => ca.CityId == cityId).AsNoTracking().ToListAsync();

            foreach (var a in area)
            {
                var item = new SelectListItem()
                {
                    Text = a.Name,
                    Value = a.Id.ToString(),
                };
                items.Add(item);
            }

            return items;
        }
        #endregion

        #region 取得客戶資料詳細內容
        public async Task<CustomerDto> GetCustomerByIdAsync(int id)
        {
            var data = await GetByIdAsync(id);

            var model = _mapper.Map<Customer, CustomerDto>(data);

            return model;
        }
        #endregion

        public async Task<CustomerOrderViewModel> GetOrderByIdAsync(int id)
        {
            var result = new CustomerOrderViewModel
            {
                Order = await _context.Order.Select(o => new Order
                {

                    Id = o.Id,
                    CustomerId = o.CustomerId,
                    OrderType = o.OrderType,
                    Module = o.Module,
                    Number = o.Module,
                    PurchaseDate = o.PurchaseDate,
                    Salesperson = o.Salesperson,
                    Deliveryperson = o.Deliveryperson,
                    IsReceipt = o.IsReceipt,
                    AuthorizeType = o.AuthorizeType,
                    Version = o.Version,
                    Revision = o.Revision,
                    Warranty = o.Warranty,
                    WarrantyPeriodStr = o.WarrantyPeriodStr,
                    WarrantyPeriodEnd = o.WarrantyPeriodEnd,
                    Lease = o.Module,
                    LeaseDateEnd = o.LeaseDateEnd,
                    LeaseDateStr = o.LeaseDateStr,
                    SerialNumber = o.SerialNumber,
                    StartDate = o.StartDate,
                    IsAutoUpdate = o.IsAutoUpdate,
                    Status = o.Status,
                    CreateDate = o.CreateDate,
                    //Customer = o.Customer
                    //ProductKey = o.ProductKey.Select(p => new ProductKey
                    //{
                    //    Id = p.Id,
                    //    Key = p.Key,
                    //    Date = p.Date

                    //}).ToList()

                }).SingleOrDefaultAsync(o => o.Id == id),

            };

            result.Customer = await _context.Customer.SingleOrDefaultAsync(c => c.Id == result.Order.CustomerId);
            result.City = await GetCityAsync();
            result.CityArea = await GetCityAreaAsync(result.Customer.CityId.Value);

            return result;
        }

        #region 取得客戶所有訂單資料
        public async Task<Customer> GetAllOrderAsync(int cid)
        {
            //var data = await _context.Customer.Select(c => new CustomerModel
            //{
            //    Id = c.Id,
            //    Industry = c.Industry,
            //    Area = c.Area,
            //    Attribute = c.Attribute,
            //    Name = c.Name,
            //    Number = c.Number,
            //    Order = c.Order.Select(o => new OrderDto
            //    {
            //        Id = o.Id,
            //        CustomerId = o.CustomerId,
            //        PurchaseDate = o.PurchaseDate,
            //        Deliveryperson = o.Deliveryperson,
            //        Salesperson = o.Salesperson,
            //        IsReceipt = o.IsReceipt,
            //        Version = o.Version,
            //        IsAutoUpdate = o.IsAutoUpdate,
            //        Status = o.Status,
            //        ProductKey = o.ProductKey.Select(p => new ProductKey
            //        {

            //            Key = p.Key,
            //            Date = p.Date

            //        }).ToList()

            //    }).ToList()

            //}).AsNoTracking().SingleOrDefaultAsync(c => c.Id == cid);

            var result = await _context.Customer
                .Include(o => o.Order)
                .ThenInclude(p => p.ProductKey).AsNoTracking().SingleOrDefaultAsync(o => o.Id == cid);

            return result;
        }
        #endregion

        #region 取得所有訂單資料並作匯出
        public async Task<IEnumerable<Customer>> GetOrderExcelAsync()
        {
            var data = await _context.Customer
                .Include(o => o.Order)
                .ThenInclude(p => p.ProductKey).AsNoTracking().ToListAsync();

            return data;
        }
        #endregion

        #region 取得縣市名稱
        public async Task<string> GetCityNameAsync(int id)
        {
            var result = await _context.City.SingleOrDefaultAsync(c => c.Id == id);

            return result.Name;
        }
        #endregion

        #region 取得地區名稱
        public async Task<string> GetCityAreaNameAsync(int id)
        {
            var result = await _context.CityArea.SingleOrDefaultAsync(ca => ca.Id == id);

            return result.Name;
        }
        #endregion

    }
}
