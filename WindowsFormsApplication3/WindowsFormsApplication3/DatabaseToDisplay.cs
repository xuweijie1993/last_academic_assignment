using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MapWinGIS;

namespace WindowsFormsApplication3
{
    public partial class DatabaseToDisplay : Form
    {
        public DatabaseToDisplay()
        {
            InitializeComponent();
        }
        MainForm mf1;
        private void button1_Click(object sender, EventArgs e)
        {
            string con = textBox1.Text;
            string ss = textBox2.Text;
            try
            {
                mf1.drawHandle = mf1.axMap1.AddLayerFromDatabase(con, ss, true);
                if (mf1.drawHandle == -1)
                {

                    MessageBox.Show("Failed to open layer: " + mf1.axMap1.FileManager.get_ErrorMsg(mf1.axMap1.FileManager.LastErrorCode));

                    // in case the reason of failure is still unclear, let's ask GDAL for details
                    var gs = new GlobalSettings();
                    MessageBox.Show("Last GDAL error: " + gs.GdalLastErrorMsg);
                }
                else
                {
                    var l = mf1.axMap1.get_OgrLayer(mf1.drawHandle);
                    var sf = l.GetBuffer();
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }
        }

        private void DatabaseToDisplay_Load(object sender, EventArgs e)
        {
            mf1 = (MainForm)this.Owner;
        }
    }
}
