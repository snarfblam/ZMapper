using System;
using System.Collections.Generic;
using FasTrak;
using System.Text.RegularExpressions;

/*
 *  Settings schema
 *  
 *  {
 *      captionRegex?: string,
 *      classRegex?: string,
 *      topmost: boolean,
 *      noFocus: boolean
 *      
 *  }
 */

namespace ZMapper
{
    class Settings
    {
        static class Keys
        {
            public const string CaptionRegex = "captionRegex";
            public const string ClassRegex = "classRegex";
            public const string Topmost = "topmost";
            public const string NoFocus = "noFocus";
        }
        static class Defaults
        {
            public const string CaptionRegex = null;
            public const string ClassRegex = null;
            public const bool Topmost = true;
            public const bool NoFocus = true;
        }

        public string CaptionRegex { get; set; }
        public string ClassRegex { get; set; }
        public bool Topmost { get; set; }
        public bool NoFocus { get; set; }

        internal Settings() {
            this.CaptionRegex = Defaults.CaptionRegex;
            this.ClassRegex = Defaults.ClassRegex;
            this.Topmost = Defaults.Topmost;
            this.NoFocus = Defaults.NoFocus;
        }

        public string Serialize() {
            var data = new Cereal();
            if (CaptionRegex != null) data[Keys.CaptionRegex] = Escape(CaptionRegex);
            if (ClassRegex != null) data[Keys.ClassRegex] = Escape(ClassRegex);
            data[Keys.Topmost] = Topmost;
            data[Keys.NoFocus] = NoFocus;

            return data.Encode();
        }

        public static Settings Deserialize(string cereal) {
            Settings result = new Settings();
            Cereal data = Cereal.FromString(cereal) as Cereal;

            if (data != null) {
                result.CaptionRegex = Unescape(data.String[Keys.CaptionRegex]);
                result.ClassRegex = Unescape(data.String[Keys.ClassRegex]);
                result.Topmost = data.Boolean[Keys.Topmost] ?? false;
                result.NoFocus = data.Boolean[Keys.NoFocus] ?? false;
            }

            return result;
        }

        static string Escape(string str) {
            if (str == null) return null;
            return Regex.Escape(str);
        }

        static string Unescape(string str) {
            if (str == null) return null;
            return Regex.Unescape(str);
        }
    }
}
