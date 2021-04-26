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
    public partial class RenameKey : Form
    {
        public event EventHandler onRenameCommitted;
        private string theNewName;
        public class RenameEventArgs : EventArgs
        {
            public string newName { get; set; }
        }
        public RenameKey()
        {
            InitializeComponent();
        }
        private void PressCancel()
        {
            Close();
        }
        private void PressOK()
        {
            RenameEventArgs e1 = new RenameEventArgs();
            e1.newName = theNewName;
            onRenameCommitted.Invoke(this, e1);
            Close();
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
        private void onButtonClicked(object sender, EventArgs e)
        {
            Button b = sender as Button;
            if(b == okButton)
            {
                PressOK();
            }
            else if(b == cancelButton)
            {
                PressCancel();
            }
        }
        private void onTextChanged(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            theNewName = tb.Text;
        }
    }
}
