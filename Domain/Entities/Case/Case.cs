﻿using Domain.Contracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Case
{
    [Table("case")]
    public class Case : AuditableBaseEntity<long>
    {
        [Column("reason", TypeName = "nvarchar(500)")]
        public string Reason { get; set; }
        [Column("murder_weapon",TypeName = "nvarchar(100)")]
        public string MurderWeapion { get; set; }
        [Column("start_date", TypeName = "datetime")]
        public DateTime? StartDate { get; set; }
        [Column("end_date",TypeName = "datetime")]
        public DateTime EndDate { get; set; }
        [Column("type_of_violation", TypeName = "smallint")]
        public int? TypeOfViolation { get; set; }
        [Column("status", TypeName = "smallint")]
        public int? Status { get; set; }
        [Column("charge", TypeName = "nvarchar(100)")]
        public string? Charge { get; set; }
    }
}
