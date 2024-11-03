namespace DotNet_Salon_Manager.modules.Classes
{
    public static class UserProfile
    {
        public static int UserId { get; private set; }
        public static string Name { get; private set; }
        public static string CPF { get; private set; }
        public static string Email { get; private set; }
        public static int AccessLevelId { get; private set; }

        public static void StartProfile(int UserId, string Name, string CPF, string Email, int AccessLevelId)
        {
            UserProfile.UserId = UserId;
            UserProfile.Name = Name;
            UserProfile.CPF = CPF;
            UserProfile.Email = Email;
            UserProfile.AccessLevelId = AccessLevelId;
        }
    }
}
