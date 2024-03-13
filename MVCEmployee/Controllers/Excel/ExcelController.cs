using Microsoft.AspNetCore.Mvc;
using MVCEmployee.Models;
using Npgsql;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Text;

namespace MVCEmployee.Controllers.Excel
{
    public class ExcelController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly AppDbContext context;

        public ExcelController(IConfiguration configuration, AppDbContext context)
        {
            this.configuration = configuration;
            this.context = context;
        }
        public IActionResult Index()
        {
            var data = context.Customers.ToList();
            return View(data);
        }
        public IActionResult ImportExcelFile()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ImportExcelFile(IFormFile formFile)
        {
            try
            {
                if(formFile.FileName !="" && formFile.Length > 0)
                {
                    var mainPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "UploadExcelFile");
                    if (!Directory.Exists(mainPath))
                    {
                        Directory.CreateDirectory(mainPath);
                    }
                    var filePath = Path.Combine(mainPath, formFile.FileName);
                    using (FileStream stream = new FileStream(filePath, FileMode.Create))
                    {
                        formFile.CopyTo(stream);
                    }
                    var fileName = formFile.FileName;
                    string extension = Path.GetExtension(fileName);
                    string conString = string.Empty;
                    switch (extension)
                    {
                        case ".xls":
                            conString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source =" + filePath + ";Extended Properties = 'Excel 8.0;' HDR=Yes";
                            break;
                        case ".xlsx":
                            conString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source =" + filePath + ";Extended Properties = 'Excel 8.0; HDR=Yes'";
                            break;

                    }

                    DataTable dt = new DataTable();
                    conString = string.Format(conString, filePath);
                    using (OleDbConnection conExcel = new OleDbConnection(conString))
                    {
                        using (OleDbCommand cmdExcel = new OleDbCommand())
                        {
                            using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                            {
                                cmdExcel.Connection = conExcel;
                                conExcel.Open();
                                DataTable dtExcelSchema = conExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                cmdExcel.CommandText = "SELECT * FROM [" + sheetName + "]";
                                odaExcel.SelectCommand = cmdExcel;
                                odaExcel.Fill(dt);
                                conExcel.Close();

                            }
                        }
                    }

                    conString = configuration.GetConnectionString("DbConnection");

                    using (NpgsqlConnection con = new NpgsqlConnection(conString))
                    {
                        using (var writer = con.BeginBinaryImport("Copy Customers (CustomerCode, FirstName, LastName, Gender, Age, Country) FROM STDIN (FORMAT BINARY)"))
                        {
                            foreach (var d in dt.Rows)
                            {
                                writer.Write(d.ToString());
                            }
                        }

                    }
                    ViewBag.message = "File Imported Successfully, Data Saved Successfully";
                    return RedirectToAction("Index");
                }
                
            }
            catch (Exception ex)
            {

                string msg = ex.Message;
            }
            return View();
        }
    }
}
