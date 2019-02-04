using System;
using System.Collections.Generic;

namespace CGEvents.Models
{
    public partial class SecretSanta
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Company { get; set; }
        public string SecretSanta1 { get; set; }
        public int? Year { get; set; }
        public int Id { get; set; }
        public Guid? UniqId { get; set; }
        public bool Flag { get; set; }
        public string Dept { get; set; }
        public string Floor { get; set; }
    }
}
