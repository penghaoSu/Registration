using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using Registration.Service.Interface;
using Registration.Data.Enums;
using Registration.Data.Enums.Extension;

namespace Registration.Web.Controllers
{
    public class ExcelController : Controller
    {
        private ICustomerService _customerService;

        private static readonly string[][] _smapleData = new string[][]
        {
            new string[]{ "John Wu Blog","https://blog.johnwu.cc/" },
            new string[]{ "大內攻城粉絲團", "https://www.facebook.com/SoftwareENG.NET" }
        };

        public ExcelController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<FileStreamResult> Export()
        {
            var memoryStream = new MemoryStream();
            using (var document = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();
                var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());
                var sheets = workbookPart.Workbook.AppendChild(new Sheets());
                sheets.Append(new Sheet()
                {
                    Id = workbookPart.GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = "Sheet 1"
                });
                // 從 Worksheet 取得要編輯的 SheetData
                var sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();
                // 建立資料列物件
                var row = new Row();
                // 在資料列中插入欄位
                row.Append(
                    new Cell()
                    {
                        CellValue = new CellValue("行業別"),
                        DataType = CellValues.String
                    },
                    new Cell()
                    {
                        CellValue = new CellValue("區域"),
                        DataType = CellValues.String
                    },
                    new Cell()
                    {
                        CellValue = new CellValue("客戶屬性"),
                        DataType = CellValues.String
                    },
                    new Cell()
                    {
                        CellValue = new CellValue("客戶編號"),
                        DataType = CellValues.String
                    },
                    new Cell()
                    {
                        CellValue = new CellValue("客戶名稱"),
                        DataType = CellValues.String
                    },
                    new Cell()
                    {
                        CellValue = new CellValue("聯絡人"),
                        DataType = CellValues.String
                    },
                    new Cell()
                    {
                        CellValue = new CellValue("電話"),
                        DataType = CellValues.String
                    },
                    new Cell()
                    {
                        CellValue = new CellValue("地址"),
                        DataType = CellValues.String
                    }
                );
                // 插入資料列
                sheetData.AppendChild(row);

                // 取資料
                var data2 = await _customerService.GetOrderExcelAsync();

                foreach (var item in data2)
                {
                    // 行業別
                    var industry = (IndustryEnum)Enum.Parse(typeof(IndustryEnum), item.Industry.ToString());
                    // 區域
                    var area = (AreaEnum)Enum.Parse(typeof(AreaEnum), item.Area.ToString());
                    // 屬性
                    var attribu = (AttributeEnum)Enum.Parse(typeof(AttributeEnum), item.Attribute.ToString());
                    // 客戶編號
                    var number = CheckNull(item.Number);
                    // 客戶名稱
                    var name = item.Name;
                    // 聯絡人
                    var contact = item.Contact;
                    // 電話
                    var phone = CheckNull(item.Phone);
                    // 地址
                    var city = await _customerService.GetCityNameAsync(item.CityId.Value);
                    var cityArea = await _customerService.GetCityAreaNameAsync(item.CityAreaId.Value);
                    var address = city + cityArea + item.Address;

                    row = new Row();
                    row.Append(
                        new Cell()
                        {
                            CellValue = new CellValue(industry.Description()),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue(area.Description()),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue(attribu.Description()),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue(number),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue(name),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue(contact),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue(phone),
                            DataType = CellValues.String
                        },
                        new Cell()
                        {
                            CellValue = new CellValue(address),
                            DataType = CellValues.String
                        }
                    );
                    sheetData.AppendChild(row);
                }
            }
            memoryStream.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(memoryStream,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public string CheckNull(string data)
        {
            if (data == null)
            {
                return string.Empty;
            }
            else
            {
                return data;
            }

        }
    }
}