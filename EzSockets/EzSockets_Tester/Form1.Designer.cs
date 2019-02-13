namespace EzSockets_Tester
{
    partial class Form1
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.serverOutTextBox = new System.Windows.Forms.TextBox();
            this.startServerBtn = new System.Windows.Forms.Button();
            this.portInput = new System.Windows.Forms.TextBox();
            this.portLabel = new System.Windows.Forms.Label();
            this.ServerSendMsgText = new System.Windows.Forms.TextBox();
            this.ServerSendMsgBtn = new System.Windows.Forms.Button();
            this.ClientSocketsList = new System.Windows.Forms.ListBox();
            this.NewConnectionBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.CloseSocketBtn = new System.Windows.Forms.Button();
            this.ClientOutputBox = new System.Windows.Forms.TextBox();
            this.ClientSendMsgBtn = new System.Windows.Forms.Button();
            this.ClientSendMsgText = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ServerSendMsgBtn);
            this.groupBox1.Controls.Add(this.ServerSendMsgText);
            this.groupBox1.Controls.Add(this.portLabel);
            this.groupBox1.Controls.Add(this.portInput);
            this.groupBox1.Controls.Add(this.startServerBtn);
            this.groupBox1.Controls.Add(this.serverOutTextBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(320, 426);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Server Side";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ClientSendMsgBtn);
            this.groupBox2.Controls.Add(this.ClientSendMsgText);
            this.groupBox2.Controls.Add(this.ClientOutputBox);
            this.groupBox2.Controls.Add(this.CloseSocketBtn);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.NewConnectionBtn);
            this.groupBox2.Controls.Add(this.ClientSocketsList);
            this.groupBox2.Location = new System.Drawing.Point(338, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(320, 426);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Client Side";
            // 
            // serverOutTextBox
            // 
            this.serverOutTextBox.Location = new System.Drawing.Point(6, 68);
            this.serverOutTextBox.Multiline = true;
            this.serverOutTextBox.Name = "serverOutTextBox";
            this.serverOutTextBox.ReadOnly = true;
            this.serverOutTextBox.Size = new System.Drawing.Size(308, 320);
            this.serverOutTextBox.TabIndex = 0;
            // 
            // startServerBtn
            // 
            this.startServerBtn.Location = new System.Drawing.Point(6, 34);
            this.startServerBtn.Name = "startServerBtn";
            this.startServerBtn.Size = new System.Drawing.Size(95, 28);
            this.startServerBtn.TabIndex = 1;
            this.startServerBtn.Text = "Start Server";
            this.startServerBtn.UseVisualStyleBackColor = true;
            this.startServerBtn.Click += new System.EventHandler(this.startServerBtn_Click);
            // 
            // portInput
            // 
            this.portInput.Location = new System.Drawing.Point(142, 39);
            this.portInput.MaxLength = 8;
            this.portInput.Name = "portInput";
            this.portInput.Size = new System.Drawing.Size(97, 20);
            this.portInput.TabIndex = 0;
            this.portInput.Text = "8080";
            this.portInput.TextChanged += new System.EventHandler(this.portInput_TextChanged);
            // 
            // portLabel
            // 
            this.portLabel.AutoSize = true;
            this.portLabel.Location = new System.Drawing.Point(107, 42);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(29, 13);
            this.portLabel.TabIndex = 2;
            this.portLabel.Text = "Port:";
            // 
            // ServerSendMsgText
            // 
            this.ServerSendMsgText.Location = new System.Drawing.Point(6, 394);
            this.ServerSendMsgText.Name = "ServerSendMsgText";
            this.ServerSendMsgText.Size = new System.Drawing.Size(233, 20);
            this.ServerSendMsgText.TabIndex = 0;
            // 
            // ServerSendMsgBtn
            // 
            this.ServerSendMsgBtn.Location = new System.Drawing.Point(245, 394);
            this.ServerSendMsgBtn.Name = "ServerSendMsgBtn";
            this.ServerSendMsgBtn.Size = new System.Drawing.Size(69, 22);
            this.ServerSendMsgBtn.TabIndex = 3;
            this.ServerSendMsgBtn.Text = "Send";
            this.ServerSendMsgBtn.UseVisualStyleBackColor = true;
            this.ServerSendMsgBtn.Click += new System.EventHandler(this.SendMsgBtn_Click);
            // 
            // ClientSocketsList
            // 
            this.ClientSocketsList.FormattingEnabled = true;
            this.ClientSocketsList.Location = new System.Drawing.Point(12, 46);
            this.ClientSocketsList.Name = "ClientSocketsList";
            this.ClientSocketsList.Size = new System.Drawing.Size(296, 199);
            this.ClientSocketsList.TabIndex = 0;
            // 
            // NewConnectionBtn
            // 
            this.NewConnectionBtn.Location = new System.Drawing.Point(12, 251);
            this.NewConnectionBtn.Name = "NewConnectionBtn";
            this.NewConnectionBtn.Size = new System.Drawing.Size(127, 32);
            this.NewConnectionBtn.TabIndex = 1;
            this.NewConnectionBtn.Text = "Create New Client";
            this.NewConnectionBtn.UseVisualStyleBackColor = true;
            this.NewConnectionBtn.Click += new System.EventHandler(this.NewConnectionBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Active Client Sockets:";
            // 
            // CloseSocketBtn
            // 
            this.CloseSocketBtn.Location = new System.Drawing.Point(145, 251);
            this.CloseSocketBtn.Name = "CloseSocketBtn";
            this.CloseSocketBtn.Size = new System.Drawing.Size(162, 31);
            this.CloseSocketBtn.TabIndex = 3;
            this.CloseSocketBtn.Text = "Close Client Socket";
            this.CloseSocketBtn.UseVisualStyleBackColor = true;
            this.CloseSocketBtn.Click += new System.EventHandler(this.CloseSocketBtn_Click);
            // 
            // ClientOutputBox
            // 
            this.ClientOutputBox.Location = new System.Drawing.Point(12, 289);
            this.ClientOutputBox.Multiline = true;
            this.ClientOutputBox.Name = "ClientOutputBox";
            this.ClientOutputBox.ReadOnly = true;
            this.ClientOutputBox.Size = new System.Drawing.Size(296, 99);
            this.ClientOutputBox.TabIndex = 4;
            // 
            // ClientSendMsgBtn
            // 
            this.ClientSendMsgBtn.Location = new System.Drawing.Point(239, 398);
            this.ClientSendMsgBtn.Name = "ClientSendMsgBtn";
            this.ClientSendMsgBtn.Size = new System.Drawing.Size(69, 22);
            this.ClientSendMsgBtn.TabIndex = 5;
            this.ClientSendMsgBtn.Text = "Send";
            this.ClientSendMsgBtn.UseVisualStyleBackColor = true;
            this.ClientSendMsgBtn.Click += new System.EventHandler(this.ClientSendMsgBtn_Click);
            // 
            // ClientSendMsgText
            // 
            this.ClientSendMsgText.Location = new System.Drawing.Point(12, 398);
            this.ClientSendMsgText.Name = "ClientSendMsgText";
            this.ClientSendMsgText.Size = new System.Drawing.Size(221, 20);
            this.ClientSendMsgText.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(670, 447);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "EzSockets Tester";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox serverOutTextBox;
        private System.Windows.Forms.TextBox portInput;
        private System.Windows.Forms.Button startServerBtn;
        private System.Windows.Forms.Label portLabel;
        private System.Windows.Forms.Button ServerSendMsgBtn;
        private System.Windows.Forms.TextBox ServerSendMsgText;
        private System.Windows.Forms.ListBox ClientSocketsList;
        private System.Windows.Forms.Button NewConnectionBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button CloseSocketBtn;
        private System.Windows.Forms.TextBox ClientOutputBox;
        private System.Windows.Forms.Button ClientSendMsgBtn;
        private System.Windows.Forms.TextBox ClientSendMsgText;
    }
}

