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
    public class StudentControllerTests
    {
        private List<StudentViewModel> students;
        private Mock<IStudentService> studentServiceMock;
        private StudentController studentController;

        [SetUp]
        public void Setup()
        {
            students = new List<StudentViewModel>
            {
                new StudentViewModel { Id = 1, Name = "Asterix", IndexNumber = "000001" },
                new StudentViewModel { Id = 2, Name = "Obelix", IndexNumber = "000002" }
            };

            studentServiceMock = new Mock<IStudentService>();
            studentController = new StudentController(studentServiceMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            // Mocks nie wymagają Dispose(), więc pozostaje pusta.
        }

        [Test]
        public void TestDetailsAction_ValidId_ReturnsView()
        {
            var student = students.First();
            studentServiceMock.Setup(m => m.FindById(1)).Returns(student);

            var result = studentController.Details(1);

            Assert.That(result, Is.Not.Null);
            var viewResult = result as ViewResult;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult!.Model, Is.EqualTo(student));
        }

        [Test]
        public void TestDetailsAction_InvalidId_ReturnsNotFound()
        {
            studentServiceMock.Setup(m => m.FindById(It.IsAny<int>())).Returns((StudentViewModel)null);

            var result = studentController.Details(999);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public void TestCreateAction_ValidModel_RedirectsToIndex()
        {
            var newStudent = new StudentViewModel { Name = "Panoramix", IndexNumber = "000003" };
            studentServiceMock.Setup(m => m.Add(It.IsAny<StudentViewModel>()));

            var result = studentController.Create(newStudent);

            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
            var redirectResult = result as RedirectToActionResult;
            Assert.That(redirectResult, Is.Not.Null);
            Assert.That(redirectResult!.ActionName, Is.EqualTo("Index"));
        }

        [Test]
        public void TestCreateAction_InvalidModel_ReturnsView()
        {
            var invalidStudent = new StudentViewModel { Name = "", IndexNumber = "000" };
            studentController.ModelState.AddModelError("Name", "Name is required");

            var result = studentController.Create(invalidStudent);

            Assert.That(result, Is.InstanceOf<ViewResult>());
            var viewResult = result as ViewResult;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult!.Model, Is.EqualTo(invalidStudent));
        }

        [Test]
        public void TestEditAction_ValidModel_RedirectsToIndex()
        {
            var existingStudent = students.First();
            studentServiceMock.Setup(m => m.Update(existingStudent));

            var result = studentController.Edit(existingStudent);

            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
            var redirectResult = result as RedirectToActionResult;
            Assert.That(redirectResult, Is.Not.Null);
            Assert.That(redirectResult!.ActionName, Is.EqualTo("Index"));
        }

        [Test]
        public void TestEditAction_InvalidModel_ReturnsView()
        {
            var invalidStudent = new StudentViewModel { Id = 1, Name = "", IndexNumber = "000" };
            studentController.ModelState.AddModelError("Name", "Name is required");

            var result = studentController.Edit(invalidStudent);

            Assert.That(result, Is.InstanceOf<ViewResult>());
            var viewResult = result as ViewResult;
            Assert.That(viewResult, Is.Not.Null);
            Assert.That(viewResult!.Model, Is.EqualTo(invalidStudent));
        }

        [Test]
        public void TestDeleteAction_ValidId_RedirectsToIndex()
        {
            studentServiceMock.Setup(m => m.Delete(1));

            var result = studentController.DeleteConfirmed(1);

            Assert.That(result, Is.InstanceOf<RedirectToActionResult>());
            var redirectResult = result as RedirectToActionResult;
            Assert.That(redirectResult, Is.Not.Null);
            Assert.That(redirectResult!.ActionName, Is.EqualTo("Index"));
        }

        [Test]
        public void TestDeleteAction_InvalidId_ReturnsNotFound()
        {
            studentServiceMock.Setup(m => m.Delete(It.IsAny<int>()));

            var result = studentController.Delete(999);

            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }
}
