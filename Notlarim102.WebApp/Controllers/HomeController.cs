using Notlarim102.BusinessLayer;
using Notlarim102.Entity;
using Notlarim102.Entity.Messages;
using Notlarim102.Entity.ValueObject;
using Notlarim102.WebApp.ViewModel;
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
            Test test = new Test();
            //test.InsertTest();
            //test.UpdateTest();
            //test.DeleteTest();
            //test.CommentTest();

            //if (TempData["mm"]!=null)
            //{
            //    return View(TempData["mm"] as List<Note>);
            //}

            NoteManager nm = new NoteManager();

            //return View(nm.GetAllNotes().OrderByDescending(x => x.ModifiedOn).ToList());
            return View(nm.GetAllNoteQueryable().OrderByDescending(x => x.ModifiedOn).ToList());
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
                return HttpNotFound();
            }

            //TempData["mm"] = cat.Notes;

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
                if (res.Errors.Count > 0)
                {
                    if (res.Errors.Find(x => x.Code == ErrorMessageCode.UserIsNotActive) != null)
                    {
                        ViewBag.SetLink = $"https://localhost:44379/Home/UserActive/{res.Result.ActivateGuid}";
                    }
                    res.Errors.ForEach(s => ModelState.AddModelError("", s.Message));
                    return View(model);
                }
                Session["login"] = res.Result; //sessiona kullanici bilgileri gonderme

                return RedirectToAction("Index"); //yonlendirme
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
            //kullanici adinin uygunlugu unique
            //email kontrolu
            //aktivasyon islemi yapilmali
            //bool hasError = false;
            if (ModelState.IsValid)
            {
                NotlarimUserManager num = new NotlarimUserManager();

                BusinessLayerResult<NotlarimUser> res = num.RegisterUser(model);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(s => ModelState.AddModelError("", s.Message));
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

                //kontrol eder. yani modelden gelen yapi uygunsa
                //if (model.Username=="aaa")
                //{
                //    ModelState.AddModelError("","Bu kullanici adi uygun degil!");
                //    //hasError = true;
                //}
                //if (model.Email=="aaa@aaa.com")
                //{
                //    ModelState.AddModelError("", "Email adresi daha once kullanilmis. Baska bir email deneyin.");
                //    //hasError = true;
                //}
                ////if (hasError==true)
                ////{
                ////    return View(model);
                ////}
                ////else
                ////{
                ////    return RedirectToAction("RegisterOk");
                ////}

                //foreach (var item in ModelState)
                //{
                //    if (item.Value.Errors.Count > 0)
                //    {
                //        return View(model);
                //    }
                //}
                OkViewModel notifyObj = new OkViewModel()
                {
                    Title = "Kayıt Başarılı",
                    RedirectingUrl = "/Home/Login"
                };
                notifyObj.Items.Add("Lütfen e-posta adresinize gönderilen aktivasyon koduna tıklayarak hesabını aktif edin. Hesabınızı aktif etmeden not ekleyemez ve beğenme yapamazsınız.");
                return View("Ok", notifyObj);
            }

            return View(model);
        }
        public ActionResult RegisterOk()
        {
            return View();
        }

        public ActionResult UserActivete(Guid id)
        {
            NotlarimUserManager num = new NotlarimUserManager();
            BusinessLayerResult<NotlarimUser> res = num.ActiveUser(id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Gecersiz Aktivasyon Islemi",
                    Items = res.Errors
                };
                return View("Error", errorNotifyObj);
                //TempData["errors"] = res.Errors;
                //return RedirectToAction("UserActivateCancel");
            }
        }

        public ActionResult UserActiveteOk()
        {
            return View();
        }
        public ActionResult UserActiveteCancel()
        {
            List<ErrorMessageObject> errors = null;
            if (TempData["errors"] != null)
            {
                errors = TempData["errors"] as List<ErrorMessageObject>;
            }
            return View(errors);
        }


        public ActionResult ShowProfile()
        {
            NotlarimUser currentUser = Session["login"] as NotlarimUser;
            NotlarimUserManager num = new NotlarimUserManager();
            BusinessLayerResult<NotlarimUser> res = num.GetUserById(currentUser.Id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Geçersiz profil işlemi.",
                    RedirectingUrl = "/Home/Login"
                };
                errorNotifyObj.Items.Add("Geçersiz aktivasyon işlemi.");
                return View("Error", errorNotifyObj);
                //Kullaniciya ait bir hata ekranina yönlendiricegim.
            }
            return View(res.Result);
        }

        public ActionResult EditProfile()
        {
            return View();
        }

        public ActionResult EditProfile(int id)
        {
            return View();
        }

        public ActionResult DeleteProfile()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DeleteProfile(int id)
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }
    }
}