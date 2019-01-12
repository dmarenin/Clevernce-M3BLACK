using System;
using System.Threading;
using System.Windows.Forms;
using Cleverence.Barcoding;
using Cleverence.CompactForms;
using Microsoft.Win32;
using ImagerNet;

namespace Cleverence.Warehouse.Compact
{
    // Token: 0x02000003 RID: 3
    public class M3TScanner : BarcodeScanner
    {
        // Token: 0x0600000A RID: 10 RVA: 0x00002370 File Offset: 0x00000570
        public M3TScanner()
        {

            this._scannerNew = new Imager();
            this._scannerNew.ImagerDataEvent += this._scannerNew_ScannerDataEvent;
            if (!this._scannerNew.Open())
            {
                Log.Write("M3TScanner::Open failed");
            }
            else
            {
                Log.Write("M3TScanner::Open succed");
            }

            //this._scannerNew.RegHotKey(1, 125, false);

            this._scannerNew.SetSymbologyAll();
            EAN13_PARAMS ean13_PARAMS;
            this._scannerNew.GetEAN13(out ean13_PARAMS);
            //ean13_PARAMS.bBOOKLAND = false;
            ean13_PARAMS.bXCD = true;
            this._scannerNew.SetEAN13(ref ean13_PARAMS);
            //CODE25_PARAMS code25_PARAMS;
            //this._scannerNew.GetCODE25(out code25_PARAMS);
            //code25_PARAMS.bITF14 = true;
            //code25_PARAMS.bXCD = true;
            //this._scannerNew.SetCODE25(ref code25_PARAMS);

            FormControl.GlobalKeyDown += new KeyEventHandler(this.FormControl_GlobalKeyDown);
            FormControl.GlobalKeyUp += new KeyEventHandler(this.FormControl_GlobalKeyUp);
        }

        // Token: 0x0600000B RID: 11 RVA: 0x000024E0 File Offset: 0x000006E0
        private void FormControl_GlobalKeyDown(object sender, KeyEventArgs e)
        {
            if (!this.isTurnOn)
            {
                return;
            }
            if (this._fScanning)
            {
                return;
            }
            Keys keyCode = e.KeyCode;
            if (keyCode.ToString() != SCAN_KEY)
            {
                return;
            }
            e.Handled = true;

            bool flag = this._scannerNew.Read();
            Log.Write("scannerNew::Read: " + flag);

            this._fScanning = true;
            Thread.Sleep(50);
        }

        // Token: 0x0600000C RID: 12 RVA: 0x00002570 File Offset: 0x00000770
        private void FormControl_GlobalKeyUp(object sender, KeyEventArgs e)
        {
            if (!this.isTurnOn)
            {
                return;
            }
            if (!this._fScanning)
            {
                return;
            }
            Keys keyCode = e.KeyCode;
            if (keyCode.ToString() != SCAN_KEY)
            {
                return;
            }
            e.Handled = true;
            this.TurnOffBeam();
            this._fScanning = false;
        }

        // Token: 0x0600000D RID: 13 RVA: 0x000025B3 File Offset: 0x000007B3
        private void TurnOffBeam()
        {

            this._scannerNew.ReadCancel();

            Thread.Sleep(50);
        }

        // Token: 0x0600000E RID: 14 RVA: 0x000025E0 File Offset: 0x000007E0
        private void _scannerNew_ScannerDataEvent(object sender, ImagerNet.ImagerDataArgs e)
        {
            if (!this.isTurnOn)
            {
                return;
            }
            this.TurnOffBeam();
            string text = e.ScanData;
            if (e.ScanType == "UCC/EAN-128" && text.IndexOf("(") == -1)
            {
                string text2 = Ean128.Format(text);
                if (!string.IsNullOrEmpty(text2))
                {
                    text = text2;
                }
            }
            base.OnScan(text);
            Log.Write("_scannerNew_ScannerDataEvent::e: " + text);
        }

        // Token: 0x06000010 RID: 16 RVA: 0x00002697 File Offset: 0x00000897
        public override void TurnOn()
        {
            this.isTurnOn = true;
        }

        // Token: 0x06000011 RID: 17 RVA: 0x000026A0 File Offset: 0x000008A0
        public override void TurnOff()
        {
            this.isTurnOn = false;
            this.TurnOffBeam();
        }

        // Token: 0x17000003 RID: 3
        // (get) Token: 0x06000012 RID: 18 RVA: 0x000026AF File Offset: 0x000008AF
        public override bool IsTurnedOn
        {
            get
            {
                return this.isTurnOn;
            }
        }

        // Token: 0x06000013 RID: 19 RVA: 0x000026B7 File Offset: 0x000008B7
        public override void Dispose()
        {
            base.Dispose();

            if (this._scannerNew != null)
            {
                
                this._scannerNew.Close();
                
                Thread.Sleep(50);
                
                this._scannerNew = null;
                
                return;
            }
        }

        // Token: 0x04000008 RID: 8
        //private const Keys SCAN_KEY1 = 133;

        // Token: 0x0400000A RID: 10
        private Imager _scannerNew;

        // Token: 0x0400000C RID: 12
        private bool _fScanning;

        private string SCAN_KEY = "F14";

        // Token: 0x0400000E RID: 14
        private bool isTurnOn;

        // Token: 0x02000004 RID: 4
        // (Invoke) Token: 0x06000015 RID: 21
        private delegate void DecodeCallback(IntPtr pBuffer, int nLength);
    }
}
