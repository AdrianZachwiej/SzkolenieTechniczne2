namespace ProjektAdrianZachwiej56233.Models
{
    public class UserRequest
    {
        public int UserId { get; set; }
        public int TableId { get; set; }
        public DateTime Date { get; set; }
        public string FirstName { get; set; } 
        public string LastName { get; set; }  
        public string PhoneNumber { get; set; }
    }
}