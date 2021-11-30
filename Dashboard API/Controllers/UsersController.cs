using Dashboard_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Dashboard_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UsersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get(string username)
        {
            string query = @"select Id, UserName from Users";
            if (!String.IsNullOrEmpty(username))
            {
                query = $"select Id, UserName from Users WHERE UserName = '{username}'";
            }

            DataTable table = new();
            string sqlDataSource = _configuration.GetConnectionString("Dashboard");
            MySqlDataReader sqlDataReader;
            using (MySqlConnection sqlConnection = new(sqlDataSource))
            {
                sqlConnection.Open();
                using (MySqlCommand sqlCommand = new(query, sqlConnection))
                {
                    sqlDataReader = sqlCommand.ExecuteReader();
                    table.Load(sqlDataReader);
                    sqlDataReader.Close();
                    sqlConnection.Close();
                }
            }

            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(DashboardUser dashboardUser)
        {
            string query = @"insert into Users (UserName) values (@UserName)";

            DataTable table = new();
            string sqlDataSource = _configuration.GetConnectionString("Dashboard");
            MySqlDataReader sqlDataReader;
            using (MySqlConnection sqlConnection = new(sqlDataSource))
            {
                sqlConnection.Open();
                using (MySqlCommand sqlCommand = new(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@UserName", dashboardUser.UserName);
                    sqlDataReader = sqlCommand.ExecuteReader();
                    table.Load(sqlDataReader);
                    sqlDataReader.Close();
                    sqlConnection.Close();
                }
            }

            return new JsonResult("Added Successfully");
        }

        [HttpPut]
        public JsonResult Put(DashboardUser dashboardUser)
        {
            string query = @"update Users set 
                             UserName = @UserName
                             where Id=@Id";

            DataTable table = new();
            string sqlDataSource = _configuration.GetConnectionString("Dashboard");
            MySqlDataReader sqlDataReader;
            using (MySqlConnection sqlConnection = new(sqlDataSource))
            {
                sqlConnection.Open();
                using (MySqlCommand sqlCommand = new(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@Id", dashboardUser.Id);
                    sqlCommand.Parameters.AddWithValue("@UserName", dashboardUser.UserName);
                    sqlDataReader = sqlCommand.ExecuteReader();
                    table.Load(sqlDataReader);
                    sqlDataReader.Close();
                    sqlConnection.Close();
                }
            }

            return new JsonResult("Updated Successfully");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"delete from Users
                             where Id=@Id";

            DataTable table = new();
            string sqlDataSource = _configuration.GetConnectionString("Dashboard");
            MySqlDataReader sqlDataReader;
            using (MySqlConnection sqlConnection = new(sqlDataSource))
            {
                sqlConnection.Open();
                using (MySqlCommand sqlCommand = new(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@Id", id);
                    sqlDataReader = sqlCommand.ExecuteReader();
                    table.Load(sqlDataReader);
                    sqlDataReader.Close();
                    sqlConnection.Close();
                }
            }

            return new JsonResult("Deleted Successfully");
        }
    }
}
