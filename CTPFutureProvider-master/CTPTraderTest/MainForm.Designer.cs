using System.Windows.Forms;
namespace CTPTraderTest
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.idOrderPage = new System.Windows.Forms.TabPage();
            this.idOrderDataGridView = new System.Windows.Forms.DataGridView();
            this.idPositionPage = new System.Windows.Forms.TabPage();
            this.idPositionDetailDataGridView = new System.Windows.Forms.DataGridView();
            this.idTradePage = new System.Windows.Forms.TabPage();
            this.idTradeDataGridView = new System.Windows.Forms.DataGridView();
            this.idSymbolPage = new System.Windows.Forms.TabPage();
            this.idSymbolDataGridView = new System.Windows.Forms.DataGridView();
            this.idAmountPage = new System.Windows.Forms.TabPage();
            this.idAmountDataGridView = new System.Windows.Forms.DataGridView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.idFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.idLoginMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.idSystemQueryMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.idQuerySymbolMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.idQueryUserMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.idTradeQueryMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.idTradeQueryAmountMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.idTradeQueryPositionMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.idTradeQueryOrderMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.idTradeQueryTradeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.idQuoteDataGridView = new System.Windows.Forms.DataGridView();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.idSymbolCodeComboBox = new System.Windows.Forms.ComboBox();
            this.idQueryQuoteButton = new System.Windows.Forms.Button();
            this.idDirectionComboBox = new System.Windows.Forms.ComboBox();
            this.idOffsetComboBox = new System.Windows.Forms.ComboBox();
            this.idVolumeNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.idPriceNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.idSendOrderButton = new System.Windows.Forms.Button();
            this.idCancelButton = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.idOrderPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.idOrderDataGridView)).BeginInit();
            this.idPositionPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.idPositionDetailDataGridView)).BeginInit();
            this.idTradePage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.idTradeDataGridView)).BeginInit();
            this.idSymbolPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.idSymbolDataGridView)).BeginInit();
            this.idAmountPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.idAmountDataGridView)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.idQuoteDataGridView)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.idVolumeNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.idPriceNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControl1.Controls.Add(this.idOrderPage);
            this.tabControl1.Controls.Add(this.idPositionPage);
            this.tabControl1.Controls.Add(this.idTradePage);
            this.tabControl1.Controls.Add(this.idSymbolPage);
            this.tabControl1.Controls.Add(this.idAmountPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(688, 198);
            this.tabControl1.TabIndex = 3;
            // 
            // idOrderPage
            // 
            this.idOrderPage.Controls.Add(this.idOrderDataGridView);
            this.idOrderPage.Location = new System.Drawing.Point(4, 4);
            this.idOrderPage.Name = "idOrderPage";
            this.idOrderPage.Padding = new System.Windows.Forms.Padding(3);
            this.idOrderPage.Size = new System.Drawing.Size(680, 173);
            this.idOrderPage.TabIndex = 1;
            this.idOrderPage.Text = "报单";
            this.idOrderPage.UseVisualStyleBackColor = true;
            // 
            // idOrderDataGridView
            // 
            this.idOrderDataGridView.AllowUserToAddRows = false;
            this.idOrderDataGridView.AllowUserToDeleteRows = false;
            this.idOrderDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.idOrderDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.idOrderDataGridView.Location = new System.Drawing.Point(3, 3);
            this.idOrderDataGridView.Name = "idOrderDataGridView";
            this.idOrderDataGridView.ReadOnly = true;
            this.idOrderDataGridView.RowTemplate.Height = 23;
            this.idOrderDataGridView.Size = new System.Drawing.Size(674, 167);
            this.idOrderDataGridView.TabIndex = 0;
            this.idOrderDataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.idOrderDataGridView_DataError);
            // 
            // idPositionPage
            // 
            this.idPositionPage.Controls.Add(this.idPositionDetailDataGridView);
            this.idPositionPage.Location = new System.Drawing.Point(4, 4);
            this.idPositionPage.Name = "idPositionPage";
            this.idPositionPage.Padding = new System.Windows.Forms.Padding(3);
            this.idPositionPage.Size = new System.Drawing.Size(680, 172);
            this.idPositionPage.TabIndex = 2;
            this.idPositionPage.Text = "持仓";
            this.idPositionPage.UseVisualStyleBackColor = true;
            // 
            // idPositionDetailDataGridView
            // 
            this.idPositionDetailDataGridView.AllowUserToAddRows = false;
            this.idPositionDetailDataGridView.AllowUserToDeleteRows = false;
            this.idPositionDetailDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.idPositionDetailDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.idPositionDetailDataGridView.Location = new System.Drawing.Point(3, 3);
            this.idPositionDetailDataGridView.Name = "idPositionDetailDataGridView";
            this.idPositionDetailDataGridView.ReadOnly = true;
            this.idPositionDetailDataGridView.RowTemplate.Height = 23;
            this.idPositionDetailDataGridView.Size = new System.Drawing.Size(674, 165);
            this.idPositionDetailDataGridView.TabIndex = 0;
            this.idPositionDetailDataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.idOrderDataGridView_DataError);
            // 
            // idTradePage
            // 
            this.idTradePage.Controls.Add(this.idTradeDataGridView);
            this.idTradePage.Location = new System.Drawing.Point(4, 4);
            this.idTradePage.Name = "idTradePage";
            this.idTradePage.Padding = new System.Windows.Forms.Padding(3);
            this.idTradePage.Size = new System.Drawing.Size(680, 172);
            this.idTradePage.TabIndex = 4;
            this.idTradePage.Text = "成交";
            this.idTradePage.UseVisualStyleBackColor = true;
            // 
            // idTradeDataGridView
            // 
            this.idTradeDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.idTradeDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.idTradeDataGridView.Location = new System.Drawing.Point(3, 3);
            this.idTradeDataGridView.Name = "idTradeDataGridView";
            this.idTradeDataGridView.RowTemplate.Height = 23;
            this.idTradeDataGridView.Size = new System.Drawing.Size(674, 165);
            this.idTradeDataGridView.TabIndex = 0;
            // 
            // idSymbolPage
            // 
            this.idSymbolPage.Controls.Add(this.idSymbolDataGridView);
            this.idSymbolPage.Location = new System.Drawing.Point(4, 4);
            this.idSymbolPage.Name = "idSymbolPage";
            this.idSymbolPage.Padding = new System.Windows.Forms.Padding(3);
            this.idSymbolPage.Size = new System.Drawing.Size(680, 173);
            this.idSymbolPage.TabIndex = 3;
            this.idSymbolPage.Text = "合约";
            this.idSymbolPage.UseVisualStyleBackColor = true;
            // 
            // idSymbolDataGridView
            // 
            this.idSymbolDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.idSymbolDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.idSymbolDataGridView.Location = new System.Drawing.Point(3, 3);
            this.idSymbolDataGridView.Name = "idSymbolDataGridView";
            this.idSymbolDataGridView.RowTemplate.Height = 23;
            this.idSymbolDataGridView.Size = new System.Drawing.Size(674, 167);
            this.idSymbolDataGridView.TabIndex = 0;
            // 
            // idAmountPage
            // 
            this.idAmountPage.Controls.Add(this.idAmountDataGridView);
            this.idAmountPage.Location = new System.Drawing.Point(4, 4);
            this.idAmountPage.Name = "idAmountPage";
            this.idAmountPage.Padding = new System.Windows.Forms.Padding(3);
            this.idAmountPage.Size = new System.Drawing.Size(680, 172);
            this.idAmountPage.TabIndex = 0;
            this.idAmountPage.Text = "资金";
            this.idAmountPage.UseVisualStyleBackColor = true;
            // 
            // idAmountDataGridView
            // 
            this.idAmountDataGridView.AllowUserToAddRows = false;
            this.idAmountDataGridView.AllowUserToDeleteRows = false;
            this.idAmountDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.idAmountDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.idAmountDataGridView.Location = new System.Drawing.Point(3, 3);
            this.idAmountDataGridView.Name = "idAmountDataGridView";
            this.idAmountDataGridView.ReadOnly = true;
            this.idAmountDataGridView.RowTemplate.Height = 23;
            this.idAmountDataGridView.Size = new System.Drawing.Size(674, 165);
            this.idAmountDataGridView.TabIndex = 1;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.idFileMenuItem,
            this.idSystemQueryMenuItem,
            this.idTradeQueryMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(688, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // idFileMenuItem
            // 
            this.idFileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.idLoginMenuItem});
            this.idFileMenuItem.Name = "idFileMenuItem";
            this.idFileMenuItem.Size = new System.Drawing.Size(41, 20);
            this.idFileMenuItem.Text = "文件";
            // 
            // idLoginMenuItem
            // 
            this.idLoginMenuItem.Name = "idLoginMenuItem";
            this.idLoginMenuItem.Size = new System.Drawing.Size(152, 22);
            this.idLoginMenuItem.Text = "登录";
            this.idLoginMenuItem.Click += new System.EventHandler(this.idLoginMenuItem_Click);
            // 
            // idSystemQueryMenuItem
            // 
            this.idSystemQueryMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.idQuerySymbolMenuItem,
            this.idQueryUserMenuItem});
            this.idSystemQueryMenuItem.Name = "idSystemQueryMenuItem";
            this.idSystemQueryMenuItem.Size = new System.Drawing.Size(65, 20);
            this.idSystemQueryMenuItem.Text = "系统查询";
            // 
            // idQuerySymbolMenuItem
            // 
            this.idQuerySymbolMenuItem.Name = "idQuerySymbolMenuItem";
            this.idQuerySymbolMenuItem.Size = new System.Drawing.Size(152, 22);
            this.idQuerySymbolMenuItem.Text = "查询合约";
            this.idQuerySymbolMenuItem.Click += new System.EventHandler(this.idQuerySymbolMenuItem_Click);
            // 
            // idQueryUserMenuItem
            // 
            this.idQueryUserMenuItem.Name = "idQueryUserMenuItem";
            this.idQueryUserMenuItem.Size = new System.Drawing.Size(152, 22);
            this.idQueryUserMenuItem.Text = "查询客户信息";
            this.idQueryUserMenuItem.Click += new System.EventHandler(this.idQueryUserMenuItem_Click);
            // 
            // idTradeQueryMenuItem
            // 
            this.idTradeQueryMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.idTradeQueryAmountMenuItem,
            this.idTradeQueryPositionMenuItem,
            this.idTradeQueryOrderMenuItem,
            this.idTradeQueryTradeMenuItem});
            this.idTradeQueryMenuItem.Name = "idTradeQueryMenuItem";
            this.idTradeQueryMenuItem.Size = new System.Drawing.Size(65, 20);
            this.idTradeQueryMenuItem.Text = "交易查询";
            // 
            // idTradeQueryAmountMenuItem
            // 
            this.idTradeQueryAmountMenuItem.Name = "idTradeQueryAmountMenuItem";
            this.idTradeQueryAmountMenuItem.Size = new System.Drawing.Size(152, 22);
            this.idTradeQueryAmountMenuItem.Text = "资金";
            this.idTradeQueryAmountMenuItem.Click += new System.EventHandler(this.idTradeQueryAmountMenuItem_Click);
            // 
            // idTradeQueryPositionMenuItem
            // 
            this.idTradeQueryPositionMenuItem.Name = "idTradeQueryPositionMenuItem";
            this.idTradeQueryPositionMenuItem.Size = new System.Drawing.Size(152, 22);
            this.idTradeQueryPositionMenuItem.Text = "持仓";
            this.idTradeQueryPositionMenuItem.Click += new System.EventHandler(this.idTradeQueryPositionMenuItem_Click);
            // 
            // idTradeQueryOrderMenuItem
            // 
            this.idTradeQueryOrderMenuItem.Name = "idTradeQueryOrderMenuItem";
            this.idTradeQueryOrderMenuItem.Size = new System.Drawing.Size(152, 22);
            this.idTradeQueryOrderMenuItem.Text = "报单";
            this.idTradeQueryOrderMenuItem.Click += new System.EventHandler(this.idTradeQueryOrderMenuItem_Click);
            // 
            // idTradeQueryTradeMenuItem
            // 
            this.idTradeQueryTradeMenuItem.Name = "idTradeQueryTradeMenuItem";
            this.idTradeQueryTradeMenuItem.Size = new System.Drawing.Size(152, 22);
            this.idTradeQueryTradeMenuItem.Text = "成交";
            this.idTradeQueryTradeMenuItem.Click += new System.EventHandler(this.idTradeQueryTradeMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 372);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(688, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(29, 17);
            this.toolStripStatusLabel1.Text = "就绪";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(688, 348);
            this.splitContainer1.SplitterDistance = 146;
            this.splitContainer1.TabIndex = 6;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.idQuoteDataGridView);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.flowLayoutPanel1);
            this.splitContainer2.Size = new System.Drawing.Size(688, 146);
            this.splitContainer2.SplitterDistance = 113;
            this.splitContainer2.TabIndex = 0;
            // 
            // idQuoteDataGridView
            // 
            this.idQuoteDataGridView.AllowUserToAddRows = false;
            this.idQuoteDataGridView.AllowUserToDeleteRows = false;
            this.idQuoteDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.idQuoteDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.idQuoteDataGridView.Location = new System.Drawing.Point(0, 0);
            this.idQuoteDataGridView.Name = "idQuoteDataGridView";
            this.idQuoteDataGridView.ReadOnly = true;
            this.idQuoteDataGridView.RowTemplate.Height = 23;
            this.idQuoteDataGridView.Size = new System.Drawing.Size(688, 113);
            this.idQuoteDataGridView.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.idSymbolCodeComboBox);
            this.flowLayoutPanel1.Controls.Add(this.idQueryQuoteButton);
            this.flowLayoutPanel1.Controls.Add(this.idDirectionComboBox);
            this.flowLayoutPanel1.Controls.Add(this.idOffsetComboBox);
            this.flowLayoutPanel1.Controls.Add(this.idVolumeNumericUpDown);
            this.flowLayoutPanel1.Controls.Add(this.idPriceNumericUpDown);
            this.flowLayoutPanel1.Controls.Add(this.idSendOrderButton);
            this.flowLayoutPanel1.Controls.Add(this.idCancelButton);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(688, 29);
            this.flowLayoutPanel1.TabIndex = 7;
            // 
            // idSymbolCodeComboBox
            // 
            this.idSymbolCodeComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.idSymbolCodeComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.idSymbolCodeComboBox.Location = new System.Drawing.Point(3, 3);
            this.idSymbolCodeComboBox.Name = "idSymbolCodeComboBox";
            this.idSymbolCodeComboBox.Size = new System.Drawing.Size(100, 20);
            this.idSymbolCodeComboBox.TabIndex = 4;
            // 
            // idQueryQuoteButton
            // 
            this.idQueryQuoteButton.Location = new System.Drawing.Point(106, 3);
            this.idQueryQuoteButton.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.idQueryQuoteButton.Name = "idQueryQuoteButton";
            this.idQueryQuoteButton.Size = new System.Drawing.Size(15, 20);
            this.idQueryQuoteButton.TabIndex = 5;
            this.idQueryQuoteButton.Text = "+";
            this.idQueryQuoteButton.UseVisualStyleBackColor = true;
            this.idQueryQuoteButton.Click += new System.EventHandler(this.idQueryQuoteButton_Click);
            // 
            // idDirectionComboBox
            // 
            this.idDirectionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.idDirectionComboBox.FormattingEnabled = true;
            this.idDirectionComboBox.Location = new System.Drawing.Point(127, 3);
            this.idDirectionComboBox.Name = "idDirectionComboBox";
            this.idDirectionComboBox.Size = new System.Drawing.Size(64, 20);
            this.idDirectionComboBox.TabIndex = 1;
            // 
            // idOffsetComboBox
            // 
            this.idOffsetComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.idOffsetComboBox.FormattingEnabled = true;
            this.idOffsetComboBox.Location = new System.Drawing.Point(197, 3);
            this.idOffsetComboBox.Name = "idOffsetComboBox";
            this.idOffsetComboBox.Size = new System.Drawing.Size(64, 20);
            this.idOffsetComboBox.TabIndex = 6;
            // 
            // idVolumeNumericUpDown
            // 
            this.idVolumeNumericUpDown.Location = new System.Drawing.Point(267, 3);
            this.idVolumeNumericUpDown.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.idVolumeNumericUpDown.Name = "idVolumeNumericUpDown";
            this.idVolumeNumericUpDown.Size = new System.Drawing.Size(72, 21);
            this.idVolumeNumericUpDown.TabIndex = 2;
            this.idVolumeNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // idPriceNumericUpDown
            // 
            this.idPriceNumericUpDown.DecimalPlaces = 2;
            this.idPriceNumericUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.idPriceNumericUpDown.Location = new System.Drawing.Point(345, 3);
            this.idPriceNumericUpDown.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.idPriceNumericUpDown.Name = "idPriceNumericUpDown";
            this.idPriceNumericUpDown.Size = new System.Drawing.Size(72, 21);
            this.idPriceNumericUpDown.TabIndex = 2;
            // 
            // idSendOrderButton
            // 
            this.idSendOrderButton.Location = new System.Drawing.Point(423, 3);
            this.idSendOrderButton.Name = "idSendOrderButton";
            this.idSendOrderButton.Size = new System.Drawing.Size(75, 23);
            this.idSendOrderButton.TabIndex = 3;
            this.idSendOrderButton.Text = "报单";
            this.idSendOrderButton.UseVisualStyleBackColor = true;
            this.idSendOrderButton.Click += new System.EventHandler(this.idSendOrderButton_Click);
            // 
            // idCancelButton
            // 
            this.idCancelButton.Location = new System.Drawing.Point(504, 3);
            this.idCancelButton.Name = "idCancelButton";
            this.idCancelButton.Size = new System.Drawing.Size(75, 23);
            this.idCancelButton.TabIndex = 7;
            this.idCancelButton.Text = "撤单";
            this.idCancelButton.UseVisualStyleBackColor = true;
            this.idCancelButton.Click += new System.EventHandler(this.idCancelButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(688, 394);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "CTP平台测试";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.idOrderPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.idOrderDataGridView)).EndInit();
            this.idPositionPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.idPositionDetailDataGridView)).EndInit();
            this.idTradePage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.idTradeDataGridView)).EndInit();
            this.idSymbolPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.idSymbolDataGridView)).EndInit();
            this.idAmountPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.idAmountDataGridView)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.idQuoteDataGridView)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.idVolumeNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.idPriceNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.TabPage idAmountPage;
    private System.Windows.Forms.TabPage idOrderPage;
    private System.Windows.Forms.DataGridView idOrderDataGridView;
    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem idFileMenuItem;
    private System.Windows.Forms.ToolStripMenuItem idLoginMenuItem;
    private System.Windows.Forms.StatusStrip statusStrip1;
    private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    private System.Windows.Forms.ToolStripMenuItem idTradeQueryMenuItem;
    private System.Windows.Forms.ToolStripMenuItem idTradeQueryAmountMenuItem;
    private System.Windows.Forms.ToolStripMenuItem idTradeQueryOrderMenuItem;
    private System.Windows.Forms.SplitContainer splitContainer1;
    private System.Windows.Forms.DataGridView idAmountDataGridView;
    private System.Windows.Forms.SplitContainer splitContainer2;
    private System.Windows.Forms.NumericUpDown idVolumeNumericUpDown;
    private System.Windows.Forms.ComboBox idDirectionComboBox;
    private System.Windows.Forms.Button idSendOrderButton;
    private System.Windows.Forms.NumericUpDown idPriceNumericUpDown;
    private System.Windows.Forms.ToolStripMenuItem idTradeQueryPositionMenuItem;
    private ComboBox idSymbolCodeComboBox;
    private System.Windows.Forms.ToolStripMenuItem idSystemQueryMenuItem;
    private System.Windows.Forms.ToolStripMenuItem idQuerySymbolMenuItem;
    private System.Windows.Forms.ToolStripMenuItem idTradeQueryTradeMenuItem;
    private System.Windows.Forms.Button idQueryQuoteButton;
    private System.Windows.Forms.TabPage idPositionPage;
    private System.Windows.Forms.DataGridView idPositionDetailDataGridView;
    private System.Windows.Forms.DataGridView idQuoteDataGridView;
    private System.Windows.Forms.ToolStripMenuItem idQueryUserMenuItem;
    private System.Windows.Forms.TabPage idSymbolPage;
    private System.Windows.Forms.DataGridView idSymbolDataGridView;
    private FlowLayoutPanel flowLayoutPanel1;
    private ComboBox idOffsetComboBox;
    private TabPage idTradePage;
    private DataGridView idTradeDataGridView;
    private Button idCancelButton;
  }
}

