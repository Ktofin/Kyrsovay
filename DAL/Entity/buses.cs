namespace DAL.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("public.buses")]
    public partial class buses
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public buses()
        {
            schedules = new HashSet<schedules>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int bus_id { get; set; }

        [Required]
        [StringLength(50)]
        public string model { get; set; }

        public int capacity { get; set; }

        [Required]
        [StringLength(20)]
        public string license_plate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<schedules> schedules { get; set; }

        public string BusInfo => $"{model} ({license_plate})";
    }
}
