using Microsoft.EntityFrameworkCore;

namespace GaiaProject.Models
{
    // השורה הזו קריטית: היא אומרת שהקלאס שלנו יורש את היכולות של Entity Framework
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // הטבלה שתיווצר ב-SQL
        public DbSet<CalculationLog> CalculationLogs { get; set; }
    }
}