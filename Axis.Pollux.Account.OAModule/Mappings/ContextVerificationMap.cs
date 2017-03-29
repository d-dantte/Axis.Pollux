using Axis.Pollux.Account.Objects;
using Axis.Pollux.Identity.OAModule.Mappings;

namespace Axis.Pollux.Account.OAModule.Mappings
{
    public class ContextVerificationMap: BaseMap<ContextVerification, long>
    {
        public ContextVerificationMap()
        {
            ///Conifgure Relationships.
            this.HasRequired(e => e.Target) //one way to owner
                .WithMany()
                .HasForeignKey(e => e.TargetId);            
        }
    }
}
