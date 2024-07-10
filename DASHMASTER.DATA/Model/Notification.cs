using System;
using System.Collections.Generic;
using Vleko.DAL.Interface;


namespace DASHMASTER.DATA.Model 
{
    public partial class Notification : IEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Message { get; set; } = null!;
        public bool ReadStatus { get; set; }
        public DateTime CreateDate { get; set; }

        public virtual MstUser User { get; set; } = null!;
    }
}
