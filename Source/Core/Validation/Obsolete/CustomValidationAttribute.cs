// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.DataAnnotations;
using MultiLanguage.Core.Exception;
using MultiLanguage.Core.Resource;

namespace MultiLanguage.Core.Validation.Obsolete
{
    /// <summary>
    ///     Base class for all validation attributes.
    ///     <para>Override <see cref="IsValid(object, ValidationContext)" /> to implement validation logic.</para>
    /// </summary>
    [Obsolete]
    public abstract class CustomValidationAttribute : Attribute
    {
        #region Member Fields

        private volatile bool _hasBaseIsValid;

        #endregion

        #region All Constructors

        #endregion

        #region Internal Properties

        #endregion

        #region Protected Properties

        protected virtual string[] GetArgs(string memberName)
        {
            return new[]
            {
                memberName
            };
        }

        protected abstract string ErrorMessage { get; }

        #endregion

        #region Public Properties

        #endregion

        #region Private Methods

        #endregion

        #region Protected & Public Methods

        /// <summary>
        ///     Gets the value indicating whether or not the specified <paramref name="value" /> is valid
        ///     with respect to the current validation attribute.
        ///     <para>
        ///         Derived classes should not override this method as it is only available for backwards compatibility.
        ///         Instead, implement <see cref="IsValid(object, ValidationContext)" />.
        ///     </para>
        /// </summary>
        /// <remarks>
        ///     The preferred public entry point for clients requesting validation is the <see cref="GetValidationResult" />
        ///     method.
        /// </remarks>
        /// <param name="value">The value to validate</param>
        /// <returns><c>true</c> if the <paramref name="value" /> is acceptable, <c>false</c> if it is not acceptable</returns>
        /// <exception cref="InvalidOperationException"> is thrown if the current attribute is malformed.</exception>
        /// <exception cref="NotImplementedException">
        ///     is thrown when neither overload of IsValid has been implemented
        ///     by a derived class.
        /// </exception>
        public virtual bool IsValid(object value)
        {
            if (!_hasBaseIsValid)
                // track that this method overload has not been overridden.
                _hasBaseIsValid = true;

            // call overridden method.
            return IsValid(value, null) == CustomValidationResult.Success;
        }

        /// <summary>
        ///     Protected virtual method to override and implement validation logic.
        ///     <para>
        ///         Derived classes should override this method instead of <see cref="IsValid(object)" />, which is deprecated.
        ///     </para>
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">
        ///     A <see cref="ValidationContext" /> instance that provides
        ///     context about the validation operation, such as the object and member being validated.
        /// </param>
        /// <returns>
        ///     When validation is valid, <see cref="ValidationResult.Success" />.
        ///     <para>
        ///         When validation is invalid, an instance of <see cref="ValidationResult" />.
        ///     </para>
        /// </returns>
        /// <exception cref="InvalidOperationException"> is thrown if the current attribute is malformed.</exception>
        /// <exception cref="NotImplementedException">
        ///     is thrown when <see cref="IsValid(object, ValidationContext)" />
        ///     has not been implemented by a derived class.
        /// </exception>
        protected virtual CustomValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (_hasBaseIsValid)
                // this means neither of the IsValid methods has been overridden, throw.
                throw new MultiLanguageException(MultiLanguageErrorTexts.NotImplementedException);

            var result = CustomValidationResult.Success;

            // call overridden method.
            if (!IsValid(value))
                result = new CustomValidationResult(ErrorMessage, validationContext.MemberName,
                    GetArgs(validationContext.DisplayName));

            return result;
        }

        /// <summary>
        ///     Tests whether the given <paramref name="value" /> is valid with respect to the current
        ///     validation attribute without throwing a <see cref="ValidationException" />
        /// </summary>
        /// <remarks>
        ///     If this method returns <see cref="ValidationResult.Success" />, then validation was successful, otherwise
        ///     an instance of <see cref="ValidationResult" /> will be returned with a guaranteed non-null
        ///     <see cref="ValidationResult.ErrorMessage" />.
        /// </remarks>
        /// <param name="value">The value to validate</param>
        /// <param name="validationContext">
        ///     A <see cref="ValidationContext" /> instance that provides
        ///     context about the validation operation, such as the object and member being validated.
        /// </param>
        /// <returns>
        ///     When validation is valid, <see cref="ValidationResult.Success" />.
        ///     <para>
        ///         When validation is invalid, an instance of <see cref="ValidationResult" />.
        ///     </para>
        /// </returns>
        /// <exception cref="InvalidOperationException"> is thrown if the current attribute is malformed.</exception>
        /// <exception cref="ArgumentNullException">When <paramref name="validationContext" /> is null.</exception>
        /// <exception cref="NotImplementedException">
        ///     is thrown when <see cref="IsValid(object, ValidationContext)" />
        ///     has not been implemented by a derived class.
        /// </exception>
        public CustomValidationResult GetValidationResult(object value, ValidationContext validationContext)
        {
            if (validationContext == null) throw new ArgumentNullException(nameof(validationContext));

            var result = IsValid(value, validationContext);

            // If validation fails, we want to ensure we have a ValidationResult that guarantees it has an ErrorMessage
            if (result != null)
                if (string.IsNullOrEmpty(result.Message))
                    result = new CustomValidationResult(ErrorMessage, result.MemberName,
                        GetArgs(validationContext.DisplayName));

            return result;
        }

        /// <summary>
        ///     Validates the specified <paramref name="value" /> and throws <see cref="CustomValidationException" /> if it is not.
        ///     <para>
        ///         The overloaded <see cref="Validate(object, ValidationContext)" /> is the recommended entry point as it
        ///         can provide additional context to the <see cref="CustomValidationAttribute" /> being validated.
        ///     </para>
        /// </summary>
        /// <remarks>
        ///     This base method invokes the <see cref="IsValid(object)" /> method to determine whether or not the
        ///     <paramref name="value" /> is acceptable.  If <see cref="IsValid(object)" /> returns <c>false</c>, this base
        ///     method will invoke the <see cref="FormatErrorMessage" /> to obtain a localized message describing
        ///     the problem, and it will throw a <see cref="CustomValidationException" />
        /// </remarks>
        /// <param name="value">The value to validate</param>
        /// <param name="name">The string to be included in the validation error message if <paramref name="value" /> is not valid</param>
        /// <exception cref="CustomValidationException">
        ///     is thrown if <see cref="IsValid(object)" /> returns <c>false</c>.
        /// </exception>
        /// <exception cref="InvalidOperationException"> is thrown if the current attribute is malformed.</exception>
        public void Validate(object value, string name)
        {
            if (!IsValid(value)) throw new CustomValidationException(ErrorMessage, this, value);
        }

        /// <summary>
        ///     Validates the specified <paramref name="value" /> and throws <see cref="ValidationException" /> if it is not.
        /// </summary>
        /// <remarks>
        ///     This method invokes the <see cref="IsValid(object, ValidationContext)" /> method
        ///     to determine whether or not the <paramref name="value" /> is acceptable given the
        ///     <paramref name="validationContext" />.
        ///     If that method doesn't return <see cref="ValidationResult.Success" />, this base method will throw
        ///     a <see cref="ValidationException" /> containing the <see cref="ValidationResult" /> describing the problem.
        /// </remarks>
        /// <param name="value">The value to validate</param>
        /// <param name="validationContext">Additional context that may be used for validation.  It cannot be null.</param>
        /// <exception cref="ValidationException">
        ///     is thrown if <see cref="IsValid(object, ValidationContext)" />
        ///     doesn't return <see cref="ValidationResult.Success" />.
        /// </exception>
        /// <exception cref="InvalidOperationException"> is thrown if the current attribute is malformed.</exception>
        /// <exception cref="NotImplementedException">
        ///     is thrown when <see cref="IsValid(object, ValidationContext)" />
        ///     has not been implemented by a derived class.
        /// </exception>
        public void Validate(object value, ValidationContext validationContext)
        {
            if (validationContext == null) throw new ArgumentNullException(nameof(validationContext));

            var result = GetValidationResult(value, validationContext);

            if (result != null)
                // Convenience -- if implementation did not fill in an error message,
                throw new CustomValidationException(result, this, value);
        }

        #endregion
    }
}