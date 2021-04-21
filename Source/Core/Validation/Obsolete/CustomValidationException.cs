using System;
using System.Runtime.Serialization;

namespace MultiLanguage.Core.Validation.Obsolete
{
    [Obsolete]
    public class CustomValidationException : System.Exception
    {
        private CustomValidationResult? _validationResult;

        /// <summary>
        ///     Constructor that accepts a structured <see cref="ValidationResult" /> describing the problem.
        /// </summary>
        /// <param name="validationResult">The value describing the validation error</param>
        /// <param name="validatingAttribute">The attribute that triggered this exception</param>
        /// <param name="value">The value that caused the validating attribute to trigger the exception</param>
        public CustomValidationException(CustomValidationResult validationResult,
            CustomValidationAttribute? validatingAttribute,
            object? value)
            : this(validationResult.Message, validatingAttribute, value)
        {
            _validationResult = validationResult;
        }

        /// <summary>
        ///     Constructor that accepts an error message, the failing attribute, and the invalid value.
        /// </summary>
        /// <param name="errorMessage">The localized error message</param>
        /// <param name="validatingAttribute">The attribute that triggered this exception</param>
        /// <param name="value">The value that caused the validating attribute to trigger the exception</param>
        public CustomValidationException(string? message, CustomValidationAttribute? validatingAttribute, object? value)
            : base(message)
        {
            Value = value;
            ValidationAttribute = validatingAttribute;
        }

        /// <summary>
        ///     Default constructor.
        /// </summary>
        /// <remarks>The long form of this constructor is preferred because it gives better error reporting.</remarks>
        public CustomValidationException()
        {
        }

        /// <summary>
        ///     Constructor that accepts only a localized message
        /// </summary>
        /// <remarks>The long form of this constructor is preferred because it gives better error reporting.</remarks>
        /// <param name="message">The localized message</param>
        public CustomValidationException(string? message)
            : base(message)
        {
        }

        /// <summary>
        ///     Constructor that accepts a localized message and an inner exception
        /// </summary>
        /// <remarks>The long form of this constructor is preferred because it gives better error reporting</remarks>
        /// <param name="message">The localized error message</param>
        /// <param name="innerException">inner exception</param>
        public CustomValidationException(string? message, System.Exception? innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        ///     Constructor that takes a SerializationInfo.
        /// </summary>
        /// <param name="info">The SerializationInfo.</param>
        /// <param name="context">The StreamingContext.</param>
        protected CustomValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        ///     Gets the <see>ValidationAttribute</see> instance that triggered this exception.
        /// </summary>
        public CustomValidationAttribute? ValidationAttribute { get; }

        /// <summary>
        ///     Gets the <see cref="ValidationResult" /> instance that describes the validation error.
        /// </summary>
        /// <value>
        ///     This property will never be null.
        /// </value>
        public CustomValidationResult ValidationResult =>
            _validationResult ??= new CustomValidationResult(Message, "", new string[ushort.MinValue]);

        /// <summary>
        ///     Gets the value that caused the validating attribute to trigger the exception
        /// </summary>
        public object? Value { get; }
    }
}