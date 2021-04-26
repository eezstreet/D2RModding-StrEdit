using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace D2RModding_StrEdit
{
    public partial class AddString : Form
    {
        private string currentName;
        public event EventHandler onAddCommitted;
        public class AddStringEventArgs : EventArgs
        {
            public string newStringName { get; set; }
        }
        public AddString()
        {
            InitializeComponent();
        }
        public void onStringChanged(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            currentName = tb.Text;
        }
        public void onOkPressed(object sender, EventArgs e)
        {
            PressOK();
        }
        public void onCancelPressed(object sender, EventArgs e)
        {
            PressCancel();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if(keyData == Keys.Enter || keyData == Keys.Return)
            {
                // same as pressing OK
                PressOK();
                return true;
            }
            else if(keyData == Keys.Escape)
            {
                // same as pressing CANCEL
                PressCancel();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void PressOK()
        {
            AddStringEventArgs e1 = new AddStringEventArgs();
            e1.newStringName = currentName;
            onAddCommitted.Invoke(this, e1);
            Close();
        }
        private void PressCancel()
        {
            Close();
        }
    }
}
