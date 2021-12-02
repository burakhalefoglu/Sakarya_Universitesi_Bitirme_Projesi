namespace Core.Entities.ClaimModels
{
    public class UserClaimModel
    {
        public string UserId { get; set; }
        public string[] OperationClaims { get; set; }
    }
}