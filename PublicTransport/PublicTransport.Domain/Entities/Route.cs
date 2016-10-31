﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PublicTransport.Domain.Enums;

namespace PublicTransport.Domain.Entities
{
    /// <summary>
    ///     Represents a time-independent route, which is equivalent to a line in public transportation systems.
    /// </summary>
    public class Route : Entity
    {
        /// <summary>
        ///     Defines an agency for the specified route.
        /// </summary>
        [Required]
        public int AgencyId { get; set; }

        /// <summary>
        ///     Contains the <see cref="Entities.Agency" /> operating the particular route.
        /// </summary>
        [ForeignKey("AgencyId")]
        public Agency Agency { get; set; }

        /// <summary>
        ///     Contains the short name of a route. This often will be a short, abstract identifier like "32", "100X" or "Green"
        ///     that riders use to identify a route, but which doesn't give any indication of what places the route serves.
        /// </summary>
        [Required]
        public string ShortName { get; set; }

        /// <summary>
        ///     Contains the full name of a route. This name is generally more descriptive than the <see cref="ShortName" /> and
        ///     will often include the route's destination or stop.
        /// </summary>
        [Required]
        public string LongName { get; set; }

        /// <summary>
        ///     Describes the type of transportation used on a route.
        /// </summary>
        [Required]
        public RouteType RouteType { get; set; }
    }
}