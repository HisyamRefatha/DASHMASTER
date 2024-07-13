using AutoMapper;
using DASHMASTER.CORE.Helper;
using DASHMASTER.DATA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DASHMASTER.CORE.Service.Category.Object
{
    public partial class CategoryResponse : IMapResponse<CategoryResponse, MstCategory>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } 
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public void Mapping(IMappingExpression<MstCategory, CategoryResponse> map)
        {
            
        }
    }
}
