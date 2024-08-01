using System.ComponentModel.DataAnnotations;
using ProjektAdrianZachwiej56233.Validators;

namespace ProjektAdrianZachwiej56233.Models
{
    public class ReservationRequest
    {
        public int UserId { get; set; }
        public int TableId { get; set; }
        public DateTime Date { get; set; }
        public string FirstName { get; set; } 
        public string LastName { get; set; }  
        public string PhoneNumber { get; set; } 
    }
}

