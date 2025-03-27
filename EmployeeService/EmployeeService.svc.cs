using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Newtonsoft.Json;

namespace EmployeeService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IEmployeeService
    {
        private string _connectionString = "Data Source=(local);Initial Catalog=Test;User ID=sa;Password=pass@word1; ";
        //private string _connectionString = "Server=localhost;Database=test;Trusted_Connection=True; ";

        public EmployeeDto GetEmployeeById(int id)
        {
            var allEmployees = GetAllEmployeesFromDb();
            return BuildTree(allEmployees, id);
        }

        public void EnableEmployee(int id, bool enable)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("UPDATE Employee SET Enable = @Enable WHERE ID = @ID", conn))
            {
                cmd.Parameters.AddWithValue("@Enable", enable);
                cmd.Parameters.AddWithValue("@ID", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private List<EmployeeDto> GetAllEmployeesFromDb()
        {
            var result = new List<EmployeeDto>();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("SELECT ID, Name, Enable, ManagerID FROM Employee", conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        bool enable = reader.GetBoolean(2);
                        int? managerId = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3);

                        result.Add(new EmployeeDto()
                        {
                            ID = id,
                            Name = name,
                            Enable = enable,
                            ManagerID = managerId
                        });
                    }
                }
            }

            return result;
        }

        private EmployeeDto BuildTree(List<EmployeeDto> employees, int rootId)
        {
            var dict = new Dictionary<int, EmployeeDto>();
            foreach (var emp in employees)
            {
                dict[emp.ID] = new EmployeeDto
                {
                    ID = emp.ID,
                    Name = emp.Name,
                    Enable = emp.Enable,
                    Employees = new List<EmployeeDto>()
                };
            }

            EmployeeDto root = null;
            foreach (var emp in employees)
            {
                if (emp.ManagerID.HasValue && dict.ContainsKey(emp.ManagerID.Value))
                {
                    dict[emp.ManagerID.Value].Employees.Add(dict[emp.ID]);
                }

                if (emp.ID == rootId)
                    root = dict[emp.ID];
            }

            return root;
        }
    }
}
