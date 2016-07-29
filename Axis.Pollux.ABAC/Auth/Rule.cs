using Axis.Pollux.Identity.Principal;

namespace Axis.Pollux.ABAC.Auth
{
    public class Rule: PolluxEntity<int>
    {
        public string Name
        {
            get { return get<string>(); }
            set { set(ref value); }
        }
        public string Title
        {
            get { return get<string>(); }
            set { set(ref value); }
        }

        #region Conditions
        public string SubjectCondition
        {
            get { return get<string>(); }
            set { set(ref value); }
        }
        public string ActionCondition
        {
            get { return get<string>(); }
            set { set(ref value); }
        }
        public string ResourceCondition
        {
            get { return get<string>(); }
            set { set(ref value); }
        }
        public string EnvironmentCondition
        {
            get { return get<string>(); }
            set { set(ref value); }
        }
        #endregion
    }
}
