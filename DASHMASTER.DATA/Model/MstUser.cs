using System;
using System.Collections.Generic;
using Vleko.DAL.Interface;


namespace DASHMASTER.DATA.Model 
{
    public partial class MstUser : IEntity
    {
        public MstUser()
        {
            Notification = new HashSet<Notification>();
            Reviews = new HashSet<Reviews>();
            TrsTransaction = new HashSet<TrsTransaction>();
        }

        public Guid Id { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; } = null!;
        public DateTime? UpdateDate { get; set; }
        public string? UpdateBy { get; set; }
        public string? Token { get; set; }

        public virtual ICollection<Notification> Notification { get; set; }
        public virtual ICollection<Reviews> Reviews { get; set; }
        public virtual ICollection<TrsTransaction> TrsTransaction { get; set; }
    }
}
