using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO.Ports;
using System.Threading;
using Microsoft.VisualBasic; //要在引用里面添加

using MySql.Data.MySqlClient;
using System.Runtime.InteropServices;
using System.IO;

namespace demo
{
    public partial class Form1 : Form
    {
        SerialPort s = new SerialPort();  // 1 实例化一个串口对象，最好在后端代码中写代码，这样复制到其他地方不会出错。S是一个串口的句柄。

        public Form1()
        {
            InitializeComponent();

            synt = SynchronizationContext.Current;


        }

        static SynchronizationContext synt;
        private void button1_Click(object sender, EventArgs e)
        {
            String s = Interaction.InputBox("请输入权限密码", "权限确认", "输入框默认内容", -1, -1);

            if (!string.IsNullOrEmpty(s))
            {
                label2.Text = s;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Thread t1 = new Thread(new ThreadStart(TestMethod));
            //t1.IsBackground = true;
            //t1.Start();
            //------
            Thread t2 = new Thread(new ThreadStart(delegate
            {
                Control.CheckForIllegalCrossThreadCalls = false;//添加这一句即可 线程间操作无效: 从不是创建控件“label1”的线程访问它。”

                toto();
            }));
            t2.IsBackground = true;
            t2.Start();

        }

        int a = 0;
        public void TestMethod()
        {
            while (true)
            {
                this.label1.Invoke(
                  new Action(
                       delegate
                       {
                           label1.Text = a.ToString();
                           a++;

                       }
                         ));
                Thread.Sleep(1000);
            }
        }
        public void toto()
        {
            while (true)
            {
                label1.Text = a.ToString();
                a++;
                Thread.Sleep(1000);
            }

        }

        MySqlConnection conn;   //数据库链接对象
        string connectionStr = "server = 10.148.208.25; port = 3306; database = tjdemo; user = smtdsm; password = smtdsm; Sslmode = none;";

        private void button3_Click(object sender, EventArgs e)
        {
            conn = new MySqlConnection(connectionStr);          //创建数据库连接对象
            if (conn.State == System.Data.ConnectionState.Closed)
                conn.Open();
            string sql_SQL = "SELECT 姓名 FROM smt_chajian_user ";

            MySqlDataAdapter da = new MySqlDataAdapter(sql_SQL, conn); //参数1：SQL语句；参数2：数据库连接对象
            DataSet ds = new DataSet();
            da.Fill(ds, "gw_suju");
            int b = ds.Tables[0].Rows.Count;
            string[] a = new string[b];

            for (int i = 0; i < b; i++)
            {
                a[i] = ds.Tables[0].Rows[i][0].ToString();
                comboBox1.Items.Insert(0, a[i]);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            MysqlRead mysqlRead = new MysqlRead();
            DataSet dt11 = new DataSet();
            DataTable dt12 = new DataTable();
            string sql_SQL = "SELECT * FROM smt_chajian_weixiu ";

            mysqlRead.MysqlReadRerurnDatatable(sql_SQL, "ji", ref dt12);
            mysqlRead.MysqlReadRerurnDataset(sql_SQL, "ji", ref dt11);
        }

        [DllImport("kernel32")] //读取INI文件
        public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        [DllImport("kernel32")]//向INI文件中写入数据
        public static extern long WritePrivateProfileString(string mpAppName, string mpKeyName, string mpDefault, string mpFileName);

        public static string FInipath;
        string strPath = Application.StartupPath + "\\Set.ini";
        private void button5_Click(object sender, EventArgs e)
        {
            if (!File.Exists(strPath))  // 判断是否有ini文件，没有就新建
            {
                FileStream myFile = new FileStream(strPath, FileMode.Create);
                myFile.Dispose();
            }

            WritePrivateProfileString("asdasdasd", "asdsdss", "1", strPath);  //写入
            //WritePrivateProfileString("SET", "AUTO", "1", strPath);

            string ss = GetIniFileString("SET", "start", "false", strPath); //读取
        }
        /// <summary>
        /// 从INI文件中读取指定节点的内容
        /// </summary>
        /// <param name="section">INI节点</param>
        /// <param name="key">节点下的项</param>
        /// <param name="def">没有找到内容时返回的默认值</param>
        /// <param name="filePath">要读取的INI文件</param>
        /// <returns>读取的节点内容</returns>
        public static string GetIniFileString(string section, string key, string def, string filePath)
        {
            
            StringBuilder temp = new StringBuilder(1024);
            GetPrivateProfileString(section, key, def, temp, 1024, filePath);
            return temp.ToString();
        }
        /// 写入INI文件
        /// </summary>
        /// <param name="Section">项目名称(如 [TypeName] )</param>
        /// <param name="Key">键</param>
        /// <param name="Value">值</param>
        public static void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, FInipath);
        }
        
        string strPathLog = Application.StartupPath + "\\Log";
        string sFileName = Application.StartupPath + "\\Log\\" +  DateTime.Now.ToString("yyy-MM-dd") + ".log";
        private void builLog_Click(object sender, EventArgs e)
        {
            string strlog = txtLog.Text.Trim();
            WriteLog(strlog);

        }

        public void WriteLog( string strLog)
        {
            if (!Directory.Exists(strPathLog))//验证路径是否存在
            {
                Directory.CreateDirectory(strPathLog);
                //不存在则创建
            }
            FileStream fs;
            StreamWriter sw;
            if (File.Exists(sFileName))//验证文件是否存在，有则追加，无则创建
            {
                fs = new FileStream(sFileName, FileMode.Append, FileAccess.Write);
            }
            else
            {
                fs = new FileStream(sFileName, FileMode.Create, FileAccess.Write);
            }
            sw = new StreamWriter(fs);
            sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") + "   ---   " + strLog);
            sw.Close();
            fs.Close();
        }
    }
 }


