using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
                    cmd.CommandText = @"SELECT Id, FirstName, LastName, Cohort, Specialty FROM Instructor";

                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Instructor> instructors = new List<Instructor>();

                    while (reader.Read())
                    {
                        int idColumnPosition = reader.GetOrdinal("Id");
                        int idValue = reader.GetInt32(idColumnPosition);

                        int firstNameColumnPostion = reader.GetOrdinal("FirstName");
                        string firstNameValue = reader.GetString(firstNameColumnPostion);

                        int lastNameColumnPosition = reader.GetOrdinal("LastName");
                        string lastNameValue = reader.GetString(lastNameColumnPosition);

                        int CohortColumnPosition = reader.GetOrdinal("Cohort");
                        int cohortValue = reader.GetInt32(CohortColumnPosition);

                        int specialtyColumnPosition = reader.GetOrdinal("Specialty");
                        string specialtyValue = reader.GetString(specialtyColumnPosition);

                        Instructor instructor = new Instructor()
                        {
                            Id = idValue,
                            FirstName = firstNameValue,
                            LastName = lastNameValue,
                            Cohort = cohortValue,
                            Specialty = specialtyValue
                        };

                        instructors.Add(instructor);
                    }

                    conn.Close();
                    return instructors;
                }
            }

        }
    }
}
