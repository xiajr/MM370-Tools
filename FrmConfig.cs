using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace MM370_Tools
{
    public partial class FrmConfig : Form
    {
        Boolean Gfhw;
        public FrmConfig()
        {
            InitializeComponent();
        }

        private void FrmConfig_Load(object sender, EventArgs e)
        {
            if(ExGlobal.iPort==0)
            {
                return;
            }
            cbPort.Text="COM"+ExGlobal.iPort.ToString();
            cbBaudRate.SelectedIndex = ExGlobal.ibaudrate;
            cbParity.SelectedIndex = ExGlobal.iparity;
            cbByteSize.SelectedIndex = ExGlobal.ibytesize;
            cbSpotBits.SelectedIndex = ExGlobal.istopbits;
            chHw.Checked = ExGlobal.Hw;
            chSw.Checked = ExGlobal.Sw;
            chRts.Checked = ExGlobal.Rts;
            chDtr.Checked = ExGlobal.Dtr;
            Gfhw = ExGlobal.Hw;
            chRts.Enabled = !Gfhw;
            cbPort.Enabled = !ExGlobal.GbOpen;
        }

        private void CfgOK_Click(object sender, EventArgs e)
        {
            ExGlobal.ibaudrate = cbBaudRate.SelectedIndex;
            ExGlobal.iparity = cbParity.SelectedIndex;
            ExGlobal.ibytesize = cbByteSize.SelectedIndex;
            ExGlobal.istopbits = cbSpotBits.SelectedIndex;
            if (string.IsNullOrEmpty(cbPort.Text))
            {
                MessageBox.Show("PortName is null or empty!", "IsNullOrEmpty", MessageBoxButtons.OKCancel);
                return;
            }
            try
            {
                string strPort = cbPort.Text.Trim().Substring(3);
                int.TryParse(strPort, out ExGlobal.iPort);
            }
            catch (ArgumentOutOfRangeException e1)
            {
                MessageBox.Show(e1.Message, e1.StackTrace, MessageBoxButtons.OKCancel);
                System.Environment.Exit(0);
            }
            ExGlobal.Port = ExGlobal.iPort;
            ExGlobal.BaudRate = ExGlobal.GBaudTable[ExGlobal.ibaudrate];
            ExGlobal.ByteSize = ExGlobal.GByteSizeTable[ExGlobal.ibytesize];
            ExGlobal.Parity = ExGlobal.GParityTable[ExGlobal.iparity];
            ExGlobal.StopBits = ExGlobal.GStopBitsTable[ExGlobal.istopbits];
            ExGlobal.Hw = chHw.Checked;
            ExGlobal.Sw = chSw.Checked;
            ExGlobal.Rts = chRts.Checked;
            ExGlobal.Dtr = chDtr.Checked;
            DialogResult = DialogResult.OK;
            cbPort.Focus();
        }

        private void CfgCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            cbPort.Focus();
        }

        private void ChHw_Click(object sender, EventArgs e)
        {
            chRts.Enabled = Gfhw;
            Gfhw = !Gfhw;
        }

        /// <summary>
        /// 获取可用端口
        /// </summary>
        public void GetPort()
        {
            foreach (string portName in SerialPort.GetPortNames())
            {
                cbPort.Items.Add(portName);
            }
            if (cbPort.Items.Count > 0)
            {
                cbPort.SelectedIndex = 0;
            }
            if (string.IsNullOrEmpty(cbPort.Text))
            {
                MessageBox.Show("PortName is null or empty!", "IsNullOrEmpty", MessageBoxButtons.OKCancel);
                return;
            } 
            try
            {
                string strPort = cbPort.Text.Trim().Substring(3);
                int.TryParse(strPort, out ExGlobal.iPort);
            }
            catch(ArgumentOutOfRangeException e)
            {
                MessageBox.Show(e.Message, e.StackTrace, MessageBoxButtons.OKCancel);
                System.Environment.Exit(0);
            }
        }
    }
}
