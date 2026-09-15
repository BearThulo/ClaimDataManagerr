namespace ClaimDataManager
{
    public class Claim
    {
        public int ClaimId { get; set; }

        public string LecturerName { get; set; } = "";

        public string ModuleCode { get; set; } = "";

        public decimal HoursWorked { get; set; }

        public decimal HourlyRate { get; set; }

        public string ClaimMonth { get; set; } = "";

        public string Status { get; set; } = "Draft";

        // Calculates the amount of the claim
        public decimal TotalAmount
        {
            get
            {
                return HoursWorked * HourlyRate;
            }
        }
    }
}