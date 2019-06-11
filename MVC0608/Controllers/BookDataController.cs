using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC0608.Controllers
{
    public class BookDataController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost()]
        public JsonResult Search(Models.BookSearch arg)
        {
            Models.BookService BookService = new Models.BookService();
            var result = BookService.GetBookByCondtioin(arg);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost()]
        public JsonResult Insert(Models.BookData book)
        {
            Models.BookService BookService = new Models.BookService();
            var result = BookService.Insert(book);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost()]
        public JsonResult Delete(string BookId)
        {
            Models.BookService BookService = new Models.BookService();
            var result = BookService.Delete(BookId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet()]
        public JsonResult Book_Class()
        {
            Models.BookService BookService = new Models.BookService();
            var result = BookService.Book_Class();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet()]
        public JsonResult Book_Status()
        {
            Models.BookService BookService = new Models.BookService();
            var result = BookService.Book_Status();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet()]
        public JsonResult Book_Keeper()
        {
            Models.BookService BookService = new Models.BookService();
            var result = BookService.Book_Keeper();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}