using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using StudentExercises.Models;


namespace StudentExercises.DAL
{
    public class Repository
    {
        public SqlConnection Connection
        {
            get
            {
                // this is the address of the database you want to access
                string _connectionString = "Data Source=localhost\\SQLEXPRESS;Initial Catalog=StudentExercises;Integrated Security=True";
                return new SqlConnection(_connectionString);
            }
        }

        public List<Exercise> GetAllExercises()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name, Language FROM Exercise";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Exercise> exercises = new List<Exercise>();

                    while (reader.Read())
                    {
                        int idColumnPosition = reader.GetOrdinal("Id");
                        int idValue = reader.GetInt32(idColumnPosition);

                        int nameColumnPostion = reader.GetOrdinal("Name");
                        string nameValue = reader.GetString(nameColumnPostion);

                        int languageColumnPosition = reader.GetOrdinal("Language");
                        string languageValue = reader.GetString(languageColumnPosition);

                        Exercise exercise = new Exercise
                        {
                            Id = idValue,
                            Name = nameValue,
                            Language = languageValue
                        };

                        exercises.Add(exercise);
                    }
                    reader.Close();

                    return exercises;
                }
            }
        }

        public List<Exercise> GetAllExercisesByLanguage(string languageName)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, Name, Language 
                                       FROM Exercise
                                       WHERE Language = @languageName";
                    cmd.Parameters.Add(new SqlParameter("@languageName", languageName));

                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Exercise> exercises = new List<Exercise>();

                    while (reader.Read())
                    {
                        int idColumnPosition = reader.GetOrdinal("Id");
                        int idValue = reader.GetInt32(idColumnPosition);

                        int nameColumnPostion = reader.GetOrdinal("Name");
                        string nameValue = reader.GetString(nameColumnPostion);

                        int languageColumnPosition = reader.GetOrdinal("Language");
                        string languageValue = reader.GetString(languageColumnPosition);

                        Exercise exercise = new Exercise
                        {
                            Id = idValue,
                            Name = nameValue,
                            Language = languageValue
                        };

                        exercises.Add(exercise);
                    }

                    reader.Close();

                    return exercises;
                }
            }
        }

        public void AddNewExercise(Exercise exercise)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText =
                        $@"INSERT INTO Exercise (Name, Language)
                                        VALUES (@name, @language)";
                    cmd.Parameters.Add(new SqlParameter("@name", exercise.Name));
                    cmd.Parameters.Add(new SqlParameter("@language", exercise.Language));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Instructor> GetInstructors()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT 
                                        i.Id, i.FirstName, i.LastName, i.CohortId, i.Specialty, c.CohortName FROM Instructor i
                                        LEFT JOIN Cohort c ON c.Id = i.CohortId";

                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Instructor> instructors = new List<Instructor>();

                    while (reader.Read())
                    {
                        int idValue = reader.GetInt32(reader.GetOrdinal("Id"));
                        string firstNameValue = reader.GetString(reader.GetOrdinal("FirstName"));
                        string lastNameValue = reader.GetString(reader.GetOrdinal("LastName"));
                        string specialtyValue = reader.GetString(reader.GetOrdinal("Specialty"));
                        int cohortValue = reader.GetInt32(reader.GetOrdinal("CohortName"));

                        Instructor instructor = new Instructor()
                        {
                            Id = idValue,
                            FirstName = firstNameValue,
                            LastName = lastNameValue,
                            Specialty = specialtyValue,
                            CohortId = cohortValue,
                            Cohort = new Cohort
                            {
                                CohortName = cohortValue
                            }
                        };

                        instructors.Add(instructor);
                    }

                    conn.Close();
                    return instructors;
                }
            }

        }

        public void AddNewInstructor(Instructor instructor)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Instructor (FirstName, LastName, SlackHandle, CohortId, Specialty)
                                            VALUES (@FirstName, @LastName, @SlackHandle, @CohortId, @Specialty)";
                    cmd.Parameters.Add(new SqlParameter("@FirstName", instructor.FirstName));
                    cmd.Parameters.Add(new SqlParameter("@LastName", instructor.LastName));
                    cmd.Parameters.Add(new SqlParameter("@SlackHandle", instructor.SlackHandle));
                    cmd.Parameters.Add(new SqlParameter("@CohortId", instructor.CohortId));
                    cmd.Parameters.Add(new SqlParameter("@Specialty", instructor.Specialty));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void AssignExercise(int exerciseId, int studentId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO CurrentExercises (StudentId, ExerciseId)
                                            VALUES(@StudentId, @ExerciseId)";
                    cmd.Parameters.Add(new SqlParameter("@StudentId", studentId));
                    cmd.Parameters.Add(new SqlParameter("@ExerciseId", exerciseId));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Student> GetAllStudentsAndExercises()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT s.Id, s.FirstName, s.LastName, e.[Name], c.CohortName, ce.StudentId, ce.ExerciseId
                                        FROM Student s 
                                        LEFT JOIN CurrentExercises ce ON ce.StudentId = s.Id
                                        LEFT JOIN Exercise e ON e.Id = ce.ExerciseId
                                        LEFT JOIN Cohort c ON c.id = s.Cohort;";

                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Student> students = new List<Student>();

                    while (reader.Read())
                    {
                        
                        int idValue = reader.GetInt32(reader.GetOrdinal("Id"));
                        string firstNameValue = reader.GetString(reader.GetOrdinal("FirstName"));
                        string lastNameValue = reader.GetString(reader.GetOrdinal("LastName"));
                        int cohortValue = reader.GetInt32(reader.GetOrdinal("CohortName"));
                        int currentExerciseStudent = reader.GetInt32(reader.GetOrdinal("StudentId"));
                        int currentExerciseExercise = reader.GetInt32(reader.GetOrdinal("ExerciseId"));
                        string exerciseName = reader.GetString(reader.GetOrdinal("Name"));

                        Exercise exercise = new Exercise()
                        {
                            Name = exerciseName
                        };

                        if (!students.Any(stu => stu.Id == idValue))
                        {
                            Student student = new Student()
                            {
                                Id = idValue,
                                FirstName = firstNameValue,
                                LastName = lastNameValue,
                                Cohort = new Cohort
                                {
                                    CohortName = cohortValue
                                },
                                CurrentExercises = new List<Exercise>()
                            };

                            student.CurrentExercises.Add(exercise);
                            students.Add(student);
                        }
                        else
                        {
                            var studentSearch = students.Find(stu => stu.Id == idValue);
                            studentSearch.CurrentExercises.Add(exercise);
                        }
                    }

                    conn.Close();
                    return students;
                }
            }
        }
    }
}
