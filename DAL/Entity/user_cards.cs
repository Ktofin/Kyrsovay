namespace DAL.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("public.user_cards")]
    public partial class user_cards
    {
        [Key]
        public int card_id { get; set; }

        public int? user_id { get; set; }

        [Required]
        [StringLength(16)]
        public string card_number { get; set; }

        [Column(TypeName = "date")]
        public DateTime expiration_date { get; set; }

        [Required]
        [StringLength(4)]
        public string cvv_code { get; set; }

        public virtual users users { get; set; }
    }
}
