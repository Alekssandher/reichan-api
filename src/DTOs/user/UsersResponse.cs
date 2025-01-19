public class UsersResponse {
    public List<UserDto> Users { get; set; }

    public UsersResponse() {
        Users = new List<UserDto>();
    }
}