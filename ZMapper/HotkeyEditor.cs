using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace ZMapper
{
    public partial class HotkeyEditor : Form
    {
        public HotkeyEditor() {
            InitializeComponent();

        }

        public void SetMappings(IDictionary<Keys, MapOperation> mapping) {
            var mappingProps = new HotkeyPropertyContainer();
            mappingProps.CopyFrom(mapping);
            this.propertyGrid1.SelectedObject = mappingProps;
        }

        private void propertyGrid1_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e) {
        }
    }

    class KeyInputForm : Form
    {
        internal KeyInputForm() {
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterParent;
            Width = 150;
            Height = 64;
            KeyPreview = true;
            ShowInTaskbar = false;

            Label pressKey = new Label() {
                BackColor = SystemColors.Info,
                BorderStyle = BorderStyle.FixedSingle,
                Text = "Press a key...",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.TopCenter,
            };

            Button btnCancel = new Button() {
                Left = 10,
                Top = 30,
                Height = 20,
                Width = 60,
                Text = "&Cancel",
            };

            Button btnReset = new Button() {
                Left = 80,
                Top = 30,
                Height = 20,
                Width = 60,
                Text = "&Reset",
            };

            btnCancel.Click += (sender, args) => {
                DialogResult = System.Windows.Forms.DialogResult.Cancel;
                PressedKey = null;
                PressedKeyChar = null;
            };
            btnReset.Click += (sender, args) => {
                DialogResult = System.Windows.Forms.DialogResult.OK;
                PressedKey = Keys.None;
                PressedKeyChar = null;
            };

            Controls.Add(btnReset);
            Controls.Add(btnCancel);
            Controls.Add(pressKey);

            pressKey.Focus();
        }

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);
        }

        public Keys? PressedKey { get; private set; }
        public string PressedKeyChar { get; private set; }

        protected override void OnVisibleChanged(EventArgs e) {
            base.OnVisibleChanged(e);
            if (this.Visible) PressedKeyChar = "";
        }

        //public new DialogResult ShowDialog(IWin32Window owner = null) {
        //    PressedKeyChar = "";
        //    if (owner == null) return base.ShowDialog();
        //    return base.ShowDialog(owner);
        //}

        protected override void OnKeyDown(KeyEventArgs e) {
            base.OnKeyDown(e);

            PressedKey = e.KeyCode;
        }

        protected override void OnKeyPress(KeyPressEventArgs e) {
            base.OnKeyPress(e);

            PressedKeyChar = e.KeyChar.ToString();
        }

        protected override void OnKeyUp(KeyEventArgs e) {
            base.OnKeyUp(e);

            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    sealed class HotkeyMappingAttribute : Attribute
    {
        public HotkeyMappingAttribute(MapOperation op) { this.Operation = op; }
        public MapOperation Operation { get; private set; }
    }

    class HotkeyPropertyContainer
    {
        internal HotkeyPropertyContainer() {
            DirUp = Keys.NumPad8;
        }

        public void CopyFrom(IDictionary<Keys, MapOperation> sourceMapping) {
            var properties = this.GetType().GetProperties();
            foreach (var prop in properties) {
                var atts = prop.GetCustomAttributes(typeof (HotkeyMappingAttribute), false);
                if(atts.Length == 1) {
                    var operation = ((HotkeyMappingAttribute)atts[0]).Operation;
                    Keys? key = GetAssociatedKey(sourceMapping, operation);
                    if (key != null) {
                        prop.SetValue(this, key.Value, null);
                    } else {
                        prop.SetValue(this, Keys.None, null);
                    }
                }
            }
        }

        public IDictionary<Keys, MapOperation> GetMapping() {
            var result = new Dictionary<Keys, MapOperation>();

            var properties = this.GetType().GetProperties();
            foreach (var prop in properties) {
                var atts = prop.GetCustomAttributes(typeof(HotkeyMappingAttribute), false);
                if (atts.Length == 1) {
                    var operation = ((HotkeyMappingAttribute)atts[0]).Operation;
                    object key = prop.GetValue(this, null);
                    if (key is Keys) result[(Keys)key] = operation;
                }
            }

            return result;
        }

        private Keys? GetAssociatedKey(IDictionary<Keys, MapOperation> sourceMapping, MapOperation operation) {
            foreach (var entry in sourceMapping) {
                if (entry.Value == operation) return entry.Key;
            }

            return null;
        }

        [Description("Navigate the map and mark walls.")]
        [DisplayName("Up")]
        [Category("Directional")]
        [Editor(typeof(KeyEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(KeysConverter))]
        [HotkeyMapping(MapOperation.NavNorth)]
        public Keys DirUp { get; set; }
        private bool ShouldSerializeDirUp() { return false; }

        [Description("Navigate the map and mark walls.")]
        [DisplayName("Down")]
        [Category("Directional")]
        [Editor(typeof(KeyEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(KeysConverter))]
        [HotkeyMapping(MapOperation.NavSouth)]
        public Keys DirDown { get; set; }
        private bool ShouldSerializeDirDown() { return false; }

        [Description("Navigate the map and mark walls.")]
        [DisplayName("Left")]
        [Category("Directional")]
        [Editor(typeof(KeyEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(KeysConverter))]
        [HotkeyMapping(MapOperation.NavWest)]
        public Keys DirLeft { get; set; }
        private bool ShouldSerializeDirLeft() { return false; }

        [Description("Navigate the map and mark walls.")]
        [DisplayName("Right")]
        [Category("Directional")]
        [Editor(typeof(KeyEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(KeysConverter))]
        [HotkeyMapping(MapOperation.NavEast)]
        public Keys DirRight { get; set; }
        private bool ShouldSerializeDirRight() { return false; }


        [Description("Mark overworld boundaries or add dungeon room connections")]
        [DisplayName("Connections")]
        [Category("Markers")]
        [Editor(typeof(KeyEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(KeysConverter))]
        [HotkeyMapping(MapOperation.MarkMode)]
        public Keys Connections { get; set; }
        private bool ShouldSerializeConnections() { return false; }

        [Category("Markers")]
        [DisplayName("Bomb")]
        [Description("Toggle bomb marker (overworld) or mark bombed walls (dungeon)")]
        [Editor(typeof(KeyEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(KeysConverter))]
        [HotkeyMapping(MapOperation.MarkBombed)]
        public Keys Bomb { get; set; }
        private bool ShouldSerializeBomb() { return false; }

        [Category("Markers")]
        [DisplayName("Recorder")]
        [Description("Toggle recorder marker (overworld)")]
        [Editor(typeof(KeyEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(KeysConverter))]
        [HotkeyMapping(MapOperation.MarkFluted)]
        public Keys Recorder { get; set; }
        private bool ShouldSerializeRecorder() { return false; }

        [Category("Markers")]
        [DisplayName("Candle")]
        [Description("Toggle candle marker (overworld)")]
        [Editor(typeof(KeyEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(KeysConverter))]
        [HotkeyMapping(MapOperation.MarkBurned)]
        public Keys Candle { get; set; }
        private bool ShouldSerializeCandle() { return false; }


        [Category("Markers")]
        [DisplayName("Clear")]
        [Description("Mark screen as cleared")]
        [Editor(typeof(KeyEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(KeysConverter))]
        [HotkeyMapping(MapOperation.MarkClear)]
        public Keys Clear { get; set; }
        private bool ShouldSerializeClear() { return false; }


        [Category("Markers")]
        [DisplayName("POI")]
        [Description("Add a point-of-interest marker")]
        [Editor(typeof(KeyEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(KeysConverter))]
        [HotkeyMapping(MapOperation.MarkPoi)]
        public Keys POI { get; set; }
        private bool ShouldSerializePOI() { return false; }

        [Category("Other")]
        [DisplayName("Unreveal Screen")]
        [Description("Mark a screen as unvisited")]
        [Editor(typeof(KeyEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(KeysConverter))]
        [HotkeyMapping(MapOperation.MarkUnvisited)]
        public Keys DeleteScreen { get; set; }
        private bool ShouldSerializeDeleteScreen() { return false; }

        [Category("Other")]
        [DisplayName("Undo")]
        [Description("Undoes last change")]
        [Editor(typeof(KeyEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(KeysConverter))]
        [HotkeyMapping(MapOperation.Undo)]
        public Keys Undo { get; set; }
        private bool ShouldSerializeUndo() { return false; }


        [Category("Map Selection")]
        [DisplayName("Overworld")]
        [Description("Navigates to the overworld")]
        [Editor(typeof(KeyEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(KeysConverter))]
        [HotkeyMapping(MapOperation.GotoOverworld)]
        public Keys Overworld { get; set; }
        private bool ShouldSerializeOverworld() { return false; }

        [Category("Map Selection")]
        [DisplayName("Level 1")]
        [Description("Navigates to Level 1")]
        [Editor(typeof(KeyEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(KeysConverter))]
        [HotkeyMapping(MapOperation.GotoDungeon1)]
        public Keys Level1 { get; set; }
        private bool ShouldSerializeLevel1() { return false; }

        [Category("Map Selection")]
        [DisplayName("Level 2")]
        [Description("Navigates to Level 2")]
        [Editor(typeof(KeyEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(KeysConverter))]
        [HotkeyMapping(MapOperation.GotoDungeon2)]
        public Keys Level2 { get; set; }
        private bool ShouldSerializeLevel2() { return false; }

        [Category("Map Selection")]
        [DisplayName("Level 3")]
        [Description("Navigates to Level 3")]
        [Editor(typeof(KeyEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(KeysConverter))]
        [HotkeyMapping(MapOperation.GotoDungeon3)]
        public Keys Level3 { get; set; }
        private bool ShouldSerializeLevel3() { return false; }

        [Category("Map Selection")]
        [DisplayName("Level 4")]
        [Description("Navigates to Level 4")]
        [Editor(typeof(KeyEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(KeysConverter))]
        [HotkeyMapping(MapOperation.GotoDungeon4)]
        public Keys Level4 { get; set; }
        private bool ShouldSerializeLevel4() { return false; }

        [Category("Map Selection")]
        [DisplayName("Level 5")]
        [Description("Navigates to Level 5")]
        [Editor(typeof(KeyEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(KeysConverter))]
        [HotkeyMapping(MapOperation.GotoDungeon5)]
        public Keys Level5 { get; set; }
        private bool ShouldSerializeLevel5() { return false; }

        [Category("Map Selection")]
        [DisplayName("Level 6")]
        [Description("Navigates to Level 6")]
        [Editor(typeof(KeyEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(KeysConverter))]
        [HotkeyMapping(MapOperation.GotoDungeon6)]
        public Keys Level6 { get; set; }
        private bool ShouldSerializeLevel6() { return false; }

        [Category("Map Selection")]
        [DisplayName("Level 7")]
        [Description("Navigates to Level 7")]
        [Editor(typeof(KeyEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(KeysConverter))]
        [HotkeyMapping(MapOperation.GotoDungeon7)]
        public Keys Level7 { get; set; }
        private bool ShouldSerializeLevel7() { return false; }

        [Category("Map Selection")]
        [DisplayName("Level 8")]
        [Description("Navigates to Level 8")]
        [Editor(typeof(KeyEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(KeysConverter))]
        [HotkeyMapping(MapOperation.GotoDungeon8)]
        public Keys Level8 { get; set; }
        private bool ShouldSerializeLevel8() { return false; }

        [Category("Map Selection")]
        [DisplayName("Level 9")]
        [Description("Navigates to Level 9")]
        [Editor(typeof(KeyEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(KeysConverter))]
        [HotkeyMapping(MapOperation.GotoDungeon9)]
        public Keys Level9 { get; set; }
        private bool ShouldSerializeLevel9() { return false; }


    }

    class KeysConverter: TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
            return false;
            //return sourceType == typeof (string);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
            return base.CanConvertTo(context, destinationType);
            return destinationType == typeof(string);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType) {
            if (destinationType == typeof(string) && value is Keys) return FormatKey((Keys)value);
            return base.ConvertTo(context, culture, value, destinationType);
        }

        private string FormatKey(Keys value) {
            return KeyStrings.FormatKey(value);
            //if (foo.Length > 1 && !foo.StartsWith("Numpad") && !string.IsNullOrEmpty(form.PressedKeyChar)) {
            //    var charDisplay = form.PressedKeyChar.Trim();
            //    if (charDisplay.Length > 0) foo += " { " + charDisplay + " }";
            //}
        }
    }

    class KeyEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) {
            return UITypeEditorEditStyle.Modal;
        }
        public override object EditValue(ITypeDescriptorContext context, System.IServiceProvider provider, object value) {
            IWindowsFormsEditorService svc = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            Keys? foo = (value is Keys?) ? (Keys?)value : null;
            if (svc != null && foo != null) {
                using (KeyInputForm form = new KeyInputForm()) {
                    //form.Value = foo.Bar;
                    if (svc.ShowDialog(form) == DialogResult.OK) {
                        //foo.Bar = form.Value; // update object
                        foo = form.PressedKey;
                        //foo = KeyStrings.FormatKey(form.PressedKey);
                        //if (foo.Length > 1 && !foo.StartsWith("Numpad") && !string.IsNullOrEmpty(form.PressedKeyChar)) {
                        //    var charDisplay = form.PressedKeyChar.Trim();
                        //    if(charDisplay.Length > 0) foo += " { " + charDisplay + " }";
                        //}
                    }
                }
            }
            return foo; // can also replace the wrapper object here
        }
    }

    static class KeyStrings
    {
        #region Key Listing
        internal readonly static Dictionary<Keys, string> DisplayNames = new Dictionary<Keys, string>() {
           { Keys.LButton, "Left Button"},
           { Keys.RButton, "Right Button"},
           { Keys.Cancel, "Cancel"},
           { Keys.MButton, "Middle Button"},
           { Keys.XButton1, "X-Button 1"},
           { Keys.XButton2, "X-Button 2"},
           { Keys.ShiftKey, "Shift"},
           { Keys.ControlKey, "Control"},
           { Keys.CapsLock, "Caps Lock"},
           { Keys.PageUp, "Page Up"},
           { Keys.PageDown, "Page Down"},
           { Keys.PrintScreen, "Print Screen"},
           { Keys.D0, "0"},
           { Keys.D1, "1"},
           { Keys.D2, "2"},
           { Keys.D3, "3"},
           { Keys.D4, "4"},
           { Keys.D5, "5"},
           { Keys.D6, "6"},
           { Keys.D7, "7"},
           { Keys.D8, "8"},
           { Keys.D9, "9"},
           { Keys.LWin, "Left Meta"},
           { Keys.RWin, "Right Meta"},
           { Keys.Apps, "App Menu"},
           { Keys.NumPad0, "Numpad 0"},
           { Keys.NumPad1, "Numpad 1"},
           { Keys.NumPad2, "Numpad 2"},
           { Keys.NumPad3, "Numpad 3"},
           { Keys.NumPad4, "Numpad 4"},
           { Keys.NumPad5, "Numpad 5"},
           { Keys.NumPad6, "Numpad 6"},
           { Keys.NumPad7, "Numpad 7"},
           { Keys.NumPad8, "Numpad 8"},
           { Keys.NumPad9, "Numpad 9"},
           { Keys.Multiply, "Numpad *"},
           { Keys.Enter, "Enter"},
           { Keys.Add, "Numpad +"},
           { Keys.Separator, "Separator"},
           { Keys.Subtract, "Numpad -"},
           { Keys.Decimal, "Numpad ."},
           { Keys.Divide, "Numpad /"},
           { Keys.NumLock, "Num Lock"},
           { Keys.LShiftKey, "Left Shift"},
           { Keys.RShiftKey, "Right Shift"},
           { Keys.LControlKey, "Left Control"},
           { Keys.RControlKey, "Right Control"},
           { Keys.LMenu, "Left Menu"},
           { Keys.RMenu, "Right Menu"},
           { Keys.Back, "Backspace"}, 
           { Keys.BrowserBack, "Navigate Back"},
           { Keys.BrowserForward, "Navigate Forward"},
           { Keys.BrowserRefresh, "Refresh"},
           { Keys.BrowserStop, "Stop"},
           { Keys.BrowserSearch, "Search"},
           { Keys.BrowserFavorites, "Favorites"},
           { Keys.BrowserHome, "Navigate Home"},
           { Keys.VolumeMute, "Mute"},
           { Keys.VolumeDown, "Volume Down"},
           { Keys.VolumeUp, "Volume Up"},
           { Keys.MediaNextTrack, "Next Track"},
           { Keys.MediaPreviousTrack, "Previous Track"},
           { Keys.MediaStop, "Stop Playback"},
           { Keys.MediaPlayPause, "Pause Playback"},
           { Keys.LaunchMail, "Open Mail"},
           { Keys.SelectMedia, "Select Media"},
           { Keys.LaunchApplication1, "Lanch App 1"},
           { Keys.LaunchApplication2, "Launch App 2"},
           { Keys.OemSemicolon, ""},
           { Keys.Oemplus, "Plus"},
           { Keys.Oemcomma, "Comma"},
           { Keys.OemMinus, "Minus"},
           { Keys.OemPeriod, "Period"},
           { Keys.OemQuestion, "Slash"},
           { Keys.Oemtilde, "Tilde"},
           { Keys.OemOpenBrackets, "Left Bracket"},
           { Keys.OemCloseBrackets, "Right Bracket"},
           { Keys.OemPipe, "Backslash"},
           { Keys.Oem8, "OEM 8"},
           { Keys.OemQuotes, "Quote"},
           { Keys.OemBackslash, "Backslash"},
           { Keys.ProcessKey, "Process Key"},
           { Keys.OemClear, "Clear"},
        };
        #endregion

        internal static string FormatKey(Keys keycode) {
            string keyString;
            if (DisplayNames.TryGetValue(keycode, out keyString)) return keyString;
            return keycode.ToString();
        }
    }
}
