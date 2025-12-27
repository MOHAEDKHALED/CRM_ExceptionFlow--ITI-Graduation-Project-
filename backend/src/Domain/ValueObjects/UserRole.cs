using CRM.Domain.Common;

namespace CRM.Domain.ValueObjects;

public class UserRole : ValueObject
{
    public static readonly UserRole Admin = new("Admin");
    public static readonly UserRole Manager = new("Manager");
    public static readonly UserRole Employee = new("Employee");
    public static readonly UserRole ITSupport = new("ITSupport");
    
    public string Value { get; private set; }
    
    private UserRole(string value)
    {
        Value = value;
    }
    
    public static UserRole FromString(string role)
    {
        return role switch
        {
            "Admin" => Admin,
            "Manager" => Manager,
            "Employee" => Employee,
            "ITSupport" => ITSupport,
            _ => throw new ArgumentException($"Invalid role: {role}")
        };
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    public override string ToString() => Value;
}


