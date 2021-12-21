﻿using Notlarim102.DataAccessLayer.EntityFramework;
using Notlarim102.Entity;
using Notlarim102.Entity.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notlarim102.BusinessLayer
{
    public class NotlarimUserManager
    {
         Repository<NotlarimUser> ruser = new Repository<NotlarimUser>();

        public BusinessLayerResult<NotlarimUser> RegisterUser(RegisterViewModel data)
        {
            NotlarimUser user = ruser.Find(s => s.Username == data.Username || s.Email == data.Email);
            BusinessLayerResult<NotlarimUser> layerResult = new BusinessLayerResult<NotlarimUser>();


            if (user != null)
            {
                if (user.Username == data.Username)
                {
                    layerResult.Errors.Add("Kullanıcı Adı daha önce kaydedilmiş.");
                }
                if (user.Email == data.Email)
                {
                    layerResult.Errors.Add("Email daha önce kullanılmış.");
                }


                // throw new Exception("Bu bilgiler daha önce kullanılmış.");
            }
            else
            {
                DateTime now = DateTime.Now;
                int dbResult = ruser.Insert(new NotlarimUser()
                {
                    Username = data.Username,
                    Email = data.Email,
                    Password = data.Password,
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = false,
                    IsAdmin = false,
                    ModifiedOn = now,
                    CreatedOn = now,
                    ModifiedUsername = "system"
                });
                if (dbResult > 0)
                {
                    layerResult.Result = ruser.Find(s => s.Email == data.Email && s.Username == data.Username);

                }
            }
            return layerResult;
        }
       public BusinessLayerResult<NotlarimUser> LoginUser(LoginViewModel data)
        {
            //giris kontrolu
            //hesap aktif mi
            //yonlendirme
            //session a kullanici bilgilerini gonderme.
            BusinessLayerResult<NotlarimUser> res = new BusinessLayerResult<NotlarimUser>();
            res.Result = ruser.Find(s => s.Username == data.Username && s.Password == data.Password);

            if (res.Result!=null)
            {
                if (!res.Result.IsActive)
                {
                    res.Errors.Add("Kullanıcı aktifleştirilmemiş. Lütfen Mail'inizi kontrol edin.");
                }
            }
            else
            {
                res.Errors.Add("Kullanıcı Adı yada Şifre hatalı.");
            }
            return res;
        }
    }
}
