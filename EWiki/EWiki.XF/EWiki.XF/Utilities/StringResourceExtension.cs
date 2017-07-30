using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EWiki.XF.Utilities
{
    /// <summary>
    /// https://developer.xamarin.com/guides/xamarin-forms/advanced/localization/
    /// </summary>
    [ContentProperty("Text")]
    public class StringResourceExtension : IMarkupExtension
    {
        public string Text { get; set; }
        public string Params { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return "";

            var value = ResourceHelper.GetString(Text, Params);
            if (value == null)
            {
#if DEBUG
                throw new ArgumentException(
                    $"Key '{Text}' was not found in resources for culture '{App.CurrentCulture.Name}'.", nameof(Text));
#else
                 return Text; // HACK: returns the key, which GETS DISPLAYED TO THE USER
#endif
            }
            return value;
        }
    }
}
