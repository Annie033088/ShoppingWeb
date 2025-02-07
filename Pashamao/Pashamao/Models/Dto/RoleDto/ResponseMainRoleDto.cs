namespace Pashamao.Models.Dto.RoleDto
{
    public class ResponseMainRoleDto
    {
        public ResponseMainRoleDto(Role role)
        {
            RoleId = role.RoleId;
            Name = role.Name;
            Description = role.Description;
        }

        /// <summary>
        /// 角色Id
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 角色名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }
}