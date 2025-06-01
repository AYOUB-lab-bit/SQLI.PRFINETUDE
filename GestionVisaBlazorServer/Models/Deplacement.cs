using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using GestionVisaBlazorServer.Models;
public enum Equipe { Roadmap, TMA }
[Authorize]
public class Deplacement
{
    public int Id { get; set; }

    [Required(ErrorMessage = "La date de début est obligatoire.")]
    public DateTime DateDebut { get; set; }

    [Required(ErrorMessage = "La date de fin est obligatoire.")]
    public DateTime DateFin { get; set; }

    [Required(ErrorMessage = "L'équipe est obligatoire.")]
    public Equipe Equipe { get; set; }

    public string? Observations { get; set; }

    [Required(ErrorMessage = "Le coût est obligatoire.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Le coût doit être supérieur à 0.")]
    [Precision(18, 4)]
    public decimal CoutTotal { get; set; }

    // ✅ Propriété de navigation vers la table de liaison
    public ICollection<DeplacementCollaborateur> DeplacementCollaborateurs { get; set; } = new List<DeplacementCollaborateur>();

    public ValidationResult ValidateDates()
    {
        if (DateFin < DateDebut)
        {
            return new ValidationResult("La date de fin doit être postérieure à la date de début.");
        }

        return ValidationResult.Success;
    }
}
