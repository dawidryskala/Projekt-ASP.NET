using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    [Table("exam")]
    public class ExamEntity
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        public int? CourseId { get; set; }

        public CourseEntity? Course { get; set; }
    }
}