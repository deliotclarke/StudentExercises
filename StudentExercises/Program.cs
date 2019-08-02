using System;
using System.Collections.Generic;
using StudentExercises.DAL;
using StudentExercises.Models;

namespace StudentExercises
{
    class Program
    {
        static void Main(string[] args)
        {
            Repository repository = new Repository();

            List<Exercise> exercises = repository.GetAllExercises();
            PrintExercises("All Exercises", exercises);

            Pause();

            List<Exercise> javascriptExercises = repository.GetAllExercisesByLanguage("Javascript");
            PrintExercises("All Javascript Exercises", javascriptExercises);

            Pause();

            Exercise newExercise = new Exercise()
            {
                Name = "Reverse The String",
                Language = "Javascript"
            };
            // repository.AddNewExercise(newExercise);

            var javascriptExercises2 = repository.GetAllExercisesByLanguage("Javascript");
            PrintExercises("All Javascript Exercises After Reverse String:", javascriptExercises2);

            Pause();

            List<Instructor> instructors = repository.GetInstructors();
            PrintInstructors("All Instructors:", instructors);

            Pause();

            Instructor newInstructor = new Instructor()
            {
                FirstName = "Steve",
                LastName = "Brownlee",
                SlackHandle = "coach",
                Specialty = "Being good at everything",
                CohortId = 2
            };
            // repository.AddNewInstructor(newInstructor);
            List<Instructor> instructors2 = repository.GetInstructors();
            PrintInstructors("Instructors after Steve:", instructors2);

            // repository.AssignExercise(5, 4);

            Pause();

            var studentsAndExercises = repository.GetAllStudentsAndExercises();
            PrintStudentsAndExercises("All students and exercises:", studentsAndExercises);

        }

        public static void PrintExercises(string str, List<Exercise> exercises)
        {
            Console.WriteLine(str);
            exercises.ForEach(exer =>
            {
                Console.WriteLine($"{exer.Id}. {exer.Name} in {exer.Language}");
            });
        }
        public static void PrintInstructors(string str, List<Instructor> instructors)
        {
            Console.WriteLine(str);
            instructors.ForEach(instr =>
            {
                Console.WriteLine($"{instr.Id}. {instr.FirstName} {instr.LastName} - in Cohort {instr.Cohort.CohortName}");
            });
        }
        public static void PrintStudentsAndExercises(string str, List<Student> students)
        {
            Console.WriteLine(str);
            students.ForEach(stu =>
            {
                Console.WriteLine($"{stu.Id}. {stu.FirstName} {stu.LastName} - in Cohort {stu.Cohort.CohortName}");
                Console.WriteLine($"{stu.FirstName}'s Current Exercises:");
                stu.CurrentExercises.ForEach(exer => Console.WriteLine($"{exer.Name}"));
            });
        }
        public static void Pause()
        {
            Console.WriteLine();
            Console.Write("Press any key to continue...");
            Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}