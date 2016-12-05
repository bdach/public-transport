using System.Runtime.Serialization;
using PublicTransport.Domain.Enums;

namespace PublicTransport.Services.DataTransfer
{
    /// <summary>
    /// Data transfer object for <see cref="Domain.Entities.Role"/> objects.
    /// </summary>
    [DataContract]
    public class RoleDto
    {
        /// <summary>
        /// Role ID.
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        ///     Defines the role type.
        /// </summary>
        [DataMember]
        public RoleType Name { get; set; }
    }
}