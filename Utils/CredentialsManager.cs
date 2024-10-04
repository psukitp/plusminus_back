using System.Text.Json;
using plusminus.Dtos.Users;

namespace plusminus.Utils;

public class CredentialsManager
{
    private readonly string _filePath;
    
    public CredentialsManager(string filePath)
    {
        _filePath = filePath;
    }

    public async Task<Credentials> LoadCredentialsAsync()
    {
        if (!File.Exists(_filePath))
            return null;

        var json = await File.ReadAllTextAsync(_filePath);
        var res = JsonSerializer.Deserialize<Credentials>(json);
        return res;
    }
}