namespace DAL.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("public.routes")]
    public partial class routes
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public routes()
        {
            schedules = new HashSet<schedules>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int route_id { get; set; }

        [Required]
        [StringLength(100)]
        public string start_location { get; set; }

        [Required]
        [StringLength(100)]
        public string end_location { get; set; }

        public int? distance { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<schedules> schedules { get; set; }

        public string RouteInfo => $"{start_location} - {end_location}";
    }
}
