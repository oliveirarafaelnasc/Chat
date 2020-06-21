using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using RO.Chat.IO.Domain.Interfaces;
using RO.Chat.IO.Domain.Mensagem.Entities;
using RO.Chat.IO.Domain.Mensagem.Interfaces;
using RO.Chat.IO.Domain.Mensagem.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RO.Chat.IO.Domain.Test.Services
{
    [TestClass]
    public class MensagemServiceTest
    {

        [TestMethod]
        public void Grupo_AdicionarGrupo_Sucesso()
        {
            // Arrange
            Grupo grupo = new Grupo("Fatal Fury");

            // Act
            var repo = MockRepository.GenerateStub<IMensagemRepository>();
            var uow = MockRepository.GenerateStub<IUnitOfWork>();
            repo.Stub(s => s.AdicionarGrupo(grupo)).Return(grupo);
            repo.Stub(s => s.ObterGrupoPorNome(grupo.Nome)).Return(null);
            MensagemService mensagemService = new MensagemService(repo, uow);
            grupo = mensagemService.AdicionarGrupo(grupo);

            // Assert
            Assert.AreEqual(grupo.Erros.Count, 0);
            uow.AssertWasCalled(x => x.Commit(), x => x.Repeat.Once());
        }

        [TestMethod]
        public void Grupo_NomeDoGrupoNaoInformado_Erro()
        {
            // Arrange
            Grupo grupo = new Grupo("");

            // Act
            var repo = MockRepository.GenerateStub<IMensagemRepository>();
            var uow = MockRepository.GenerateStub<IUnitOfWork>();
            repo.Stub(s => s.AdicionarGrupo(grupo)).Return(null);
            repo.Stub(s => s.ObterGrupoPorNome(grupo.Nome)).Return(null);
            MensagemService mensagemService = new MensagemService(repo, uow);
            grupo = mensagemService.AdicionarGrupo(grupo);

            // Assert
            Assert.IsTrue(grupo.Erros.Count > 0);
            uow.AssertWasNotCalled(x => x.Commit(), x => x.Repeat.Once());
        }

        [TestMethod]
        public void Grupo_Adicionar_GrupoJaExistente_Erro()
        {
            // Arrange
            Grupo grupo = new Grupo("Fatal Fury");

            // Act

            Grupo grupoExistente = new Grupo("Fatal Fury");

            var repo = MockRepository.GenerateStub<IMensagemRepository>();
            var uow = MockRepository.GenerateStub<IUnitOfWork>();
            repo.Stub(s => s.AdicionarGrupo(grupo)).Return(grupo);
            repo.Stub(s => s.ObterGrupoPorNome(grupo.Nome)).Return(grupoExistente);
            MensagemService mensagemService = new MensagemService(repo, uow);
            grupo = mensagemService.AdicionarGrupo(grupo);

            // Assert
            Assert.IsTrue(grupo.Erros.Count > 0);
            uow.AssertWasNotCalled(x => x.Commit(), x => x.Repeat.Once());
            Assert.IsTrue(grupo.Erros.Contains("Grupo já existe"));
        }


        [TestMethod]
        public void Grupo_ObterTodos_Sucesso()
        {
            // Arrange
            List<Grupo> grupos = new List<Grupo>();
            grupos.Add(new Grupo("Fatal Fury"));
            grupos.Add(new Grupo("Art of Fighting"));
            grupos.Add(new Grupo("the king of fighters"));

            // Act
            var repo = MockRepository.GenerateStub<IMensagemRepository>();
            var uow = MockRepository.GenerateStub<IUnitOfWork>();

            repo.Stub(s => s.ObterTodosGrupos()).Return(grupos);
            MensagemService mensagemService = new MensagemService(repo, uow);
            var resultado = mensagemService.ObterTodosGrupos();

            // Assert
            Assert.AreEqual(resultado.Count, 3);
            uow.AssertWasNotCalled(x => x.Commit(), x => x.Repeat.Once());
        }

        [TestMethod]
        public void MensagemIndividual_Enviar_Sucesso()
        {
            // Arrange
            Usuario.Entities.Usuario usuarioAndy = new Usuario.Entities.Usuario(Guid.NewGuid(), "andybogard@fatalfury.com", "Andy", "Andy Bogard", "123456");
            Usuario.Entities.Usuario usuarioMai = new Usuario.Entities.Usuario(Guid.NewGuid(), "maishiranui@fatalfury.com", "Mai", "Mai Shiranui", "123456");


            Mensagem.Entities.Mensagem mensagemAndy = new Mensagem.Entities.Mensagem("Olá, Mai!", usuarioAndy, usuarioAndy, usuarioMai);
            Mensagem.Entities.Mensagem mensagemMai = new Mensagem.Entities.Mensagem("Olá, Mai!", usuarioMai, usuarioAndy, usuarioMai);

            // Act
            var repo = MockRepository.GenerateStub<IMensagemRepository>();
            var uow = MockRepository.GenerateStub<IUnitOfWork>();
            MensagemService mensagemService = new MensagemService(repo, uow);
            repo.Stub(s => s.Adicionar(mensagemAndy)).Return(mensagemAndy);
            mensagemAndy = mensagemService.AdicionarMensagem(mensagemAndy);
            repo.Stub(s => s.Adicionar(mensagemMai)).Return(mensagemMai);
            mensagemMai = mensagemService.AdicionarMensagem(mensagemMai);

            // Assert
            Assert.AreEqual(mensagemAndy.Erros.Count, 0);
            Assert.AreEqual(mensagemMai.Erros.Count, 0);
            uow.AssertWasCalled(x => x.Commit(), x => x.Repeat.Twice());
        }

        [TestMethod]
        public void MensagemIndividual_MensagemEmBranco_Erro()
        {
            // Arrange
            Usuario.Entities.Usuario usuarioAndy = new Usuario.Entities.Usuario(Guid.NewGuid(), "andybogard@fatalfury.com", "Andy", "Andy Bogard", "123456");
            Usuario.Entities.Usuario usuarioMai = new Usuario.Entities.Usuario(Guid.NewGuid(), "maishiranui@fatalfury.com", "Mai", "Mai Shiranui", "123456");


            Mensagem.Entities.Mensagem mensagemAndy = new Mensagem.Entities.Mensagem("", usuarioAndy, usuarioAndy, usuarioMai);
            Mensagem.Entities.Mensagem mensagemMai = new Mensagem.Entities.Mensagem("", usuarioMai, usuarioAndy, usuarioMai);

            // Act
            var repo = MockRepository.GenerateStub<IMensagemRepository>();
            var uow = MockRepository.GenerateStub<IUnitOfWork>();
            MensagemService mensagemService = new MensagemService(repo, uow);
            repo.Stub(s => s.Adicionar(mensagemAndy)).Return(mensagemAndy);
            mensagemAndy = mensagemService.AdicionarMensagem(mensagemAndy);
            repo.Stub(s => s.Adicionar(mensagemMai)).Return(mensagemMai);
            mensagemMai = mensagemService.AdicionarMensagem(mensagemMai);

            // Assert
            Assert.IsTrue(mensagemAndy.Erros.Count > 0);
            Assert.IsTrue(mensagemMai.Erros.Count > 0);
            uow.AssertWasNotCalled(x => x.Commit(), x => x.Repeat.Once());
        }

        [TestMethod]
        public void MensagemIndividual_UsuarioDestinoInvalido_Erro()
        {
            // Arrange
            Usuario.Entities.Usuario usuarioAndy = new Usuario.Entities.Usuario(Guid.NewGuid(), "andybogard@fatalfury.com", "Andy", "Andy Bogard", "123456");
            Usuario.Entities.Usuario usuarioMai = new Usuario.Entities.Usuario(new Guid(), string.Empty, string.Empty, string.Empty, string.Empty);


            Mensagem.Entities.Mensagem mensagemAndy = new Mensagem.Entities.Mensagem("", usuarioAndy, usuarioAndy, usuarioMai);
            Mensagem.Entities.Mensagem mensagemMai = new Mensagem.Entities.Mensagem("", usuarioMai, usuarioAndy, usuarioMai);

            // Act
            var repo = MockRepository.GenerateStub<IMensagemRepository>();
            var uow = MockRepository.GenerateStub<IUnitOfWork>();
            MensagemService mensagemService = new MensagemService(repo, uow);
            repo.Stub(s => s.Adicionar(mensagemAndy)).Return(mensagemAndy);
            mensagemAndy = mensagemService.AdicionarMensagem(mensagemAndy);
            repo.Stub(s => s.Adicionar(mensagemMai)).Return(mensagemMai);
            mensagemMai = mensagemService.AdicionarMensagem(mensagemMai);

            // Assert
            Assert.IsTrue(mensagemAndy.Erros.Count > 0);
            Assert.IsTrue(mensagemMai.Erros.Count > 0);
            uow.AssertWasNotCalled(x => x.Commit(), x => x.Repeat.Once());
        }


        [TestMethod]
        public void MensageGrupo_Enviar_Sucesso()
        {
            // Arrange
            Usuario.Entities.Usuario usuarioGesse = new Usuario.Entities.Usuario(Guid.NewGuid(), "gessehoward@fatalfury.com", "Geese", "Geese Howard", "951753");
            Grupo grupo = new Grupo(1,"Fatal Fury");
            //mockContext.SetUp(c => c.Grupo).Returns(grupo);
            Mensagem.Entities.Mensagem mensagem = new Mensagem.Entities.Mensagem("Olá!", usuarioGesse, grupo);
            

            // Act
            var repo = MockRepository.GenerateStub<IMensagemRepository>();
            var uow = MockRepository.GenerateStub<IUnitOfWork>();
            MensagemService mensagemService = new MensagemService(repo, uow);
            repo.Stub(s => s.Adicionar(mensagem)).Return(mensagem);
            mensagem = mensagemService.AdicionarMensagem(mensagem);

            // Assert
            Assert.AreEqual(mensagem.Erros.Count, 0);
            uow.AssertWasCalled(x => x.Commit(), x => x.Repeat.Once());
        }

        [TestMethod]
        public void MensageGrupo_EnviarGrupoInvalido_Erro()
        {
            // Arrange
            Usuario.Entities.Usuario usuarioGesse = new Usuario.Entities.Usuario(Guid.NewGuid(), "gessehoward@fatalfury.com", "Geese", "Geese Howard", "951753");
            Grupo grupo = new Grupo("Fatal Fury");
            //mockContext.SetUp(c => c.Grupo).Returns(grupo);
            Mensagem.Entities.Mensagem mensagem = new Mensagem.Entities.Mensagem("Olá!", usuarioGesse, grupo);


            // Act
            var repo = MockRepository.GenerateStub<IMensagemRepository>();
            var uow = MockRepository.GenerateStub<IUnitOfWork>();
            MensagemService mensagemService = new MensagemService(repo, uow);
            repo.Stub(s => s.Adicionar(mensagem)).Return(mensagem);
            mensagem = mensagemService.AdicionarMensagem(mensagem);

            // Assert
            Assert.IsTrue(mensagem.Erros.Count > 0);
            uow.AssertWasNotCalled(x => x.Commit(), x => x.Repeat.Once());
        }

    }
}
