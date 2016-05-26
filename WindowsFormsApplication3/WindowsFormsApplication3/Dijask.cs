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
using MapWinGIS;

namespace WindowsFormsApplication3
{
    public partial class Dijask : Form
    {
        public Dijask()
        {
            InitializeComponent();
        }
        public class linedisplay
        {
            public int fp;
            public int bp;
            public int blid;
            public linedisplay(int a = 0, int b = 0, int c = 0)
            {
                this.fp = a;
                this.bp = b;
                this.blid = c;
            }
        }
        public class vertex
        {
            public int FromID;//前驱结点
            public int thisID;//当前结点
            public double Len;//权重值
            public int LineID;//联通的线
            public int Traffic_l;
            public int class1;
            public vertex(int a = 0, int b = 0, int e = 0, double c = 0, int d = 0, int f = 0)//构造函数
            {
                this.class1 = f;
                this.FromID = a;
                this.thisID = b;
                this.Len = c;
                this.LineID = e;
                this.Traffic_l = d;
            }
        };
        public class tr1
        {
            public int startid;
            public int lineid;
            public int endid;
            public int stop_count;
            public tr1(int a = 0, int b = 0, int c = 0)
            {
                this.startid = a;
                this.lineid =b;
                this.endid = c;
                this.stop_count = a;
            }
        };
        public class tr2
        {
            public int startid1;
            public int lineid1;
            public int endid1;
            public int startid2;
            public int lineid2;
            public int endid2;
            public int stop_count;
            public tr2(int a = 0, int b = 0, int c = 0)
            {
                this.startid1 = a;
                this.lineid1 =b;
                this.endid1 = c;
                this.startid2 = a;
                this.lineid2 = b;
                this.endid2 = c;
                this.stop_count = a;
            }
        };
        public class tr3
        {
            public int startid1;
            public int lineid1;
            public int endid1;
            public int startid2;
            public int lineid2;
            public int endid2;
            public int startid3;
            public int lineid3;
            public int endid3;
            public int stop_count;
            public tr3(int a =0, int b=0, int c=0)
            {
                this.startid1 = a;
                this.lineid1 =b;
                this.endid1 = c;
                this.startid2 = a;
                this.lineid2 = b;
                this.endid2 = c;
                this.startid3 = a;
                this.lineid3 = b;
                this.endid3 = c;
                this.stop_count = a;
            }
        };
        static int Compare1(KeyValuePair<double, int> a, KeyValuePair<double, int> b)
        {
            return a.Key.CompareTo(b.Key);
        }
        static int Compare2(KeyValuePair<string, int> a, KeyValuePair<string, int> b)
        {
            return a.Value.CompareTo(b.Value);
        }
        static int Compare3(KeyValuePair<DataRow,int> a, KeyValuePair<DataRow,int> b)
        {
            return a.Value.CompareTo(b.Value);
        }
            static int Compare4(KeyValuePair<List<KeyValuePair<DataRow, int>>, int> a, KeyValuePair<List<KeyValuePair<DataRow, int>>, int> b)
        {
            return a.Value.CompareTo(b.Value);
        }
        MainForm m1;
        int layerhandle = -1;
        int layerhandle2 = -1;
        int layerhandle3 = -1;
        string sql3;
        int depth;
        int sphere = 2500;
        double infinite = 99999999.0;//定义无限
        public List<string> sline = new List<string>();
        public List<List<linedisplay>> bline = new List<List<linedisplay>>();
        List<vertex> vls = new List<vertex>();//存放被检查过的点
        List<vertex> vl = new List<vertex>();//存放未被检查的点
        List<vertex> vls2 = new List<vertex>();
        List<vertex> vls3 = new List<vertex>();
        static DataSet myds1 = new DataSet();//
        static DataSet myds2 = new DataSet();//
        DataSet dss = new DataSet();
        DataTable dt = new DataTable();
        DataSet bus_conn = new DataSet();//公交连通表
        DataSet bus_vertrx = new DataSet();//公交节点表
        DataSet bus_line = new DataSet();//
        List<DataRow> From_rows = new List<DataRow>();//所有在不同线路上的起点
        List<DataRow> To_rows = new List<DataRow>();//所有在不同线路上的终点
        List<KeyValuePair<DataRow, int>> result1 = new List<KeyValuePair<DataRow, int>>();//fromcango
        List<KeyValuePair<DataRow, int>> result2 = new List<KeyValuePair<DataRow, int>>();//fromtotorows
        List<KeyValuePair<DataRow, int>> result3 = new List<KeyValuePair<DataRow, int>>();//cangoto
        List<KeyValuePair<DataRow, int>> result4 = new List<KeyValuePair<DataRow, int>>();//transfer1
        List<KeyValuePair<DataRow, int>> result5 = new List<KeyValuePair<DataRow, int>>();//vertexpool
        List<KeyValuePair<DataRow, int>> result6 = new List<KeyValuePair<DataRow, int>>();//transfer2
        List<KeyValuePair<DataRow, int>> result7 = new List<KeyValuePair<DataRow, int>>();//transfer2
        public void conn()
        {
            myds1.Clear();
            myds2.Clear();
            string sql = "Select ID,NOTE From test_vertex";
            string sql2 = "Select * From connectivity";
            m1.ODA = new OdbcDataAdapter(sql, m1.MyConnection);
            m1.ODABuilder = new OdbcCommandBuilder(m1.ODA);
            m1.ODA.Fill(myds1);
            m1.ODA = new OdbcDataAdapter(sql2, m1.MyConnection);
            m1.ODABuilder = new OdbcCommandBuilder(m1.ODA);
            m1.ODA.Fill(myds2);
        }
        public List<vertex> Dijask2_1(int StartID, int EndID, int depth = 0)
        {
            List<vertex> vls = new List<vertex>();//存放被检查过的点
            List<vertex> vl = new List<vertex>();//存放未被检查的点
            if (StartID == EndID)//如果终点和起点一致
            {
                MessageBox.Show("错误：终点与起点一致");
                return vls;//结束检索
            }
            int myds1count = myds1.Tables[0].Rows.Count;
            for (int i = 0; i < myds1count; i++)//遍历所有节点
            {
                vertex vt = new vertex();//新建节点实例
                vt.thisID = (int)myds1.Tables[0].Rows[i][0];//将中间变量转换为整型，并赋给vt.thisID
                //vt.class1 = (int)myds1.Tables[0].Rows[i][1];//添加点归属
                if (vt.thisID == StartID)//如果当前结点的ID是起点
                {
                    vt.Len = 0.0;//将当前点的权重设置为0
                    vt.FromID = -1;//将当前点的前驱结点设置为-1，表明没有前驱
                    vt.LineID = 0;
                    vt.Traffic_l = 0;
                    vt.class1 = (int)myds1.Tables[0].Rows[i][1];
                    vls.Add(vt);//将起点添加至vls中
                }
                else//对于不是起点的结点
                {
                    vt.Len = infinite;//将权重设置为无限，表示连通性未知或者无法连通
                    vt.FromID = -1;//前驱结点设为-1
                    vt.LineID = 0;
                    vt.Traffic_l = 0;
                    vt.class1 = (int)myds1.Tables[0].Rows[i][1];
                    vl.Add(vt);//将结点添加至vl
                }
            }
            List<System.Data.DataRow> temp = new List<System.Data.DataRow>();//用于存放从线路表中读取到的完整记录
            for (int i = 0; i < myds2.Tables[0].Rows.Count; i++)//循环整个线路表表
            {
                //如果当前起点或者终点包含有起点编号，即检索与起点相连的结点
                if ((int)myds2.Tables[0].Rows[i][6] == 2)
                {
                    if ((int)myds2.Tables[0].Rows[i][2] == StartID || (int)myds2.Tables[0].Rows[i][3] == StartID)
                    {
                        temp.Add(myds2.Tables[0].Rows[i]);//将该记录添加到temp中
                    }
                }
                else if ((int)myds2.Tables[0].Rows[i][6] == 1)
                {
                    if ((int)myds2.Tables[0].Rows[i][2] == StartID)
                    {
                        temp.Add(myds2.Tables[0].Rows[i]);//将该记录添加到temp中
                    }
                }
            }
            for (int i = 0; i < vl.Count; i++)//循环所有未被检查的结点
            {
                for (int j = 0; j < temp.Count; j++)//对于每一个节点，都循环一遍temp表
                {
                    //如果当前结点位于记录的起点或者终点处
                    if ((int)temp[j][6] == 2)
                    {
                        if (vl[i].thisID == (int)temp[j][2] || vl[i].thisID == (int)temp[j][3])
                        {
                            vl[i].FromID = StartID;//则将次结点的前驱设置为起点
                            vl[i].Len = double.Parse(temp[j][5].ToString());//将权重值设置为对应记录中的权重值
                            vl[i].LineID = (int)temp[j][1];
                            vl[i].Traffic_l = (int)temp[j][7];
                        }
                    }
                    else if ((int)temp[j][6] == 1)
                    {
                        if (vl[i].thisID == (int)temp[j][3])
                        {
                            vl[i].FromID = StartID;//则将次结点的前驱设置为起点
                            vl[i].Len = double.Parse(temp[j][5].ToString());//将权重值设置为对应记录中的权重值
                            vl[i].LineID = (int)temp[j][1];
                            vl[i].Traffic_l = (int)temp[j][7];
                        }
                    }
                }
            }
            double minLen = 0;//最小权重
            int minID = 0;//权重最小的点的ID
            int curri = 0;//当前点位于数组中下标
            int counter = 0;//计数器
            vertex tempv = new vertex();//临时结点存放变量
            while (vl.Count != 0)//如果vl不为空
            {
                minLen = vl[0].Len;//将数组中第一条记录的权重值付给minLen
                minID = vl[0].thisID;//将ID也付给minID
                curri = 0;//当前数组下标重置为0
                tempv = vl[0];//将当前结点付给tempv
                for (int i = 0; i < vl.Count; i++)//循环所有vl中的结点
                {
                    if (minLen > vl[i].Len)//比较是否有新的权重值比当前权重值小
                    {
                        minLen = vl[i].Len;//更新权重值为更小的
                        minID = vl[i].thisID;//更新ID
                        curri = i;//记录其下标，方便之后删除
                        tempv = vl[i];//更新tempv的结点为当前结点
                    }
                }
                temp.Clear();//清空temp数组
                for (int i = 0; i < myds2.Tables[0].Rows.Count; i++)//循环整个结点表
                {
                    //找出包含当前ID的所有记录
                    if ((int)myds2.Tables[0].Rows[i][6] == 2)
                    {
                        if ((int)myds2.Tables[0].Rows[i][2] == minID || (int)myds2.Tables[0].Rows[i][3] == minID)
                        {
                            temp.Add(myds2.Tables[0].Rows[i]);//保存至temp中
                        }
                    }
                    else if ((int)myds2.Tables[0].Rows[i][6] == 1)
                    {
                        if ((int)myds2.Tables[0].Rows[i][2] == minID)
                        {
                            temp.Add(myds2.Tables[0].Rows[i]);//保存至temp中
                        }
                    }
                }
                counter = 0;//计数器重置为0
                double templen = 0;//临时总的权重值，用于下面比较和更新
                while (counter < vl.Count)//遍历整个vl表
                {
                    for (int i = 0; i < temp.Count; i++)//每一个节点，比较所有temp中的记录
                    {
                        //检查记录中是否包含当前结点，找出与当前结点相关联的点
                        if ((int)temp[i][6] == 2)
                        {
                            if (vl[counter].thisID == (int)temp[i][2] || vl[counter].thisID == (int)temp[i][3])
                            {
                                templen = double.Parse(temp[i][5].ToString()) + tempv.Len;//计算总的权重值
                                if (templen < vl[counter].Len)//如果新的权重值比当前存放的权重值小
                                {
                                    vl[counter].Len = templen;//更新为新的权重值
                                    vl[counter].FromID = tempv.thisID;//同时更新该点为vl中点的前驱ID
                                    vl[counter].LineID = (int)temp[i][1];
                                    vl[counter].Traffic_l = (int)temp[i][7];
                                }
                            }
                        }
                        else if ((int)temp[i][6] == 1)
                        {
                            if (vl[counter].thisID == (int)temp[i][3])
                            {
                                templen = double.Parse(temp[i][5].ToString()) + tempv.Len;//计算总的权重值
                                if (templen < vl[counter].Len)//如果新的权重值比当前存放的权重值小
                                {
                                    vl[counter].Len = templen;//更新为新的权重值
                                    vl[counter].FromID = tempv.thisID;//同时更新该点为vl中点的前驱ID
                                    vl[counter].LineID = (int)temp[i][1];
                                    vl[counter].Traffic_l = (int)temp[i][7];
                                }
                            }
                        }
                    }
                    counter += 1;//计数器加1，知道当前while循环结束
                }
                if (vl.Count < depth)
                {
                    vls.Add(vl[curri]);//将当前结点添加至vls中
                    vl.RemoveAt(curri);//同时在vl中移除改点
                    break;
                }
                else
                {
                    vls.Add(vl[curri]);//将当前结点添加至vls中
                    vl.RemoveAt(curri);//同时在vl中移除改点
                }
            }
            return vls;
        }
        public List<vertex> Dijask3(int StartID, int EndID)
        {
            List<vertex> vls = new List<vertex>();//存放被检查过的点
            List<vertex> vl = new List<vertex>();//存放未被检查的点
            if (StartID == EndID)//如果终点和起点一致
            {
                MessageBox.Show("错误：终点与起点一致");
                return vls;//结束检索
            }
            int myds1count = myds1.Tables[0].Rows.Count;
            for (int i = 0; i < myds1count; i++)//遍历所有节点
            {
                vertex vt = new vertex();//新建节点实例
                vt.thisID = (int)myds1.Tables[0].Rows[i][0];//将中间变量转换为整型，并赋给vt.thisID
                //vt.class1 = (int)myds1.Tables[0].Rows[i][1];//添加点归属
                if (vt.thisID == StartID)//如果当前结点的ID是起点
                {
                    vt.Len = 0.0;//将当前点的权重设置为0
                    vt.FromID = -1;//将当前点的前驱结点设置为-1，表明没有前驱
                    vt.LineID = 0;
                    vt.Traffic_l = 0;
                    vls.Add(vt);//将起点添加至vls中
                }
                else//对于不是起点的结点
                {
                    vt.Len = infinite;//将权重设置为无限，表示连通性未知或者无法连通
                    vt.FromID = -1;//前驱结点设为-1
                    vt.LineID = 0;
                    vt.Traffic_l = 0;
                    vl.Add(vt);//将结点添加至vl
                }
            }
            List<System.Data.DataRow> temp = new List<System.Data.DataRow>();//用于存放从线路表中读取到的完整记录
            for (int i = 0; i < myds2.Tables[0].Rows.Count; i++)//循环整个线路表表
            {
                //如果当前起点或者终点包含有起点编号，即检索与起点相连的结点
                if ((int)myds2.Tables[0].Rows[i][6] == 1)
                {
                    if ((int)myds2.Tables[0].Rows[i][2] == StartID)
                    {
                        temp.Add(myds2.Tables[0].Rows[i]);//将该记录添加到temp中
                    }
                }
            }
            for (int i = 0; i < vl.Count; i++)//循环所有未被检查的结点
            {
                for (int j = 0; j < temp.Count; j++)//对于每一个节点，都循环一遍temp表
                {
                    //如果当前结点位于记录的起点或者终点处
                    if ((int)temp[j][6] == 1)
                    {
                        if (vl[i].thisID == (int)temp[j][3])
                        {
                            vl[i].FromID = StartID;//则将次结点的前驱设置为起点
                            vl[i].Len = double.Parse(temp[j][5].ToString());//将权重值设置为对应记录中的权重值
                            vl[i].LineID = (int)temp[j][1];
                            vl[i].Traffic_l = (int)temp[j][7];
                        }
                    }
                }
            }
            double minLen = 0;//最小权重
            int minID = 0;//权重最小的点的ID
            int curri = 0;//当前点位于数组中下标
            int counter = 0;//计数器
            vertex tempv = new vertex();//临时结点存放变量
            while (vl.Count != 0)//如果vl不为空
            {
                minLen = vl[0].Len;//将数组中第一条记录的权重值付给minLen
                minID = vl[0].thisID;//将ID也付给minID
                curri = 0;//当前数组下标重置为0
                tempv = vl[0];//将当前结点付给tempv
                for (int i = 0; i < vl.Count; i++)//循环所有vl中的结点
                {
                    if (minLen > vl[i].Len)//比较是否有新的权重值比当前权重值小
                    {
                        minLen = vl[i].Len;//更新权重值为更小的
                        minID = vl[i].thisID;//更新ID
                        curri = i;//记录其下标，方便之后删除
                        tempv = vl[i];//更新tempv的结点为当前结点
                    }
                }
                temp.Clear();//清空temp数组
                for (int i = 0; i < myds2.Tables[0].Rows.Count; i++)//循环整个结点表
                {
                    //找出包含当前ID的所有记录
                    if ((int)myds2.Tables[0].Rows[i][6] == 1)
                    {
                        if ((int)myds2.Tables[0].Rows[i][2] == minID)
                        {
                            temp.Add(myds2.Tables[0].Rows[i]);//保存至temp中
                        }
                    }
                }
                counter = 0;//计数器重置为0
                double templen = 0;//临时总的权重值，用于下面比较和更新
                while (counter < vl.Count)//遍历整个vl表
                {
                    for (int i = 0; i < temp.Count; i++)//每一个节点，比较所有temp中的记录
                    {
                        //检查记录中是否包含当前结点，找出与当前结点相关联的点
                        if ((int)temp[i][6] == 1)
                        {
                            if (vl[counter].thisID == (int)temp[i][3])
                            {
                                templen = double.Parse(temp[i][5].ToString()) + tempv.Len;//计算总的权重值
                                if (templen < vl[counter].Len)//如果新的权重值比当前存放的权重值小
                                {
                                    vl[counter].Len = templen;//更新为新的权重值
                                    vl[counter].FromID = tempv.thisID;//同时更新该点为vl中点的前驱ID
                                    vl[counter].LineID = (int)temp[i][1];
                                    vl[counter].Traffic_l = (int)temp[i][7];
                                }
                            }
                        }
                    }
                    counter += 1;//计数器加1，知道当前while循环结束
                }
                vls.Add(vl[curri]);//将当前结点添加至vls中
                vl.RemoveAt(curri);//同时在vl中移除改点
            }
            return vls;
        }
        public List<vertex> Dijask2(int StartID, int EndID, int depth = 0)
        {
            List<vertex> vls = new List<vertex>();//存放被检查过的点
            List<vertex> vl = new List<vertex>();//存放未被检查的点
            if (StartID == EndID)//如果终点和起点一致
            {
                MessageBox.Show("错误：终点与起点一致");
                return vls;//结束检索
            }
            int myds1count = myds1.Tables[0].Rows.Count;
            for (int i = 0; i < myds1count; i++)//遍历所有节点
            {
                vertex vt = new vertex();//新建节点实例
                vt.thisID = (int)myds1.Tables[0].Rows[i][0];//将中间变量转换为整型，并赋给vt.thisID
                //vt.class1 = (int)myds1.Tables[0].Rows[i][1];//添加点归属
                if (vt.thisID == StartID)//如果当前结点的ID是起点
                {
                    vt.Len = 0.0;//将当前点的权重设置为0
                    vt.FromID = -1;//将当前点的前驱结点设置为-1，表明没有前驱
                    vt.LineID = 0;
                    vt.Traffic_l = 0;
                    vt.class1 = (int)myds1.Tables[0].Rows[i][1];
                    vls.Add(vt);//将起点添加至vls中
                }
                else//对于不是起点的结点
                {
                    vt.Len = infinite;//将权重设置为无限，表示连通性未知或者无法连通
                    vt.FromID = -1;//前驱结点设为-1
                    vt.LineID = 0;
                    vt.Traffic_l = 0;
                    vt.class1 = (int)myds1.Tables[0].Rows[i][1];
                    vl.Add(vt);//将结点添加至vl
                }
            }
            List<System.Data.DataRow> temp = new List<System.Data.DataRow>();//用于存放从线路表中读取到的完整记录
            for (int i = 0; i < myds2.Tables[0].Rows.Count; i++)//循环整个线路表表
            {
                //如果当前起点或者终点包含有起点编号，即检索与起点相连的结点
                if ((int)myds2.Tables[0].Rows[i][6] == 2)
                {
                    if ((int)myds2.Tables[0].Rows[i][2] == StartID || (int)myds2.Tables[0].Rows[i][3] == StartID)
                    {
                        temp.Add(myds2.Tables[0].Rows[i]);//将该记录添加到temp中
                    }
                }
                else if ((int)myds2.Tables[0].Rows[i][6] == 1)
                {
                    if ((int)myds2.Tables[0].Rows[i][2] == StartID)
                    {
                        temp.Add(myds2.Tables[0].Rows[i]);//将该记录添加到temp中
                    }
                }
            }
            for (int i = 0; i < vl.Count; i++)//循环所有未被检查的结点
            {
                for (int j = 0; j < temp.Count; j++)//对于每一个节点，都循环一遍temp表
                {
                    //如果当前结点位于记录的起点或者终点处
                    if ((int)temp[j][6] == 2)
                    {
                        if (vl[i].thisID == (int)temp[j][2] || vl[i].thisID == (int)temp[j][3])
                        {
                            vl[i].FromID = StartID;//则将次结点的前驱设置为起点
                            vl[i].Len = double.Parse(temp[j][5].ToString());//将权重值设置为对应记录中的权重值
                            vl[i].LineID = (int)temp[j][1];
                            vl[i].Traffic_l = (int)temp[j][7];
                        }
                    }
                    else if ((int)temp[j][6] == 1)
                    {
                        if (vl[i].thisID == (int)temp[j][3])
                        {
                            vl[i].FromID = StartID;//则将次结点的前驱设置为起点
                            vl[i].Len = double.Parse(temp[j][5].ToString());//将权重值设置为对应记录中的权重值
                            vl[i].LineID = (int)temp[j][1];
                            vl[i].Traffic_l = (int)temp[j][7];
                        }
                    }
                }
            }
            double minLen = 0;//最小权重
            int minID = 0;//权重最小的点的ID
            int curri = 0;//当前点位于数组中下标
            int counter = 0;//计数器
            vertex tempv = new vertex();//临时结点存放变量
            while (vl.Count != 0)//如果vl不为空
            {
                minLen = vl[0].Len;//将数组中第一条记录的权重值付给minLen
                minID = vl[0].thisID;//将ID也付给minID
                curri = 0;//当前数组下标重置为0
                tempv = vl[0];//将当前结点付给tempv
                for (int i = 0; i < vl.Count; i++)//循环所有vl中的结点
                {
                    if (minLen > vl[i].Len)//比较是否有新的权重值比当前权重值小
                    {
                        minLen = vl[i].Len;//更新权重值为更小的
                        minID = vl[i].thisID;//更新ID
                        curri = i;//记录其下标，方便之后删除
                        tempv = vl[i];//更新tempv的结点为当前结点
                    }
                }
                temp.Clear();//清空temp数组
                for (int i = 0; i < myds2.Tables[0].Rows.Count; i++)//循环整个结点表
                {
                    //找出包含当前ID的所有记录
                    if ((int)myds2.Tables[0].Rows[i][6] == 2)
                    {
                        if ((int)myds2.Tables[0].Rows[i][2] == minID || (int)myds2.Tables[0].Rows[i][3] == minID)
                        {
                            temp.Add(myds2.Tables[0].Rows[i]);//保存至temp中
                        }
                    }
                    else if ((int)myds2.Tables[0].Rows[i][6] == 1)
                    {
                        if ((int)myds2.Tables[0].Rows[i][2] == minID)
                        {
                            temp.Add(myds2.Tables[0].Rows[i]);//保存至temp中
                        }
                    }
                }
                counter = 0;//计数器重置为0
                double templen = 0;//临时总的权重值，用于下面比较和更新
                while (counter < vl.Count)//遍历整个vl表
                {
                    for (int i = 0; i < temp.Count; i++)//每一个节点，比较所有temp中的记录
                    {
                        //检查记录中是否包含当前结点，找出与当前结点相关联的点
                        if ((int)temp[i][6] == 2)
                        {
                            if (vl[counter].thisID == (int)temp[i][2] || vl[counter].thisID == (int)temp[i][3])
                            {
                                templen = double.Parse(temp[i][5].ToString()) + tempv.Len;//计算总的权重值
                                if (templen < vl[counter].Len)//如果新的权重值比当前存放的权重值小
                                {
                                    vl[counter].Len = templen;//更新为新的权重值
                                    vl[counter].FromID = tempv.thisID;//同时更新该点为vl中点的前驱ID
                                    vl[counter].LineID = (int)temp[i][1];
                                    vl[counter].Traffic_l = (int)temp[i][7];
                                }
                            }
                        }
                        else if ((int)temp[i][6] == 1)
                        {
                            if (vl[counter].thisID == (int)temp[i][3])
                            {
                                templen = double.Parse(temp[i][5].ToString()) + tempv.Len;//计算总的权重值
                                if (templen < vl[counter].Len)//如果新的权重值比当前存放的权重值小
                                {
                                    vl[counter].Len = templen;//更新为新的权重值
                                    vl[counter].FromID = tempv.thisID;//同时更新该点为vl中点的前驱ID
                                    vl[counter].LineID = (int)temp[i][1];
                                    vl[counter].Traffic_l = (int)temp[i][7];
                                }
                            }
                        }
                    }
                    counter += 1;//计数器加1，知道当前while循环结束
                }
                vls.Add(vl[curri]);//将当前结点添加至vls中
                vl.RemoveAt(curri);//同时在vl中移除改点
            }
            return vls;
        }
        public List<vertex> Dijask1(int StartID, int EndID, int depth = 0)
        {
            List<vertex> vls = new List<vertex>();//存放被检查过的点
            List<vertex> vl = new List<vertex>();//存放未被检查的点
            if (StartID == EndID)//如果终点和起点一致
            {
                MessageBox.Show("错误：终点与起点一致");
                return vls;//结束检索
            }
            int myds1count = myds1.Tables[0].Rows.Count;
            for (int i = 0; i < myds1count; i++)//遍历所有节点
            {
                vertex vt = new vertex();//新建节点实例
                vt.thisID = (int)myds1.Tables[0].Rows[i][0];//将中间变量转换为整型，并赋给vt.thisID
                if (vt.thisID == StartID)//如果当前结点的ID是起点
                {
                    vt.Len = 0.0;//将当前点的权重设置为0
                    vt.FromID = -1;//将当前点的前驱结点设置为-1，表明没有前驱
                    vt.LineID = 0;
                    vt.Traffic_l = 0;
                    vls.Add(vt);//将起点添加至vls中
                }
                else//对于不是起点的结点
                {
                    vt.Len = infinite;//将权重设置为无限，表示连通性未知或者无法连通
                    vt.FromID = -1;//前驱结点设为-1
                    vt.LineID = 0;
                    vt.Traffic_l = 0;
                    vl.Add(vt);//将结点添加至vl
                }
            }
            List<System.Data.DataRow> temp = new List<System.Data.DataRow>();//用于存放从线路表中读取到的完整记录
            for (int i = 0; i < myds2.Tables[0].Rows.Count; i++)//循环整个线路表表
            {
                //如果当前起点或者终点包含有起点编号，即检索与起点相连的结点
                if ((int)myds2.Tables[0].Rows[i][2] == StartID || (int)myds2.Tables[0].Rows[i][3] == StartID)
                {
                    temp.Add(myds2.Tables[0].Rows[i]);//将该记录添加到temp中
                }
            }
            for (int i = 0; i < vl.Count; i++)//循环所有未被检查的结点
            {
                for (int j = 0; j < temp.Count; j++)//对于每一个节点，都循环一遍temp表
                {
                    //如果当前结点位于记录的起点或者终点处
                    if (vl[i].thisID == (int)temp[j][2] || vl[i].thisID == (int)temp[j][3])
                    {
                        vl[i].FromID = StartID;//则将次结点的前驱设置为起点
                        vl[i].Len = double.Parse(temp[j][5].ToString());//将权重值设置为对应记录中的权重值
                        vl[i].LineID = (int)temp[j][1];
                        vl[i].Traffic_l = (int)temp[j][7];
                    }
                }
            }
            double minLen = 0;//最小权重
            int minID = 0;//权重最小的点的ID
            int curri = 0;//当前点位于数组中下标
            int counter = 0;//计数器
            vertex tempv = new vertex();//临时结点存放变量
            while (vl.Count != 0)//如果vl不为空
            {
                minLen = vl[0].Len;//将数组中第一条记录的权重值付给minLen
                minID = vl[0].thisID;//将ID也付给minID
                curri = 0;//当前数组下标重置为0
                tempv = vl[0];//将当前结点付给tempv
                for (int i = 0; i < vl.Count; i++)//循环所有vl中的结点
                {
                    if (minLen > vl[i].Len)//比较是否有新的权重值比当前权重值小
                    {
                        minLen = vl[i].Len;//更新权重值为更小的
                        minID = vl[i].thisID;//更新ID
                        curri = i;//记录其下标，方便之后删除
                        tempv = vl[i];//更新tempv的结点为当前结点
                    }
                }
                temp.Clear();//清空temp数组
                for (int i = 0; i < myds2.Tables[0].Rows.Count; i++)//循环整个结点表
                {
                    //找出包含当前ID的所有记录
                    if ((int)myds2.Tables[0].Rows[i][2] == minID || (int)myds2.Tables[0].Rows[i][3] == minID)
                    {
                        temp.Add(myds2.Tables[0].Rows[i]);//保存至temp中
                    }
                }
                counter = 0;//计数器重置为0
                double templen = 0;//临时总的权重值，用于下面比较和更新
                while (counter < vl.Count)//遍历整个vl表
                {
                    for (int i = 0; i < temp.Count; i++)//每一个节点，比较所有temp中的记录
                    {
                        //检查记录中是否包含当前结点，找出与当前结点相关联的点
                        if (vl[counter].thisID == (int)temp[i][2] || vl[counter].thisID == (int)temp[i][3])
                        {
                            templen = double.Parse(temp[i][5].ToString()) + tempv.Len;//计算总的权重值
                            if (templen < vl[counter].Len)//如果新的权重值比当前存放的权重值小
                            {
                                vl[counter].Len = templen;//更新为新的权重值
                                vl[counter].FromID = tempv.thisID;//同时更新该点为vl中点的前驱ID
                                vl[counter].LineID = (int)temp[i][1];
                                vl[i].Traffic_l = (int)temp[i][7];
                            }
                        }
                    }
                    counter += 1;//计数器加1，知道当前while循环结束
                }
                vls.Add(vl[curri]);//将当前结点添加至vls中
                vl.RemoveAt(curri);//同时在vl中移除改点
            }
            return vls;
        }
        public List<string> queue_path(List<vertex> vls, int EndID)
        {
            List<vertex> routex = new List<vertex>();
            List<string> linestring = new List<string>();
            int des = EndID;
            int maxl = 0;
            while (des != -1)//如果des没有到达起点
            {
                for (int i = 0; i < vls.Count; i++)//循环整个vls
                {
                    if (vls[i].thisID == des)//找出当前ID为des的点
                    {
                        routex.Add(vls[i]);//将这个点添加至路径点中
                        maxl += vls[i].Traffic_l;
                        des = vls[i].FromID;//将des设置为当前点的前驱ID，用于回溯上一个节点
                        for (int ii = 0; ii < dss.Tables[0].Rows.Count; ii++)
                        {
                            if (vls[i].LineID == (int)dss.Tables[0].Rows[ii][0])
                            {
                                linestring.Add(dss.Tables[0].Rows[ii][1].ToString());
                            }
                        }
                    }
                }
            }
            string aa = "";
            for (int i = 0; i < routex.Count; i++)
            {
                aa += routex[i].FromID + " <- ";
            }
            aa += "\n经过 " + maxl + " 个路口";
            MessageBox.Show(aa);
            return linestring;
        }
        public void show_path(List<string> path)
        {
            //if(layerhandle != -1)
            //m1.axMap1.RemoveLayer(layerhandle);
            m1.fss = new MapWinGIS.Shapefile();
            m1.fss.CreateNewWithShapeID("", MapWinGIS.ShpfileType.SHP_POLYLINE);
            int index = 0;
            for (int i = 0; i < path.Count; i++)
            {
                m1.fst = new MapWinGIS.Shape();
                m1.fst.Create(MapWinGIS.ShpfileType.SHP_POLYLINE);
                m1.fst.ImportFromWKT(path[i]);
                index = m1.fss.NumShapes;
                m1.fss.EditInsertShape(m1.fst, ref index);
            }
            int a = m1.axMap1.NumLayers;
            layerhandle = m1.axMap1.AddLayer(m1.fss, true);
            Utils utils = new Utils();
            LinePattern pattern = new LinePattern();
            pattern = new LinePattern();
            pattern.AddLine(utils.ColorByName(tkMapColor.Yellow), 6.0f, tkDashStyle.dsSolid);
            pattern.AddLine(utils.ColorByName(tkMapColor.Red), 4.0f, tkDashStyle.dsSolid);

            ShapefileCategory ct = m1.fss.Categories.Add("River");
            ct.DrawingOptions.LinePattern = pattern;
            ct.DrawingOptions.UseLinePattern = true;
            for (int i = m1.fss.NumShapes - 1; i > -1; i--)
            {
                m1.fss.set_ShapeCategory(i, 0);
            }
        }
        public void show_path2(List<string> path)
        {
            m1.fss = new MapWinGIS.Shapefile();
            m1.fss.CreateNew("", MapWinGIS.ShpfileType.SHP_POLYLINE);
            int index = 0;
            for (int i = 0; i < path.Count; i++)
            {
                m1.fst = new MapWinGIS.Shape();
                m1.fst.Create(MapWinGIS.ShpfileType.SHP_POLYLINE);
                m1.fst.ImportFromWKT(path[i]);
                index = m1.fss.NumShapes;
                m1.fss.EditInsertShape(m1.fst, ref index);
            }
            layerhandle = m1.axMap1.AddLayer((MapWinGIS.Shapefile)m1.fss, true);
            Utils utils = new Utils();
            LinePattern pattern = new LinePattern();
            pattern.AddLine(utils.ColorByName(tkMapColor.Gray), 8.0f, tkDashStyle.dsSolid);
            pattern.AddLine(utils.ColorByName(tkMapColor.Yellow), 7.0f, tkDashStyle.dsSolid);
            LineSegment segm = pattern.AddMarker(tkDefaultPointSymbol.dpsArrowRight);
            segm.Color = utils.ColorByName(tkMapColor.Orange);
            segm.MarkerSize = 10;
            segm.MarkerInterval = 32;
            ShapefileCategory ct = m1.fss.Categories.Add("Direction");
            ct.DrawingOptions.LinePattern = pattern;
            ct.DrawingOptions.UseLinePattern = true;
            for (int i = m1.fss.NumShapes - 1; i > -1; i--)
            {
                m1.fss.set_ShapeCategory(i, 0);
            }
        }
        public int text2num(string txt)
        {
            string sql = "SELECT ID FROM test_vertex WHERE NAME = '" + txt + "'";
            DataTable dt = new DataTable();
            m1.ODA = new OdbcDataAdapter(sql, m1.MyConnection);
            m1.ODABuilder = new OdbcCommandBuilder(m1.ODA);
            m1.ODA.Fill(dt);
            if (dt.Rows.Count != 0)
            {
                return (int)dt.Rows[0][0];
            }
            else
            {
                return -1;
            }
        }
        public int pointpart(int PID, List<vertex> vls, List<vertex> vls2)
        {
            int i = 0;
            double x, y, d;
            while (PID != (int)dt.Rows[i]["ID"])
            {
                i++;
            }
            x = (double)dt.Rows[i]["X"];
            y = (double)dt.Rows[i]["Y"];
            var distance = new List<KeyValuePair<double, int>>();
            var d2 = new List<KeyValuePair<double, int>>();
            for (i = 0; i < dt.Rows.Count; i++)
            {
                if ((int)dt.Rows[i]["NOTE"] == 1)
                {
                    d = Math.Abs(Math.Sqrt(Math.Pow(x - (double)dt.Rows[i]["X"], 2) + Math.Pow(y - (double)dt.Rows[i]["Y"], 2)));
                    if (d < sphere)
                    {
                        distance.Add(new KeyValuePair<double, int>(d, (int)dt.Rows[i]["ID"]));
                    }
                }
            }
            List<vertex> vll = new List<vertex>();
            for (i = 0; i < vls.Count; i++)
            {
                for (int j = 0; j < distance.Count; j++)
                {
                    if (vls[i].thisID == distance[j].Value)
                    {
                        vll.Add(vls[i]);
                    }
                }
            }

            for(i =0;i<vll.Count;i++)
            {
                for(int j =0;j<vls2.Count;j++)
                {
                    if(vls2[j].thisID == vll[i].thisID)
                    {
                        d2.Add(new KeyValuePair<double,int>(vls2[j].Len+vll[i].Len,vls2[j].thisID));
                    }
                }
            }
            d2.Sort(Compare1);
            return d2[0].Value;
        }
        public int pointpart2(int PID, List<vertex> vls)
        {
            int i = 0;
            double x, y, d;
            while (PID != (int)dt.Rows[i]["ID"])
            {
                i++;
            }
            x = (double)dt.Rows[i]["X"];
            y = (double)dt.Rows[i]["Y"];
            var distance = new List<KeyValuePair<double, int>>();
            var d2 = new List<KeyValuePair<double, int>>();
            for (i = 0; i < dt.Rows.Count; i++)
            {
                if ((int)dt.Rows[i]["NOTE"] == 1)
                {
                    d = Math.Abs(Math.Sqrt(Math.Pow(x - (double)dt.Rows[i]["X"], 2) + Math.Pow(y - (double)dt.Rows[i]["Y"], 2)));
                    if (d < sphere)
                    {
                        distance.Add(new KeyValuePair<double, int>(d, (int)dt.Rows[i]["ID"]));
                    }
                }
            }
            List<vertex> vll = new List<vertex>();
            for (i = 0; i < vls.Count; i++)
            {
                for (int j = 0; j < distance.Count; j++)
                {
                    if (vls[i].thisID == distance[j].Value)
                    {
                        d2.Add(new KeyValuePair<double, int>(vls[i].Len, vls[i].thisID));
                        vll.Add(vls[i]);
                    }
                }
            }
            d2.Sort(Compare1);
            return d2[0].Value;
        }
        public int get_Note(int PID)
        {
            string sql = "Select NOTE From test_vertex Where ID = '"+PID+"'";
            DataTable dt = new DataTable();
            m1.ODA = new OdbcDataAdapter(sql, m1.MyConnection);
            m1.ODA = new OdbcDataAdapter(sql, m1.MyConnection);
            m1.ODABuilder = new OdbcCommandBuilder(m1.ODA);
            m1.ODA.Fill(dt);
            return (int)dt.Rows[0]["NOTE"];
        }
        public void show_pt(int SID, int EID)
        {
            //m1.axMap1.RemoveLayer(layerhandle2);
            //m1.axMap1.RemoveLayer(layerhandle3);
            DataTable dt = new DataTable();
            string sql = "Select ID,NOTE,X,Y From test_vertex WHERE ID = '" + SID + "' OR ID = '" + EID + "';";
            m1.ODA = new OdbcDataAdapter(sql, m1.MyConnection);
            m1.ODABuilder = new OdbcCommandBuilder(m1.ODA);
            m1.ODA.Fill(dt);
            double x1, x2, y1, y2;
            if ((int)dt.Rows[0]["ID"] == SID)
            {
                x1 = (double)dt.Rows[0]["X"];
                y1 = (double)dt.Rows[0]["Y"];
            }
            else
            {
                x1 = (double)dt.Rows[1]["X"];
                y1 = (double)dt.Rows[1]["Y"];
            }
            if ((int)dt.Rows[1]["ID"] == EID)
            {
                x2 = (double)dt.Rows[1]["X"];
                y2 = (double)dt.Rows[1]["Y"];
            }
            else
            {
                x2 = (double)dt.Rows[0]["X"];
                y2 = (double)dt.Rows[0]["Y"];
            }
            var sf = new Shapefile();
            sf.CreateNewWithShapeID("", ShpfileType.SHP_POINT);
            var sf2 = new Shapefile();
            sf2.CreateNewWithShapeID("", ShpfileType.SHP_POINT);
            for (int i = 0; i < 2; i++)
            {
                var pnt = new MapWinGIS.Point();
                if (i == 0)
                {
                    pnt.x = x1;
                    pnt.y = y1;
                    Shape shp = new Shape();
                    shp.Create(ShpfileType.SHP_POINT);

                    int index = 0;
                    shp.InsertPoint(pnt, ref index);
                    sf.EditInsertShape(shp, ref index);
                }
                else
                {
                    pnt.x = x2;
                    pnt.y = y2;
                    Shape shp = new Shape();
                    shp.Create(ShpfileType.SHP_POINT);

                    int index = 0;
                    shp.InsertPoint(pnt, ref index);
                    sf2.EditInsertShape(shp, ref index);
                }
                
            }
            MapWinGIS.Image img = new MapWinGIS.Image();
            img.Open("C:\\Users\\xwjtb\\Desktop\\pin4.png",ImageType.USE_FILE_EXTENSION, true, null);
            ShapeDrawingOptions options = sf.DefaultDrawingOptions;
            options.PointType = tkPointSymbolType.ptSymbolPicture;
            options.Picture = img;
            sf.CollisionMode = tkCollisionMode.AllowCollisions;
            layerhandle2 = m1.axMap1.AddLayer(sf, true);
            MapWinGIS.Image img2 = new MapWinGIS.Image();
            img2.Open("C:\\Users\\xwjtb\\Desktop\\pin5.png", ImageType.USE_FILE_EXTENSION, true, null);
            ShapeDrawingOptions options2 = sf2.DefaultDrawingOptions;
            options2.PointType = tkPointSymbolType.ptSymbolPicture;
            options2.Picture = img2;
            sf2.CollisionMode = tkCollisionMode.AllowCollisions;
            layerhandle3 = m1.axMap1.AddLayer(sf2, true);
        }
        public bool aaa(int SID,int EID)
        {
            m1.axMap1.RemoveLayer(layerhandle2);
            DataTable dt = new DataTable();
            string sql = "Select ID,NOTE,X,Y From test_vertex WHERE ID = '"+SID+"' OR ID = '"+EID+"';";
            m1.ODA = new OdbcDataAdapter(sql, m1.MyConnection);
            m1.ODABuilder = new OdbcCommandBuilder(m1.ODA);
            m1.ODA.Fill(dt);
            double x1, x2, y1, y2;
            x1 = (double)dt.Rows[0]["X"];
            y1 = (double)dt.Rows[0]["Y"];
            x2 = (double)dt.Rows[1]["X"];
            y2 = (double)dt.Rows[1]["Y"];
            double d = Math.Abs(Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2)));
            if (d < 500)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                contextMenuStrip1.Show(e.Location);
            }
        }
        public List<KeyValuePair<List<KeyValuePair<DataRow, int>>, int>> bustran(int sid, int eid)
        {
            int UP = 1;
            int DOWN = 2;
            DataRow d1 = bus_conn.Tables[0].NewRow();
            DataRow d2 = bus_conn.Tables[0].NewRow();
            DataRow d3 = bus_conn.Tables[0].NewRow();
            DataRow d4 = bus_conn.Tables[0].NewRow();
            List<KeyValuePair<DataRow, int>> startline;//存储开始节点
            List<KeyValuePair<DataRow, int>> endline;//存储开始节点
            List<KeyValuePair<DataRow, int>> middleline;//存储开始节点
            List<KeyValuePair<List<KeyValuePair<DataRow, int>>, int>> busline = new List<KeyValuePair<List<KeyValuePair<DataRow, int>>, int>>();//存储线路
            List<KeyValuePair<List<KeyValuePair<DataRow, int>>, int>> busline5 = new List<KeyValuePair<List<KeyValuePair<DataRow, int>>, int>>();//存储线路
            List<KeyValuePair<List<KeyValuePair<DataRow, int>>, int>> busline4 = new List<KeyValuePair<List<KeyValuePair<DataRow, int>>, int>>();//存储线路
            List<KeyValuePair<List<KeyValuePair<DataRow, int>>, int>> busline3 = new List<KeyValuePair<List<KeyValuePair<DataRow, int>>, int>>();//存储线路
            List<KeyValuePair<List<KeyValuePair<DataRow, int>>, int>> busline2 = new List<KeyValuePair<List<KeyValuePair<DataRow, int>>, int>>();//存储线路
            List<KeyValuePair<List<KeyValuePair<DataRow, int>>, int>> busline_result1 = new List<KeyValuePair<List<KeyValuePair<DataRow, int>>, int>>();//存储线路
            List<KeyValuePair<List<KeyValuePair<DataRow, int>>, int>> busline_result2 = new List<KeyValuePair<List<KeyValuePair<DataRow, int>>, int>>();//存储线路
            List<KeyValuePair<List<KeyValuePair<DataRow, int>>, int>> busline_result3 = new List<KeyValuePair<List<KeyValuePair<DataRow, int>>, int>>();//存储线路
            List<KeyValuePair<List<KeyValuePair<DataRow, int>>, int>> busline_result4 = new List<KeyValuePair<List<KeyValuePair<DataRow, int>>, int>>();//存储线路
            List<KeyValuePair<List<KeyValuePair<DataRow, int>>, int>> busline_result = new List<KeyValuePair<List<KeyValuePair<DataRow, int>>, int>>();//存储线路
            //分别获取上行或下行中包含sid的记录
            for (int i = 0; i < bus_conn.Tables[0].Rows.Count; i++)
            {
                if ((int)bus_conn.Tables[0].Rows[i]["STOP_ID"] == sid)
                {
                    startline = new List<KeyValuePair<DataRow, int>>();
                    if ((int)bus_conn.Tables[0].Rows[i]["SEGUP"] > 0 && (int)bus_conn.Tables[0].Rows[i]["SEGDN"] > 0)
                    {
                        if ((int)bus_conn.Tables[0].Rows[i]["SEGUP"] > (int)bus_conn.Tables[0].Rows[i]["SEGDN"])
                        {
                            startline.Add(new KeyValuePair<DataRow, int>(bus_conn.Tables[0].Rows[i], (int)bus_conn.Tables[0].Rows[i]["SEGDN"]));
                            busline.Add(new KeyValuePair<List<KeyValuePair<DataRow, int>>, int>(startline, DOWN));
                        }
                        else
                        {
                            startline.Add(new KeyValuePair<DataRow, int>(bus_conn.Tables[0].Rows[i], (int)bus_conn.Tables[0].Rows[i]["SEGUP"]));
                            busline.Add(new KeyValuePair<List<KeyValuePair<DataRow, int>>, int>(startline, UP));
                        }
                    }
                    else if ((int)bus_conn.Tables[0].Rows[i]["SEGUP"] > 0 && (int)bus_conn.Tables[0].Rows[i]["SEGDN"] < 0)
                    {
                        startline.Add(new KeyValuePair<DataRow, int>(bus_conn.Tables[0].Rows[i], (int)bus_conn.Tables[0].Rows[i]["SEGUP"]));
                        busline.Add(new KeyValuePair<List<KeyValuePair<DataRow, int>>, int>(startline, UP));
                    }
                    else if ((int)bus_conn.Tables[0].Rows[i]["SEGUP"] < 0 && (int)bus_conn.Tables[0].Rows[i]["SEGDN"] > 0)
                    {
                        startline.Add(new KeyValuePair<DataRow, int>(bus_conn.Tables[0].Rows[i], (int)bus_conn.Tables[0].Rows[i]["SEGDN"]));
                        busline.Add(new KeyValuePair<List<KeyValuePair<DataRow, int>>, int>(startline, DOWN));
                    }
                }
            }
            //获取每一条线路
            for (int i = 0; i < busline.Count; i++)
            {
                if (busline[i].Value == DOWN)
                {
                    for (int j = 0; j < bus_conn.Tables[0].Rows.Count; j++)
                    {
                        if ((int)bus_conn.Tables[0].Rows[j]["BUSLINE"] == (int)busline[i].Key[0].Key["BUSLINE"] && (int)bus_conn.Tables[0].Rows[j]["SEGDN"] > 0)
                        {
                            if ((int)bus_conn.Tables[0].Rows[j]["SEGDN"] > busline[i].Key[0].Value)
                                busline[i].Key.Add(new KeyValuePair<DataRow, int>(bus_conn.Tables[0].Rows[j], (int)bus_conn.Tables[0].Rows[j]["SEGDN"]));
                        }
                    }
                }
                else if (busline[i].Value == UP)
                {
                    for (int j = 0; j < bus_conn.Tables[0].Rows.Count; j++)
                    {
                        if ((int)bus_conn.Tables[0].Rows[j]["BUSLINE"] == (int)busline[i].Key[0].Key["BUSLINE"] && (int)bus_conn.Tables[0].Rows[j]["SEGUP"] > 0)
                        {
                            if ((int)bus_conn.Tables[0].Rows[j]["SEGUP"] > busline[i].Key[0].Value)
                                busline[i].Key.Add(new KeyValuePair<DataRow, int>(bus_conn.Tables[0].Rows[j], (int)bus_conn.Tables[0].Rows[j]["SEGUP"]));
                        }
                    }
                }
            }
            //搜索终点是否在busline中
            for (int i = 0; i < busline.Count; i++)
            {
                busline[i].Key.Sort(Compare3);
                for (int j = 0; j < busline[i].Key.Count; j++)
                {
                    if ((int)busline[i].Key[j].Key["STOP_ID"] == eid)
                    {
                        middleline = new List<KeyValuePair<DataRow,int>>();
                        for (int ii = 0; ii < j + 1; ii++)
                        {
                            middleline.Add(new KeyValuePair<DataRow, int>(busline[i].Key[ii].Key, ii + 1));
                        }
                        busline_result1.Add(new KeyValuePair<List<KeyValuePair<DataRow, int>>, int>(middleline, middleline.Count));
                        //busline_result1.Add(new KeyValuePair<List<KeyValuePair<DataRow, int>>, int>(busline[i].Key, busline[i].Key.Count - j - 1));
                        //busline_result1[busline_result1.Count - 1].Key.RemoveRange(j + 1, busline_result1[busline_result1.Count - 1].Key.Count - j - 1);
                    }
                }
            }
            
            //直达搜索完成，busline是结果；
            if (busline_result1.Count == 0)
            {
                int sid_p = 0
                    , eid_p = 0;//起点终点的同名站点
                for (int i = 0; i < bus_conn.Tables[0].Rows.Count; i++)
                {
                    if ((int)bus_conn.Tables[0].Rows[i]["STOP_ID"] == eid)
                    {
                        eid_p = (int)bus_conn.Tables[0].Rows[i]["P_STOP_ID"];
                        d3 = bus_conn.Tables[0].Rows[i];//eid
                    }
                    else if ((int)bus_conn.Tables[0].Rows[i]["STOP_ID"] == sid)
                    {
                        sid_p = (int)bus_conn.Tables[0].Rows[i]["P_STOP_ID"];
                        d4 = bus_conn.Tables[0].Rows[i];//sid
                    }
                }
                for (int i = 0; i < bus_conn.Tables[0].Rows.Count; i++)
                {
                    if ((int)bus_conn.Tables[0].Rows[i]["P_STOP_ID"] == eid_p && (int)bus_conn.Tables[0].Rows[i]["STOP_ID"] != eid)
                    {
                        d1 = bus_conn.Tables[0].Rows[i];//eid_p
                    }
                    else if ((int)bus_conn.Tables[0].Rows[i]["P_STOP_ID"] == sid_p && (int)bus_conn.Tables[0].Rows[i]["STOP_ID"] != sid)
                    {
                        d2 = bus_conn.Tables[0].Rows[i];//sid_p
                    }
                }
                for (int i = 0; i < busline.Count; i++)
                {
                    for (int j = 0; j < busline[i].Key.Count; j++)
                    {
                        int a;
                        if (int.TryParse(d1["STOP_ID"].ToString(), out a))
                            if ((int)busline[i].Key[j].Key["STOP_ID"] == (int)d1["STOP_ID"])
                            {
                                middleline = new List<KeyValuePair<DataRow,int>>();
                                for (int ii = 0; ii < j+1; ii++)
                                {
                                    middleline.Add(new KeyValuePair<DataRow, int>(busline[i].Key[ii].Key, busline[i].Key[ii].Value));
                                }
                                middleline.Add(new KeyValuePair<DataRow, int>(d3, middleline[middleline.Count - 1].Value + 1));
                                busline_result2.Add(new KeyValuePair<List<KeyValuePair<DataRow, int>>, int>(middleline, middleline.Count));
                                //busline_result2.Add(new KeyValuePair<List<KeyValuePair<DataRow, int>>, int>(busline[i].Key, busline[i].Key.Count - j - 1));
                                //busline_result2[busline_result2.Count - 1].Key.RemoveRange(j + 1, busline_result2[busline_result2.Count - 1].Key.Count - j - 1);
                                //busline_result2[busline_result2.Count - 1].Key.Add(new KeyValuePair<DataRow, int>(d1, busline_result2[busline_result2.Count - 1].Key[j].Value + 1));
                                break;
                            }
                    }
                }
                if (busline_result2.Count == 0)
                {
                    for (int i = 0; i < bus_conn.Tables[0].Rows.Count; i++)
                    {
                        if ((int)bus_conn.Tables[0].Rows[i]["STOP_ID"] == eid)
                        {
                            endline = new List<KeyValuePair<DataRow, int>>();
                            if ((int)bus_conn.Tables[0].Rows[i]["SEGUP"] > 0 && (int)bus_conn.Tables[0].Rows[i]["SEGDN"] > 0)
                            {
                                if ((int)bus_conn.Tables[0].Rows[i]["SEGUP"] > (int)bus_conn.Tables[0].Rows[i]["SEGDN"])
                                {
                                    endline.Add(new KeyValuePair<DataRow, int>(bus_conn.Tables[0].Rows[i], (int)bus_conn.Tables[0].Rows[i]["SEGDN"]));
                                    busline2.Add(new KeyValuePair<List<KeyValuePair<DataRow, int>>, int>(endline, UP));
                                }
                                else
                                {
                                    endline.Add(new KeyValuePair<DataRow, int>(bus_conn.Tables[0].Rows[i], (int)bus_conn.Tables[0].Rows[i]["SEGUP"]));
                                    busline2.Add(new KeyValuePair<List<KeyValuePair<DataRow, int>>, int>(endline, DOWN));
                                }
                            }
                            else if ((int)bus_conn.Tables[0].Rows[i]["SEGUP"] > 0 && (int)bus_conn.Tables[0].Rows[i]["SEGDN"] < 0)
                            {
                                endline.Add(new KeyValuePair<DataRow, int>(bus_conn.Tables[0].Rows[i], (int)bus_conn.Tables[0].Rows[i]["SEGUP"]));
                                busline2.Add(new KeyValuePair<List<KeyValuePair<DataRow, int>>, int>(endline, UP));
                            }
                            else if ((int)bus_conn.Tables[0].Rows[i]["SEGUP"] < 0 && (int)bus_conn.Tables[0].Rows[i]["SEGDN"] > 0)
                            {
                                endline.Add(new KeyValuePair<DataRow, int>(bus_conn.Tables[0].Rows[i], (int)bus_conn.Tables[0].Rows[i]["SEGDN"]));
                                busline2.Add(new KeyValuePair<List<KeyValuePair<DataRow, int>>, int>(endline, DOWN));
                            }
                        }
                    }
                    //获取每一条线路
                    for (int i = 0; i < busline2.Count; i++)
                    {
                        if (busline2[i].Value == DOWN)
                        {
                            for (int j = 0; j < bus_conn.Tables[0].Rows.Count; j++)
                            {
                                if ((int)bus_conn.Tables[0].Rows[j]["BUSLINE"] == (int)busline2[i].Key[0].Key["BUSLINE"] && (int)bus_conn.Tables[0].Rows[j]["SEGDN"] > 0)
                                {
                                    if ((int)bus_conn.Tables[0].Rows[j]["SEGDN"] < busline2[i].Key[0].Value)
                                        busline2[i].Key.Add(new KeyValuePair<DataRow, int>(bus_conn.Tables[0].Rows[j], (int)bus_conn.Tables[0].Rows[j]["SEGDN"]));
                                }
                            }
                        }
                        else if (busline2[i].Value == UP)
                        {
                            for (int j = 0; j < bus_conn.Tables[0].Rows.Count; j++)
                            {
                                if ((int)bus_conn.Tables[0].Rows[j]["BUSLINE"] == (int)busline2[i].Key[0].Key["BUSLINE"] && (int)bus_conn.Tables[0].Rows[j]["SEGUP"] > 0)
                                {
                                    if ((int)bus_conn.Tables[0].Rows[j]["SEGUP"] < busline2[i].Key[0].Value)
                                        busline2[i].Key.Add(new KeyValuePair<DataRow, int>(bus_conn.Tables[0].Rows[j], (int)bus_conn.Tables[0].Rows[j]["SEGUP"]));
                                }
                            }
                        }
                    }
                    for (int i = 0; i < busline2.Count; i++)
                    {
                        busline2[i].Key.Sort(Compare3);
                        for (int j = 0; j < busline2[i].Key.Count; j++)
                        {
                            int a;
                            if(int.TryParse(d2["STOP_ID"].ToString(),out a))
                            if ((int)busline2[i].Key[j].Key["STOP_ID"] == (int)d2["STOP_ID"])
                            {
                                endline = new List<KeyValuePair<DataRow, int>>();
                                endline.Add(new KeyValuePair<DataRow, int>(d4, 1));
                                for (int ii = 0; ii < busline2[i].Key.Count; ii++)
                                {
                                    endline.Add(new KeyValuePair<DataRow, int>(busline2[i].Key[ii].Key,endline[endline.Count-1].Value+1));
                                }
                                endline.Sort(Compare3);
                                busline_result2.Add(new KeyValuePair<List<KeyValuePair<DataRow, int>>, int>(endline, endline.Count));
                                //busline_result2[busline_result2.Count - 1].Key.RemoveRange(0, j + 1);
                                //busline_result2[busline_result2.Count - 1].Key.Add(new KeyValuePair<DataRow, int>(d4, busline_result2[busline_result2.Count - 1].Key[0].Value - 1));
                                //busline_result2[busline_result2.Count - 1].Key.Sort(Compare3);
                                break;
                            }
                        }
                    }
                }
                if (busline_result2.Count == 0)
                {
                    for (int i = 0; i < busline.Count; i++)
                    {
                        for (int j = 0; j < busline2.Count; j++)
                        {
                            for (int m = 0; m < busline[i].Key.Count; m++)
                            {
                                for (int n = 0; n < busline2[j].Key.Count; n++)
                                {
                                    if ((int)busline[i].Key[m].Key["STOP_ID"] != sid && (int)busline2[j].Key[n].Key["STOP_ID"] != eid)
                                    {
                                        if ((int)busline[i].Key[m].Key["P_STOP_ID"] == (int)busline2[j].Key[n].Key["P_STOP_ID"])
                                        {
                                            middleline = new List<KeyValuePair<DataRow, int>>();
                                            for (int ii = 0; ii < m; ii++)
                                            {
                                                middleline.Add(new KeyValuePair<DataRow, int>(busline[i].Key[ii].Key, ii + 1));
                                            }
                                            middleline.Add(new KeyValuePair<DataRow, int>(busline[i].Key[m].Key, middleline[middleline.Count - 1].Value + 1));
                                            middleline.Add(new KeyValuePair<DataRow, int>(busline2[j].Key[n].Key, middleline[middleline.Count - 1].Value + 1));
                                            for (int ii = n + 1; ii < busline2[j].Key.Count; ii++)
                                            {
                                                middleline.Add(new KeyValuePair<DataRow, int>(busline2[j].Key[ii].Key, middleline[middleline.Count - 1].Value + 1));
                                                //middleline.Add(new KeyValuePair<DataRow, int>(busline2[j].Key[busline2[j].Key.Count - 1].Key, 4));
                                            }
                                            busline3.Add(new KeyValuePair<List<KeyValuePair<DataRow, int>>, int>(middleline, middleline.Count));
                                        }
                                    }
                                }
                            }
                        }
                    }
                    busline_result2 = busline3;
                }
               
                //一次换乘搜索完成
                if (busline_result2.Count == 0)
                {
                    List<int> R1 = new List<int>();
                    List<int> R2 = new List<int>();
                    List<int> R = new List<int>();
                    for (int i = 0; i < busline.Count; i++)
                    {
                        for (int j = 0; j < busline[i].Key.Count; j++)
                        {
                            for (int n = 0; n < bus_conn.Tables[0].Rows.Count; n++)
                            {
                                if ((int)busline[i].Key[j].Key["STOP_ID"] == (int)bus_conn.Tables[0].Rows[n]["STOP_ID"])
                                {
                                    R1.Add((int)bus_conn.Tables[0].Rows[n]["BUSLINE"]);
                                }
                            }
                        }
                    }
                    for (int i = 0; i < busline2.Count; i++)
                    {
                        for (int j = 0; j < busline2[i].Key.Count; j++)
                        {
                            for (int n = 0; n < bus_conn.Tables[0].Rows.Count; n++)
                            {
                                if ((int)busline2[i].Key[j].Key["STOP_ID"] == (int)bus_conn.Tables[0].Rows[n]["STOP_ID"])
                                {
                                    R2.Add((int)bus_conn.Tables[0].Rows[n]["BUSLINE"]);
                                }
                            }
                        }
                    }
                    R1 = R1.Distinct().ToList();
                    R2 = R2.Distinct().ToList();
                    R = R1.Intersect(R2).ToList();
                    if (R.Count != 0)
                    {
                        for (int i = 0; i < R.Count; i++)
                        {
                            middleline = new List<KeyValuePair<DataRow, int>>();
                            endline = new List<KeyValuePair<DataRow, int>>();
                            for (int j = 0; j < bus_conn.Tables[0].Rows.Count; j++)
                            {
                                if (R[i] == (int)bus_conn.Tables[0].Rows[j]["BUSLINE"])
                                {
                                    if ((int)bus_conn.Tables[0].Rows[j]["SEGUP"] > 0)
                                    {
                                        middleline.Add(new KeyValuePair<DataRow, int>(bus_conn.Tables[0].Rows[j], (int)bus_conn.Tables[0].Rows[j]["SEGUP"]));
                                    }
                                    if ((int)bus_conn.Tables[0].Rows[j]["SEGDN"] > 0)
                                    {
                                        endline.Add(new KeyValuePair<DataRow, int>(bus_conn.Tables[0].Rows[j], (int)bus_conn.Tables[0].Rows[j]["SEGDN"]));
                                    }
                                }
                            }
                            middleline.Sort(Compare3);
                            endline.Sort(Compare3);
                            busline4.Add(new KeyValuePair<List<KeyValuePair<DataRow, int>>, int>(middleline, UP));
                            busline4.Add(new KeyValuePair<List<KeyValuePair<DataRow, int>>, int>(endline, DOWN));
                        }
                        for (int i = 0; i < busline4.Count; i++)
                        {
                            for (int j = 0; j < busline4[i].Key.Count; j++)
                            {
                                for (int ii = 0; ii < busline.Count; ii++)
                                {
                                    for (int jj = 0; jj < busline[ii].Key.Count; jj++)
                                    {
                                        if ((int)busline4[i].Key[j].Key["P_STOP_ID"] == (int)busline[ii].Key[jj].Key["P_STOP_ID"])// && (int)busline[ii].Key[jj].Key["STOP_ID"] != sid)
                                        {
                                            middleline = new List<KeyValuePair<DataRow, int>>();
                                            for (int y = 0; y < jj+1; y++)
                                            {
                                                middleline.Add(new KeyValuePair<DataRow, int>(busline[ii].Key[y].Key, busline[ii].Key[y].Value));
                                            }
                                            //busline[ii].Value));
                                            //busline_result3[busline_result3.Count - 1].Key.RemoveRange(jj + 1, busline[ii].Key.Count - jj - 1);
                                            for (int n = j; n < busline4[i].Key.Count; n++)
                                            {
                                                middleline.Add(new KeyValuePair<DataRow, int>(busline4[i].Key[n].Key, middleline[middleline.Count - 1].Value + 1));
                                                //busline_result3[busline_result3.Count - 1].Key.Add(new KeyValuePair<DataRow, int>(busline4[i].Key[n].Key, busline_result3[busline_result3.Count - 1].Key[busline_result3[busline_result3.Count - 1].Key.Count - 1].Value + 1));
                                            }
                                            int a = 0;
                                            for (int m = 0; m < middleline.Count; m++)
                                            {
                                                if ((int)middleline[m].Key["STOP_ID"] == (int)d1["STOP_ID"])
                                                {
                                                    middleline.RemoveRange(m + 1, middleline.Count - m - 1);
                                                    middleline.Add(new KeyValuePair<DataRow, int>(d3, middleline[middleline.Count-1].Value + 1));
                                                    a = 1;
                                                    break;
                                                }
                                            }
                                            if(a == 1)
                                            busline_result3.Add(new KeyValuePair<List<KeyValuePair<DataRow, int>>, int>(middleline, middleline.Count));
                                            /*for (int m = 0; m < busline_result3[busline_result3.Count - 1].Key.Count; m++)
                                            {
                                                if ((int)busline_result3[busline_result3.Count - 1].Key[m].Key["STOP_ID"] == (int)d1["STOP_ID"])
                                                {
                                                    busline_result3[busline_result3.Count - 1].Key.RemoveRange(m + 1, busline_result3[busline_result3.Count - 1].Key.Count - m - 1);
                                                    busline_result3[busline_result3.Count - 1].Key.Add(new KeyValuePair<DataRow, int>(d3, busline_result3[busline_result3.Count - 1].Key[busline_result3[busline_result3.Count - 1].Key.Count - 1].Value + 1));
                                                    break;
                                                }
                                            }*/
                                        }
                                    }
                                }
                            }
                        }
                        if (busline_result3.Count == 0)
                        {
                            for (int i = 0; i < busline4.Count; i++)
                            {
                                for (int j = 0; j < busline4[i].Key.Count; j++)
                                {
                                    for (int ii = 0; ii < busline2.Count; ii++)
                                    {
                                        for (int jj = 0; jj < busline2[ii].Key.Count; jj++)
                                        {
                                            if ((int)busline4[i].Key[j].Key["P_STOP_ID"] == (int)busline2[ii].Key[jj].Key["P_STOP_ID"])
                                            {
                                                int a;
                                                if (int.TryParse(d2["STOP_ID"].ToString(), out a))
                                                if ((int)busline4[i].Key[0].Key["STOP_ID"] == (int)d2["STOP_ID"])
                                                {
                                                    middleline = new List<KeyValuePair<DataRow, int>>();
                                                    middleline.Add(new KeyValuePair<DataRow, int>(d4, 1));
                                                    for (int y = 0; y < j + 1; y++)
                                                    {
                                                        middleline.Add(new KeyValuePair<DataRow, int>(busline4[i].Key[y].Key, middleline[middleline.Count - 1].Value + 1));
                                                    }
                                                    for (int y = jj; y < busline2[ii].Key.Count; y++)
                                                    {
                                                        middleline.Add(new KeyValuePair<DataRow, int>(busline2[ii].Key[y].Key, middleline[middleline.Count - 1].Value + 1));
                                                    }
                                                    busline_result3.Add(new KeyValuePair<List<KeyValuePair<DataRow, int>>, int>(middleline, middleline.Count));
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (busline_result3.Count == 0)
                        {
                            for (int i = 0; i < busline4.Count; i++)
                            {
                                for (int j = 0; j < busline4[i].Key.Count; j++)
                                {
                                    int a;
                                    if (int.TryParse(d2["STOP_ID"].ToString(), out a))
                                    if ((int)busline4[i].Key[j].Key["STOP_ID"] == (int)d2["STOP_ID"])
                                    {
                                        for (int n = 0; n < busline4[i].Key.Count; n++)
                                        {
                                            int b;
                                            if (int.TryParse(d1["STOP_ID"].ToString(), out b))
                                            if ((int)busline4[i].Key[n].Key["STOP_ID"] == (int)d1["STOP_ID"])
                                            {
                                                if (n > j)
                                                {
                                                    middleline = new List<KeyValuePair<DataRow, int>>();
                                                    middleline.Add(new KeyValuePair<DataRow,int>(d4,1));
                                                    for (int m = j; m < n + 1; m++)
                                                    {
                                                        middleline.Add(new KeyValuePair<DataRow, int>(busline4[i].Key[m].Key, middleline[middleline.Count-1].Value+1));
                                                    }
                                                    middleline.Add(new KeyValuePair<DataRow, int>(d3, middleline[middleline.Count - 1].Value + 1));
                                                    busline_result3.Add(new KeyValuePair<List<KeyValuePair<DataRow, int>>, int>(middleline, middleline.Count));
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (busline_result3.Count == 0)
                        {
                            for (int i = 0; i < busline.Count; i++)
                            {
                                for (int j = 0; j < busline[i].Key.Count; j++)
                                {
                                    for (int m = 0; m < busline4.Count; m++)
                                    {
                                        for (int n = 0; n < busline4[m].Key.Count; n++)
                                        {
                                            if ((int)busline[i].Key[j].Key["STOP_ID"] != sid)
                                            {
                                                if ((int)busline[i].Key[j].Key["P_STOP_ID"] == (int)busline4[m].Key[n].Key["P_STOP_ID"])
                                                {
                                                    middleline = new List<KeyValuePair<DataRow, int>>();
                                                    for (int ii = 0; ii < j + 1; ii++)
                                                    {
                                                        middleline.Add(new KeyValuePair<DataRow, int>(busline[i].Key[ii].Key, busline[i].Key[ii].Value));
                                                    }
                                                    for (int ii = n; ii < busline4[m].Key.Count; ii++)
                                                    {
                                                        middleline.Add(new KeyValuePair<DataRow, int>(busline4[m].Key[ii].Key, middleline[middleline.Count - 1].Value + 1));
                                                    }
                                                    busline_result4.Add(new KeyValuePair<List<KeyValuePair<DataRow, int>>, int>(middleline, middleline.Count));
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (busline_result4.Count != 0)
                            {
                                for (int i = 0; i < busline_result4.Count; i++)
                                {
                                    for (int j = 0; j < busline_result4[i].Key.Count; j++)
                                    {
                                        for (int m = 0; m < busline2.Count; m++)
                                        {
                                            for (int n = 0; n < busline2[m].Key.Count; n++)
                                            {
                                                if ((int)busline2[m].Key[n].Key["STOP_ID"] != eid)
                                                {
                                                    if ((int)busline_result4[i].Key[j].Key["P_STOP_ID"] == (int)busline2[m].Key[n].Key["P_STOP_ID"])
                                                    {
                                                        middleline = new List<KeyValuePair<DataRow, int>>();
                                                        for (int ii = 0; ii < j + 1; ii++)
                                                        {
                                                            middleline.Add(new KeyValuePair<DataRow, int>(busline_result4[i].Key[ii].Key, busline_result4[i].Key[ii].Value));
                                                        }
                                                        for (int ii = n; ii < busline2[m].Key.Count; ii++)
                                                        {
                                                            middleline.Add(new KeyValuePair<DataRow, int>(busline2[m].Key[ii].Key, middleline[middleline.Count - 1].Value + 1));
                                                        }
                                                        busline_result3.Add(new KeyValuePair<List<KeyValuePair<DataRow, int>>, int>(middleline, middleline.Count));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                
                int o1o = 0;
            }
            int oo = 0;
            if (busline_result3.Count != 0)
            {
                busline_result = busline_result3;
            }
            else if (busline_result2.Count != 0)
            {
                busline_result = busline_result2;
            }
            else if (busline_result1.Count != 0)
            {
                busline_result = busline_result1;
            }
            return busline_result;
        }
        public void bus_trans1(int from, int to)
        {
            ///////////////////////////////////////////直达搜索
            //获取所有包含起点的线路
            for (int i = 0; i < bus_conn.Tables[0].Rows.Count; i++)
            {
                if ((int)bus_conn.Tables[0].Rows[i]["STOP_ID"] == from)
                {
                    From_rows.Add(bus_conn.Tables[0].Rows[i]);//从连通表读取每一条包含起点的记录，一条记录相当一条线路，因为任意一条线路不会经过同一个点两次
                }
            }
            //获取From_can_go，就是每一条线路从起点出发能够到达的站点，包含起点本身
            for (int i = 0; i < From_rows.Count; i++)//对于每一条线路
            {
                for (int j = 0; j < bus_conn.Tables[0].Rows.Count; j++)//遍历一次连通表
                {
                    if ((int)From_rows[i]["BUSLINE"] == (int)bus_conn.Tables[0].Rows[j]["BUSLINE"])//当位于同一线路
                    {
                        if ((int)From_rows[i]["SEGUP"] > 0)//当当前起点是上行线路的结点时
                        {
                            if ((int)bus_conn.Tables[0].Rows[j]["SEGUP"] >= (int)From_rows[i]["SEGUP"])//如果存在位于起点后的结点
                            {
                                result1.Add(new KeyValuePair<DataRow, int>(bus_conn.Tables[0].Rows[j], i));
                            }
                        }
                        if ((int)From_rows[i]["SEGDN"] > 0)//当当前起点是下行线路的结点时
                        {
                            if ((int)bus_conn.Tables[0].Rows[j]["SEGDN"] >= (int)From_rows[i]["SEGDN"])//如果存在位于起点后的结点
                            {
                                result1.Add(new KeyValuePair<DataRow, int>(bus_conn.Tables[0].Rows[j], i));
                            }
                        }
                    }
                }
            }
            //检查终点是否位于From_can_go数表上
            for (int i = 0; i < result1.Count; i++)
            {
                if ((int)result1[i].Key["STOP_ID"] == to)
                {
                    result2.Add(result1[i]);
                }
            }
            //
            ////////////////////////////////////////////////直达搜索完成
        }
        public void bus_trans2(int from, int to)
        {
            ////////////////////////////////////////////////一次换乘搜索
                try
                {
                    //获取所有包含终点的连通纪录
                    for (int i = 0; i < bus_conn.Tables[0].Rows.Count; i++)
                    {
                        if ((int)bus_conn.Tables[0].Rows[i]["STOP_ID"] == to)
                        {
                            To_rows.Add(bus_conn.Tables[0].Rows[i]);
                        }
                    }
                    //获取From_can_go
                    for (int i = 0; i < To_rows.Count; i++)//对于每一条线路
                    {
                        for (int j = 0; j < bus_conn.Tables[0].Rows.Count; j++)//遍历一次连通表
                        {
                            if ((int)To_rows[i]["BUSLINE"] == (int)bus_conn.Tables[0].Rows[j]["BUSLINE"])//当位于同一线路
                            {
                                if ((int)To_rows[i]["SEGUP"] > 0)//当当前起点是上行线路的结点时
                                {
                                    if ((int)bus_conn.Tables[0].Rows[j]["SEGUP"] != -1 && (int)bus_conn.Tables[0].Rows[j]["SEGUP"] < (int)To_rows[i]["SEGUP"])//如果存在位于起点后的结点
                                    {
                                        result3.Add(new KeyValuePair<DataRow, int>(bus_conn.Tables[0].Rows[j], i));
                                    }
                                }
                                if ((int)To_rows[i]["SEGDN"] > 0)//当当前起点是下行线路的结点时
                                {
                                    if ((int)bus_conn.Tables[0].Rows[j]["SEGDN"] != -1 && (int)bus_conn.Tables[0].Rows[j]["SEGDN"] < (int)To_rows[i]["SEGDN"])//如果存在位于起点后的结点
                                    {
                                        result3.Add(new KeyValuePair<DataRow, int>(bus_conn.Tables[0].Rows[j], i));
                                    }
                                }
                            }
                        }
                    }
                    //根据同名ID判断两个集合之间是否有交集
                    for (int i = 0; i < result1.Count; i++)
                    {
                        for (int j = 0; j < result3.Count; j++)
                        {
                            if ((int)result1[i].Key["P_STOP_ID"] == (int)result3[j].Key["P_STOP_ID"])
                            {
                                result4.Add(result1[i]);
                                result4.Add(result3[j]);
                            }
                        }
                    }
                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.ToString());
                }
            ///////////////////////////////////////////////////一次换乘搜索完成
        }
        public void bus_trans3(int from,int to)
        {
            ///////////////////////////////////////////////////二次换乘搜索
            List<int> From_passed_line = new List<int>();//对于From结点可以到达的每一个节点所经过的线路
            List<int> To_passed_line = new List<int>();//对于每一个可以到达To结点的结点经过的线路
            if (result4.Count == 0)//当一次搜索失败时
            {
                try
                {
                    //获取From_passed_line
                    for (int i = 0; i < result1.Count; i++)
                    {
                        for (int j = 0; j < bus_conn.Tables[0].Rows.Count; j++)
                        {
                            if ((int)result1[i].Key["STOP_ID"] == (int)bus_conn.Tables[0].Rows[j]["STOP_ID"])
                            {
                                From_passed_line.Add((int)bus_conn.Tables[0].Rows[j]["BUSLINE"]);
                            }
                        }
                    }
                    //获取To_passed_line
                    for (int i = 0; i < result3.Count; i++)
                    {
                        for (int j = 0; j < bus_conn.Tables[0].Rows.Count; j++)
                        {
                            if ((int)result3[i].Key["STOP_ID"] == (int)bus_conn.Tables[0].Rows[j]["STOP_ID"])
                            {
                                To_passed_line.Add((int)bus_conn.Tables[0].Rows[j]["BUSLINE"]);
                            }
                        }
                    }
                    //消除重复项
                    List<int> FPL = From_passed_line.Distinct().ToList();
                    List<int> TPL = To_passed_line.Distinct().ToList();
                    //求取交集
                    List<int> FiT = FPL.Intersect(TPL).ToList();
                    //求取结点池
                    if (FiT.Count != 0)
                    {
                        for (int i = 0; i < FiT.Count; i++)
                        {
                            for (int j = 0; j < bus_conn.Tables[0].Rows.Count; j++)
                            {
                                if (FiT[i] == (int)bus_conn.Tables[0].Rows[j]["BUSLINE"])
                                {
                                    result5.Add(new KeyValuePair<DataRow, int>(bus_conn.Tables[0].Rows[j], i));
                                }
                            }
                        }
                    }
                    ///////////////////////////////////
                    //求取结点池与From_can_go的交集，作为第一次换乘的结点
                    for (int i = 0; i < result1.Count; i++)
                    {
                        for (int j = 0; j < result5.Count; j++)
                        {
                            if ((int)result1[i].Key["P_STOP_ID"] == (int)result5[j].Key["P_STOP_ID"])//比较同名ID
                            {
                                /*if ((int)result1[i].Key["SEGUP"] != -1 && (int)result5[j].Key["SEGUP"] != -1)
                                {
                                    result4.Add(result1[i]);
                                    result4.Add(result5[j]);
                                }
                                if ((int)result1[i].Key["SEGDN"] != -1 && (int)result5[j].Key["SEGDN"] != -1)
                                {
                                    result4.Add(result1[i]);
                                    result4.Add(result5[j]);
                                }*/
                                //result4.Add(result1[i]);
                                result4.Add(result5[j]);
                            }
                        }
                    }
                    //求取结点池与Can_go_to的交集，作为第二次换乘的结点
                    for (int i = 0; i < result3.Count; i++)
                    {
                        for (int j = 0; j < result5.Count; j++)
                        {
                            if ((int)result3[i].Key["P_STOP_ID"] == (int)result5[j].Key["P_STOP_ID"])//比较同名ID
                            {
                                /*if ((int)result3[i].Key["SEGUP"] != -1 && (int)result5[j].Key["SEGUP"] != -1)
                                {
                                    result6.Add(result5[j]);
                                    result6.Add(result3[i]);
                                }
                                if ((int)result3[i].Key["SEGDN"] != -1 && (int)result5[j].Key["SEGDN"] != -1)
                                {
                                    result6.Add(result5[j]);
                                    result6.Add(result3[i]);
                                }*/
                                result6.Add(result5[j]);
                                //result6.Add(result3[i]);
                            }
                        }
                    }
                    /////////////////////////////////////////////////
                    result4 = result4.Distinct().ToList();
                    result6 = result6.Distinct().ToList();
                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.ToString());
                }
            }
            ///////////////////////////////////////////////////二次搜索完成
        }
        public List<KeyValuePair<tr1, int>> gettr1()
        {
            List<KeyValuePair<tr1, int>> tr = new List<KeyValuePair<tr1, int>>();
            if (result2.Count != 0)
            {
                for (int i = 0; i < result2.Count; i++)
                {
                    tr1 t = new tr1();
                    t.startid = (int)From_rows[result2[i].Value]["STOP_ID"];
                    t.lineid = (int)result2[i].Key["BUSLINE"];
                    t.endid = (int)result2[i].Key["STOP_ID"];
                    if ((int)result2[i].Key["SEGUP"] != -1 && (int)From_rows[result2[i].Value]["SEGUP"] != -1)
                    {
                        if ((int)result2[i].Key["SEGUP"] - (int)From_rows[result2[i].Value]["SEGUP"] > 0)
                        {
                            t.stop_count = (int)result2[i].Key["SEGUP"] - (int)From_rows[result2[i].Value]["SEGUP"];
                        }
                    }
                    else if ((int)result2[i].Key["SEGDN"] != -1 && (int)From_rows[result2[i].Value]["SEGDN"] != -1)
                    {
                        if ((int)result2[i].Key["SEGDN"] - (int)From_rows[result2[i].Value]["SEGDN"] > 0)
                        {
                            t.stop_count = (int)result2[i].Key["SEGDN"] - (int)From_rows[result2[i].Value]["SEGDN"];
                        }
                    }
                    tr.Add(new KeyValuePair<tr1,int>(t,t.stop_count));
                }
            }
            return tr;
        }
        public List<KeyValuePair<tr2, int>> gettr2()
        {
            List<KeyValuePair<tr2, int>> tr = new List<KeyValuePair<tr2, int>>();
            if (result4.Count != 0)
            {
                for (int i = 0; i < result4.Count; i += 2)
                {
                    tr2 t = new tr2();
                    t.startid1 = (int)From_rows[result4[i].Value]["STOP_ID"];
                    t.lineid1 = (int)result4[i].Key["BUSLINE"];
                    t.endid1 = (int)result4[i].Key["STOP_ID"];
                    t.startid2 = (int)result4[i].Key["STOP_ID"];
                    t.lineid2 = (int)result4[i + 1].Key["BUSLINE"];
                    t.endid2 = (int)result4[i + 1].Key["STOP_ID"];
                    if ((int)result4[i].Key["SEGUP"] != -1 && (int)From_rows[result4[i].Value]["SEGUP"] != -1)
                    {
                        if ((int)result4[i].Key["SEGUP"] - (int)From_rows[result4[i].Value]["SEGUP"] > 0)
                        {
                            t.stop_count = (int)result4[i].Key["SEGUP"] - (int)From_rows[result4[i].Value]["SEGUP"];
                        }
                    }
                    else if ((int)result4[i].Key["SEGDN"] != -1 && (int)From_rows[result4[i].Value]["SEGDN"] != -1)
                    {
                        if ((int)result4[i].Key["SEGDN"] - (int)From_rows[result4[i].Value]["SEGDN"] > 0)
                        {
                            t.stop_count = (int)result4[i].Key["SEGDN"] - (int)From_rows[result4[i].Value]["SEGDN"];
                        }
                    }
                    if ((int)result4[i].Key["SEGUP"] != -1 && (int)result4[i+1].Key["SEGUP"] != -1)
                    {
                        if ((int)result4[i+1].Key["SEGUP"] - (int)result4[i].Key["SEGUP"] > 0)
                        {
                            t.stop_count += (int)result4[i+1].Key["SEGUP"] - (int)result4[i].Key["SEGUP"];
                        }
                    }
                    else if ((int)result4[i+1].Key["SEGDN"] != -1 && (int)result4[i].Key["SEGDN"] != -1)
                    {
                        if ((int)result4[i+1].Key["SEGDN"] - (int)result4[i].Key["SEGDN"] > 0)
                        {
                            t.stop_count += (int)result4[i+1].Key["SEGDN"] - (int)result4[i].Key["SEGDN"];
                        }
                    }
                    tr.Add(new KeyValuePair<tr2, int>(t, t.stop_count));
                }
            }
            return tr;
        }
        public List<KeyValuePair<tr3, int>> gettr3()
        {
            List<KeyValuePair<tr3, int>> tr3 = new List<KeyValuePair<tr3, int>>();
            if (result7.Count != 0)
            {
                for (int i = 0; i < result7.Count; i+=4)
                {
                    tr3 t = new tr3();
                    t.startid1 = (int)result7[i].Key["STOP_ID"];
                    t.lineid1 = (int)result7[i].Key["BUSLINE"];
                    t.endid1 = (int)result7[i+1].Key["STOP_ID"];
                    t.startid2 = (int)result7[i+1].Key["STOP_ID"];
                    t.lineid2 = (int)result7[i+2].Key["BUSLINE"];
                    t.endid2 = (int)result7[i+2].Key["STOP_ID"];
                    t.startid3 = (int)result7[i+2].Key["STOP_ID"];
                    t.lineid3 = (int)result7[i+3].Key["BUSLINE"];
                    t.endid3 = (int)result7[i+3].Key["STOP_ID"];
                    if((int)result7[i].Key["SEGUP"] != -1 && (int)result7[i+1].Key["SEGUP"] != -1)
                    {
                        if ((int)result7[i + 1].Key["SEGUP"] - (int)result7[i].Key["SEGUP"] > 0)
                        {
                            t.stop_count += (int)result7[i + 1].Key["SEGUP"] - (int)result7[i].Key["SEGUP"];
                        }
                        else
                        {
                            t.stop_count += 99;
                        }
                    }
                    else if((int)result7[i].Key["SEGDN"] != -1 && (int)result7[i+1].Key["SEGDN"] != -1)
                    {
                        if((int)result7[i+1].Key["SEGDN"] - (int)result7[i].Key["SEGDN"] >0)
                        {
                        t.stop_count += (int)result7[i+1].Key["SEGDN"] - (int)result7[i].Key["SEGDN"];
                        }
                        else
                        {
                            t.stop_count += 99;
                        }
                    }
                    //
                     if((int)result7[i+1].Key["SEGUP"] != -1 && (int)result7[i+2].Key["SEGUP"] != -1)
                    {
                        if((int)result7[i+2].Key["SEGUP"] - (int)result7[i+1].Key["SEGUP"] >0)
                        {
                        t.stop_count += (int)result7[i+2].Key["SEGUP"] - (int)result7[i+1].Key["SEGUP"];
                        }
                        else
                        {
                            t.stop_count += 99;
                        }
                    }
                    else if((int)result7[i+1].Key["SEGDN"] != -1 && (int)result7[i+2].Key["SEGDN"] != -1)
                    {
                        if((int)result7[i+2].Key["SEGDN"] - (int)result7[i+1].Key["SEGDN"] >0)
                        {
                        t.stop_count += (int)result7[i+2].Key["SEGDN"] - (int)result7[i+1].Key["SEGDN"];
                        }
                        else
                        {
                            t.stop_count += 99;
                        }
                    }
                    //
                     if((int)result7[i+2].Key["SEGUP"] != -1 && (int)result7[i+3].Key["SEGUP"] != -1)
                    {
                        if((int)result7[i+3].Key["SEGUP"] - (int)result7[i+2].Key["SEGUP"] >0)
                        {
                        t.stop_count += (int)result7[i+3].Key["SEGUP"] - (int)result7[i+2].Key["SEGUP"];
                        }
                        else
                        {
                            t.stop_count += 99;
                        }
                    }
                    else if((int)result7[i+3].Key["SEGDN"] != -1 && (int)result7[i+2].Key["SEGDN"] != -1)
                    {
                        if((int)result7[i+3].Key["SEGDN"] - (int)result7[i+2].Key["SEGDN"] >0)
                        {
                        t.stop_count += (int)result7[i+3].Key["SEGDN"] - (int)result7[i+2].Key["SEGDN"];
                        }
                        else
                        {
                            t.stop_count += 99;
                        }
                    }
                    tr3.Add(new KeyValuePair<tr3,int>(t,t.stop_count));
                }
            }
            return tr3;

        }
        public void bus_trans(int sid, int eid)
        {
            result1.Clear();
            result2.Clear();
            result3.Clear();
            result4.Clear();
            result5.Clear();
            result6.Clear();
            result7.Clear();
            From_rows.Clear();
            To_rows.Clear();
            try
            {
                ///////////////////////////////////////////设置起点终点
                int from = sid;
                int to = eid;
                int from_unq = 0;
                int to_unq = 0;
                ///////////////////////////////////////////获取同名ID
                for (int i = 0; i < bus_vertrx.Tables[0].Rows.Count; i++)
                {
                    if ((int)bus_vertrx.Tables[0].Rows[i]["ID"] == from)
                    {
                        from_unq = (int)bus_vertrx.Tables[0].Rows[i]["BUS_ID"];
                    }
                    else if ((int)bus_vertrx.Tables[0].Rows[i]["ID"] == to)
                    {
                        to_unq = (int)bus_vertrx.Tables[0].Rows[i]["BUS_ID"];
                    }
                }
                //////////////////////////////////////////////
                bus_trans1(from, to);
                List<KeyValuePair<tr1, int>> tr = gettr1();
                string str2 = "";
                if (result2.Count != 0)
                {
                    for (int i = 0; i < result2.Count; i++)
                    {
                        str2 += "出发:\n";
                        for (int j = 0; j < bus_conn.Tables[0].Columns.Count; j++)
                        {
                            if (j == bus_conn.Tables[0].Columns.Count - 1)
                            {
                                str2 += From_rows[result2[i].Value][j] + "   \n";
                            }
                            else
                            {
                                str2 += From_rows[result2[i].Value][j] + "   ";
                            }
                        }
                        for (int j = 0; j < bus_conn.Tables[0].Columns.Count; j++)
                        {
                            if (j == bus_conn.Tables[0].Columns.Count - 1)
                            {
                                str2 += result2[i].Key[j] + "   \n";
                            }
                            else
                            {
                                str2 += result2[i].Key[j] + "   ";
                            }
                        }
                    }
                    MessageBox.Show(str2);
                }
                if (result2.Count == 0)
                {
                    bus_trans2(from, to);
                    List<KeyValuePair<tr2, int>> tr2 = gettr2();
                    if (result4.Count != 0)
                    {
                        str2 = "";
                        for (int i = 0; i < result4.Count; i += 2)
                        {
                            str2 += "-------------------------------\n";
                            for (int j = 0; j < bus_conn.Tables[0].Columns.Count; j++)
                            {
                                if (j == bus_conn.Tables[0].Columns.Count - 1)
                                {
                                    str2 += From_rows[result4[i].Value][j] + "    \n";
                                }
                                else
                                {
                                    str2 += From_rows[result4[i].Value][j] + "    ";
                                }
                            }
                            for (int j = 0; j < bus_conn.Tables[0].Columns.Count; j++)
                            {
                                if (j == bus_conn.Tables[0].Columns.Count - 1)
                                {
                                    str2 += result4[i].Key[j] + "    \n";
                                }
                                else
                                {
                                    str2 += result4[i].Key[j] + "    ";
                                }
                            }
                            for (int j = 0; j < bus_conn.Tables[0].Columns.Count; j++)
                            {
                                if (j == bus_conn.Tables[0].Columns.Count - 1)
                                {
                                    str2 += result4[i + 1].Key[j] + "    \n";
                                }
                                else
                                {
                                    str2 += result4[i + 1].Key[j] + "    ";
                                }
                            }
                            for (int j = 0; j < bus_conn.Tables[0].Columns.Count; j++)
                            {
                                if (j == bus_conn.Tables[0].Columns.Count - 1)
                                {
                                    str2 += To_rows[result4[i + 1].Value][j] + "    \n";
                                }
                                else
                                {
                                    str2 += To_rows[result4[i + 1].Value][j] + "    ";
                                }
                            }
                        }
                        MessageBox.Show(str2);
                    }
                }
                if (result4.Count == 0)
                {
                    bus_trans3(from, to);
                    if (result4.Count != 0 && result6.Count != 0)
                    {
                        for(int i = 0;i<result4.Count;i++)
                        {
                            for(int j = 0;j<result6.Count;j++)
                            {
                                result7.Add(new KeyValuePair<DataRow, int>(From_rows[result4[i].Value], 0));
                                result7.Add(new KeyValuePair<DataRow, int>(result4[i].Key, result4[i].Value));
                                result7.Add(new KeyValuePair<DataRow,int>(result6[j].Key,result6[j].Value));
                                result7.Add(new KeyValuePair<DataRow,int>(To_rows[result6[j].Value],0));
                            }
                        }
                        List<KeyValuePair<tr3, int>> tr3 = gettr3();
                        string str4 = "一\n";
                        for (int i = 0; i < result4.Count; i += 1)
                        {
                            str4 += "-----------------------------------------------------\n";
                            for (int j = 0; j < bus_conn.Tables[0].Columns.Count; j++)
                            {
                                if (j == bus_conn.Tables[0].Columns.Count - 1)
                                {
                                    str4 += From_rows[result4[i].Value][j] + "    \n";
                                }
                                else
                                {
                                    str4 += From_rows[result4[i].Value][j] + "    ";
                                }
                            }
                            for (int j = 0; j < bus_conn.Tables[0].Columns.Count; j++)
                            {
                                if (j == bus_conn.Tables[0].Columns.Count - 1)
                                {
                                    str4 += result4[i].Key[j] + "   \n";
                                }
                                else
                                {
                                    str4 += result4[i].Key[j] + "   ";
                                }
                            }
                            /*for (int j = 0; j < bus_conn.Tables[0].Columns.Count; j++)
                            {
                                if (j == bus_conn.Tables[0].Columns.Count - 1)
                                {
                                    str4 += result4[i + 1].Key[j] + "   \n";
                                }
                                else
                                {
                                    str4 += result4[i + 1].Key[j] + "   ";
                                }
                            }*/
                        }
                        str4 += "二\n";
                        for (int i = 0; i < result6.Count; i += 1)
                        {
                            str4 += "-----------------------------------------------------\n";
                            for (int j = 0; j < bus_conn.Tables[0].Columns.Count; j++)
                            {
                                if (j == bus_conn.Tables[0].Columns.Count - 1)
                                {
                                    str4 += result6[i].Key[j] + "   \n";
                                }
                                else
                                {
                                    str4 += result6[i].Key[j] + "   ";
                                }
                            }
                            /*for (int j = 0; j < bus_conn.Tables[0].Columns.Count; j++)
                            {
                                if (j == bus_conn.Tables[0].Columns.Count - 1)
                                {
                                    str4 += result6[i + 1].Key[j] + "   \n";
                                }
                                else
                                {
                                    str4 += result6[i + 1].Key[j] + "   ";
                                }
                            }*/
                            for (int j = 0; j < bus_conn.Tables[0].Columns.Count; j++)
                            {
                                if (j == bus_conn.Tables[0].Columns.Count - 1)
                                {
                                    str4 += To_rows[result6[i].Value][j] + "    \n";
                                }
                                else
                                {
                                    str4 += To_rows[result6[i].Value][j] + "    ";
                                }
                            }
                        }
                        MessageBox.Show(str4);
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
        public List<List<int>> show_path_bus(List<List<linedisplay>> ldc)
        {
            List<List<int>> r1 = new List<List<int>>();
            for (int i = 0; i < ldc.Count; i++)
            {
                List<int> t = new List<int>();
                for (int j = 0; j < ldc[i].Count; j++)
                {
                    if (ldc[i][j].blid != 0 && ldc[i][j].blid != -1)
                    {
                        for (int m = 0; m < bus_line.Tables[0].Rows.Count; m++)
                        {
                            if ((int)bus_line.Tables[0].Rows[m]["LINE_ID"] == ldc[i][j].blid)
                            {
                                t.Add((int)bus_line.Tables[0].Rows[m]["SEGID"]);
                            }
                        }
                    }
                }
                t = t.Distinct().ToList();
                r1.Add(t);
            }
            return r1;
        }

        private void Dijask_Load(object sender, EventArgs e)
        {
            m1 = (MainForm)this.Owner;
            try
            {
                bus_conn.Clear();
                bus_vertrx.Clear();
                dt.Clear();
                dss.Clear();
                bus_line.Clear();
                if (m1.MyConnection.State != ConnectionState.Open)
                {
                    Database db = new Database();
                    db.ShowDialog(this.Owner);
                    m1.MyConnection = new OdbcConnection(m1.MyConString);
                    m1.MyConnection.Open();
                    conn();
                    sql3 = "Select ID,POLYLINE from test_polyline";
                    m1.ODA = new OdbcDataAdapter(sql3, m1.MyConnection);
                    m1.ODA.Fill(dss);
                    sql3 = "Select * from bus_line";
                    m1.ODA = new OdbcDataAdapter(sql3, m1.MyConnection);
                    m1.ODA.Fill(bus_line);
                    string sql = "Select ID,NOTE,X,Y From test_vertex";
                    m1.ODA = new OdbcDataAdapter(sql, m1.MyConnection);
                    m1.ODABuilder = new OdbcCommandBuilder(m1.ODA);
                    m1.ODA.Fill(dt);
                    depth = (int)Math.Round(dt.Rows.Count - dt.Rows.Count * 0.1);
                    //BUS Transit
                    string db_sql_syntax = "Select * From bus_conn";
                    OdbcConnection db_connection = new OdbcConnection(m1.MyConString);
                    db_connection.Open();
                    OdbcDataAdapter ODA = new OdbcDataAdapter(db_sql_syntax, db_connection);
                    ODA.Fill(bus_conn);
                    db_sql_syntax = "Select ID,BUS_ID From test_vertex Where NOTE = 4;";
                    ODA = new OdbcDataAdapter(db_sql_syntax, db_connection);
                    ODA.Fill(bus_vertrx);
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

        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            System.Drawing.Point pt = new System.Drawing.Point();
            pt.X = e.X + this.Left + 150;
            pt.Y = e.Y + this.Top + this.listBox1.Top + 20;
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                contextMenuStrip1.Show(pt);
            }

        }

        private void fromHereToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                textBox2.Text = listBox1.SelectedItem.ToString();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }
        }

        private void toHereToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                textBox3.Text = listBox1.SelectedItem.ToString();
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
                
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                try
                {
                    if (rbvehi.Checked == true)
                    {
                        string SQLString = "Select NAME FROM test_vertex WHERE NAME LIKE '%" + textBox1.Text + "%' AND NOTE = '1' OR (NAME LIKE'%" + textBox1.Text + "%' AND NOTE = '2') " + "OR (NAME LIKE'%" + textBox1.Text + "%' AND NOTE = '4');";
                        DataSet myDS = new DataSet();
                        m1.ODA = new OdbcDataAdapter(SQLString, m1.MyConnection);
                        m1.ODABuilder = new OdbcCommandBuilder(m1.ODA);
                        m1.ODA.Fill(myDS);
                        int i = myDS.Tables[0].Rows.Count;
                        listBox1.Items.Clear();
                        for(int q = 0;q<i;q++)
                        {
                            listBox1.Items.Add(myDS.Tables[0].Rows[q][0].ToString());
                        }
                    }
                    else if (rbrw.Checked == true)
                    {
                        string SQLString = "Select NAME FROM test_vertex WHERE NAME LIKE '%" + textBox1.Text + "%'";
                        DataSet myDS = new DataSet();
                        m1.ODA = new OdbcDataAdapter(SQLString, m1.MyConnection);
                        m1.ODABuilder = new OdbcCommandBuilder(m1.ODA);
                        m1.ODA.Fill(myDS);
                        int i = myDS.Tables[0].Rows.Count;
                        listBox1.Items.Clear();
                        for (int q = 0; q < i; q++)
                        {
                            listBox1.Items.Add(myDS.Tables[0].Rows[q][0].ToString());
                        }
                    }
                    else if (rbb.Checked == true)
                    {
                        string SQLString = "Select NAME FROM test_vertex WHERE NAME LIKE '%" + textBox1.Text + "%' AND NOTE = '1' OR (NAME LIKE'%" + textBox1.Text + "%' AND NOTE = '2');";
                        DataSet myDS = new DataSet();
                        m1.ODA = new OdbcDataAdapter(SQLString, m1.MyConnection);
                        m1.ODABuilder = new OdbcCommandBuilder(m1.ODA);
                        m1.ODA.Fill(myDS);
                        int i = myDS.Tables[0].Rows.Count;
                        listBox1.Items.Clear();
                        for (int q = 0; q < i; q++)
                        {
                            listBox1.Items.Add(myDS.Tables[0].Rows[q][0].ToString());
                        }
                    }
                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.ToString());
                }
            }
        }
        
        int curfrom = 0;
        private void button3_Click(object sender, EventArgs e)
        {
            m1.axMap1.RemoveAllLayers();
            try
            {
                int from;
                int to;
                if (int.TryParse(textBox2.Text, out from) && int.TryParse(textBox3.Text, out to))
                {
                }
                else
                {
                    from = text2num(textBox2.Text);
                    to = text2num(textBox3.Text);
                    if (from == to)
                    {
                        MessageBox.Show("--");
                        return;
                    }
                }
                
                if (textBox2.Text != "" && textBox3.Text != "")
                {
                    if (rbvehi.Checked == true)
                    {
                        if (get_Note(from) == 1 )
                        {
                            if (get_Note(to) == 1)
                            {
                                vls.Clear();
                                vls = Dijask3(from, to);
                                if (vls.Count == 0)
                                    return;
                                show_path(queue_path(vls, to));
                            }
                            else if (get_Note(to) == 2 || get_Note(to) == 4)
                            {
                                vls.Clear();
                                vls2.Clear();
                                vls2 = Dijask2_1(to, 0,depth);
                                if (vls2.Count == 0)
                                    return;
                                //int p1 = pointpart(to,vls2,vls);
                                int p1 = pointpart2(to, vls2);
                                vls = Dijask3(from, p1);
                                if (vls.Count == 0)
                                    return;
                                vls2 = Dijask2_1(p1, to,depth);
                                List<string> str = new List<string>();
                                str = queue_path(vls, p1);
                                str.AddRange(queue_path(vls2, to));
                                show_path(str);
                            }
                        }
                        else if (get_Note(from) == 2 || get_Note(from) == 4)
                        {
                            if(get_Note(to) == 1)
                            {
                                vls.Clear();
                                vls2.Clear();
                                vls2 = Dijask2_1(from, 0,depth);
                                //int p1 = pointpart(from, vls2, vls);
                                if (vls2.Count == 0)
                                    return;
                                int p1 = pointpart2(from, vls2);
                                vls = Dijask3(p1, to);
                                if (vls.Count == 0)
                                    return;
                                List<string> str = queue_path(vls, to);
                                str.AddRange(queue_path(vls2, p1));
                                show_path(str);
                            }
                            else if (get_Note(to) == 2 || get_Note(to) == 4)
                            {
                                if (aaa(from, to) != true)
                                {
                                    vls.Clear();
                                    vls2.Clear();
                                    vls3.Clear();
                                    vls2 = Dijask2_1(from, 0,depth);
                                    vls3 = Dijask2_1(to, 0,depth);
                                    if (vls3.Count == 0)
                                        return;
                                    if (vls2.Count == 0)
                                        return;
                                    int p1 = pointpart2(from, vls2);
                                    int p2 = pointpart2(to, vls3);
                                    vls = Dijask3(p1, p2);
                                    if (vls.Count == 0)
                                        return;
                                    List<string> str = queue_path(vls2, p1);
                                    str.AddRange(queue_path(vls3, p2));
                                    str.AddRange(queue_path(vls, p2));
                                    show_path(str);

                                }
                                else
                                {
                                    show_path(queue_path(Dijask2(from,to),to));
                                }

                            }
                        }

                    }
                    else if (rbrw.Checked == true)
                    {
                        if (curfrom != from)
                        {
                            vls = Dijask1(from, to);
                            show_path(queue_path(vls,to));
                            curfrom = from;
                        }
                        else
                        {
                            m1.axMap1.RemoveLayer(layerhandle);
                            show_path(queue_path(vls,to));
                        }
                    }
                    else if (rbb.Checked == true)
                    {
                        bline.Clear();
                        m1.checkedListBox1.Items.Clear();
                        //bus_trans(from,to);
                        List<string> line = new List<string>();
                        //List<string> sline = new List<string>();
                        sline.Clear();
                        string str2 = "";
                        List<KeyValuePair<List<KeyValuePair<DataRow, int>>, int>> result = bustran(from, to);
                        result.Sort(Compare4);
                        if (result.Count != 0)
                        {
                            for (int i = 0; i < result.Count; i++)
                            {
                                List<linedisplay> lb = new List<linedisplay>();
                                string str = "";
                                for (int ii = 1; ii < result[i].Key.Count; ii++)
                                {
                                    linedisplay ld = new linedisplay();
                                    if (ii != result[i].Key.Count - 1)
                                    {
                                        if ((int)result[i].Key[ii - 1].Key["P_STOP_ID"] == (int)result[i].Key[ii].Key["P_STOP_ID"])
                                        {
                                            if ((int)result[i].Key[ii - 1].Key["STOP_ID"] != (int)result[i].Key[ii].Key["STOP_ID"])
                                            {
                                                str += "从" + result[i].Key[ii - 1].Key["STOP_NAME"] + "步行至" + result[i].Key[ii].Key["STOP_NAME"] + "乘坐 [" + result[i].Key[ii].Key["BUS_NAME"] + "]\r\n";
                                                ld.fp = (int)result[i].Key[ii - 1].Key["STOP_ID"];
                                                ld.bp = (int)result[i].Key[ii].Key["STOP_ID"];
                                                ld.blid = -1;//需要步行换乘
                                                line.Add(result[i].Key[ii].Key["BUS_NAME"].ToString());
                                            }
                                            else
                                            {
                                                str += "在" + result[i].Key[ii - 1].Key["STOP_ID"] + "换乘坐 [" + result[i].Key[ii].Key["BUS_NAME"] + "]\r\n";
                                                ld.fp = (int)result[i].Key[ii - 1].Key["STOP_ID"];
                                                ld.bp = (int)result[i].Key[ii].Key["STOP_ID"];
                                                ld.blid = 0;//直接当站换乘
                                                line.Add(result[i].Key[ii].Key["BUS_NAME"].ToString());
                                            }
                                        }
                                        else
                                        {
                                            line.Add(result[i].Key[ii-1].Key["BUS_NAME"].ToString());
                                            str += "本站：" + result[i].Key[ii - 1].Key["STOP_NAME"] + " [搭乘" + result[i].Key[ii - 1].Key["BUS_NAME"] + "] 下一站：" + result[i].Key[ii].Key["STOP_NAME"] + "\r\n";
                                            ld.fp = (int)result[i].Key[ii - 1].Key["STOP_ID"];
                                            ld.bp = (int)result[i].Key[ii].Key["STOP_ID"];
                                            if ((int)result[i].Key[ii - 1].Key["LINEUP"] != -1)
                                                ld.blid = (int)result[i].Key[ii - 1].Key["LINEUP"];
                                            else if ((int)result[i].Key[ii - 1].Key["LINEDN"] != -1)
                                                ld.blid = (int)result[i].Key[ii - 1].Key["LINEDN"];
                                        }
                                    }
                                    else
                                    {
                                        if ((int)result[i].Key[ii - 1].Key["P_STOP_ID"] == (int)result[i].Key[ii].Key["P_STOP_ID"])
                                        {
                                            if ((int)result[i].Key[ii - 1].Key["STOP_ID"] != (int)result[i].Key[ii].Key["STOP_ID"])
                                            {
                                                str += "从" + result[i].Key[ii - 1].Key["STOP_NAME"] + "步行到达" + result[i].Key[ii].Key["STOP_NAME"] + "\r\n";
                                                ld.fp = (int)result[i].Key[ii - 1].Key["STOP_ID"];
                                                ld.bp = (int)result[i].Key[ii].Key["STOP_ID"];
                                                ld.blid = -1;//需要步行换乘
                                            }
                                            else
                                            {
                                                str += "到达" + result[i].Key[ii - 1].Key["STOP_ID"];
                                                ld.fp = (int)result[i].Key[ii - 1].Key["STOP_ID"];
                                                ld.bp = (int)result[i].Key[ii].Key["STOP_ID"];
                                                ld.blid = 0;//直接当站换乘
                                            }
                                        }
                                        else
                                        {
                                            line.Add(result[i].Key[ii - 1].Key["BUS_NAME"].ToString());
                                            str += "本站：" + result[i].Key[ii - 1].Key["STOP_NAME"] + " [搭乘" + result[i].Key[ii - 1].Key["BUS_NAME"] + "] 终点：" + result[i].Key[ii].Key["STOP_NAME"] + "\r\n";
                                            ld.fp = (int)result[i].Key[ii - 1].Key["STOP_ID"];
                                            ld.bp = (int)result[i].Key[ii].Key["STOP_ID"];
                                            if ((int)result[i].Key[ii - 1].Key["LINEUP"] != -1)
                                                ld.blid = (int)result[i].Key[ii - 1].Key["LINEUP"];
                                            else if ((int)result[i].Key[ii - 1].Key["LINEDN"] != -1)
                                                ld.blid = (int)result[i].Key[ii - 1].Key["LINEDN"];
                                        }
                                    }
                                    lb.Add(ld);
                                }
                                bline.Add(lb);
                                line = line.Distinct().ToList();
                                str2 = "";
                                for (int qq = 0; qq < line.Count; qq++)
                                    if (qq != line.Count - 1)
                                        str2 += line[qq] + "-换-";
                                    else
                                        str2 += line[qq];
                                str2 += " #站数："+ result[i].Value;
                                line.Clear();
                                m1.checkedListBox1.Items.Add(str2);
                                sline.Add(str);
                            }
                        }
                        //MessageBox.Show(str);

                    }
                }
                else
                {
                }
                show_pt(from, to);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.ToString());
            }
        }
    }
}
