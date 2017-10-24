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
        private int pageSize = 10;

        public CustomerService(IMapper mapper, RegistrationContext context) : base(mapper, context)
        { }

        #region 新增
        public async Task CreateOrderAsync(CustomerCreateViewModel model)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                // 新增客戶資料
                var customer = _mapper.Map<CustomerDto, Customer>(model.CustomerNew);
                customer.IsDelete = false;
                customer.CreateDate = DateTime.Now;
                _context.Customer.Add(customer);

                // 新增訂單資料
                var order = _mapper.Map<OrderDto, Order>(model.OrderDto);
                order.CustomerId = customer.Id;
                order.OrderType = 1;
                order.Module = 0;
                _context.Order.Add(order);

                // 新增訂單明細資料
                var orderDetail = new List<OrderDetail>();

                foreach (var item in model.OrderDetailDto)
                {
                    for (int i = 0; i < item.Quantity; i++)
                    {
                        var detail = _mapper.Map<OrderDetailDto, OrderDetail>(item);

                        detail.OrderId = order.Id;
                        detail.SerialNumber = CreateRandomCode();
                        detail.Status = 2;
                        detail.CreateDate = DateTime.Now;

                        orderDetail.Add(detail);
                    }
                }

                _context.OrderDetail.AddRange(orderDetail);


                // 新增產品金鑰
                var productKey = new List<ProductKey>();
                foreach (var item in orderDetail)
                {
                    productKey.Add(new ProductKey()
                    {
                        OrderDetailId = item.Id,
                        LicenceKey = Guid.NewGuid().ToString(),
                        Date = DateTime.Now,
                        Status = 1
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

                //foreach (var item in model.SerialNumber)
                //{
                //    var order = _mapper.Map<OrderDto, Order>(model.OrderDto);

                //    order.CustomerId = model.CustomerNew.Id;
                //    order.OrderType = 2;
                //    order.Module = model.OrderDto.Module;
                //    //order.Status = 2;
                //    //order.SerialNumber = item.SN;
                //    //order.CreateDate = DateTime.Now;
                //    //order.IsDelete = false;
                //    orders.Add(order);
                //}

                //_context.Order.AddRange(orders);

                // 新增產品金鑰
                var productKey = new List<ProductKey>();

                foreach (var item in orders)
                {
                    productKey.Add(new ProductKey()
                    {
                        //OrderId = item.Id,
                        //Key = Guid.NewGuid().ToString(),
                        Date = DateTime.Now,
                        Status = 1
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
        public async Task EditAsync(int id, Order dto)
        {
            //var model = _mapper.Map<CustomerDto, Customer>(dto);

            var model = await _context.Order.SingleOrDefaultAsync(o => o.Id == id);

            model.Module = dto.Module;
            //model.Number = dto.Number;
            model.PurchaseDate = dto.PurchaseDate;
            model.Salesperson = dto.Salesperson;
            //model.Deliveryperson = dto.Deliveryperson;
            //model.IsReceipt = dto.IsReceipt;
            //model.AuthorizeType = dto.AuthorizeType;
            //model.Version = dto.Version;
            //model.Revision = dto.Revision;
            //model.Warranty = dto.Warranty;
            //model.WarrantyPeriodStr = dto.WarrantyPeriodStr;
            //model.WarrantyPeriodEnd = dto.WarrantyPeriodEnd;
            //model.Lease = dto.Lease;
            //model.LeaseDateEnd = dto.LeaseDateEnd;
            //model.LeaseDateStr = dto.LeaseDateStr;
            //model.SerialNumber = dto.SerialNumber;
            //model.StartDate = dto.StartDate;
            //model.IsAutoUpdate = dto.IsAutoUpdate;
            //model.Remark = dto.Remark;


            _context.Update(model);

            await _context.SaveChangesAsync();

        }
        #endregion

        #region 取得所有客戶(包括訂單)列表
        public async Task<IEnumerable<OrderDto>> GetAllCustomerAsync(CustomerSearch param, int page)
        {
            int pageNumber = page < 1 ? 1 : page;

            var query = _context.Order.AsQueryable();

            if (param != null)
            {
                if (!string.IsNullOrWhiteSpace(param.Keyword))
                {
                    if (param.Selector == "1")
                    {
                        //query = query.Where(o => o.SerialNumber.Contains(param.Keyword));
                    }
                    else if (param.Selector == "2")
                    {
                        query = query.Where(o => o.Salesperson.Contains(param.Keyword));
                    }
                    else if (param.Selector == "3")
                    {
                        //query = query.Where(o => o.Deliveryperson.Contains(param.Keyword));
                    }
                }

                if (!string.IsNullOrWhiteSpace(param.StartDate.ToString()) && !string.IsNullOrWhiteSpace(param.EndDate.ToString()))
                {
                    if (param.Selector == "4")
                    {
                        query = query.Where(o => o.PurchaseDate >= param.StartDate.Date && o.PurchaseDate <= param.EndDate.Date);
                    }
                    else if (param.Selector == "5")
                    {
                        //query = query.Where(o => o.WarrantyPeriodStr >= param.StartDate.Date && o.WarrantyPeriodEnd <= param.EndDate.Date);
                    }
                }
            }

            var data = await query.Select(o => new OrderDto
            {
                Id = o.Id,
                CustomerId = o.CustomerId,
                OrderType = o.OrderType,
                Module = o.Module,
                PurchaseDate = o.PurchaseDate.Value,
                Salesperson = o.Salesperson,
                Customer = o.Customer

            }).OrderByDescending(o => o.Id).AsNoTracking().ToPagedListAsync(pageSize, pageNumber);


            //var query = _context.Customer.AsQueryable();

            //if (param != null && param.Keyword != null)
            //{
            //    if (param.Selector == "1")
            //    {
            //        query = query.Where(p => p.Number.Contains(param.Keyword));
            //    }
            //    else if (param.Selector == "2")
            //    {
            //        query = query.Where(p => p.Name.Contains(param.Keyword));
            //    }
            //    else
            //    {
            //        query = query.Where(p => p.Area.ToString().Contains(param.Keyword));
            //    }
            //}

            //var data = query.Select(c => new CustomerModel
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
            //        OrderType = o.OrderType,
            //        Module = o.Module,
            //        PurchaseDate = o.PurchaseDate,
            //        Deliveryperson = o.Deliveryperson,
            //        Salesperson = o.Salesperson,
            //        IsReceipt = o.IsReceipt,
            //        Version = o.Version,
            //        IsAutoUpdate = o.IsAutoUpdate,
            //        Status = o.Status

            //    }).ToList()

            //}).OrderByDescending(o => o.Id).AsNoTracking().ToPagedList(pageSize, pageNumber);

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

        public async Task<CustomerOrderViewModel> GetOrderByCustomerIdAsync(int id)
        {
            //var result = new CustomerOrderViewModel
            //{
            //    Order = await _context.Order.Select(o => new Order
            //    {

            //        Id = o.Id,
            //        CustomerId = o.CustomerId,
            //        OrderType = o.OrderType,
            //        Module = o.Module,
            //        //Number = o.Module,
            //        PurchaseDate = o.PurchaseDate,
            //        Salesperson = o.Salesperson,
            //        //Deliveryperson = o.Deliveryperson,
            //        //IsReceipt = o.IsReceipt,
            //        //AuthorizeType = o.AuthorizeType,
            //        //Version = o.Version,
            //        //Revision = o.Revision,
            //        //Warranty = o.Warranty,
            //        //WarrantyPeriodStr = o.WarrantyPeriodStr,
            //        //WarrantyPeriodEnd = o.WarrantyPeriodEnd,
            //        //Lease = o.Module,
            //        //LeaseDateEnd = o.LeaseDateEnd,
            //        //LeaseDateStr = o.LeaseDateStr,
            //        //SerialNumber = o.SerialNumber,
            //        //StartDate = o.StartDate,
            //        //IsAutoUpdate = o.IsAutoUpdate,
            //        //Status = o.Status,
            //        //CreateDate = o.CreateDate,
            //        //IsDelete = o.IsDelete,
            //        //Remark = o.Remark
            //        //Customer = o.Customer
            //        //ProductKey = o.ProductKey.Select(p => new ProductKey
            //        //{
            //        //    Id = p.Id,
            //        //    Key = p.Key,
            //        //    Date = p.Date

            //        //}).ToList()

            //    }).SingleOrDefaultAsync(o => o.Id == id),

            //};

            var result = new CustomerOrderViewModel();

            result.Customer = await _context.Customer.SingleOrDefaultAsync(c => c.Id == id);
            //result.Order = await _context.Order.Select(o => new Order
            //{
            //    Id = o.Id,
            //    CustomerId = o.CustomerId,
            //    OrderType = o.OrderType,
            //    Module = o.Module,
            //    PurchaseDate = o.PurchaseDate,
            //    Salesperson = o.Salesperson,
            //    IsReceipt = o.IsReceipt,
            //    OrderDetail = o.OrderDetail.Select(od => new OrderDetail
            //    {
            //        Deliveryperson = od.Deliveryperson,
            //        AuthorizeType = od.AuthorizeType,
            //        Version = od.Version,
            //        Revision = od.Revision,
            //        Warranty = od.Warranty,
            //        WarrantyPeriodStr = od.WarrantyPeriodStr,
            //        WarrantyPeriodEnd = od.WarrantyPeriodEnd,
            //    }).ToList()

            //}).AsNoTracking().Where(o => o.CustomerId == id).ToListAsync();

            result.Order = await _context.Order
               .Include(o => o.OrderDetail).Where(o => o.CustomerId == id).AsNoTracking().ToListAsync();
            //.ThenInclude(p => p.ProductKey)


            result.City = await GetCityAsync();
            result.CityArea = await GetCityAreaAsync(result.Customer.CityId.Value);



            return result;
        }

        #region 取得客戶所有訂單資料
        public async Task<Customer> GetAllOrderByAsync(int cid)
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
                //.ThenInclude(p => p.ProductKey)
                .AsNoTracking().SingleOrDefaultAsync(o => o.Id == cid);

            return result;
        }
        #endregion

        #region 取得所有訂單資料並作匯出
        public async Task<IEnumerable<Customer>> GetOrderExcelAsync()
        {
            var data = await _context.Customer
                .Include(o => o.Order)
                //.ThenInclude(p => p.ProductKey)
                .AsNoTracking().ToListAsync();

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

        #region 重製產品金鑰
        public async Task ResetProductKey(int id)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                // 讓舊的產品金鑰失效
                var result = await _context.ProductKey.SingleOrDefaultAsync(p => p.OrderDetailId == id && p.Status == 1);
                result.Status = 2;
                _context.Update(result);

                // 新增產品金鑰
                var productKey = new ProductKey()
                {
                    //OrderId = id,
                    //Key = Guid.NewGuid().ToString(),
                    Date = DateTime.Now,
                    Status = 1
                };
                _context.ProductKey.Add(productKey);
                await _context.SaveChangesAsync();

                transaction.Commit();
            }
        }
        #endregion

        #region 取得產品
        public async Task<IEnumerable<ProductKey>> GetProductKeyByOrderId(int id)
        {
            var result = await _context.ProductKey.Where(p => p.OrderDetailId == id).AsNoTracking().ToListAsync();

            return result;
        }
        #endregion

        public string CreateRandomCode()
        {
            string allChar = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z";
            string[] allCharArray = allChar.Split(',');
            string randomCode = "";
            int temp = -1;

            Random rand = new Random();
            for (int i = 0; i < 16; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
                }
                int t = rand.Next(36);
                if (temp != -1 && temp == t)
                {
                    return CreateRandomCode();
                }
                temp = t;
                randomCode += allCharArray[t];
            }
            return randomCode;
        }


    }
}
