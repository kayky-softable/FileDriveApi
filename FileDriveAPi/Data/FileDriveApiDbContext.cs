using FileDriveAPi.Model.Entity;
using Microsoft.EntityFrameworkCore;

namespace FileDriveAPi.Data;

public class FileDriveApiDbContext : DbContext
{
    public FileDriveApiDbContext(DbContextOptions<FileDriveApiDbContext> options) : base(options)
    {
        
    }

    public DbSet<MyFile> MyFiles { get; set; }
    
}