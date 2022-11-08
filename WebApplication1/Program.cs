using System.Diagnostics;
using Medallion.Threading;
using WebApplication1;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRedisDistributedLock(builder.Configuration);
var app = builder.Build();
app.MapGet("/lock", async (IDistributedLockProvider lockProvider) =>
{
    Console.WriteLine(Guid.NewGuid().ToString());
    Stopwatch sw = new Stopwatch();
    sw.Start();
    await using var handle =
        await lockProvider.TryAcquireLockAsync("mylockkey", TimeSpan.FromSeconds(5));
    if (handle != null)
    {
        await Task.Delay(TimeSpan.FromSeconds(500));
        return "ok";
    }

    sw.Stop();
    var x = sw.ElapsedMilliseconds;
    return "not got lock" + x;
});

app.Run();