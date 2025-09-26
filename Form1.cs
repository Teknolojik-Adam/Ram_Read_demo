using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        // P/Invoke declarations
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

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool VirtualQueryEx(
            IntPtr hProcess,
            IntPtr lpAddress,
            out MEMORY_BASIC_INFORMATION lpBuffer,
            uint dwLength);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool WriteProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            byte[] lpBuffer,
            int nSize,
            out int lpNumberOfBytesWritten);

        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORY_BASIC_INFORMATION
        {
            public IntPtr BaseAddress;
            public IntPtr AllocationBase;
            public uint AllocationProtect;
            public IntPtr RegionSize;
            public uint State;
            public uint Protect;
            public uint Type;
        }

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

        // Enums for new features
        public enum SearchType
        {
            String,
            Integer,
            Hex,
            ByteArray
        }

        public enum MemoryRegion
        {
            All,
            Heap,
            Stack,
            Modules
        }

        [Flags]
        public enum MemoryProtection : uint
        {
            NoAccess = 0x01,
            ReadOnly = 0x02,
            ReadWrite = 0x04,
            WriteCopy = 0x08,
            Execute = 0x10,
            ExecuteRead = 0x20,
            ExecuteReadWrite = 0x40,
            ExecuteWriteCopy = 0x80
        }

        // New structures
        public struct MemoryRegionInfo
        {
            public IntPtr BaseAddress;
            public IntPtr RegionSize;
            public MemoryProtection Protection;
            public string State;
            public string Type;
        }

        public class PerformanceData
        {
            public DateTime Timestamp { get; set; }
            public float CpuUsage { get; set; }
            public long MemoryUsage { get; set; }
        }

        public class MemoryChange
        {
            public IntPtr Address { get; set; }
            public byte[] OldValue { get; set; }
            public byte[] NewValue { get; set; }
            public DateTime Timestamp { get; set; }
        }

        private Process selectedProcess;
        private IntPtr processHandle = IntPtr.Zero;
        private CancellationTokenSource monitoringCts;
        private CancellationTokenSource perfMonitoringCts;

        public Form1()
        {
            InitializeComponent();
            SetupDataGridView();
            SetupListView();
            SetupCharts();
            SetupTreeView();
            SetupControls();
            SetupContextMenus();
        }

        private void SetupDataGridView()
        {
            dataGridViewProcesses.AutoGenerateColumns = false;
            dataGridViewProcesses.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "PID", Width = 60 });
            dataGridViewProcesses.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ProcessName", HeaderText = "��lem Ad�", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dataGridViewProcesses.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MemoryUsage", HeaderText = "Bellek (MB)", Width = 100 });
            dataGridViewProcesses.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Priority", HeaderText = "�ncelik", Width = 80 });
            dataGridViewProcesses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewProcesses.MultiSelect = false;

            // Visual improvements
            dataGridViewProcesses.EnableHeadersVisualStyles = false;
            dataGridViewProcesses.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dataGridViewProcesses.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridViewProcesses.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dataGridViewProcesses.RowsDefaultCellStyle.BackColor = Color.LightGray;
            dataGridViewProcesses.AlternatingRowsDefaultCellStyle.BackColor = Color.WhiteSmoke;
        }

        private void SetupListView()
        {
            listViewMemory.Columns.Add("Adres", 120);
            listViewMemory.Columns.Add("Ofset", 80);
            listViewMemory.Columns.Add("Hex De�er", 120);
            listViewMemory.Columns.Add("ASCII De�er", 120);
            listViewMemory.Columns.Add("T�r", 100);

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

        private void SetupCharts()
        {
            try
            {
                // CPU Chart
                if (chartCpu != null)
                {
                    chartCpu.Series.Clear();

                    // ChartArea kontrol�
                    if (chartCpu.ChartAreas.Count == 0)
                    {
                        chartCpu.ChartAreas.Add(new ChartArea());
                    }

                    var cpuSeries = new Series("CPU");
                    cpuSeries.ChartType = SeriesChartType.Line;
                    cpuSeries.Color = Color.Red;
                    chartCpu.Series.Add(cpuSeries);

                    if (chartCpu.ChartAreas.Count > 0)
                    {
                        chartCpu.ChartAreas[0].AxisX.Title = "Zaman";
                        chartCpu.ChartAreas[0].AxisY.Title = "CPU Kullan�m� (%)";
                        chartCpu.ChartAreas[0].AxisY.Maximum = 100;
                    }
                }

                // Memory Chart
                if (chartMemory != null)
                {
                    chartMemory.Series.Clear();

                    // ChartArea kontrol�
                    if (chartMemory.ChartAreas.Count == 0)
                    {
                        chartMemory.ChartAreas.Add(new ChartArea());
                    }

                    var memorySeries = new Series("Memory");
                    memorySeries.ChartType = SeriesChartType.Line;
                    memorySeries.Color = Color.Blue;
                    chartMemory.Series.Add(memorySeries);

                    if (chartMemory.ChartAreas.Count > 0)
                    {
                        chartMemory.ChartAreas[0].AxisX.Title = "Zaman";
                        chartMemory.ChartAreas[0].AxisY.Title = "Bellek Kullan�m� (MB)";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Grafikler ayarlan�rken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupTreeView()
        {
            treeViewProcesses.HideSelection = false;
            treeViewProcesses.AfterSelect += TreeViewProcesses_AfterSelect;
        }

        private void SetupControls()
        {
            // Search controls
            comboBoxSearchType.Items.AddRange(Enum.GetNames(typeof(SearchType)));
            comboBoxSearchType.SelectedIndex = 0;

            comboBoxMemoryRegion.Items.AddRange(Enum.GetNames(typeof(MemoryRegion)));
            comboBoxMemoryRegion.SelectedIndex = 0;

            // Monitoring controls
            numericUpDownInterval.Minimum = 100;
            numericUpDownInterval.Maximum = 5000;
            numericUpDownInterval.Value = 1000;
            numericUpDownInterval.Increment = 100;

            // Memory editor
            listViewChanges.Columns.Add("Adres", 120);
            listViewChanges.Columns.Add("Eski De�er", 150);
            listViewChanges.Columns.Add("Yeni De�er", 150);
            listViewChanges.Columns.Add("Zaman Damgas�", 120);
            listViewChanges.View = View.Details;
            listViewChanges.FullRowSelect = true;
        }

        private void SetupContextMenus()
        {
            // DataGridView context menu
            var gridContextMenu = new ContextMenuStrip();
            gridContextMenu.Items.Add("Yenile", null, (s, e) => btnRefresh_Click(s, e));
            gridContextMenu.Items.Add("��lemi Sonland�r", null, (s, e) => KillSelectedProcess(s, e));
            gridContextMenu.Items.Add("Detaylar� G�ster", null, (s, e) => ShowProcessDetails(s, e));
            dataGridViewProcesses.ContextMenuStrip = gridContextMenu;

            // ListView context menu
            var listContextMenu = new ContextMenuStrip();
            listContextMenu.Items.Add("Hex G�r�n�m", null, (s, e) => ToggleHexView(s, e));
            listContextMenu.Items.Add("Belle�i Yeniden Tara", null, (s, e) => RescanMemory(s, e));
            listContextMenu.Items.Add("Adresi Kopyala", null, (s, e) => CopyAddress(s, e));
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
            UpdateStatus("��lemler listeleniyor...", true);
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
                UpdateStatus($"��lemler y�klendi. Toplam: {processes.Count}");
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

            // H�cre say�s�n� kontrol et
            if (selectedRow.Cells.Count < 1) return;

            // H�cre de�erinin null olup olmad���n� kontrol et
            if (selectedRow.Cells[0].Value == null) return;

            // De�erin integer olup olmad���n� kontrol et
            if (!int.TryParse(selectedRow.Cells[0].Value.ToString(), out int processId)) return;

            try
            {
                selectedProcess = Process.GetProcessById(processId);
                await AnalyzeProcessMemoryAsync(selectedProcess);
            }
            catch (ArgumentException)
            {
                UpdateStatus($"Hata: {processId} ID'li i�lem art�k �al��m�yor.");
            }
            catch (Exception ex)
            {
                UpdateStatus($"Hata: {ex.Message}");
            }
        }

        private async Task AnalyzeProcessMemoryAsync(Process process)
        {
            listViewMemory.Items.Clear();
            UpdateStatus($"{process.ProcessName} belle�i analiz ediliyor...", true);

            // �nceki process handle'� kapat
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
                    UpdateStatus($"Hata: Process a��lamad�. Hata kodu: {error}");
                    return;
                }

                var memoryItems = await Task.Run(() =>
                {
                    var items = new List<ListViewItem>();
                    try
                    {
                        IntPtr baseAddress = process.MainModule.BaseAddress;
                        byte[] buffer = new byte[128]; // AccessViolationException'ı önlemek için küçültüldü
                        int bytesRead;

                        // Bellek okuma işlemi öncesi güvenlik kontrolü
                        if (IsMemoryRegionReadable(baseAddress, buffer.Length) && 
                            SafeReadProcessMemory(processHandle, baseAddress, buffer, buffer.Length, out bytesRead))
                        {
                            for (int i = 0; i < bytesRead; i += 8)
                            {
                                // Buffer s�n�rlar�n� kontrol et
                                if (i + 8 > bytesRead) break;

                                IntPtr currentAddress = IntPtr.Add(baseAddress, i);
                                string hexValue = BitConverter.ToString(buffer, i, Math.Min(8, bytesRead - i)).Replace("-", " ");
                                string asciiValue = Encoding.ASCII.GetString(buffer, i, Math.Min(8, bytesRead - i));
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
                            errorItem.SubItems.Add($"Bellek okunamad�: {error.Message}");
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

                // UI g�ncellemesini daha g�venli yap
                SafeInvoke(() =>
                {
                    try
                    {
                        listViewMemory.BeginUpdate();
                        listViewMemory.Items.AddRange(memoryItems.ToArray());
                        listViewMemory.EndUpdate();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"UI g�ncelleme hatas�: {ex.Message}");
                    }
                });

                UpdateStatus($"{process.ProcessName} bellek analizi tamamland�. {memoryItems.Count} kay�t.");
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

            // H�cre say�s�n� kontrol et
            if (selectedRow.Cells.Count < 1) return;

            // H�cre de�erinin null olup olmad���n� kontrol et
            if (selectedRow.Cells[0].Value == null) return;

            // De�erin integer olup olmad���n� kontrol et
            if (!int.TryParse(selectedRow.Cells[0].Value.ToString(), out int processId)) return;

            if (selectedRow.Cells.Count < 2 || selectedRow.Cells[1].Value == null) return;

            var processName = selectedRow.Cells[1].Value.ToString();

            var result = MessageBox.Show(
                $"{processName} (PID: {processId}) i�lemini sonland�rmak istedi�inize emin misiniz?",
                "��lem Sonland�rma",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    var process = Process.GetProcessById(processId);
                    process.Kill();
                    UpdateStatus($"{processName} i�lemi sonland�r�ld�.");
                    _ = RefreshProcessListAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"��lem sonland�r�lamad�: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void ShowProcessDetails(object sender, EventArgs e)
        {
            if (dataGridViewProcesses.SelectedRows.Count == 0) return;

            var selectedRow = dataGridViewProcesses.SelectedRows[0];

            // H�cre say�s�n� kontrol et
            if (selectedRow.Cells.Count < 1) return;

            // H�cre de�erinin null olup olmad���n� kontrol et
            if (selectedRow.Cells[0].Value == null) return;

            // De�erin integer olup olmad���n� kontrol et
            if (!int.TryParse(selectedRow.Cells[0].Value.ToString(), out int processId)) return;

            try
            {
                var process = Process.GetProcessById(processId);

                var detailsForm = new Form
                {
                    Text = $"{process.ProcessName} Detaylar� - PID: {process.Id}",
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
                MessageBox.Show($"Detaylar g�sterilemedi: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // NEW: Memory Search
        private async void buttonSearchMemory_Click(object sender, EventArgs e)
        {
            if (selectedProcess == null || selectedProcess.HasExited)
            {
                MessageBox.Show("�nce bir i�lem se�in.", "Uyar�", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SearchType searchType = (SearchType)comboBoxSearchType.SelectedIndex;
            string searchValue = textBoxSearchValue.Text;
            MemoryRegion region = (MemoryRegion)comboBoxMemoryRegion.SelectedIndex;

            if (string.IsNullOrEmpty(searchValue))
            {
                MessageBox.Show("Arama de�eri girin.", "Uyar�", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            UpdateStatus("Bellekte arama yap�l�yor...", true);

            Task.Run(async () =>
            {
                var results = await SearchMemoryAsync(searchType, searchValue, region);

                this.Invoke((MethodInvoker)delegate
                {
                    UpdateStatus($"Arama tamamland�. {results.Count} sonu� bulundu.");
                    ShowSearchResults(results);
                });
            });
        }

        private async Task<List<IntPtr>> SearchMemoryAsync(SearchType searchType, string searchValue, MemoryRegion region)
        {
            List<IntPtr> results = new List<IntPtr>();

            if (selectedProcess == null || selectedProcess.HasExited || processHandle == IntPtr.Zero)
                return results;

            await Task.Run(() =>
            {
                try
                {
                    List<MemoryRegionInfo> regions = GetMemoryRegions(region);

                    foreach (var memRegion in regions)
                    {
                        try
                        {
                            // Daha k���k par�alar halinde belle�i oku
                            IntPtr currentAddress = memRegion.BaseAddress;
                            long regionEnd = memRegion.BaseAddress.ToInt64() + memRegion.RegionSize.ToInt64();

                            while (currentAddress.ToInt64() < regionEnd)
                            {
                                // Daha k���k buffer boyutu kullan
                                int bufferSize = Math.Min(512, (int)(regionEnd - currentAddress.ToInt64())); // 1KB'l�k par�alar
                                byte[] buffer = new byte[bufferSize];
                                int bytesRead;

                                // Bellek okuma işlemi öncesi güvenlik kontrolü
                                if (IsMemoryRegionReadable(currentAddress, buffer.Length) && 
                                    SafeReadProcessMemory(processHandle, currentAddress, buffer, buffer.Length, out bytesRead))
                                {
                                    switch (searchType)
                                    {
                                        case SearchType.String:
                                            results.AddRange(SearchString(buffer, searchValue, currentAddress));
                                            break;
                                        case SearchType.Integer:
                                            if (int.TryParse(searchValue, out int intValue))
                                                results.AddRange(SearchInt(buffer, intValue, currentAddress));
                                            break;
                                        case SearchType.Hex:
                                            results.AddRange(SearchHex(buffer, searchValue, currentAddress));
                                            break;
                                        case SearchType.ByteArray:
                                            results.AddRange(SearchByteArray(buffer, searchValue, currentAddress));
                                            break;
                                    }
                                }
                                else
                                {
                                    // Bellek okunamadı, bir sonraki bölgeye geç
                                    bytesRead = bufferSize;
                                }

                                currentAddress = IntPtr.Add(currentAddress, bytesRead);

                                // Daha uzun bir bekleme ekle
                                Thread.Sleep(10); // 10ms bekle
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Bellek b�lgesi i�lenemedi: {memRegion.BaseAddress.ToString("X16")}, Hata: {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Arama hatas�: {ex.Message}");
                }
            });

            return results;
        }
        private List<IntPtr> SearchString(byte[] buffer, string searchValue, IntPtr baseAddress)
        {
            List<IntPtr> results = new List<IntPtr>();
            byte[] searchBytes = Encoding.ASCII.GetBytes(searchValue);

            for (int i = 0; i <= buffer.Length - searchBytes.Length; i++)
            {
                bool found = true;
                for (int j = 0; j < searchBytes.Length; j++)
                {
                    if (buffer[i + j] != searchBytes[j])
                    {
                        found = false;
                        break;
                    }
                }

                if (found)
                {
                    results.Add(IntPtr.Add(baseAddress, i));
                }
            }

            return results;
        }

        private List<IntPtr> SearchInt(byte[] buffer, int searchValue, IntPtr baseAddress)
        {
            List<IntPtr> results = new List<IntPtr>();
            byte[] searchBytes = BitConverter.GetBytes(searchValue);

            for (int i = 0; i <= buffer.Length - searchBytes.Length; i++)
            {
                bool found = true;
                for (int j = 0; j < searchBytes.Length; j++)
                {
                    if (buffer[i + j] != searchBytes[j])
                    {
                        found = false;
                        break;
                    }
                }

                if (found)
                {
                    results.Add(IntPtr.Add(baseAddress, i));
                }
            }

            return results;
        }

        private List<IntPtr> SearchHex(byte[] buffer, string searchValue, IntPtr baseAddress)
        {
            List<IntPtr> results = new List<IntPtr>();
            string hex = searchValue.Replace(" ", "");
            byte[] searchBytes = new byte[hex.Length / 2];

            for (int i = 0; i < searchBytes.Length; i++)
            {
                searchBytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }

            for (int i = 0; i <= buffer.Length - searchBytes.Length; i++)
            {
                bool found = true;
                for (int j = 0; j < searchBytes.Length; j++)
                {
                    if (buffer[i + j] != searchBytes[j])
                    {
                        found = false;
                        break;
                    }
                }

                if (found)
                {
                    results.Add(IntPtr.Add(baseAddress, i));
                }
            }

            return results;
        }

        private List<IntPtr> SearchByteArray(byte[] buffer, string searchValue, IntPtr baseAddress)
        {
            // Similar to SearchHex but with different input format
            return SearchHex(buffer, searchValue, baseAddress);
        }

        private List<MemoryRegionInfo> GetMemoryRegions(MemoryRegion region)
        {
            List<MemoryRegionInfo> regions = new List<MemoryRegionInfo>();

            if (selectedProcess == null || selectedProcess.HasExited || processHandle == IntPtr.Zero)
                return regions;

            try
            {
                IntPtr address = IntPtr.Zero;
                MEMORY_BASIC_INFORMATION mbi;

                while (VirtualQueryEx(processHandle, address, out mbi, (uint)Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION))) != false)
                {
                    if (region == MemoryRegion.All ||
                        (region == MemoryRegion.Heap && (mbi.Type & 0x00040000) != 0) ||
                        (region == MemoryRegion.Stack && (mbi.Type & 0x00100000) != 0) ||
                        (region == MemoryRegion.Modules && (mbi.Type & 0x10000000) != 0))
                    {
                        var memRegion = new MemoryRegionInfo
                        {
                            BaseAddress = mbi.BaseAddress,
                            RegionSize = mbi.RegionSize,
                            Protection = (MemoryProtection)mbi.Protect,
                            State = mbi.State == 0x1000 ? "MEM_COMMIT" : "MEM_FREE",
                            Type = mbi.Type == 0x1000000 ? "MEM_IMAGE" :
                                   mbi.Type == 0x00040000 ? "MEM_MAPPED" :
                                   mbi.Type == 0x00200000 ? "MEM_PRIVATE" : "UNKNOWN"
                        };

                        regions.Add(memRegion);
                    }

                    address = IntPtr.Add(mbi.BaseAddress, mbi.RegionSize.ToInt32());
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"Bellek b�lgeleri al�namad�: {ex.Message}");
            }

            return regions;
        }

        private void ShowSearchResults(List<IntPtr> results)
        {
            var resultsForm = new Form
            {
                Text = "Arama Sonu�lar�",
                Size = new Size(600, 400),
                StartPosition = FormStartPosition.CenterParent
            };

            var listView = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true
            };
            listView.Columns.Add("Adres", 150);
            listView.Columns.Add("Hex De�er", 200);
            listView.Columns.Add("ASCII De�er", 200);

            foreach (var address in results)
            {
                try
                {
                    byte[] buffer = new byte[16];
                    int bytesRead;

                    // Bellek okuma işlemi öncesi güvenlik kontrolü
                    if (IsMemoryRegionReadable(address, buffer.Length) && 
                        SafeReadProcessMemory(processHandle, address, buffer, buffer.Length, out bytesRead))
                    {
                        string hexValue = BitConverter.ToString(buffer, 0, bytesRead).Replace("-", " ");
                        string asciiValue = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                        asciiValue = new string(asciiValue.Select(c => char.IsControl(c) ? '.' : c).ToArray());

                        var item = new ListViewItem(address.ToString("X16"));
                        item.SubItems.Add(hexValue);
                        item.SubItems.Add(asciiValue);
                        listView.Items.Add(item);
                    }
                }
                catch (Exception ex)
                {
                    // Hata durumunda devam et
                    Debug.WriteLine($"Adres okunamad�: {address.ToString("X16")}, Hata: {ex.Message}");
                }
            }

            resultsForm.Controls.Add(listView);
            resultsForm.Show();
        }

        // NEW: Memory Monitoring
        private void buttonStartMonitoring_Click(object sender, EventArgs e)
        {
            if (selectedProcess == null || selectedProcess.HasExited)
            {
                MessageBox.Show("�nce bir i�lem se�in.", "Uyar�", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (listViewMemory.SelectedItems.Count == 0)
            {
                MessageBox.Show("�zlenecek bellek adreslerini se�in.", "Uyar�", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<IntPtr> addresses = new List<IntPtr>();
            foreach (ListViewItem item in listViewMemory.SelectedItems)
            {
                addresses.Add(IntPtr.Parse(item.Text, System.Globalization.NumberStyles.HexNumber));
            }

            int interval = (int)numericUpDownInterval.Value;
            listViewChanges.Items.Clear();

            buttonStartMonitoring.Enabled = false;
            buttonStopMonitoring.Enabled = true;

            UpdateStatus("Bellek izleme ba�lat�ld�...");

            monitoringCts = new CancellationTokenSource();
            Task.Run(async () =>
            {
                await StartMemoryMonitoringAsync(addresses, interval, monitoringCts.Token);

                this.Invoke((MethodInvoker)delegate
                {
                    buttonStartMonitoring.Enabled = true;
                    buttonStopMonitoring.Enabled = false;
                    UpdateStatus("Bellek izleme durduruldu.");
                });
            });
        }

        private void buttonStopMonitoring_Click(object sender, EventArgs e)
        {
            monitoringCts?.Cancel();
        }
        private bool IsMemoryRegionReadable(IntPtr address, int size)
        {
            if (selectedProcess == null || selectedProcess.HasExited || processHandle == IntPtr.Zero)
                return false;

            try
            {
                MEMORY_BASIC_INFORMATION mbi;
                if (VirtualQueryEx(processHandle, address, out mbi, (uint)Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION))) != false)
                {
                    // Bellek b�lgesinin durumunu kontrol et
                    if (mbi.State == 0x1000) // MEM_COMMIT
                    {
                        // Bellek koruma seviyesini kontrol et
                        if ((mbi.Protect & (uint)MemoryProtection.NoAccess) == 0)
                        {
                            // Bellek b�lgesinin boyutunu kontrol et
                            long regionEnd = mbi.BaseAddress.ToInt64() + mbi.RegionSize.ToInt64();
                            long addressEnd = address.ToInt64() + size;

                            return addressEnd <= regionEnd;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Bellek b�lgesi kontrol edilemedi: {address.ToString("X16")}, Hata: {ex.Message}");
            }

            return false;
        }
        private async Task StartMemoryMonitoringAsync(List<IntPtr> addresses, int intervalMs, CancellationToken token)
        {
            if (selectedProcess == null || selectedProcess.HasExited || processHandle == IntPtr.Zero)
                return;

            Dictionary<IntPtr, byte[]> previousValues = new Dictionary<IntPtr, byte[]>();

            // Get initial values
            foreach (var address in addresses)
            {
                try
                {
                    // Bellek b�lgesinin okunabilir olup olmad���n� kontrol et
                    if (!IsMemoryRegionReadable(address, 8))
                    {
                        Debug.WriteLine($"Adres okunabilir de�il: {address.ToString("X16")}");
                        continue;
                    }

                    byte[] buffer = new byte[8];
                    int bytesRead;

                    // Daha g�venli bellek okuma
                    if (IsMemoryRegionReadable(address, buffer.Length) && 
                        SafeReadProcessMemory(processHandle, address, buffer, buffer.Length, out bytesRead))
                    {
                        previousValues[address] = buffer;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Bellek okunamad�: {address.ToString("X16")}, Hata: {ex.Message}");
                }
            }

            // Monitoring loop
            while (!token.IsCancellationRequested && selectedProcess != null && !selectedProcess.HasExited)
            {
                try
                {
                    // Her d�ng� ad�m�nda process handle'�n ge�erli olup olmad���n� kontrol et
                    if (processHandle == IntPtr.Zero)
                    {
                        // Handle'� yeniden al
                        processHandle = OpenProcess(
                            ProcessAccessFlags.VirtualMemoryRead | ProcessAccessFlags.QueryInformation,
                            false,
                            selectedProcess.Id);

                        if (processHandle == IntPtr.Zero)
                        {
                            Debug.WriteLine("Process handle yeniden al�namad�");
                            break;
                        }
                    }

                    foreach (var address in addresses)
                    {
                        try
                        {
                            // Bellek b�lgesinin okunabilir olup olmad���n� kontrol et
                            if (!IsMemoryRegionReadable(address, 8))
                            {
                                continue;
                            }

                            byte[] buffer = new byte[8];
                            int bytesRead;

                            // Daha g�venli bellek okuma
                            if (IsMemoryRegionReadable(address, buffer.Length) && 
                                SafeReadProcessMemory(processHandle, address, buffer, buffer.Length, out bytesRead))
                            {
                                if (previousValues.ContainsKey(address))
                                {
                                    if (!buffer.SequenceEqual(previousValues[address]))
                                    {
                                        var change = new MemoryChange
                                        {
                                            Address = address,
                                            OldValue = previousValues[address],
                                            NewValue = buffer,
                                            Timestamp = DateTime.Now
                                        };

                                        // Daha g�venli UI g�ncelleme
                                        SafeInvoke(() => AddMemoryChangeToList(change));

                                        previousValues[address] = buffer;
                                    }
                                }
                            }
                        }
                        catch (AccessViolationException)
                        {
                            Debug.WriteLine($"Eri�im ihlali: {address.ToString("X16")}");
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Genel hata: {address.ToString("X16")}, Hata: {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"�zleme d�ng�s�nde hata: {ex.Message}");
                }

                try
                {
                    // Daha g�venli gecikme
                    await Task.Delay(intervalMs, token).ConfigureAwait(false);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Task.Delay hatas�: {ex.Message}");
                    break;
                }
            }
        }
        private bool SafeReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead)
        {
            lpNumberOfBytesRead = 0;

            try
            {
                // Bellek b�lgesinin ge�erli olup olmad���n� kontrol et
                if (lpBaseAddress == IntPtr.Zero)
                    return false;

                // Bellek b�lgesinin okunabilir olup olmad���n� kontrol et
                MEMORY_BASIC_INFORMATION mbi;
                if (VirtualQueryEx(hProcess, lpBaseAddress, out mbi, (uint)Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION))) != false)
                {
                    if (mbi.State != 0x1000) // MEM_COMMIT
                        return false;

                    if ((mbi.Protect & (uint)MemoryProtection.NoAccess) != 0)
                        return false;

                    // Bellek b�lgesinin boyutunu kontrol et
                    long regionEnd = mbi.BaseAddress.ToInt64() + mbi.RegionSize.ToInt64();
                    long addressEnd = lpBaseAddress.ToInt64() + dwSize;

                    if (addressEnd > regionEnd)
                        return false;
                }

                // Belle�i oku
                return ReadProcessMemory(hProcess, lpBaseAddress, lpBuffer, dwSize, out lpNumberOfBytesRead);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"G�venli bellek okuma hatas�: {ex.Message}");
                return false;
            }
        }

        // Daha g�venli UI g�ncelleme metodu
        private void SafeInvoke(Action action)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        try
                        {
                            action();
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"UI g�ncelleme hatas�: {ex.Message}");
                        }
                    }));
                }
                else
                {
                    action();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SafeInvoke hatas�: {ex.Message}");
            }
        }
        private void AddMemoryChangeToList(MemoryChange change)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => AddMemoryChangeToList(change)));
                return;
            }

            var item = new ListViewItem(change.Address.ToString("X16"));
            item.SubItems.Add(BitConverter.ToString(change.OldValue).Replace("-", " "));
            item.SubItems.Add(BitConverter.ToString(change.NewValue).Replace("-", " "));
            item.SubItems.Add(change.Timestamp.ToString("HH:mm:ss.fff"));
            item.BackColor = Color.LightYellow;

            listViewChanges.Items.Insert(0, item);

            // En fazla 1000 de�i�iklik tut
            if (listViewChanges.Items.Count > 1000)
            {
                listViewChanges.Items.RemoveAt(listViewChanges.Items.Count - 1);
            }
        }

        // NEW: Performance Monitoring
        private void buttonStartPerfMonitoring_Click(object sender, EventArgs e)
        {
            if (selectedProcess == null || selectedProcess.HasExited)
            {
                MessageBox.Show("�nce bir i�lem se�in.", "Uyar�", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            buttonStartPerfMonitoring.Enabled = false;
            buttonStopPerfMonitoring.Enabled = true;

            UpdateStatus("Performans izleme ba�lat�ld�...");

            perfMonitoringCts = new CancellationTokenSource();
            Task.Run(async () =>
            {
                await StartPerformanceMonitoringAsync(1000, perfMonitoringCts.Token);

                this.Invoke((MethodInvoker)delegate
                {
                    buttonStartPerfMonitoring.Enabled = true;
                    buttonStopPerfMonitoring.Enabled = false;
                    UpdateStatus("Performans izleme durduruldu.");
                });
            });
        }

        private void buttonStopPerfMonitoring_Click(object sender, EventArgs e)
        {
            perfMonitoringCts?.Cancel();
        }

        private async Task StartPerformanceMonitoringAsync(int intervalMs, CancellationToken token)
        {
            if (selectedProcess == null || selectedProcess.HasExited)
                return;

            while (!token.IsCancellationRequested && selectedProcess != null && !selectedProcess.HasExited)
            {
                try
                {
                    float cpuUsage = CalculateCpuUsage(selectedProcess);
                    long memoryUsage = selectedProcess.WorkingSet64;

                    var data = new PerformanceData
                    {
                        Timestamp = DateTime.Now,
                        CpuUsage = cpuUsage,
                        MemoryUsage = memoryUsage
                    };

                    this.Invoke((MethodInvoker)delegate
                    {
                        UpdatePerformanceChart(data);
                    });
                }
                catch (Exception ex)
                {
                    UpdateStatus($"Performans izleme hatas�: {ex.Message}");
                }

                try
                {
                    await Task.Delay(intervalMs, token);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
            }
        }

        private float CalculateCpuUsage(Process process)
        {
            var startTime = DateTime.UtcNow;
            var startCpuUsage = process.TotalProcessorTime;

            System.Threading.Thread.Sleep(500);

            var endTime = DateTime.UtcNow;
            var endCpuUsage = process.TotalProcessorTime;

            var cpuUsedMs = (endCpuUsage - startCpuUsage).TotalMilliseconds;
            var totalMsPassed = (endTime - startTime).TotalMilliseconds;

            var cpuUsageTotal = cpuUsedMs / (Environment.ProcessorCount * totalMsPassed);

            return (float)cpuUsageTotal * 100;
        }

        private void UpdatePerformanceChart(PerformanceData data)
        {
            try
            {
                // CPU grafi�ini g�ncelle
                if (chartCpu != null && chartCpu.Series.Count > 0 && chartCpu.ChartAreas.Count > 0)
                {
                    chartCpu.Series["CPU"].Points.AddXY(data.Timestamp.ToString("HH:mm:ss"), data.CpuUsage);

                    // En fazla 60 nokta tut
                    if (chartCpu.Series["CPU"].Points.Count > 60)
                    {
                        chartCpu.Series["CPU"].Points.RemoveAt(0);
                    }
                }

                // Bellek grafi�ini g�ncelle
                if (chartMemory != null && chartMemory.Series.Count > 0 && chartMemory.ChartAreas.Count > 0)
                {
                    chartMemory.Series["Memory"].Points.AddXY(data.Timestamp.ToString("HH:mm:ss"), data.MemoryUsage / 1024 / 1024);

                    // En fazla 60 nokta tut
                    if (chartMemory.Series["Memory"].Points.Count > 60)
                    {
                        chartMemory.Series["Memory"].Points.RemoveAt(0);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Performans grafi�i g�ncellenemedi: {ex.Message}");
            }
        }

        // NEW: Memory Editor
        private void buttonWriteMemory_Click(object sender, EventArgs e)
        {
            if (selectedProcess == null || selectedProcess.HasExited)
            {
                MessageBox.Show("�nce bir i�lem se�in.", "Uyar�", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!IntPtr.TryParse(textBoxMemoryAddress.Text, System.Globalization.NumberStyles.HexNumber, null, out IntPtr address))
            {
                MessageBox.Show("Ge�erli bir bellek adresi girin.", "Uyar�", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] data;
            try
            {
                string hexValues = textBoxMemoryValue.Text.Replace(" ", "");
                data = new byte[hexValues.Length / 2];
                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = Convert.ToByte(hexValues.Substring(i * 2, 2), 16);
                }
            }
            catch
            {
                MessageBox.Show("Ge�erli bir hex de�eri girin.", "Uyar�", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (WriteMemory(address, data))
            {
                MessageBox.Show("Bellek ba�ar�yla g�ncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _ = AnalyzeProcessMemoryAsync(selectedProcess);
            }
        }

        private bool WriteMemory(IntPtr address, byte[] data)
        {
            if (selectedProcess == null || selectedProcess.HasExited || processHandle == IntPtr.Zero)
                return false;

            try
            {
                // Bellek b�lgesinin yaz�labilir olup olmad���n� kontrol et
                if (!IsMemoryRegionWritable(address, data.Length))
                {
                    UpdateStatus($"Bellek b�lgesi yaz�labilir de�il: {address.ToString("X16")}");
                    return false;
                }

                // Daha g�venli bellek yazma
                return SafeWriteProcessMemory(processHandle, address, data);
            }
            catch (AccessViolationException ex)
            {
                UpdateStatus($"Bellek eri�im hatas�: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                UpdateStatus($"Bellek yazma hatas�: {ex.Message}");
                return false;
            }
        }
        private bool SafeWriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer)
        {
            try
            {
                // Bellek b�lgesinin ge�erli olup olmad���n� kontrol et
                if (lpBaseAddress == IntPtr.Zero)
                    return false;

                // Bellek b�lgesinin yaz�labilir olup olmad���n� kontrol et
                MEMORY_BASIC_INFORMATION mbi;
                if (VirtualQueryEx(hProcess, lpBaseAddress, out mbi, (uint)Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION))) != false)
                {
                    if (mbi.State != 0x1000) // MEM_COMMIT
                        return false;

                    if ((mbi.Protect & (uint)MemoryProtection.ReadWrite) == 0 &&
                        (mbi.Protect & (uint)MemoryProtection.ExecuteReadWrite) == 0)
                        return false;
                }

                // Belle�i yaz
                int bytesWritten;
                return WriteProcessMemory(hProcess, lpBaseAddress, lpBuffer, lpBuffer.Length, out bytesWritten);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"G�venli bellek yazma hatas�: {ex.Message}");
                return false;
            }
        }
        private bool IsMemoryRegionWritable(IntPtr address, int size)
        {
            if (selectedProcess == null || selectedProcess.HasExited || processHandle == IntPtr.Zero)
                return false;

            try
            {
                MEMORY_BASIC_INFORMATION mbi;
                if (VirtualQueryEx(processHandle, address, out mbi, (uint)Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION))) != false)
                {
                    // Bellek b�lgesinin durumunu kontrol et
                    if (mbi.State == 0x1000) // MEM_COMMIT
                    {
                        // Bellek koruma seviyesini kontrol et
                        if ((mbi.Protect & (uint)MemoryProtection.ReadWrite) != 0 ||
                            (mbi.Protect & (uint)MemoryProtection.ExecuteReadWrite) != 0)
                        {
                            // Bellek b�lgesinin boyutunu kontrol et
                            long regionEnd = mbi.BaseAddress.ToInt64() + mbi.RegionSize.ToInt64();
                            long addressEnd = address.ToInt64() + size;

                            return addressEnd <= regionEnd;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Bellek b�lgesi kontrol edilemedi: {address.ToString("X16")}, Hata: {ex.Message}");
            }

            return false;
        }
        // NEW: Process Tree
        private void buttonShowProcessTree_Click(object sender, EventArgs e)
        {
            treeViewProcesses.Nodes.Clear();

            var processes = Process.GetProcesses()
                .Where(p => !string.IsNullOrEmpty(p.ProcessName))
                .ToList();

            Dictionary<int, TreeNode> nodes = new Dictionary<int, TreeNode>();

            foreach (var process in processes)
            {
                try
                {
                    TreeNode node = new TreeNode($"{process.ProcessName} ({process.Id})");
                    node.Tag = process;
                    nodes[process.Id] = node;

                    int parentId = GetParentProcessId(process);

                    if (nodes.ContainsKey(parentId))
                    {
                        nodes[parentId].Nodes.Add(node);
                    }
                    else
                    {
                        treeViewProcesses.Nodes.Add(node);
                    }
                }
                catch
                {
                    // Skip inaccessible processes
                }
            }

            treeViewProcesses.ExpandAll();
        }

        private void TreeViewProcesses_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag is Process process)
            {
                selectedProcess = process;
                _ = AnalyzeProcessMemoryAsync(process);
            }
        }

        private int GetParentProcessId(Process process)
        {
            try
            {
                using (var query = new ManagementObjectSearcher(
                    $"SELECT ParentProcessId FROM Win32_Process WHERE ProcessId={process.Id}"))
                {
                    foreach (ManagementObject obj in query.Get())
                    {
                        return Convert.ToInt32(obj["ParentProcessId"]);
                    }
                }
            }
            catch
            {
                // Error handling
            }

            return 0;
        }

        // NEW: Memory Map
        private void buttonShowMemoryMap_Click(object sender, EventArgs e)
        {
            if (selectedProcess == null || selectedProcess.HasExited)
            {
                MessageBox.Show("�nce bir i�lem se�in.", "Uyar�", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var regions = GetMemoryRegions(MemoryRegion.All);
            ShowMemoryMap(regions);
        }

        private void ShowMemoryMap(List<MemoryRegionInfo> regions)
        {
            var mapForm = new Form
            {
                Text = "Bellek Haritas�",
                Size = new Size(800, 600),
                StartPosition = FormStartPosition.CenterParent
            };

            var listView = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true
            };
            listView.Columns.Add("Ba�lang�� Adresi", 150);
            listView.Columns.Add("B�lge Boyutu", 150);
            listView.Columns.Add("Koruma", 100);
            listView.Columns.Add("Durum", 100);
            listView.Columns.Add("T�r", 100);

            foreach (var region in regions)
            {
                var item = new ListViewItem(region.BaseAddress.ToString("X16"));
                item.SubItems.Add(region.RegionSize.ToString("X"));
                item.SubItems.Add(region.Protection.ToString());
                item.SubItems.Add(region.State);
                item.SubItems.Add(region.Type);
                listView.Items.Add(item);
            }

            var exportButton = new Button
            {
                Text = "D��a Aktar",
                Dock = DockStyle.Bottom
            };
            exportButton.Click += (s, ev) =>
            {
                using (var saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "CSV Files|*.csv";
                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        ExportMemoryMap(regions, saveDialog.FileName);
                    }
                }
            };

            mapForm.Controls.Add(listView);
            mapForm.Controls.Add(exportButton);
            mapForm.Show();
        }

        private void ExportMemoryMap(List<MemoryRegionInfo> regions, string filePath)
        {
            try
            {
                using (var writer = new StreamWriter(filePath))
                {
                    writer.WriteLine("Base Address,Region Size,Protection,State,Type");

                    foreach (var region in regions)
                    {
                        writer.WriteLine($"{region.BaseAddress.ToInt64():X},{region.RegionSize.ToInt64():X},{region.Protection},{region.State},{region.Type}");
                    }
                }

                UpdateStatus($"Bellek haritas� d��a aktar�ld�: {filePath}");
            }
            catch (Exception ex)
            {
                UpdateStatus($"D��a aktarma hatas�: {ex.Message}");
            }
        }

        // Utility methods
        private void ToggleHexView(object sender, EventArgs e)
        {
            // Toggle hex view implementation
            MessageBox.Show("Hex g�r�n�m� de�i�tirilecek", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void RescanMemory(object sender, EventArgs e)
        {
            if (selectedProcess != null && !selectedProcess.HasExited)
            {
                _ = AnalyzeProcessMemoryAsync(selectedProcess);
            }
        }

        private void CopyAddress(object sender, EventArgs e)
        {
            if (listViewMemory.SelectedItems.Count > 0)
            {
                var address = listViewMemory.SelectedItems[0].Text;
                Clipboard.SetText(address);
                UpdateStatus($"Adres kopyaland�: {address}");
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            monitoringCts?.Cancel();
            perfMonitoringCts?.Cancel();

            if (processHandle != IntPtr.Zero)
            {
                CloseHandle(processHandle);
                processHandle = IntPtr.Zero;
            }

            base.OnFormClosing(e);
        }
    }
}