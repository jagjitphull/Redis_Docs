using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using RedisAndEntityFrameworkInWebApi.Data;

//Below the line 
var builder = WebApplication.CreateBuilder(args);      //add Redis and SQL Server to the service collection:

builder.Services.AddStackExchangeRedisCache(options =>
{
   options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

builder.Services.AddDbContext<KeyAndValueContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("MyDatabase")));

// Below the line 
var app = builder.Build();                   // add the following:
using(var scope = app.Services.CreateScope())
{
   var keyAndValueContext = scope.ServiceProvider.GetRequiredService<KeyAndValueContext>();
   await keyAndValueContext.Database.EnsureDeletedAsync();
   await keyAndValueContext.Database.EnsureCreatedAsync();
   await keyAndValueContext.SeedAsync();
}
var slidingExpiration = new DistributedCacheEntryOptions
{
    SlidingExpiration = TimeSpan.FromSeconds(5)
};

app.MapGet("/{key}", async (
    string key,
    IDistributedCache distributedCache,
    KeyAndValueContext keyAndValueContext
) =>
{
    string? value = distributedCache.GetString(key);
    if (value is not null)
    {
        return $"Key:{key}, Value:{value}. Source:Redis";
    }

    var keyAndValue = await keyAndValueContext.FindAsync<KeyAndValue>(key);

    if (keyAndValue is null)
    {
        return $"{key} not found";
    }

    await distributedCache.SetStringAsync(key, keyAndValue.Value, slidingExpiration);
    return $"Key:{key}, Value:{keyAndValue.Value}. Source:MSSQL";
});
