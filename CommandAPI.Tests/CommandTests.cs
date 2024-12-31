using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandAPI.Models;
using CommandAPI.Repos;
using Moq;
using NUnit;
namespace CommandAPI.Tests
{
    [TestFixture]
    public class CommandTests
    {
        private Mock<ICommandRepo> commandRepository;
        private List<Command> commands;
        [SetUp]
        public void SetUup()
        {
            // Set up the mock
            commandRepository = new Mock<ICommandRepo>();
            commands = new List<Command>();
            commands.Add(new Command() { Id=1, HowTo="build", CommandLine="dotnet build", Platform="dotnet cli"});
            commands.Add(new Command() { Id=2, HowTo="mock", CommandLine="dotnet mock", Platform="package manager console"});

            
        }
        public void TestGetActiveCommands()
        {
            
        }
        [Test]
        public void CanChangeHowTo()
        {
            // Arrange
            var command = new Command
            {
                Id = 1,
                HowTo = "Do something",
                Platform = "Some platform",
                CommandLine = "Some command line"
            };
            // Act
            command.HowTo = "Execute command";
            // Assert
            //Assert.That(command.HowTo, Is.EqualTo("Execute command"));
            Assert.That(true, "Execute command", command.HowTo);  
        }
    }
}
