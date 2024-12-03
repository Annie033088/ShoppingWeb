using Pashamao.Models;
using Pashamao.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Pashamao.Service
{
    public class UserRoleService
    {
        private RoleRepository roleRepository;
        public UserRoleService() { 
            roleRepository = new RoleRepository();
        }

        public List<Role> GetAllRole()
        {
            return roleRepository.GetAllRole().ToList();
        }
    }
}