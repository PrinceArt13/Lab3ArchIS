using Server.Csv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Entities;

namespace Server.DataBase
{
    class DBController
    {
        public DBhandler dbhandler { get; set; }

        public DBController()
        {
            dbhandler = new DBhandler();
        }

        private static DBController? instance;
        public static DBController GetInstance()
        {
            instance ??= new DBController();
            return instance;
        }

        public string GetString(List<string> records, int pos)
        {
            //string result = pos.ToString() + " | ";
            string result = "";
            foreach (string rec in records)
                result += string.Format("{0,15}", rec + " | ");
            return result;
        }

        public List<string> GetAllRecords()
        {
            var data = dbhandler.LoadAll();
            var outputRecords = new List<string>();
            int num = 1;
            foreach (var student in data)
            {
                outputRecords.Add(GetString(student.GetStringList(), num));
                num++;
            }
            return outputRecords;
        }
        //public List<Student> GetAllRecords()
        //{
        //    return dbhandler.LoadAll();
        //}

        public List<string> GetSepRecord(int number)
        {
            var data = dbhandler.SeparatelyLoad(number);
            List<string> outputRecord = new();
            outputRecord.Add(GetString(data[0].GetStringList(), number));
            return outputRecord;
        }

        public void DeleteRecord(int number)
        {
            dbhandler.StudentDelete(number);
        }

        public void AddRecord(string str)
        {
            Student student = new();
            using var context = new PrintsevContext();
            var entries = context.Set<Student>();
            string[] lines = str.Split(";");
            //FirstName;LastName;Group;Grant;Course;Sex
            //Oleg;Olegov;8I11;8500;4;True
            student.StudentId = Guid.NewGuid();
            student.FirstName = lines[0];
            student.LastName = lines[1];
            student.Group = lines[2];
            student.Grant = int.Parse(lines[3]);
            student.Course = int.Parse(lines[4]);
            student.Sex = bool.Parse(lines[5]);
            entries.Add(student);
            context.SaveChanges();
        }
        public void EditRecord(List<string> data)
        {
            try
            {
                dbhandler.EditRecord(int.Parse(data[0]), data[1], data[2], data[3], int.Parse(data[4]),
                    int.Parse(data[5]), bool.Parse(data[6]));
            }
            catch
            {
                return;
            }
        }
    }
}
