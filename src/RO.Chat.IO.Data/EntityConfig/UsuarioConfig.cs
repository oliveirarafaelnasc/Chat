using RO.Chat.IO.Domain.Usuario.Entities;
using System.Data.Entity.ModelConfiguration;

namespace RO.Chat.IO.Data.EntityConfig
{
    public class UsuarioConfig : EntityTypeConfiguration<Usuario>
    {
        public UsuarioConfig()
        {
            HasKey(c => c.Id_Usuario);

            Property(c => c.Nome)
               .IsRequired()
               .HasMaxLength(150);

            Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(500);

            Property(c => c.NomeUsuario)
                .IsRequired()
                .HasMaxLength(150);

            Property(c => c.Senha)
                .IsRequired()
                .HasMaxLength(150);

            HasMany(c => c.Mensagens);
            HasMany(c => c.Mensagens_Destino);
            HasMany(c => c.Mensagens_Remetente);

            

            Ignore(c => c.Erros);


            ToTable("dbo.Usuario");
        }

    }
}
