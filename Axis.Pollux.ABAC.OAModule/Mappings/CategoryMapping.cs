using Axis.Jupiter.Europa.Mappings;
using Axis.Pollux.ABAC.Auth;
using Axis.Pollux.Identity.OAModule.Mappings;

namespace Axis.Pollux.ABAC.OAModule.Mappings
{
    public class CategoryMapping: BaseComplexMap<Category>
    {
        public CategoryMapping()
        {
            Property(c => c.Name).HasColumnName("CategoryName")
                                 .HasMaxLength(250);
        }
    }
}
