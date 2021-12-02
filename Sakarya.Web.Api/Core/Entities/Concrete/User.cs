using System;

namespace Core.Entities.Concrete
{
    public class User : DocumentDbEntity
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool Status { get; set; }
        public DateTime RecordDate { get; set; }
        public DateTime UpdateContactDate => DateTime.Now;

        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
        public string ResetPasswordToken { get; set; }
        public DateTime ResetPasswordExpires { get; set; }
    }
}