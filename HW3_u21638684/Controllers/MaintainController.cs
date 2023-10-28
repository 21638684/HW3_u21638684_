using HW3_u21638684.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace HW3_u21638684.Controllers
{
    public class MaintainController : Controller
    {
        private const int PageSize = 10;
        private LibraryEntities db = new LibraryEntities();

        public async Task<ActionResult> Maintain(int? authorPage,int? typePage, int? borrowPage)
        {
            var authors = await db.authors.ToListAsync();
            var authorPagedList = authors.ToPagedList(authorPage ?? 1, PageSize);

            var types = await db.types.ToListAsync();
            var typePagedList = types.ToPagedList(typePage ?? 1, PageSize);

            var borrows = await db.borrows
                .Include(s => s.student)
                .Include(b => b.book)
                .ToListAsync();
            var borrowPagedList = borrows.ToPagedList(borrowPage ?? 1, PageSize);

            var viewModel = new MaintainVM
            {
                Authors = authorPagedList,
                Types = typePagedList,
                Borrows = borrowPagedList
            };

            return View(viewModel);
        }
    }

}