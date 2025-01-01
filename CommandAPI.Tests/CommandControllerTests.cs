using AutoMapper;
using CommandAPI.Controllers;
using CommandAPI.Dtos;
using CommandAPI.Models;
using CommandAPI.Profiles;
using CommandAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework.Legacy;
using System.Net;

namespace CommandAPI.Tests
{
    [TestFixture]
    public class CommandControllerTests
    {
        private Mock<ICommandService> _commandService;
        private IMapper _mapper;
        CommandsProfile _realProfile;
        MapperConfiguration _configuration;
        private CommandsController _controller;
        [SetUp]
        public void SetUp()
        {
            _commandService = new Mock<ICommandService>();
            _controller = new CommandsController(_commandService.Object);
            _realProfile = new CommandsProfile();
            _configuration = new MapperConfiguration(cfg=> cfg.AddProfile(_realProfile));
            _mapper = new Mapper(_configuration);
        }
        [Test]
        public void GetCommands_Returns200OK_WhenDbIsEmpty()
        {
            // Arrange
            var commandsDto = _mapper.Map<IEnumerable<CommandReadDto>>(GetCommands(0));
            var response = APIResponse<IEnumerable<CommandReadDto>>.Create(HttpStatusCode.OK, "Request successful", commandsDto);
            _commandService.Setup(s => s.GetCommands()).Returns(response);

            // Act
            var result = _controller.GetAllCommands();

            // Assert
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(response.Data));
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
            
            var commandsDtos = _mapper.Map<IEnumerable<CommandReadDto>>(expectedCommands);
            var response = APIResponse<IEnumerable<CommandReadDto>>.Create(HttpStatusCode.OK, "Request successful", commandsDtos);
            _commandService.Setup(service => service.GetCommands()).Returns(response);
            
            // Act
            var result = _controller.GetAllCommands();

            // Assert
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(response.Data));  
        }
        [Test]
        public void GetCommandById_ExistingId_ReturnsOkResultWithCommand()
        {
            // Arrange
            var command = new Command { Id = 1, HowTo = "how to do", CommandLine = "command to", Platform = "platform to" };
            var commandDto = new CommandReadDto { Id = command.Id, HowTo = command.HowTo, CommandLine = command.CommandLine, Platform = command.Platform };
            var response = APIResponse<CommandReadDto>.Create(HttpStatusCode.OK, "Request successful", commandDto);
            _commandService.Setup(service => service.GetCommandById(1)).Returns(response);
           // _mapper.Setup(m => m.Map<CommandReadDto>(It.IsAny<Command>())).Returns(commandDto);

            // Act
            var result = _controller.GetCommandById(1);

            // Assert
            Assert.That(result, Is.TypeOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo(commandDto));
        }

        [Test]
        public void GetCommandById_NonExistingId_ReturnsBadRequest()
        {
            // Arrange
            var response = APIResponse<CommandReadDto>.Create(HttpStatusCode.BadRequest, "Command not found", null);
            _commandService.Setup(service => service.GetCommandById(1)).Returns(response);
            
            // Act
            var result = _controller.GetCommandById(1);

            // Assert
            Assert.That(result, Is.TypeOf<ObjectResult>());
            var objectResult = result as ObjectResult;
            Assert.That(objectResult.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));

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
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public void CreateCommand_ValidCommandObjectSubmitted_ReturnsCorrectResourceType()
        {
            // Arrange
            var commandCreateDto = new CommandCreateDto { HowTo = "mock", CommandLine = "mock", Platform = "Mock" };
            var commandReadDto = new CommandReadDto { Id = 1, HowTo = "mock", CommandLine = "mock", Platform = "Mock" };
            var response = APIResponse<CommandReadDto>.Create(HttpStatusCode.Created, "Command created successfully", commandReadDto);
            _commandService.Setup(x => x.CreateCommand(commandCreateDto)).Returns(response);
            
            // Act
            var result = _controller.CreateCommand(commandCreateDto);

            // Assert
            Assert.That(result, Is.TypeOf<CreatedAtActionResult>());
            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.That(createdAtActionResult.Value, Is.EqualTo(commandReadDto));

        }

        [Test]
        public void UpdateCommand_ValidCommandSubmitted_Returns204NoContent()
        {
            // Arrange
            var commandUpdateDto = new CommandUpdateDto { HowTo = "how to do", CommandLine = "command to", Platform = "platform to" };
            var response = APIResponse<object>.Create(HttpStatusCode.NoContent, null, null);
            _commandService.Setup(x => x.UpdateCommand(1, commandUpdateDto)).Returns(response);

            // Act
            var result = _controller.UpdateCommand(1, commandUpdateDto);

            // Assert
            Assert.That(result, Is.TypeOf<NoContentResult>());

        }

        [Test]
        public void UpdateCommand_InvalidCommandIDSubmitted_ReturnsBadRequest()
        {
            // Arrange
            var commandUpdateDto = new CommandUpdateDto { HowTo = "", CommandLine = "", Platform = "" };
            _controller.ModelState.AddModelError("HowTo", "Required");

            // Act
            var result = _controller.UpdateCommand(1, commandUpdateDto);

            // Assert
            Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        }

        [Test]
        public void DeleteCommand_ExistingId_ReturnsNoContent()
        {
            // Arrange
            var response = APIResponse<object>.Create(HttpStatusCode.NoContent, null, null);
            _commandService.Setup(x => x.DeleteCommand(1)).Returns(response);

            // Act
            var result = _controller.DeleteCommand(1);

            // Assert
            Assert.That(result, Is.TypeOf<NoContentResult>());
        }
        [Test]
        public void DeleteCommand_NonExistingId_ReturnsBadRequest()
        {
            // Arrange
            var response = APIResponse<object>.Create(HttpStatusCode.BadRequest, "Command not found", null);
            _commandService.Setup(x => x.DeleteCommand(1)).Returns(response);

            // Act
            var result = _controller.DeleteCommand(1);

            // Assert
            Assert.That(result, Is.TypeOf<ObjectResult>());
            var objectResult = result as ObjectResult;
            Assert.That(objectResult.StatusCode, Is.EqualTo((int)response.StatusCode));
        }

        #region
        private List<Command> GetCommands(int num)
        {
            var commands = new List<Command>();
            if (num > 0)
            {
                commands.Add(new Command
                {
                    Id = 0,
                    HowTo = "How to generate a migration",
                    CommandLine = "dotnet ef migrations add <Name of Migration>",
                    Platform = ".Net Core EF"
                });
            }
            return commands;
        }
        #endregion


    }


}
