using AutoMapper;
using DASHMASTER.CORE.Helper;
using DASHMASTER.DATA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DASHMASTER.CORE.Service.Identity.Object
{
    public partial class UserRequest : IMapRequest<MstUser, UserRequest>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        public void Mapping(IMappingExpression<UserRequest, MstUser> map)
        {
            
        }
    }
}
