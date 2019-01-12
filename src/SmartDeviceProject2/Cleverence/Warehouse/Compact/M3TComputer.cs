using System;
using System.Text;
using System.Windows.Forms;

namespace Cleverence.Warehouse.Compact
{
    // Token: 0x02000002 RID: 2
    public class M3TComputer : MobileComputer
    {
        // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        public M3TComputer()
        {
            this.scanner = new M3TScanner();
        }

        // Token: 0x17000001 RID: 1
        // (get) Token: 0x06000002 RID: 2 RVA: 0x00002063 File Offset: 0x00000263
        public override BarcodeScanner Scanner
        {
            get
            {
                return this.scanner;
            }
        }

        // Token: 0x06000003 RID: 3 RVA: 0x0000206C File Offset: 0x0000026C
        public override string GetDeviceId()
        {
            string result;
            try
            {
                string text = "M3";

                var m_System = new SystemNet.SystemNet();
                    
                var m_DeviceInfo = m_System.GetDeviceInfo();

                string text3 = m_System.GetSerialNumber();

                string text2 = m_System.GetGuid();

                text = text + "-" + text2;

                text = text + "-" + text3;

                text = text.Replace("UUID : ", "");
                
                //MessageBox.Show(text);

                //string text = "M3";
                //string text2 = "";
                //StringBuilder stringBuilder = new StringBuilder();
                //byte[] rawDeviceID = M3TComputer.GetRawDeviceID(M3TComputer.IOCTL_GET_GUID, 16u);
                //if (rawDeviceID != null)
                //{
                //    for (int i = 0; i < rawDeviceID.Length; i++)
                //    {
                //        stringBuilder.Append(string.Format("{0:X2}", rawDeviceID[i]));
                //    }
                //    text2 = stringBuilder.ToString();
                //    text = text + "-" + text2;
                //}
                //string text3 = null;
                //byte[] rawDeviceID2 = M3TComputer.GetRawDeviceID(M3TComputer.IOCTL_GET_SERIAL_NUMBER, 14u);
                //if (rawDeviceID2 != null)
                //{
                //    text3 = M3TComputer.GetString(rawDeviceID2);
                //    if (!string.IsNullOrEmpty(text3) && !text3.StartsWith("??"))
                //    {
                //        text = text + "-" + text3;
                //    }
                //}

                if (string.IsNullOrEmpty(text2) && string.IsNullOrEmpty(text3))
                {
                    Log.Write("Не удалось сформировать DeviceId");
                    result = "DeviceId не сформирован";
                }
                else
                {
                    result = base.CleanupDeviceId(text);
                }
            }
            catch (Exception ex)
            {
                Log.Write("Не удалось сформировать DeviceId", ex);
                result = "DeviceId не сформирован";
            }
            return result;
        }

        // Token: 0x06000004 RID: 4 RVA: 0x00002178 File Offset: 0x00000378
        private static string GetString(byte[] bBuffer)
        {
            if (bBuffer.Length <= 1)
            {
                return "";
            }
            int i;
            Encoding encoding;
            if (bBuffer[1] == 0)
            {
                for (i = 0; i < bBuffer.Length; i += 2)
                {
                    ushort num = BitConverter.ToUInt16(bBuffer, i);
                    if (num == 0)
                    {
                        break;
                    }
                }
                encoding = Encoding.Unicode;
            }
            else
            {
                i = 0;
                while (i < bBuffer.Length && bBuffer[i] != 0)
                {
                    i++;
                }
                encoding = Encoding.ASCII;
            }
            string text = encoding.GetString(bBuffer, 0, i);
            string text2 = text;
            char[] array = new char[1];
            text = text2.Trim(array);
            return text.Trim();
        }

        // Token: 0x06000005 RID: 5 RVA: 0x000021F3 File Offset: 0x000003F3
        private static byte[] GetRawDeviceID(int data)
        {
            return M3TComputer.GetRawDeviceID(data, 0u);
        }

        // Token: 0x06000006 RID: 6 RVA: 0x000021FC File Offset: 0x000003FC
        private static byte[] GetRawDeviceID(int data, uint resLength)
        {
            int num = 0;
            int num2 = 256;
            byte[] array = new byte[num2];
            BitConverter.GetBytes(num2).CopyTo(array, 0);
            if (NativeMethods.KernelIoControl((uint)data, null, 0, array, num2, ref num))
            {
                int num3 = (resLength == 0u) ? num : ((int)resLength);
                byte[] array2 = new byte[num3];
                Buffer.BlockCopy(array, 0, array2, 0, num3);
                return array2;
            }
            return null;
        }

        // Token: 0x06000007 RID: 7 RVA: 0x00002252 File Offset: 0x00000452
        public override void Dispose()
        {
            base.Dispose();
            if (this.scanner != null)
            {
                this.scanner.Dispose();
            }
        }

        // Token: 0x04000001 RID: 1
        internal const int METHOD_BUFFERED = 0;

        // Token: 0x04000002 RID: 2
        internal const int FILE_ANY_ACCESS = 0;

        // Token: 0x04000003 RID: 3
        internal const int FILE_DEVICE_HAL = 257;

        // Token: 0x04000004 RID: 4
        private M3TScanner scanner;

        // Token: 0x04000005 RID: 5
        internal static int IOCTL_GET_SERIAL_NUMBER = 16858780;

        // Token: 0x04000006 RID: 6
        internal static int IOCTL_GET_GUID = 16858768;

        // Token: 0x04000007 RID: 7
        internal static int IOCTL_HAL_GET_DEVICEID = 16842836;
    }
}
