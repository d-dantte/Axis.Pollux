using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Pollux.Identity.Services.Queries;
using Moq;
using Axis.Jupiter.Commands;
using Axis.Luna.Operation;
using Axis.Pollux.Identity.Services;
using Axis.Luna.Extensions;

namespace Axis.Pollux.Identity.Test.DefaultServices
{
    [TestClass]
    public class UserManagerTest
    {
        #region Biodata
        [TestMethod] public void UpdateBioData()
        {
            var testUser = new Principal.User
            {
                UserId = "test-user@test.test"
            };
            var bio = new Principal.BioData
            {
                Dob = DateTime.Now,
                FirstName = "Dev",
                Gender = Principal.Gender.Female,
                LastName = "Fasbender",
                MiddleName = "Toroid",
                Nationality = "Blue Nowhere",
                Owner = testUser,
                StateOfOrigin = "Blue Somewhere",
                UniqueId = 1
            };
            var persisted = bio;

            var query = new Mock<IUserQuery>();
            var pcommand = new Mock<IPersistenceCommands>();
            var cxt = new Mock<IUserContext>();

            query
                .Setup(q => q.GetBioData(It.IsAny<string>()))
                .Returns<string>(_uid =>
                {
                    if (_uid != testUser.UserId) return null;
                    else return bio;
                });

            pcommand
                .Setup(p => p.Update(It.IsAny<Principal.BioData>()))
                .Returns<Principal.BioData>(_bio => LazyOp.Try(() => persisted = _bio));

            cxt.Setup(c => c.User()).Returns(testUser);

            var userManager = new UserManager(cxt.Object, query.Object, pcommand.Object);

            //make sure that nulls fail
            var op1 = userManager.UpdateBioData(null);
            op1.ResolveSafely();
            Assert.IsTrue(op1.Succeeded == false);

            //try wiping the data
            var op2 = userManager.UpdateBioData(new Principal.BioData { });
            op2.ResolveSafely();
            Assert.IsTrue(op2.Succeeded == true);
            Assert.IsNull(persisted.Dob);
            Assert.IsNull(persisted.FirstName);
            Assert.IsNull(persisted.LastName);
            Assert.IsNull(persisted.MiddleName);
            Assert.IsNull(persisted.Nationality);
            Assert.IsNull(persisted.StateOfOrigin);

            //try an actual update
            var op3 = userManager.UpdateBioData(new Principal.BioData { FirstName = "Updated" });
            op3.ResolveSafely();
            Assert.IsTrue(op2.Succeeded == true);
            Assert.AreEqual(persisted.FirstName, "Updated");
        }
        #endregion
    }
}
