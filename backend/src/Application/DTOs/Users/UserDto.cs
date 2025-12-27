namespace CRM.Application.DTOs.Users;

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string? Department { get; set; }
    public int? TeamId { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLogin { get; set; }
}

public class CreateUserRequest
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string? Department { get; set; }
    public int? TeamId { get; set; }
}

public class UpdateUserRequest
{
    public string FullName { get; set; } = string.Empty;
    public string? Department { get; set; }
    public string? Role { get; set; }
    public bool? IsActive { get; set; }
}


