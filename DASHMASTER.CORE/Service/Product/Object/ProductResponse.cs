using AutoMapper;
using DASHMASTER.CORE.Helper;
using DASHMASTER.DATA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DASHMASTER.CORE.Service.Product.Object
{
    public partial class ProductResponse : IMapResponse<ProductResponse, MstProduct>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }    
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public Guid CategoryId { get; set; }
        public void Mapping(IMappingExpression<MstProduct, ProductResponse> map)
        {
            
        }
    }
}
