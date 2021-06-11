using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ZMapper
{
    /// <summary>
    /// This class adds on to the functionality provided in System.Windows.Forms.ToolStrip.
    /// </summary>
    public class ClickThruToolStrip : MenuStrip
    {

        private bool m_clickThrough = true;

        /// <summary>
        /// Gets or sets whether the ToolStripEx honors item clicks when its containing form does
        /// not have input focus.
        /// </summary>
        /// <remarks>
        /// Default value is false, which is the same behavior provided by the base ToolStrip class.
        /// </remarks>
        public bool ClickThrough {
            get {
                return m_clickThrough;
            }
            set {
                m_clickThrough = value;
            }
        }

        protected override void WndProc(ref Message m) {
            base.WndProc(ref m);

            if (m_clickThrough && (m.Msg == NativeConstants.WM_MOUSEACTIVATE) && (m.Result == new IntPtr(NativeConstants.MA_ACTIVATEANDEAT))) {
                m.Result = new IntPtr(NativeConstants.MA_ACTIVATE);
            }
        }


        static class NativeConstants
        {
            internal const uint WM_MOUSEACTIVATE = 0x21;
            internal const uint MA_ACTIVATE = 1;
            internal const uint MA_ACTIVATEANDEAT = 2;
            internal const uint MA_NOACTIVATE = 3;
            internal const uint MA_NOACTIVATEANDEAT = 4;
        }
    }
     
 
}