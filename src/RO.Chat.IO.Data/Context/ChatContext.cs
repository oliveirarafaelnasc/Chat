using RO.Chat.IO.Data.EntityConfig;
using RO.Chat.IO.Domain.Mensagem.Entities;
using RO.Chat.IO.Domain.Usuario.Entities;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

namespace RO.Chat.IO.Data.Context
{
    public class ChatContext : DbContext
    {
        public ChatContext() :
            base("ChatContext")
        {

            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            Configuration.AutoDetectChangesEnabled = false;
        }

        public DbSet<Grupo> Grupos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Mensagem> Mensagens { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Properties<string>()
              .Configure(p => p.HasColumnType("varchar"));

            modelBuilder.Properties<string>()
              .Configure(p => p.HasMaxLength(100));

            modelBuilder.Configurations.Add(new GrupoConfig());
            modelBuilder.Configurations.Add(new UsuarioConfig());
            modelBuilder.Configurations.Add(new MensagemConfig());


            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries()
              .Where(entry => entry.Entity.GetType().GetProperty("Data_Inclusao") != null))
            {
                if (entry.State == EntityState.Added)
                    entry.Property("Data_Inclusao").CurrentValue = DateTime.Now;

                if (entry.State == EntityState.Modified)
                    entry.Property("Data_Inclusao").IsModified = false;
            }

            return base.SaveChanges();
        }

    }
}
