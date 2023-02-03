using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Api2022.NewFolder1;
using System.Runtime.Intrinsics.Arm;
using Microsoft.AspNetCore.Hosting;

namespace Api2022.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        //to read the connections from the appsettings.cs - use the dependency injection
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public EmployeesController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        //API method to get all data from department table

        [HttpGet]
        [Route("GetEmployee")]

        public JsonResult GetEmployee()
        {
            string query = @"
                            select EmployeeId, EmployeeName,Department,DateOfJoining,PhotoFileName
                            from Employee
                           ";

            //get data to adatatable object
            DataTable dataTable = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myreader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    //fill the data in the datatable using sql data reader
                    myreader = myCommand.ExecuteReader();
                    dataTable.Load(myreader);
                    myreader.Close();
                    myCon.Close();
                }

            }

            //return the json result
            return new JsonResult(dataTable);
        }


        //API method to post new records to department table

        [HttpPost]
        [Route("AddNewEmployee")]

        public JsonResult AddNewEmployee(Employee emp) //Employee object
        {

            string query = @"
                           insert into Employee
                           (EmployeeName, Department, DateOfJoining, PhotoFileName)
                           values (@EmployeeName, @Department, @DateOfJoining, @PhotoFileName)
                           ";

            //get data to adatatable object
            DataTable dataTable = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myreader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@EmployeeName", emp.EmployeeName);
                    myCommand.Parameters.AddWithValue("@Department", emp.Department);
                    myCommand.Parameters.AddWithValue("@DateOfJoining", emp.DateOfJoining);
                    myCommand.Parameters.AddWithValue("@PhotoFileName", emp.PhotoFileName);
                    //
                    myreader = myCommand.ExecuteReader();
                    dataTable.Load(myreader);
                    myreader.Close();
                    myCon.Close();
                }

            }

            //return the json result
            return new JsonResult("Added Successfully");
        }

        //API method to put to update records

        [HttpPut]
        [Route("UpdateEmployee")]
        public JsonResult UpdateEmployee(Employee emp) //Department object
        {
            string query = @"
                           update Employee
                           set EmployeeName = @EmployeeName,
                           Department = @Department,
                           DateOfJoining = @DateOfJoining,
                           PhotoFileName = @PhotoFileName
                           where EmployeeId = @EmployeeId
                           ";

            //get data to adatatable object
            DataTable dataTable = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myreader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@EmployeeId", emp.EmployeeId);
                    myCommand.Parameters.AddWithValue("@EmployeeName", emp.EmployeeName);
                    myCommand.Parameters.AddWithValue("@Department", emp.Department);
                    myCommand.Parameters.AddWithValue("@DateOfJoining", emp.DateOfJoining);
                    myCommand.Parameters.AddWithValue("@PhotoFileName", emp.PhotoFileName);
                    //
                    myreader = myCommand.ExecuteReader();
                    dataTable.Load(myreader);
                    myreader.Close();
                    myCon.Close();
                }

            }

            //return the json result
            return new JsonResult("Updated Successfully");
        }


        //API method to delete records

        [HttpDelete]
        [Route("DeleteEmployee")]

        public JsonResult DeleteEmployee(int id)
        {
            string query = @"
                           delete from Employee
                           where EmployeeId = @EmployeeId
                           ";

            //get data to adatatable object
            DataTable dataTable = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myreader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@EmployeeId", id);
                    //
                    myreader = myCommand.ExecuteReader();
                    dataTable.Load(myreader);
                    myreader.Close();
                    myCon.Close();
                }

            }

            //return the json result
            return new JsonResult("Deleted Successfully");

        }

        [HttpPost]
        [Route("SaveFile")]

        public JsonResult SaveFile()
        {
            try {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos" + filename;

                using (var stream = new FileStream(physicalPath, FileMode.Create)) 
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(filename);


            }

            catch (Exception ex) {

                return new JsonResult("first.png");
            }
        }

    }
}
