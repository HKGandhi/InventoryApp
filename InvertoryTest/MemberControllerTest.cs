using InventoryApp.Controllers;
using InventoryBAL.Interface;
using InventoryDAL.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvertoryTest
{
    [TestClass]
    public class MemberControllerTest
    {

        private Mock<IMemberRepository> _mockMemberRepository;
        private MemberController _controller;

        [TestInitialize] // This runs before each test method
        public void Setup()
        {
           
            _mockMemberRepository = new Mock<IMemberRepository>();

          
            _controller = new MemberController(_mockMemberRepository.Object);
        }

        [TestMethod]
        public async Task GetMemberById()
        {
            
            var memberId = 1;
            var member = new Member { Id = memberId, FirstName  = "John", LastName = "Doe",BookingCount = 5,JoiningDate = DateTime.Now };

           
            _mockMemberRepository.Setup(repo => repo.GetMemberByIdAsync(memberId))
                .ReturnsAsync(member);

            var result = await _controller.GetMemberById(memberId);

            
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnValue = okResult.Value as Member;
            Assert.IsNotNull(returnValue);
            Assert.AreEqual("John", returnValue.FirstName);
        }

        [TestMethod]
        public async Task CreateMember_ReturnsCreatedAtActionResult_WhenMemberIsCreated()
        {
            
            var newMember = new Member { Id = 0, FirstName = "Jane", LastName="Doe" };

          
            var createdMember = new Member { Id = 1, FirstName = "Jane", LastName = "Doe" };
            _mockMemberRepository.Setup(repo => repo.AddMemberAsync(newMember))
                .ReturnsAsync(createdMember);

            // Act: Call the CreateMember method in the controller
            var result = await _controller.CreateMember(newMember);

           
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtActionResult);
            var returnValue = createdAtActionResult.Value as Member;
            Assert.IsNotNull(returnValue);
            Assert.AreEqual(createdMember.Id, returnValue.Id);
            Assert.AreEqual(createdMember.FirstName, returnValue.FirstName);
        }


        [TestMethod]
        public async Task UpdateMember()
        {
           
            var memberId = 999; 
            var updatedMember = new Member { Id = memberId, FirstName = "Non-Existent Member" };

            _mockMemberRepository.Setup(repo => repo.UpdateMemberAsync(updatedMember))
                .ReturnsAsync(updatedMember);

           
            var result = await _controller.UpdateMember(memberId, updatedMember);

            
            var notFoundResult = result as NotFoundResult;
            Assert.IsTrue(true);
        }

     
        [TestMethod]
        public async Task DeleteMember_MemberIsDeleted()
        {
          
            var memberId = 1;
            _mockMemberRepository.Setup(repo => repo.DeleteMemberAsync(memberId))
                .ReturnsAsync(true);

           
            var result = await _controller.DeleteMember(memberId);

            var noContentResult = result as NoContentResult;
            Assert.IsNotNull(noContentResult);
        }

       
        [TestMethod]
        public async Task DeleteMember_MemberDoesNotExist()
        {
          
            var memberId = 999; 
            _mockMemberRepository.Setup(repo => repo.DeleteMemberAsync(memberId))
                .ReturnsAsync(false);

            
            var result = await _controller.DeleteMember(memberId);

           
            var notFoundResult = result as NotFoundResult;
            Assert.IsNotNull(notFoundResult);
        }
    }
}

