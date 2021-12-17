using Notlarim102.BusinessLayer;
using Notlarim102.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Notlarim102.WebApp.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            //Test test = new Test();
            //test.InsertTest();
            //test.UpdateTest();
            //test.DeleteTest();
            //test.CommentTest();

            //if (TempData["mm"] != null)
            //{
            //    return View(TempData["mm"] as List<Note>);
            //    //Indeximize Tempdata'yı List Tipinde göndermek için yazdım.
            //}


            NoteManager nm = new NoteManager();

            return View(nm.GetAllNotes().OrderByDescending(x => x.ModifiedOn).ToList());
            //return View(nm.GetAllNoteQueryable().OrderByDescending(x=>x.ModifiedOn));
        }
        public ActionResult ByCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CategoryManager cm = new CategoryManager();
            Category cat = cm.GetCategoriesById(id.Value);

            if (cat == null)
            {
                return HttpNotFound(); //id bulunmazsa bu yazdığımız kod sayfada hata olarak gözükecektir.
            }
            //TempData["mm"] = cat.Notes;//bu yaptığımız tempdata ile sayfamıza taşıyacak yukarıda yaptıklarımı.(bu veriyi home controllere taşıdım.)
            return View("Index", cat.Notes.OrderByDescending(x => x.ModifiedOn).ToList());
        }

        public ActionResult MostLiked()
        {
            NoteManager nm = new NoteManager();

            return View("Index", nm.GetAllNotes().OrderByDescending(x => x.LikeCount).ToList());
        }


    }
}