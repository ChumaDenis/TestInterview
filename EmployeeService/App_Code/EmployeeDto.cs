using System.Collections.Generic;
using Newtonsoft.Json;

namespace EmployeeService
{
    public class EmployeeDto
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public bool Enable { get; set; }

        public int? ManagerID { get; set; }

        public ICollection<EmployeeDto> Employees { get; set; }
    }
}
