using AutoMapper;
using BCrypt.Net;
using eBoardAPI.Common;
using eBoardAPI.Helpers;
using eBoardAPI.Interfaces.Repositories;
using eBoardAPI.Interfaces.Services;
using eBoardAPI.Models.Auth;
using eBoardAPI.Models.Class;
using eBoardAPI.Models.Parent;
using eBoardAPI.Models.Student;

namespace eBoardAPI.Services;

public class ParentService(IParentRepository parentRepository, IMapper mapper) : IParentService
{
    public async Task<Result<ParentInfoDto>> GetByIdAsync(Guid id)
    {
        if(id == Guid.Empty)
        {
            return Result<ParentInfoDto>.Failure("Id cannot be empty");
        }

        var resultGet = await parentRepository.GetByIdAsync(id);

        if(resultGet.IsSuccess == false)
        {
            return Result<ParentInfoDto>.Failure(resultGet.ErrorMessage ?? "Error retrieving parent");
        }

        var parentDto = mapper.Map<ParentInfoDto?>(resultGet.Value);
        return parentDto == null
            ? Result<ParentInfoDto>.Failure("Không tìm thấy phụ huynh")
            : Result<ParentInfoDto>.Success(parentDto);
    }

    public async Task<Result<ParentInfoDto>> UpdateAsync(Guid id, UpdateParentInfoDto updateParentInfoDto)
    {
        // get existing parent
        var resultGet = await parentRepository.GetByIdAsync(id);
        var existingParent = resultGet.IsSuccess ? resultGet.Value : null;

        if (existingParent == null)
        {
            return Result<ParentInfoDto>.Failure("Không tìm thấy phụ huynh");
        }
        
        if (updateParentInfoDto.Address != null)
            existingParent.Address = updateParentInfoDto.Address;
        if (updateParentInfoDto.Email != null)
            existingParent.Email = updateParentInfoDto.Email;
        if (updateParentInfoDto.FullName != null)
            existingParent.FullName = updateParentInfoDto.FullName;
        if (updateParentInfoDto.HealthCondition != null)
            existingParent.HealthCondition = updateParentInfoDto.HealthCondition;
        
        //check phone number
        if (updateParentInfoDto.PhoneNumber != null && updateParentInfoDto.PhoneNumber != existingParent.PhoneNumber)
        {
            var phoneNumberExistsResult = await parentRepository.GetByPhoneNumberAsync(updateParentInfoDto.PhoneNumber);
            if (phoneNumberExistsResult.IsSuccess)
            {
                return Result<ParentInfoDto>.Failure("Số điện thoại đã được sử dụng bởi phụ huynh khác");
            }
            existingParent.PhoneNumber = updateParentInfoDto.PhoneNumber;
        }
        

        // Update parent in repository
        var result = await parentRepository.Update(existingParent);

        if (!result.IsSuccess)
        {
            return Result<ParentInfoDto>.Failure("Cập nhật thông tin phụ huynh thất bại");
        }

        var updatedParent = mapper.Map<ParentInfoDto>(existingParent);
        return Result<ParentInfoDto>.Success(updatedParent);
    }

    public async Task<IEnumerable<ParentInfoDto>> CreateAccountForParent(List<Guid> parentIds)
    {
        var parents = await parentRepository.GetParentsByIdsAsync(parentIds);
        var generatePassword = RandomGeneratorHelper.GenerateRandomPassword();
        foreach (var parent in parents)
        {
            parent.GeneratedPassword = generatePassword;
            parent.PasswordHash = BCrypt.Net.BCrypt.HashPassword(generatePassword);
        }    
        await parentRepository.UpdateRangeParentsAsync(parents);
        var dtos = mapper.Map<List<ParentInfoDto>>(parents);
        return dtos;
    }

    public async Task<IEnumerable<ParentInfoDto>> GetParentNotCreateAccountByClassId(Guid classId, int pageNumber = 1, int pageSize = 20)
    {
        var parents = await parentRepository.GetParentNotCreateAccountByClassId(classId, pageNumber, pageSize);
        var dtos = mapper.Map<List<ParentInfoDto>>(parents);
        return dtos;
    }

    public async Task<IEnumerable<ParentInfoDto>> GetParentCreateAccountByClassId(Guid classId, int pageNumber = 1, int pageSize = 20)
    {
        var parents = await parentRepository.GetParentCreateAccountByClassId(classId, pageNumber, pageSize);
        var dtos = mapper.Map<List<ParentInfoDto>>(parents);
        return dtos;
    }

    public async Task<IEnumerable<ChildInClassDto>> GetChildInClassesByClassId(Guid parentId, int pageNumber, int pageSize)
    {
        var studentWithClasses = await parentRepository.GetStudentsWithClassesByParentIdAsync(parentId);
        var dtos = studentWithClasses.Select(x => new ChildInClassDto
        {
            StudentInfo = mapper.Map<StudentInfoDto>(x.Item1),
            ClassInfo = mapper.Map<ClassInfoDto>(x.Item2)
        });
        
        return dtos;
    }

    public async Task<Result> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
    {
        if (changePasswordDto.NewPassword != changePasswordDto.ConfirmPassword)
            return Result.Failure("Mật khẩu mới và xác nhận mật khẩu không khớp");

        var parentResult = await parentRepository.GetByIdAsync(changePasswordDto.Id);
        if (!parentResult.IsSuccess || parentResult.Value == null)
        {
            return Result.Failure("Không tồn tại phụ huynh có Id này");
        }

        var parent = parentResult.Value;
        var check = BCrypt.Net.BCrypt.Verify(changePasswordDto.OldPassword, parent.PasswordHash);
        if (!check)
            return Result.Failure("Sai mật khẩu");
        parent.PasswordHash = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);
        var result = await parentRepository.Update(parent);
        return result;
    }
}