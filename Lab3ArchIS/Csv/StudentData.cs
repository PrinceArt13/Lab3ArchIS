using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Csv
{
    public class StudentData : IDataList
    {
        public string FirstName { get; set; } /// <summary>
                                              /// </summary>
        public string LastName { get; set; }
        public string Group { get; set; }
        public double Grant { get; set; } //socialsalary
        public int Course { get; set; } //month
        public bool isRussianCitizen { get; set; }

        public StudentData(string firstName, string lastName, string group, double grant, int course, bool isrussianCitizen)
        {
            FirstName = firstName;
            LastName = lastName;
            Group = group;
            Grant = grant;
            Course = course;
            isRussianCitizen = isrussianCitizen;
        }

        public List<string> GetStringList()
        {
            List<string> studentList = new();
            studentList.Add(FirstName);
            studentList.Add(LastName);
            studentList.Add(Group);
            studentList.Add(Grant.ToString());
            studentList.Add(Course.ToString());
            studentList.Add(isRussianCitizen.ToString());
            return studentList;
        }
    }
}
