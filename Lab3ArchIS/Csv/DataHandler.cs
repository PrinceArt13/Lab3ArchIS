using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Csv
{
    class DataHandler : IDataChanger, IDataConverter, IDataLoader
    {
        string delim = ";";
        public string ConvertStudentToText(StudentData student)
        {
            string str = student.FirstName + delim + student.LastName + delim
                + student.Group + delim + student.Grant.ToString()
                + delim + student.Course.ToString() + delim + student.isRussianCitizen.ToString();

            return str;
        }

        public StudentData ConvertTextToStudent(string txtLine)
        {
            string[] line = txtLine.Split(delim);
            return new StudentData(
                line[0], line[1], line[2], double.Parse(line[3]), int.Parse(line[4]), bool.Parse(line[5]));
        }

        public List<StudentData> LoadAll(string path)
        {
            List<StudentData> allStudents = new();
            string[] lines = File.ReadAllLines(path);
            foreach (string line in lines)
            {
                allStudents.Add(ConvertTextToStudent(line));
            }
            return allStudents;
        }

        public StudentData SeparatelyLoad(string path, int number)
        {
            string[] lines = File.ReadAllLines(path);
            return ConvertTextToStudent(lines[number]);
        }

        public void StudentDelete(string path, int recordNumber)
        {
            File.WriteAllLines(path,
                File.ReadLines(path).Where((line, index) => index != recordNumber - 1).ToList());
        }

        public void StudentAdd(string path, StudentData student)
        {
            File.AppendAllText(path, ConvertStudentToText(student));
        }
    }
}
