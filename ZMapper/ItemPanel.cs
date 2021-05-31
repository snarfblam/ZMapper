using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using ZMapper.Properties;
using FasTrak;
using System.Windows.Forms;
using System.ComponentModel;

namespace ZMapper
{
    class ItemPanel: BufferedPanel
    {
        Bitmap BaseImage = (Bitmap)Resources.items0.Clone();
        Graphics gfx;
        static Bitmap[] Images = { Resources.items0, Resources.items1, Resources.items2, Resources.items3 };
        MapIconLayout icons = new MapIconLayout();

        public ItemPanel() {
            base.BackgroundImage = BaseImage;
            this.gfx = Graphics.FromImage(BaseImage);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Image BackgroundImage {
            get {
                return null;
            }
            set {
                base.BackgroundImage = value;
            }
        }

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e) {
            base.OnMouseDown(e);

            int amt = 0;
            if (e.Button == System.Windows.Forms.MouseButtons.Left) amt = 1;
            if (e.Button == System.Windows.Forms.MouseButtons.Right) amt = -1;
            if (amt == 0) return;

            var icon = icons.IconAt(e.X, e.Y);
            if (icon != null) {
                var state = icons.SetState(icon, icons.GetState(icon).Value + amt).Value;
                var rect = icons.GetBounds(icon).Value;
                var src = GetStateImage(state);
                gfx.DrawImage(src, rect, rect, GraphicsUnit.Pixel);
                this.Invalidate(rect);
            }
        }

        private static Bitmap GetStateImage(int state) {
            return Images[state.Clamp(0, Images.Length - 1)];
        }

        internal Cereal Serialize() {
            return this.icons.Serialize();
        }

        internal void Deserialize(Cereal data) {
            this.icons.Deserialize(data);
            RenderAll();
        }

        internal void ClearItemData() {
            foreach (var icon in icons.Icons) {
                icon.Value = 0;
            }

            RenderAll();
        }

        private void RenderAll() {
            gfx.DrawImage(BaseImage, 0, 0);

            foreach (var icon in icons.Icons) {
                var rect = icon.Bounds;
                var img = GetStateImage(icon.Value);
                gfx.DrawImage(img, rect, rect, GraphicsUnit.Pixel);
            }

            Invalidate();
        }
    }


    class MapIconLayout
    {
        static string rawConfig = System.Text.Encoding.UTF8.GetString(Resources.ItemLayout);
        static Cereal config = (Cereal)Cereal.FromString(rawConfig);

        List<Icon> icons = new List<Icon>();
        public MapIconLayout() {
            foreach (var key in config.Keys) {
                var item = config.Group[key];
                var bounds = new Rectangle(item.Int["x"].Value, item.Int["y"].Value, item.Int["w"].Value, item.Int["h"].Value);
                this.icons.Add(new Icon(key, bounds, item.Int["max"] ?? 1));
            }
        }

        public IEnumerable<Icon> Icons { get { return icons; } }

        /// <summary>
        /// Returns null if not found
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>Icon name, or null if no icon found at specified location</returns>
        public string IconAt(int x, int y) {
            foreach (var icon in icons) {
                if (icon.Bounds.Contains(x, y)) return icon.Name;
            }

            return null;
        }
        public Rectangle? GetBounds(string name) {
            foreach (var icon in icons) {
                if (icon.Name == name) return icon.Bounds;
            }

            return null;
        }
        public int? GetState(string name) {
            foreach (var icon in icons) {
                if (icon.Name == name) return icon.Value;
            }

            return null;
        }
        public int? SetState(string name, int value) {
            foreach (var icon in icons) {
                if (icon.Name == name) return icon.Value = value.Clamp(0, icon.Max);
            }

            return null;
        }
        public class Icon
        {
            public Icon(string name, Rectangle bounds, int max) {
                this.Name = name;
                this.Bounds = bounds;
                this.Max = max;
                this.Value = 0;
            }
            public Rectangle Bounds { get; private set; }
            public int Max { get; private set; }
            public string Name { get; private set; }
            public int Value { get; set; }
        }


        internal Cereal Serialize() {
            Cereal result = new Cereal();
            foreach (var icon in icons) {
                result[icon.Name] = icon.Value;
            }
            return result;
        }
        internal void Deserialize(Cereal data) {
            foreach (var key in data.Keys) {
                this.SetState(key, data.Int[key] ?? 0);
            }
        }
    }
}
