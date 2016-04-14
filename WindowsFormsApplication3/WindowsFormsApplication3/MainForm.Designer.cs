namespace WindowsFormsApplication3
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStrip_mainform = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel_db = new System.Windows.Forms.ToolStripLabel();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.axMap1 = new AxMapWinGIS.AxMap();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.draw_indicator = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.legend = new System.Windows.Forms.TabPage();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.toolStrip_mainform.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axMap1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.legend.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip_mainform
            // 
            this.toolStrip_mainform.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.toolStripLabel_db,
            this.toolStripButton1,
            this.toolStripLabel2,
            this.toolStripLabel3,
            this.toolStripButton2});
            this.toolStrip_mainform.Location = new System.Drawing.Point(0, 0);
            this.toolStrip_mainform.Name = "toolStrip_mainform";
            this.toolStrip_mainform.Size = new System.Drawing.Size(739, 25);
            this.toolStrip_mainform.TabIndex = 0;
            this.toolStrip_mainform.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(64, 22);
            this.toolStripLabel1.Text = "OPENMAP";
            this.toolStripLabel1.Click += new System.EventHandler(this.toolStripLabel1_Click);
            // 
            // toolStripLabel_db
            // 
            this.toolStripLabel_db.Name = "toolStripLabel_db";
            this.toolStripLabel_db.Size = new System.Drawing.Size(63, 22);
            this.toolStripLabel_db.Text = "DATABASE";
            this.toolStripLabel_db.Click += new System.EventHandler(this.toolStripLabel_db_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(27, 22);
            this.toolStripLabel2.Text = "Pan";
            this.toolStripLabel2.Click += new System.EventHandler(this.toolStripLabel2_Click);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(42, 22);
            this.toolStripLabel3.Text = "Cursor";
            this.toolStripLabel3.Click += new System.EventHandler(this.toolStripLabel3_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "toolStripButton2";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // axMap1
            // 
            this.axMap1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axMap1.Enabled = true;
            this.axMap1.Location = new System.Drawing.Point(0, 0);
            this.axMap1.Name = "axMap1";
            this.axMap1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axMap1.OcxState")));
            this.axMap1.Size = new System.Drawing.Size(627, 394);
            this.axMap1.TabIndex = 1;
            this.axMap1.MouseDownEvent += new AxMapWinGIS._DMapEvents_MouseDownEventHandler(this.axMap1_MouseDownEvent);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.draw_indicator});
            this.statusStrip1.Location = new System.Drawing.Point(0, 419);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(739, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // draw_indicator
            // 
            this.draw_indicator.Name = "draw_indicator";
            this.draw_indicator.Size = new System.Drawing.Size(85, 17);
            this.draw_indicator.Text = "draw_indicator";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.axMap1);
            this.splitContainer1.Size = new System.Drawing.Size(739, 394);
            this.splitContainer1.SplitterDistance = 108;
            this.splitContainer1.TabIndex = 3;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.legend);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(108, 394);
            this.tabControl1.TabIndex = 0;
            // 
            // legend
            // 
            this.legend.Controls.Add(this.checkedListBox1);
            this.legend.Location = new System.Drawing.Point(4, 22);
            this.legend.Name = "legend";
            this.legend.Padding = new System.Windows.Forms.Padding(3);
            this.legend.Size = new System.Drawing.Size(100, 368);
            this.legend.TabIndex = 1;
            this.legend.Text = "tabPage2";
            this.legend.UseVisualStyleBackColor = true;
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(3, 3);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(94, 362);
            this.checkedListBox1.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(739, 441);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip_mainform);
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.toolStrip_mainform.ResumeLayout(false);
            this.toolStrip_mainform.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axMap1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.legend.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip_mainform;
        private System.Windows.Forms.ToolStripLabel toolStripLabel_db;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        public AxMapWinGIS.AxMap axMap1;
        private System.Windows.Forms.ToolStripStatusLabel draw_indicator;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage legend;
        private System.Windows.Forms.CheckedListBox checkedListBox1;

    }
}

