namespace MM370_Tools
{
    partial class FrmConfig
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbSpotBits = new System.Windows.Forms.ComboBox();
            this.cbByteSize = new System.Windows.Forms.ComboBox();
            this.cbParity = new System.Windows.Forms.ComboBox();
            this.cbBaudRate = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbPort = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chSw = new System.Windows.Forms.CheckBox();
            this.chHw = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chRts = new System.Windows.Forms.CheckBox();
            this.chDtr = new System.Windows.Forms.CheckBox();
            this.CfgOK = new System.Windows.Forms.Button();
            this.CfgCancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbSpotBits);
            this.groupBox1.Controls.Add(this.cbByteSize);
            this.groupBox1.Controls.Add(this.cbParity);
            this.groupBox1.Controls.Add(this.cbBaudRate);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cbPort);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 178);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Com Option";
            // 
            // cbSpotBits
            // 
            this.cbSpotBits.FormattingEnabled = true;
            this.cbSpotBits.Items.AddRange(new object[] {
            "STOP_1 ",
            "STOP_2"});
            this.cbSpotBits.Location = new System.Drawing.Point(77, 147);
            this.cbSpotBits.Name = "cbSpotBits";
            this.cbSpotBits.Size = new System.Drawing.Size(103, 20);
            this.cbSpotBits.TabIndex = 4;
            // 
            // cbByteSize
            // 
            this.cbByteSize.FormattingEnabled = true;
            this.cbByteSize.Items.AddRange(new object[] {
            "BIT_5 ",
            "BIT_6",
            "BIT_7 ",
            "BIT_8"});
            this.cbByteSize.Location = new System.Drawing.Point(78, 119);
            this.cbByteSize.Name = "cbByteSize";
            this.cbByteSize.Size = new System.Drawing.Size(102, 20);
            this.cbByteSize.TabIndex = 3;
            // 
            // cbParity
            // 
            this.cbParity.FormattingEnabled = true;
            this.cbParity.Items.AddRange(new object[] {
            "P_NONE",
            "P_EVEN",
            "P_ODD",
            "P_MRK",
            "P_SPC"});
            this.cbParity.Location = new System.Drawing.Point(77, 90);
            this.cbParity.Name = "cbParity";
            this.cbParity.Size = new System.Drawing.Size(103, 20);
            this.cbParity.TabIndex = 2;
            // 
            // cbBaudRate
            // 
            this.cbBaudRate.FormattingEnabled = true;
            this.cbBaudRate.Items.AddRange(new object[] {
            "B50 ",
            "B75 ",
            "B110 ",
            "B134 ",
            "B150 ",
            "B300 ",
            "B600 ",
            "B1200",
            "B1800 ",
            "B2400 ",
            "B4800 ",
            "B7200",
            "B9600",
            "B19200 ",
            "B38400 ",
            "B57600 ",
            "B115200 ",
            "B230400 ",
            "B460800 ",
            "B921600 "});
            this.cbBaudRate.Location = new System.Drawing.Point(77, 58);
            this.cbBaudRate.Name = "cbBaudRate";
            this.cbBaudRate.Size = new System.Drawing.Size(103, 20);
            this.cbBaudRate.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 150);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 5;
            this.label5.Text = "Stop Bits:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(7, 122);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "Data Bits:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(7, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "Parity:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(7, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "Baud Rate:";
            // 
            // cbPort
            // 
            this.cbPort.FormattingEnabled = true;
            this.cbPort.Location = new System.Drawing.Point(77, 26);
            this.cbPort.Name = "cbPort";
            this.cbPort.Size = new System.Drawing.Size(103, 20);
            this.cbPort.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(6, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "Port:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chSw);
            this.groupBox2.Controls.Add(this.chHw);
            this.groupBox2.Location = new System.Drawing.Point(12, 219);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 69);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Flow Control";
            // 
            // chSw
            // 
            this.chSw.AutoSize = true;
            this.chSw.Location = new System.Drawing.Point(9, 44);
            this.chSw.Name = "chSw";
            this.chSw.Size = new System.Drawing.Size(72, 16);
            this.chSw.TabIndex = 7;
            this.chSw.Text = "XON/XOFF";
            this.chSw.UseVisualStyleBackColor = true;
            // 
            // chHw
            // 
            this.chHw.AutoSize = true;
            this.chHw.Location = new System.Drawing.Point(9, 20);
            this.chHw.Name = "chHw";
            this.chHw.Size = new System.Drawing.Size(66, 16);
            this.chHw.TabIndex = 6;
            this.chHw.Text = "RTS/CTS";
            this.chHw.UseVisualStyleBackColor = true;
            this.chHw.Click += new System.EventHandler(this.ChHw_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chRts);
            this.groupBox3.Controls.Add(this.chDtr);
            this.groupBox3.Location = new System.Drawing.Point(225, 219);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(96, 69);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Output State";
            // 
            // chRts
            // 
            this.chRts.AutoSize = true;
            this.chRts.Location = new System.Drawing.Point(12, 47);
            this.chRts.Name = "chRts";
            this.chRts.Size = new System.Drawing.Size(42, 16);
            this.chRts.TabIndex = 10;
            this.chRts.Text = "RTS";
            this.chRts.UseVisualStyleBackColor = true;
            // 
            // chDtr
            // 
            this.chDtr.AutoSize = true;
            this.chDtr.Location = new System.Drawing.Point(12, 20);
            this.chDtr.Name = "chDtr";
            this.chDtr.Size = new System.Drawing.Size(42, 16);
            this.chDtr.TabIndex = 9;
            this.chDtr.Text = "DTR";
            this.chDtr.UseVisualStyleBackColor = true;
            // 
            // CfgOK
            // 
            this.CfgOK.Location = new System.Drawing.Point(237, 62);
            this.CfgOK.Name = "CfgOK";
            this.CfgOK.Size = new System.Drawing.Size(75, 23);
            this.CfgOK.TabIndex = 12;
            this.CfgOK.Text = "OK";
            this.CfgOK.UseVisualStyleBackColor = true;
            this.CfgOK.Click += new System.EventHandler(this.CfgOK_Click);
            // 
            // CfgCancel
            // 
            this.CfgCancel.Location = new System.Drawing.Point(237, 123);
            this.CfgCancel.Name = "CfgCancel";
            this.CfgCancel.Size = new System.Drawing.Size(75, 23);
            this.CfgCancel.TabIndex = 13;
            this.CfgCancel.Text = "Cancel";
            this.CfgCancel.UseVisualStyleBackColor = true;
            this.CfgCancel.Click += new System.EventHandler(this.CfgCancel_Click);
            // 
            // FrmConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(333, 304);
            this.Controls.Add(this.CfgCancel);
            this.Controls.Add(this.CfgOK);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "FrmConfig";
            this.Text = "Com Option";
            this.Load += new System.EventHandler(this.FrmConfig_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cbSpotBits;
        private System.Windows.Forms.ComboBox cbByteSize;
        private System.Windows.Forms.ComboBox cbParity;
        private System.Windows.Forms.ComboBox cbBaudRate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chSw;
        private System.Windows.Forms.CheckBox chHw;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chRts;
        private System.Windows.Forms.CheckBox chDtr;
        private System.Windows.Forms.Button CfgOK;
        private System.Windows.Forms.Button CfgCancel;
    }
}