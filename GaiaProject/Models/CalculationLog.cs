using System.ComponentModel.DataAnnotations;

namespace GaiaProject.Models
{
    public class CalculationLog
    {
        [Key]
        public int Id { get; set; }

        public string? OperationType { get; set; }
        public string? ActionName { get; set; }
        public string? InputData { get; set; }
        public string? ResultValue { get; set; }
        public DateTime ExecutionTime { get; set; } = DateTime.Now;
        public string? PerformedBy { get; set; } = "E.G.";
    }
}