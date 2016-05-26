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
    public partial class Database : Form
    {
        public Database()
        {
            InitializeComponent();
        }
        public MainForm m1;

        private void Database_Load(object sender, EventArgs e)
        {
            m1 = (MainForm)this.Owner;
            m1.dbf = this;
            m1.MyConnection = new OdbcConnection();
        }

        private void toolStripLabel_OPENDB_Click(object sender, EventArgs e)
        {
            ConnectConfig entity = new ConnectConfig();
            entity.Show(this);
        }

        private void tablecluster_TextChanged(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.DataSource = null;
                dataGridView1.Rows.Clear();
                dataGridView1.Refresh();
                string SQLSring = "Select * From " + tablecluster.Text;
                DataSet myDS = new DataSet();
                m1.ODA = new OdbcDataAdapter(SQLSring, m1.MyConnection);
                m1.ODABuilder = new OdbcCommandBuilder(m1.ODA);
                m1.ODA.Fill(myDS);
                MessageBox.Show(myDS.Tables[0].Columns[0].DataType.ToString());
                dataGridView1.DataSource = myDS.Tables[0];
                m1.opened_table = tablecluster.Text;
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

        private void update_datagrid_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count != 0)
            {
                dataGridView1.EndEdit();
                DataTable dt_source = (DataTable)dataGridView1.DataSource;
                DataTable dt_chaged = dt_source.GetChanges();
                int rowchanges = m1.ODA.Update(dt_source);
                MessageBox.Show(rowchanges + " rows changed!");
            }
            else
            {
                MessageBox.Show("无数据更新!");
            }
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            AddTables N1 = new AddTables();
            N1.Show(this);
        }

        public int create_record(MapWinGIS.Point pt)
        {
            try
            {
                DataTable dt_source = (DataTable)dataGridView1.DataSource;
                DataRow nrow = dt_source.NewRow();
                DataGridViewRow row = (DataGridViewRow)dataGridView1.RowTemplate.Clone();
                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    nrow[i] = 0;
                }
                nrow["XCOOR"] = pt.x;
                nrow["YCOOR"] = pt.y;
                dt_source.Rows.Add(nrow);
                dataGridView1.DataSource = dt_source;
                return 1;
            }
            catch (Exception errors)
            {
                MessageBox.Show(errors.ToString());
                return 0;
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
        }

        private void Database_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m1.MyConnection.State == ConnectionState.Open)
            {
                if (MessageBox.Show("确认关闭？", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    m1.opened_table = "";
                    if (m1.draw_int_indicator == 1)
                    {
                        m1.axMap1.ClearDrawing(m1.drawHandle);
                    }
                    //m1.MyConnection.Close();

                }
                else
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
