using HW3_u21638684.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace HW3_u21638684.Controllers
{
    public class HomeController : Controller
    {
        private const int PageSize = 10;
        private LibraryEntities db = new LibraryEntities();

        public async Task<ActionResult> Index(int? studentPage, int? bookPage)
        {
            var students = await db.students.Include(b => b.borrows).ToListAsync();
            var studentPagedList = students.ToPagedList(studentPage ?? 1, PageSize);

            var books = await db.books
                .Include(a => a.borrows)
                .Include(t => t.type)
                .Include(b => b.author)
                .ToListAsync();
            var bookPagedList = books.ToPagedList(bookPage ?? 1, PageSize);
            var viewModel = new HomeVM
            {
                Students = studentPagedList,
                Books = bookPagedList
            };

            return View(viewModel);
        }
    }
}
