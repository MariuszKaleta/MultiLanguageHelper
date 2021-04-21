using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace MultiLanguage.Core.Validation.Obsolete
{
    /// <summary>
    ///     Container class for the results of a validation request.
    ///     <para>
    ///         Use the static <see cref="ValidationResult.Success" /> to represent successful validation.
    ///     </para>
    /// </summary>
    /// <seealso cref="ValidationAttribute.GetValidationResult" />
    [Obsolete]
    public class CustomValidationResult
    {
        #region Member Fields

        /// <summary>
        ///     Gets a <see cref="ValidationResult" /> that indicates Success.
        /// </summary>
        /// <remarks>
        ///     The <c>null</c> value is used to indicate success.  Consumers of <see cref="ValidationResult" />s
        ///     should compare the values to <see cref="ValidationResult.Success" /> rather than checking for null.
        /// </remarks>
        public static readonly CustomValidationResult? Success;

        #endregion

        #region All Constructors

        /// <summary>
        ///     Constructor that accepts an error message as well as a list of member names involved in the validation.
        ///     This error message would override any error message provided on the <see cref="ValidationAttribute" />.
        /// </summary>
        /// <param name="message">
        ///     The user-visible error message.  If null, <see cref="ValidationAttribute.GetValidationResult" />
        ///     will use <see cref="ValidationAttribute.FormatErrorMessage" /> for its error message.
        /// </param>
        /// <param name="memberNames">
        ///     The list of member names affected by this result.
        ///     This list of member names is meant to be used by presentation layers to indicate which fields are in error.
        /// </param>
        public CustomValidationResult(string? message, string memberName, string[] memberArgs)
        {
            if (string.IsNullOrEmpty(memberName)) throw new NullReferenceException();

            Message = message;
            MemberName = memberName;
            MemberArgs = memberArgs ?? new string[ushort.MinValue];
        }

        /// <summary>
        ///     Constructor that creates a copy of an existing ValidationResult.
        /// </summary>
        /// <param name="validationResult">The validation result.</param>
        /// <exception cref="System.ArgumentNullException">The <paramref name="validationResult" /> is null.</exception>
        protected CustomValidationResult(CustomValidationResult validationResult)
        {
            if (validationResult == null) throw new ArgumentNullException(nameof(validationResult));


            Message = validationResult.Message;
            MemberName = validationResult.MemberName;
            MemberArgs = validationResult.MemberArgs;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the collection of member names affected by this result.  The collection may be empty but will never be null.
        /// </summary>
        public string[] MemberArgs { get; }

        /// <summary>
        ///     Gets the error message for this result.  It may be null.
        /// </summary>
        public string? Message { get; }


        public string MemberName { get; }

        #endregion

        #region Methods

        /// <summary>
        ///     Override the string representation of this instance, returning
        ///     the <see cref="Message" /> if not <c>null</c>, otherwise
        ///     the base <see cref="object.ToString" /> result.
        /// </summary>
        /// <remarks>
        ///     If the <see cref="Message" /> is empty, it will still qualify
        ///     as being specified, and therefore returned from <see cref="ToString" />.
        /// </remarks>
        /// <returns>
        ///     The <see cref="Message" /> property value if specified,
        ///     otherwise, the base <see cref="object.ToString" /> result.
        /// </returns>
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Message)) return Message;

            return base.ToString();
        }

        public string CombineText => JsonSerializer.Serialize(new
        {
            MemberName,
            Message,
            MemberArgs
        }); // $"{.Message} {MemberName} {string.Join(',', MemberArgs)}";

        #endregion Methods

        #region Static Methods

        #endregion
    }
}