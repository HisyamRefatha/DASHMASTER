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
    public partial class CategoryRequest : IMapRequest<MstCategory, CategoryRequest>
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public void Mapping(IMappingExpression<CategoryRequest, MstCategory> map)
        {
            
        }
    }
}
