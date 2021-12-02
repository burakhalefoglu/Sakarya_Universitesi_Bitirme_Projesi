using System;
using System.Collections.Generic;
using Core.Entities.Concrete;
using Core.Utilities.Security.Hashing;

namespace Tests.Helpers
{
    public static class DataHelper
    {
        public static User GetUser(string name)
        {
            HashingHelper.CreatePasswordHash("123456", out var passwordSalt, out var passwordHash);

            return new User
            {
                UserId = "A126C8212",
                Email = "test@test.com",
                Name = string.Format("{0} {1} {2}", name, name, name),
                RecordDate = DateTime.Now,
                PasswordHash = passwordSalt,
                PasswordSalt = passwordHash,
                Status = true
            };
        }

        public static List<User> GetUserList()
        {
            HashingHelper.CreatePasswordHash("123456", out var passwordSalt, out var passwordHash);
            var list = new List<User>();

            for (var i = 1; i <= 5; i++)
            {
                var user = new User
                {
                    UserId = "A126C8212" + i,
                    Email = "test@test.com",
                    Name = string.Format("name {0} name {1} name {2}", i, i, i),
                    RecordDate = DateTime.Now,
                    PasswordHash = passwordSalt,
                    PasswordSalt = passwordHash,
                    Status = true
                };
                list.Add(user);
            }

            return list;
        }
    }
}