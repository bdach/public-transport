using System.ComponentModel.DataAnnotations;

namespace PublicTransport.Domain.Entities
{
    /// <summary>
    ///     Base abstract class for all the entities.
    /// </summary>
    public abstract class Entity
    {
        /// <summary>
        ///     Contains the unique identification number of the given entity.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        ///     Contains the last modification version of the given entity.
        /// </summary>
        [Timestamp]
        public byte[] Version { get; set; }
    }
}