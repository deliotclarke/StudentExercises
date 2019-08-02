using System;
using System.Collections.Generic;
using System.Text;

namespace StudentExercises.Models
{
    public class Cohort
    {
        public int Id { get; set; }
        public int CohortName { get; set; }
        public string CohortTime { get; set; }
        public List<Student> Students { get; set; }
        public List<Instructor> Instructors { get; set; }
    }
}
