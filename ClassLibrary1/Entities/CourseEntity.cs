using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Data.Entities
{
    [Table("course")]
    public class CourseEntity
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        [Required]
        public string Title { get; set; }

        [Required]
        public int Credits { get; set; }

        [Column("instructor_id")]
        [Required]
        public int InstructorId { get; set; }
        public virtual InstructorEntity Instructor { get; set; }
        public ICollection<EnrollmentEntity> Enrollments { get; set; }


    }
}

//namespace Data.Entities
//{
//    internal class CourseEntity
//    {
//    }
//}
