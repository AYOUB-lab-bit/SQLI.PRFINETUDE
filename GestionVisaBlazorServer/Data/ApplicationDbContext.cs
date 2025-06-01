using Microsoft.EntityFrameworkCore;
using GestionVisaBlazorServer.Models;

namespace GestionVisaBlazorServer.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Collaborateur> Collaborateurs { get; set; }
        public DbSet<DemandeVisa> DemandesVisa { get; set; }
        public DbSet<Deplacement> Deplacements { get; set; }
        public DbSet<DeplacementCollaborateur> DeplacementCollaborateurs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // base.OnModelCreating(builder); // optionnel

            builder.Entity<DeplacementCollaborateur>()
                .HasKey(dc => new { dc.DeplacementId, dc.CollaborateurId });

            builder.Entity<DeplacementCollaborateur>()
                .HasOne(dc => dc.Deplacement)
                .WithMany(d => d.DeplacementCollaborateurs)
                .HasForeignKey(dc => dc.DeplacementId);

            builder.Entity<DeplacementCollaborateur>()
                .HasOne(dc => dc.Collaborateur)
                .WithMany(c => c.DeplacementCollaborateurs)
                .HasForeignKey(dc => dc.CollaborateurId);
        }
    }
}
