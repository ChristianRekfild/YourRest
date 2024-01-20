namespace YourRest.Infrastructure.Core.Contracts.Models
{
    public class AgeRangeDto: IntBaseEntityDto
    {
        public int AgeFrom { get; set; }
        public int AgeTo { get; set; }
    }
}
