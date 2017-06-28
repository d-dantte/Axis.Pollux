using Axis.Pollux.Identity.OAModule.Entities;
using Axis.Pollux.Identity.Principal;

namespace Axis.Pollux.Identity.OAModule.Mappings
{
    public abstract class BaseMap<KeyType, EType, MType> : Jupiter.Europa.Mappings.BaseEntityMapConfig<MType, EType>
    where EType: PolluxEntity<KeyType>, new()
    where MType: PolluxModel<KeyType>, new()
    {
        protected BaseMap(bool useDefaultTable)
        {
            if (useDefaultTable) ToTable(typeof(MType).Name);

            //configure the primary key
            HasKey(e => e.UniqueId);
        }

        protected BaseMap():
        this(true)
        { }
    }
}
