namespace DAL.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("public.tickets")]
    public partial class tickets
    {
        [Key]
        public int ticket_id { get; set; }

        public int? trip_id { get; set; }

        public int? buyer_id { get; set; }

        [Required]
        [StringLength(50)]
        public string passenger_first_name { get; set; }

        [StringLength(50)]
        public string passenger_middle_name { get; set; }

        [Required]
        [StringLength(50)]
        public string passenger_last_name { get; set; }

        [Required]
        [StringLength(20)]
        public string passenger_passport_number { get; set; }

        public DateTime? purchase_date { get; set; }

        public decimal price { get; set; }

        public virtual users users { get; set; }

        public virtual trips trips { get; set; }
    }
}
