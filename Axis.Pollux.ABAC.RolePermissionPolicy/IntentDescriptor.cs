using Axis.Luna.Extensions;
using System;
using System.Text.RegularExpressions;

namespace Axis.Pollux.ABAC.RolePermissionPolicy
{
    public class IntentDescriptor
    {
        private static Regex ActionNamePattern = new Regex("[A-Za-z][A-Za-z0-9\\-]*", RegexOptions.IgnoreCase);

        public IntentDescriptor(string intent)
        : this(intent.Split('@')[0], intent.Split('@')[1])
        {
        }

        public IntentDescriptor(string resource, string action)
        {
            if ((IsOpenEnded = resource.EndsWith("*"))) resource = resource.TrimEnd("*").Trim("/");
            Uri = new Uri(resource);

            if (!ActionNamePattern.IsMatch(action)) throw new Exception("invalid action name");
            else Action = action;
        }

        public bool IsOpenEnded { get; private set; }

        /// <summary>
        /// Urls with no scheme limitation. Thus abac://domain/something/here/* is a valid resource descriptor
        /// </summary>
        public string Resource => Uri.ToString();
        public Uri Uri { get; private set; }

        /// <summary>
        /// Simple c# identifier names, with the exception that "-" is allowed
        /// </summary>
        public string Action { get; private set; }

        public bool IsBaseOf(IntentDescriptor descriptor)
        => Equals(descriptor) ||
           (IsOpenEnded && Uri.IsBaseOf(descriptor.Uri));

        public override int GetHashCode() => this.PropertyHash();
        public override bool Equals(object obj)
        {
            var _ = obj as IntentDescriptor;
            return Resource?.Equals(_?.Resource) == true &&
                   Action?.Equals(_?.Action) == true &&
                   GetHashCode().Equals(_?. GetHashCode());
        }
    }
}
