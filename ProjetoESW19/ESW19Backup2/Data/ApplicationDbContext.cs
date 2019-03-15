using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ESW19Backup2.Models;
using Microsoft.EntityFrameworkCore.Design;
using ESW19Backup2.Models.Apoios;

namespace ESW19Backup2.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(builder);
        }

        public DbSet<Tarefa> Tarefa { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<Horarios> Horarios { get; set; }
        public DbSet<Atribuicao> Atribuicao { get; set; }
        public DbSet<Saude> Saude { get; set; }
        public DbSet<Evento> Evento { get; set; }
        public DbSet<Image> Image { get; set; }
        public DbSet<Erro> Erros { get; set; }
        public DbSet<Cao> Cao { get; set; }
        public DbSet<Ajuda> Ajudas { get; set; }
        public DbSet<TipoApadrinhamento> TipoApadrinhamento { get; set; }
        public DbSet<Adocao> Adocao { get; set; }
        public DbSet<Voluntario> Voluntario { get; set; }
        public DbSet<Socios> Socios { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Apadrinhar> Apadrinhar { get; set; }
        public DbSet<ReportarOcorrencia> ReportarOcorrencia { get; set; }
        public DbSet<Models.Upload.FileDetails> FileDetails { get; set; }
        public DbSet<Models.Upload.FilesViewModel> FilesViewModel { get; set; }
        public DbSet<TipoPrioridade> TipoPrioridades { get; set; }
    }
}
