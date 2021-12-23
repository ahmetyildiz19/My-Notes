﻿using Notlarim102.Common;
using Notlarim102.DataAccessLayer;
using Notlarim102.DataAccessLayer.Abstract;
using Notlarim102.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Notlarim102.DataAccessLayer.EntityFramework
{
    public class Repository<T>:RepositoryBase,IRepository<T> where T : class
    {
        private NotlarimContext db;
        private DbSet<T> objSet;

        public Repository()
        {
            db = RepositoryBase.CreateContext();
            objSet = db.Set<T>();
        }

        public List<T> List()
        {
            return objSet.ToList();
        }
        public List<T> List(Expression<Func<T, bool>> eresult)
        {
            return db.Set<T>().Where(eresult).ToList();
            //db.Categories.Where(x => x.Id == 5).ToList();
        }

        public int Insert(T obj)
        {
            objSet.Add(obj);
            //NotlarimUser user =new NotlarimUser(obj) olmadi
            if (obj is MyEntityBase)
            {
                MyEntityBase o = obj as MyEntityBase;
                DateTime now = DateTime.Now;
                o.CreatedOn = now;
                o.ModifiedOn = now;
                o.ModifiedUserName = App.Common.GetCurrentUsername();
                //  o.ModifiedUserName = "system";
            }
            return Save();
        }

        public int Update(T obj)
        {
            if (obj is MyEntityBase)
            {
                MyEntityBase o = obj as MyEntityBase;
                o.ModifiedOn = DateTime.Now;
                o.ModifiedUserName = App.Common.GetCurrentUsername();
            }
            return Save();
        }

        public int Delete(T obj)
        {
            objSet.Remove(obj);
            return Save();
        }


        public int Save()
        {
            return db.SaveChanges();
        }

        public T Find(Expression<Func<T, bool>> eresult)
        {
            return objSet.FirstOrDefault(eresult);
        }

        public IQueryable<T> listQueryable()
        {
            return objSet.AsQueryable<T>();
        }

        public IQueryable<T> ListQueryable()
        {
            return objSet.AsQueryable<T>();
        }
    }
}
