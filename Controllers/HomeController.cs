using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rotativa;
using Test.Context;
using Test.Models;


namespace Test.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index(string? values)
        {
            List<Person> persons;

            switch (values)
            {
                case "discName":
                    persons =await _context.Persons.OrderByDescending(a => a.Name).ToListAsync();
                    break;
                case "discDescription":
                    persons =await _context.Persons.OrderByDescending(a => a.Description).ToListAsync();
                    break;
                case "discCreateDate":
                    persons =await _context.Persons.OrderByDescending(a => a.CreatedDate).ToListAsync(); 
                    break;
                case "discUpdateDate":
                    persons = await _context.Persons.OrderByDescending(a => a.UpdatedDate).ToListAsync();
                    break;
                default:
                    persons = await _context.Persons.ToListAsync(); 
                    break;  
            }

            return View( persons);
        }

       
        public async  Task<IActionResult> Filter(string? search)
        {
           
            if (string.IsNullOrEmpty(search)) return Redirect("/");
           var  filter = await  _context.Persons.Where(a => a.Name.ToLower().Contains(search.ToLower()) || a.Description.ToLower().Contains(search.ToLower())).ToListAsync() ;

            return View(filter );    
        }

       

       
    }
}
