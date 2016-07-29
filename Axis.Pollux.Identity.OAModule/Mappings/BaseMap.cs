using Axis.Pollux.Identity.Principal;
using System.Data.Entity.ModelConfiguration;

namespace Axis.Pollux.Identity.OAModule.Mappings
{
    public abstract class BaseMap<EType, KeyType> : Jupiter.Europa.Mappings.BaseMap<EType>
    where EType: PolluxEntity<KeyType>
    {
        protected BaseMap(bool useDefaultTable)
        : base(useDefaultTable)
        {
            //configure the primary key
            this.HasKey(e => e.EntityId);
        }

        protected BaseMap():
        this(true)
        { }
    }
}
