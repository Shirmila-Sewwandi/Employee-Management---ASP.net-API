using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Api2022.NewFolder1;

namespace Api2022.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        //to read the connections from the appsettings.cs - use the dependency injection
        private readonly IConfiguration _configuration;

        public DepartmentsController(IConfiguration configuration)
        { 
            _configuration = configuration;
        }

        //API method to get all data from department table

        [HttpGet]
        [Route("GetDepartment")]

        public JsonResult GetDepartment()
        {
            string query = @"
                           select * from Department
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
        [Route("AddNewDepartment")]

        public JsonResult AddNewDepartment(Department dep) //Department object
        {
           
            string query = @"
                           insert into Department
                           values (@DepartmentName)
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
                    myCommand.Parameters.AddWithValue("@DepartmentName", dep.DepartmentName);
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
        [Route("UpdateDepartment")]
        public JsonResult UpdateDepartment(Department dep) //Department object
        {
            string query = @"
                           update Department
                           set DepartmentName = @DepartmentName
                           where DepartmentId = @DepartmentId
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
                    myCommand.Parameters.AddWithValue("@DepartmentId", dep.DepartmentId);
                    myCommand.Parameters.AddWithValue("@DepartmentName", dep.DepartmentName);
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
        [Route("DeleteDepartment")]

        public JsonResult DeleteDepartment(int id)
        {
            string query = @"
                           delete from Department
                           where DepartmentId = @DepartmentId
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
                    myCommand.Parameters.AddWithValue("@DepartmentId", id);
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

    }
}
