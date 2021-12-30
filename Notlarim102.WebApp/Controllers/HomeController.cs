using Notlarim102.BusinessLayer;
using Notlarim102.Common;
using Notlarim102.Entity;
using Notlarim102.Entity.Messages;
using Notlarim102.Entity.ValueObject;
using Notlarim102.WebApp.Models;
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
        private readonly NoteManager nm = new NoteManager(); //Fake Datalari silmek icin kullandigimiz 2.ci islemimiz.

        private readonly CategoryManager cm = new CategoryManager();
        private readonly NotlarimUserManager num = new NotlarimUserManager();
        private  BusinessLayerResult<NotlarimUser> res;

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
            App.Common.GetCurrentUsername();
            //return View(nm.GetAllNotes().OrderByDescending(x => x.ModifiedOn).ToList());
            return View(nm.QList().Where(x=>x.IsDraft==false).OrderByDescending(x => x.ModifiedOn).ToList());
        }

        public ActionResult ByCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            List<Note> notes = nm.QList().Where(x => x.IsDraft == false && x.CategoryID == id).OrderByDescending(x => x.ModifiedOn).ToList();

            //TempData["mm"] = cat.Notes;

            return View("Index", notes);
        }

        public ActionResult MostLiked()
        {
            return View("Index", nm.QList().OrderByDescending(x => x.LikeCount).ToList());
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
                //Session["login"] = res.Result; //sessiona kullanici bilgileri gonderme
                CurrentSession.Set("login",res.Result);
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

        public ActionResult UserActivate(Guid id)
        {
            BusinessLayerResult<NotlarimUser> res = num.ActivateUser(id);

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
            return View();
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
            //NotlarimUser currentUser = Session["login"] as NotlarimUser;
            //BusinessLayerResult<NotlarimUser> res = num.GetUserById(currentUser.Id);
            if (CurrentSession.User is NotlarimUser currentUser) res = num.GetUserById(currentUser.Id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Geçersiz profil işlemi.",
                    Items = res.Errors
                    //RedirectingUrl = "/Home/Login"
                };
                return View("Error", errorNotifyObj);
                //Kullaniciya ait bir hata ekranina yönlendiricegim.
            }
            return View(res.Result);
        }

        public ActionResult EditProfile()
        {

            if (CurrentSession.User is NotlarimUser currentUser) res = num.GetUserById(currentUser.Id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata oluştu",
                    Items = res.Errors
                };
                return View("Error", errorNotifyObj);
            }
            return View(res.Result);
        }
        [HttpPost]
        public ActionResult EditProfile(NotlarimUser model, HttpPostedFileBase ProfileImage)
        {
            ModelState.Remove("ModifiedUsername");
            if (ModelState.IsValid)
            {
                if (ProfileImage != null && (ProfileImage.ContentType == "image/jpeg" || ProfileImage.ContentType == "image/jpg" || ProfileImage.ContentType == "image/png"))
                {
                    string filename = $"user_{model.Id}.{ProfileImage.ContentType.Split('/')[1]}";
                    //user_5.png
                    ProfileImage.SaveAs(Server.MapPath($"~/images/{filename}"));
                    model.ProfileImageFilename = filename;
                }
                res = num.UpdateProfile(model);
                if (res.Errors.Count > 0)
                {
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Title = "Hata oluştu",
                        Items = res.Errors
                    };
                    return View("Error", errorNotifyObj);
                }
                CurrentSession.Set("login", res.Result);
                return RedirectToAction("ShowProfile");
                
            }


            return View(model);
        }

        public ActionResult DeleteProfile()
        {

            if (CurrentSession.User is NotlarimUser currentUser) res = num.DeleteProfile(currentUser.Id);
            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata oluştu",
                    Items = res.Errors
                };
                return View("Error", errorNotifyObj);
            }
            CurrentSession.Clear();
            return RedirectToAction("Index");
        }

        //[HttpPost]
        //public ActionResult DeleteProfile(int id)
        //{
        //    return View();
        //}

        public ActionResult Logout()
        {
            CurrentSession.Clear();
            return RedirectToAction("Index");
        }
    }
}