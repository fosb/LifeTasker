public class Command
{
    private readonly Action<object> _execute;

    public Command(Action<object> execute)
    {
        _execute = execute;
    }

    public void Execute(object parameter)
    {
        _execute(parameter);
    }
}