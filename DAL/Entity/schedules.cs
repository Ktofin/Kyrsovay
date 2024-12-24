namespace DAL.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("public.schedules")]
    public partial class schedules
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public schedules()
        {
            trips = new HashSet<trips>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int schedule_id { get; set; }

        public int? route_id { get; set; }

        public int? bus_id { get; set; }

        public int? driver_id { get; set; }

        [Column(TypeName = "time")]
        public TimeSpan start_time { get; set; }

        [Column(TypeName = "time")]
        public TimeSpan end_time { get; set; }

        public int interval_minutes { get; set; }

        public virtual buses buses { get; set; }

        public virtual drivers drivers { get; set; }

        public virtual routes routes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<trips> trips { get; set; }

        [NotMapped]
        public string RouteInfo => $"{routes?.start_location} - {routes?.end_location}";

        [NotMapped]
        public string BusInfo => $"{buses?.model} ({buses?.license_plate})";

        [NotMapped]
        public string DriverInfo => $"{drivers?.first_name} {drivers?.last_name}";


    }
}
