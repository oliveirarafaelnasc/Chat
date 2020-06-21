using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using RO.Chat.IO.Domain.Usuario.ValueObject;

namespace RO.Chat.IO.Domain.Test.Entities
{


    [TestClass]
    public class UsuarioTests
    {
        [TestMethod]
        public void Usuario_Valido()
        {
            // Arrange 
            var usuario = new Usuario.Entities.Usuario(Guid.NewGuid(), "joe_higashi@outlook.com", "Higashi", "Joe Higashi", "123456");
            // Act 
            // Assert
            Assert.IsTrue(usuario.IsValid);
            Assert.IsTrue(usuario.Erros.Count == 0);
        }

        [TestMethod]
        public void Usuario_Invalido_EmailNãoPreenchido()
        {
            // Arrange 
            var usuario = new Usuario.Entities.Usuario(new Guid(), "", "Higashi", "Joe Higashi", "123456");
            // Act 
            // Assert
            Assert.IsFalse(usuario.IsValid);
            Assert.IsTrue(usuario.Erros.Count > 0);
        }

        [TestMethod]
        public void Usuario_Invalido_MsgEmailNaoPreenchido()
        {
            // Arrange 
            var usuario = new Usuario.Entities.Usuario(new Guid(), "", "Higashi", "Joe Higashi", "123456");
            // Act 

            // Assert
            Assert.IsFalse(usuario.IsValid);
            Assert.IsTrue(usuario.Erros.Contains("E-mail não preenchido!"));
        }

        [TestMethod]
        public void Usuario_Invalido_MsgEmailPreenchidoErrado()
        {
            // Arrange 
            var usuario = new Usuario.Entities.Usuario(new Guid(), "joe_higashi@", "Higashi", "Joe Higashi", "123456");
            // Act 


            // Assert
            Assert.IsFalse(usuario.IsValid);
            Assert.IsTrue(usuario.Erros.Contains("E-mail inválido!"));
        }

        [TestMethod]
        public void Usuario_Invalido_NomeUsuarioNaoPreechido()
        {
            // Arrange 
            var usuario = new Usuario.Entities.Usuario(new Guid(), "joe_higashi@fatalfury.com", "", "Joe Higashi", "123456");
            // Act 
            // Assert
            Assert.IsFalse(usuario.IsValid);
            Assert.IsTrue(usuario.Erros.Count > 0);
        }

        [TestMethod]
        public void Usuario_Invalido_MsgNomeUsuarioNaoPreechido()
        {
            // Arrange 
            var usuario = new Usuario.Entities.Usuario(new Guid(), "joe_higashi@fatalfury.com", "", "Joe Higashi", "123456");
            // Act 
            // Assert
            Assert.IsFalse(usuario.IsValid);
            Assert.IsTrue(usuario.Erros.Contains("Nome de usuário não preenchido!"));
        }


        [TestMethod]
        public void Usuario_Invalido_NomeNaoPreechido()
        {
            // Arrange 
            var usuario = new Usuario.Entities.Usuario(new Guid(), "joe_higashi@fatalfury.com", "Higashi", "", "123456");
            // Act 
            // Assert
            Assert.IsFalse(usuario.IsValid);
            Assert.IsTrue(usuario.Erros.Count > 0);
        }

        [TestMethod]
        public void Usuario_Invalido_MsgNomeNaoPreechido()
        {
            // Arrange 
            var usuario = new Usuario.Entities.Usuario(new Guid(), "joe_higashi@fatalfury.com", "Higashi", "", "123456");
            // Act 
            // Assert
            Assert.IsFalse(usuario.IsValid);
            Assert.IsTrue(usuario.Erros.Contains("Nome não preenchido!"));
        }

        [TestMethod]
        public void Usuario_Invalido_SenhaNaoPreechida()
        {
            // Arrange 
            var usuario = new Usuario.Entities.Usuario(new Guid(), "joe_higashi@fatalfury.com", "Higashi", "Joe Higashi", "");
            // Act 
            // Assert
            Assert.IsFalse(usuario.IsValid);
            Assert.IsTrue(usuario.Erros.Count > 0);
        }

        [TestMethod]
        public void Usuario_Invalido_MsgSenhaNaoPreechida()
        {
            // Arrange 
            var usuario = new Usuario.Entities.Usuario(new Guid(), "joe_higashi@fatalfury.com", "Higashi", "Joe Higashi", "");
            // Act 
            // Assert
            Assert.IsFalse(usuario.IsValid);
            Assert.IsTrue(usuario.Erros.Contains("Senha não preenchida!"));
        }


        [TestMethod]
        public void Usuario_Invalido_TodosCamposInvalidos()
        {
            // Arrange 
            var usuario = new Usuario.Entities.Usuario(new Guid(), "", "", "", "");
            // Act 
            // Assert
            Assert.IsFalse(usuario.IsValid);
            Assert.IsTrue(usuario.Erros.Contains("E-mail não preenchido!"));
            Assert.IsTrue(usuario.Erros.Contains("E-mail inválido!"));
            Assert.IsTrue(usuario.Erros.Contains("Nome de usuário não preenchido!"));
            Assert.IsTrue(usuario.Erros.Contains("Nome não preenchido!"));
            Assert.IsTrue(usuario.Erros.Contains("Senha não preenchida!"));
        }

        [TestMethod]
        public void Usuario_Sucesso_SenhaIguais()
        {
            // Arrange 
            var usuario = new Usuario.Entities.Usuario(Guid.NewGuid(), "joe_higashi@fatalfury.com", "Higashi", "Joe Higashi", "123456");
            // Act 
            usuario.CriptografarSenha();
            string Senha = Criptografia.GerarHash("123456");
            // Assert
            Assert.IsTrue(usuario.IsValid);
            Assert.AreEqual(usuario.Senha, Senha);
        }
    }
}
