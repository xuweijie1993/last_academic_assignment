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
    public partial class AddTables : Form
    {
        public AddTables()
        {
            InitializeComponent();
        }
        Database FORM_DB;
        private void AddTables_Load(object sender, EventArgs e)
        {
            FORM_DB = (Database)this.Owner;
            script_box.Text = "CREATE TABLE IF NOT EXISTS ";
            fieldlength.Enabled = false;
            utfsupp_cbx.Enabled = false;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            script_box.Text = "CREATE TABLE IF NOT EXISTS " + tablename.Text + " (";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (tablename.Text == "")
            {
                MessageBox.Show("Please input TABLE NAME first!");
                return;
            }
            if (fieldname.Text == "" || fieldtype_cbx.Text == "")
            {
                MessageBox.Show("Please input something meaningful!");
                return;
            }
            if (fieldtype_cbx.Text == "VARCHAR")
            {
                int result;
                if (int.TryParse(fieldlength.Text, out result))
                {
                    if (utfsupp_cbx.Text == "YES")
                    {
                        script_box.Text += ", "+fieldname.Text + " " + fieldtype_cbx.Text + "(" + result + ") " + "SET utf8;";
                    }
                    else
                    {
                        script_box.Text += ", "+ fieldname.Text + " " + fieldtype_cbx.Text + "(" + result + ")";
                    }
                }
                else
                {
                    result = 30;
                    if (utfsupp_cbx.Text == "YES")
                    {
                        script_box.Text += ", "+ fieldname.Text + " " + fieldtype_cbx.Text + "(" + result + ") " + "SET utf8";
                    }
                    else
                    {
                        script_box.Text += ", "+fieldname.Text + " " + fieldtype_cbx.Text + "(" + result + ")";
                    }
                }
            }
            else if (fieldtype_cbx.Text == "CHAR")
            {
                int result;
                if (int.TryParse(fieldlength.Text, out result))
                {
                    if (utfsupp_cbx.Text == "YES")
                    {
                        script_box.Text += "," + fieldname.Text + " " + fieldtype_cbx.Text + "(" + result + ") " + "SET utf8";
                    }
                    else
                    {
                        script_box.Text += ","+ fieldname.Text + " " + fieldtype_cbx.Text + "(" + result + ")";
                    }
                }
                else
                {
                    result = 10;
                    if (utfsupp_cbx.Text == "YES")
                    {
                        script_box.Text += ", " + fieldname.Text + " " + fieldtype_cbx.Text + "(" + result + ") " + "SET utf8";
                    }
                    else
                    {
                        script_box.Text += " , " + fieldname.Text + " " + fieldtype_cbx.Text + "(" + result + ")";
                    }
                }
            }
            else
            {
                script_box.Text +=","+ fieldname.Text + " " + fieldtype_cbx.Text;
            }
        }

        private void fieldtype_cbx_TextChanged(object sender, EventArgs e)
        {
            if (fieldtype_cbx.SelectedIndex == 3 || fieldtype_cbx.SelectedIndex == 4)
            {
                fieldlength.Enabled = true;
                utfsupp_cbx.Enabled = true;
            }
            else
            {
                fieldlength.Enabled = false;
                utfsupp_cbx.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (FORM_DB.m1.MyConnection.State == ConnectionState.Open)
                {
                    script_box.Text += ");";
                    string SQLString = script_box.Text;
                    FORM_DB.m1.cmd = FORM_DB.m1.MyConnection.CreateCommand();
                    FORM_DB.m1.cmd.CommandText = SQLString;
                    int i = FORM_DB.m1.cmd.ExecuteNonQuery();
                    switch (i)
                    {
                        case 0:
                            MessageBox.Show("Executed successfully!");
                            break;
                        default:
                            MessageBox.Show(i + "rows have changed!");
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("No no no, that's not the right move!\nFirst and foremost,you should connect to a database!");
                }
            }
            catch (OdbcException MyOdbcException)
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

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The best way to create a table is to type the SQL syntax by yourself. However, if you don't know much about SQL, then it may give you a little help, but don't expect it to be robust.", "Attention");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (FORM_DB.m1.MyConnection.State == ConnectionState.Open)
                {
                    string SQLString = script_box.Text;
                    FORM_DB.m1.cmd = FORM_DB.m1.MyConnection.CreateCommand();
                    FORM_DB.m1.cmd.CommandText = SQLString;
                    int i = FORM_DB.m1.cmd.ExecuteNonQuery();
                    switch (i)
                    {
                        case 0:
                            MessageBox.Show("Executed successfully!");
                            break;
                        default:
                            MessageBox.Show(i + "rows have changed!");
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("No no no, that's not the right move!\nFirst and foremost,you should connect to a database!");
                }
            }
            catch (OdbcException MyOdbcException)
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
    }
}
