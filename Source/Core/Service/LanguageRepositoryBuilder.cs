using System.Collections.Generic;
using System.Globalization;
using MultiLanguage.Core.Exception;
using MultiLanguage.Core.Extension;
using MultiLanguage.Core.Resource;

namespace MultiLanguage.Core.Service
{
    public class LanguageRepositoryBuilder
    {
        public LanguageRepositoryBuilder()
        {
            this.Register(typeof(MultiLanguageErrorTexts)).Register(typeof(ValidatorTexts));
        }

        #region Field

        public CultureInfo DefaultCulture = new CultureInfo("en");

        private readonly List<string> Elements = new List<string>();

        #endregion

        #region Methods

        public LanguageRepositoryBuilder Register(string element)
        {
            if (Elements.Exists(x => x == element)) throw new MultiLanguageException(MultiLanguageErrorTexts.ElementAlreadyExist, element);


            Elements.Add(element);

            return this;
        }

        public IEnumerable<string> RegisteredElements => Elements;

        #endregion
    }
}