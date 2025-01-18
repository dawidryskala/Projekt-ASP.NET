using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApplication1.Controllers;
using WebApplication1.Models;
using WebApplication1.Services;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace WebApplicationTestProject
{
    [SuppressMessage("Usage", "NUnit1032:An IDisposable field/property should be Disposed in a TearDown method", Justification = "Mocks do not require Dispose.")]
    public class InstructorControllerTests
    {
        private List<InstructorViewModel> instructors;
        private Mock<IInstructorService> instructorServiceMock;
        private InstructorController instructorController;

        [SetUp]
        public void Setup()
        {
            instructors = new List<InstructorViewModel>
            {
                new InstructorViewModel { Id = 1, Name = "John Doe", AcademicTitle = "Dr." },
                new InstructorViewModel { Id = 2, Name = "Jane Smith", AcademicTitle = "Prof." }
            };

            instructorServiceMock = new Mock<IInstructorService>();
            instructorController = new InstructorController(instructorServiceMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            // Mocks nie wymagają Dispose(), więc pozostaje pusta.
        }

        [Test]
        public void TestIndexAction_ReturnsViewWithInstructors()
        {
            // Arrange
            instructorServiceMock.Setup(m => m.GetAllInstructors()).Returns(instructors);

            // Act
            var result = instructorController.Index();

            // Assert
            Assert.That(result, Is.Not.Null);
            var viewResult = result as ViewResult;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult!.Model, Is.EqualTo(instructors));
        }

        [Test]
        public void TestDetailsAction_ValidId_ReturnsView()
        {
            // Arrange
            var instructor = instructors.First();
            instructorServiceMock.Setup(m => m.GetInstructorById(1)).Returns(instructor);

            // Act
            var result = instructorController.Details(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            var viewResult = result as ViewResult;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult!.Model, Is.EqualTo(instructor));
        }

        [Test]
        public void TestDetailsAction_InvalidId_ReturnsNotFound()
        {
            // Arrange
            instructorServiceMock.Setup(m => m.GetInstructorById(It.IsAny<int>())).Returns((InstructorViewModel)null);

            // Act
            var result = instructorController.Details(999);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public void TestCreateAction_ValidModel_RedirectsToIndex()
        {
            // Arrange
            var newInstructor = new InstructorViewModel { Name = "Emily Brown", AcademicTitle = "PhD" };
            instructorServiceMock.Setup(m => m.AddInstructor(It.IsAny<InstructorViewModel>()));

            // Act
            var result = instructorController.Create(newInstructor);

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
            var redirectResult = result as RedirectToActionResult;
            Assert.That(redirectResult, Is.Not.Null);
            Assert.That(redirectResult!.ActionName, Is.EqualTo("Index"));
        }

        [Test]
        public void TestCreateAction_InvalidModel_ReturnsView()
        {
            // Arrange
            var invalidInstructor = new InstructorViewModel { Name = "", AcademicTitle = "" };
            instructorController.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = instructorController.Create(invalidInstructor);

            // Assert
            Assert.That(result, Is.InstanceOf<ViewResult>());
            var viewResult = result as ViewResult;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult!.Model, Is.EqualTo(invalidInstructor));
        }

        [Test]
        public void TestEditAction_ValidModel_RedirectsToIndex()
        {
            // Arrange
            var existingInstructor = instructors.First();
            instructorServiceMock.Setup(m => m.UpdateInstructor(existingInstructor));

            // Act
            var result = instructorController.Edit(existingInstructor);

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
            var redirectResult = result as RedirectToActionResult;
            Assert.That(redirectResult, Is.Not.Null);
            Assert.That(redirectResult!.ActionName, Is.EqualTo("Index"));
        }

        [Test]
        public void TestEditAction_InvalidModel_ReturnsView()
        {
            // Arrange
            var invalidInstructor = new InstructorViewModel { Id = 1, Name = "", AcademicTitle = "" };
            instructorController.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = instructorController.Edit(invalidInstructor);

            // Assert
            Assert.That(result, Is.InstanceOf<ViewResult>());
            var viewResult = result as ViewResult;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult!.Model, Is.EqualTo(invalidInstructor));
        }

        [Test]
        public void TestDeleteAction_ValidId_RedirectsToIndex()
        {
            // Arrange
            instructorServiceMock.Setup(m => m.DeleteInstructor(1));

            // Act
            var result = instructorController.DeleteConfirmed(1);

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
            var redirectResult = result as RedirectToActionResult;
            Assert.That(redirectResult, Is.Not.Null);
            Assert.That(redirectResult!.ActionName, Is.EqualTo("Index"));
        }

        [Test]
        public void TestDeleteAction_InvalidId_ReturnsNotFound()
        {
            // Arrange
            instructorServiceMock.Setup(m => m.DeleteInstructor(It.IsAny<int>()));

            // Act
            var result = instructorController.Delete(999);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }
}
