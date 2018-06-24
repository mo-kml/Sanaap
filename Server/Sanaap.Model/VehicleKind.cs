using Sanaap.Model.Contracts;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sanaap.Model
{
    public class VehicleKind : IChangeTrackEnableEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public DateTimeOffset ModifiedOn { get; set; }

        public string Name { get; set; }
    }
}
