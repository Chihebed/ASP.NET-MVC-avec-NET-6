using EmployeeCrud.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeCrud.Controllers
{
    public class DepartmentController : Controller
    {
        ApplicationDbContext DbContext = new ApplicationDbContext();

        public IActionResult Index()
        {
            var departments = GetDepartments();
            return View(departments);
        }


        private List<Department> GetDepartments()
        {
            var departments = DbContext.Departments.ToList();
            return departments;
        }

        [HttpPost]
        public ActionResult AddDepartment(Department department)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    DbContext.Departments.Add(department);
                    DbContext.SaveChanges();

                    return RedirectToAction("Index"); // Replace "Index" with the action you want to redirect to
                }
                catch (Exception ex)
                {

                    ModelState.AddModelError("", "An error occurred while adding the department.");
                }
            }

            return View("CreateDep", department);
        }

        [HttpGet]
        public ActionResult AddDepartment()
        {
            return View("CreateDep", new Department());
        }
        public ActionResult DeleteDepartment(int id)
        {

            var departmentToDelete = DbContext.Departments.Find(id);

            DbContext.Departments.Remove(departmentToDelete);
            DbContext.SaveChanges();

            return RedirectToAction("Index");

        }
        [HttpGet]
        public ActionResult EditDepartment(int id)
        {
            var departmentToEdit = DbContext.Departments.Find(id);

            if (departmentToEdit == null)
            {
                return NotFound();
            }

            return View("EditDep", departmentToEdit);
        }

        [HttpPost]
        public ActionResult EditDepartment(Department department)
        {
            if (ModelState.IsValid)
            {
                DbContext.Entry(department).State = EntityState.Modified;
                DbContext.SaveChanges();

                return RedirectToAction("Index");
            }

            return View("EditDep", department);
        }




    }
}
