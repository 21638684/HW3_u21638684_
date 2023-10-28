using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HW3_u21638684.Models
{
    public class HomeVM
    {
        public IPagedList<student> Students { get; set; }
        public IPagedList<book> Books { get; set; }
        public IPagedList<borrow> Borrows { get; set; }
        
    }
}