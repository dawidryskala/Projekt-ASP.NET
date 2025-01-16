using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    //internal class StudentEntity
    //{
    //}


    [Table("student")]
    public class StudentEntity
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [MaxLength(50)]
        [Required]
        public string Email { get; set; }

        [MinLength(5)]
        [MaxLength(6)]
        [Column("index")]
        [Required]
        public string IndexNumber { get; set; }

        [Column("birth_date")]
        public DateTime Birth { get; set; }

        public ICollection<EnrollmentEntity> Enrollments { get; set; }

    }
}
