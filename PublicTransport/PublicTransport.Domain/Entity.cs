﻿using System.ComponentModel.DataAnnotations;

namespace PublicTransport.Domain
{
    public abstract class Entity
    {
        [Key]
        public int Id { get; set; }

        [Timestamp]
        public byte[] Version { get; set; }
    }
}
