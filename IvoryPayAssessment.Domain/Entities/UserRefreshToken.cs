



namespace IvoryPayAssessment.Domain.Entities
{
    [Table("UserRefreshToken")]
    public partial class UserRefreshToken: BaseObject
    {
     
        [StringLength(500)]
        [Unicode(false)]
        public string? UserName { get; set; }
        
        public string? RefreshToken { get; set; }    
      
        public string? UserId { get; set; }

 
        public string MerchantId { get; set; }

    }
}
