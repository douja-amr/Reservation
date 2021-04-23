using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Reservation.Models
{
    public class ReservationModel
    {

        [Key]
        [ForeignKey("StudentId,TypeReservationId")]

        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Status { get; set; }

        public string Cause { get; set; }

        public string StudentId { get; set; }

        public int TypeReservationId { get; set; }

        public Student Student { get; set; }
        public TypeReservations TypeReservation { get; set; }
    }
}
