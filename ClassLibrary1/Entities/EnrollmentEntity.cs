using Data.Entities;

public class EnrollmentEntity
{
    public int Id { get; set; }
    public int StudentID { get; set; }
    public int CourseID { get; set; }
    public string Grade { get; set; } // Pole typu string

    public StudentEntity Student { get; set; }
    public CourseEntity Course { get; set; }
}
