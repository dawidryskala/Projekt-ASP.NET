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
    public class EnrollmentControllerTests
    {
        private List<EnrollmentViewModel> enrollments;
        private Mock<IEnrollmentService> enrollmentServiceMock;
        private Mock<IStudentService> studentServiceMock;
        private Mock<ICourseService> courseServiceMock;
        private EnrollmentController enrollmentController;

        [SetUp]
        public void Setup()
        {
            enrollments = new List<EnrollmentViewModel>
            {
                new EnrollmentViewModel { Id = 1, CourseID = 101, StudentID = 1001, Grade = "A" },
                new EnrollmentViewModel { Id = 2, CourseID = 102, StudentID = 1002, Grade = "B" }
            };

            // Mockowanie usług
            enrollmentServiceMock = new Mock<IEnrollmentService>();
            studentServiceMock = new Mock<IStudentService>();
            courseServiceMock = new Mock<ICourseService>();

            // Tworzenie kontrolera z wszystkimi zależnościami
            enrollmentController = new EnrollmentController(
                enrollmentServiceMock.Object,
                studentServiceMock.Object,
                courseServiceMock.Object
            );
        }

        [TearDown]
        public void TearDown()
        {
            // Mocks nie wymagają Dispose(), więc pozostaje pusta.
        }

        [Test]
        public void TestIndexAction_ReturnsViewWithEnrollments()
        {
            // Arrange
            enrollmentServiceMock.Setup(m => m.GetAllEnrollments()).Returns(enrollments);

            // Act
            var result = enrollmentController.Index();

            // Assert
            Assert.That(result, Is.Not.Null);
            var viewResult = result as ViewResult;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult!.Model, Is.EqualTo(enrollments));
        }

        [Test]
        public void TestDetailsAction_ValidId_ReturnsView()
        {
            // Arrange
            var enrollment = enrollments.First();
            enrollmentServiceMock.Setup(m => m.GetEnrollmentById(1)).Returns(enrollment);

            // Act
            var result = enrollmentController.Details(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            var viewResult = result as ViewResult;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult!.Model, Is.EqualTo(enrollment));
        }

        [Test]
        public void TestDetailsAction_InvalidId_ReturnsNotFound()
        {
            // Arrange
            enrollmentServiceMock.Setup(m => m.GetEnrollmentById(It.IsAny<int>())).Returns((EnrollmentViewModel)null);

            // Act
            var result = enrollmentController.Details(999);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public void TestCreateAction_ValidModel_RedirectsToIndex()
        {
            // Arrange
            var newEnrollment = new EnrollmentViewModel { CourseID = 103, StudentID = 1003, Grade = "A" };
            enrollmentServiceMock.Setup(m => m.AddEnrollment(It.IsAny<EnrollmentViewModel>()));

            // Act
            var result = enrollmentController.Create(newEnrollment);

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
            var invalidEnrollment = new EnrollmentViewModel { CourseID = 0, StudentID = 0, Grade = "" };
            enrollmentController.ModelState.AddModelError("CourseID", "CourseID is required");

            // Act
            var result = enrollmentController.Create(invalidEnrollment);

            // Assert
            Assert.That(result, Is.InstanceOf<ViewResult>());
            var viewResult = result as ViewResult;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult!.Model, Is.EqualTo(invalidEnrollment));
        }

        [Test]
        public void TestEditAction_ValidModel_RedirectsToIndex()
        {
            // Arrange
            var existingEnrollment = enrollments.First();
            enrollmentServiceMock.Setup(m => m.UpdateEnrollment(existingEnrollment));

            // Act
            var result = enrollmentController.Edit(existingEnrollment);

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
            var invalidEnrollment = new EnrollmentViewModel { Id = 1, CourseID = 0, StudentID = 0, Grade = "" };
            enrollmentController.ModelState.AddModelError("CourseID", "CourseID is required");

            // Act
            var result = enrollmentController.Edit(invalidEnrollment);

            // Assert
            Assert.That(result, Is.InstanceOf<ViewResult>());
            var viewResult = result as ViewResult;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult!.Model, Is.EqualTo(invalidEnrollment));
        }

        [Test]
        public void TestDeleteAction_ValidId_RedirectsToIndex()
        {
            // Arrange
            enrollmentServiceMock.Setup(m => m.DeleteEnrollment(1));

            // Act
            var result = enrollmentController.DeleteConfirmed(1);

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
            enrollmentServiceMock.Setup(m => m.DeleteEnrollment(It.IsAny<int>()));

            // Act
            var result = enrollmentController.Delete(999);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }
}
