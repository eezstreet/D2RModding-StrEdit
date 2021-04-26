using System.Windows.Forms;
using System;

namespace D2RModding_StrEdit
{
    public partial class FindReplace : Form
    {
        private string currentText;
        private bool findInKeys = true;

        public event EventHandler FindClicked;

        public class FindEventArgs : EventArgs
        {
            public bool inKeys { get; set; }
            public string text { get; set; }
        }

        public FindReplace()
        {
            InitializeComponent();
        }

        public void onFindClicked(object sender, EventArgs e)
        {
            FindEventArgs e1 = new FindEventArgs();
            e1.inKeys = findInKeys;
            e1.text = currentText;
            FindClicked.Invoke(sender, e1);
        }

        public void onRadioSelection(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            findInKeys = rb.Name.Equals("find_InKeys");
        }

        public void onTextChanged(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            currentText = tb.Text;
        }

        public void onKeyPressed(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                onFindClicked(sender, e);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
    }
}
