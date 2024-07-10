using AutoMapper;
using DASHMASTER.CORE.Helper;
using DASHMASTER.DATA.Model;

namespace DASHMASTER.CORE.Service.Identity.Object
{
    public partial class UserResponse : IMapResponse<UserResponse, MstUser>
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        public void Mapping(IMappingExpression<MstUser, UserResponse> map)
        {
           
        }
    }
}
