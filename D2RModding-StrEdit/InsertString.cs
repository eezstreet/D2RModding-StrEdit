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
    public partial class InsertString : Form
    {
        public event EventHandler onInsertCommitted;
        private string currentName;
        private bool insertBeforeSelected = true;
        public class InsertStringEventArgs : EventArgs
        {
            public string newStringName { get; set; }
            public bool insertBefore { get; set; }
        }
        public InsertString()
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
        public void onBeforeSelected(object sender, EventArgs e)
        {
            insertBeforeSelected = true;
        }
        public void onAfterSelected(object sender, EventArgs e)
        {
            insertBeforeSelected = false;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter || keyData == Keys.Return)
            {
                // same as pressing OK
                PressOK();
                return true;
            }
            else if (keyData == Keys.Escape)
            {
                // same as pressing CANCEL
                PressCancel();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        void PressOK()
        {
            InsertStringEventArgs e1 = new InsertStringEventArgs();
            e1.newStringName = currentName;
            e1.insertBefore = insertBeforeSelected;
            onInsertCommitted.Invoke(this, e1);
            Close();
            Close();
        }
        void PressCancel()
        {
            Close();
        }
    }
}
