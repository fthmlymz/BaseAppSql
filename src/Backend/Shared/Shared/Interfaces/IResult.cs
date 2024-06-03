namespace Shared.Interfaces
{
    /// <summary>
    /// Represents the result of an operation.
    /// </summary>
    /// <typeparam name="T">The type of the data in the result.</typeparam>
    public interface IResult<T>
    {
        /// <summary>
        /// Gets or sets the list of messages associated with the result.
        /// </summary>
        List<string> Messages { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the operation succeeded.
        /// </summary>
        bool Succeeded { get; set; }

        /// <summary>
        /// Gets or sets the data associated with the result.
        /// </summary>
        T Data { get; set; }

        /// <summary>
        /// Gets or sets the exception associated with the result (if any).
        /// </summary>
        Exception Exception { get; set; }

        /// <summary>
        /// Gets or sets the code associated with the result (if any).
        /// </summary>
        int Code { get; set; }
    }
}