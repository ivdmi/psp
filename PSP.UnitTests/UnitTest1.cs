using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Web.Mvc;
using PSP.WebUI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;
using PSP.Domain;
using PSP.Domain.Abstract;
using PSP.Domain.Concrete;
using PSP.Domain.Service;
using PSP.WebUI.Controllers;
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


        // BaseUserController

        // Index
        [TestMethod]
        public void Can_GetBaseUsers()
        {
            // Arrange
            Mock<IRepository> mockRepository = new Mock<IRepository>();
            mockRepository.Setup(mRepo => mRepo.BaseUsers).Returns(new baseusers[]
            {
                new baseusers
                {
                    Login = "Login",
                    Password = "Password",
                    ID = "d1f7908b-6967-42c2-be5d-b4f157168ac9"
                },
                new baseusers
                {
                    Login = "guest",
                    Password = "guest-Гость",
                    ID = "da9e2795-4818-4606-883f-8cf7d0a9c2fb"
                },
                new baseusers
                {
                    Login = "sushko",
                    Password = "121277",
                    ID = "713e77ca-d526-49b1-8e34-8b71bc96bcd4"
                }
            }.AsQueryable());
            
            
            // Create controller
            BaseUserController controller = new BaseUserController(mockRepository.Object);
            
            // Act
            var resultIndex = (IList<baseusers>)controller.Index().Model;
            var resultDetails = (baseusers)controller.Details("da9e2795-4818-4606-883f-8cf7d0a9c2fb").Model;

            // Assert Index
            Assert.AreEqual(resultIndex.Count, 3);
            Assert.AreEqual(resultIndex[0].Login, "Login");
            Assert.AreEqual(resultIndex[2].ID, "713e77ca-d526-49b1-8e34-8b71bc96bcd4");

            // Assert Details
            Assert.AreEqual(resultDetails.Login, "guest");
        }

        // BaseUserService

        // Index
        [TestMethod]
        public void Can_BaseUserService()
        {
            // Arrange
            Mock<IRepository> mockRepository = new Mock<IRepository>();
            mockRepository.Setup(mRepo => mRepo.BaseUsers).Returns(new baseusers[]
            {
                new baseusers
                {
                    Login = "Login",
                    Password = "Password",
                    ID = "d1f7908b-6967-42c2-be5d-b4f157168ac9"
                },
                new baseusers
                {
                    Login = "guest",
                    Password = "guest-Гость",
                    ID = "da9e2795-4818-4606-883f-8cf7d0a9c2fb"
                },
                new baseusers
                {
                    Login = "sushko",
                    Password = "121277",
                    ID = "713e77ca-d526-49b1-8e34-8b71bc96bcd4"
                }
            }.AsQueryable());

            // Create service
            BaseUsersService target = new BaseUsersService(mockRepository.Object);

            // Act
            baseusers user = target.GetUser("da9e2795-4818-4606-883f-8cf7d0a9c2fb");
 //           target.RemoveUser("d1f7908b-6967-42c2-be5d-b4f157168ac9");
            target.AddUser(new baseusers(){Login = "NewUser", Password = "EmptyPassword"});
           
            // Assert

            // GetUser
            Assert.AreEqual(user.Login, "guest");

            // RemoveUser, GetAllBaseUsers
            Assert.AreEqual(target.GetAllBaseUsers().Count, 3);
            Assert.AreEqual(target.GetAllBaseUsers()[0].Login, "guest");
            Assert.AreEqual(target.GetAllBaseUsers()[2].Login, "NewUser");
        }

        [TestMethod]
        public void Can_CreateRepository()
        {
            var mockSet = new Mock<DbSet<baseusers>>();
            var mockContext = new Mock<PspEnty>();
            
            mockContext.Setup(m => m._baseusers).Returns(mockSet.Object);
            
            IRepository reposuitory = new Repository();
   //         reposuitory.Context = mockContext.Object;
            var service = new BaseUsersService(reposuitory); 
            service.AddUser(new baseusers(){Login = "NewUser-0", Password = "EmptyPassword"});
            service.AddUser(new baseusers() { Login = "NewUser-1", Password = "Password-1" });

            mockSet.Verify(m => m.Add(It.IsAny<baseusers>()), Times.Once()); 
            mockContext.Verify(m => m.SaveChanges(), Times.Once()); 
        } 

    }
}
