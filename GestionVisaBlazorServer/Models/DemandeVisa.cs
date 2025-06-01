using System.ComponentModel.DataAnnotations;

using GestionVisaBlazorServer.Models;
public enum StatutVisa
{
    EnCours,
    Acceptee,
    Refusee
}

public class DemandeVisa : IValidatableObject
{
    public int Id { get; set; }
    public int CollaborateurId { get; set; }
    public DateTime DateDemande { get; set; }
    public StatutVisa Statut { get; set; }
    public int? DureeVisa { get; set; }
    public string? Observations { get; set; }

    // Navigation
    public Collaborateur Collaborateur { get; set; }

    // Validation personnalisée 
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Statut == StatutVisa.Refusee && string.IsNullOrWhiteSpace(Observations))
        {
            yield return new ValidationResult(
                "Le champ Observations est obligatoire lorsque le statut est 'Refusée'.",
                new[] { nameof(Observations) }
            );
        }

        if (Statut == StatutVisa.Acceptee && (!DureeVisa.HasValue || DureeVisa <= 0))
        {
            yield return new ValidationResult(
                "Le champ Durée de VISA est obligatoire et doit être supérieur à 0 lorsque le statut est 'Acceptée'.",
                new[] { nameof(DureeVisa) }
            );
        }
    }
}
