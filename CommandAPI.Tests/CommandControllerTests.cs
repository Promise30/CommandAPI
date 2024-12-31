using AutoMapper;
using CommandAPI.Controllers;
using CommandAPI.Dtos;
using CommandAPI.Models;
using CommandAPI.Repos;
using CommandAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework.Legacy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandAPI.Tests
{
    [TestFixture]
    public class CommandControllerTests
    {
        private Mock<ICommandService> _commandService;
        private Mock<IMapper> _mapperMock;
        private CommandsController _controller;
        [SetUp]
        public void SetUp()
        {
            _commandService = new Mock<ICommandService>();
            _mapperMock = new Mock<IMapper>();
            _controller = new CommandsController(_commandService.Object);
        }
        [Test]
        public void GetCommands_ReturnsOkResultWithCommands()
        {
            // Arrange
            var expectedCommands = new List<Command>
            {
               new Command { Id = 1, HowTo= "how to do", CommandLine= "command to", Platform= "platform to" },
                new Command { Id = 2, HowTo= "another how to", CommandLine= "another command to", Platform="another platform to" }
            };
            var commandDtos = expectedCommands.Select(c => new CommandReadDto
            {
                Id = c.Id,
                HowTo = c.HowTo,
                CommandLine = c.CommandLine,
                Platform = c.Platform
            }).ToList();
            _commandService.Setup(service => service.GetCommands());
            _mapperMock.Setup(m => m.Map<IEnumerable<CommandReadDto>>(It.IsAny<IEnumerable<Command>>())).Returns(commandDtos);


            // Act
            var result = _controller.GetAllCommands();

            // Assert
            ClassicAssert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            ClassicAssert.IsNotNull(okResult);
            ClassicAssert.IsInstanceOf<IEnumerable<CommandReadDto>>(okResult.Value);
        }
        [Test]
        public void GetCommandById_ExistingId_ReturnsOkResultWithCommand()
        {
            // Arrange
            var command = new Command { Id = 1, HowTo = "how to do", CommandLine = "command to", Platform = "platform to" };
            var commandDto = new CommandReadDto { Id = command.Id, HowTo = command.HowTo, CommandLine = command.CommandLine, Platform = command.Platform };
            _commandService.Setup(service => service.GetCommandById(1)).Returns(commandDto);
            _mapperMock.Setup(m => m.Map<CommandReadDto>(It.IsAny<Command>())).Returns(commandDto);

            // Act
            var result = _controller.GetCommandById(1);

            // Assert
            ClassicAssert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            ClassicAssert.IsNotNull(okResult);
            ClassicAssert.IsInstanceOf<CommandReadDto>(okResult.Value);
        }

        [Test]
        public void GetCommandById_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            _commandService.Setup(service => service.GetCommandById(1)).Returns((CommandReadDto)null);

            // Act
            var result = _controller.GetCommandById(1);

            // Assert
            ClassicAssert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        [Test]
        public void CreateCommand_ValidCommand_ReturnsNoContent()
        {
            // Arrange
            var commandCreateDto = new CommandCreateDto { HowTo = "how to do", CommandLine = "command to", Platform = "platform to" };

            // Act
            var result = _controller.CreateCommand(commandCreateDto);

            // Assert
            ClassicAssert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public void CreateCommand_InvalidCommand_ReturnsBadRequest()
        {
            // Arrange
            var commandCreateDto = new CommandCreateDto { HowTo = "", CommandLine = "", Platform = "" };
            _controller.ModelState.AddModelError("HowTo", "Required");

            // Act
            var result = _controller.CreateCommand(commandCreateDto);

            // Assert
            ClassicAssert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void UpdateCommand_ValidCommand_ReturnsNoContent()
        {
            // Arrange
            var commandUpdateDto = new CommandUpdateDto { HowTo = "how to do", CommandLine = "command to", Platform = "platform to" };

            // Act
            var result = _controller.UpdateCommand(1, commandUpdateDto);

            // Assert
            ClassicAssert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public void UpdateCommand_InvalidCommand_ReturnsBadRequest()
        {
            // Arrange
            var commandUpdateDto = new CommandUpdateDto { HowTo = "", CommandLine = "", Platform = "" };
            _controller.ModelState.AddModelError("HowTo", "Required");

            // Act
            var result = _controller.UpdateCommand(1, commandUpdateDto);

            // Assert
            ClassicAssert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void DeleteCommand_ExistingId_ReturnsNoContent()
        {
            // Act
            var result = _controller.DeleteCommand(1);

            // Assert
            ClassicAssert.IsInstanceOf<NoContentResult>(result);
        }

    }
}
