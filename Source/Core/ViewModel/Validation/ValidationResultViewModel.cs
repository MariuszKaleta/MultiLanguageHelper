using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using FluentValidation.Results;

namespace MultiLanguage.Core.ViewModel.Validation
{
    public class ValidationResultViewModel
    {
        public ValidationResultViewModel(IEnumerable<ValidationFailure> list)
        {
            foreach (var failure in list)
            {
               Add(failure);
            }
        }

        public ValidationResultViewModel()
        {
        }

        public List<FieldValidationResultViewModel> Errors { get; set; } = new List<FieldValidationResultViewModel>();

        #region Method

        public void Add(ValidationFailure failure)
        {
            var rule = IsJson(failure.ErrorMessage)
                ? JsonSerializer.Deserialize<ValidationRuleResultViewModel>(failure.ErrorMessage)
                : new ValidationRuleResultViewModel
                {
                    ErrorCode = failure.ErrorMessage,
                    Args = failure.FormattedMessagePlaceholderValues
                        .ToDictionary(x => x.Key, x => x.Value?.ToString())
                };

            var field = Errors.FirstOrDefault(x => x.FieldName == failure.PropertyName);

            if (field == null)
            {
                field = new FieldValidationResultViewModel
                {
                    FieldName = failure.PropertyName
                };

                Errors.Add(field);
            }

            field.Rules.Add(rule);
        }

        private static bool IsJson(string text)
        {
            return text.StartsWith("{") && text.EndsWith("}") || text.StartsWith("[") && text.EndsWith("]");
        }

        #endregion


    }
}