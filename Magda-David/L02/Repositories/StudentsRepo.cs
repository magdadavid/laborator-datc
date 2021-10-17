using System.Collections.Generic;
using Models;

namespace Respositories
{
    public static class StudentsRepo
    {
        public static List<Student> Students = new List<Student>(){
            new Student(){ Id = 1, Name = "Popescu Ion", Faculty = "AC", StudyYear = 4},
            new Student(){ Id = 2, Name = "Dumitrescu Bogdan", Faculty = "ETC", StudyYear = 3},
        }; 
    }
}