using Microsoft.EntityFrameworkCore;
using CRMSystem.Models;

namespace CRMSystem.Data
{
    // The ApplicationDbContext class, inheriting from DbContext, is the main class that coordinates Entity Framework functionality for a data model.
    public class ApplicationDbContext : DbContext
    {
        // Constructor for ApplicationDbContext. It accepts DbContextOptions and passes them to the base class constructor. 
        // This is needed to configure the DbContext with settings such as the database provider to use and the connection string.
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
         : base(options)
          { 
            // Intentionally left blank. In this case, all necessary actions are performed by the base class constructor.
          }

        // Declaration of DbSets. Each DbSet represents a table in the database and the entity type that maps to it.
        // This is how EF knows about the entities (User, Customer, Call) you are working with.

        // DbSet for Users. It represents the User table in the database.
        public DbSet<User> Users { get; set; }

        // DbSet for Customers. It represents the Customer table in the database.
        public DbSet<Customer> Customers { get; set; }

        // DbSet for Calls. It represents the Call table in the database.
        public DbSet<Call> Calls { get; set; }

        // OnModelCreating is a method that's called by EF to further configure the model that was discovered by convention from your entity classes.
        // It's overridden here to specify table names explicitly.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuring the User entity to map to the "User" table.
            modelBuilder.Entity<User>().ToTable("User");

            // Configuring the Customer entity to map to the "Customer" table.
            modelBuilder.Entity<Customer>().ToTable("Customer");

            // Configuring the Call entity to map to the "Call" table.
            modelBuilder.Entity<Call>().ToTable("Call");
        }
    }
}
