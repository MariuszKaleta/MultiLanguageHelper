using System.Collections.Generic;
using System.Globalization;

namespace MultiLanguage.Core.Service
{
    public interface ILanguageRepository
    {
        CultureInfo DefaultLanguage { get; }

        IEnumerable<string> DefaultTexts { get; }
    }
}