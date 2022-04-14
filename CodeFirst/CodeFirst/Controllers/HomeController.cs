using CodeFirst.DB_Context;
using CodeFirst.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CodeFirst.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDB dbobj;

        public HomeController(AppDB db)
        {
            dbobj = db;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Show()
        {
            List<Employee> empobj = new List<Employee>();

            var res = dbobj.Employees.ToList();

            foreach (var item in res)
            {
                empobj.Add(new Employee
                {
                    Id = item.Id,
                    Name = item.Name,
                    Email = item.Email,
                    Salary = item.Salary,
                    City = item.City
                });
            }


            return View(empobj);
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Employee empobj)
        {
            Employee tblobj = new Employee();
            tblobj.Id = empobj.Id;
            tblobj.Name = empobj.Name;
            tblobj.Email = empobj.Email;
            tblobj.Salary = empobj.Salary;
            tblobj.City = empobj.City;
            if (empobj.Id == 0)
            {
                dbobj.Employees.Add(tblobj);
                dbobj.SaveChanges();
            }
            else
            {
                dbobj.Entry(tblobj).State = EntityState.Modified;
                dbobj.SaveChanges();
            }

            return RedirectToAction("Show","Home");
        }
        public IActionResult Edit(int id)
        {
            Employee tblobj = new Employee();
            var edititem = dbobj.Employees.Where(a => a.Id == id).First();
            tblobj.Id = edititem.Id;
            tblobj.Name = edititem.Name;
            tblobj.Email = edititem.Email;
            tblobj.Salary = edititem.Salary;
            tblobj.City = edititem.City;
            return View("Add", edititem);
        }
        public IActionResult Delete(int id)
        {
            Employee tbl = new Employee();
            var delitem = dbobj.Employees.Where(a => a.Id == id).First();
            dbobj.Employees.Remove(delitem);
            dbobj.SaveChanges();
            return RedirectToAction("Show", "Home");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
