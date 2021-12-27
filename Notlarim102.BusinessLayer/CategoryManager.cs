using Notlarim102.BusinessLayer.Abstract;
using Notlarim102.DataAccessLayer.EntityFramework;
using Notlarim102.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notlarim102.BusinessLayer
{
    public class CategoryManager:ManagerBase<Category>
    {
        NoteManager nm = new NoteManager();
        LikedManager lm = new LikedManager();
        CommentManager cmn = new CommentManager();
        //public override int Delete(Category obj)
        //{
        //    foreach (Note note in obj.Notes.ToList())
        //    {
        //        foreach (Liked like in note.Likes.ToList())
        //        {
        //            lm.Delete(like);
        //        }
        //        foreach (Comment comment in note.Comments.ToList())
        //        {
        //            cmn.Delete(comment);
        //        }

        //        nm.Delete(note);
        //    }
        //    return base.Delete(obj);
        //}




        // public abstract class ManagerBase<T> : IDataAccess<T> where T : class

        //private Repository<Category> rcat = new Repository<Category>();

        // public List<Category> GetCategories()
        // {
        //     return rcat.List();
        // }
        // public Category GetCategoriesById(int id)
        // {
        //     return rcat.Find(s=>s.Id==id);
        // }
    }
}
