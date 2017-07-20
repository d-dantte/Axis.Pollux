using Axis.Pollux.Identity.Principal;

namespace Axis.Pollux.Identity
{
    public interface IUserOwned
    {
        User Owner { get; set; }
    }

    public interface IUserTargeted
    {
        User Target { get; set; }
    }

    public interface IUserIdentified
    {
        User User { get; set; }
    }
}
