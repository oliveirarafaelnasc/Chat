using RO.Chat.IO.Domain.Mensagem.Entities;
using RO.Chat.IO.Domain.Usuario.Entities;
using System;
using System.Data.Entity.ModelConfiguration;

namespace RO.Chat.IO.Data.EntityConfig
{
    public class MensagemConfig : EntityTypeConfiguration<Mensagem>
    {
        public MensagemConfig()
        {
            HasKey(c => c.Id_Mensagem);

            Property(c => c.Texto_Mensagem)
                .IsRequired()
                .HasMaxLength(2000);

            Property(c => c.Data_Criacao)
                .HasColumnType("datetime")
                .IsRequired();

            Property(c => c.Particular)
                .IsRequired();

            HasRequired<Usuario>(s => s.Usuario)
                .WithMany(g => g.Mensagens)
                .HasForeignKey(k => k.Id_Usuario);
            //    .HasForeignKey(k => k.Usuario);
            // .HasForeignKey<Guid>(s => (Guid)s.Id_Usuario);
            // .HasForeignKey<Guid>(s => (Guid)s.Usuario.Id_Usuario);

            HasOptional<Usuario>(s => s.Usuario_Destino)
                .WithMany(g => g.Mensagens_Destino)
                .HasForeignKey(k => k.Id_Usuario_Destino);
            //.HasForeignKey<Guid>(s => (Guid)s.Id_Usuario_Destino);
            //   .HasForeignKey<Guid>(s => (Guid)s.Usuario_Destino.Id_Usuario);
            // .HasForeignKey(k => k.Usuario_Destino);

            HasOptional<Usuario>(s => s.Usuario_Remetente)
                .WithMany(g => g.Mensagens_Remetente)
                .HasForeignKey(k => k.Id_Usuario_Remetente);
            //.HasForeignKey<Guid>(s => (Guid)s.Id_Usuario_Remetente);
            // .HasForeignKey<Guid>(s => (Guid)s.Usuario_Remetente.Id_Usuario);
            //   .HasForeignKey(k => k.Usuario_Remetente);

            HasOptional<Grupo>(s => s.Grupo)
                .WithMany(g => g.Mensagens)
                .HasForeignKey(k => k.Id_Grupo);
            //.HasForeignKey<int>(s => (int)s.Id_Grupo);
            //  .HasForeignKey<int>(s => (int)s.Grupo.Id_Grupo);
            //  .HasForeignKey(k => k.Grupo);


            Ignore(c => c.Erros);

            ToTable("dbo.Mensagem");
        }
    }
}
