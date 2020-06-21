using RO.Chat.IO.Domain.Mensagem.Entities;
using System.Data.Entity.ModelConfiguration;

namespace RO.Chat.IO.Data.EntityConfig
{
    public class GrupoConfig : EntityTypeConfiguration<Grupo>
    {
        public GrupoConfig()
        {
            HasKey(c => c.Id_Grupo);

            Property(c => c.Nome)
               .IsRequired()
               .HasMaxLength(50);

            HasMany(c => c.Mensagens)
                .WithOptional(o => o.Grupo)
                .HasForeignKey(k => k.Id_Grupo);

            Ignore(c => c.Erros);

            ToTable("dbo.Grupo");
        }
    }
}
