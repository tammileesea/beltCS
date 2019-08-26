using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace belt.Models {
    public class Association {
        [Key]
        public int AssociationId {get;set;}

        public int PersonId {get;set;}
        public User Person {get;set;}

        public int ThingId {get;set;}
        public Occasion Thing {get;set;}

        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
    }
}