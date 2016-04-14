namespace WindowsFormsApplication3
{
    partial class ConnectConfig
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
            this.label1 = new System.Windows.Forms.Label();
            this.username = new System.Windows.Forms.TextBox();
            this.password = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.server = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.option = new System.Windows.Forms.TextBox();
            this.lable6 = new System.Windows.Forms.Label();
            this.port = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.database = new System.Windows.Forms.ComboBox();
            this.disconnect = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.connect_status = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "User Name";
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(12, 25);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(100, 21);
            this.username.TabIndex = 1;
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(118, 25);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(100, 21);
            this.password.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(118, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "Password";
            // 
            // server
            // 
            this.server.Location = new System.Drawing.Point(224, 25);
            this.server.Name = "server";
            this.server.Size = new System.Drawing.Size(100, 21);
            this.server.TabIndex = 5;
            this.server.Text = "localhost";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(224, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "Server";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(74, 97);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Accept";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // option
            // 
            this.option.Location = new System.Drawing.Point(226, 70);
            this.option.Name = "option";
            this.option.Size = new System.Drawing.Size(100, 21);
            this.option.TabIndex = 12;
            this.option.Text = "3";
            // 
            // lable6
            // 
            this.lable6.AutoSize = true;
            this.lable6.Location = new System.Drawing.Point(226, 55);
            this.lable6.Name = "lable6";
            this.lable6.Size = new System.Drawing.Size(41, 12);
            this.lable6.TabIndex = 11;
            this.lable6.Text = "Option";
            // 
            // port
            // 
            this.port.Location = new System.Drawing.Point(120, 70);
            this.port.Name = "port";
            this.port.Size = new System.Drawing.Size(100, 21);
            this.port.TabIndex = 10;
            this.port.Text = "3306";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(120, 55);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "Port";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 55);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 7;
            this.label6.Text = "DataBase";
            // 
            // database
            // 
            this.database.FormattingEnabled = true;
            this.database.Location = new System.Drawing.Point(12, 71);
            this.database.Name = "database";
            this.database.Size = new System.Drawing.Size(100, 20);
            this.database.TabIndex = 13;
            // 
            // disconnect
            // 
            this.disconnect.Location = new System.Drawing.Point(190, 97);
            this.disconnect.Name = "disconnect";
            this.disconnect.Size = new System.Drawing.Size(75, 23);
            this.disconnect.TabIndex = 14;
            this.disconnect.Text = "Disconnect";
            this.disconnect.UseVisualStyleBackColor = true;
            this.disconnect.Click += new System.EventHandler(this.disconnect_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connect_status});
            this.statusStrip1.Location = new System.Drawing.Point(0, 129);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(337, 22);
            this.statusStrip1.TabIndex = 15;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // connect_status
            // 
            this.connect_status.Name = "connect_status";
            this.connect_status.Size = new System.Drawing.Size(104, 17);
            this.connect_status.Text = "Connection Status";
            // 
            // ConnectConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(337, 151);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.disconnect);
            this.Controls.Add(this.database);
            this.Controls.Add(this.option);
            this.Controls.Add(this.lable6);
            this.Controls.Add(this.port);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.server);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.password);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.username);
            this.Controls.Add(this.label1);
            this.MaximumSize = new System.Drawing.Size(353, 190);
            this.MinimumSize = new System.Drawing.Size(353, 190);
            this.Name = "ConnectConfig";
            this.Text = "ConnectConfig";
            this.Load += new System.EventHandler(this.ConnectConfig_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox server;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox option;
        private System.Windows.Forms.Label lable6;
        private System.Windows.Forms.TextBox port;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox database;
        private System.Windows.Forms.Button disconnect;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel connect_status;
    }
}