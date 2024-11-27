using BT.Shared.Domain;
using BT.Shared.Domain.DTO.Product;
using BT.Shared.Domain.DTO.User;
using BT.Shared.Helpers;

namespace BT.Admin.Helpers
{
    public static class ModelHelpers
    {
        public static EditUserDTO ToEntity(this BTUserLite dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            else
            {
                return new EditUserDTO
                {
                    Id = dto.Id,
                    FirstName = dto.FirstName,
                    LasttName = dto.LasttName,
                    Email = dto.Email,
                    Address = dto.Address,
                    ProfilePicture = dto.ProfilePicture,
                    Mobile = dto.Mobile,
                    AccountLockedOut = dto.AccountLockedOut
                };
            }
        }

        public static AddProductEntityDTO ToEntity(this AddProductDTO dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            else
            {
                return new AddProductEntityDTO
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
