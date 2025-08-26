namespace WinFormsApp1
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
            this.listViewMemory = new System.Windows.Forms.ListView();
            this.columnHeaderAddress = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderOffset = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderHex = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderAscii = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderType = new System.Windows.Forms.ColumnHeader();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonKill = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDetails = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSearch = new System.Windows.Forms.ToolStripButton();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStripProcesses = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.killProcessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showDetailsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripMemory = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.hexViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rescanMemoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProcesses)).BeginInit();
            this.panel2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.contextMenuStripProcesses.SuspendLayout();
            this.contextMenuStripMemory.SuspendLayout();
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
            this.splitContainer1.Panel2.Controls.Add(this.listViewMemory);
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
            this.dataGridViewProcesses.ContextMenuStrip = this.contextMenuStripProcesses;
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
            this.panel2.Controls.Add(this.toolStrip1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(578, 0);
            this.panel2.TabIndex = 1;
            // 
            // listViewMemory
            // 
            this.listViewMemory.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderAddress,
            this.columnHeaderOffset,
            this.columnHeaderHex,
            this.columnHeaderAscii,
            this.columnHeaderType});
            this.listViewMemory.ContextMenuStrip = this.contextMenuStripMemory;
            this.listViewMemory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewMemory.FullRowSelect = true;
            this.listViewMemory.Location = new System.Drawing.Point(0, 0);
            this.listViewMemory.Name = "listViewMemory";
            this.listViewMemory.Size = new System.Drawing.Size(578, 580);
            this.listViewMemory.TabIndex = 0;
            this.listViewMemory.UseCompatibleStateImageBehavior = false;
            this.listViewMemory.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderAddress
            // 
            this.columnHeaderAddress.Text = "Adres";
            this.columnHeaderAddress.Width = 120;
            // 
            // columnHeaderOffset
            // 
            this.columnHeaderOffset.Text = "Ofset";
            this.columnHeaderOffset.Width = 80;
            // 
            // columnHeaderHex
            // 
            this.columnHeaderHex.Text = "Hex Değer";
            this.columnHeaderHex.Width = 120;
            // 
            // columnHeaderAscii
            // 
            this.columnHeaderAscii.Text = "ASCII Değer";
            this.columnHeaderAscii.Width = 120;
            // 
            // columnHeaderType
            // 
            this.columnHeaderType.Text = "Tür";
            this.columnHeaderType.Width = 100;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonRefresh,
            this.toolStripButtonKill,
            this.toolStripButtonDetails,
            this.toolStripSeparator1,
            this.toolStripButtonSearch});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(578, 27);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonRefresh
            // 
            this.toolStripButtonRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRefresh.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonRefresh.Image")));
            this.toolStripButtonRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRefresh.Name = "toolStripButtonRefresh";
            this.toolStripButtonRefresh.Size = new System.Drawing.Size(29, 24);
            this.toolStripButtonRefresh.Text = "Yenile";
            this.toolStripButtonRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // toolStripButtonKill
            // 
            this.toolStripButtonKill.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonKill.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonKill.Image")));
            this.toolStripButtonKill.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonKill.Name = "toolStripButtonKill";
            this.toolStripButtonKill.Size = new System.Drawing.Size(29, 24);
            this.toolStripButtonKill.Text = "İşlemi Sonlandır";
            this.toolStripButtonKill.Click += new System.EventHandler(this.KillSelectedProcess);
            // 
            // toolStripButtonDetails
            // 
            this.toolStripButtonDetails.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDetails.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDetails.Image")));
            this.toolStripButtonDetails.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDetails.Name = "toolStripButtonDetails";
            this.toolStripButtonDetails.Size = new System.Drawing.Size(29, 24);
            this.toolStripButtonDetails.Text = "Detaylar";
            this.toolStripButtonDetails.Click += new System.EventHandler(this.ShowProcessDetails);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripButtonSearch
            // 
            this.toolStripButtonSearch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonSearch.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSearch.Image")));
            this.toolStripButtonSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSearch.Name = "toolStripButtonSearch";
            this.toolStripButtonSearch.Size = new System.Drawing.Size(29, 24);
            this.toolStripButtonSearch.Text = "Bellekte Ara";
            this.toolStripButtonSearch.Click += new System.EventHandler(this.SearchInMemory);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // contextMenuStripProcesses
            // 
            this.contextMenuStripProcesses.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripProcesses.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshToolStripMenuItem,
            this.killProcessToolStripMenuItem,
            this.showDetailsToolStripMenuItem});
            this.contextMenuStripProcesses.Name = "contextMenuStripProcesses";
            this.contextMenuStripProcesses.Size = new System.Drawing.Size(171, 76);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(170, 24);
            this.refreshToolStripMenuItem.Text = "Yenile";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // killProcessToolStripMenuItem
            // 
            this.killProcessToolStripMenuItem.Name = "killProcessToolStripMenuItem";
            this.killProcessToolStripMenuItem.Size = new System.Drawing.Size(170, 24);
            this.killProcessToolStripMenuItem.Text = "İşlemi Sonlandır";
            this.killProcessToolStripMenuItem.Click += new System.EventHandler(this.KillSelectedProcess);
            // 
            // showDetailsToolStripMenuItem
            // 
            this.showDetailsToolStripMenuItem.Name = "showDetailsToolStripMenuItem";
            this.showDetailsToolStripMenuItem.Size = new System.Drawing.Size(170, 24);
            this.showDetailsToolStripMenuItem.Text = "Detayları Göster";
            this.showDetailsToolStripMenuItem.Click += new System.EventHandler(this.ShowProcessDetails);
            // 
            // contextMenuStripMemory
            // 
            this.contextMenuStripMemory.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripMemory.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hexViewToolStripMenuItem,
            this.rescanMemoryToolStripMenuItem});
            this.contextMenuStripMemory.Name = "contextMenuStripMemory";
            this.contextMenuStripMemory.Size = new System.Drawing.Size(191, 52);
            // 
            // hexViewToolStripMenuItem
            // 
            this.hexViewToolStripMenuItem.Name = "hexViewToolStripMenuItem";
            this.hexViewToolStripMenuItem.Size = new System.Drawing.Size(190, 24);
            this.hexViewToolStripMenuItem.Text = "Hex Görünüm";
            this.hexViewToolStripMenuItem.Click += new System.EventHandler(this.ToggleHexView);
            // 
            // rescanMemoryToolStripMenuItem
            // 
            this.rescanMemoryToolStripMenuItem.Name = "rescanMemoryToolStripMenuItem";
            this.rescanMemoryToolStripMenuItem.Size = new System.Drawing.Size(190, 24);
            this.rescanMemoryToolStripMenuItem.Text = "Belleği Yeniden Tara";
            this.rescanMemoryToolStripMenuItem.Click += new System.EventHandler(this.RescanMemory);
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
            this.panel2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.contextMenuStripProcesses.ResumeLayout(false);
            this.contextMenuStripMemory.ResumeLayout(false);
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
        private System.Windows.Forms.ListView listViewMemory;
        private System.Windows.Forms.ColumnHeader columnHeaderAddress;
        private System.Windows.Forms.ColumnHeader columnHeaderHex;
        private System.Windows.Forms.ColumnHeader columnHeaderAscii;
        private System.Windows.Forms.ColumnHeader columnHeaderType;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonRefresh;
        private System.Windows.Forms.ToolStripButton toolStripButtonKill;
        private System.Windows.Forms.ToolStripButton toolStripButtonDetails;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonSearch;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripProcesses;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem killProcessToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showDetailsToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripMemory;
        private System.Windows.Forms.ToolStripMenuItem hexViewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rescanMemoryToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader columnHeaderOffset;
    }
}