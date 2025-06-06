﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Domain.Entities
{
    public class EmailLog:BaseEntity
    {
        public string? EmailId { get; set; }
        public DateTime EmailDate { get; set; }
        public string? MemberName { get; set; }
        public int? UserId { get; set; }
        public string? Message { get; set; }
        public string? Status { get; set; }
    }
}
