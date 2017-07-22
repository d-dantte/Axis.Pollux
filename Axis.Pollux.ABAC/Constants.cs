
namespace Axis.Pollux.ABAC
{
    public static class Constants
    {
        #region Intent Attributes
        public static readonly string IntentAttribute_ResourceDescriptor = "ResourceDescriptor";
        public static readonly string IntentAttribute_ActionDescriptor = "ActionDescriptor";
        #endregion

        #region Subject Attributes
        public static readonly string SubjectAttribute_UserRole = "User: Role";
        public static readonly string SubjectAttribute_UserName = "User: Name";
        public static readonly string SubjectAttribute_UserUUID = "User: UUID";

        //bio data
        public static readonly string SubjectAttribute_BioFirstName = "Bio: FirstName";
        public static readonly string SubjectAttribute_BioLastName = "Bio: LastName";
        public static readonly string SubjectAttribute_BioMiddleName = "Bio: MiddleName";
        public static readonly string SubjectAttribute_BioGenderName = "Bio: GenderName";
        public static readonly string SubjectAttribute_BioNationality = "Bio: NationalityName";
        public static readonly string SubjectAttribute_BioDOB = "Bio: DOB";

        //address data
        public static readonly string SubjectAttribute_AddressStreet = "Address: Street";
        public static readonly string SubjectAttribute_AddressCity = "Address: City";
        public static readonly string SubjectAttribute_AddressStateProvince = "Address: StateProvince";
        public static readonly string SubjectAttribute_AddressCountry = "Address: Country";

        //contact data
        public static readonly string SubjectAttribute_ContactPhone = Identity.Constants.ContactType_Phone;
        public static readonly string SubjectAttribute_ContactEmail = Identity.Constants.ContactType_Email;
        
        #endregion

        #region Environment/Context Attributes
        #endregion
    }
}
