using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RO.Chat.IO.Domain.Interfaces;
using Rhino.Mocks;
using RO.Chat.IO.Domain.Usuario.Services;
using RO.Chat.IO.Domain.Usuario.Interfaces;
using RO.Chat.IO.Domain.Usuario.ValueObject;

namespace RO.Chat.IO.Domain.Test.Services
{
    [TestClass]
    public class UsuarioServiceTest
    {
        private List<Usuario.Entities.Usuario> _fakeUsuarios = new List<Usuario.Entities.Usuario>
        {
            new Usuario.Entities.Usuario(Guid.NewGuid(), "joe_higashi@outlook.com", "Joe Higashi", "Joe Higashi", "123456"),
            new Usuario.Entities.Usuario(Guid.NewGuid(), "andybogard@fatalfury.com", "Andy", "Andy Bogard", "123456"),
            new Usuario.Entities.Usuario(Guid.Parse("81E93CAA-C63D-439C-9478-09AC47318068"), "terrybogard@fatalfury.com", "Terry", "Terry Bogard", "123456"),
            new Usuario.Entities.Usuario(Guid.NewGuid(), "bluemary@fatalfury.com", "Blue Mary", "Blue Mary", "951753"),
            new Usuario.Entities.Usuario(Guid.NewGuid(), "gessehoward@fatalfury.com", "Geese", "Geese Howard", "951753")
        };

        [TestMethod]
        public void Usuario_AdicionarUsuario_Sucesso()
        {
            // Arrange
            Usuario.Entities.Usuario usuario = new Usuario.Entities.Usuario(Guid.NewGuid(), "joe_higashi@outlook.com", "Higashi", "Joe Higashi", "123456");

            // Act
            var repo = MockRepository.GenerateStub<IUsuarioRepository>();
            var uow = MockRepository.GenerateStub<IUnitOfWork>();
            repo.Stub(s => s.ValidarUsuarioUnico(usuario)).Return(null);
            repo.Stub(s => s.Adicionar(usuario)).Return(usuario);
            uow.Stub(s => s.Commit()).Return(1);
            var usuarioReturn = new UsuarioService(repo, uow).AdicionarUsuario(usuario);

            // Assert
            
            Assert.AreEqual(usuarioReturn.Erros.Count, 0);
            uow.AssertWasCalled(x => x.Commit(), x => x.Repeat.Once());
        }

        [TestMethod]
        public void Usuario_UsuarioEmailInvalido_Erro()
        {
            // Arrange
            Usuario.Entities.Usuario usuario = new Usuario.Entities.Usuario(Guid.NewGuid(), "", "Higashi", "Joe Higashi", "123456");

            // Act
            var repo = MockRepository.GenerateStub<IUsuarioRepository>();
            var uow = MockRepository.GenerateStub<IUnitOfWork>();
            repo.Stub(s => s.ValidarUsuarioUnico(usuario)).Return(null);
            repo.Stub(s => s.Adicionar(usuario)).Return(usuario);
            var usuarioReturn = new UsuarioService(repo, uow).AdicionarUsuario(usuario);

            // Assert
            Assert.AreNotEqual(usuarioReturn.Erros.Count, 0);
            uow.AssertWasNotCalled(x => x.Commit());
        }


        [TestMethod]
        public void Usuario_UsuarioNomeInvalido_Erro()
        {
            // Arrange
            Usuario.Entities.Usuario usuario = new Usuario.Entities.Usuario(Guid.NewGuid(), "joe_higashi@outlook.com", "", "Joe Higashi", "123456");

            // Act
            var repo = MockRepository.GenerateStub<IUsuarioRepository>();
            var uow = MockRepository.GenerateStub<IUnitOfWork>();
            repo.Stub(s => s.ValidarUsuarioUnico(usuario)).Return(null);
            repo.Stub(s => s.Adicionar(usuario)).Return(usuario);
            var usuarioReturn = new UsuarioService(repo, uow).AdicionarUsuario(usuario);

            // Assert
            Assert.AreNotEqual(usuarioReturn.Erros.Count, 0);
            uow.AssertWasNotCalled(x => x.Commit());
        }


        [TestMethod]
        public void Usuario_EmailJaExiste_Erro()
        {
            // Arrange
            Usuario.Entities.Usuario usuario = new Usuario.Entities.Usuario(Guid.NewGuid(), "joe_higashi@outlook.com", "Joe Higashi", "Joe Higashi", "123456");

            // Act
            var repo = MockRepository.GenerateStub<IUsuarioRepository>();
            var uow = MockRepository.GenerateStub<IUnitOfWork>();
            
            repo.Stub(s => s.ValidarUsuarioUnico(usuario)).Return(new Usuario.Entities.Usuario(Guid.NewGuid(), "joe_higashi@outlook.com", "Geese", "Geese Howard", "951753"));
            repo.Stub(s => s.Adicionar(usuario)).Return(usuario);
            var usuarioReturn = new UsuarioService(repo, uow).AdicionarUsuario(usuario);

            // Assert
            Assert.AreNotEqual(usuarioReturn.Erros.Count, 0);
            uow.AssertWasNotCalled(x => x.Commit());
        }


        [TestMethod]
        public void Usuario_NomeUsuarioJaExiste_Erro()
        {
            // Arrange
            Usuario.Entities.Usuario usuario = new Usuario.Entities.Usuario(Guid.NewGuid(), "joe_higashi@outlook.com", "Joe Higashi", "Joe Higashi", "123456");

            // Act
            var repo = MockRepository.GenerateStub<IUsuarioRepository>();
            var uow = MockRepository.GenerateStub<IUnitOfWork>();

            repo.Stub(s => s.ValidarUsuarioUnico(usuario)).Return(new Usuario.Entities.Usuario(Guid.NewGuid(), "joehigashi@outlook.com", "Joe Higashi", "Joe Higashy", "951753"));
            repo.Stub(s => s.Adicionar(usuario)).Return(usuario);
            var usuarioReturn = new UsuarioService(repo, uow).AdicionarUsuario(usuario);

            // Assert
            Assert.AreNotEqual(usuarioReturn.Erros.Count, 0);
            uow.AssertWasNotCalled(x => x.Commit());
        }


        [TestMethod]
        public void Usuario_ExecutarLogin_Sucesso()
        {
            // Arrange
            string email = "bluemary@fatalfury.com";
            string senha = "951753";

            Usuario.Entities.Usuario usuarioExistente = new Usuario.Entities.Usuario(Guid.NewGuid(), "bluemary@fatalfury.com", "Blue Mary", "Blue Mary", Criptografia.GerarHash(senha));

            // Act
            var repo = MockRepository.GenerateStub<IUsuarioRepository>();
            var uow = MockRepository.GenerateStub<IUnitOfWork>();

            repo.Stub(s => s.ObterUsuarioPorEmailEhSenha(email, Criptografia.GerarHash(senha))).Return(usuarioExistente);
            var usuarioReturn = new UsuarioService(repo, uow).ObterUsuarioPorEmailEhSenha(email, senha);
            string senhaCriptograda = Criptografia.GerarHash(senha);
            // Assert
            Assert.IsTrue(usuarioExistente.IsValid);
            Assert.AreEqual(usuarioReturn.Erros.Count, 0);
            Assert.AreEqual(usuarioReturn.Senha, senhaCriptograda);
            Assert.AreEqual(usuarioReturn.Email, email);


        }

        [TestMethod]
        public void Usuario_ExecutarLoginSenhaInvalida_Erro()
        {
            // Arrange
            string email = "bluemary@fatalfury.com";
            string senha = "95175";

            // Act
            var repo = MockRepository.GenerateStub<IUsuarioRepository>();
            var uow = MockRepository.GenerateStub<IUnitOfWork>();

            repo.Stub(s => s.ObterUsuarioPorEmailEhSenha(email, senha)).Return(null);
            var usuarioReturn = new UsuarioService(repo, uow).ObterUsuarioPorEmailEhSenha(email, senha);
            string senhaCriptograda = Criptografia.GerarHash(senha);

            // Assert
            Assert.AreNotEqual(usuarioReturn.Erros.Count, 0);
            Assert.AreNotEqual(usuarioReturn.Email, Criptografia.GerarHash(senha));
        }


    }
}
