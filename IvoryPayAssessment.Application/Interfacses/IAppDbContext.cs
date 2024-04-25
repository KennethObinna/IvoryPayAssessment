








using IvoryPayAssessment.Application.Common;
using IvoryPayAssessment.Application.Common.DTOs;
using IvoryPayAssessment.Application.Common.Models;
using IvoryPayAssessment.Domain.Entities;
 
using Microsoft.Data.SqlClient;
 

namespace IvoryPayAssessment.Application.Interfacses
{
    public interface IAppDbContext
    { 
        #region DbSet
        public  DbSet<ApplicationRoleClaim> ApplicationRoleClaim { get; set; }
        public  DbSet<ApplicationUserClaim> ApplicationUserClaim { get; set; }
        public  DbSet<ApplicationUserToken> ApplicationUserToken { get; set; }
        public  DbSet<ApplicationUserLogin> ApplicationUserLogin { get; set; }

        public  DbSet<Restaurant>  Restaurants { get; set; }
        public  DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
        public  DbSet<Session> Sessions { get; set; }
        public  DbSet<ApplicationUser> Users { get; set; }
        public  DbSet<ApplicationRole> Roles { get; set; }
        public  DbSet<ApplicationUserRole> UserRoles { get; set; }     
        public  DbSet<OTPs> OTPs { get; set; }
        public   DbSet<ProductCategory> ProductCategories { get; set; }
        public   DbSet<Product> Products { get; set; }
 


        #endregion

        #region Procedure

        public   DbSet<AdminShortDetailModel> AdminShortDetailModel { get; set; }
        
        #endregion






        Task<List<T>> GetData<T>(string query, params object[] param) where T : class;
        Task<T> GetDataSingle<T>(string query, params object[] param) where T : class;
         
        Task<int> ExecuteSQLCommand(string storedprocedure, params SqlParameter[] parameters);
        IDbContextTransaction Begin();
        Task CommitAsync();
        Task RollbackAsync();
        DbContext GetAppDbContext();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        int SaveChanges();

    }
}
