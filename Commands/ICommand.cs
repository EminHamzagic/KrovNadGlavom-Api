namespace krov_nad_glavom_api.Commands
{
    public interface ICommand
    {
        public int Invoke();
        public Task<int> InvokeAsync();
        public bool MatchCommand(string command);
        public string GetCommandName();
    }
}