using API.Controllers;
using API.Interfaces;
using API.Models;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Test.Controllers;

public class UserControllerTest
{
    private readonly IUserRepository UserRepository;
    private readonly UsersController UsersController;
    public UserControllerTest()
    {
        this.UserRepository = A.Fake<IUserRepository>();
        this.UsersController = new UsersController(UserRepository);
    }
    private static User CreateFakeUser() => A.Fake<User>();

    [Fact]
    public async void UserControler_Create_returnCreated()
    {   // Arrange
        var user = CreateFakeUser();
        //Act
        A.CallTo(() => UserRepository.CreateAsync(user)).Returns(true);
        var result = (CreatedAtActionResult)await UsersController.Create(user);
        // Assert
        result.StatusCode.Should().Be(201);
        result.Should().NotBeNull();
    }
    [Fact]
    public async void UserController_GetUsers_ReturnOK()
    {
        // Arrange
        var users = A.Fake<List<User>>();
        users.Add(new User() { Name = "TestController", Email = "Controller Email" });

        //Act
        A.CallTo(() => UserRepository.GetAllAsync()).Returns(users);
        var result = (OkObjectResult)await UsersController.GetAll();

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }
    // Read Single
    //This method returns Ok(success) | NotFound(fails) action results
    [Theory]
    [InlineData(1)]
    public async void UserController_GetUser_ReturnOk(int id)
    {
        // Arrange
        var user = CreateFakeUser();
        user.Name = "TestController"; user.Email = "Controller Email"; user.Id = id;

        //Act
        A.CallTo(() => UserRepository.GetByIdAsync(id)).Returns(user);
        var result = (OkObjectResult)await UsersController.GetById(id);

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
        result.Should().NotBeNull();
    }
    // Update
    // This method returns Ok(success) | NotFound(fails) action results
    [Fact]
    public async void UserController_Update_ReturnOk()
    {
        // Arrange
        var user = CreateFakeUser();

        // Act
        A.CallTo(() => UserRepository.UpdateAsync(user)).Returns(true);
        var result = (OkResult)await UsersController.Update(user);

        // Assert
        result.StatusCode.Should().Be(200);
        result.Should().NotBeNull();
    }
    // Delete
    // This method returns NoContent(success) | NotFound(fails) action results
    [Fact]
    public async void UserController_Delete_ReturnNoContent()
    {
        // Arrange
        int userId = 1;

        // Act
        A.CallTo(() => UserRepository.DeleteAsync(userId)).Returns(true);
        var result = (NoContentResult)await UsersController.Delete(userId);

        // Assert
        result.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        result.Should().NotBeNull();
    }
}
