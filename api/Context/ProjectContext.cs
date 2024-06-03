using api.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace api.Context;

public class ProjectContext (IConfiguration configuration) : DbContext {
    public DbSet<User> Users { get; set; }
    public DbSet<Event> Events { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder options) { 
        SqlConnectionStringBuilder connStringBuilder = new SqlConnectionStringBuilder {
            DataSource = configuration["SqlServer:Source"],
            InitialCatalog = configuration["SqlServer:Schema"],
            UserID =  configuration["SqlServer:User"],
            Password = configuration["SqlServer:Password"],
            TrustServerCertificate = true
        };
        
        options.UseSqlServer(connStringBuilder.ConnectionString);
    }
}