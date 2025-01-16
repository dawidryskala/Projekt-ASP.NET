using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApplication1.Controllers;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplicationTestProject
{
    public class StudentControllerTests
    {
        List<StudentViewModel> students;
        Mock<IStudentService> studentServiceMock;

        [SetUp]
        public void Setup()
        {
            // fill students model mock
            students = new List<StudentViewModel>();
            students.Add(new StudentViewModel() { Id = 1, Name = "Asterix", IndexNumber = "000001" });
            students.Add(new StudentViewModel() { Id = 2, Name = "Obelix", IndexNumber = "000002" });

            // create service mock
            studentServiceMock = new Mock<IStudentService>();
        }

        [Test]
        public void TestIndexAction()
        {
            // Arrange
            this.studentServiceMock.Setup(m => m.FindAll()).Returns(students);
            var studentController = new StudentController(studentServiceMock.Object);

            // Act
            var result = studentController.Index();

            // Assert
            Assert.IsNotNull(result);
            Assert.True(result is ViewResult);
            var viewResult = result as ViewResult;
            Assert.IsTrue(string.IsNullOrEmpty(viewResult.ViewName) || viewResult.ViewName == "Index");
            Assert.IsNotNull(viewResult.Model);
            Assert.IsTrue(viewResult.Model is List<StudentViewModel>);
            var studentsModel = viewResult.Model as List<StudentViewModel>;
            Assert.That(studentsModel, Has.Count.EqualTo(2));
        }

        // Test akcjia Create

        [Test]
        public void TestCreatePost_ValidModel_RedirectsToIndex()
        {
            // Arrange
            var newStudent = new StudentViewModel { Id = 3, Name = "New Student", Email = "new@student.com" };
            this.studentServiceMock.Setup(m => m.Add(It.IsAny<StudentViewModel>()));

            var studentController = new StudentController(studentServiceMock.Object);

            // Act
            var result = studentController.Create(newStudent);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("Index", redirectResult.ActionName);
        }

        [Test]
        public void TestCreatePost_InvalidModel_ReturnsView()
        {
            // Arrange
            var newStudent = new StudentViewModel { Name = "", Email = "invalid_email" }; // Invalid data
            var studentController = new StudentController(studentServiceMock.Object);
            studentController.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = studentController.Create(newStudent);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(newStudent, viewResult.Model);
        }

        // Test akcji Edit

        [Test]
        public void TestEditGet_ValidId_ReturnsViewWithModel()
        {
            // Arrange
            var student = new StudentViewModel { Id = 1, Name = "Existing Student", Email = "existing@student.com" };
            this.studentServiceMock.Setup(m => m.FindById(1)).Returns(student);

            var studentController = new StudentController(studentServiceMock.Object);

            // Act
            var result = studentController.Edit(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(student, viewResult.Model);
        }

        [Test]
        public void TestEditGet_InvalidId_ReturnsNotFound()
        {
            // Arrange
            this.studentServiceMock.Setup(m => m.FindById(2)).Returns((StudentViewModel)null);

            var studentController = new StudentController(studentServiceMock.Object);

            // Act
            var result = studentController.Edit(2);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        // Test akcjia Details

        [Test]
        public void TestDetails_ValidId_ReturnsViewWithModel()
        {
            // Arrange
            var student = new StudentViewModel { Id = 1, Name = "Student Details", Email = "details@student.com" };
            this.studentServiceMock.Setup(m => m.FindById(1)).Returns(student);

            var studentController = new StudentController(studentServiceMock.Object);

            // Act
            var result = studentController.Details(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(student, viewResult.Model);
        }

        [Test]
        public void TestDetails_InvalidId_ReturnsNotFound()
        {
            // Arrange
            this.studentServiceMock.Setup(m => m.FindById(3)).Returns((StudentViewModel)null);

            var studentController = new StudentController(studentServiceMock.Object);

            // Act
            var result = studentController.Details(3);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        // Test akcji Delete

        [Test]
        public void TestDeleteGet_ValidId_ReturnsViewWithModel()
        {
            // Arrange
            var student = new StudentViewModel { Id = 1, Name = "Student To Delete", Email = "delete@student.com" };
            this.studentServiceMock.Setup(m => m.FindById(1)).Returns(student);

            var studentController = new StudentController(studentServiceMock.Object);

            // Act
            var result = studentController.Delete(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = result as ViewResult;
            Assert.AreEqual(student, viewResult.Model);
        }

        [Test]
        public void TestDeleteGet_InvalidId_ReturnsNotFound()
        {
            // Arrange
            this.studentServiceMock.Setup(m => m.FindById(4)).Returns((StudentViewModel)null);

            var studentController = new StudentController(studentServiceMock.Object);

            // Act
            var result = studentController.Delete(4);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void TestDeletePost_ValidId_RedirectsToIndex()
        {
            // Arrange
            this.studentServiceMock.Setup(m => m.Delete(1));

            var studentController = new StudentController(studentServiceMock.Object);

            // Act
            var result = studentController.DeleteConfirmed(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = result as RedirectToActionResult;
            Assert.AreEqual("Index", redirectResult.ActionName);
        }


    }
}