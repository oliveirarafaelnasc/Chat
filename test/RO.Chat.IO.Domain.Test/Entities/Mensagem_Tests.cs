using Microsoft.VisualStudio.TestTools.UnitTesting;
using RO.Chat.IO.Domain.Mensagem.Entities;
using System;
using System.Linq;

namespace RO.Chat.IO.Domain.Test.Entities
{

    [TestClass]
    public class Mensagem_Tests
    {
        [TestMethod]
        public void MensagemGrupo_Sucesso()
        {
            // Arrange 
            Usuario.Entities.Usuario usuarioBase = new Usuario.Entities.Usuario(Guid.NewGuid(), "andybogard@fatalfury.com", "Andy", "Andy Bogard", "123456");
            Grupo grupo = new Grupo(1,"Fatal Fury");
            Mensagem.Entities.Mensagem  mensagem_Grupo = new Mensagem.Entities.Mensagem("Olá a todos!", usuarioBase, grupo);

            // Act 
            mensagem_Grupo.IsValid();

            // Assert
            Assert.IsTrue(mensagem_Grupo.Erros.Count == 0);
            Assert.IsTrue(!mensagem_Grupo.Particular);

        }
        

        [TestMethod]
        public void MensagemGrupo_UsuarioInvalido_Erro()
        {

            // Arrange 
            Usuario.Entities.Usuario usuario = new Usuario.Entities.Usuario(new Guid(), "", "", "", "");
            
            Grupo grupo = new Grupo(1, "Fatal Fury");
            Mensagem.Entities.Mensagem mensagem_Grupo = new Mensagem.Entities.Mensagem("Olá a todos!", usuario, grupo);

            // Act 
            mensagem_Grupo.IsValid();

            // Assert
            Assert.IsTrue(mensagem_Grupo.Erros.Count > 0);
            Assert.IsTrue(mensagem_Grupo.Erros.Contains("Id do usuário base inválido!"));
        }
        
        [TestMethod]
        public void MensagemGrupo_GrupoInvalido_Erro()
        {

            // Arrange 
            Usuario.Entities.Usuario usuarioBase = new Usuario.Entities.Usuario(Guid.NewGuid(), "andybogard@fatalfury.com", "Andy", "Andy Bogard", "123456");
            Mensagem.Entities.Grupo grupo = new Mensagem.Entities.Grupo("teste");
            Mensagem.Entities.Mensagem mensagem_Grupo = new Mensagem.Entities.Mensagem("Olá a todos!", usuarioBase, grupo);

            // Act 
            mensagem_Grupo.IsValid();

            // Assert
            Assert.IsTrue(mensagem_Grupo.Erros.Count > 0);
            Assert.IsTrue(mensagem_Grupo.Erros.Contains("Grupo não informado ou inválido!"));
        }

        [TestMethod]
        public void MensagemGrupo_MsgInvalida_Erro()
        {
            // Arrange 
            Usuario.Entities.Usuario usuarioBase = new Usuario.Entities.Usuario(Guid.NewGuid(), "andybogard@fatalfury.com", "Andy", "Andy Bogard", "123456");
            Grupo grupo = new Grupo(1, "Fatal Fury");
            Mensagem.Entities.Mensagem mensagem_Grupo = new Mensagem.Entities.Mensagem("", usuarioBase, grupo);

            // Act 
            mensagem_Grupo.IsValid();

            // Assert
            Assert.IsTrue(mensagem_Grupo.Erros.Count > 0);
            Assert.IsTrue(mensagem_Grupo.Erros.Contains("Mensagem não preenchida!"));

        }


        [TestMethod]
        public void MensagemIndividual_Sucesso()
        {
            // Arrange 
            Usuario.Entities.Usuario usuarioBase = new Usuario.Entities.Usuario(Guid.NewGuid(), "terrybogard@fatalfury.com", "Terry", "Terry Bogard", "123456");
            Usuario.Entities.Usuario usuarioRemetente = new Usuario.Entities.Usuario(usuarioBase.Id_Usuario, "terrybogard@fatalfury.com", "Terry", "Terry Bogard", "123456");
            Usuario.Entities.Usuario usuarioDestino = new Usuario.Entities.Usuario(Guid.NewGuid(), "bluemary@fatalfury.com", "Blue Mary", "Blue Mary", "951753");

            Mensagem.Entities.Mensagem mensagem_Individual = new Mensagem.Entities.Mensagem("olá", usuarioRemetente, usuarioRemetente, usuarioDestino);
            // Act 
            mensagem_Individual.IsValid();

            // Assert
            Assert.IsTrue(mensagem_Individual.Erros.Count == 0);

        }

        [TestMethod]
        public void MensagemIndividual_IdUsuariosInvalidos_Erro()
        {
            // Arrange 
            Usuario.Entities.Usuario usuarioRemetente = new Usuario.Entities.Usuario(new Guid(), "terrybogard@fatalfury.com", "Terry", "Terry Bogard", "123456");
            Usuario.Entities.Usuario usuarioDestino = new Usuario.Entities.Usuario(new Guid(), "bluemary@fatalfury.com", "Blue Mary", "Blue Mary", "951753");
            Mensagem.Entities.Mensagem mensagem_Individual = new Mensagem.Entities.Mensagem("olá", usuarioRemetente, usuarioRemetente, usuarioDestino);

            // Act 
            mensagem_Individual.IsValid();

            // Assert
            Assert.IsTrue(mensagem_Individual.Erros.Count >= 1);
            Assert.IsTrue(mensagem_Individual.Erros.Contains("Id do usuário base inválido!"));
            Assert.IsTrue(mensagem_Individual.Erros.Contains("Id do usuário remetente inválido!"));
            Assert.IsTrue(mensagem_Individual.Erros.Contains("Id do usuário destino inválido!"));
        }


        [TestMethod]
        public void MensagemIndividual_TextoEmBranco_Erro()
        {
            // Arrange 
            Usuario.Entities.Usuario usuarioRemetente = new Usuario.Entities.Usuario(Guid.NewGuid(), "terrybogard@fatalfury.com", "Terry", "Terry Bogard", "123456");
            Usuario.Entities.Usuario usuarioDestino = new Usuario.Entities.Usuario(Guid.NewGuid(), "bluemary@fatalfury.com", "Blue Mary", "Blue Mary", "951753");
            Mensagem.Entities.Mensagem mensagem_Individual = new Mensagem.Entities.Mensagem("", usuarioRemetente, usuarioRemetente, usuarioDestino);

            // Act 
            mensagem_Individual.IsValid();

            // Assert
            Assert.IsTrue(mensagem_Individual.Erros.Count >= 1);
            Assert.IsTrue(mensagem_Individual.Erros.Contains("Mensagem não preenchida!"));
        }


        [TestMethod]
        public void MensagemIndividual_RemetenteEnviandoParaRemetente_Erro()
        {
            // Arrange 
            Usuario.Entities.Usuario usuarioRemetente = new Usuario.Entities.Usuario(Guid.NewGuid(), "terrybogard@fatalfury.com", "Terry", "Terry Bogard", "123456");
            Usuario.Entities.Usuario usuarioDestino = new Usuario.Entities.Usuario(usuarioRemetente.Id_Usuario, "terrybogard@fatalfury.com", "Terry", "Terry Bogard", "123456");
            Mensagem.Entities.Mensagem mensagem_Individual = new Mensagem.Entities.Mensagem("Olá!", usuarioRemetente, usuarioRemetente, usuarioDestino);

            // Act 
            mensagem_Individual.IsValid();

            // Assert
            Assert.IsTrue(mensagem_Individual.Erros.Count >= 1);
            Assert.IsTrue(mensagem_Individual.Erros.Contains("Mensagem não pode ser enviada para você mesmo!"));
        }


        [TestMethod]
        public void MensagemIndividual_UsuarioBaseNaoSerRemetenteNemDestino_Erro()
        {
            // Arrange 
            Usuario.Entities.Usuario usuarioBase = new Usuario.Entities.Usuario(Guid.NewGuid(), "gessehoward@fatalfury.com", "Geese", "Geese Howard", "951753");
            Usuario.Entities.Usuario usuarioRemetente = new Usuario.Entities.Usuario(Guid.NewGuid(), "terrybogard@fatalfury.com", "Terry", "Terry Bogard", "123456");
            Usuario.Entities.Usuario usuarioDestino = new Usuario.Entities.Usuario(Guid.NewGuid(), "bluemary@fatalfury.com", "Blue Mary", "Blue Mary", "951753");

            Mensagem.Entities.Mensagem mensagem_Individual = new Mensagem.Entities.Mensagem("Olá!", usuarioBase, usuarioRemetente, usuarioDestino);
            // Act 
            mensagem_Individual.IsValid();

            // Assert
            Assert.IsTrue(mensagem_Individual.Erros.Count >= 1);
            Assert.IsTrue(mensagem_Individual.Erros.Contains("Usuário base não corresponde!"));
        }



    }
}


