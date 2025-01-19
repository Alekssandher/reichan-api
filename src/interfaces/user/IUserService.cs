public interface IUserService
{
    Task<UsersResponse> GetAllAsync();
    Task CreateAsync(CreateUserDto userDto);
}

