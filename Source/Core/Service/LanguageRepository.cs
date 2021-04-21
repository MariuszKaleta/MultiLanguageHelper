using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MultiLanguage.Core.Service
{
    public class LanguageRepository : ILanguageRepository
    {
        #region Fields

        public string[] Elements;

        #endregion

        public LanguageRepository(LanguageRepositoryBuilder builder)
        {
            DefaultLanguage = builder.DefaultCulture ?? throw new NullReferenceException();
            Elements = builder.RegisteredElements.ToArray();
        }

        public CultureInfo DefaultLanguage { get; }

        public IEnumerable<string> DefaultTexts => Elements;
    }
}