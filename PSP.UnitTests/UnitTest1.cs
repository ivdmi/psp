using System;
using System.Linq;
using PSP.WebUI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// using NUnit.Framework;

using Moq;
using PSP.Domain;
using PSP.Domain.Abstract;
using PSP.WebUI.Helpers;
using Assert = NUnit.Framework.Assert;

namespace PSP.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_GetTreeView()
        {
            // Arrange
            Mock<IRepository> mockRepository = new Mock<IRepository>();
            mockRepository.Setup(mRepo => mRepo.Users).Returns(new users[]
            {
                new users
                {
                    ID = "f1748b39-5b99-453a-af29-110ba468db74",
                    Name = "Асса Н.",
                    GroupID = "c2d956fd-4092-4b74-acf1-f05dee72e512"
                },
                new users
                {
                    ID = "9ece259f-5ede-4146-8f1a-aad025554433",
                    Name = "Салова Н.А.",
                    GroupID = "59da6b00-8b85-4179-9199-99aaa9002daa"
                },
                new users
                {
                    ID = "36d001bb-d12a-4229-8b1c-12f29ed385e5",
                    Name = "Саламаха Е.П.",
                    GroupID = "6d76a185-8319-4912-93d4-268acf16c698"
                },
                new users
                {
                    ID = "ef68fbec-7fd3-4046-a721-c60e39961eb4",
                    Name = "Ильченко И.М.",
                    GroupID = "c2d956fd-4092-4b74-acf1-f05dee72e512"
                }
            }.AsQueryable());

            mockRepository.Setup(mRepo => mRepo.Groups).Returns(new groups[]
            {
                new groups
                {
                    ID = "c2d956fd-4092-4b74-acf1-f05dee72e512",
                    Name = "Аудит"
                },
                new groups
                {
                    ID = "6d76a185-8319-4912-93d4-268acf16c698",
                    Name = "Методист"
                },
                new groups
                {
                    ID = "59da6b00-8b85-4179-9199-99aaa9002daa",
                    Name = "Учет"
                }
            }.AsQueryable());

            UsersTreeView testTreeView = new UsersTreeView(mockRepository.Object);

            // Act
            var testTreeViewData = testTreeView.GetTreeViewData();
            
            // Assert
            var treeViewDataArray = testTreeViewData.ToArray();
            Assert.AreEqual(treeViewDataArray[0].Name, "Аудит");
            Assert.AreEqual(treeViewDataArray[0].Id, "c2d956fd-4092-4b74-acf1-f05dee72e512");
            Assert.AreEqual(treeViewDataArray[0].List.Count, 2);
            Assert.AreEqual(treeViewDataArray[0].List[0].Name, "Асса Н.");
            Assert.AreEqual(treeViewDataArray[0].List[1].Name, "Ильченко И.М.");
            Assert.AreEqual(treeViewDataArray[1].List[0].Name, "Саламаха Е.П.");
            Assert.AreEqual(treeViewDataArray[2].List[0].Name, "Салова Н.А.");
        }
    }
}
