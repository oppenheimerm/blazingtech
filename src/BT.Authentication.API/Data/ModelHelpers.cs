using BT.Shared.Domain;
using BT.Shared.Helpers;
using BT.Shared.Domain.DTO.Admin;
using BT.Shared.Domain.DTO.User;
using BT.Shared.Domain.DTO.Product;

namespace BT.Authentication.API.Data
{
    public static class ModelHelpers
    {
        public static BTUser ToEntity(this NewUserDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            else
            {
                return new BTUser
                {
                    Id = dto.Id,
                    JoinDate = dto.JoinDate,
                    FirstName = StringHelpers.ToTitleCase(dto.FirstName),
                    LasttName = StringHelpers.ToTitleCase(dto.LasttName),
                    Email = dto.Email.ToLower(),
                    Address = !string.IsNullOrEmpty(dto.Address) ?StringHelpers.ToTitleCase(dto.Address) : "",
                    PasswordHash = dto.Password,
                    Mobile = dto.Mobile,
                };
            }
        }

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

        public static Role ToEntity(this AddRoleDTO dto)
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


        public static Product ToEntity(this AddProductEntityDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            else
            {
                return new Product
                {
                    Title = StringHelpers.ToTitleCase(dto.Title!),
                    Description = dto.Description,
                    Price = dto.Price,
                    CategoryId = dto.CategoryId
                };
            }
        }

    }
}
