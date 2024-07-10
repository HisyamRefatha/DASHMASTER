using AutoMapper;
using DASHMASTER.CORE.Helper;
using DASHMASTER.DATA.Model;

namespace DASHMASTER.CORE.Service.Product.Object
{
    public partial class ProductRequest : IMapRequest<MstProduct, ProductRequest>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public Guid CategoryId { get; set; }
        public void Mapping(IMappingExpression<ProductRequest, MstProduct> map)
        {
            
        }
    }
}
