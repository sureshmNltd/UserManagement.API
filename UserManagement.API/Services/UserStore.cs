using System.Text.Json;
using UserManagement.API.Models;

namespace UserManagement.API.Services;

// A lightweight JSON-file store for CI/CD practice, not a production database.
public sealed class UserStore
{
    private readonly object _gate = new();
    private readonly string _path;
    private readonly JsonSerializerOptions _json = new() { WriteIndented = true };
    private List<User> _users;

    public UserStore(IWebHostEnvironment environment)
    {
        var folder = Path.Combine(environment.ContentRootPath, "data");
        Directory.CreateDirectory(folder);
        _path = Path.Combine(folder, "users.json");
        _users = File.Exists(_path)
            ? JsonSerializer.Deserialize<List<User>>(File.ReadAllText(_path)) ?? new()
            : new List<User>
            {
                new() { Id = 1, Name = "Alex Johnson", Email = "alex@example.com", Department = "Engineering" },
                new() { Id = 2, Name = "Priya Sharma", Email = "priya@example.com", Department = "QA" }
            };
        if (!File.Exists(_path)) Save();
    }

    public List<User> All() { lock (_gate) return _users.Select(Clone).OrderBy(x => x.Id).ToList(); }
    public User? Get(int id) { lock (_gate) return _users.Where(x => x.Id == id).Select(Clone).FirstOrDefault(); }

    public (User? User, bool Duplicate) Add(UserInput input)
    {
        lock (_gate)
        {
            if (ExistsEmail(input.Email)) return (null, true);
            var user = new User { Id = _users.Count == 0 ? 1 : _users.Max(x => x.Id) + 1,
                Name = input.Name.Trim(), Email = input.Email.Trim(), Department = input.Department.Trim() };
            _users.Add(user);
            Save();
            return (Clone(user), false);
        }
    }

    public (User? User, bool Duplicate) Update(int id, UserInput input)
    {
        lock (_gate)
        {
            var user = _users.FirstOrDefault(x => x.Id == id);
            if (user is null) return (null, false);
            if (ExistsEmail(input.Email, id)) return (null, true);
            user.Name = input.Name.Trim();
            user.Email = input.Email.Trim();
            user.Department = input.Department.Trim();
            Save();
            return (Clone(user), false);
        }
    }

    public bool Delete(int id)
    {
        lock (_gate)
        {
            var user = _users.FirstOrDefault(x => x.Id == id);
            if (user is null) return false;
            _users.Remove(user);
            Save();
            return true;
        }
    }

    private bool ExistsEmail(string email, int? exceptId = null) =>
        _users.Any(x => x.Id != exceptId && string.Equals(x.Email, email.Trim(), StringComparison.OrdinalIgnoreCase));
    private void Save() => File.WriteAllText(_path, JsonSerializer.Serialize(_users, _json));
    private static User Clone(User u) => new() { Id = u.Id, Name = u.Name, Email = u.Email, Department = u.Department };
}
