// Controllers/EmployeeController.cs
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const int PageSize = 5; 

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var totalEmployees = await _context.Employees.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalEmployees / PageSize);

            var employees = await _context.Employees
                                          .OrderBy(e => e.Name)
                                          .Skip((page - 1) * PageSize)
                                          .Take(PageSize)
                                          .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = PageSize;

            return View(employees);
        }

        // GET: Employee/Create
        public IActionResult Create()
        {
            return PartialView("_CreateEditPartial", new Employee());
        }

        // POST: Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Email,Department,HireDate,Salary")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Employee added successfully!";
                return Json(new { success = true });
            }
            // If validation fails, return the partial view with errors
            return PartialView("_CreateEditPartial", employee);
        }

        // GET: Employee/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return PartialView("_CreateEditPartial", employee);
        }

        // POST: Employee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Department,HireDate,Salary")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Employee updated successfully!";
                    return Json(new { success = true });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return PartialView("_CreateEditPartial", employee);
        }

        // GET: Employee/Delete/5 (Modal confirmation for delete)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return PartialView("_DeleteConfirmationPartial", employee);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Employee deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Employee not found.";
            }
            return Json(new { success = true }); // Return JSON for AJAX
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}