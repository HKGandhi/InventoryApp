using InventoryApp.Controllers;
using InventoryBAL.DTO;
using InventoryBAL.Interface;
using InventoryDAL;
using InventoryDAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace InvertoryTest
{
    [TestClass]
    public class InventoryControllerTest
    {
        private Mock<IInventoryRepository> _mockInvRepository;
        private InventoryController _controller;

        [TestInitialize] // Runs before each test method
        public void Setup()
        {
            _mockInvRepository = new Mock<IInventoryRepository>();
            _controller = new InventoryController(_mockInvRepository.Object);
        }

        private List<Invertory> DummyInvetoryList()
        {
            List<Invertory> listinvdata = new List<Invertory>();
            listinvdata.Add(new Invertory { Id = 1, Description = "324324324", ExpireDate = DateTime.Now, Title = "ABC", RemainingCOunt = 5 });
            listinvdata.Add(new Invertory { Id = 2, Description = "GDG", ExpireDate = DateTime.Now, Title = "vcb", RemainingCOunt = 5 });
            listinvdata.Add(new Invertory { Id = 3, Description = "RETTET", ExpireDate = DateTime.Now, Title = "tr5", RemainingCOunt = 0 });

            return listinvdata;

        }

        [TestMethod]
        public async Task  GetAllInvetoryData()
        {

            
            // Set up the mock to return a successful booking response
            _mockInvRepository.Setup(repo => repo.GetAllInventoriesAsync())
                .ReturnsAsync(DummyInvetoryList());

           
            var result = await _controller.GetAllInventories();

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnValue = okResult.Value as List<Invertory>;


            Assert.IsNotNull(returnValue);
            Assert.AreEqual(3, returnValue.Count);
            Assert.AreEqual("ABC", returnValue[0].Title);

        }

        [TestMethod]
        public async Task UploadCSVInventory()
        {
            // Arrange: Prepare a mock file (simulate a CSV file upload)
            var fileName = "inventory.csv";
            var fileContent = "Title,Description,RemainingCOunt,ExpireDate\nBali 1,Suspendisse congue erat ac ex venenatis mattis,20,19-11-2030\nMadeira,risus non mollis sollicitudin,10,10-02-2030";
            var file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes(fileContent)), 0, fileContent.Length, "file", fileName);

            // Arrange the response you expect from the ProcessCSVFileAsync method
            var expectedResponse = new APIResponse
            {
                IsSuccess = true,
                Message = "File processed successfully"
            };

            // Set up the mock to return the expected response
            _mockInvRepository.Setup(repo => repo.ProcessCSVFileAsync(file))
                    .ReturnsAsync(expectedResponse);

            // Act: Call the UploadInventoryCSV method in the controller
            var result = await _controller.UploadInventoryCSV(file);

            // Assert: Verify the result is a successful response (APIResponse)
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual("File processed successfully", result.Message);
        }


        [TestMethod]
        public async Task UploadCSVInventory_Fail()
        {
            // Arrange: Prepare a mock file (simulate a CSV file upload)
            var fileName = "inventory.csv";
            var fileContent = "Title,Description,RemainingCOunt,ExpireDate\nBali 1,Suspendisse congue erat ac ex venenatis mattis,45,20,19-11-2030\nMadeira,risus non mollis sollicitudin,45,10,10-02-2030";
            var file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes(fileContent)), 0, fileContent.Length, "file", fileName);

            // Arrange the response you expect from the ProcessCSVFileAsync method
            var expectedResponse = new APIResponse
            {
                IsSuccess = false,
                Message = "Fail to Read"
            };

            // Set up the mock to return the expected response
            _mockInvRepository.Setup(repo => repo.ProcessCSVFileAsync(file))
                    .ReturnsAsync(expectedResponse);

            // Act: Call the UploadInventoryCSV method in the controller
            var result = await _controller.UploadInventoryCSV(file);

            // Assert: Verify the result is a successful response (APIResponse)
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Fail to Read", result.Message);
        }

        [TestMethod]

        public async Task GetInventoryById()
        {

            var inventoryId = 1;
            var inventory = new Invertory
            {
                Id = inventoryId,
                Description = "Item 1",
                ExpireDate = DateTime.Now,
                RemainingCOunt = 20,
                Title = "Title 1"
            };
            // Set up the mock to return a successful booking response
            _mockInvRepository.Setup(repo => repo.GetInventoryByIdAsync(inventoryId))
                .ReturnsAsync(inventory);


            var result = await _controller.GetInventoryById(1);

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnValue = okResult.Value as Invertory;


            Assert.IsNotNull(returnValue);
            Assert.AreEqual("Title 1", returnValue.Title);
        }

        [TestMethod]
        public async Task DeleteInventory()
        {
          
            var inventoryId = 1;

           
            _mockInvRepository.Setup(repo => repo.DeleteInventoryAsync(inventoryId))
                .ReturnsAsync(true);

           
            var result = await _controller.DeleteInventory(inventoryId);

            
            var noContentResult = result as NoContentResult;
            Assert.IsNotNull(noContentResult);
        }

       
        [TestMethod]
        public async Task DeleteInventory_DoesNotExist()
        {
         
            var inventoryId = 999;

         
            _mockInvRepository.Setup(repo => repo.DeleteInventoryAsync(inventoryId))
                .ReturnsAsync(false);

        
            var result = await _controller.DeleteInventory(inventoryId);

           
            var notFoundResult = result as NotFoundResult;
            Assert.IsNotNull(notFoundResult); 
        }
    }


}

