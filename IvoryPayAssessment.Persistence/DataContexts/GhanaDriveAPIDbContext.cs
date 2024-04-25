using IvoryPayAssessment.Application.Common;
using IvoryPayAssessment.Application.Common.Constants.ErrorBuldles;
using IvoryPayAssessment.Application.Common.DTOs;
using IvoryPayAssessment.Application.Common.Helpers;
using IvoryPayAssessment.Domain.Entities;
using System.Data;
using System.Reflection;


namespace IvoryPayAssessment.Persistence.DataContexts
{
    public partial class IvoryPayAssessmentDbContext : IdentityDbContext<ApplicationUser,
        ApplicationRole, string, ApplicationUserClaim, ApplicationUserRole,
        ApplicationUserLogin, ApplicationRoleClaim, ApplicationUserToken>, IAppDbContext
    {
        public IvoryPayAssessmentDbContext()
        {
        }


        public IvoryPayAssessmentDbContext(DbContextOptions<IvoryPayAssessmentDbContext> options)
            : base(options)
        {
        }



        public virtual DbSet<ApplicationRoleClaim> ApplicationRoleClaim { get; set; }
        public virtual DbSet<ApplicationUserClaim> ApplicationUserClaim { get; set; }
        public virtual DbSet<ApplicationUserToken> ApplicationUserToken { get; set; }
        public virtual DbSet<ApplicationUserLogin> ApplicationUserLogin { get; set; }


        public virtual DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<ApplicationUser> Users { get; set; }
        public virtual DbSet<ApplicationRole> Roles { get; set; }
        public virtual DbSet<ApplicationUserRole> UserRoles { get; set; }
        public virtual DbSet<Restaurant> Restaurants { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }
        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<OTPs> OTPs { get; set; }



        #region Procedure


        public virtual DbSet<AdminShortDetailModel> AdminShortDetailModel { get; set; }

        #endregion


        public async Task<List<T>> GetData<T>(string query, params object[] param) where T : class
        {
            var data = new List<T>();
            if (param != null && param.Length > 0)
            {
                var para = string.Join(",", param);
                data = await this.Set<T>().FromSqlRaw($"{query} {para}", param).ToListAsync();
            }
            else
            {
                data = await this.Set<T>().FromSqlRaw(query).ToListAsync();
            }
            return data;
        }
        public async Task<T> GetDataSingle<T>(string query, params object[] param) where T : class
        {

            if (param != null && param.Length > 0)
            {
                var para = string.Join(",", param);
                var data = await this.Set<T>().FromSqlRaw($"{query} {para}", param).ToListAsync();
                return data?.FirstOrDefault();
            }
            else
            {
                var data = await this.Set<T>().FromSqlRaw(query).ToListAsync();
                return data?.FirstOrDefault();
            }


        }
        public async Task<int> ExecuteSQLCommand(string storedprocedure, params SqlParameter[] parameters)
        {

            string par = string.Join(", ", parameters.Select(p => p.ParameterName));
            var result = await this.Database.ExecuteSqlRawAsync($"{storedprocedure} {par}", parameters);
            return result;
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AdminShortDetailModel>(entity => { entity.HasNoKey(); });

            modelBuilder.Entity<ApplicationRole>(entity => { entity.HasKey(p => p.Id); });
            modelBuilder.Entity<ApplicationUser>(entity => { entity.HasKey(p => p.Id); });

            modelBuilder.Entity<ProductCategory>(entity =>
                                 {

                                     entity.HasKey(e => e.Id);

                                     entity.ToTable(nameof(ProductCategory));

                                 });
            modelBuilder.Entity<Product>(entity =>
                                {

                                    entity.HasKey(e => e.Id);

                                    entity.ToTable(nameof(Product));

                                });

            modelBuilder.Entity<Restaurant>(entity =>
                        {

                            entity.HasKey(e => e.Id);

                            entity.ToTable(nameof(Restaurant));

                        });

            modelBuilder.Entity<OTPs>(entity =>
                       {

                           entity.HasKey(e => e.Id);

                           entity.ToTable(nameof(OTPs));

                       });
            modelBuilder.Entity<City>(entity =>
                       {
                           entity.HasKey(e => e.Id);
                           entity.HasIndex(p => p.Name).IsUnique();
                           entity.ToTable(nameof(City));

                       });
            modelBuilder.Entity<ApplicationRole>(entity =>
            {

                entity.HasKey(e => e.Id);

                entity.ToTable(nameof(Roles));

            });
            modelBuilder.Entity<ApplicationUser>(entity =>
            {

                entity.ToTable(nameof(Users));

            });
            modelBuilder.Entity<ApplicationUserRole>(entity =>
            {

                entity.ToTable(nameof(UserRoles));

            });
            modelBuilder.Entity<ApplicationUserClaim>(entity =>
            {

                entity.ToTable(nameof(UserClaims));

            });
            modelBuilder.Entity<ApplicationRoleClaim>(entity =>
            {

                entity.ToTable(nameof(RoleClaims));

            });


            modelBuilder.Entity<ApplicationUserLogin>(entity =>
            {

                entity.ToTable(nameof(UserLogins));

            });
            modelBuilder.Entity<ApplicationUserToken>(entity =>
            {

                entity.ToTable(nameof(UserTokens));

            });



            modelBuilder.Entity<Session>(entity =>
                                {
                                    entity.ToTable(nameof(Session));
                                    entity.HasKey(d => d.Id);
                                    entity.Property(d => d.Id).ValueGeneratedOnAdd();

                                });



            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
        public IDbContextTransaction Begin()
        {
            var trans = this.Database.CurrentTransaction;
            if (this.Database.CurrentTransaction == null)
            {
                trans = this.Database.BeginTransaction();
            }

            return trans;
        }
        public async Task CommitAsync()
        {

            var trans = Begin();

            if (trans != null)
            {

                await trans.CommitAsync();
            }

        }
        public async Task RollbackAsync()
        {
            var trans = Begin();

            if (trans != null)
            {
                await trans.RollbackAsync();
            }

        }
        public DbContext GetAppDbContext()
        {
            return this;
        }
        private T ConvertDataTableToObject<T>(DataTable dt) where T : class
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                T item = GetItem<T>(dt.Rows[0]);
                return item;
            }
            return null;
        }
        public T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }
    }
}
