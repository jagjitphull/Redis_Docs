//Create the dir Data, then create this file.

namespace RedisAndEntityFrameworkInWebApi.Data;

public static class Seeder
{
    public static async Task SeedAsync(this KeyAndValueContext keyAndValueContext)
    {
        if (!keyAndValueContext.KeyAndValues.Any())
        {
            int i = 1;
            foreach (string letter in Alphabet)
            {
                keyAndValueContext.Add(new KeyAndValue 
                { 
                    Key = letter, 
                    Value = $"The letter \"{letter}\" is at position {i++} in the alphabet" 
                });
            }

            await keyAndValueContext.SaveChangesAsync();
        }
    }

    public static IEnumerable<string> Alphabet
    {
        get
        {
            for (char letter = 'a'; letter <= 'z'; letter++)
            {
                yield return letter.ToString();
            }
        }
    }
}
