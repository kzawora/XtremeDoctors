using Microsoft.Extensions.Localization;
using System;
using System.Reflection;

namespace XtremeDoctors.Resources
{
    internal class SharedResource
    {
    }
    public class SharedViewLocalizer
    {
        private readonly IStringLocalizer _localizer;

        public SharedViewLocalizer(IStringLocalizerFactory factory)
        {
            Type type = typeof(SharedResource);
            AssemblyName assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
            _localizer = factory.Create("SharedResource", assemblyName.Name);
        }

        public LocalizedString this[string key] => _localizer[key];

        public LocalizedString GetLocalizedString(string key)
        {
            return _localizer[key];
        }
    }
}
