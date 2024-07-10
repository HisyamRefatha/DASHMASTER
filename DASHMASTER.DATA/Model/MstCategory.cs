using System;
using System.Collections.Generic;
using Vleko.DAL.Interface;


namespace DASHMASTER.DATA.Model 
{
    public partial class MstCategory : IEntity
    {
        public MstCategory()
        {
            MstProduct = new HashSet<MstProduct>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; } = null!;
        public DateTime? UpdateDate { get; set; }
        public string? UpdateBy { get; set; }

        public virtual ICollection<MstProduct> MstProduct { get; set; }
    }
}
