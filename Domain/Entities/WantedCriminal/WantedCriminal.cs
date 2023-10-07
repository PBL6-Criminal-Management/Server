﻿
using Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.WantedCriminal
{
    [Table("wanted_criminal")]
    public class WantedCriminal : AuditableBaseEntity<long>
    {
        [Column("criminal_id", TypeName = "bigint")]
        public long? CriminalId { get; set; }
        [Column("wanted_type", TypeName = "smallint")]
        public int? WantedType { get; set; }
        [Column("current_activity", TypeName = "nvarchar(200)")]
        public string CurrentActivity { get; set; }
        [Column("wanted_decision_no", TypeName = "nvarchar(50)")]
        public string? WantedDecisionNo { get; set; }
        [Column("wanted_decision_day", TypeName = "date")]
        public DateTime? WantedDecisionDay { get; set; }
        [Column("decision_making_unit", TypeName = "nvarchar(100)")]
        public string? DecisionMakingUnit { get; set; }
    }
}