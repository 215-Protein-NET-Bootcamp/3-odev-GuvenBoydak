namespace JwtHomework.Base
{
    public static class CheckPassword
    {
        public static bool CheckingPassword(string newPassword, string confirmPassword)
        {
            if (string.Equals(newPassword, confirmPassword))
                return true;
            else
                return false;

        }
    }
}
