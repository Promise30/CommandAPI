using CommandAPI.Data;
using CommandAPI.Models;
using CommandAPI.Repos;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework.Legacy;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CommandAPI.Tests
{
    [TestFixture]
    public class CommandRepoTests
    {
        private Command command1;
        private Command command2;
        private DbContextOptions<CommandDbContext> options;
        private CommandDbContext _context;
        private ICommandRepo _repository;
        public CommandRepoTests()
        {
            command1 = new Command()
            {
                Id = 1,
                HowTo = "Do something",
                Platform = "Some platform",
                CommandLine = "Some command"
            };
            command2 = new Command()
            {
                Id = 2,
                HowTo = "Do something else",
                Platform = "Some other platform",
                CommandLine = "Some other command"
            };
        }
        [SetUp]
        public void Setup()
        {
            options = new DbContextOptionsBuilder<CommandDbContext>()
                .UseInMemoryDatabase(databaseName: "temp_Command").Options;
            _context = new CommandDbContext(options);
            _repository = new CommandRepo(_context);

            // clear database
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }
        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void AddCommand_Command_One_SavesToDatabase()
        {
            // Arrange
            
            // Act
            _repository.CreateCommand(command1);
            _repository.SaveChanges();

            // Assert
            Assert.That(_context.Commands.Count(), Is.EqualTo(1));
        }
        [Test]
        public void GetAllCommands_CommandOneAndTwo_CheckBothFromDatabase()
        {
            // Arrange
            var expectedResult = new List<Command> { command1, command2 };           
            
            _repository.CreateCommand(command1);
            _repository.CreateCommand(command2);
            _repository.SaveChanges();
            
            // Act
            List<Command> actualList;
            actualList = _repository.GetAllCommands().ToList();

            // Assert
            CollectionAssert.AreEqual(expectedResult, actualList, new CommandCompare());
            Assert.That(actualList.Count, Is.EqualTo(2));
        }
        [Test]
        public void GetCommandById_ExistingId_ReturnsCommand()
        {
            // Arrange

            // Act
            _repository.CreateCommand(command1);
            _repository.SaveChanges();

            var result = _repository.GetCommandById(1);

            // Assert
            Assert.That(result, Is.EqualTo(command1));
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
        }
        [Test]
        public void GetCommandById_NonExistingId_ReturnsNull()
        {
            // Arrange

            // Act
            _repository.CreateCommand(command1);
            _repository.SaveChanges();

            var result = _repository.GetCommandById(2);

            // Assert
            Assert.That(result, Is.Null);
        }
        [Test]
        public void UpdateCommand_ExistingId_UpdatesCommand()
        {
            // Arrange
            var updatedCommand = new Command()
            {
                Id = 1,
                HowTo = "Do something updated",
                Platform = "Some platform updated",
                CommandLine = "Some command updated"
            };

            // Act
            _repository.CreateCommand(command1);
            _repository.SaveChanges();
            _repository.UpdateCommand(updatedCommand);
            _repository.SaveChanges();

            var result = _repository.GetCommandById(1);

            // Assert
            Assert.That(result, Is.EqualTo(updatedCommand));
            Assert.That(result.HowTo, Is.EqualTo("Do something updated"));
            Assert.That(result.Platform, Is.EqualTo("Some platform updated"));
            Assert.That(result.CommandLine, Is.EqualTo("Some command updated"));
        }
        [Test]
        public void DeleteCommand_ExistingId_DeletesCommand()
        {
            // Arrange

            // Act
            _repository.CreateCommand(command1);
            _repository.SaveChanges();
            _repository.DeleteCommand(command1);
            _repository.SaveChanges();

            var result = _repository.GetCommandById(1);

            // Assert
            Assert.That(result, Is.Null);
        }

        private class CommandCompare : IComparer
        {
            public int Compare(object? x, object? y)
            {
               var command1 = (Command)x;
               var command2 = (Command)y;
               if(command1.Id != command2.Id)
               {
                    return 1;
               }
                else
                {
                    return 0;
                }
            }

        }
    }
}
