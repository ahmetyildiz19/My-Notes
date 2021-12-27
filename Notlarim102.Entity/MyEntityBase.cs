using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notlarim102.Entity
{
    public class MyEntityBase
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [DisplayName("Oluşturma Tarihi"),Required]
        public DateTime CreatedOn { get; set; }
        [DisplayName("Değiştirme Tarihi"),Required]
        public DateTime ModifiedOn { get; set; }
        [DisplayName("Düzenleyen"),Required, StringLength(30)]
        public string ModifiedUserName { get; set; }
    }
}
