using Axis.Sigma.Core;

namespace Axis.Pollux.ABAC.Auth
{
    public class Category: Luna.StructuredEnum
    {
        #region Enum Values
        public static readonly Category
        Subject,
        Action,
        Resource,
        Environment;
        #endregion

        public string Name { get; private set; }

        protected Category()
        { }
        static Category()
        {
            SynthesizeValues(name => new Category { Name = name });
        }
    }
}
