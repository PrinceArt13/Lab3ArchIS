using System;
using System.Collections.Generic;

namespace Server.Entities;

public partial class Student
{
    public Guid StudentId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Group { get; set; } = null!;

    public int Grant { get; set; }

    public int Course { get; set; }

    public bool Sex { get; set; }

    public Student ReturnStudent(List<string> studentList)
    {
        Student student = new Student();
        student.StudentId = Guid.Parse(studentList[0]);
        student.FirstName = studentList[1];
        student.LastName = studentList[2];
        student.Group = studentList[3];
        student.Grant = int.Parse(studentList[4]);
        student.Course = int.Parse(studentList[5]);
        student.Sex = bool.Parse(studentList[6]);
        return student;
    }

    public List<string> GetStringList()
    {
        List<string> studentList = new()
        {
        StudentId.ToString(),
        FirstName,
        LastName,
        Group,
        Grant.ToString(),
        Course.ToString(),
        Sex.ToString()
        };
        return studentList;
    }
}
