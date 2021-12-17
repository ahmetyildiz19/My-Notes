using Notlarim102.DataAccessLayer.EntityFramework;
using Notlarim102.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notlarim102.BusinessLayer
{
    public class NoteManager
    {
        private Repository<Note> rnote = new Repository<Note>();

        public List<Note> GetAllNotes()
        {
            return rnote.List();
        }

        public object GetAllNoteQueryable()
        {
            return rnote.ListQueryable();
        }
    }
}
