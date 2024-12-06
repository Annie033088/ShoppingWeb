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
            Role role = new Role();
            long allPermission = 0;

            foreach (string strPermission in strPermissions)
            {
                UserPermission permission = (UserPermission)Enum.Parse(typeof(UserPermission), strPermission);
                allPermission += (long)permission;
            }

            role.Name = roleName;
            role.Description = roleDiscript;
            role.Permissions = allPermission;
            return roleRepository.AddRole(role);
        }

        /// <summary>
        /// 取得對應角色權限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public List<string> GetRolePermissions(string roleId)
        {
            string permissionString = roleRepository.GetRolePermissions(int.Parse(roleId));

            long permissionValue = long.Parse(permissionString);
            List<string> permissions = new List<string>();

            foreach (UserPermission permission in Enum.GetValues(typeof(UserPermission)))
            {
                if (permission != UserPermission.None && (permissionValue & (long)permission) == (long)permission)
                {
                    permissions.Add(permission.ToString());
                }
            }

            return permissions;
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="strPermissions"></param>
        /// <param name="roleId"></param>
        /// <param name="roleName"></param>
        /// <param name="roleDiscript"></param>
        /// <returns></returns>
        public bool EditRole(List<string> strPermissions, string roleId, string roleName, string roleDiscript)
        {
            List<UserPermission> permissions = new List<UserPermission>();
            Role role = new Role();
            long allPermission = 0;

            foreach (string strPermission in strPermissions)
            {
                UserPermission permission = (UserPermission)Enum.Parse(typeof(UserPermission), strPermission);
                allPermission += (long)permission;
            }

            role.RoleId = int.Parse(roleId);
            role.Name = roleName;
            role.Description = roleDiscript;
            role.Permissions = allPermission;
            return roleRepository.EditRole(role);
        }

        /// <summary>
        /// 刪除角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public bool DeleteRole(string roleId)
        {
            int intRoleId = int.Parse(roleId);
            return roleRepository.DeleteRole(intRoleId);
        }

        /// <summary>
        /// 取得角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public Role GetRole(string roleId)
        {
            return roleRepository.GetRole(int.Parse(roleId));
        }

    }
}