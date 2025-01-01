using AutoMapper;
using CommandAPI.Dtos;
using CommandAPI.Models;
using CommandAPI.Profiles;
using CommandAPI.Repos;
using CommandAPI.Services;
using Microsoft.Extensions.Hosting;
using Moq;
using NUnit.Framework.Legacy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CommandAPI.Tests
{
    [TestFixture]
    public class CommandServiceTests : IDisposable
    {
        private Mock<ICommandRepo> _commandRepoMock;
        private Mock<IMapper> _mapperMock;
        private ICommandService _commandService;
        [SetUp]
        public void SetUp()
        {
            _commandRepoMock = new Mock<ICommandRepo>();
            _mapperMock = new Mock<IMapper>();
            _commandService = new CommandService(_commandRepoMock.Object, 
                                                _mapperMock.Object);
        }
        [Test]
        public void GetAllCommands_ReturnsAllComamnds()
        {
            // Arrange
            var commands = new List<Command>
            {
                new Command { Id = 1, HowTo= "how to do", CommandLine= "command to", Platform= "platform to" },
                new Command { Id = 2, HowTo= "another how to", CommandLine= "another command to", Platform="another platform to" }

            };
            _commandRepoMock.Setup(repo=> repo.GetAllCommands()).Returns(commands);

            var commandDtos = commands.Select(c => new CommandReadDto
            {
                Id = c.Id,
                HowTo = c.HowTo,
                CommandLine = c.CommandLine,
                Platform = c.Platform
            }).ToList();

            _mapperMock.Setup(m => m.Map<IEnumerable<CommandReadDto>>(It.IsAny<IEnumerable<Command>>())).Returns(commandDtos);
            // Act
            var actualResult =  _commandService.GetCommands();

            // Assert
            Assert.That(actualResult.Data, Is.EqualTo(commandDtos));

        }
        [Test]
        public void GetCommandById_ExistingId_ReturnsCommand()
        {
            // Arrange
            var expectedCommand = new Command { Id = 1, HowTo = "how to", CommandLine = "command to", Platform = "platform to" };
            _commandRepoMock.Setup(repo => repo.GetCommandById(1)).Returns(expectedCommand);

            var commandDto = new CommandReadDto
            {
                Id = expectedCommand.Id,
                HowTo = expectedCommand.HowTo,
                CommandLine = expectedCommand.CommandLine,
                Platform = expectedCommand.Platform
            };
            _mapperMock.Setup(m => m.Map<CommandReadDto>(It.IsAny<Command>())).Returns(commandDto);
            // Act
            var result = _commandService.GetCommandById(1);

            // Assert
            Assert.That(result.Data, Is.EqualTo(commandDto));
        }
        [Test]
        public void GetCommandById_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            _commandRepoMock.Setup(repo => repo.GetCommandById(It.IsAny<int>())).Returns((Command)null);

            // Act
            var result = _commandService.GetCommandById(1);

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        public void CreateCommand_ValidCommand_CreatesCommand()
        {
            // Arrange
            var commandCreateDto = new CommandCreateDto { HowTo = "Test", CommandLine = "Test", Platform = "Test" };
            var command = new Command { Id = 1, HowTo = "Test", CommandLine = "Test", Platform = "Test" };

            _mapperMock.Setup(m => m.Map<Command>(commandCreateDto)).Returns(command);

            // Act
            _commandService.CreateCommand(commandCreateDto);
            // Assert
            _mapperMock.Verify(m => m.Map<Command>(commandCreateDto), Times.Once);
            _commandRepoMock.Verify(r => r.CreateCommand(command), Times.Once);
            
        }
        [Test]
        public void UpdateCommand_ValidCommand_UpdatesCommand()
        {
            // Arrange
            var commandUpdateDto = new CommandUpdateDto { HowTo = "Test Updated", CommandLine = "Test Updated", Platform = "Test Updated" };
            var command = new Command { Id = 1, HowTo = "Test", CommandLine = "Test", Platform = "Test" };

            _commandRepoMock.Setup(repo => repo.GetCommandById(1)).Returns(command);
            _mapperMock.Setup(m => m.Map(commandUpdateDto, command)).Returns(command);

            // Act
            _commandService.UpdateCommand(1, commandUpdateDto);

            // Assert
            _commandRepoMock.Verify(r => r.UpdateCommand(command), Times.Once);
        }
        [Test]
        public void UpdateCommand_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var commandUpdateDto = new CommandUpdateDto { HowTo = "Test Updated", CommandLine = "Test Updated", Platform = "Test Updated" };
            _commandRepoMock.Setup(repo => repo.GetCommandById(It.IsAny<int>())).Returns((Command)null);

            // Act
            var result = _commandService.UpdateCommand(1, commandUpdateDto);

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
           
        }

        [Test]
        public void GetAllCommands_InvokeMethod_CheckIfRepoIsCalled()
        {
            _commandService.GetCommands();
            _commandRepoMock.Verify(x => x.GetAllCommands(), Times.Once);
        }
        
        [Test]
        public void CreateCommand_ValidResponse_CheckIfRepoIsCalled()
        {
            _mapperMock.Setup(x => x.Map<Command>(It.IsAny<CommandCreateDto>())).Returns(new Command());
            _commandService.CreateCommand(new CommandCreateDto());
            _commandRepoMock.Verify(x => x.CreateCommand(It.IsAny<Command>()), Times.Once);
        }
        [Test]
        public void DeleteCommand_ExistingId_DeletesCommand()
        {
            // Arrange
            var command = new Command { Id = 1, HowTo = "Test", CommandLine = "Test", Platform = "Test" };
            _commandRepoMock.Setup(repo => repo.GetCommandById(1)).Returns(command);

            // Act
            _commandService.DeleteCommand(1);

            // Assert
            _commandRepoMock.Verify(r => r.DeleteCommand(command), Times.Once);
        }
        [Test]
        public void DeleteCommand_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            _commandRepoMock.Setup(repo => repo.GetCommandById(It.IsAny<int>())).Returns((Command)null);

            // Act
            var result = _commandService.DeleteCommand(1);

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            
        }


        #region
        public void Dispose()
        {
            _commandRepoMock = null;
            _commandService = null;
            _commandRepoMock = null; 
        }
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

    
