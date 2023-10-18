using Server.Csv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Entities;
using System.Text.RegularExpressions;

namespace Server.DataBase
{
    public class DBhandler
    {
        //Guid g = Guid.NewGuid();

        string delim = ";";
        public string ConvertStudentToText(Student student)
        {
            string str = student.StudentId.ToString() + delim + student.FirstName + delim + student.LastName
                        + delim + student.Group + delim + student.Grant.ToString()
                        + delim + student.Course.ToString() + delim + student.Sex.ToString();
            return str;
        }
        //public Student ConvertTextToStudent(string txtLine)
        //{
        //    Student student = new Student();
        //    return student.ReturnStudent(txtLine);
        //}

        public List<Student> LoadAll()
        {
            using PrintsevContext context = new();
            var entities = context.Set<Student>();
            List<Student> studentsList = new();
            foreach (var ent in entities.OrderBy(x => x.StudentId).ToList())
            {
                studentsList.Add(ent);
            }
            return studentsList;
        }

        public List<Student> SeparatelyLoad(int number)
        {
            using PrintsevContext context = new();
            var entities = context.Set<Student>();
            return new List<Student> { entities.OrderBy(x => x.StudentId).ToList()[number - 1] };
        }

        public void StudentDelete(int recordNumber)
        {
            using PrintsevContext context = new();
            var entities = context.Set<Student>();
            Student student = new();
            entities.Remove(entities.OrderBy(x => x.StudentId).ToList().ElementAt(recordNumber));
            context.SaveChanges();
        }

        public void EditRecord(int num, string firstname, string lastname, string group, int grant, int course, bool sex)
        {
            using var context = new PrintsevContext();
            var entities = context.Set<Student>();
            Student student = entities.OrderBy(x => x.StudentId).ToList().ElementAt(num);
            student.FirstName = firstname;
            student.LastName = lastname;
            student.Group = group;
            student.Grant = grant;
            student.Course = course;
            student.Sex = sex;
            entities.Update(student);
            context.SaveChanges();
        }
    }
}
