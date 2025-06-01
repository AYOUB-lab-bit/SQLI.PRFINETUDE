using System;
using System.Collections.Generic;

namespace GestionVisaBlazorServer.Models
{
    public class Collaborateur
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Poste { get; set; }
        public int AnciennetéProjet { get; set; }
        public DateTime DateValiditePasseport { get; set; }
        public DateTime? DateDebutVisa { get; set; }
        public DateTime? DateFinVisa { get; set; }

        // Navigation properties
        public ICollection<DemandeVisa> DemandesVisa { get; set; }
        public ICollection<DeplacementCollaborateur> DeplacementCollaborateurs { get; set; }
    }
}
