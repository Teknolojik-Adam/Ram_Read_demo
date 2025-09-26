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
            dataGridViewProcesses.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ProcessName", HeaderText = "lem Ad", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dataGridViewProcesses.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "MemoryUsage", HeaderText = "Bellek (MB)", Width = 100 });
            dataGridViewProcesses.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Priority", HeaderText = "ncelik", Width = 80 });
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
            listViewMemory.Columns.Add("Hex Deer", 120);
            listViewMemory.Columns.Add("ASCII Deer", 120);
            listViewMemory.Columns.Add("Tr", 100);

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
               
                if (chartCpu != null)
                {
                    chartCpu.Series.Clear();

                   
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
                        chartCpu.ChartAreas[0].AxisY.Title = "CPU Kullanm (%)";
                        chartCpu.ChartAreas[0].AxisY.Maximum = 100;
                    }
                }

                if (chartMemory != null)
                {
                    chartMemory.Series.Clear();

                
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
                        chartMemory.ChartAreas[0].AxisY.Title = "Bellek Kullanm (MB)";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Grafikler ayarlanrken hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupTreeView()
        {
            treeViewProcesses.HideSelection = false;
            treeViewProcesses.AfterSelect += TreeViewProcesses_AfterSelect;
        }

        private void SetupControls()
        {
          
            comboBoxSearchType.Items.AddRange(Enum.GetNames(typeof(SearchType)));
            comboBoxSearchType.SelectedIndex = 0;

            comboBoxMemoryRegion.Items.AddRange(Enum.GetNames(typeof(MemoryRegion)));
            comboBoxMemoryRegion.SelectedIndex = 0;

          
            numericUpDownInterval.Minimum = 100;
            numericUpDownInterval.Maximum = 5000;
            numericUpDownInterval.Value = 1000;
            numericUpDownInterval.Increment = 100;

           
            listViewChanges.Columns.Add("Adres", 120);
            listViewChanges.Columns.Add("Eski Deer", 150);
            listViewChanges.Columns.Add("Yeni Deer", 150);
            listViewChanges.Columns.Add("Zaman Damgas", 120);
            listViewChanges.View = View.Details;
            listViewChanges.FullRowSelect = true;
        }

        private void SetupContextMenus()
        {
          
            var gridContextMenu = new ContextMenuStrip();
            gridContextMenu.Items.Add("Yenile", null, (s, e) => btnRefresh_Click(s, e));
            gridContextMenu.Items.Add("lemi Sonlandr", null, (s, e) => KillSelectedProcess(s, e));
            gridContextMenu.Items.Add("Detaylar Gster", null, (s, e) => ShowProcessDetails(s, e));
            dataGridViewProcesses.ContextMenuStrip = gridContextMenu;

            
            var listContextMenu = new ContextMenuStrip();
            listContextMenu.Items.Add("Hex Grnm", null, (s, e) => ToggleHexView(s, e));
            listContextMenu.Items.Add("Bellei Yeniden Tara", null, (s, e) => RescanMemory(s, e));
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
            UpdateStatus("lemler listeleniyor...", true);
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
                UpdateStatus($"lemler yklendi. Toplam: {processes.Count}");
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

            
            if (selectedRow.Cells.Count < 1) return;

            
            if (selectedRow.Cells[0].Value == null) return;

            if (!int.TryParse(selectedRow.Cells[0].Value.ToString(), out int processId)) return;

            try
            {
                selectedProcess = Process.GetProcessById(processId);
                await AnalyzeProcessMemoryAsync(selectedProcess);
            }
            catch (ArgumentException)
            {
                UpdateStatus($"Hata: {processId} ID'li ilem artk almyor.");
            }
            catch (Exception ex)
            {
                UpdateStatus($"Hata: {ex.Message}");
            }
        }

        private async Task AnalyzeProcessMemoryAsync(Process process)
        {
            listViewMemory.Items.Clear();
            UpdateStatus($"{process.ProcessName} bellei analiz ediliyor...", true);

            if (processHandle != IntPtr.Zero)
            {
                CloseHandle(processHandle);
                processHandle = IntPtr.Zero;
            }

            try
            {
                processHandle = OpenProcess(
                    ProcessAccessFlags.VirtualMemoryRead | ProcessAccessFlags.QueryInformation,
                    false,
                    process.Id);

                if (processHandle == IntPtr.Zero)
                {
                    var error = Marshal.GetLastWin32Error();
                    UpdateStatus($"Hata: Process alamad. Hata kodu: {error}");
                    return;
                }

                var memoryItems = await Task.Run(() =>
                {
                    var items = new List<ListViewItem>();
                    try
                    {
                        IntPtr baseAddress = process.MainModule.BaseAddress;
                        byte[] buffer = new byte[128]; 
                        int bytesRead;

                        if (IsMemoryRegionReadable(baseAddress, buffer.Length) && 
                            SafeReadProcessMemory(processHandle, baseAddress, buffer, buffer.Length, out bytesRead))
                        {
                            for (int i = 0; i < bytesRead; i += 8)
                            {
                               
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
                            errorItem.SubItems.Add($"Bellek okunamad: {error.Message}");
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
                        Debug.WriteLine($"UI gncelleme hatas: {ex.Message}");
                    }
                });

                UpdateStatus($"{process.ProcessName} bellek analizi tamamland. {memoryItems.Count} kayt.");
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

            if (selectedRow.Cells.Count < 1) return;

            if (selectedRow.Cells[0].Value == null) return;

            if (!int.TryParse(selectedRow.Cells[0].Value.ToString(), out int processId)) return;

            if (selectedRow.Cells.Count < 2 || selectedRow.Cells[1].Value == null) return;

            var processName = selectedRow.Cells[1].Value.ToString();

            var result = MessageBox.Show(
                $"{processName} (PID: {processId}) ilemini sonlandrmak istediinize emin misiniz?",
                "lem Sonlandrma",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                try
                {
                    var process = Process.GetProcessById(processId);
                    process.Kill();
                    UpdateStatus($"{processName} ilemi sonlandrld.");
                    _ = RefreshProcessListAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"lem sonlandrlamad: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void ShowProcessDetails(object sender, EventArgs e)
        {
            if (dataGridViewProcesses.SelectedRows.Count == 0) return;

            var selectedRow = dataGridViewProcesses.SelectedRows[0];

            if (selectedRow.Cells.Count < 1) return;

            if (selectedRow.Cells[0].Value == null) return;

            if (!int.TryParse(selectedRow.Cells[0].Value.ToString(), out int processId)) return;

            try
            {
                var process = Process.GetProcessById(processId);

                var detailsForm = new Form
                {
                    Text = $"{process.ProcessName} Detaylar - PID: {process.Id}",
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
                MessageBox.Show($"Detaylar gsterilemedi: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void buttonSearchMemory_Click(object sender, EventArgs e)
        {
            if (selectedProcess == null || selectedProcess.HasExited)
            {
                MessageBox.Show("nce bir ilem sein.", "Uyar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SearchType searchType = (SearchType)comboBoxSearchType.SelectedIndex;
            string searchValue = textBoxSearchValue.Text;
            MemoryRegion region = (MemoryRegion)comboBoxMemoryRegion.SelectedIndex;

            if (string.IsNullOrEmpty(searchValue))
            {
                MessageBox.Show("Arama deeri girin.", "Uyar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            UpdateStatus("Bellekte arama yaplyor...", true);

            Task.Run(async () =>
            {
                var results = await SearchMemoryAsync(searchType, searchValue, region);

                this.Invoke((MethodInvoker)delegate
                {
                    UpdateStatus($"Arama tamamland. {results.Count} sonu bulundu.");
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
                            IntPtr currentAddress = memRegion.BaseAddress;
                            long regionEnd = memRegion.BaseAddress.ToInt64() + memRegion.RegionSize.ToInt64();

                            while (currentAddress.ToInt64() < regionEnd)
                            {
                                int bufferSize = Math.Min(512, (int)(regionEnd - currentAddress.ToInt64())); // 1KB'lk paralar
                                byte[] buffer = new byte[bufferSize];
                                int bytesRead;

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
                                    bytesRead = bufferSize;
                                }

                                currentAddress = IntPtr.Add(currentAddress, bytesRead);

                                Thread.Sleep(10); 
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Bellek blgesi ilenemedi: {memRegion.BaseAddress.ToString("X16")}, Hata: {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Arama hatas: {ex.Message}");
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
                UpdateStatus($"Bellek blgeleri alnamad: {ex.Message}");
            }

            return regions;
        }

        private void ShowSearchResults(List<IntPtr> results)
        {
            var resultsForm = new Form
            {
                Text = "Arama Sonular",
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
            listView.Columns.Add("Hex Deer", 200);
            listView.Columns.Add("ASCII Deer", 200);

            foreach (var address in results)
            {
                try
                {
                    byte[] buffer = new byte[16];
                    int bytesRead;

                   
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
                    
                    Debug.WriteLine($"Adres okunamad: {address.ToString("X16")}, Hata: {ex.Message}");
                }
            }

            resultsForm.Controls.Add(listView);
            resultsForm.Show();
        }

      
        private void buttonStartMonitoring_Click(object sender, EventArgs e)
        {
            if (selectedProcess == null || selectedProcess.HasExited)
            {
                MessageBox.Show("nce bir ilem sein.", "Uyar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (listViewMemory.SelectedItems.Count == 0)
            {
                MessageBox.Show("zlenecek bellek adreslerini sein.", "Uyar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            UpdateStatus("Bellek izleme balatld...");

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
                    if (mbi.State == 0x1000) 
                    {
                        if ((mbi.Protect & (uint)MemoryProtection.NoAccess) == 0)
                        {
                            long regionEnd = mbi.BaseAddress.ToInt64() + mbi.RegionSize.ToInt64();
                            long addressEnd = address.ToInt64() + size;

                            return addressEnd <= regionEnd;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Bellek blgesi kontrol edilemedi: {address.ToString("X16")}, Hata: {ex.Message}");
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
                    if (!IsMemoryRegionReadable(address, 8))
                    {
                        Debug.WriteLine($"Adres okunabilir deil: {address.ToString("X16")}");
                        continue;
                    }

                    byte[] buffer = new byte[8];
                    int bytesRead;

                    if (IsMemoryRegionReadable(address, buffer.Length) && 
                        SafeReadProcessMemory(processHandle, address, buffer, buffer.Length, out bytesRead))
                    {
                        previousValues[address] = buffer;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Bellek okunamad: {address.ToString("X16")}, Hata: {ex.Message}");
                }
            }

            while (!token.IsCancellationRequested && selectedProcess != null && !selectedProcess.HasExited)
            {
                try
                {
                    if (processHandle == IntPtr.Zero)
                    {
                        processHandle = OpenProcess(
                            ProcessAccessFlags.VirtualMemoryRead | ProcessAccessFlags.QueryInformation,
                            false,
                            selectedProcess.Id);

                        if (processHandle == IntPtr.Zero)
                        {
                            Debug.WriteLine("Process handle yeniden alnamad");
                            break;
                        }
                    }

                    foreach (var address in addresses)
                    {
                        try
                        {
                            if (!IsMemoryRegionReadable(address, 8))
                            {
                                continue;
                            }

                            byte[] buffer = new byte[8];
                            int bytesRead;

                           
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

                                       
                                        SafeInvoke(() => AddMemoryChangeToList(change));

                                        previousValues[address] = buffer;
                                    }
                                }
                            }
                        }
                        catch (AccessViolationException)
                        {
                            Debug.WriteLine($"Erişim ihlali: {address.ToString("X16")}");
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Genel hata: {address.ToString("X16")}, Hata: {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"izleme döngüsünde hata: {ex.Message}");
                }

                try
                {
                    
                    await Task.Delay(intervalMs, token).ConfigureAwait(false);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Task.Delay hata: {ex.Message}");
                    break;
                }
            }
        }
        private bool SafeReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead)
        {
            lpNumberOfBytesRead = 0;

            try
            {
              
                if (lpBaseAddress == IntPtr.Zero)
                    return false;

               
                MEMORY_BASIC_INFORMATION mbi;
                if (VirtualQueryEx(hProcess, lpBaseAddress, out mbi, (uint)Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION))) != false)
                {
                    if (mbi.State != 0x1000) 
                        return false;

                    if ((mbi.Protect & (uint)MemoryProtection.NoAccess) != 0)
                        return false;

                    
                    long regionEnd = mbi.BaseAddress.ToInt64() + mbi.RegionSize.ToInt64();
                    long addressEnd = lpBaseAddress.ToInt64() + dwSize;

                    if (addressEnd > regionEnd)
                        return false;
                }

                
                return ReadProcessMemory(hProcess, lpBaseAddress, lpBuffer, dwSize, out lpNumberOfBytesRead);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Güvenli bellek okuma hata: {ex.Message}");
                return false;
            }
        }

      
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
                            Debug.WriteLine($"UI güncelleme hata: {ex.Message}");
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
                Debug.WriteLine($"SafeInvoke hata: {ex.Message}");
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

            
            if (listViewChanges.Items.Count > 1000)
            {
                listViewChanges.Items.RemoveAt(listViewChanges.Items.Count - 1);
            }
        }

        private void buttonStartPerfMonitoring_Click(object sender, EventArgs e)
        {
            if (selectedProcess == null || selectedProcess.HasExited)
            {
                MessageBox.Show("önce bir işlem seçin.", "Uyari", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            buttonStartPerfMonitoring.Enabled = false;
            buttonStopPerfMonitoring.Enabled = true;

            UpdateStatus("Performans izleme başlataldi...");

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
                    UpdateStatus($"Performans izleme hata: {ex.Message}");
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
                
                if (chartCpu != null && chartCpu.Series.Count > 0 && chartCpu.ChartAreas.Count > 0)
                {
                    chartCpu.Series["CPU"].Points.AddXY(data.Timestamp.ToString("HH:mm:ss"), data.CpuUsage);

                    
                    if (chartCpu.Series["CPU"].Points.Count > 60)
                    {
                        chartCpu.Series["CPU"].Points.RemoveAt(0);
                    }
                }

               
                if (chartMemory != null && chartMemory.Series.Count > 0 && chartMemory.ChartAreas.Count > 0)
                {
                    chartMemory.Series["Memory"].Points.AddXY(data.Timestamp.ToString("HH:mm:ss"), data.MemoryUsage / 1024 / 1024);

                   
                    if (chartMemory.Series["Memory"].Points.Count > 60)
                    {
                        chartMemory.Series["Memory"].Points.RemoveAt(0);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Performans grafiği güncellenemedi: {ex.Message}");
            }
        }

        private void buttonWriteMemory_Click(object sender, EventArgs e)
        {
            if (selectedProcess == null || selectedProcess.HasExited)
            {
                MessageBox.Show("önce bir işlem seçin.", "Uyari", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!IntPtr.TryParse(textBoxMemoryAddress.Text, System.Globalization.NumberStyles.HexNumber, null, out IntPtr address))
            {
                MessageBox.Show("Geçerli bir bellek adresi girin.", "Uyari", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                MessageBox.Show("Geçerli bir hex değeri girin.", "Uyari", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (WriteMemory(address, data))
            {
                MessageBox.Show("Bellek başariyla güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _ = AnalyzeProcessMemoryAsync(selectedProcess);
            }
        }

        private bool WriteMemory(IntPtr address, byte[] data)
        {
            if (selectedProcess == null || selectedProcess.HasExited || processHandle == IntPtr.Zero)
                return false;

            try
            {
               
                if (!IsMemoryRegionWritable(address, data.Length))
                {
                    UpdateStatus($"Bellek bölgesi yazilabilir değil: {address.ToString("X16")}");
                    return false;
                }

              
                return SafeWriteProcessMemory(processHandle, address, data);
            }
            catch (AccessViolationException ex)
            {
                UpdateStatus($"Bellek erişim hatasi: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                UpdateStatus($"Bellek yazma hatasi: {ex.Message}");
                return false;
            }
        }
        private bool SafeWriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer)
        {
            try
            {
                
                if (lpBaseAddress == IntPtr.Zero)
                    return false;

          
                MEMORY_BASIC_INFORMATION mbi;
                if (VirtualQueryEx(hProcess, lpBaseAddress, out mbi, (uint)Marshal.SizeOf(typeof(MEMORY_BASIC_INFORMATION))) != false)
                {
                    if (mbi.State != 0x1000)
                        return false;

                    if ((mbi.Protect & (uint)MemoryProtection.ReadWrite) == 0 &&
                        (mbi.Protect & (uint)MemoryProtection.ExecuteReadWrite) == 0)
                        return false;
                }

                
                int bytesWritten;
                return WriteProcessMemory(hProcess, lpBaseAddress, lpBuffer, lpBuffer.Length, out bytesWritten);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Güvenli bellek yazma hatasi: {ex.Message}");
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
                    
                    if (mbi.State == 0x1000) 
                    {
                     
                        if ((mbi.Protect & (uint)MemoryProtection.ReadWrite) != 0 ||
                            (mbi.Protect & (uint)MemoryProtection.ExecuteReadWrite) != 0)
                        {
                           
                            long regionEnd = mbi.BaseAddress.ToInt64() + mbi.RegionSize.ToInt64();
                            long addressEnd = address.ToInt64() + size;

                            return addressEnd <= regionEnd;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Bellek blgesi kontrol edilemedi: {address.ToString("X16")}, Hata: {ex.Message}");
            }

            return false;
        }
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
          
            }

            return 0;
        }

        private void buttonShowMemoryMap_Click(object sender, EventArgs e)
        {
            if (selectedProcess == null || selectedProcess.HasExited)
            {
                MessageBox.Show("önce bir iilem seçin.", "Uyari", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var regions = GetMemoryRegions(MemoryRegion.All);
            ShowMemoryMap(regions);
        }

        private void ShowMemoryMap(List<MemoryRegionInfo> regions)
        {
            var mapForm = new Form
            {
                Text = "Bellek Haritasi",
                Size = new Size(800, 600),
                StartPosition = FormStartPosition.CenterParent
            };

            var listView = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true
            };
            listView.Columns.Add("Balang Adresi", 150);
            listView.Columns.Add("Blge Boyutu", 150);
            listView.Columns.Add("Koruma", 100);
            listView.Columns.Add("Durum", 100);
            listView.Columns.Add("Tür", 100);

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
                Text = "Da Aktar",
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

                UpdateStatus($"Bellek haritas da aktarld: {filePath}");
            }
            catch (Exception ex)
            {
                UpdateStatus($"Da aktarma hatas: {ex.Message}");
            }
        }
        private void ToggleHexView(object sender, EventArgs e)
        {
            MessageBox.Show("Hex grnm deitirilecek", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                UpdateStatus($"Adres kopyaland: {address}");
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