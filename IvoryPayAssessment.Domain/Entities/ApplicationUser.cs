

using IvoryPayAssessment.Domain.Entities;

namespace IvoryPayAssessment.Domain.Entities
{
    public partial class ApplicationUser : IdentityUser<string>
    {
        public ApplicationUser() : base()
        {
            
            IsActive = true;
            IsDeleted = false;
        }

      
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? FullName { get; set; }  
        public DateTime DateCreated { get; set; }
        public DateTime DateofBirth { get; set; } 
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int LoginCount { get; set; }            
        public string?Address { get; set; }    
        public string? DefaultRole { get; set; }
        public string? Gender { get; set; } 
      


    }
}
