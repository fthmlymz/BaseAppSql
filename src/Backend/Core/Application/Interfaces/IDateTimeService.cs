namespace Application.Interfaces
{
    public interface IDateTimeService
    {
        /// <summary>
        /// Gets the current UTC date and time.
        /// </summary>
        /// <returns>The current UTC date and time as a <see cref="DateTime"/> object.</returns>
        DateTime NowUtc { get; }
    }
}
