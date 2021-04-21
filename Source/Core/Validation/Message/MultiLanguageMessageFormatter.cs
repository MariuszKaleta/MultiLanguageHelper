using System.Linq;
using System.Text.Json;
using FluentValidation.Internal;
using MultiLanguage.Core.ViewModel.Validation;

namespace MultiLanguage.Core.Validation.Message
{
    public class MultiLanguageMessageFormatter : MessageFormatter
    {
        public override string BuildMessage(string messageTemplate)
        {
            var result = new ValidationRuleResultViewModel
            {
                ErrorCode = messageTemplate,
                Args = PlaceholderValues.ToDictionary(x => x.Key, x => x.Value?.ToString())
            };

            return JsonSerializer.Serialize(result);
        }
    }
}