#nullable enable

using System;
using System.Threading.Tasks;

public class DeferredResult<T>
{
    private readonly T initialValue;
    private readonly Task<T> task;

    public DeferredResult(Func<T> valueFactory, T initialValue)
    {
        this.initialValue = initialValue;
        this.task = Task.Run(valueFactory); 
    }

    /// <summary>
    /// Gets the value of the task if it is completed successfully; otherwise, returns the initial value.
    /// </summary>
    public T Value
    {
        get
        {
            if (!this.task.IsCompletedSuccessfully)
            {
                return this.initialValue;
            }

            if (this.task.IsFaulted)
            {
                // Handle or rethrow the exception as needed
                throw this.task.Exception?.InnerException ?? this.task.Exception!;
            }

            return this.task.Result;
        }
    }
}