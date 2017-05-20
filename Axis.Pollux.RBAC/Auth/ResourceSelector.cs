using System;

namespace Axis.Pollux.RBAC.Auth
{
    public class ResourceSelector
    {
        private string[] _selectorParts = null;

        public ResourceSelector(string selector)
        {
            if (selector == null) throw new ArgumentNullException();
            else if ((selector = selector.Trim()) == "") throw new Exception("Invalid selector");
            else if (selector.Contains("*") && !selector.EndsWith("*")) throw new Exception("Invalid selector format");
            else _selectorParts = selector.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public bool Match(string resourcePath)
        {
            if (_selectorParts == null || string.IsNullOrWhiteSpace(resourcePath)) return false;
            else
            {
                var _rparts = resourcePath.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                if (_rparts.Length < _selectorParts.Length) return false;

                for(int cnt=0; cnt<_selectorParts.Length; cnt++)
                {
                    if (_selectorParts[cnt].Trim() == "*") return true;
                    else if (_selectorParts[cnt].Trim() != _rparts[cnt].Trim()) return false;
                }
                return true;
            }
        }
    }
}