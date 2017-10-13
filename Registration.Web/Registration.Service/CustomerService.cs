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

namespace Registration.Service
{
    public class CustomerService : ServiceBase, ICustomerService
    {
        private int pageSize = 8;

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
                    order.Status = 2;
                    order.SerialNumber = item.SN;
                    order.CreateDate = DateTime.Now;
                    orders.Add(order);
                }
                _context.Order.AddRange(orders);

                // 新增產品金鑰
                var productKey = new List<ProductKey>();
                for (int i = 0; i < model.OrderDto.Number; i++)
                {
                    productKey.Add(new ProductKey()
                    {
                        OrderId = customer.Id,
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
                    order.Status = 2;
                    order.SerialNumber = item.SN;
                    order.CreateDate = DateTime.Now;
                    orders.Add(order);
                }

                _context.Order.AddRange(orders);

                // 新增產品金鑰
                var productKey = new List<ProductKey>();

                for (int i = 0; i < model.OrderDto.Number; i++)
                {
                    productKey.Add(new ProductKey()
                    {
                        OrderId = model.CustomerNew.Id,
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

            }).OrderByDescending(c => c.Id).AsNoTracking().ToPagedList(pageSize, pageNumber);

            return data;
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

        #region 
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
            //Order = c.Order.Select(o => new OrderDto
            //{
            //Id = o.Id,
            //CustomerId = o.CustomerId,
            //PurchaseDate = o.PurchaseDate,
            //Deliveryperson = o.Deliveryperson,
            //Salesperson = o.Salesperson,
            //IsReceipt = o.IsReceipt,
            //Version = o.Version,
            //IsAutoUpdate = o.IsAutoUpdate,
            //Status = o.Status,
            //ProductKey = o.ProductKey.Select(p => new ProductKey
            //{

            //    Key = p.Key,
            //    Date = p.Date

            //}).ToList()

            //}).ToList()

            //}).AsNoTracking().SingleOrDefaultAsync(c=>c.Id == cid);

            var data = await _context.Customer
                .Include(o => o.Order)
                .ThenInclude(p => p.ProductKey).SingleOrDefaultAsync(o => o.Id == cid);

            return data;
        }
        #endregion

    }
}
