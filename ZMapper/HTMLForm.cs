using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZMapper.Properties;

namespace ZMapper
{
    public partial class HTMLForm : Form
    {
        static HTMLForm HelpForm;

        public HTMLForm() {
            InitializeComponent();
        }

        private void ShowTopic(string topic) {
            switch (topic) {
                case "about":
                    ShowHtml(Resources.HelpAbout);
                    break;
                case "input":
                    ShowHtml(Resources.HelpInput);
                    break;
                case "license":
                    ShowHtml(Resources.HelpLicense);
                    break;
                case "options":
                    ShowHtml(Resources.HelpOptions);
                    break;
                default:
                    ShowHtml(Resources.HelpNotFound);
                    break;
            }
        }

        protected override void OnActivated(EventArgs e) {
            base.OnActivated(e);


            if(Owner != null) this.TopMost = Owner.TopMost;
        }

        void ShowHtml(string html) {
            this.contentPanel.BaseStylesheet = Resources.HelpCSS;
            this.contentPanel.Text = Program.InsertVersion(html);
        }

        public static void ShowInput(Form owner = null) {
            EnsureInstance();
            HelpForm.ShowTopic("input");
            HelpForm.Owner = owner;
            if(owner != null) HelpForm.TopMost = owner.TopMost;

            HelpForm.Show();
        }

        public static void ShowAbout(Form owner = null) {
            EnsureInstance();
            HelpForm.ShowTopic("about");
            HelpForm.Owner = owner;
            if (owner != null) HelpForm.TopMost = owner.TopMost;
            HelpForm.Show();
        }

        private static void EnsureInstance() {
            if (HelpForm == null || HelpForm.IsDisposed) {
                HelpForm = new HTMLForm();
            }
        }

        private void contentPanel_LinkClicked(object sender, TheArtOfDev.HtmlRenderer.Core.Entities.HtmlLinkClickedEventArgs e) {
            if (e.Link.StartsWith("page://")) {
                e.Handled = true;
                var topic = e.Link.Substring(7);
                ShowTopic(topic);
            } else if (e.Link.StartsWith("#")) {
                e.Handled = true;
                var id = e.Link.Substring(1);
                contentPanel.ScrollToElement(id);
            }
        }

        private void contentPanel_ImageLoad(object sender, TheArtOfDev.HtmlRenderer.Core.Entities.HtmlImageLoadEventArgs e) {
            if (e.Src.StartsWith("img://")) {
                e.Handled = true;
                var name = e.Src.Substring(6);
                switch (name) {
                    case "input":
                        e.Callback(Resources.img_input);
                        break;
                    case "input2":
                        e.Callback(Resources.Input2);
                        break;
                    case "RedDot":
                        e.Callback(Resources.RedDot);
                        break;
                    case "help":
                        e.Callback(Resources.Help);
                        break;
                }
            }
        }

    }
}
