namespace WindowsFormsApplication3
{
    partial class AddTables
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddTables));
            this.script_box = new System.Windows.Forms.TextBox();
            this.fieldtype_cbx = new System.Windows.Forms.ComboBox();
            this.tablename = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.utfsupp_cbx = new System.Windows.Forms.ComboBox();
            this.fieldlength = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.fieldname = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // script_box
            // 
            this.script_box.Dock = System.Windows.Forms.DockStyle.Top;
            this.script_box.Location = new System.Drawing.Point(0, 0);
            this.script_box.Multiline = true;
            this.script_box.Name = "script_box";
            this.script_box.Size = new System.Drawing.Size(357, 106);
            this.script_box.TabIndex = 0;
            // 
            // fieldtype_cbx
            // 
            this.fieldtype_cbx.FormattingEnabled = true;
            this.fieldtype_cbx.Items.AddRange(new object[] {
            "INT",
            "FLOAT",
            "DOUBLE",
            "CHAR",
            "VARCHAR",
            "DATE",
            "BOOLEAN"});
            this.fieldtype_cbx.Location = new System.Drawing.Point(75, 34);
            this.fieldtype_cbx.Name = "fieldtype_cbx";
            this.fieldtype_cbx.Size = new System.Drawing.Size(85, 20);
            this.fieldtype_cbx.TabIndex = 1;
            this.fieldtype_cbx.TextChanged += new System.EventHandler(this.fieldtype_cbx_TextChanged);
            // 
            // tablename
            // 
            this.tablename.Location = new System.Drawing.Point(12, 129);
            this.tablename.Name = "tablename";
            this.tablename.Size = new System.Drawing.Size(72, 21);
            this.tablename.TabIndex = 3;
            this.tablename.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 114);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "表名";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "字段名";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.utfsupp_cbx);
            this.groupBox1.Controls.Add(this.fieldlength);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.fieldname);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.fieldtype_cbx);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(90, 112);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(265, 110);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // button3
            // 
            this.button3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button3.BackgroundImage")));
            this.button3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button3.Image = ((System.Drawing.Image)(resources.GetObject("button3.Image")));
            this.button3.Location = new System.Drawing.Point(145, 73);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(23, 23);
            this.button3.TabIndex = 13;
            this.button3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 58);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 12);
            this.label5.TabIndex = 12;
            this.label5.Text = "设置UTF-8";
            // 
            // utfsupp_cbx
            // 
            this.utfsupp_cbx.FormattingEnabled = true;
            this.utfsupp_cbx.Items.AddRange(new object[] {
            "YES"});
            this.utfsupp_cbx.Location = new System.Drawing.Point(6, 73);
            this.utfsupp_cbx.Name = "utfsupp_cbx";
            this.utfsupp_cbx.Size = new System.Drawing.Size(85, 20);
            this.utfsupp_cbx.TabIndex = 11;
            // 
            // fieldlength
            // 
            this.fieldlength.Location = new System.Drawing.Point(166, 33);
            this.fieldlength.Name = "fieldlength";
            this.fieldlength.Size = new System.Drawing.Size(83, 21);
            this.fieldlength.TabIndex = 10;
            this.fieldlength.Text = "VARCHAR/CHAR";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(164, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "字段长度";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(75, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "字段类型";
            // 
            // fieldname
            // 
            this.fieldname.Location = new System.Drawing.Point(6, 34);
            this.fieldname.Name = "fieldname";
            this.fieldname.Size = new System.Drawing.Size(63, 21);
            this.fieldname.TabIndex = 7;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(174, 73);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "添加";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 187);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(72, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "确认";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(12, 158);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 8;
            this.button4.Text = "SQL语句";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // AddTables
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(357, 222);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tablename);
            this.Controls.Add(this.script_box);
            this.Name = "AddTables";
            this.Text = "添加表";
            this.Load += new System.EventHandler(this.AddTables_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox script_box;
        private System.Windows.Forms.ComboBox fieldtype_cbx;
        private System.Windows.Forms.TextBox tablename;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox utfsupp_cbx;
        private System.Windows.Forms.TextBox fieldlength;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox fieldname;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
    }
}