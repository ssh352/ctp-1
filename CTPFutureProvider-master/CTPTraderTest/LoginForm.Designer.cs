namespace CTPTraderTest
{
  partial class LoginForm
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
      this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.idTradeServerAddressTextBox = new System.Windows.Forms.TextBox();
      this.idQuoteServerAddressTextBox = new System.Windows.Forms.TextBox();
      this.idBrokerTextBox = new System.Windows.Forms.TextBox();
      this.idAccountTextBox = new System.Windows.Forms.TextBox();
      this.idPasswordTextBox = new System.Windows.Forms.TextBox();
      this.idLoginButton = new System.Windows.Forms.Button();
      this.panel1 = new System.Windows.Forms.Panel();
      this.idCloseButton = new System.Windows.Forms.Button();
      this.statusStrip1 = new System.Windows.Forms.StatusStrip();
      this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
      this.tableLayoutPanel1.SuspendLayout();
      this.panel1.SuspendLayout();
      this.statusStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.ColumnCount = 2;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.01266F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 78.98734F));
      this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
      this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
      this.tableLayoutPanel1.Controls.Add(this.label3, 0, 3);
      this.tableLayoutPanel1.Controls.Add(this.label4, 0, 4);
      this.tableLayoutPanel1.Controls.Add(this.label5, 0, 5);
      this.tableLayoutPanel1.Controls.Add(this.idTradeServerAddressTextBox, 1, 1);
      this.tableLayoutPanel1.Controls.Add(this.idQuoteServerAddressTextBox, 1, 2);
      this.tableLayoutPanel1.Controls.Add(this.idBrokerTextBox, 1, 3);
      this.tableLayoutPanel1.Controls.Add(this.idAccountTextBox, 1, 4);
      this.tableLayoutPanel1.Controls.Add(this.idPasswordTextBox, 1, 5);
      this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 6);
      this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 8;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel1.Size = new System.Drawing.Size(395, 237);
      this.tableLayoutPanel1.TabIndex = 0;
      // 
      // label1
      // 
      this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.label1.Location = new System.Drawing.Point(3, 20);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(77, 30);
      this.label1.TabIndex = 0;
      this.label1.Text = "交易服务器";
      this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // label2
      // 
      this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.label2.Location = new System.Drawing.Point(3, 50);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(77, 30);
      this.label2.TabIndex = 0;
      this.label2.Text = "行情服务器";
      this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // label3
      // 
      this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
      this.label3.Location = new System.Drawing.Point(3, 80);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(77, 30);
      this.label3.TabIndex = 0;
      this.label3.Text = "经纪商ID";
      this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // label4
      // 
      this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
      this.label4.Location = new System.Drawing.Point(3, 110);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(77, 30);
      this.label4.TabIndex = 0;
      this.label4.Text = "交易账户";
      this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // label5
      // 
      this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
      this.label5.Location = new System.Drawing.Point(3, 140);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(77, 30);
      this.label5.TabIndex = 0;
      this.label5.Text = "交易密码";
      this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // idTradeServerAddressTextBox
      // 
      this.idTradeServerAddressTextBox.Location = new System.Drawing.Point(86, 23);
      this.idTradeServerAddressTextBox.Name = "idTradeServerAddressTextBox";
      this.idTradeServerAddressTextBox.Size = new System.Drawing.Size(291, 21);
      this.idTradeServerAddressTextBox.TabIndex = 1;
      // 
      // idQuoteServerAddressTextBox
      // 
      this.idQuoteServerAddressTextBox.Location = new System.Drawing.Point(86, 53);
      this.idQuoteServerAddressTextBox.Name = "idQuoteServerAddressTextBox";
      this.idQuoteServerAddressTextBox.Size = new System.Drawing.Size(291, 21);
      this.idQuoteServerAddressTextBox.TabIndex = 1;
      // 
      // idBrokerTextBox
      // 
      this.idBrokerTextBox.Location = new System.Drawing.Point(86, 83);
      this.idBrokerTextBox.Name = "idBrokerTextBox";
      this.idBrokerTextBox.Size = new System.Drawing.Size(100, 21);
      this.idBrokerTextBox.TabIndex = 1;
      // 
      // idAccountTextBox
      // 
      this.idAccountTextBox.Location = new System.Drawing.Point(86, 113);
      this.idAccountTextBox.Name = "idAccountTextBox";
      this.idAccountTextBox.Size = new System.Drawing.Size(100, 21);
      this.idAccountTextBox.TabIndex = 1;
      // 
      // idPasswordTextBox
      // 
      this.idPasswordTextBox.Location = new System.Drawing.Point(86, 143);
      this.idPasswordTextBox.Name = "idPasswordTextBox";
      this.idPasswordTextBox.Size = new System.Drawing.Size(100, 21);
      this.idPasswordTextBox.TabIndex = 1;
      // 
      // idLoginButton
      // 
      this.idLoginButton.Location = new System.Drawing.Point(128, 8);
      this.idLoginButton.Name = "idLoginButton";
      this.idLoginButton.Size = new System.Drawing.Size(77, 30);
      this.idLoginButton.TabIndex = 2;
      this.idLoginButton.Text = "登录";
      this.idLoginButton.UseVisualStyleBackColor = true;
      this.idLoginButton.Click += new System.EventHandler(this.idLoginButton_Click);
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.idCloseButton);
      this.panel1.Controls.Add(this.idLoginButton);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel1.Location = new System.Drawing.Point(86, 173);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(306, 44);
      this.panel1.TabIndex = 3;
      // 
      // idCloseButton
      // 
      this.idCloseButton.Location = new System.Drawing.Point(211, 8);
      this.idCloseButton.Name = "idCloseButton";
      this.idCloseButton.Size = new System.Drawing.Size(77, 30);
      this.idCloseButton.TabIndex = 2;
      this.idCloseButton.Text = "关闭";
      this.idCloseButton.UseVisualStyleBackColor = true;
      // 
      // statusStrip1
      // 
      this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
      this.statusStrip1.Location = new System.Drawing.Point(0, 215);
      this.statusStrip1.Name = "statusStrip1";
      this.statusStrip1.Size = new System.Drawing.Size(395, 22);
      this.statusStrip1.SizingGrip = false;
      this.statusStrip1.TabIndex = 1;
      this.statusStrip1.Text = "statusStrip1";
      // 
      // toolStripStatusLabel1
      // 
      this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
      this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
      // 
      // LoginForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.idCloseButton;
      this.ClientSize = new System.Drawing.Size(395, 237);
      this.ControlBox = false;
      this.Controls.Add(this.statusStrip1);
      this.Controls.Add(this.tableLayoutPanel1);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "LoginForm";
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "登录";
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.statusStrip1.ResumeLayout(false);
      this.statusStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.TextBox idTradeServerAddressTextBox;
    private System.Windows.Forms.TextBox idQuoteServerAddressTextBox;
    private System.Windows.Forms.TextBox idBrokerTextBox;
    private System.Windows.Forms.TextBox idAccountTextBox;
    private System.Windows.Forms.TextBox idPasswordTextBox;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Button idCloseButton;
    private System.Windows.Forms.Button idLoginButton;
    private System.Windows.Forms.StatusStrip statusStrip1;
    private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
  }
}