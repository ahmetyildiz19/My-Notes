﻿using Notlarim102.BusinessLayer.Abstract;
using Notlarim102.Common.Helper;
using Notlarim102.DataAccessLayer.EntityFramework;
using Notlarim102.Entity;
using Notlarim102.Entity.Messages;
using Notlarim102.Entity.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notlarim102.BusinessLayer
{
    public class NotlarimUserManager : ManagerBase<NotlarimUser>
    {

        readonly BusinessLayerResult<NotlarimUser> res = new BusinessLayerResult<NotlarimUser>();
        //home cont. yaptigimiz hata gosterme islemini burada yapacagiz.
        //database ile karsilastircaz
        public BusinessLayerResult<NotlarimUser> RegisterUser(RegisterViewModel data)
        {
            NotlarimUser user = Find(s => s.Username == data.Username || s.Email == data.Email);


            if (user != null)
            {
                if (user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExist, "Kullanici adi daha once kaydedilmis");
                }
                if (user.Email == data.Email) //else diyemeyiz cunku biri digerinin on kosulu degil
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExist, "Email daha once kullanilmis");
                }

                //hata firlatma teknigini kullanabiliriz.

                //throw new Exception("Bu bilgiler daha once kullanilmis!");

                //try catchini home cont. yazdik
            }
            else
            {
                DateTime now = DateTime.Now;
                int dbResult = base.Insert(new NotlarimUser()
                {
                    Username = data.Username,
                    Email = data.Email,
                    Password = data.Password,
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = false,
                    IsAdmin = false,
                    ProfileImageFilename = "user1.jpg"
                    //kapatilanlar repositoryde otomatik eklencek sekilde duzenlenecektir
                    //ModifiedOn = now,
                    //CreatdOn = now,
                    //ModifiedUserName = "system",
                });
                if (dbResult > 0)
                {
                    res.Result = Find(s => s.Email == data.Email && s.Username == data.Username);

                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string activeUri = $"{siteUri}/Home/UserActivete/{res.Result.ActivateGuid}";
                    string body = $"Merhaba {res.Result.Username};<br><br> Hesabinizi aktiflestirmek icin <a href='{activeUri}' target='_blank'> Tiklayiniz </a>.";
                    MailHelper.SendMail(body, res.Result.Email, "Notlarim102 hesap aktiflestirme");

                }
            }
            return res;
        }

        public BusinessLayerResult<NotlarimUser> LoginUser(LoginViewModel data)
        {
            //giris kontrolu
            //hesap aktif mi kontrolu

            //controllerda yapilacaklar
            //yonlendirme
            //session a kullanici bilgilerini gonderme 


            res.Result = Find(s => s.Username == data.Username && s.Password == data.Password);

            if (res.Result != null)
            {
                if (!res.Result.IsActive) //varsayalani true oldugu icin false ise iceri girecek acemice == yapmadik :D
                {
                    res.AddError(ErrorMessageCode.UserIsNotActive, "Kullanici aktiflestirilmemis.");
                    res.AddError(ErrorMessageCode.CheckYourEmail, "Lutfen mailinizi kontrol edin");
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.UsernameOrPassWrong, "Kullanici adi ya da sifre yanlis");
            }
            return res;
        }

        public BusinessLayerResult<NotlarimUser> ActivateUser(Guid id)
        {

            res.Result = Find(x => x.ActivateGuid == id);
            if (res.Result != null)
            {
                if (res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserAlreadyActive, "Bu hesap daha once aktif edilmistir.");
                    return res;
                }
                res.Result.IsActive = true;
                Update(res.Result);
            }
            else
            {
                res.AddError(ErrorMessageCode.ActiveIdDoesNotExist, "Hatali islem!!!");
            }
            return res;
        }

        public BusinessLayerResult<NotlarimUser> GetUserById(int id)
        {
            BusinessLayerResult<NotlarimUser> res = new
                BusinessLayerResult<NotlarimUser>();
            res.Result = Find(s => s.Id == id);
            if (res.Result == null)
            {
                res.AddError
                    (ErrorMessageCode.UserNotFound, "Kullanıcı Bulunamadı.");
            }
            return res;
        }

        public BusinessLayerResult<NotlarimUser> UpdateProfile(NotlarimUser data)
        {
            NotlarimUser user = Find(x => x.Id != data.Id && (x.Username == data.Username || x.Email == data.Email));

            if (user != null && user.Id != data.Id)
            {
                if (user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExist, "Bu kullanıcı adı daha önce alınmış.");
                }
                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExist, "Bu E-Posta daha önce alınmış.");
                }
                return res;
            }

            res.Result = Find(s => s.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;
            res.Result.Username = data.Username;
            if (string.IsNullOrEmpty(data.ProfileImageFilename) == false)
            {
                res.Result.ProfileImageFilename = data.ProfileImageFilename;
            }
            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.UserCouldNotUpdate, "Profil Güncellenemedi.");
            }
            return res;
        }

        public BusinessLayerResult<NotlarimUser> DeleteProfile(int id)
        {
            NotlarimUser user = Find(x => x.Id == id);


            if (user != null)
            {
                if (Delete(user) == 0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotRemove, "Kullanıcı Silinemedi...");
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.UserCouldNotFind, "Kullanıcı Bulunamadı.");
            }
            return res;
        }

        public new BusinessLayerResult<NotlarimUser> Insert(NotlarimUser data)
        {
            NotlarimUser user = Find(s => s.Username == data.Username || s.Email == data.Email);
            res.Result = data;


            if (user != null)
            {
                if (user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExist, "Kullanici adi daha once kaydedilmis");
                }
                if (user.Email == data.Email) //else diyemeyiz cunku biri digerinin on kosulu degil
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExist, "Email daha once kullanilmis");
                }
            }
            else
            {
                res.Result.ProfileImageFilename = "User1.jpg";
                res.Result.ActivateGuid = Guid.NewGuid();

                if (base.Insert(res.Result)==0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotInsert, "Kullanıcı Eklenemedi");
                }
            }
            return res;
        }

        public new BusinessLayerResult<NotlarimUser> Update(NotlarimUser data)
        {
            NotlarimUser user = Find(x => x.Id != data.Id && x.Username == data.Username || x.Email == data.Email);
            res.Result = data;

            if (user != null)
            {
                if (user.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExist, "Bu kullanıcı adı daha önce alınmış.");
                }
                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExist, "Bu E-Posta daha önce girilmiş.");
                }
                return res;
            }

            res.Result = Find(s => s.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;
            res.Result.Username = data.Username;
            res.Result.IsActive = data.IsActive;
            res.Result.IsAdmin = data.IsAdmin;

            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.UserCouldNotUpdate, "Profil Güncellenemedi.");
            }
            return res;
        }
    }
}
