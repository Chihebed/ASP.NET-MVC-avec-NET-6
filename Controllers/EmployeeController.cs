using EmployeeCrud.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeCrud.Controllers
{
    public enum SortDirection
    {
        Ascending,
        Descending
    }
    public class EmployeeController : Controller
    {
        ApplicationDbContext DbContext = new ApplicationDbContext();
        public IActionResult Index(string searchByName)
        {
            var employees = GetEmployees();

            if (!string.IsNullOrEmpty(searchByName))
            {
                employees = employees.Where(e => e.EmployeeName.ToLower().Contains(searchByName.ToLower())).ToList();
            }

            return View(employees);
        }


        private List<Employee> GetEmployees()
        {
            var employees = (from employee in DbContext.Employees
                             join department in DbContext.Departments on employee.Departmentid equals department.DepartmentID
                             select new Employee
                             {
                                 EmployeeID = employee.EmployeeID,
                                 EmployeeName = employee.EmployeeName,
                                 EmployeeNumber = employee.EmployeeNumber,
                                 DOB = employee.DOB,
                                 HiringDate = employee.HiringDate,
                                 GrossSalary = employee.GrossSalary,
                                 NetSalary = employee.NetSalary,
                                 Departmentid = employee.Departmentid,
                                 DepartmentName = department.DepartmentName,
                             }).ToList();
            return employees;
        }

        public IActionResult Create()
        {
            ViewBag.Departments = this.DbContext.Departments.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Employee model)
        {
            ModelState.Remove("EmployeeID");
            ModelState.Remove("Department");
            ModelState.Remove("DepartmentName");
            if (ModelState.IsValid)
            {
                DbContext.Employees.Add(model);
                DbContext.SaveChanges();
                return RedirectToAction("Index");

            }
            ViewBag.Departments = this.DbContext.Departments.ToList();
            return View();
        }
        public IActionResult Edit(int ID)
        {
            Employee data = this.DbContext.Employees.Where(e => e.EmployeeID == ID).FirstOrDefault();
            ViewBag.Departments = this.DbContext.Departments.ToList();
            return View("Create", data);
        }
        [HttpPost]
        public IActionResult Edit(Employee model)
        {
            ModelState.Remove("EmployeeID");
            ModelState.Remove("Department");
            ModelState.Remove("DepartmentName");
            if (ModelState.IsValid)
            {
                DbContext.Employees.Update(model);
                DbContext.SaveChanges();
                return RedirectToAction("Index");

            }
            ViewBag.Departments = this.DbContext.Departments.ToList();
            return View("Create", model);

        }
        public IActionResult Delete(int ID)
        {
            Employee data = this.DbContext.Employees.Where(e => e.EmployeeID == ID).FirstOrDefault();
            if (data != null)
            {
                DbContext.Employees.Remove(data);
                DbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        private List<Employee> SertEmployees(List<Employee> employees, string sortFeild, string currentSortFeild, SortDirection sortDirection)
        {
            if (string.IsNullOrEmpty(sortFeild))
            {
                ViewBag.SortFeild = "EmployeeNumber";
                ViewBag.SortDirection = SortDirection.Ascending;
            }
            else
            {
                if (currentSortFeild == sortFeild)
                    ViewBag.SortDirection = sortDirection == SortDirection.Ascending ? SortDirection.Descending : SortDirection.Ascending;
                else
                    ViewBag.SortDirection = SortDirection.Ascending;
                ViewBag.SortField = sortFeild;

            }
            var propertyInfo = typeof(Employee).GetProperty(ViewBag.SortFeild);
            if (ViewBag.SortDirection == SortDirection.Ascending)
                employees = employees.OrderBy(e => propertyInfo.GetValue(e, null)).ToList();
            else
                employees = employees.OrderBy(e => propertyInfo.GetValue(e, null)).ToList();
            return employees;

        }


    }
}
