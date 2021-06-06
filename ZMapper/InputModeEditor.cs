using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace ZMapper
{
    public partial class InputModeEditor : Form
    {
        public InputModeEditor() {
            InitializeComponent();
        }

        static InputModeEditor Instance;

        public new static DialogResult ShowDialog(Form owner = null) {
            if (Instance == null || Instance.IsDisposed) Instance = new InputModeEditor();
            if (owner != null) Instance.TopMost = owner.TopMost;

            var result = ((Form)Instance).ShowDialog();
            if (result == DialogResult.Cancel) {
                MatchRegex = null;
            } else {
                if (Instance.radPattern.Checked) {
                    try {
                        MatchRegex = PatternToRegex(Instance.txtMatchString.Text);
                        var justToTest = new Regex(MatchRegex);
                    } catch (Exception ex) {
                        MessageBox.Show("An error occurred with the specified pattern.\n" + ex.GetType().ToString() + ": " + ex.Message, "Input Mode Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        result = DialogResult.Cancel;
                        MatchRegex = null;
                    }
                } else { // Regex
                    try {
                        MatchRegex = Instance.txtMatchString.Text;
                        var justToTest = new Regex(MatchRegex);
                    } catch (Exception ex) {
                        MessageBox.Show("An error occurred with the specified regular expression.\n" + ex.GetType().ToString() + ": " + ex.Message, "Input Mode Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        result = DialogResult.Cancel;
                        MatchRegex = null;
                    }
                }
            }

            return result;
        }

        public static string MatchRegex { get; private set; }

        public static string PatternToRegex(string pattern) {
            return "^" + Regex.Escape(pattern).Replace("\\?", ".").Replace("\\*", ".*") + "$";
        }

        private void btnOK_Click(object sender, EventArgs e) {
            if (radRegex.Checked) {
                try {
                    var justToTest = new Regex(txtMatchString.Text);
                } catch (Exception ex) {
                    MessageBox.Show("The regular expression is not valid.\n" + ex.GetType().ToString() + ": " + ex.Message, "Input Mode Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    DialogResult = System.Windows.Forms.DialogResult.None;
                }
            }

            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();

        }

    }
}
