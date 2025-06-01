using GestionVisaBlazorServer.Models;



public class DeplacementCollaborateur
{
    public int Id { get; set; }

    public int DeplacementId { get; set; }
    public Deplacement Deplacement { get; set; }

    public int CollaborateurId { get; set; }
    public Collaborateur Collaborateur { get; set; }
}
