using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;


namespace ServelLog.Models
{
    [DataContract]
    internal class User
    {
        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Password { get; set; }
    }
}
