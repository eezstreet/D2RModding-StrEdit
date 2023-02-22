using System;
using System.Windows.Forms;
using System.IO;
using System.Linq;

namespace D2RModding_StrEdit
{
    public partial class MainForm : Form
    {
        private string currentWorkspace = "";
        private Workspace workspace;
        private string selectedKey = "";
        private int selectedStrId = 0;
        private string selectedBank = "";
        private StringEntry.StringLanguages language = StringEntry.StringLanguages.LANG_enUS;
        private TextBox focusedTextBox;
        private int selectedColor = 0;

        private string[] colorCodeNames = new string[] { 
            "White 1", "White 2", "Gray 1", "Gray 2", "Gray 3", "Black 1", "Black 2", "Light Red",
            "Red 1", "Red 2", "Dark Red", "Orange 1", "Orange 2", "Orange 3", "Orange 4", "Light Gold 1",
            "Light Gold 2", "Gold 1", "Gold 2", "Yellow 1", "Yellow 2", "Green 1", "Green 2", "Green 3",
            "Green 4", "Dark Green 1", "Dark Green 2", "Turquoise", "Sky Blue", "Light Blue 1", "Light Blue 2",
            "Blue 1", "Blue 2", "Light Pink", "Pink", "Purple"
        };
        private string[] colorCodeValues = new string[] { 
            "ÿc0", "ÿc=", "ÿc5", "ÿcK", "ÿcI", "ÿc6", "ÿcM", "ÿcE", "ÿc1", "ÿcU", "ÿcS", "ÿc@", 
            "ÿc8", "ÿcJ", "ÿcL", "ÿc7", "ÿcH", "ÿc4", "ÿcD", "ÿc9", "ÿcR", "ÿc2", "ÿcQ", "ÿcC", 
            "ÿc<", "ÿcA", "ÿc:", "ÿcN", "ÿcT", "ÿcF", "ÿcP", "ÿc3", "ÿcB", "ÿcG", "ÿcO", "ÿc;"
        };

        public string CurrentWorkspace
        {
            get { return currentWorkspace; }
            set { currentWorkspace = value; UpdateText(); }
        }

        public bool modified = false;
        public bool Modified
        {
            get { return modified; }
            set { modified = value; UpdateText(); }
        }

        private void AddColors()
        {
            foreach (string s in colorCodeNames)
            {
                colorSelectBox.Items.Add(s);
            }
        }

        public MainForm()
        {
            InitializeComponent();
            UpdateText();
            AddColors();
        }

        public void UpdateText()
        {
            if(currentWorkspace.Equals(""))
            {
                Text = "D2RStrEdit - No workspace opened";
                toolStripStatusLabel2.Text = "No workspace loaded.";
            }
            else if(modified)
            {
                Text = "(*) D2RStrEdit - " + currentWorkspace;
                toolStripStatusLabel2.Text = "Workspace Loaded.";
            }
            else
            {
                Text = "D2RStrEdit - " + currentWorkspace;
                toolStripStatusLabel2.Text = "Workspace Loaded.";
            }
            
            toolStripStatusLabel1.Text = "";
            toolStripStatusLabel3.Text = "";
        }
        public void InitializeWorkspace()
        {
            string[] dirs = Directory.GetDirectories(currentWorkspace).Select(x => Path.GetFileName(x)).ToArray();
            string[] files = Directory.GetFiles(currentWorkspace).Select(x => Path.GetFileName(x)).ToArray();

            if(!Array.Exists(files, x => x.Equals("next_string_id.txt")) && 
                !Array.Exists(dirs, x => x.Equals("strings")) && 
                !Array.Exists(dirs, x => x.Equals("strings-legacy")))
            {
                MessageBox.Show("The workspace is invalid. Please select a directory with next_string_id.txt and strings/strings-legacy folders.",
                    "Error", MessageBoxButtons.OK);
                return;
            }

            Text = "D2RStrEdit - " + currentWorkspace;

            // Load the new workspace.
            workspace = new Workspace(currentWorkspace);

            // Update the list of banks.
            languageComboBox.Enabled = true;
            stringBankComboBox.DataSource = workspace.GetAllBanks();
            stringBankComboBox.Enabled = true;
            languageComboBox.SelectedIndex = 0;
            stringBankComboBox.SelectedIndex = 0;
            colorButton.Enabled = true;
            colorSelectBox.Enabled = true;
        }

        private void changeIndexToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        /**
         * Occurs when the "Open Workspace" button is pressed.
         */
        private void openWorkspaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenNewWorkspace();
        }

        /**
         * Occurs when the "Save Workspace" button is pressed.
         */
        private void saveWorkspaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveCurrentWorkspace();
        }

        /**
         * Occurs when the "Exit" button is pressed.
         */
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /**
         * Occurs when the "Rename Index" button is pressed.
         */
        private void renameIndexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RenameKey();
        }

        /**
         * Occurs when the "About" button is pressed.
         */
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox about = new AboutBox();
            about.ShowDialog();
            about.Dispose();
        }

        /**
         * Occurs when the "D2R Modding Discord" button is pressed.
         */
        private void d2RModdingDiscordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://discord.gg/MMgEH95daz");
        }

        /**
         * Occurs when the "Delete Key" button is pressed.
         */
        private void deleteKeyStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteCurrentKey();
        }

        private void changeBank(object sender, EventArgs e)
        {
            // when we change banks, we reset the contents of the string list
            StringEntry[] legacy, resurrected;
            ComboBox cb = sender as ComboBox;
            workspace.GetStringsInBank(cb.SelectedItem as string, out legacy, out resurrected);
            selectedBank = (string)cb.SelectedItem;

            stringListBox.DataSource = legacy.Union(resurrected).ToArray();
            stringListBox.Enabled = true;
        }

        private void changeSelectedString(object sender, EventArgs e)
        {
            // when we change selected string, we change text on legacy and resurrected
            ListBox lb = sender as ListBox;
            StringEntry se = lb.SelectedItem as StringEntry;
            if(se == null)
            {
                selectedKey = null;
                selectedStrId = -1;
                legacyTextBox.Enabled = resurrectedTextBox.Enabled = false;
                addButton.Enabled = removeButton.Enabled = renameIndexToolStripMenuItem.Enabled = deleteIndexToolStripMenuItem.Enabled = false;
                return;
            }
            selectedKey = se.Key;
            selectedStrId = se.id;
            legacyTextBox.Enabled = resurrectedTextBox.Enabled = true;
            addButton.Enabled = removeButton.Enabled = renameIndexToolStripMenuItem.Enabled = deleteIndexToolStripMenuItem.Enabled = true;
            if (!languageComboBox.Enabled)
            {
                languageComboBox.Enabled = true;
                languageComboBox.SelectedIndex = 0;
            }
            else
            {
                UpdateSelectedString();
            }
        }
        private void changeSelectedLanguage(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            StringEntry.StringLanguages language = (StringEntry.StringLanguages)cb.SelectedIndex;
            this.language = language;
            UpdateSelectedString();
        }
        private void UpdateSelectedString()
        {
            string legacy, resurrected;

            workspace.GetStringById(selectedBank, selectedStrId, language, out legacy, out resurrected);
            legacyTextBox.Enabled = true;
            resurrectedTextBox.Enabled = true;

            legacyTextBox.Text = legacy;
            resurrectedTextBox.Text = resurrected;
        }
        private void onTextGainedFocus(object sender, EventArgs e)
        {
            focusedTextBox = sender as TextBox;
        }
        private void onTextLostFocus(object sender, EventArgs e)
        {
            focusedTextBox = null;
        }

        private void insertColorCode(string text)
        {
            if (focusedTextBox == null)
            {
                MessageBox.Show("Please insert your cursor in some text.", "Error", MessageBoxButtons.OK);
                return;
            }

            focusedTextBox.Paste(text);
        }

        private void openFindReplace()
        {
            FindReplace fr = new FindReplace();
            fr.FindClicked += Fr_FindClicked;
            fr.ShowDialog();
        }
        private void onFindReplaceSelected(object sender, EventArgs e)
        {
            openFindReplace();
        }
        private void goTo_key(string key)
        {
            StringEntry[] x = stringListBox.DataSource as StringEntry[];
            for(var i = 0; i < x.Length; i++)
            {
                if(x[i].Key == key)
                {
                    stringListBox.SelectedIndex = i;
                    break;
                }
            }
        }
        private bool goTo_nextKeyInList(string[] keys)
        {
            if(keys.Contains(selectedKey))
            {
                // if selected key is last in list, go to first key in list
                if(keys.Last().Equals(selectedKey))
                {
                    goTo_key(keys.First());
                    return true;
                }

                // otherwise, go to next key
                for(var i = 0; i < keys.Length - 1; i++)
                {
                    if(keys[i].Equals(selectedKey))
                    {
                        goTo_key(keys[i + 1]);
                        break;
                    }
                }
            }
            else
            {
                // go to first key in list
                goTo_key(keys.First());
            }
            return false;
        }
        private void find_nextKey(string text)
        {
            if (workspace == null)
            {
                return;
            }

            string[] allMatchingKeys = workspace.FindKeysWithKeyMatching(text, selectedBank);
            if(allMatchingKeys.Length <= 0)
            {
                MessageBox.Show("No matches found.");
                return;
            }

            if(goTo_nextKeyInList(allMatchingKeys))
            {
                MessageBox.Show("No more matches found; returning to top.");
            }
        }
        private void find_nextValue(string text)
        {
            if (workspace == null) 
            {
                return;
            }
            string[] allMatchingKeys = workspace.FindKeysWithTextMatching(text, selectedBank, language);
            if(allMatchingKeys.Length <= 0)
            {
                MessageBox.Show("No matches found.");
                return;
            }

            if(goTo_nextKeyInList(allMatchingKeys))
            {
                MessageBox.Show("No more matches found; returning to top.");
            }
        }
        private void Fr_FindClicked(object sender, EventArgs e)
        {
            FindReplace.FindEventArgs fea = e as FindReplace.FindEventArgs;

            if (workspace == null)
            {
                return;
            }
            
            if(fea.inKeys)
            {
                find_nextKey(fea.text);
            }
            else
            {
                find_nextValue(fea.text);
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.F))
            { // Ctrl+F = find/replace
                openFindReplace();
                return true;
            }
            else if (keyData == (Keys.Control | Keys.O))
            { // Ctrl+O = open workspace
                OpenNewWorkspace();
                return true;
            }
            else if (keyData == (Keys.Control | Keys.S))
            { // Ctrl+S = save workspace
                SaveCurrentWorkspace();
                return true;
            }
            else if (keyData == (Keys.Control | Keys.Delete))
            { // Ctrl+Delete = delete selected key
                DeleteCurrentKey();
                return true;
            }
            else if (keyData == (Keys.Control | Keys.Up))
            { // Ctrl+Up = go to previous key
                ScrollKeyUp();
                return true;
            }
            else if (keyData == (Keys.Control | Keys.Down))
            { // Ctrl+Down = go to next key
                ScrollKeyDown();
                return true;
            }
            else if (keyData == (Keys.F2))
            { // F2 = rename key
                RenameKey();
                return true;
            }
            else if (keyData == (Keys.Control | Keys.D1))
            { // Ctrl+1 = select string list
                SelectStringList();
                return true;
            }
            else if (keyData == (Keys.Control | Keys.D2))
            { // Ctrl+2 = select resurrected string
                SelectResurrectedString();
                return true;
            }
            else if (keyData == (Keys.Control | Keys.D3))
            { // Ctrl+3 = select legacy string
                SelectLegacyString();
                return true;
            }
            /*else if(keyData == (Keys.Control | Keys.Q))
            { // Ctrl+Q = insert
                onInsertPressed();
                return true;
            }*/
            else if(keyData == (Keys.Control | Keys.R | Keys.Alt))
            { // Ctrl+Alt+R = add
                onAddPressed();
                return true;
            }
            else if(keyData == (Keys.Control | Keys.R))
            {
                onReversePressed();
                return true;
            }
            else if(keyData == (Keys.Control | Keys.Alt | Keys.Right))
            {
                onCopyFromKeyToValuePressed();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void SaveCurrentWorkspace()
        {
            workspace.SaveWorkspace();
            modified = false;
        }
        private void OpenNewWorkspace()
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.Description = "Choose a new workspace directory";
            dlg.ShowNewFolderButton = false;
            DialogResult result = dlg.ShowDialog();

            if (result == DialogResult.OK)
            {
                currentWorkspace = dlg.SelectedPath;
                modified = false;
                saveWorkspaceToolStripMenuItem.Enabled = true;
                InitializeWorkspace();
            }
        }
        private void DeleteCurrentKey()
        {
            if (workspace == null)
            {
                return;
            }
            workspace.RemoveString(selectedStrId);
            
            StringEntry[] legacy, resurrected;
            StringEntryEqualityComparer eq = new StringEntryEqualityComparer();
            workspace.GetStringsInBank(selectedBank, out legacy, out resurrected);
            var newDataSource = legacy.Union(resurrected, eq).ToArray();
            stringListBox.DataSource = newDataSource;
            stringListBox.SelectedIndex = -1;
        }
        private void ScrollKeyUp()
        {
            if(stringListBox.SelectedIndex != 0)
            {
                stringListBox.SelectedIndex--;
            }
        }
        private void ScrollKeyDown()
        {
            if(stringListBox.SelectedIndex < stringListBox.Items.Count - 1)
            {
                stringListBox.SelectedIndex++;
            }
        }
        private void RenameKey()
        {
            RenameKey dlg = new RenameKey();
            dlg.onRenameCommitted += Dlg_onRenameCommitted;
            dlg.ShowDialog();
        }

        private void Dlg_onRenameCommitted(object sender, EventArgs e)
        {
            RenameKey.RenameEventArgs re = e as RenameKey.RenameEventArgs;
            workspace.RenameKeyTo(selectedBank, selectedStrId, re.newName);
            modified = true;
        }
        private void onLegacyStringEdited(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            workspace.ChangeLegacyStringTo(selectedBank, selectedStrId, tb.Text, language);
            modified = true;
        }
        private void onResurrectedStringEdited(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            workspace.ChangeResurrectedStringTo(selectedBank, selectedStrId, tb.Text, language);
            modified = true;
        }
        private void onInsertPressed()
        {
            InsertString is1 = new InsertString();
            is1.onInsertCommitted += onInsertCommitted;
            is1.ShowDialog();
        }
        private void onAddPressed()
        {
            AddString add = new AddString();
            add.onAddCommitted += onAddCommitted;
            add.ShowDialog();
        }
        private void onCopyFromKeyToValuePressed()
        {
            string key = stringListBox.SelectedItem.ToString();
            resurrectedTextBox.Text = key;
            legacyTextBox.Text = key;
        }
        private void onReversePressed()
        {
            // reverse both resurrected and classic lines
            string resurrected = resurrectedTextBox.Text;
            string legacy = legacyTextBox.Text;
            string[] resStrings = resurrected.Split('\n');
            string[] legStrings = legacy.Split('\n');
            string[] revResStrings = resStrings.Reverse().ToArray();
            string[] revLegStrings = legStrings.Reverse().ToArray();
            revResStrings[0] = revResStrings[0] + "\r\n";
            revLegStrings[0] = revLegStrings[0] + "\r\n";

            resurrectedTextBox.Text = string.Join("\n", revResStrings);
            legacyTextBox.Text = string.Join("\n", revLegStrings);
        }
        private void onInsertCommitted(object sender, EventArgs e)
        {

        }
        private void onAddCommitted(object sender, EventArgs e)
        {
            StringEntry[] legacy, resurrected;
            AddString.AddStringEventArgs e1 = e as AddString.AddStringEventArgs;
            StringEntryEqualityComparer eq = new StringEntryEqualityComparer();
            if(e1.newStringName == null || workspace == null)
            {
                return;
            }
            workspace.AddString(e1.newStringName, selectedBank);
            workspace.GetStringsInBank(selectedBank, out legacy, out resurrected);
            var newDataSource = legacy.Union(resurrected, eq).ToArray();
            stringListBox.DataSource = newDataSource;
            stringListBox.SelectedIndex = newDataSource.Length - 1;
            modified = true;
        }
        private void onInsertPressed_Event(object sender, EventArgs e)
        {
            onInsertPressed();
        }
        private void onAddPressed_Event(object sender, EventArgs e)
        {
            onAddPressed();
        }
        private void onReversePressed_Event(object sender, EventArgs e)
        {
            onReversePressed();
        }
        private void SelectStringList()
        {
            stringListBox.Focus();
        }
        private void SelectLegacyString()
        {
            legacyTextBox.Focus();
            legacyTextBox.SelectAll();
        }
        private void SelectResurrectedString()
        {
            resurrectedTextBox.Focus();
            resurrectedTextBox.SelectAll();
        }
        private void ExportAsSpreadsheet()
        {
            // open a file dlg, use this as the spreadsheet to export to
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "tab-delimited txt files (*.txt)|*.txt|All files (*.*)|*.*";
            dlg.DefaultExt = ".txt";

            if(dlg.ShowDialog() == DialogResult.OK)
            {
                string filePath = dlg.FileName;

            }
        }
        private void CompileSpreadsheet()
        {
            // open a file dlg, use this as the spreadsheet to compile
        }
        private void onExportAsSpreadsheet_Pressed(object sender, EventArgs e)
        {
            ExportAsSpreadsheet();
        }
        private void onCompileSpreadsheet_Pressed(object sender, EventArgs e)
        {
            CompileSpreadsheet();
        }

        private void colorSelectBox_IndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            selectedColor = cb.SelectedIndex;
        }

        private void colorButton_Click(object sender, EventArgs e)
        {
            insertColorCode(colorCodeValues[selectedColor]);
        }
    }
}
