// See https://aka.ms/new-console-template for more information

using System.Text.Json;

Console.WriteLine("Hello, World!");

string filePath = "values.json";

var user = new User("Rupert", "Tanner");

await WriteFile(user);
var str = await ReadFile<User>();
Console.WriteLine(str.name);
Console.WriteLine(str.surname);

async Task WriteFile<T>(T data)
{
    
    using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
    {
        await JsonSerializer.SerializeAsync(stream, data, new JsonSerializerOptions());
    }
}

async Task<T> ReadFile<T>()
{
    using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
    {
        return await JsonSerializer.DeserializeAsync<T>(stream);
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