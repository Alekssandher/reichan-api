public interface IUserService
{
    Task<List<UserDto>> GetAllAsync();
    Task CreateAsync(CreateUserDto userDto);
}

