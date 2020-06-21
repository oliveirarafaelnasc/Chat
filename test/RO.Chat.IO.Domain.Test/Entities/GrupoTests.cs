using Microsoft.VisualStudio.TestTools.UnitTesting;
using RO.Chat.IO.Domain.Mensagem.Entities;
using System.Linq;

namespace RO.Chat.IO.Domain.Test.Entities
{


    [TestClass]
    public class GrupoTests
    {
        [TestMethod]
        public void Grupo_Valido()
        {
            // Arrange 
            var grupo = new Grupo("Fatal Fury");
            // Act 
            grupo.IsValid();

            // Assert
            Assert.IsTrue(grupo.Erros.Count == 0);
        }

        [TestMethod]
        public void Grupo_Invalido_NomeNãoPreenchido()
        {
            // Arrange 
            var grupo = new Grupo("");
            // Act 
            grupo.IsValid();

            // Assert
            Assert.IsTrue(grupo.Erros.Count > 0);
        }

        [TestMethod]
        public void Grupo_Invalido_NomeNãoPreenchidoMsg()
        {
            // Arrange 
            var grupo = new Grupo("");
            // Act 
            grupo.IsValid();

            // Assert
            Assert.IsTrue(grupo.Erros.Count > 0);
            Assert.IsTrue(grupo.Erros.Contains("Nome do grupo não preenchido!"));
        }


    }
}
