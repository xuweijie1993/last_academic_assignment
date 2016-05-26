using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Odbc;

namespace WindowsFormsApplication3
{
    public partial class ConnectConfig : Form
    {
        public ConnectConfig()
        {
            InitializeComponent();
        }
        Database FORM_DB;
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                FORM_DB.m1.MyConString = "DRIVER={MariaDB ODBC 1.0 Driver};" +
                "SERVER=" + server.Text +
                ";DATABASE=" + database.Text +
                ";UID=" + username.Text +
                ";PASSWORD=" + password.Text +
                ";Port=" + port.Text +
                ";OPTION=" + option.Text;
                FORM_DB.m1.MyConnection = new OdbcConnection(FORM_DB.m1.MyConString);
                FORM_DB.m1.MyConnection.Open();
                if (FORM_DB.m1.MyConnection.State == ConnectionState.Open)
                {
                    if (database.Text == "")
                    {
                        DataSet myDS = new DataSet();
                        OdbcDataAdapter ODA = new OdbcDataAdapter("Show databases;", FORM_DB.m1.MyConnection);
                        ODA.Fill(myDS);
                        if (myDS != null)
                        {
                            int i = myDS.Tables[0].Rows.Count;
                            for (int j = 0; j < i; j++)
                            {
                                database.Items.Add(myDS.Tables[0].Rows[j][0].ToString());
                            }
                        }
                        connect_status.Text = "Connecting with " + FORM_DB.m1.MyConnection.DataSource + " to database: " + FORM_DB.m1.MyConnection.Database;
                        MessageBox.Show("登陆成功!! \n选择需要连接的数据库!");
                    }
                    else
                    {
                        DataSet myDS = new DataSet();
                        OdbcDataAdapter ODA = new OdbcDataAdapter("Show tables;", FORM_DB.m1.MyConnection);
                        ODA.Fill(myDS);
                        FORM_DB.tablecluster.Items.Clear();
                        if (myDS != null)
                        {
                            int i = myDS.Tables[0].Rows.Count;
                            for (int j = 0; j < i; j++)
                            {
                                FORM_DB.tablecluster.Items.Add(myDS.Tables[0].Rows[j][0].ToString());
                            }
                        }
                        connect_status.Text = "Connecting with " + FORM_DB.m1.MyConnection.DataSource + " to database: " + FORM_DB.m1.MyConnection.Database;
                        MessageBox.Show("成功!! \n主窗口获得表名!");
                    }
                        
                }
            }
            catch (OdbcException MyOdbcException) //Catch any ODBC exception ..
            {
                string exp = ""; ;
                for (int i = 0; i < MyOdbcException.Errors.Count; i++)
                {
                    exp = "ERROR #" + i + "\n" +
                                  "Message: " +
                                  MyOdbcException.Errors[i].Message + "\n" +
                                  "Native: " +
                                  MyOdbcException.Errors[i].NativeError.ToString() + "\n" +
                                  "Source: " +
                                  MyOdbcException.Errors[i].Source + "\n" +
                                  "SQL: " +
                                  MyOdbcException.Errors[i].SQLState + "\n";
                }
                MessageBox.Show(exp);
            }
        }

        private void ConnectConfig_Load(object sender, EventArgs e)
        {
            FORM_DB = (Database)this.Owner;
            if (FORM_DB.m1.MyConnection.State == ConnectionState.Open)
            {
                connect_status.Text = "Connecting with " + FORM_DB.m1.MyConnection.DataSource + " to database: " + FORM_DB.m1.MyConnection.Database;
            }
            else
            {
                connect_status.Text = "请链接.";
            }
        }

        private void disconnect_Click(object sender, EventArgs e)
        {
            if (FORM_DB.m1.MyConnection.State == ConnectionState.Open)
            {
                FORM_DB.m1.MyConnection.Close();
                connect_status.Text = "请链接.";
            }
        }
    }
}
