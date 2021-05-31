using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;

namespace ZMapper
{
    class MinimapPanel: BufferedPanel
    {
        const int padding = 8;

        const int CellWidth = 96;
        const int CellHeight = 96;
        const int MapWidth = CellWidth * 3 + padding * 2;
        const int MapHeight = CellWidth * 3 + padding * 2;

        Bitmap image = new Bitmap(MapWidth, MapHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        Graphics gfx;
        static readonly Rectangle ThumbSource = new Rectangle(96, 0, 192, 192);

        List<Rectangle> ThumbPositions = new List<Rectangle>();

        public MinimapPanel() {
            this.gfx = Graphics.FromImage(image);

            for (int row = 0; row < 3; row++) {
                for (int column = 0; column < 3; column++) {
                    ThumbPositions.Add(new Rectangle(column * (CellWidth + padding), row * (CellHeight + padding), CellWidth, CellHeight));
                }
            }

            this.BackgroundImage = image;
            gfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
            gfx.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
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

        public void Clear() {
            gfx.Clear(Color.Black);
        }

        public void UpdateThumb(Bitmap image, int index) {
            var thumbBounds = ThumbPositions[index];
            gfx.DrawImage(image, thumbBounds, ThumbSource, GraphicsUnit.Pixel);
            this.Invalidate(thumbBounds);
        }
    }
}
