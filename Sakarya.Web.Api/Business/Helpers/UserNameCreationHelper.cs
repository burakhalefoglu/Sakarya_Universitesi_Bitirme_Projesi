using System;
using System.Text;

namespace Business.Helpers
{
    public static class UserNameCreationHelper
    {
        public static string EmailToUsername(string email)
        {
            var bld = new StringBuilder();

            foreach (var item in email)
            {
                if (item.ToString() == "@") break;
                bld.Append(item);
            }

            var username = bld.ToString();

            return AppendRandomNumber(username);
        }

        private static string AppendRandomNumber(string username)
        {
            var random = new Random();
            var number = random.Next(11111111, 999999999);

            return username + number;
        }
    }
}