using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using PagedList;

namespace HW3_u21638684.Models
{
    public class MaintainVM
    {
        public IPagedList<author>Authors { get; set; }
        public IPagedList<type> Types { get; set; }
        public IEnumerable<book> Books { get; set; }
        public IPagedList<borrow> Borrows { get; set; }

    }
}