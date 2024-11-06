using BT.Shared.Domain.DTO;
using BT.Shared.Domain;
using BT.Shared;
using BT.Shared.Helpers;

namespace BT.Authentication.API.Data
{
    public static class ModelHelpers
    {
        /*public static GetUserDTO ToEntity(this BTUser entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            else
            {
                return new GetUserDTO
                {
                    FirstName = entity.FirstName,
                    Email = entity.Email,
                    Mobile = entity.Mobile,
                    Role = entity.PrimaryRole.ToString(),
                    ProfilePicture = entity.ProfilePicture,
                    Address = entity.Address,
                    RegisterDate = entity.RegisterDate                    
                };
            }
        }*/

        public static BTUser ToEntity(this RegisterDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            else
            {
                return new BTUser
                {
                    FirstName = StringHelpers.ToTitleCase( dto.FirstName!),
                    LasttName = StringHelpers.ToTitleCase(dto.LastName!),
                    Email = dto.Email.ToLower(),
                    Address = dto.Address,
                    Mobile = dto.Mobile,
                };
            }
        }

        public static Role ToEntity(this CreateRoleDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            else
            {
                return new Role
                {
                    RoleCode = dto.RoleCode!.ToUpper(),
                    RoleName = dto.RoleName,
                    Description = dto.Description
                };
            }
        }

    }
}
