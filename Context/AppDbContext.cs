using Microsoft.EntityFrameworkCore;
using Test.Models;
using Test.Models.Common;


namespace Test.Context
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}


        public DbSet<Person> Persons { get; set; }




        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

           
            var datas = ChangeTracker
                .Entries<BaseEntity>();

            foreach (var data in datas)
            {
                _ = data.State switch
                {

                    EntityState.Added => data.Entity.CreatedDate=DateTime.UtcNow.ToLocalTime(),
                    EntityState.Modified =>data.Entity.UpdatedDate=DateTime.UtcNow.ToLocalTime(),
                    _ => DateTime.UtcNow.ToLocalTime()
                };

            }

            return await base.SaveChangesAsync(cancellationToken);
        }

    }
}
