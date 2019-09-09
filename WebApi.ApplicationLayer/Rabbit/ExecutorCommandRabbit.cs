namespace WebApi.ApplicationLayer
{
    public abstract class ExecutorCommandRabbit<T> : IExecuteRabbit<T>
    {
        public abstract object Execute(T command);
    }
}
