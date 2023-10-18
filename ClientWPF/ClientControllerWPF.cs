using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using ServerClientConnection;
using System.Printing;
using System.Windows;
using System.Data.SqlTypes;
using System.Windows.Controls;

namespace ClientWPF
{
    class ClientControllerWPF : INotifyPropertyChanged
    {
        #region определение PropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        #endregion

        #region ClientServerInteraction
        private readonly TcpClient tcpClient;
        private readonly NetworkStream stream;

        private StudentCon student;
        public ClientControllerWPF(string ip, int port)
        {
            tcpClient = new TcpClient();
            tcpClient.Connect(ip, port);
            stream = tcpClient.GetStream();

            selectedStudentindex = -1;
            selectedStudent = new();
            allRecords = new();
            student = new();

            firstname = new("");
            lastname = new("");
            group = new("");
            grant = new("");
            course = new("");
            sex = new();

            GetAllRecordsDB.Execute(null);
        }
        ~ClientControllerWPF()
        {
            stream.Dispose();
            tcpClient.Close();
        }

        public Connection SendRequest(Connection connection)
        {
            List<byte> data = new List<byte>();
            stream.Write(Encoding.Unicode.GetBytes(connection.GetJson()));
            do
            {
                data.Add((byte)stream.ReadByte());
            }
            while (stream.DataAvailable);
            string json = Encoding.Unicode.GetString(data.ToArray());
            return Connection.GetRequest(json);
        }
        #endregion

        #region CsvFileHandler
        public bool SetAndCheckPath(string path)
        {
            Connection con = SendRequest(new Connection("SetAndCheckPath", path));
            return (con.Content[0] == "True");
        }
        public void AddRecord(string newRecord)
        {
            SendRequest(new Connection("AddRecord", newRecord));
        }

        public bool DeleteRecord(int number)
        {
            Connection con = SendRequest(new Connection("DeleteRecord", number.ToString()));
            return (con.Content[0] == "True");
        }

        public List<string> GetAllRecords()
        {
            Connection con = SendRequest(new Connection("GetAllRecords", ""));
            return con.Content;
        }

        public List<string> GetSepRecord(int number)
        {
            Connection con = SendRequest(new Connection("GetSepRecord", number.ToString()));
            return con.Content;
        }

        public void ShutDown()
        {
            SendRequest(new Connection("ShutDown", ""));
        }
        #endregion

        private int selectedStudentindex;

        public int SelectedStudentIndex
        {
            get
            {
               return selectedStudentindex;
            }
            set
            {
                selectedStudentindex = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<StudentCon> allRecords;
        public ObservableCollection<StudentCon> AllRecords
        {
            get
            {
                return allRecords;
            }
            set
            {
                allRecords = value;
                OnPropertyChanged();
            }
        }

        private Command getAllRecordsDB;
        public Command GetAllRecordsDB
        {
            get
            {
                return getAllRecordsDB ??= new Command(obj =>
                {
                    AllRecords.Clear();
                    Connection con = SendRequest(new Connection("DBGetAllRecords", ""));
                    List<string> allrec = con.Content;
                    for (int i = 0; i < allrec.Count; i++)
                    {
                        List<string> stud = allrec[i].Split("|").Select(x => x.Trim()).ToList();
                        AllRecords.Add(student.ReturnStudent(stud));
                    }
                });
            }
        }
        private Command deleteRecord;
        public Command DeleteRecordDB
        {
            get
            {
                return deleteRecord ??= new Command(obj =>
                {
                    if (SelectedStudentIndex != -1)
                    {
                        Connection con = SendRequest(new Connection("DBDeleteRecord", SelectedStudentIndex.ToString()));
                        GetAllRecordsDB.Execute(null);
                    }
                });
            }
        }
        #region Данные для добавления студента
        private string firstname;

        public string FirstName
        {
            get
            {
                return firstname;
            }
            set
            {
                firstname = value;
                OnPropertyChanged();
            }
        }
        private string lastname;

        public string LastName
        {
            get
            {
                return lastname;
            }
            set
            {
                lastname = value;
                OnPropertyChanged();
            }
        }
        private string group;

        public string Group
        {
            get
            {
                return group;
            }
            set
            {
                group = value;
                OnPropertyChanged();
            }
        }
        private string grant;

        public string Grant
        {
            get
            {
                return grant;
            }
            set
            {
                grant = value;
                OnPropertyChanged();
            }
        }
        private string course;

        public string Course
        {
            get
            {
                return course;
            }
            set
            {
                course = value;
                OnPropertyChanged();
            }
        }
        private bool sex;

        public int Sex
        {
            get
            {
                return Convert.ToInt32(sex);
            }
            set
            {
                sex = Convert.ToBoolean(value);
                OnPropertyChanged();
            }
        }
        #endregion

        private Command addRecord;
        public Command AddRecordDB
        {
            get
            {
                return addRecord ??= new Command(obj =>
                {
                    try
                    {
                        string newRecord =
                          FirstName + ";"
                        + LastName + ";"
                        + Group + ";"
                        + Grant.ToString() + ";"
                        + Course.ToString() + ";"
                        + sex.ToString();
                        SendRequest(new Connection("DBAddRecord", newRecord));
                        GetAllRecordsDB.Execute(null);
                    }
                    catch
                    {
                        MessageBox.Show("Правила добавления записи (пример):\n" +
                            "Ivanov\n" +
                            "Ivan\n" +
                            "8I11\n" +
                            "3500\n" +
                            "1\n" +
                            "(Выбрать пол)");
                    }
                });
            }
        }

        private StudentCon selectedStudent;

        public StudentCon SelectedStudent
        {
            get
            {
                return selectedStudent;
            }
            set
            {
                selectedStudent = value;
                OnPropertyChanged();
            }
        }

        private bool selectedSex;

        public int SelectedSex
        {
            get
            {
                return Convert.ToInt32(selectedSex);
            }
            set
            {
                selectedSex = Convert.ToBoolean(value);
                OnPropertyChanged();
            }
        }

        private Command editRecord;
        public Command EditRecordDB
        {
            get
            {
                return editRecord ??= new Command(obj =>
                {
                    if (SelectedStudent is not null)
                    {
                        List<string> studentdata = new()
                        {
                            SelectedStudentIndex.ToString(),
                            SelectedStudent.FirstName,
                            SelectedStudent.LastName,
                            SelectedStudent.Group,
                            SelectedStudent.Grant.ToString(),
                            SelectedStudent.Course.ToString(),
                            SelectedStudent.Sex.ToString(),
                        };
                            Connection con = SendRequest(new Connection("DBEditRecord", studentdata));
                            GetAllRecordsDB.Execute(null);
                    }
                    else MessageBox.Show("Сначала выберите студента!");
                });
            }
        }
    }
}
