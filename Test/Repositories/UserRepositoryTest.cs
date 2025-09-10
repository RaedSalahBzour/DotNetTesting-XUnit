using API.Data;
using API.Models;
using API.Repositories;
using FakeItEasy;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Repositories;

public class UserRepositoryTest
{
    private readonly UserRepository UserRepository;
    public UserRepositoryTest()
    {
        var userDbContext = GetDatabaseContext().Result;
        UserRepository = new UserRepository(userDbContext);

    }
    private async Task<AppDBContext> GetDatabaseContext()
    {
        var options = new DbContextOptionsBuilder<AppDBContext>()
            .UseSqlite("Data Source=unitTest.db")
            .Options;

        var appDBContext = new AppDBContext(options);

        // Always reset DB before each test
        appDBContext.Database.EnsureDeleted();
        appDBContext.Database.EnsureCreated();

        // Seed predictable data
        appDBContext.Users.AddRange(
            new User { Email = "u1@mail.com", Name = "User1" }, // Id=1
            new User { Email = "u2@mail.com", Name = "User2" }, // Id=2
            new User { Email = "u3@mail.com", Name = "User3" }  // Id=3
        );

        await appDBContext.SaveChangesAsync();
        return appDBContext;
    }
    // Create
    // This returns bool (true) for success
    [Fact]
    public async void UserRepository_Create_ReturnTrue()
    {
        // Arrange
        var user = A.Fake<User>();

        // Act
        var result = await UserRepository.CreateAsync(user);

        // Assert
        result.Should().BeTrue();
    }
    // Read All
    // This returns List of User
    [Fact]
    public async void UserRepository_GetUsers_ReturnUsers()
    {
        // Arrange

        // Act
        var result = await UserRepository.GetAllAsync();

        // Assert
        result.Should().AllBeOfType<User>();
    }

    // Read Single
    // This returns User object
    [Theory]
    [InlineData(1)]
    public async void UserRepository_GetUser_ReturnUser(int id)
    {
        // Arrange

        // Act
        var result = await UserRepository.GetByIdAsync(id);

        // Assert
        result.Should().BeOfType<User>();
    }
    // Update
    // This returns true (success)
    [Theory]
    [InlineData(1)]
    public async void UserRepository_UpdateUser_ReturnTrue(int id)
    {
        // Arrange

        // Act
        var user = await UserRepository.GetByIdAsync(id);
        user.Name = "Updated";
        var result = await UserRepository.UpdateAsync(user);

        // Assert
        result.Should().BeTrue();
    }
    // Delete
    // This returns True
    [Fact]
    public async void UserRepository_DeleteUser_ReturnTrue()
    {
        // Arrange
        int id = 3;

        // Act
        var result = await UserRepository.DeleteAsync(id);

        // Assert
        result.Should().BeTrue();
    }

}
