using Pashamao.Models;
using Pashamao.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pashamao.Service
{
    public class UserRoleService
    {
        private RoleRepository roleRepository;
        public UserRoleService()
        {
            roleRepository = new RoleRepository();
        }

        /// <summary>
        /// 取得所有角色
        /// </summary>
        /// <returns></returns>
        public List<Role> GetAllRole()
        {
            return roleRepository.GetAllRole().ToList();
        }

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="strPermissions"></param>
        /// <param name="roleName"></param>
        /// <param name="roleDiscript"></param>
        /// <returns></returns>
        public bool AddRole(List<string> strPermissions, string roleName, string roleDiscript)
        {

            List<UserPermission> permissions = new List<UserPermission>();
            long allPermission = 0;

            foreach (string strPermission in strPermissions)
            {
                UserPermission permission = (UserPermission)Enum.Parse(typeof(UserPermission), strPermission);
                allPermission += (long)permission;
            }

            return roleRepository.AddRole(roleName, roleDiscript, allPermission);
        }

        /// <summary>
        /// 取得所有角色權限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public List<UserPermission> GetRolePermissions(string roleId)
        {
            string permissionString = roleRepository.GetRolePermissions(int.Parse(roleId));

            long permissionValue = long.Parse(permissionString);
            List<UserPermission> permissions = new List<UserPermission>();

            foreach (UserPermission permission in Enum.GetValues(typeof(UserPermission)))
            {
                if (permission != UserPermission.None && (permissionValue & (long)permission) == (long)permission)
                {
                    permissions.Add(permission);
                }
            }

            return permissions;
        }

    }
}