using System;
using System.Collections.Generic;
using Vleko.DAL.Interface;


namespace DASHMASTER.DATA.Model 
{
    public partial class Reports : IEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CreateDate { get; set; }
        public string GeneratedBy { get; set; } = null!;
    }
}
