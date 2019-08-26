using System.Collections.Generic;

namespace belt.Models {
    public class WrapperModel {
        public User LoggedInUser {get;set;}

        public Occasion NewOccasion {get;set;}
        public Occasion ThisOccasion {get;set;}
        public List<Occasion> AllOccasions {get;set;}
        public Association OneAssociation {get;set;}
        public List<Association> ListAssociation {get;set;}
    }
}