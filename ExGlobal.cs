using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace MM370_Tools
{
    public class ExGlobal
    {
        /*	BAUD rate setting */
        const byte B50 = 0x00;
        const byte B75 = 0x01;
        const byte B110 = 0x02;
        const int B134 = 0x03;
        const byte B150 = 0x04;
        const byte B300 = 0x05;
        const byte B600 = 0x06;
        const byte B1200 = 0x07;
        const byte B1800 = 0x08;
        const byte B2400 = 0x09;
        const byte B4800 = 0x0A;
        const byte B7200 = 0x0B;
        const byte B9600 = 0x0C;
        const byte B19200 = 0x0D;
        const byte B38400 = 0x0E;
        const byte B57600 = 0x0F;
        const byte B115200 = 0x10;
        const byte B230400 = 0x11;
        const byte B460800 = 0x12;
        const byte B921600 = 0x13;
        /*	MODE setting */
        const byte BIT_5 = 0x00;
        const byte BIT_6 = 0x01;      /* Word length define	*/
        const byte BIT_7 = 0x02;
        const byte BIT_8 = 0x03;

        const byte STOP_1 = 0x00;      /* Stop bits define	*/
        const byte STOP_2 = 0x04;

        const byte P_EVEN = 0x18;
        const byte P_ODD = 0x08;    /* Parity define	*/
        const byte P_SPC = 0x38;
        const byte P_MRK = 0x28;
        const byte P_NONE = 0x00;
        /*	MODEM CONTROL setting	*/
        public const byte C_DTR = 0x00;
        public const byte C_RTS = 0x01;
        /*	MODEM LINE STATUS	*/
        public const byte S_CTS = 0x01;
        public const byte S_DSR = 0x02;
        public const byte S_RI = 0x04;
        public const byte S_CD = 0x08;
        /*  error code */
        public static  int SIO_OK = 0;
        public const int SIO_BADPORT = -1;             //No such port or port not opened //
        public const int SIO_OUTCONTROL = -2;         //Can't control board //
        public const int SIO_NODATA = -4;             //No data to read or no buffer to write //
        public const int SIO_OPENFAIL = -5;           // No such port or port has opened //
        public const int  SIO_RTS_BY_HW = -6;          //Can't set because H/W flowctrl //
        public const int  SIO_BADPARM = -7;           //Bad parameter //
        public const int  SIO_WIN32FAIL = -8;          //Call win32 function fail, please call GetLastError to get the error code //
        public const int  SIO_BOARDNOTSUPPORT  = -9;  //Board does not support this function//
        public const int  SIO_FAIL         = -10;     //PComm function run result fail //
        public const int  SIO_ABORT_WRITE  = -11;     //Write has blocked, and user abort write //
        public const int SIO_WRITETIMEOUT = -12;      //Write timeout has happened //

        // file transfer error code //
        public const int SIOFT_OK  = 0;
        public const int SIOFT_BADPORT = -1;	                      // No such port or port not open //
        public const int  SIOFT_TIMEOUT = -2;	                     // Protocol timeout //
        public const int  SIOFT_ABORT  = -3;	                    //User key abort //
        public const int  SIOFT_FUNC  = -4;	                   //Func return abort //
        public const int SIOFT_FOPEN  = -5;	                  // Can not open files //
        public const int SIOFT_CANABORT = -6;	                 // Ymodem CAN signal abort //
        public const int  SIOFT_PROTOCOL = -7;	                // Protocol checking error abort //
        public const int  SIOFT_SKIP  = -8;	               // Zmodem remote skip this send file //
        public const int  SIOFT_LACKRBUF = -9;	              // Zmodem Recv-Buff size must >= 2K bytes //
        public const int SIOFT_WIN32FAIL = -10;	         /* OS fail GetLastError to get the error code */
        public const int SIOFT_BOARDNOTSUPPORT = -11;	    // Board does not support this function//
        /*     global variable     */
        public static int Port;  
        public static int BaudRate;
        public static int Parity;
        public static int ByteSize;
        public static int StopBits;
        public static int iPort=0;
        public static int ibaudrate;
        public static int iparity;
        public static int ibytesize;
        public static int istopbits;
        public static Boolean Hw;
        public static Boolean Sw;
        public static Boolean Dtr;
        public static Boolean Rts;
        public static string recbuf1, recbuf2, s, s1, s2, s3, s4,s5, GszAppName;
        public static Boolean GbOpen, GhExit, GbRead;
//      public static char*  s5, pstr;
        public static List<String> list = new List<string>();
        public static int len, len1;
        public static int [] GBaudTable = {B50,B75,B110,B134,B150,B300,B600,B1200,B1800,B2400,B4800,
B7200,B9600,B19200,B38400,B57600,B115200,B230400,B460800,B921600 };
        public static int[] GParityTable = { P_NONE, P_EVEN, P_ODD, P_MRK, P_SPC };
        public static int[] GByteSizeTable = { BIT_5, BIT_6, BIT_7, BIT_8 };
        public static int[] GStopBitsTable = { STOP_1, STOP_2 };
        public static string[] GstrBaudTable = { "50","75","110","134","150","300","600","1200","1800","2400","4800",
"7200", "9600","19200","38400","57600","115200", "230400","460800","921600" };
        public static string[] GstrParityTable = { "None", "Even", "Odd", "Mark", "Space" };
        public static string[] GstrByteSizeTable = { "5", "6", "7" ,"8" };
        public static string[] GstrStopBitsTable = {"1","2" };

        public static string ShowStatus()
        {
            string szMessage = GszAppName;
            if (ExGlobal.GbOpen)
            {
                szMessage += "-- COM" + Port + ",";
                szMessage += ExGlobal.GstrBaudTable[ibaudrate] + ",";
                szMessage += ExGlobal.GstrParityTable[iparity] + ",";
                szMessage += ExGlobal.GstrByteSizeTable[ibytesize] + ",";
                szMessage += ExGlobal.GstrStopBitsTable[istopbits];
                if (Hw)
                { szMessage += ",RTS/CTS"; }
                if (Sw)
                { szMessage += ",XON/XOFF"; }
            }
            return szMessage;
        }
    }
}
