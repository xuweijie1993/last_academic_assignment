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
using System.Data.Odbc;
using System.Drawing.Printing;

namespace WindowsFormsApplication3
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            this.axMap1.PreviewKeyDown += delegate(object sender, PreviewKeyDownEventArgs e)
            {
                switch (e.KeyCode)
                {
                    case Keys.Left:
                    case Keys.Right:
                    case Keys.Up:
                    case Keys.Down:
                        e.IsInputKey = true;
                        return;
                }
            };
        }
        public int drawHandle;
        public int draw_int_indicator;
        public Database dbf;
        public OdbcConnection MyConnection;
        public OdbcCommand cmd;
        public OdbcCommandBuilder ODABuilder;
        public string MyConString;
        public OdbcDataAdapter ODA;
        public string opened_table;
        public MapWinGIS.Point get_mouselocation(AxMapWinGIS._DMapEvents_MouseDownEvent _e, AxMapWinGIS.AxMap _axMap1)
        {
            MapWinGIS.Point pt = new MapWinGIS.Point();
            Extents etx = (Extents)axMap1.Extents;
            Extents getx = (Extents)axMap1.GeographicExtents;
            double extentX = 0;
            double extentY = 0;
            if (etx.xMax * etx.xMin < 0)
            {
                extentX = Math.Abs(Math.Abs(etx.xMax) + Math.Abs(etx.xMin));
                extentY = Math.Abs(Math.Abs(etx.yMax) + Math.Abs(etx.yMin));
            }
            else
            {
                extentX = Math.Abs(Math.Abs(etx.xMax) - Math.Abs(etx.xMin));
                extentY = Math.Abs(Math.Abs(etx.yMax) - Math.Abs(etx.yMin + 3));
            }
            double scale_w = extentX / _axMap1.Width;
            double scale_h = extentY / _axMap1.Height;
            pt.x = etx.Center.x + (_e.x - _axMap1.Width / 2) * scale_w;
            pt.y = etx.Center.y + (_axMap1.Height / 2 - _e.y) * scale_h;
            return pt;
        }

        private void toolStripLabel_db_Click(object sender, EventArgs e)
        {
            Database d1 = new Database();
            d1.Show(this);
        }
        
        private void MainForm_Load(object sender, EventArgs e)
        {
            checkedListBox1.Items.Add(new CheckBox());
            opened_table = "";
            draw_int_indicator = 0;
            MyConnection = new OdbcConnection();
            draw_indicator.Text = "Draw Mode: OFF";
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.ShowDialog();
            string filename = OFD.FileName;
            axMap1.GrabProjectionFromData = true;   // default value
            axMap1.AddLayerFromFilename(filename, tkFileOpenStrategy.fosVectorLayer, true);
            axMap1.SendMouseDown = false;
            draw_int_indicator = 0;
            draw_indicator.Text = "Draw Mode: OFF";
        }
        
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (MyConnection.State == ConnectionState.Closed)
            {
                MessageBox.Show("Plese connect to a database to advance to edit.");
                Database db = new Database();
                db.Show(this);
            }
            else
            {
                if (opened_table != "")
                {
                    if (axMap1.SendMouseDown == false)
                    {
                        axMap1.SendMouseDown = true;
                        drawHandle = axMap1.NewDrawing(tkDrawReferenceList.dlSpatiallyReferencedList);
                        draw_int_indicator = 1;
                        
                        draw_indicator.Text = "Draw Mode: ON";
                    }
                    else
                    {
                        axMap1.SendMouseDown = false;
                        draw_int_indicator = 0;
                        draw_indicator.Text = "Draw Mode: OFF";
                        MessageBox.Show("All your previous drawings would be discard!");
                    }
                }
                else
                {
                    MessageBox.Show("You haven't select a table yet.");
                }
            }
        }

        private void axMap1_MouseDownEvent(object sender, AxMapWinGIS._DMapEvents_MouseDownEvent e)
        {
            try
            {
                if (axMap1.CursorMode == tkCursorMode.cmNone)
                {
                    MapWinGIS.Point pt = new MapWinGIS.Point();
                    pt = get_mouselocation(e, axMap1);
                    if (dbf.create_record(pt) == 1)
                    {
                        double X = 0;
                        double Y = 0;
                        axMap1.PixelToProj(e.x, e.y, ref X, ref Y);
                        MessageBox.Show("X: " + X + "  Y: " + Y + "\n pt.x: " + pt.x + "    pt.y: " + pt.y);
                        axMap1.DrawPointEx(drawHandle, pt.x, pt.y, 5, 0);
                        axMap1.Redraw();
                    }
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }
        }

        private void toolStripLabel2_Click(object sender, EventArgs e)
        {
            axMap1.CursorMode = tkCursorMode.cmPan;
        }

        private void toolStripLabel3_Click(object sender, EventArgs e)
        {
            axMap1.CursorMode = tkCursorMode.cmNone;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            string con = "MySQL:db1,user=root,password=123456,port=3306";
            string ss = "Select * From geotest1";
            drawHandle = axMap1.AddLayerFromDatabase(con, ss, true);
            if (drawHandle == -1)
            {

                MessageBox.Show("Failed to open layer: " + axMap1.FileManager.get_ErrorMsg(axMap1.FileManager.LastErrorCode));

                // in case the reason of failure is still unclear, let's ask GDAL for details
                var gs = new GlobalSettings();
                MessageBox.Show("Last GDAL error: " + gs.GdalLastErrorMsg);
            }
            else
            {
                var l = axMap1.get_OgrLayer(drawHandle);
                var sf = l.GetBuffer();
            }
        }
    }
}
