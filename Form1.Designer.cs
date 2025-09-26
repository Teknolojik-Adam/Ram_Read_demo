namespace WinFormsApp1
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridViewProcesses = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageMemory = new System.Windows.Forms.TabPage();
            this.listViewMemory = new System.Windows.Forms.ListView();
            this.tabPageChanges = new System.Windows.Forms.TabPage();
            this.listViewChanges = new System.Windows.Forms.ListView();
            this.tabPagePerformance = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.chartCpu = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartMemory = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabPageTree = new System.Windows.Forms.TabPage();
            this.treeViewProcesses = new System.Windows.Forms.TreeView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.buttonShowProcessTree = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.buttonSearchMemory = new System.Windows.Forms.Button();
            this.comboBoxSearchType = new System.Windows.Forms.ComboBox();
            this.comboBoxMemoryRegion = new System.Windows.Forms.ComboBox();
            this.textBoxSearchValue = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.buttonStartMonitoring = new System.Windows.Forms.Button();
            this.buttonStopMonitoring = new System.Windows.Forms.Button();
            this.numericUpDownInterval = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.buttonStartPerfMonitoring = new System.Windows.Forms.Button();
            this.buttonStopPerfMonitoring = new System.Windows.Forms.Button();
            this.panel7 = new System.Windows.Forms.Panel();
            this.buttonWriteMemory = new System.Windows.Forms.Button();
            this.textBoxMemoryAddress = new System.Windows.Forms.TextBox();
            this.textBoxMemoryValue = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonShowMemoryMap = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProcesses)).BeginInit();
            this.panel2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageMemory.SuspendLayout();
            this.tabPageChanges.SuspendLayout();
            this.tabPagePerformance.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartCpu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartMemory)).BeginInit();
            this.tabPageTree.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownInterval)).BeginInit();
            this.panel6.SuspendLayout();
            this.panel7.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripProgressBar1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 611);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1082, 26);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(50, 20);
            this.toolStripStatusLabel1.Text = "Hazır.";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 18);
            this.toolStripProgressBar1.Visible = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 31);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            this.splitContainer1.Panel1.Controls.Add(this.dataGridViewProcesses);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel2);
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(1082, 580);
            this.splitContainer1.SplitterDistance = 500;
            this.splitContainer1.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnRefresh);
            this.panel1.Controls.Add(this.txtSearch);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(500, 45);
            this.panel1.TabIndex = 4;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.Location = new System.Drawing.Point(409, 10);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(84, 29);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "Yenile";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.Location = new System.Drawing.Point(55, 13);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(348, 27);
            this.txtSearch.TabIndex = 2;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Ara:";
            // 
            // dataGridViewProcesses
            // 
            this.dataGridViewProcesses.AllowUserToAddRows = false;
            this.dataGridViewProcesses.AllowUserToDeleteRows = false;
            this.dataGridViewProcesses.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewProcesses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewProcesses.Location = new System.Drawing.Point(0, 45);
            this.dataGridViewProcesses.Name = "dataGridViewProcesses";
            this.dataGridViewProcesses.ReadOnly = true;
            this.dataGridViewProcesses.RowHeadersWidth = 51;
            this.dataGridViewProcesses.RowTemplate.Height = 29;
            this.dataGridViewProcesses.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewProcesses.Size = new System.Drawing.Size(500, 535);
            this.dataGridViewProcesses.TabIndex = 0;
            this.dataGridViewProcesses.SelectionChanged += new System.EventHandler(this.dataGridViewProcesses_SelectionChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel7);
            this.panel2.Controls.Add(this.panel6);
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(578, 120);
            this.panel2.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageMemory);
            this.tabControl1.Controls.Add(this.tabPageChanges);
            this.tabControl1.Controls.Add(this.tabPagePerformance);
            this.tabControl1.Controls.Add(this.tabPageTree);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 120);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(578, 460);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPageMemory
            // 
            this.tabPageMemory.Controls.Add(this.listViewMemory);
            this.tabPageMemory.Location = new System.Drawing.Point(4, 29);
            this.tabPageMemory.Name = "tabPageMemory";
            this.tabPageMemory.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMemory.Size = new System.Drawing.Size(570, 427);
            this.tabPageMemory.TabIndex = 0;
            this.tabPageMemory.Text = "Bellek";
            this.tabPageMemory.UseVisualStyleBackColor = true;
            // 
            // listViewMemory
            // 
            this.listViewMemory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewMemory.FullRowSelect = true;
            this.listViewMemory.Location = new System.Drawing.Point(3, 3);
            this.listViewMemory.Name = "listViewMemory";
            this.listViewMemory.Size = new System.Drawing.Size(564, 421);
            this.listViewMemory.TabIndex = 0;
            this.listViewMemory.UseCompatibleStateImageBehavior = false;
            this.listViewMemory.View = System.Windows.Forms.View.Details;
            // 
            // tabPageChanges
            // 
            this.tabPageChanges.Controls.Add(this.listViewChanges);
            this.tabPageChanges.Location = new System.Drawing.Point(4, 29);
            this.tabPageChanges.Name = "tabPageChanges";
            this.tabPageChanges.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageChanges.Size = new System.Drawing.Size(570, 427);
            this.tabPageChanges.TabIndex = 1;
            this.tabPageChanges.Text = "Değişiklikler";
            this.tabPageChanges.UseVisualStyleBackColor = true;
            // 
            // listViewChanges
            // 
            this.listViewChanges.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewChanges.FullRowSelect = true;
            this.listViewChanges.Location = new System.Drawing.Point(3, 3);
            this.listViewChanges.Name = "listViewChanges";
            this.listViewChanges.Size = new System.Drawing.Size(564, 421);
            this.listViewChanges.TabIndex = 0;
            this.listViewChanges.UseCompatibleStateImageBehavior = false;
            this.listViewChanges.View = System.Windows.Forms.View.Details;
            // 
            // tabPagePerformance
            // 
            this.tabPagePerformance.Controls.Add(this.splitContainer2);
            this.tabPagePerformance.Location = new System.Drawing.Point(4, 29);
            this.tabPagePerformance.Name = "tabPagePerformance";
            this.tabPagePerformance.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePerformance.Size = new System.Drawing.Size(570, 427);
            this.tabPagePerformance.TabIndex = 2;
            this.tabPagePerformance.Text = "Performans";
            this.tabPagePerformance.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.chartCpu);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.chartMemory);
            this.splitContainer2.Size = new System.Drawing.Size(564, 421);
            this.splitContainer2.SplitterDistance = 210;
            this.splitContainer2.TabIndex = 0;
            // 
            // chartCpu
            // 
            this.chartCpu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartCpu.Location = new System.Drawing.Point(0, 0);
            this.chartCpu.Name = "chartCpu";
            this.chartCpu.Size = new System.Drawing.Size(564, 210);
            this.chartCpu.TabIndex = 0;
            this.chartCpu.Text = "CPU Kullanımı";
            // 
            // chartMemory
            // 
            this.chartMemory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartMemory.Location = new System.Drawing.Point(0, 0);
            this.chartMemory.Name = "chartMemory";
            this.chartMemory.Size = new System.Drawing.Size(564, 207);
            this.chartMemory.TabIndex = 0;
            this.chartMemory.Text = "Bellek Kullanımı";
            // 
            // tabPageTree
            // 
            this.tabPageTree.Controls.Add(this.treeViewProcesses);
            this.tabPageTree.Location = new System.Drawing.Point(4, 29);
            this.tabPageTree.Name = "tabPageTree";
            this.tabPageTree.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTree.Size = new System.Drawing.Size(570, 427);
            this.tabPageTree.TabIndex = 3;
            this.tabPageTree.Text = "İşlem Ağacı";
            this.tabPageTree.UseVisualStyleBackColor = true;
            // 
            // treeViewProcesses
            // 
            this.treeViewProcesses.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewProcesses.Location = new System.Drawing.Point(3, 3);
            this.treeViewProcesses.Name = "treeViewProcesses";
            this.treeViewProcesses.Size = new System.Drawing.Size(564, 421);
            this.treeViewProcesses.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.buttonShowProcessTree);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 96);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(578, 24);
            this.panel3.TabIndex = 4;
            // 
            // buttonShowProcessTree
            // 
            this.buttonShowProcessTree.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonShowProcessTree.Location = new System.Drawing.Point(493, 0);
            this.buttonShowProcessTree.Name = "buttonShowProcessTree";
            this.buttonShowProcessTree.Size = new System.Drawing.Size(85, 24);
            this.buttonShowProcessTree.TabIndex = 0;
            this.buttonShowProcessTree.Text = "Ağacı Göster";
            this.buttonShowProcessTree.UseVisualStyleBackColor = true;
            this.buttonShowProcessTree.Click += new System.EventHandler(this.buttonShowProcessTree_Click);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.buttonSearchMemory);
            this.panel4.Controls.Add(this.comboBoxSearchType);
            this.panel4.Controls.Add(this.comboBoxMemoryRegion);
            this.panel4.Controls.Add(this.textBoxSearchValue);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(578, 48);
            this.panel4.TabIndex = 0;
            // 
            // buttonSearchMemory
            // 
            this.buttonSearchMemory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSearchMemory.Location = new System.Drawing.Point(493, 12);
            this.buttonSearchMemory.Name = "buttonSearchMemory";
            this.buttonSearchMemory.Size = new System.Drawing.Size(75, 29);
            this.buttonSearchMemory.TabIndex = 5;
            this.buttonSearchMemory.Text = "Ara";
            this.buttonSearchMemory.UseVisualStyleBackColor = true;
            this.buttonSearchMemory.Click += new System.EventHandler(this.buttonSearchMemory_Click);
            // 
            // comboBoxSearchType
            // 
            this.comboBoxSearchType.FormattingEnabled = true;
            this.comboBoxSearchType.Location = new System.Drawing.Point(55, 13);
            this.comboBoxSearchType.Name = "comboBoxSearchType";
            this.comboBoxSearchType.Size = new System.Drawing.Size(121, 28);
            this.comboBoxSearchType.TabIndex = 4;
            // 
            // comboBoxMemoryRegion
            // 
            this.comboBoxMemoryRegion.FormattingEnabled = true;
            this.comboBoxMemoryRegion.Location = new System.Drawing.Point(182, 13);
            this.comboBoxMemoryRegion.Name = "comboBoxMemoryRegion";
            this.comboBoxMemoryRegion.Size = new System.Drawing.Size(121, 28);
            this.comboBoxMemoryRegion.TabIndex = 3;
            // 
            // textBoxSearchValue
            // 
            this.textBoxSearchValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSearchValue.Location = new System.Drawing.Point(309, 14);
            this.textBoxSearchValue.Name = "textBoxSearchValue";
            this.textBoxSearchValue.Size = new System.Drawing.Size(178, 27);
            this.textBoxSearchValue.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Ara:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 20);
            this.label3.TabIndex = 0;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.buttonStartMonitoring);
            this.panel5.Controls.Add(this.buttonStopMonitoring);
            this.panel5.Controls.Add(this.numericUpDownInterval);
            this.panel5.Controls.Add(this.label4);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 48);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(578, 24);
            this.panel5.TabIndex = 1;
            // 
            // buttonStartMonitoring
            // 
            this.buttonStartMonitoring.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonStartMonitoring.Location = new System.Drawing.Point(408, 0);
            this.buttonStartMonitoring.Name = "buttonStartMonitoring";
            this.buttonStartMonitoring.Size = new System.Drawing.Size(85, 24);
            this.buttonStartMonitoring.TabIndex = 3;
            this.buttonStartMonitoring.Text = "İzlemeyi Başlat";
            this.buttonStartMonitoring.UseVisualStyleBackColor = true;
            this.buttonStartMonitoring.Click += new System.EventHandler(this.buttonStartMonitoring_Click);
            // 
            // buttonStopMonitoring
            // 
            this.buttonStopMonitoring.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonStopMonitoring.Enabled = false;
            this.buttonStopMonitoring.Location = new System.Drawing.Point(493, 0);
            this.buttonStopMonitoring.Name = "buttonStopMonitoring";
            this.buttonStopMonitoring.Size = new System.Drawing.Size(85, 24);
            this.buttonStopMonitoring.TabIndex = 2;
            this.buttonStopMonitoring.Text = "Durdur";
            this.buttonStopMonitoring.UseVisualStyleBackColor = true;
            this.buttonStopMonitoring.Click += new System.EventHandler(this.buttonStopMonitoring_Click);
            // 
            // numericUpDownInterval
            // 
            this.numericUpDownInterval.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownInterval.Location = new System.Drawing.Point(150, 1);
            this.numericUpDownInterval.Name = "numericUpDownInterval";
            this.numericUpDownInterval.Size = new System.Drawing.Size(120, 27);
            this.numericUpDownInterval.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(132, 20);
            this.label4.TabIndex = 0;
            this.label4.Text = "İzleme Aralığı (ms):";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.buttonStartPerfMonitoring);
            this.panel6.Controls.Add(this.buttonStopPerfMonitoring);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 72);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(578, 24);
            this.panel6.TabIndex = 2;
            // 
            // buttonStartPerfMonitoring
            // 
            this.buttonStartPerfMonitoring.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonStartPerfMonitoring.Location = new System.Drawing.Point(408, 0);
            this.buttonStartPerfMonitoring.Name = "buttonStartPerfMonitoring";
            this.buttonStartPerfMonitoring.Size = new System.Drawing.Size(85, 24);
            this.buttonStartPerfMonitoring.TabIndex = 1;
            this.buttonStartPerfMonitoring.Text = "Performansı İzle";
            this.buttonStartPerfMonitoring.UseVisualStyleBackColor = true;
            this.buttonStartPerfMonitoring.Click += new System.EventHandler(this.buttonStartPerfMonitoring_Click);
            // 
            // buttonStopPerfMonitoring
            // 
            this.buttonStopPerfMonitoring.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonStopPerfMonitoring.Enabled = false;
            this.buttonStopPerfMonitoring.Location = new System.Drawing.Point(493, 0);
            this.buttonStopPerfMonitoring.Name = "buttonStopPerfMonitoring";
            this.buttonStopPerfMonitoring.Size = new System.Drawing.Size(85, 24);
            this.buttonStopPerfMonitoring.TabIndex = 0;
            this.buttonStopPerfMonitoring.Text = "Durdur";
            this.buttonStopPerfMonitoring.UseVisualStyleBackColor = true;
            this.buttonStopPerfMonitoring.Click += new System.EventHandler(this.buttonStopPerfMonitoring_Click);
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.buttonWriteMemory);
            this.panel7.Controls.Add(this.textBoxMemoryAddress);
            this.panel7.Controls.Add(this.textBoxMemoryValue);
            this.panel7.Controls.Add(this.label5);
            this.panel7.Controls.Add(this.label6);
            this.panel7.Controls.Add(this.buttonShowMemoryMap);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(0, 24);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(578, 72);
            this.panel7.TabIndex = 3;
            // 
            // buttonWriteMemory
            // 
            this.buttonWriteMemory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonWriteMemory.Location = new System.Drawing.Point(493, 41);
            this.buttonWriteMemory.Name = "buttonWriteMemory";
            this.buttonWriteMemory.Size = new System.Drawing.Size(75, 29);
            this.buttonWriteMemory.TabIndex = 5;
            this.buttonWriteMemory.Text = "Yaz";
            this.buttonWriteMemory.UseVisualStyleBackColor = true;
            this.buttonWriteMemory.Click += new System.EventHandler(this.buttonWriteMemory_Click);
            // 
            // textBoxMemoryAddress
            // 
            this.textBoxMemoryAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMemoryAddress.Location = new System.Drawing.Point(150, 13);
            this.textBoxMemoryAddress.Name = "textBoxMemoryAddress";
            this.textBoxMemoryAddress.Size = new System.Drawing.Size(337, 27);
            this.textBoxMemoryAddress.TabIndex = 4;
            // 
            // textBoxMemoryValue
            // 
            this.textBoxMemoryValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMemoryValue.Location = new System.Drawing.Point(150, 43);
            this.textBoxMemoryValue.Name = "textBoxMemoryValue";
            this.textBoxMemoryValue.Size = new System.Drawing.Size(337, 27);
            this.textBoxMemoryValue.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(132, 20);
            this.label5.TabIndex = 2;
            this.label5.Text = "Bellek Adresi (Hex):";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 46);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(118, 20);
            this.label6.TabIndex = 1;
            this.label6.Text = "Değer (Hex):";
            // 
            // buttonShowMemoryMap
            // 
            this.buttonShowMemoryMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonShowMemoryMap.Location = new System.Drawing.Point(408, 12);
            this.buttonShowMemoryMap.Name = "buttonShowMemoryMap";
            this.buttonShowMemoryMap.Size = new System.Drawing.Size(170, 29);
            this.buttonShowMemoryMap.TabIndex = 0;
            this.buttonShowMemoryMap.Text = "Bellek Haritasını Göster";
            this.buttonShowMemoryMap.UseVisualStyleBackColor = true;
            this.buttonShowMemoryMap.Click += new System.EventHandler(this.buttonShowMemoryMap_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1082, 637);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gelişmiş Süreç Analiz Aracı";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProcesses)).EndInit();
            this.panel2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPageMemory.ResumeLayout(false);
            this.tabPageChanges.ResumeLayout(false);
            this.tabPagePerformance.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartCpu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartMemory)).EndInit();
            this.tabPageTree.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownInterval)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridViewProcesses;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageMemory;
        private System.Windows.Forms.ListView listViewMemory;
        private System.Windows.Forms.TabPage tabPageChanges;
        private System.Windows.Forms.ListView listViewChanges;
        private System.Windows.Forms.TabPage tabPagePerformance;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartCpu;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartMemory;
        private System.Windows.Forms.TabPage tabPageTree;
        private System.Windows.Forms.TreeView treeViewProcesses;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button buttonShowProcessTree;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button buttonSearchMemory;
        private System.Windows.Forms.ComboBox comboBoxSearchType;
        private System.Windows.Forms.ComboBox comboBoxMemoryRegion;
        private System.Windows.Forms.TextBox textBoxSearchValue;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button buttonStartMonitoring;
        private System.Windows.Forms.Button buttonStopMonitoring;
        private System.Windows.Forms.NumericUpDown numericUpDownInterval;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button buttonStartPerfMonitoring;
        private System.Windows.Forms.Button buttonStopPerfMonitoring;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Button buttonWriteMemory;
        private System.Windows.Forms.TextBox textBoxMemoryAddress;
        private System.Windows.Forms.TextBox textBoxMemoryValue;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button buttonShowMemoryMap;
    }
}