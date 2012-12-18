using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Entities
{
    [DataContract]
    public class Entity
    {
        [DataMember]
        [Timestamp]
        public byte[] Version { get; set; }
    }
}
