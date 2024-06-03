using Shared.Interfaces;

namespace Shared
{
    public class Result<T> : IResult<T>
    {
        /// <summary>
        /// Gets or sets the list of messages.
        /// </summary>
        /// <value>The list of messages.</value>
        public List<string> Messages { get; set; } = new List<string>();
      
        /// <summary>
        /// Gets or sets a value indicating whether the operation succeeded.
        /// </summary>
        /// <value>True if the operation succeeded, false otherwise.</value>
        public bool Succeeded { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>The data.</value>
        public T Data { get; set; }

        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        /// <value>The exception.</value>
        public Exception Exception { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>The code.</value>
        public int Code { get; set; }



        #region Non-Async Methods
        #region Success Methods
        /// <summary>
        /// Returns a successful Result with a message.
        /// </summary>
        /// <param name="message">The message to include in the Result.</param>
        /// <returns>A Result of type T with Succeeded set to true and a message.</returns>
        public static Result<T> Success(string message)
        {
            return new Result<T>
            {
                Succeeded = true,
                Messages = new List<string> { message }
            };
        }
        /// <summary>
        /// Returns a successful Result with data.
        /// </summary>
        /// <param name="data">The data to include in the Result.</param>
        /// <returns>A Result of type T with Succeeded set to true and data.</returns>
        public static Result<T> Success(T data)
        {
            return new Result<T>
            {
                Succeeded = true,
                Data = data
            };
        }
        /// <summary>
        /// Returns a successful Result with data and a message.
        /// </summary>
        /// <param name="data">The data to include in the Result.</param>
        /// <param name="message">The message to include in the Result.</param>
        /// <returns>A Result of type T with Succeeded set to true, data, and a message.</returns>
        public static Result<T> Success(T data, string message)
        {
            return new Result<T>
            {
                Succeeded = true,
                Messages = new List<string> { message },
                Data = data
            };
        }
        #endregion

        #region Failure Methods
        /// <summary>
        /// Returns a failed Result.
        /// </summary>
        /// <typeparam name="T">The type of the Result.</typeparam>
        /// <returns>A Result of type T with Succeeded set to false.</returns>
        public static Result<T> Failure()
        {
            return new Result<T>
            {
                Succeeded = false
            };
        }
        /// <summary>
        /// Returns a failed Result with a message.
        /// </summary>
        /// <typeparam name="T">The type of the Result.</typeparam>
        /// <param name="message">The message to include in the Result.</param>
        /// <returns>A Result of type T with Succeeded set to false and a message.</returns>
        public static Result<T> Failure(string message)
        {
            return new Result<T>
            {
                Succeeded = false,
                Messages = new List<string> { message }
            };
        }
        /// <summary>
        /// Returns a failed Result with messages.
        /// </summary>
        /// <typeparam name="T">The type of the Result.</typeparam>
        /// <param name="messages">The messages to include in the Result.</param>
        /// <returns>A Result of type T with Succeeded set to false and multiple messages.</returns>
        public static Result<T> Failure(List<string> messages)
        {
            return new Result<T>
            {
                Succeeded = false,
                Messages = messages
            };
        }
        /// <summary>
        /// Returns a failed Result with data.
        /// </summary>
        /// <typeparam name="T">The type of the Result.</typeparam>
        /// <param name="data">The data to include in the Result.</param>
        /// <returns>A Result of type T with Succeeded set to false and data.</returns>
        public static Result<T> Failure(T data)
        {
            return new Result<T>
            {
                Succeeded = false,
                Data = data
            };
        }
        /// <summary>
        /// Returns a failed Result with data and a message.
        /// </summary>
        /// <typeparam name="T">The type of the Result.</typeparam>
        /// <param name="data">The data to include in the Result.</param>
        /// <param name="message">The message to include in the Result.</param>
        /// <returns>A Result of type T with Succeeded set to false, data, and a message.</returns>
        public static Result<T> Failure(T data, string message)
        {
            return new Result<T>
            {
                Succeeded = false,
                Messages = new List<string> { message },
                Data = data
            };
        }
        /// <summary>
        /// Returns a failed Result with data and messages.
        /// </summary>
        /// <typeparam name="T">The type of the Result.</typeparam>
        /// <param name="data">The data to include in the Result.</param>
        /// <param name="messages">The messages to include in the Result.</param>
        /// <returns>A Result of type T with Succeeded set to false, data, and messages.</returns>
        public static Result<T> Failure(T data, List<string> messages)
        {
            return new Result<T>
            {
                Succeeded = false,
                Messages = messages,
                Data = data
            };
        }
        /// <summary>
        /// Returns a failed Result with an exception.
        /// </summary>
        /// <typeparam name="T">The type of the Result.</typeparam>
        /// <param name="exception">The exception to include in the Result.</param>
        /// <returns>A Result of type T with Succeeded set to false and an exception.</returns>
        public static Result<T> Failure(Exception exception)
        {
            return new Result<T>
            {
                Succeeded = false,
                Exception = exception
            };
        }
        #endregion
        #endregion

        #region Async Methods
        #region Success Methods
        /// <summary>
        /// Returns a task with a successful Result with a message.
        /// </summary>
        /// <param name="message">The message to include in the Result.</param>
        /// <returns>A Task that represents the asynchronous operation. The task result contains the successful Result.</returns>
        public static Task<Result<T>> SuccessAsync(string message)
        {
            return Task.FromResult(Success(message));
        }
        /// <summary>
        /// Returns a task with a successful Result with data.
        /// </summary>
        /// <param name="data">The data to include in the Result.</param>
        /// <returns>A Task that represents the asynchronous operation. The task result contains the successful Result.</returns>
        public static Task<Result<T>> SuccessAsync(T data)
        {
            return Task.FromResult(Success(data));
        }
        /// <summary>
        /// Returns a task with a successful Result with data and a message.
        /// </summary>
        /// <param name="data">The data to include in the Result.</param>
        /// <param name="message">The message to include in the Result.</param>
        /// <returns>A Task that represents the asynchronous operation. The task result contains the successful Result.</returns>
        public static Task<Result<T>> SuccessAsync(T data, string message)
        {
            return Task.FromResult(Success(data, message));
        }
        #endregion


        #region Failure Methods
        /// <summary>
        /// Returns a task with a failed Result.
        /// </summary>
        /// <typeparam name="T">The type of the Result.</typeparam>
        /// <returns>A Task that represents the asynchronous operation. The task result contains the failed Result.</returns>
        public static Task<Result<T>> FailureAsync()
        {
            return Task.FromResult(Failure());
        }
        /// <summary>
        /// Returns a task with a failed Result with a message.
        /// </summary>
        /// <typeparam name="T">The type of the Result.</typeparam>
        /// <param name="message">The message to be included in the Result.</param>
        /// <returns>A Task that represents the asynchronous operation. The task result contains the failed Result with the provided message.</returns>
        public static Task<Result<T>> FailureAsync(string message)
        {
            return Task.FromResult(Failure(message));
        }
        /// <summary>
        /// Returns a task with a failed Result with messages.
        /// </summary>
        /// <param name="messages">The list of messages to be included in the Result.</param>
        /// <returns>A Task that represents the asynchronous operation. The task result contains the failed Result with the provided messages.</returns>
        public static Task<Result<T>> FailureAsync(List<string> messages)
        {
            return Task.FromResult(Failure(messages));
        }
        /// <summary>
        /// Returns a task with a failed Result with data and a message.
        /// </summary>
        /// <param name="data">The data to be included in the Result.</param>
        /// <param name="message">The message to be included in the Result.</param>
        /// <returns>A Task that represents the asynchronous operation. The task result contains the failed Result with the provided data and message.</returns>
        public static Task<Result<T>> FailureAsync(T data)
        {
            return Task.FromResult(Failure(data));
        }
        public static Task<Result<T>> FailureAsync(T data, string message)
        {
            return Task.FromResult(Failure(data, message));
        }
        /// <summary>
        /// Returns a task with a failed Result with data and messages.
        /// </summary>
        /// <param name="data">The data to be included in the Result.</param>
        /// <param name="messages">The messages to be included in the Result.</param>
        /// <returns>A Task that represents the asynchronous operation. The task result contains the failed Result with the provided data and messages.</returns>
        public static Task<Result<T>> FailureAsync(T data, List<string> messages)
        {
            return Task.FromResult(Failure(data, messages));
        }
        /// <summary>
        /// Returns a task with a failed Result with an exception.
        /// </summary>
        /// <param name="exception">The exception to be included in the Result.</param>
        /// <returns>A Task that represents the asynchronous operation. The task result contains the failed Result with the provided exception.</returns>
        public static Task<Result<T>> FailureAsync(Exception exception)
        {
            return Task.FromResult(Failure(exception));
        }
        #endregion
        #endregion
    }
}
