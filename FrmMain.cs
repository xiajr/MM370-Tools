using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using System.IO;

namespace MM370_Tools
{
    public struct CommData
    {
        public int cPort;
        public int cBaudRate;
        public int cParity;
        public int cByteSize;
        public int cStopBits;
        public void GetCommData(int iPort,int iBaudRate, int iParity, int iByteSize, int iStopBits)
        {
            cPort = iPort;
            cBaudRate = iBaudRate;
            cParity = iParity;
            cByteSize = iByteSize;
            cStopBits = iStopBits;
        }
    }
    public partial class FrmMain : Form
    {
        [DllImport("PCOMM.dll")]
        static extern int sio_close(int port);
        [DllImport("PCOMM.dll")]
        static extern int sio_open(int port);
        [DllImport("PCOMM.dll")]
        static extern int sio_flush(int port, int func);
        [DllImport("PCOMM.dll")]
        static extern int sio_read(int port, byte[] buf, int len);
        [DllImport("PCOMM.dll")]
        static extern int sio_write(int port, byte[] buf, int len);
        [DllImport("PCOMM.dll")]
        static extern int sio_ioctl(int port, int baud, int mode);
        [DllImport("PCOMM.dll")]
        static extern int sio_flowctrl(int port, int mode);
        [DllImport("PCOMM.dll")]
        static extern int sio_DTR(int port, int mode);
        [DllImport("PCOMM.dll")]
        static extern int sio_RTS(int port, int mode);
        [DllImport("PCOMM.dll")]
        static extern int sio_getbaud(int port);
        [DllImport("PCOMM.dll")]
        static extern int sio_SetWriteTimeouts(int port, int timeouts);
        readonly FrmConfig cfgForm = new FrmConfig();
        CommData getCommData = new CommData();
        private Thread thReadMeas;
        delegate void Mydel(string[] recvbuffer);
        Mydel delRead;
        int iRecvBufferOffest=0;
        string counts = "000000";
        int r = 1;
        public string[] analyData = new string[512];
        private readonly object readLock = new object();
        public FrmMain()
        {
            InitializeComponent();
        }
        private void FrmMain_Load(object sender, EventArgs e)
        {
            
            ExGlobal.GszAppName = "MM370-Tools";
            ExGlobal.GbOpen = false;
            ExGlobal.GbRead = false;
            
            //获取并显示系统时间
            timer1.Interval = 1000;
            timer1.Start();

            //Port开关使能
            SwitchMenu();

            //Form状态显示
            this.Text=ExGlobal.ShowStatus();
        }
        private void FrmMain_Shown(object sender, EventArgs e)
        {
            //调用缺省端口
            IniPort();
        }
        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (ExGlobal.GbOpen)
            { 
                ClosePort();
            }
        }
        private void CmSetting_Click(object sender, EventArgs e)
        {
            cfgForm.DialogResult = cfgForm.ShowDialog();
            if (cfgForm.DialogResult == DialogResult.Cancel)
            {
                return;
            }
            if (ExGlobal.GbOpen)
            {
                if (PortSet() == false)
                {
                    ExGlobal.iPort = getCommData.cPort;
                    ExGlobal.ibaudrate = getCommData.cBaudRate;
                    ExGlobal.iparity = getCommData.cParity;
                    ExGlobal.ibytesize = getCommData.cByteSize;
                    ExGlobal.istopbits = getCommData.cStopBits;
                    return;
                }
            }
            this.Text = ExGlobal.ShowStatus();
        }
        private void CmOpen_Click(object sender, EventArgs e)
        {
            OpenPort();
        }
        private void CmClose_Click(object sender, EventArgs e)
        {
            ClosePort();
        }
        private void CmExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void BtnReadPara_Click(object sender, EventArgs e)
        {
            string strSchNo= txtSchNo.Text;
            Request_Parameter(strSchNo);
        }
        private void BtnWritePara_Click(object sender, EventArgs e)
        {
            string strSchNo = txtSchNo.Text;
            Write_Parameter(strSchNo);
        }
        private void BtnExpMeas_Click(object sender, EventArgs e)
        {
            string saveFile;
            if (lvMeas.Items.Count == 0)
            {
                MessageBox.Show("数据为空请重新查询", "导出", MessageBoxButtons.OKCancel);
                return;
            }
            sfDlg = new SaveFileDialog
            {
                InitialDirectory = "C:\\",
                Filter = "Excel Files|*.xls",
                RestoreDirectory = true,
                FileName = "MeasData" + "(" + DateTime.Now.ToString("yyyyMMddHHmmss") + ")"
            };
            // 显示对话框
            DialogResult r = sfDlg.ShowDialog();
            if (r == DialogResult.Cancel)
            {
                return;
            }
            saveFile = sfDlg.FileName;
            string savePath = saveFile;
            if (saveFile.Length == 0)
            {
                return;
            }
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ExportToExcel(lvMeas, savePath);
                MessageBox.Show("导出成功!");
            }
            catch
            {
                MessageBox.Show("导出Excel失败", "导出", MessageBoxButtons.OKCancel);
            }
        }
        private void BtnClearMeas_Click(object sender, EventArgs e)
        {
            lvMeas.Items.Clear();
            r = 1;
        }
        private void BtnReadAllCyc_Click(object sender, EventArgs e)
        {
            int rc;
            int lenAllCyc = 0;
            string strSendCmd, strRecvAllCyc;
            byte[] byteRecvAllCyc = new byte[10000];
            strSendCmd = "#01?2\r\n";
            byte[] byteSendCmd = Encoding.Default.GetBytes(strSendCmd);
            btnReadAllCyc.Enabled = false;
            this.Cursor = Cursors.WaitCursor;
            if (ExGlobal.GbOpen)
            {
                lock (readLock)
                {
                    sio_flush(ExGlobal.Port, 2);
                    Thread.Sleep(100);
                    sio_write(ExGlobal.Port, @byteSendCmd, 7);
                    for (rc = 0; rc < 2; rc++)
                    {
                        Thread.Sleep(4000);
                        int len = sio_read(ExGlobal.Port, @byteRecvAllCyc, 4096);
                        Array.Copy(byteRecvAllCyc, 0, byteRecvAllCyc, lenAllCyc, len);
                        lenAllCyc += len;
                        sio_flush(ExGlobal.Port, 2);
                        if (len <= 0)
                        { break; } 
                    }
                    if (lenAllCyc <= 0)
                    {
                        MessageBox.Show("No Data Received!", "sio_read", MessageBoxButtons.OKCancel);
                        btnReadAllCyc.Enabled = true;
                        this.Cursor = Cursors.Default;
                        return;
                    }
                    strRecvAllCyc = Encoding.Default.GetString(byteRecvAllCyc);
                    string[] delimiters = { "\r\n" };
                    string[] analyAllCyc = strRecvAllCyc.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    if (analyAllCyc[0].Substring(0, 3) == "!01")
                    {
                        for (int i = 1; i < analyAllCyc.Length - 1; i++)
                        {
                            string[] split = { ",", " ,", "*" };
                            string[] cycData = analyAllCyc[i].ToString().Split(split, StringSplitOptions.RemoveEmptyEntries);
                            lvCurrAllCyc.BeginUpdate();
                            {
                                ListViewItem list = new ListViewItem()
                                {
                                    Text = cycData[0]
                                };
                                for (int j = 1; j < cycData.Length; j++)
                                {
                                    list.SubItems.Add(cycData[j]);
                                }
                                lvCurrAllCyc.Items.Add(list);
                            }
                            lvCurrAllCyc.EndUpdate();
                        }
                    }
                }
            }
            btnReadAllCyc.Enabled = true;
            this.Cursor = Cursors.Default;
        }
        private void BtnExpAllCyc_Click(object sender, EventArgs e)
        {
            string saveFile;
            if (lvCurrAllCyc.Items.Count == 0)
            {
                MessageBox.Show("数据为空请重新查询", "导出", MessageBoxButtons.OKCancel);
                return;
            }
            sfDlg = new SaveFileDialog
            {
                InitialDirectory = "C:\\",
                Filter = "Excel Files|*.xls",
                RestoreDirectory = true,
                FileName = "AllCyc Data" + "(" + DateTime.Now.ToString("yyyyMMddHHmmss") + ")"
            };
            // 显示对话框
            DialogResult r = sfDlg.ShowDialog();
            if (r == DialogResult.Cancel)
            {
                return;
            }
            saveFile = sfDlg.FileName;
            string savePath = saveFile;
            if (saveFile.Length == 0)
            {
                return;
            }
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ExportToExcel(lvCurrAllCyc, savePath);
                MessageBox.Show("导出成功!");
            }
            catch
            {
                MessageBox.Show("导出Excel失败", "导出", MessageBoxButtons.OKCancel);
            }
        }
        private void BtnClearAllCyc_Click(object sender, EventArgs e)
        {
            lvCurrAllCyc.Items.Clear();
        }
        private void BtnReadWave_Click(object sender, EventArgs e)
        {
            int rc;
            int lenWave = 0;
            string strSendCmd, strRecvWave;
            byte[] byteRecvWave = new byte[40960];
            strSendCmd = "#01?4\r\n";
            byte[] byteSendCmd = Encoding.Default.GetBytes(strSendCmd);
            btnReadWave.Enabled = false;
            this.Cursor = Cursors.WaitCursor;
            if (ExGlobal.GbOpen)
            {
                lock (readLock)
                {
                    sio_flush(ExGlobal.Port, 2);
                    Thread.Sleep(50);
                    sio_write(ExGlobal.Port, @byteSendCmd, 7);
                    for (rc = 0; rc < 10; rc++)
                    {
                        Thread.Sleep(4000);
                        int len = sio_read(ExGlobal.Port, @byteRecvWave, 4096);
                        Array.Copy(byteRecvWave, 0, byteRecvWave, lenWave, len);
                        lenWave += len;
                        sio_flush(ExGlobal.Port, 2);
                        /*
                        if (len <= 0)
                        { break; } */
                    }
                    if (lenWave <= 0)
                    {
                        MessageBox.Show("No Data Received!", "sio_read", MessageBoxButtons.OKCancel);
                        btnReadWave.Enabled = true;
                        this.Cursor = Cursors.Default;
                        return;
                    }
                    strRecvWave = Encoding.Default.GetString(byteRecvWave);
                    string[] delimiters = { "\r\n" };
                    string[] analyWave = strRecvWave.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                    if (analyWave[0].Substring(0, 3) == "!01")
                    {
                        for (int i = 1; i < analyWave.Length - 1; i++)
                        {
                            string[] split = { ",", " ,", "*" };
                            string[] waveData = analyWave[i].ToString().Split(split, StringSplitOptions.RemoveEmptyEntries);
                            lvWave.BeginUpdate();
                            {
                                ListViewItem list = new ListViewItem()
                                {
                                    Text = waveData[0]
                                };
                                for (int j = 1; j < waveData.Length; j++)
                                {
                                    list.SubItems.Add(waveData[j]);
                                }
                                lvCurrAllCyc.Items.Add(list);
                            }
                            lvWave.EndUpdate();
                        }
                    }
                }
            }
            btnReadWave.Enabled = true;
            this.Cursor = Cursors.Default;
        }
        private void BtnExpWave_Click(object sender, EventArgs e)
        {
            string saveFile;
            if (lvWave.Items.Count == 0)
            {
                MessageBox.Show("数据为空请重新查询", "导出", MessageBoxButtons.OKCancel);
                return;
            }
            sfDlg = new SaveFileDialog
            {
                InitialDirectory = "C:\\",
                Filter = "Excel Files|*.xls",
                RestoreDirectory = true,
                FileName = "WaveData" + "(" + DateTime.Now.ToString("yyyyMMddHHmmss") + ")"
            };
            // 显示对话框
            DialogResult r = sfDlg.ShowDialog();
            if (r == DialogResult.Cancel)
            {
                return;
            }
            saveFile = sfDlg.FileName;
            string savePath = saveFile;
            if (saveFile.Length == 0)
            {
                return;
            }
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ExportToExcel(lvWave, savePath);
                MessageBox.Show("导出成功!");
            }
            catch
            {
                MessageBox.Show("导出Excel失败", "导出", MessageBoxButtons.OKCancel);
            }
        }
        private void BtnClearWave_Click(object sender, EventArgs e)
        {
            lvWave.Items.Clear();
        }

        /// <summary>
        /// Port开关使能
        /// </summary>
        public void SwitchMenu()
        {
            cmOpen.Enabled = !ExGlobal.GbOpen;
            cmClose.Enabled = ExGlobal.GbOpen;
            btnClearMeas.Enabled = !ExGlobal.GbOpen;
        }
        /// <summary>
        /// 获取系统时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer1_Tick(object sender, EventArgs e)
        {
            StatusDate.Text = DateTime.Now.ToString();
        }
        /// <summary>
        /// 初始化缺省端口属性
        /// </summary>
        public void IniPort()
        {
            cfgForm.GetPort();      //获取本机可用端口
            ExGlobal.Port = ExGlobal.iPort;
            ExGlobal.BaudRate = ExGlobal.GBaudTable[13];
            ExGlobal.Parity = ExGlobal.GParityTable[1];
            ExGlobal.ByteSize = ExGlobal.GByteSizeTable[3];
            ExGlobal.StopBits = ExGlobal.GStopBitsTable[0];
            ExGlobal.ibaudrate = 13;
            ExGlobal.iparity = 1;
            ExGlobal.ibytesize = 3;
            ExGlobal.istopbits = 0;
            ExGlobal.Hw = false;
            ExGlobal.Sw = false;
            ExGlobal.Dtr = false;
            ExGlobal.Rts = false;
        }
        /// <summary>
        /// 打开端口，读取设备信息
        /// </summary>
        /// <returns></returns>
        public Boolean OpenPort()
        {
            int ret;
            Boolean Openport = false;
            ret = sio_open(ExGlobal.Port);
            if (ret != ExGlobal.SIO_OK)
            {
               MxTool.MxShowError("sio_open",ret);
                return Openport;
            }
            if (PortSet() == false)
            {
                sio_close(ExGlobal.Port);
                return Openport;
            }
            getCommData.GetCommData(ExGlobal.iPort,ExGlobal.ibaudrate, ExGlobal.iparity, ExGlobal.ibytesize, ExGlobal.istopbits);
            ExGlobal.GhExit = false;
            ExGlobal.GbOpen = true;
            StatusConnect.Text = "Online";
            StatusConnect.BackColor = Color.LimeGreen;
            SwitchMenu();
            this.Text = ExGlobal.ShowStatus();
            Openport = true;
            Request_Ver();
            thReadMeas = new Thread(new ThreadStart(ReadThreadExecute))
            {IsBackground = true};
            thReadMeas.Start();
            delRead = new Mydel(ShowData);
            return Openport;
        }
        /// <summary>
        /// 关闭端口
        /// </summary>
        public void ClosePort()
        {
            ExGlobal.GhExit = true;
            sio_close(ExGlobal.Port);
            ExGlobal.GbOpen = false;
            StatusConnect.Text = "Offline";
            StatusConnect.BackColor = Color.Empty;
            SwitchMenu();
            this.Text = ExGlobal.ShowStatus();
        }
        /// <summary>
        /// 端口设置
        /// </summary>
        /// <returns></returns>
        public Boolean PortSet()
        {
            int port, mode, hw, sw, ret, tout;
            Boolean PortSet = false;
            port = ExGlobal.Port;
            mode = ExGlobal.ByteSize | ExGlobal.StopBits | ExGlobal.Parity;
            
            if (ExGlobal.Hw)
            { hw = 3; }
            else
            { hw = 0; }
            if (ExGlobal.Sw)
            { sw = 12; }
            else
            { sw = 0; }
            ret = sio_ioctl(port, ExGlobal.BaudRate, mode);
            if (ret != ExGlobal.SIO_OK)
            {
                MxTool.MxShowError("sio_ioctl", ret);
                return PortSet;
            }
            ret = sio_flowctrl(port, hw | sw);
            if (ret != ExGlobal.SIO_OK)
            {
                MxTool.MxShowError("sio_flowctrl", ret);
                return PortSet;
            }
            tout = 512 * 1000 / sio_getbaud(ExGlobal.Port) * 3;    //ms /byte
            ret = sio_SetWriteTimeouts(ExGlobal.Port, tout);
            if (ret != ExGlobal.SIO_OK)
            {
                MxTool.MxShowError("sio_SetWriteTimeouts", ret);
                return PortSet;
            }
            this.Text = ExGlobal.ShowStatus();
            PortSet = true;
            return PortSet;
        }
        /// <summary>
        /// 读设备信息
        /// </summary>
        public void Request_Ver()
        {
            string strSendVer,strRecVer;
            int lenVer,ret;
            byte[] byteRecVer = new byte[21]; 
            strSendVer = "#01I\r\n";
            byte[] byteSendVer = Encoding.Default.GetBytes(strSendVer);
            ret = sio_flush(ExGlobal.Port, 2);
            if (ret != ExGlobal.SIO_OK)
            {
                MxTool.MxShowError("sio_flush", ret);
                return;
            }
            sio_write(ExGlobal.Port, @byteSendVer, byteSendVer.Length);
            Thread.Sleep(100);
            lenVer = sio_read(ExGlobal.Port, @byteRecVer, 21);
            if (lenVer <= 0)
            {
                MessageBox.Show("No Data Received!", "sio_read", MessageBoxButtons.OKCancel);
                return;
            }
            strRecVer= Encoding.Default.GetString(byteRecVer);
            string[] delimiters = { ":" };
            string[] analyVer = strRecVer.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
            StatusVer.Text = analyVer[1];
        }
        /// <summary>
        /// 读参数
        /// </summary>
        public void Request_Parameter(string strSchNo)
        {
            int lenPara;
            string strSendCmd, strRecvPara;
            byte[] byteRecvPara = new byte[512];
            strSendCmd = "#01R"+ strSchNo+"\r\n";
            byte[] byteSendCmd = Encoding.Default.GetBytes(strSendCmd);
            this.Cursor = Cursors.WaitCursor;
            if (ExGlobal.GbOpen)
            {
                lock (readLock)
                {
                    sio_flush(ExGlobal.Port, 2);
                    Thread.Sleep(100);
                    sio_write(ExGlobal.Port, @byteSendCmd, byteSendCmd.Length);
                    Thread.Sleep(800);
                    lenPara = sio_read(ExGlobal.Port, @byteRecvPara, 512);
                    if (lenPara <= 0)
                    {
                        MessageBox.Show("No Data Received!", "sio_read", MessageBoxButtons.OKCancel);
                        this.Cursor = Cursors.Default;
                        return;
                    }  
                }
                strRecvPara = Encoding.Default.GetString(byteRecvPara);
                string[] delimiters = { ":", "," };
                string[] analyPara = strRecvPara.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                txtSchNo.Text = analyPara[0].Substring(3);
                //遍历控件，根据控件类型赋值
                foreach (Control c in this.grpBoxPara.Controls)
                {
                    if (c is ComboBox)
                    {
                        ComboBox cb = c as ComboBox;
                        cb.Tag = analyPara[cb.TabIndex];
                        switch (cb.Name)
                        {
                            case "cmbFrequency":
                                if ((cb.Tag as string) == "050")
                                { cb.SelectedIndex = 0; }
                                if ((cb.Tag as string) == "060")
                                { cb.SelectedIndex = 1; }
                                break;
                            default:
                                cb.SelectedIndex = Convert.ToInt32(cb.Tag);
                                break;
                        }
                    }
                    if (c is TextBox)
                    {
                        TextBox tb = c as TextBox;
                        tb.Text = analyPara[tb.TabIndex].Trim();
                    }
                }
                this.Cursor = Cursors.Default;
            }
        }
        /// <summary>
        /// 写参数
        /// </summary>
        /// <param name="strSchNo"></param>
        public void Write_Parameter(string strSchNo)
        {
            int lenPara;
            string strRecvPara;
            byte[] byteRecvPara = new byte[512];
            StringBuilder strSendPara = new StringBuilder("#01W");
            strSendPara.Append(strSchNo);
            strSendPara.Append(":");
            //遍历控件，根据控件类型取值,添加到发送字符串中
            foreach (Control c in grpBoxPara.Controls)
            {
                if (c is ComboBox)
                {
                    ComboBox cb = c as ComboBox;
                    switch (cb.Name)
                    {
                        case "cmbFrequency":
                            if (cb.SelectedIndex == 0)
                            { cb.Tag ="050"; }
                            if (cb.SelectedIndex == 1)
                            { cb.Tag ="060"; }
                            break;
                        case ("cmbMeasType1"):
                            cb.Tag = "0" + cb.SelectedIndex.ToString();
                            break;
                        case ("cmbMeasType2"):
                            cb.Tag = "0" + cb.SelectedIndex.ToString();
                            break;
                        case ("cmbMeasType3"):
                            cb.Tag = "0" + cb.SelectedIndex.ToString();
                            break;
                        case ("cmbMeasType4"):
                            cb.Tag = "0" + cb.SelectedIndex.ToString();
                            break;
                        case ("cmbMeasType5"):
                            cb.Tag = "0" + cb.SelectedIndex.ToString();
                            break;
                        default:
                            cb.Tag = cb.SelectedIndex;
                            break;
                    }
                    strSendPara.Append(cb.Tag.ToString());
                    strSendPara.Append(",");
                }
                if (c is TextBox)
                {
                    TextBox tb = c as TextBox;
                    strSendPara.Append(tb.Text.Trim());
                    strSendPara.Append(",");
                }
            }
            strSendPara.Remove(strSendPara.Length - 1, 1);
            strSendPara.Append("\r\n");
            byte[] byteSendPara= Encoding.Default.GetBytes(strSendPara.ToString());
            this.Cursor = Cursors.WaitCursor;
            if (ExGlobal.GbOpen)
            {
                lock (readLock)
                {
                    sio_flush(ExGlobal.Port, 2);
                    Thread.Sleep(50);
                    sio_write(ExGlobal.Port, @byteSendPara, byteSendPara.Length);
                    Thread.Sleep(1500);
                    lenPara = sio_read(ExGlobal.Port, @byteRecvPara, 512);
                    if (lenPara <= 0)
                    {
                        MessageBox.Show("No Data Received!", "sio_read", MessageBoxButtons.OKCancel);
                        this.Cursor = Cursors.Default;
                        return;
                    }
                }
                strRecvPara = Encoding.Default.GetString(byteRecvPara);
                string[] delimiters = { ":", "," };
                string[] analyPara = strRecvPara.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                txtSchNo.Text = analyPara[0].Substring(3);
                //遍历控件，根据控件类型赋值
                foreach (Control c in this.grpBoxPara.Controls)
                {
                    if (c is ComboBox)
                    {
                        ComboBox cb = c as ComboBox;
                        cb.Tag = analyPara[cb.TabIndex];
                        switch (cb.Name)
                        {
                            case "cmbFrequency":
                                if ((cb.Tag as string) == "050")
                                { cb.SelectedIndex = 0; }
                                if ((cb.Tag as string) == "060")
                                { cb.SelectedIndex = 1; }
                                break;
                            default:
                                cb.SelectedIndex = Convert.ToInt32(cb.Tag);
                                break;
                        }
                    }
                    if (c is TextBox)
                    {
                        TextBox tb = c as TextBox;
                        tb.Text = analyPara[tb.TabIndex].Trim();
                    }
                }
                this.Cursor = Cursors.Default;
            }
        }
        /// <summary>
        /// 接收线程，接收测量数据
        /// </summary>
        public void ReadThreadExecute()
        {
            string strReadCmd;
            int lenMeas;
            while (!ExGlobal.GhExit)
            {
                Thread.Sleep(100);
                strReadCmd = "#01?1\r\n";
                byte[] byteReadCmd = Encoding.Default.GetBytes(strReadCmd);
                byte[] byteReadMeas = new byte[512];
                lock (readLock)
                {
                     sio_write(ExGlobal.Port, @byteReadCmd, 7);
                     Thread.Sleep(200);
                     lenMeas = sio_read(ExGlobal.Port, @byteReadMeas, 512);
                }
                if (lenMeas > 0)
                {
                    string strReadMeas = Encoding.Default.GetString(byteReadMeas);
                    string[] delimiters = { ",","\r\n" };
                    string[] recvbuffer = strReadMeas.Split(delimiters, StringSplitOptions.None);
                    this.Invoke(delRead, new object[] { recvbuffer });
                    Thread.Sleep(200);
                }   
            }
        }
        /// <summary>
        /// 测量数据UI显示
        /// </summary>
        /// <param name="recvbuffer"></param>
        private void ShowData(string[] recvbuffer)
        {
            string bCmd;
            string bHeader;
            /*
            if (recvbuffer.Length<18)
            {
                //数据长度不足，返回
                iRecvBufferOffest = 0;
                return;
            }
            */
            recvbuffer.CopyTo(analyData, iRecvBufferOffest);
            iRecvBufferOffest += recvbuffer.Length;
            if (iRecvBufferOffest < 18)
            { return; }
        DoChkHeader:
            bCmd = analyData[0];
            bHeader = analyData[1];
            if (bCmd != "!01")
            {
                //搜索“!01”开头的数组
                Array.Copy(analyData, 1, analyData, 0, iRecvBufferOffest-1);
                iRecvBufferOffest -= 1;
                if (iRecvBufferOffest > 2)
                { goto DoChkHeader; }
                return; 
            }
            else if((bCmd == "!01")&&bHeader.Contains("0001"))
            {
                //长度不足18，退出方法
                if (iRecvBufferOffest < 18)
                { return; }
                //bCmd=“!01”时，为测量数据
                //bHeader="0001"为MeasCurrent
                txtSchNo.Text = analyData[1].Substring(5);
                string strDt = analyData[2];
                string strTotalCurr = analyData[5];
                string strWeldCyc = analyData[14];
                string strCount = analyData[17];
                //  写入LISTVIEW   
                if (strCount != counts)
                {
                    counts = strCount;
                    lvMeas.BeginUpdate();
                    {
                        ListViewItem list = new ListViewItem
                        {
                            ImageIndex = r,
                            Text = r.ToString()
                        };
                        list.SubItems.Add(strTotalCurr);
                        list.SubItems.Add(strWeldCyc);
                        list.SubItems.Add(strCount);
                        list.SubItems.Add(strDt);
                        lvMeas.Items.Add(list);
                        r++;
                    }
                    lvMeas.EndUpdate();
                }
                Array.Copy(analyData, 18, analyData, 0, iRecvBufferOffest - 18);
                iRecvBufferOffest -= 18;
                if (iRecvBufferOffest > 2)
                { goto DoChkHeader; }
                return;
            }
        }
        /// <summary>
        /// 导出EXCEL文件
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="savePath"></param>
        public void ExportToExcel(ListView dt, string savePath)
        {
            //创建文件
            FileStream file = new FileStream(savePath, FileMode.CreateNew, FileAccess.Write);
            //以指定的字符编码向指定的流写入字符
            StreamWriter sw = new StreamWriter(file, Encoding.GetEncoding("GB2312"));
            StringBuilder strbu = new StringBuilder();
            //写入标题
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                strbu.Append(dt.Columns[i].Text + "\t");
            }
            //加入换行字符串
            strbu.Append(Environment.NewLine);
            //写入内容
            for (int j = 0; j < dt.Items.Count; j++)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    strbu.Append(dt.Items[j].SubItems[i].Text + "\t");
                }
                strbu.Append(Environment.NewLine);
            }
            sw.Write(strbu.ToString());
            sw.Flush();
            file.Flush();

            sw.Close();
            sw.Dispose();

            file.Close();
            file.Dispose();
        }  
    }
}

