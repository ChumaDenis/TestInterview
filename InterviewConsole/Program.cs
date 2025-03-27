using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeService;

namespace InterviewConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1. Get employee by ID");
            Console.WriteLine("2. Enable/Disable employee");
            Console.Write("Enter option number: ");
            string input = Console.ReadLine();
            var employeeService = new Service1();
            switch (input)
            {
                case "1":
                    Console.Write("Enter employee ID: ");
                    int id = int.Parse(Console.ReadLine());
                    var employee = employeeService.GetEmployeeById(id);
                    if (employee != null)
                    {
                        PrintEmployeeTree(employee);
                    }
                    else
                    {
                        Console.WriteLine("Employee not found.");
                    }
                    break;

                case "2":
                    Console.Write("Enter employee ID: ");
                    int empId = int.Parse(Console.ReadLine());
                    Console.Write("Set Enable (true/false): ");
                    bool enable = bool.Parse(Console.ReadLine());
                    employeeService.EnableEmployee(empId, enable);
                    Console.WriteLine("Employee status updated.");
                    break;

                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }

        private static void PrintEmployeeTree(EmployeeDto employee, int indent = 0)
        {
            string indentStr = new string(' ', indent * 4);

            Console.WriteLine();
            Console.WriteLine($"{indentStr}ID: {employee.ID}");
            Console.WriteLine($"{indentStr}Name: {employee.Name}");
            Console.WriteLine($"{indentStr}Enable: {employee.Enable}");
            Console.WriteLine($"{indentStr}ManagerID: {(employee.ManagerID.HasValue ? employee.ManagerID.ToString() : "None")}");
            Console.WriteLine($"{indentStr}Employees: {(employee.Employees.Count > 0 ? "" : "None")}");

            foreach (var subordinate in employee.Employees)
            {
                PrintEmployeeTree(subordinate, indent + 1);
            }
        }
    }
}
