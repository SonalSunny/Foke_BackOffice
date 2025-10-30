using FOKE.Startup;

var builder = WebApplication.CreateBuilder(args);
var startup = new Startup(builder.Configuration); // My custom startup class.
startup.ConfigureServices(builder.Services);
var app = builder.Build();
startup.Configure(app, app.Environment);
app.MapRazorPages();

app.Run();