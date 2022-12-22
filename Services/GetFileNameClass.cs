using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace bagant.Services
{
    class GetFileNameClass
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        FolderBrowserDialog _oFileDialog;

        // Properties
        public string Path
        {
            get { return _oFileDialog.SelectedPath; }
            set { _oFileDialog.SelectedPath = value; }
        }

        // Constructor
        public GetFileNameClass()
        {
            _oFileDialog = new FolderBrowserDialog();
        }

        // Methods

        public void GetFileName()
        {
            IntPtr ptr = GetForegroundWindow();
            WindowWrapper oWindow = new WindowWrapper(ptr);
            if (_oFileDialog.ShowDialog(oWindow) != DialogResult.OK)
            {
                _oFileDialog.SelectedPath = string.Empty;
            }
            oWindow = null;
        } // End of GetFileName
    }
}
