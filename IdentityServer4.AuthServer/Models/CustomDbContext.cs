using Microsoft.EntityFrameworkCore;

namespace IdentityServer4.AuthServer.Models
{
    public class CustomDbContext:DbContext
    {
        public CustomDbContext(DbContextOptions opts):base(opts) { }    
        
        public DbSet<CustomUser> CustomUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomUser>().HasData(new CustomUser { Name="Murat", Surname="Resuloğulları", Password="test.123", Username="murat.resulogullari",Email="murat.resulogullari1@gmail.com",City="İzmir"});
            modelBuilder.Entity<CustomUser>().HasData(new CustomUser { Name="Ali", Surname="Veli" ,Username="ali.veli", Password = "test.123", Email ="ali.veli@gmail.com",City="Konya"});
            modelBuilder.Entity<CustomUser>().HasData(new CustomUser { Name="Mehmet", Surname="Hasan" ,Username="mehmet.hasan", Password = "test.123", Email = "mehmet.hasan@gmail.com", City="Amasya"});

            base.OnModelCreating(modelBuilder);
        }
    }
}
