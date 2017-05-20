using Axis.Jupiter.Europa;
using Axis.Pollux.Account.Objects;
using Axis.Pollux.Identity.OAModule.Mappings;

namespace Axis.Pollux.AccountManagement.OAModule.Mappings
{
    public class ContextVerificationMap: BaseMap<ContextVerification, long>
    {
        public ContextVerificationMap()
        {
            ///Conifgure Relationships.
            this.HasRequired(e => e.Target) //one way to owner
                .WithMany()
                .HasForeignKey(e => e.TargetId);

            this.Property(e => e.VerificationToken)
                .HasMaxLength(100)
                .IsIndex("VerificationContextName", true); //<-- do i need this to be unique?
        }
    }
}
