using NLog;
using Pashamao.Models;
using Pashamao.Models.Dto.RoleDto;
using Pashamao.Repositories;
using System;
using System.Collections.Generic;

namespace Pashamao.Service
{
    public class UserRoleService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private RoleRepository roleRepository;
        public UserRoleService()
        {
            roleRepository = new RoleRepository();
        }

        /// <summary>
        /// 取得所有角色
        /// </summary>
        public (List<Role> roles, int totalPage) GetAllRole(RequestGetAllRoleDto getAllRoleDto)
        {
            try
            {
                return roleRepository.GetAllRole(getAllRoleDto);
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

        /// <summary>
        /// 新增角色
        /// </summary>
        public bool AddRole(RequestAddRoleDto addRoleDto)
        {
            List<UserPermission> permissions = new List<UserPermission>();
            Role role = new Role();
            long allPermission = 0;

            try
            {
                foreach (string strPermission in addRoleDto.PermissionCkbs)
                {
                    UserPermission permission = (UserPermission)Enum.Parse(typeof(UserPermission), strPermission);
                    allPermission += (long)permission;
                }

                role.Name = addRoleDto.Name;
                role.Description = addRoleDto.Description;
                role.Permissions = allPermission;
                return roleRepository.AddRole(role);
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

        /// <summary>
        /// 取得對應角色權限
        /// </summary>
        public List<string> GetRolePermissions(int roleId)
        {
            try
            {
                string permissionString = roleRepository.GetRolePermissions(roleId);
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
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        public bool EditRole(RequestEditRoleDto editRoleDto)
        {
            List<UserPermission> permissions = new List<UserPermission>();
            Role role = new Role();
            long allPermission = 0;

            try
            {
                foreach (string strPermission in editRoleDto.PermissionCkbs)
                {
                    UserPermission permission = (UserPermission)Enum.Parse(typeof(UserPermission), strPermission);
                    allPermission += (long)permission;
                }

                role.RoleId = editRoleDto.RoleId;
                role.Name = editRoleDto.Name;
                role.Description = editRoleDto.Description;
                role.Permissions = allPermission;
                return roleRepository.EditRole(role);
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

        /// <summary>
        /// 刪除角色
        /// </summary>
        public bool DeleteRole(int roleId)
        {
            try
            {
                return roleRepository.DeleteRole(roleId);
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

        /// <summary>
        /// 取得角色
        /// </summary>
        public Role GetRoleById(int roleId)
        {
            try
            {
                return roleRepository.GetRoleById(roleId);
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }
    }
}