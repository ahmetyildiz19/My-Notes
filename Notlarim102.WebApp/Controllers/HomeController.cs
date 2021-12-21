﻿using Notlarim102.BusinessLayer;
using Notlarim102.Entity;
using Notlarim102.Entity.ValueObject;
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
            ////TempData["mm"] = cat.Notes;//bu yaptığımız tempdata ile sayfamıza taşıyacak yukarıda yaptıklarımı.(bu veriyi home controllere taşıdım.)
            return View("Index", cat.Notes.OrderByDescending(x => x.ModifiedOn).ToList());
        }

        public ActionResult MostLiked()
        {
            NoteManager nm = new NoteManager();

            return View("Index", nm.GetAllNotes().OrderByDescending(x => x.LikeCount).ToList());
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                NotlarimUserManager num = new NotlarimUserManager();
                BusinessLayerResult<NotlarimUser> res = num.LoginUser(model);
                if (res.Errors.Count>0)
                {
                    res.Errors.ForEach(s => ModelState.AddModelError("", s));
                    return View(model);
                }
                //yonlendirme
                Session["Login"] = res.Result;//session a kullanici bilgilerini gonderme.
                return RedirectToAction("Index");//yonlendirme
            }
                return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            //kullanici adinin uygunlugu
            //email kontrolu
            //activasyon islemi
            //bool hasError = false;
            if (ModelState.IsValid)
            {
                NotlarimUserManager num = new NotlarimUserManager();
                BusinessLayerResult<NotlarimUser> res = num.RegisterUser(model);

                if (res.Errors.Count>0)
                {
                    res.Errors.ForEach(s=>ModelState.AddModelError("",s));
                    return View(model);
                }






                //NotlarimUser user = null;
                //try
                //{
                //    user = num.RegisterUser(model);
                //}
                //catch (Exception ex)
                //{
                //    ModelState.AddModelError("", ex.Message);
                //}

                //if (model.Username == "aaa")
                //{
                //    ModelState.AddModelError("", "Bu kullanıcı adı uygun değil.");
                //    //hasError = true;
                //}
                //if (model.Email == "aaa@aaa.com")
                //{
                //    ModelState.AddModelError("", "Bu Email adresi daha önce kullanılmıştır.");
                //    //hasError = true;
                //}
                // if (hasError == true)
                // {
                //     return View(model);
                // }
                // else
                // {
                //     return RedirectToAction("RegisterOk");
                // }

                //foreach (var item in ModelState)
                //{
                //    if (item.Value.Errors.Count > 0)
                //    {
                //        return View(model);
                //    }
                //}
                return RedirectToAction("RegisterOk");
            }
            return View();
        }
        public ActionResult RegisterOk()
        {
            return View();
        }
    }
}