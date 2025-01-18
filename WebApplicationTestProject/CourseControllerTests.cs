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
    public class CourseControllerTests
    {
        private List<CourseViewModel> courses;
        private Mock<ICourseService> courseServiceMock;
        private CourseController courseController;

        [SetUp]
        public void Setup()
        {
            courses = new List<CourseViewModel>
            {
                new CourseViewModel { Id = 1, Title = "Math", Credits = 3 },
                new CourseViewModel { Id = 2, Title = "Science", Credits = 4 }
            };

            courseServiceMock = new Mock<ICourseService>();
            courseController = new CourseController(courseServiceMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            // Mocks nie wymagają Dispose(), więc pozostaje pusta.
        }

        [Test]
        public void TestIndexAction_ReturnsViewWithCourses()
        {
            // Arrange
            courseServiceMock.Setup(m => m.GetAllCourses()).Returns(courses);

            // Act
            var result = courseController.Index();

            // Assert
            Assert.That(result, Is.Not.Null);
            var viewResult = result as ViewResult;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult!.Model, Is.EqualTo(courses));
        }

        [Test]
        public void TestDetailsAction_ValidId_ReturnsView()
        {
            // Arrange
            var course = courses.First();
            courseServiceMock.Setup(m => m.GetCourseById(1)).Returns(course);

            // Act
            var result = courseController.Details(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            var viewResult = result as ViewResult;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult!.Model, Is.EqualTo(course));
        }

        [Test]
        public void TestDetailsAction_InvalidId_ReturnsNotFound()
        {
            // Arrange
            courseServiceMock.Setup(m => m.GetCourseById(It.IsAny<int>())).Returns((CourseViewModel)null);

            // Act
            var result = courseController.Details(999);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public void TestCreateAction_ValidModel_RedirectsToIndex()
        {
            // Arrange
            var newCourse = new CourseViewModel { Title = "Physics", Credits = 3 };
            courseServiceMock.Setup(m => m.AddCourse(It.IsAny<CourseViewModel>()));

            // Act
            var result = courseController.Create(newCourse);

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
            var invalidCourse = new CourseViewModel { Title = "", Credits = -1 };
            courseController.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = courseController.Create(invalidCourse);

            // Assert
            Assert.That(result, Is.InstanceOf<ViewResult>());
            var viewResult = result as ViewResult;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult!.Model, Is.EqualTo(invalidCourse));
        }

        [Test]
        public void TestEditAction_ValidModel_RedirectsToIndex()
        {
            // Arrange
            var existingCourse = courses.First();
            courseServiceMock.Setup(m => m.UpdateCourse(existingCourse));

            // Act
            var result = courseController.Edit(existingCourse);

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
            var invalidCourse = new CourseViewModel { Id = 1, Title = "", Credits = -1 };
            courseController.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = courseController.Edit(invalidCourse);

            // Assert
            Assert.That(result, Is.InstanceOf<ViewResult>());
            var viewResult = result as ViewResult;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult!.Model, Is.EqualTo(invalidCourse));
        }

        [Test]
        public void TestDeleteAction_ValidId_RedirectsToIndex()
        {
            // Arrange
            courseServiceMock.Setup(m => m.DeleteCourse(1));

            // Act
            var result = courseController.DeleteConfirmed(1);

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
            courseServiceMock.Setup(m => m.DeleteCourse(It.IsAny<int>()));

            // Act
            var result = courseController.Delete(999);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }
}
