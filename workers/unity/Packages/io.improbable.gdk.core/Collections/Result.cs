using System;

namespace Improbable.Gdk.Core.Collections
{
    /// <summary>
    ///     A type to represent a result. Can either have a success value or an error, but not both.
    /// </summary>
    /// <typeparam name="T">The type of the success value.</typeparam>
    /// <typeparam name="E">The type of the error.</typeparam>
    public struct Result<T, E>
    {
        private T okValue;
        private E errorValue;

        private bool isOkay;

        /// <summary>
        ///     True if the result contains a success, false otherwise.
        /// </summary>
        public bool IsOkay => isOkay;

        /// <summary>
        ///     True if the result contains an error, false otherwise.
        /// </summary>
        public bool IsError => !isOkay;

        /// <summary>
        ///     Creates a result which contains a success value.
        /// </summary>
        /// <param name="value">The value of the result.</param>
        /// <returns>The result object.</returns>
        public static Result<T, E> Ok(T value)
        {
            return new Result<T, E>
            {
                okValue = value,
                isOkay = true
            };
        }

        /// <summary>
        ///     Creates a result which contains an error.
        /// </summary>
        /// <param name="error">The value of the error.</param>
        /// <returns>The result object.</returns>
        public static Result<T, E> Error(E error)
        {
            return new Result<T, E>
            {
                errorValue = error,
                isOkay = false
            };
        }

        /// <summary>
        ///     Attempts to get the success value from the result.
        /// </summary>
        /// <returns>The success value of the result.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the result contains an error.</exception>
        public T Unwrap()
        {
            if (!isOkay)
            {
                throw new InvalidOperationException("Cannot unwrap a result value which has an error.");
            }

            return okValue;
        }

        /// <summary>
        ///     Attempts to get the error from the result.
        /// </summary>
        /// <returns>The error from the result.</returns>
        /// <exception cref="InvalidOperationException">Thrown if result contains a success value.</exception>
        public E UnwrapError()
        {
            if (isOkay)
            {
                throw new InvalidOperationException("Cannot unwrap a result error which has a value.");
            }

            return errorValue;
        }
    }
}
