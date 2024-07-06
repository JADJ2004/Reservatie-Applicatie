namespace ReservationApplication
{
    public class ReservationModel
    {
        public int ReservationId { get; set; }
        public List<int> TableIds { get; set; }
        public int NumOfPeople { get; set; }
        public string FirstName { get; set; }
        public string Infix { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Date { get; set; }
        public string TimeSlot { get; set; }
        public string Remarks { get; set; }
        public int TableId { get; set; }
        public int Capacity { get; set; }
    }
}