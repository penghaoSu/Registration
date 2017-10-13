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

                    row = new Row();
                    row.Append(
                        new Cell()
                        {
                            CellValue = new CellValue(item.Industry.ToString()),
                            DataType = CellValues.Number
                        },
                        
                        new Cell()
                        {
                            CellValue = new CellValue(CheckNull(item.Number.ToString())),
                            DataType = null
                        },
                        new Cell()
                        {
                            CellValue = new CellValue(item.Attribute.ToString()),
                            DataType = null
                        }
                        //new Cell()
                        //{
                        //    CellValue = new CellValue(CheckNull(item.Number.ToString())),
                        //    DataType = CellValues.String
                        //},
                        //new Cell()
                        //{
                        //    CellValue = new CellValue(CheckNull(item.Name.ToString())),
                        //    DataType = CellValues.String
                        //}
                    );
                    sheetData.AppendChild(row);
                }

                //foreach (var item in data)
                //{
                //    row = new Row();
                //    row.Append(
                //        new Cell()
                //        {
                //            CellValue = new CellValue(item.Industry.ToString()),
                //            DataType = CellValues.String
                //        },
                //        new Cell()
                //        {
                //            CellValue = new CellValue(item.Area.ToString()),
                //            DataType = CellValues.String
                //        },
                //        new Cell()
                //        {
                //            CellValue = new CellValue(item.Attribute.ToString()),
                //            DataType = CellValues.String
                //        },
                //        new Cell()
                //        {
                //            CellValue = new CellValue(item.Number.ToString()),
                //            DataType = CellValues.String
                //        },
                //        new Cell()
                //        {
                //            CellValue = new CellValue(item.Name.ToString()),
                //            DataType = CellValues.String
                //        },
                //        new Cell()
                //        {
                //            CellValue = new CellValue(item.Contact.ToString()),
                //            DataType = CellValues.String
                //        },
                //        new Cell()
                //        {
                //            CellValue = new CellValue(item.Phone.ToString()),
                //            DataType = CellValues.String
                //        },
                //        new Cell()
                //        {
                //            CellValue = new CellValue(item.Address.ToString()),
                //            DataType = CellValues.String
                //        }
                //    );
                //    sheetData.AppendChild(row);
                //}
            }
            memoryStream.Seek(0, SeekOrigin.Begin);
            return new FileStreamResult(memoryStream,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }


        public string CheckNull(string data)
        {
            if (data == null)
            {
                return "test";
            }
            else
            {
                return "test2";
            }

        }
    }
}