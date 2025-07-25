using krov_nad_glavom_api.Middleware;
using Serilog;

namespace krov_nad_glavom_api.Commands
{
    public class ServeCommand : ICommand
    {
        public static readonly string Name = "serve";
        private readonly WebApplication _app;
        private readonly string _allowedSpecificOrigins;

        public ServeCommand(WebApplication app, string allowedSpecificOrigins)
        {
            _app = app;
            _allowedSpecificOrigins = allowedSpecificOrigins;
        }

        public int Invoke()
        {
            Console.WriteLine("Application Starting Up");
            _app.UseSwagger();
            _app.UseSwaggerUI();
            Console.WriteLine("Using swagger");

            _app.UseSerilogRequestLogging();

            _app.UseRouting();

            _app.UseCors(_allowedSpecificOrigins);
            _app.UseRateLimiter();

            _app.UseAuthentication();

            _app.UseAuthorization();

            // Register the custom middleware
            _app.UseMiddleware<TokenAuthenticationMiddleware>();

            _ = _app.UseEndpoints(endpoints =>
            {
                _ = endpoints.MapControllers();
            });
            _app.Run();
            return 1;
        }

        public Task<int> InvokeAsync()
        {
            return Task.FromResult(Invoke());
        }

        public bool MatchCommand(string command)
        {
            return command.Equals(Name);
        }

        public string GetCommandName() => Name;
    }
}