// See https://aka.ms/new-console-template for more information

using System.Text.Json;

Console.WriteLine("Hello, World!");

string filePath = "values.json";


using (var filemanager = new JsonFileManager(filePath))
{
    List<User> userList = new List<User>()
    {
        new User("John", "Doe"),
        new User("Alice", "Smith"),
        new User("Bob", "Johnson"),
        new User("Emily", "Davis"),
        new User("Michael", "Brown")
    };
    await filemanager.WriteJsonAsync(userList);
    var obj = await filemanager.ReadJsonAsync<List<User>>();
    foreach (var usr in obj)
    {
        Console.WriteLine($"{usr.name} {usr.surname}");
    }
}

public class JsonFileManager : IDisposable
{
    private readonly string _filePath;
    private FileStream _fileStream;

    public JsonFileManager(string filePath)
    {
        _filePath = filePath;
        _fileStream = new FileStream(_filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
    }

    public async Task WriteJsonAsync<T>(T data)
    {
        await _fileStream.FlushAsync();
        _fileStream.Seek(0, SeekOrigin.Begin);
        await JsonSerializer.SerializeAsync(_fileStream, data, data.GetType());
    }

    public async Task<T> ReadJsonAsync<T>()
    {
        await _fileStream.FlushAsync();
        _fileStream.Seek(0, SeekOrigin.Begin);
        return await JsonSerializer.DeserializeAsync<T>(_fileStream);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _fileStream.Dispose();
        }
    }

    ~JsonFileManager()
    {
        Dispose(false);
    }
}

class User
{
    public string name { get; }
    public string surname { get; }

    public User(string name, string surname)
    {
        this.name = name;
        this.surname = surname;
    }
}