using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        // P/Invoke: Windows API'sinden bellek okuma fonksiyonu
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            byte[] lpBuffer,
            int dwSize,
            out int lpNumberOfBytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr OpenProcess(
            ProcessAccessFlags processAccess,
            bool bInheritHandle,
            int processId);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool CloseHandle(IntPtr hObject);

        [Flags]
        public enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VirtualMemoryOperation = 0x00000008,
            VirtualMemoryRead = 0x00000010,
            VirtualMemoryWrite = 0x00000020,
            DuplicateHandle = 0x00000040,
            CreateProcess = 0x00000080,
            SetQuota = 0x00000100,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            QueryLimitedInformation = 0x00001000,
            Synchronize = 0x00100000
        }

        private Process selectedProcess;
        private IntPtr processHandle = IntPtr.Zero;

        public Form1()
        {
            InitializeComponent();
            SetupDataGridView();
            SetupListView();
            // The SetupToolbar method is commented out because it references non-existent resources.
            // SetupToolbar(); 
            SetupContextMenus();
        }

        private void SetupDataGridView()
        {
            dataGridViewProcesses.AutoGenerateColumns = false;
            dataGridViewProcesses.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "PID", Width = 60 });
            dataGridViewProcesses.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ProcessName", HeaderText = "Ýþlem Adý", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dataGridViewProcesses.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MemoryUsage", HeaderText = "Bellek (MB)", Width = 100 });
            dataGridViewProcesses.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Priority", HeaderText = "Öncelik", Width = 80 });
            dataGridViewProcesses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewProcesses.MultiSelect = false;

            // Görsel iyileþtirmeler
            dataGridViewProcesses.EnableHeadersVisualStyles = false;
            dataGridViewProcesses.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dataGridViewProcesses.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridViewProcesses.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dataGridViewProcesses.RowsDefaultCellStyle.BackColor = Color.LightGray;
            dataGridViewProcesses.AlternatingRowsDefaultCellStyle.BackColor = Color.WhiteSmoke;
        }

        private void SetupListView()
        {
            // Bellek görünümü için sütunlar
            listViewMemory.Columns.Add("Adres", 120);
            listViewMemory.Columns.Add("Ofset", 80);
            listViewMemory.Columns.Add("Hex Deðer", 120);
            listViewMemory.Columns.Add("ASCII Deðer", 120);
            listViewMemory.Columns.Add("Tür", 100);

            // Görsel iyileþtirmeler
            listViewMemory.OwnerDraw = true;
            listViewMemory.FullRowSelect = true;
            listViewMemory.View = View.Details;
            listViewMemory.DrawColumnHeader += ListViewMemory_DrawColumnHeader;
            listViewMemory.DrawSubItem += ListViewMemory_DrawSubItem;
        }

        private void ListViewMemory_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            using (var brush = new SolidBrush(Color.Navy))
            {
                e.Graphics.FillRectangle(brush, e.Bounds);
            }
            using (var brush = new SolidBrush(Color.White))
            {
                e.Graphics.DrawString(e.Header.Text, e.Font, brush, e.Bounds);
            }
        }

        private void ListViewMemory_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawBackground();

            if (e.Item.Selected)
            {
                e.Graphics.FillRectangle(Brushes.LightBlue, e.Bounds);
            }

            using (var brush = new SolidBrush(e.ItemIndex % 2 == 0 ? Color.Black : Color.DarkBlue))
            {
                e.Graphics.DrawString(e.SubItem.Text, listViewMemory.Font, brush, e.Bounds);
            }

            e.DrawFocusRectangle(e.Bounds);
        }

        private void SetupToolbar()
        {
            // Araç çubuðu oluþtur
            var toolbar = new ToolStrip();
            toolbar.ImageScalingSize = new Size(24, 24);

            var refreshBtn = new ToolStripButton("Yenile", null, (s, e) => btnRefresh_Click(s, e));
            var killBtn = new ToolStripButton("Ýþlemi Sonlandýr", null, (s, e) => KillSelectedProcess(s, e));
            var detailsBtn = new ToolStripButton("Detaylar", null, (s, e) => ShowProcessDetails(s, e));
            var searchMemBtn = new ToolStripButton("Bellekte Ara", null, (s, e) => SearchInMemory(s, e));

            toolbar.Items.Add(refreshBtn);
            toolbar.Items.Add(killBtn);
            toolbar.Items.Add(detailsBtn);
            toolbar.Items.Add(new ToolStripSeparator());
            toolbar.Items.Add(searchMemBtn);

            // Toolbar'ý formun üstüne ekle
            this.Controls.Add(toolbar);
            toolbar.Dock = DockStyle.Top;

            // Diðer kontrollerin konumunu ayarla
            splitContainer1.Top = toolbar.Height;
            splitContainer1.Height = this.Height - toolbar.Height - statusStrip1.Height;
        }

        private void SetupContextMenus()
        {
            // DataGridView için sað týk menüsü
            var gridContextMenu = new ContextMenuStrip();
            gridContextMenu.Items.Add("Yenile", null, (s, e) => btnRefresh_Click(s, e));
            gridContextMenu.Items.Add("Ýþlemi Sonlandýr", null, (s, e) => KillSelectedProcess(s, e));
            gridContextMenu.Items.Add("Detaylarý Göster", null, (s, e) => ShowProcessDetails(s, e));
            dataGridViewProcesses.ContextMenuStrip = gridContextMenu;

            // ListView için sað týk menüsü
            var listContextMenu = new ContextMenuStrip();
            listContextMenu.Items.Add("Hex Görünüm", null, (s, e) => ToggleHexView(s, e));
            listContextMenu.Items.Add("Belleði Yeniden Tara", null, (s, e) => RescanMemory(s, e));
            listViewMemory.ContextMenuStrip = listContextMenu;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await RefreshProcessListAsync();
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await RefreshProcessListAsync();
        }

        private async Task RefreshProcessListAsync()
        {
            UpdateStatus("Ýþlemler listeleniyor...", true);
            btnRefresh.Enabled = false;

            try
            {
                var processes = await Task.Run(() =>
                {
                    return Process.GetProcesses()
                        .Select(p =>
                        {
                            try
                            {
                                return new
                                {
                                    p.Id,
                                    p.ProcessName,
                                    MemoryUsage = (p.WorkingSet64 / 1024.0 / 1024.0).ToString("N2"),
                                    Priority = p.BasePriority,
                                    ProcessObject = p
                                };
                            }
                            catch
                            {
                                return null;
                            }
                        })
                        .Where(p => p != null)
                        .OrderBy(p => p.ProcessName)
                        .ToList();
                });

                dataGridViewProcesses.DataSource = processes;
                UpdateStatus($"Ýþlemler yüklendi. Toplam: {processes.Count}");
            }
            catch (Exception ex)
            {
                UpdateStatus($"Hata: {ex.Message}");
            }
            finally
            {
                btnRefresh.Enabled = true;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            var filter = txtSearch.Text.ToLower();
            if (string.IsNullOrEmpty(filter))
            {
                (dataGridViewProcesses.DataSource as BindingSource)?.RemoveFilter();
            }
            else
            {
                var bs = new BindingSource { DataSource = dataGridViewProcesses.DataSource };
                bs.Filter = $"ProcessName LIKE '%{filter}%' OR Id LIKE '%{filter}%'";
                dataGridViewProcesses.DataSource = bs;
            }
        }

        private async void dataGridViewProcesses_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewProcesses.SelectedRows.Count == 0) return;

            var selectedRow = dataGridViewProcesses.SelectedRows[0];
            var processId = (int)selectedRow.Cells[0].Value;

            try
            {
                selectedProcess = Process.GetProcessById(processId);
                await AnalyzeProcessMemoryAsync(selectedProcess);
            }
            catch (ArgumentException)
            {
                UpdateStatus($"Hata: {processId} ID'li iþlem artýk çalýþmýyor.");
            }
            catch (Exception ex)
            {
                UpdateStatus($"Hata: {ex.Message}");
            }
        }

        private async Task AnalyzeProcessMemoryAsync(Process process)
        {
            listViewMemory.Items.Clear();
            UpdateStatus($"{process.ProcessName} belleði analiz ediliyor...", true);

            // Önceki process handle'ý kapat
            if (processHandle != IntPtr.Zero)
            {
                CloseHandle(processHandle);
                processHandle = IntPtr.Zero;
            }

            try
            {
                // Process handle al
                processHandle = OpenProcess(
                    ProcessAccessFlags.VirtualMemoryRead | ProcessAccessFlags.QueryInformation,
                    false,
                    process.Id);

                if (processHandle == IntPtr.Zero)
                {
                    var error = Marshal.GetLastWin32Error();
                    UpdateStatus($"Hata: Process açýlamadý. Hata kodu: {error}");
                    return;
                }

                var memoryItems = await Task.Run(() =>
                {
                    var items = new List<ListViewItem>();
                    try
                    {
                        IntPtr baseAddress = process.MainModule.BaseAddress;
                        byte[] buffer = new byte[256];
                        int bytesRead;

                        if (ReadProcessMemory(processHandle, baseAddress, buffer, buffer.Length, out bytesRead))
                        {
                            for (int i = 0; i < bytesRead; i += 8)
                            {
                                if (i + 8 > bytesRead) break;

                                IntPtr currentAddress = IntPtr.Add(baseAddress, i);
                                string hexValue = BitConverter.ToString(buffer, i, Math.Min(8, bytesRead - i)).Replace("-", " ");
                                string asciiValue = Encoding.ASCII.GetString(buffer, i, Math.Min(8, bytesRead - i));

                                // Özel karakterleri temizle
                                asciiValue = new string(asciiValue.Select(c => char.IsControl(c) ? '.' : c).ToArray());

                                string type = GetMemoryType(buffer, i);

                                var item = new ListViewItem(currentAddress.ToString("X16"));
                                item.SubItems.Add(i.ToString("X4"));
                                item.SubItems.Add(hexValue);
                                item.SubItems.Add(asciiValue);
                                item.SubItems.Add(type);

                                items.Add(item);
                            }
                        }
                        else
                        {
                            var error = new Win32Exception(Marshal.GetLastWin32Error());
                            var errorItem = new ListViewItem("Hata");
                            errorItem.SubItems.Add($"");
                            errorItem.SubItems.Add($"Bellek okunamadý: {error.Message}");
                            items.Add(errorItem);
                        }
                    }
                    catch (Exception ex)
                    {
                        var errorItem = new ListViewItem("Hata");
                        errorItem.SubItems.Add($"");
                        errorItem.SubItems.Add($"Genel Hata: {ex.Message}");
                        items.Add(errorItem);
                    }
                    return items;
                });

                listViewMemory.BeginUpdate();
                listViewMemory.Items.AddRange(memoryItems.ToArray());
                listViewMemory.EndUpdate();

                UpdateStatus($"{process.ProcessName} bellek analizi tamamlandý. {memoryItems.Count} kayýt.");
            }
            catch (Exception ex)
            {
                UpdateStatus($"Hata: {ex.Message}");
            }
        }

        private string GetMemoryType(byte[] buffer, int index)
        {
            if (index + 4 <= buffer.Length)
            {
                try
                {
                    int intValue = BitConverter.ToInt32(buffer, index);
                    return $"Int32: {intValue}";
                }
                catch
                {
                    return "Binary";
                }
            }
            return "Binary";
        }

        private void UpdateStatus(string message, bool showProgress = false)
        {
            if (statusStrip1.InvokeRequired)
            {
                statusStrip1.Invoke(new Action(() =>
                {
                    toolStripStatusLabel1.Text = message;
                    toolStripProgressBar1.Visible = showProgress;
                    toolStripProgressBar1.Style = showProgress ? ProgressBarStyle.Marquee : ProgressBarStyle.Continuous;
                }));
            }
            else
            {
                toolStripStatusLabel1.Text = message;
                toolStripProgressBar1.Visible = showProgress;
                toolStripProgressBar1.Style = showProgress ? ProgressBarStyle.Marquee : ProgressBarStyle.Continuous;
            }
        }

        private void KillSelectedProcess(object sender, EventArgs e)
        {
            if (dataGridViewProcesses.SelectedRows.Count == 0) return;

            var selectedRow = dataGridViewProcesses.SelectedRows[0];
            var processId = (int)selectedRow.Cells[0].Value;
            var processName = selectedRow.Cells[1].Value.ToString();

            var result = MessageBox.Show(
                $"{processName} (PID: {processId}) iþlemini sonlandýrmak istediðinize emin misiniz?",
                "Ýþlem Sonlandýrma",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    var process = Process.GetProcessById(processId);
                    process.Kill();
                    UpdateStatus($"{processName} iþlemi sonlandýrýldý.");
                    // Listeyi yenile
                    _ = RefreshProcessListAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ýþlem sonlandýrýlamadý: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ShowProcessDetails(object sender, EventArgs e)
        {
            if (dataGridViewProcesses.SelectedRows.Count == 0) return;

            var selectedRow = dataGridViewProcesses.SelectedRows[0];
            var processId = (int)selectedRow.Cells[0].Value;

            try
            {
                var process = Process.GetProcessById(processId);

                var detailsForm = new Form
                {
                    Text = $"{process.ProcessName} Detaylarý - PID: {process.Id}",
                    Size = new Size(500, 400),
                    StartPosition = FormStartPosition.CenterParent
                };

                var propertyGrid = new PropertyGrid
                {
                    Dock = DockStyle.Fill,
                    SelectedObject = process,
                    ToolbarVisible = false
                };

                detailsForm.Controls.Add(propertyGrid);
                detailsForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Detaylar gösterilemedi: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SearchInMemory(object sender, EventArgs e)
        {
            if (selectedProcess == null || selectedProcess.HasExited)
            {
                MessageBox.Show("Önce bir iþlem seçin.", "Uyarý", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var searchForm = new Form
            {
                Text = "Bellekte Ara",
                Size = new Size(300, 150),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var lblSearch = new Label { Text = "Aranacak deðer:", Left = 10, Top = 15, Width = 100 };
            var txtSearchValue = new TextBox { Left = 120, Top = 12, Width = 150 };
            var btnSearch = new Button { Text = "Ara", Left = 120, Top = 50, Width = 75 };
            var btnCancel = new Button { Text = "Ýptal", Left = 205, Top = 50, Width = 75 };

            btnSearch.Click += (s, ev) =>
            {
                // Arama iþlemini burada gerçekleþtir
                MessageBox.Show($"'{txtSearchValue.Text}' deðeri aranýyor...", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                searchForm.Close();
            };

            btnCancel.Click += (s, ev) => searchForm.Close();

            searchForm.Controls.AddRange(new Control[] { lblSearch, txtSearchValue, btnSearch, btnCancel });
            searchForm.ShowDialog();
        }

        private void ToggleHexView(object sender, EventArgs e)
        {
            // Hex görünümü deðiþtirme iþlevi
            MessageBox.Show("Hex görünümü deðiþtirilecek", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void RescanMemory(object sender, EventArgs e)
        {
            if (selectedProcess != null && !selectedProcess.HasExited)
            {
                _ = AnalyzeProcessMemoryAsync(selectedProcess);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Process handle'ý temizle
            if (processHandle != IntPtr.Zero)
            {
                CloseHandle(processHandle);
                processHandle = IntPtr.Zero;
            }

            base.OnFormClosing(e);
        }
    }
}