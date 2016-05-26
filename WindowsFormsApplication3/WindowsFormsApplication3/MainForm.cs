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
        public Dijask dij;
        public int drawHandle;
        public int draw_int_indicator;
        public Database dbf;
        public OdbcConnection MyConnection;
        public OdbcCommand cmd;
        public OdbcCommandBuilder ODABuilder;
        public string MyConString;
        public OdbcDataAdapter ODA;
        public string opened_table;
        public Shape fst;
        public Shapefile fss;
        
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
            //axMap1.AddLayer(fss,true);
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
                MessageBox.Show("要编辑先连接数据库.");
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
                        MessageBox.Show("编辑将会丢失!");
                    }
                }
                else
                {
                    MessageBox.Show("未选择表.");
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
            DatabaseToDisplay dtd = new DatabaseToDisplay();
            dtd.ShowDialog(this);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            OpenFileDialog OFD_import = new OpenFileDialog();
            OFD_import.Filter = "Shapefile (*.shp)|*.shp";
            OFD_import.ShowDialog();
            Shapefile fs = new Shapefile();
            try
            {
                if (fs.Open(OFD_import.FileName))
                {
                    MessageBox.Show("选择要导入的表.");
                    Database tb = new Database();
                    tb.ShowDialog(this);
                    if (tb.tablecluster.Text != "")
                    {
                        tb.m1.cmd = tb.m1.MyConnection.CreateCommand();
                        int rownum = fs.Table.NumRows;
                        Shape shp = new Shape();
                        int ID = 0;
                        int NOTE = 0;
                        int busid = 0;
                        double x = 0;
                        double y = 0;
                        string part = "";
                        string NAME = "";
                        switch (fs.ShapefileType)
                        {

                            case ShpfileType.SHP_POINT:
                                {
                                    try
                                    {
                                        string sql = "INSERT INTO " + tb.tablecluster.Text + "(ID,X,Y,NOTE,NAME,BUS_ID) VALUES ";
                                        for (int i = 0; i < rownum; i++)
                                        {
                                            x = (double)fs.Table.get_CellValue(1, i);
                                            y = (double)fs.Table.get_CellValue(2, i);
                                            ID = (int)fs.Table.get_CellValue(0, i);
                                            NOTE = (int)fs.Table.get_CellValue(4, i);
                                            NAME = fs.Table.get_CellValue(3, i).ToString();
                                            busid = (int)fs.Table.get_CellValue(5, i);
                                            if (i != rownum - 1)
                                            {
                                                //sql += "('" + ID + "','"+x+"','"+y+"',PointFromText('" + fs.get_Shape(i).ExportToWKT() + "'), '" + NOTE + "','" + NAME + "','" + busid + "'),";
                                                sql += "('" + ID + "','" + x + "','" + y + "','" + NOTE + "','" + NAME + "','" + busid + "'),";
                                            }
                                            else
                                            {
                                                //sql += "('" + ID + "','" + x + "','" + y + "',PointFromText('" + fs.get_Shape(i).ExportToWKT() + "'), '" + NOTE + "','" + NAME + "','" + busid + "');";
                                                sql += "('" + ID + "','" + x + "','" + y + "','" + NOTE + "','" + NAME + "','" + busid + "');";
                                            }
                                        }
                                        tb.m1.cmd.CommandText = sql;
                                        int j = tb.m1.cmd.ExecuteNonQuery();
                                        switch (j)
                                        {
                                            case 0:
                                                MessageBox.Show("执行成功!");
                                                break;
                                            default:
                                                MessageBox.Show(j + " 行获得更新!");
                                                break;
                                        }
                                        MessageBox.Show(part);
                                    }
                                    catch (OdbcException MyOdbcException)
                                    {
                                        string exp = "";
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
                                break;
                            case ShpfileType.SHP_POLYLINE:
                                {
                                    try
                                    {
                                        int SID, EID, CLASS, TRAFFIC_L;
                                        double LENGTH;
                                        NAME = "";
                                        string sql = "INSERT INTO " + tb.tablecluster.Text + "(ID,POLYLINE) VALUES ";
                                        string conn_sql = "INSERT INTO " + "connectivity " + "(GID,SID,EID,CLASS,LENGTH,TRAFFIC_L,NAME) VALUES ";
                                        for (int i = 0; i < rownum; i++)
                                        {
                                            NAME = fs.Table.get_CellValue(3, i).ToString();
                                            SID = (int)fs.Table.get_CellValue(1, i);
                                            EID = (int)fs.Table.get_CellValue(2, i);
                                            CLASS = (int)fs.Table.get_CellValue(5, i);
                                            TRAFFIC_L = (int)fs.Table.get_CellValue(6, i);
                                            LENGTH = (double)fs.Table.get_CellValue(4, i);
                                            ID = (int)fs.Table.get_CellValue(0, i);
                                            if (i != rownum - 1)
                                            {
                                                conn_sql += "('" + ID + "','" + SID + "','" + EID + "','" + CLASS + "','" +LENGTH+"','"+ TRAFFIC_L + "','" + NAME + "'),";
                                                //sql += "('" + ID + "',LineFromText('" + fs.get_Shape(i).ExportToWKT() + "')),";
                                                sql += "('" + ID + "','" + fs.get_Shape(i).ExportToWKT().ToString() + "'),";
                                            }
                                            else
                                            {
                                                //sql += "('" + ID + "',LineFromText('" + fs.get_Shape(i).ExportToWKT() + "'));";
                                                sql += "('" + ID + "','" + fs.get_Shape(i).ExportToWKT().ToString() + "');";
                                                conn_sql += "('" + ID + "','" + SID + "','" + EID + "','" + CLASS + "','" + LENGTH + "','" + TRAFFIC_L + "','" + NAME + "');";
                                            }
                                        }
                                        tb.m1.cmd.CommandText = sql;
                                        int j = tb.m1.cmd.ExecuteNonQuery();
                                        switch (j)
                                        {
                                            case 0:
                                                MessageBox.Show("执行成功!");
                                                break;
                                            default:
                                                MessageBox.Show(j + " 行获得更新!");
                                                break;
                                        }
                                        tb.m1.cmd.CommandText = conn_sql;
                                        j = tb.m1.cmd.ExecuteNonQuery();
                                        switch (j)
                                        {
                                            case 0:
                                                MessageBox.Show("执行成功!");
                                                break;
                                            default:
                                                MessageBox.Show(j + " 行获得更新!");
                                                break;
                                        }
                                        MessageBox.Show(part);
                                    }
                                    catch (OdbcException MyOdbcException)
                                    {
                                        string exp = "";
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
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        MessageBox.Show("非法表.");
                    }
                }
                else
                {
                    MessageBox.Show("矢量文件打开失败.");
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }
        }

        private void toolStripLabel4_Click(object sender, EventArgs e)
        {
            dij = new Dijask();
            dij.Show(this);
        }

        private void toolStripLabel5_Click(object sender, EventArgs e)
        {
            axMap1.RemoveAllLayers();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            detail_txtbox.Text = "";
            if (checkedListBox1.CheckedItems.Count != 0)
            {
                for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
                {
                    detail_txtbox.Text += dij.sline[checkedListBox1.Items.IndexOf(checkedListBox1.CheckedItems[i])];
                    detail_txtbox.Text += "------------------------\r\n";
                }
            }
        }

    }
}
