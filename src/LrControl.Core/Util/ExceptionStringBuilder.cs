using System;
using System.Text;

namespace LrControl.Core.Util
{
    /// <summary>
    ///     Build a string with message and stacktrace of the provided exception,
    ///     included any nested inner exceptions.
    /// </summary>
    public class ExceptionStringBuilder
    {
        private readonly Exception _exception;
        private readonly Lazy<string> _stringHolder;

        public ExceptionStringBuilder(Exception exception)
        {
            _exception = exception;
            _stringHolder = new Lazy<string>(BuildString);
        }

        private string BuildString()
        {
            var buffer = new StringBuilder();
            VisitException(buffer, _exception);
            return buffer.ToString();
        }

        public override string ToString()
        {
            return _stringHolder.Value;
        }

        private static void VisitException(StringBuilder buffer, Exception e)
        {
            buffer.AppendLine($"Exception message: {e.Message}");
            buffer.AppendLine("Stacktrace:");
            buffer.AppendLine(e.StackTrace);

            // Aggregate exception?
            var aggregatedException = e as AggregateException;
            if (aggregatedException != null)
            {
                foreach (var innerException in aggregatedException.InnerExceptions)
                {
                    AppendLabel(buffer, "Inner exception:");
                    VisitException(buffer, innerException);
                }

                // Base exception?
                var baseException = aggregatedException.GetBaseException();
                AppendLabel(buffer, "Base exception");
                VisitException(buffer, baseException);
            }

            // Inner exception?
            if (e.InnerException != null)
            {
                AppendLabel(buffer, "Inner exception");
                VisitException(buffer, e.InnerException);
            }
        }

        private static void AppendLabel(StringBuilder buffer, string label)
        {
            buffer.AppendLine();
            buffer.AppendLine(label);
        }
    }
}