using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.ComponentModel;
namespace MM370_Tools
{
     public class MxTool
    {
        public static void MxShowError(string title,int errcode)
        {
            string strErr;
            if (errcode != ExGlobal.SIO_WIN32FAIL)
            {
                switch(errcode)
                {
                    case ExGlobal.SIOFT_BADPORT:
                       strErr = "Port number is invalid or port is not opened in advance";
                       break;
                    case ExGlobal.SIO_OUTCONTROL:
                        strErr = "This board does not support this function";
                        break;
                    case ExGlobal.SIO_NODATA:
                        strErr = "No data to read";
                        break;
                    case ExGlobal.SIO_OPENFAIL:
                        strErr = "No such port or port is occupied by other program";
                        break;
                    case ExGlobal.SIO_RTS_BY_HW:
                        strErr = "RTS can''t be set because H/W flowctrl";
                        break;
                    case ExGlobal.SIO_BADPARM:
                        strErr = "Bad parameter";
                        break;
                    case ExGlobal.SIO_BOARDNOTSUPPORT:
                        strErr = "This board does not support this function";
                        break;
                    case ExGlobal.SIO_ABORT_WRITE:
                        strErr = "Write has blocked, and user abort write";
                        break;
                    case ExGlobal.SIO_WRITETIMEOUT:
                        strErr = "Write timeout has happened";
                        break;
                    default:
                        strErr = "Unknown Error:" + errcode.ToString();
                        break;
                }
                MessageBox.Show(strErr,title,MessageBoxButtons.OKCancel);
            }
            else
            {
                MessageBox.Show("非法参数", title, MessageBoxButtons.OKCancel);
            }
        }
        /*   
          ///GetLastWin32Error()获取错误代码为0
        static void ShowSysErr(string title)
        {
            int syserr;
            syserr = Marshal.GetLastWin32Error();
            Win32Exception win32Exception = new Win32Exception();
            if(syserr != 0)
            {
                MessageBox.Show(win32Exception.Message,title,MessageBoxButtons.OKCancel);
            }
            else
            {
                MessageBox.Show(win32Exception.Message, title, MessageBoxButtons.OKCancel);
            } 

         } 
        */
    }
}
