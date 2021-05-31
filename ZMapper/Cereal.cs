using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;
using Romulus;

namespace FasTrak
{
    /// <summary>
    /// Not for public consumption. Does not handle escaping key names at all, floats, and other things.
    /// </summary>
    class Cereal : ICerealInts, ICerealBools, ICerealStrings, ICerealGroups, ICerealArray, IEnumerable<KeyValuePair<string, object>>
    {
        Dictionary<string, object> values = new Dictionary<string, object>();

        public static IList List() { return new ArrayList(); }


        /// <summary>
        /// Gets a value if it is an integer. Returns null if the value is undefined or null.
        /// </summary>
        public ICerealInts Int { get { return this as ICerealInts; } }
        /// <summary>
        /// Gets a value if it is a string. Returns null if the value is undefined or null.
        /// </summary>
        public ICerealStrings String { get { return this as ICerealStrings; } }
        /// <summary>
        /// Gets a value if it is a group. Returns null if the value is undefined or null.
        /// </summary>
        public ICerealGroups Group { get { return this as ICerealGroups; } }
        /// <summary>
        /// Gets a value if it is an array. Returns null if the value is undefined or null.
        /// </summary>
        public ICerealArray Array { get { return this as ICerealArray; } }
        /// <summary>
        /// Gets a value if it is a boolean. Returns null if the value is undefined or null.
        /// </summary>
        public ICerealBools Boolean { get { return (ICerealBools)this; } }

        public static readonly object undefined = new object();

        public Cereal() { }
        public static object FromString(string data) {
            return new CerealDecoder(data).Decode();
        }

        /// <summary>
        /// Gets/sets a value by name. When read, if value does not exist, Cereal.undefined is returned.
        /// </summary>
        public object this[string name] {
            get {
                object result;
                if (values.TryGetValue(name, out result)) return result;
                return undefined;
            }
            set {
                if (value == null || value == undefined || value is string || value is int || value is bool || value is Cereal || value is System.Collections.IList) {
                    values[name] = value;
                } else {
                    throw new ArgumentException("Invalid type for Cereal value");
                }
            }
        }

        Cereal ICerealGroups.this[string name] {
            get { return this[name] as Cereal; }
            set {
                this[name] = value;
            }
        }

        bool? ICerealBools.this[string name] {
            get { return this[name] as bool?; }
            set {
                this[name] = value;
            }
        }

        System.Collections.IList ICerealArray.this[string name] {
            get { return this[name] as System.Collections.IList; }
            set {
                this[name] = value;
            }
        }

        string ICerealStrings.this[string name] {
            get {
                return this[name] as string;
            }
            set {
                this[name] = value;
            }
        }

        int? ICerealInts.this[string name] {
            get {
                var result = this[name];
                if (result is int) return (int)result;
                return null;
            }
            set {
                values[name] = value;
            }
        }

        public bool Remove(string name) {
            return this.values.Remove(name);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() {
            return this.values.GetEnumerator();
        }

        public IEnumerable<string> Keys { get { return this.values.Keys; } }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public string Encode() {
            return new CerealEncoder(this).Encode();
        }



        private class CerealEncoder
        {
            int indentSize = 2;
            string indent = "  ";
            bool newlines = true;
            HashSet<Object> processedObjects = new HashSet<Object>();
            StringBuilder builder = new StringBuilder();
            object root;

            public CerealEncoder(object c) {
                this.root = c;
            }
            public CerealEncoder(object c, int? indentSize) {
                this.indentSize = indentSize ?? 0;
                this.newlines = indentSize != null;
                this.root = c;
            }

            private void applyIndent(int lvl) {
                for (var i = 0; i < lvl; i++) builder.Append(indent);
            }

            // Todo: escape strings
            public string Encode() {
                builder.Clear();
                processedObjects.Clear();

                if (root is Cereal) {
                    encodeCereal((Cereal)root, 0);
                } else if (root is System.Collections.IList) {
                    encodeList((System.Collections.IList)root, 0);
                }



                return builder.ToString();
            }

            private void encodeList(System.Collections.IList l, int iLevel) {
                if (processedObjects.Contains(l)) throw new ArgumentException("The specified Cereal object contains one or more cycles.");
                processedObjects.Add(l);

                builder.Append('[');
                if (newlines) builder.AppendLine();
                iLevel++;

                bool comma = false;
                foreach (var value in l) {
                    if (comma) {
                        builder.Append(',');
                        if (newlines) builder.AppendLine();
                    }

                    applyIndent(iLevel);
                    encodeValue(value, iLevel);
                    comma = true;
                }

                iLevel--;
                if (newlines) {
                    builder.AppendLine();
                    applyIndent(iLevel);
                }
                builder.Append("]");
            }

            private void encodeCereal(Cereal c, int iLevel) {
                if (processedObjects.Contains(c)) throw new ArgumentException("The specified Cereal object contains one or more cycles.");
                processedObjects.Add(c);

                builder.Append("{");
                if (newlines) builder.AppendLine();
                iLevel++;

                bool comma = false;
                foreach (var entry in c) {
                    var value = entry.Value;

                    if (comma) {
                        builder.Append(',');
                        if (newlines) builder.AppendLine();
                    }

                    applyIndent(iLevel);
                    builder.Append('"');
                    builder.Append(entry.Key);
                    builder.Append("\" : ");

                    encodeValue(value, iLevel);
                    comma = true;
                }

                iLevel--;
                if (newlines) {
                    builder.AppendLine();
                    applyIndent(iLevel);
                }
                builder.Append("}");
            }

            private void encodeValue(object value, int iLevel) {
                if (value is int) {
                    builder.Append((int)value);
                }else if (value is bool) {
                    builder.Append(((bool)value) ? "true" : "false");
                } else if (value is string) {
                    builder.Append('\"');
                    builder.Append((string)value);
                    builder.Append('\"');
                } else if (value == null) {
                    builder.Append("null");
                } else if (value is Cereal) {
                    encodeCereal(((Cereal)value), iLevel);
                } else if (value is System.Collections.IList) {
                    encodeList((System.Collections.IList)value, iLevel);
                }
            }
        }
        private class CerealDecoder
        {
            string originalSource;
            StringSection source;

            public CerealDecoder(string data) {
                this.originalSource = data;
            }

            public object Decode() {
                this.source = originalSource;

                source = source.TrimLeft();
                var result = DecodeCereal();
                source = source.TrimLeft();
                if (source.Length > 0) throw new ArgumentException("Could not parse json"); // Todo: json-specific exceptions

                return result;
            }

            private object DecodeCereal() {
                if (TryChar('{', true)) {
                    return ParseObject();
                } else if (TryChar('[', true)) {
                    return ParseArray();
                } else {
                    throw new ArgumentException("Could not parse json");
                }
            }

            private System.Collections.IList ParseArray() {
                var result = new List<object>();

                if (TryChar(']', true)) return result;

                result.Add(ParseValue());

                while (TryChar(',', true)) {
                    result.Add(ParseValue());
                }

                EatChar(']', true);

                return result;
            }

            private Cereal ParseObject() {
                Cereal result = new Cereal();

                if (TryChar('}', true)) return result;

                if (!TryChar('\"', false)) throw new ArgumentException("Could not parse json");
                while (true) {
                    var iStrEnd = source.IndexOf('\"');
                    if (iStrEnd < 0) throw new ArgumentException("Could not parse json");
                    var name = source.Substring(0, iStrEnd).ToString();
                    source = source.Substring(iStrEnd + 1).TrimLeft();

                    EatChar(':', true);

                    result[name] = ParseValue();

                    if (TryChar(',', true)) {
                        EatChar('\"', true);
                    } else {
                        EatChar('}', true);
                        return result;
                    }
                }

            }

            private object ParseValue() {
                object value;
                if (TryChar('\"', false)) {
                    var iStrEnd = source.IndexOf('\"');
                    if (iStrEnd < 0) throw new ArgumentException("Could not parse json");
                    value = source.Substring(0, iStrEnd).ToString();
                    source = source.Substring(iStrEnd + 1).TrimLeft();
                } else if (TryChar('[', true)) {
                    value = ParseArray();
                } else if (PeekChar() == '{') {
                    value = DecodeCereal();
                } else if (source.Length >= 4 && source.Substring(0, 4).ToString() == "true") {
                    source = source.Substring(4).TrimLeft();
                    value = true;
                } else if (source.Length >= 5 && source.Substring(0, 5).ToString() == "false") {
                    source = source.Substring(5).TrimLeft();
                    value = false;
                } else if (source.Length >= 4 && source.Substring(0, 4).ToString() == "null") {
                    source = source.Substring(4).TrimLeft();
                    value = null;
                } else {
                    // try to parse a number
                    int i = 0;
                    char c;

                    while (i < source.Length && (char.IsDigit(c = source[i]) || c == '.' || c == '-' || c == 'e' || c == 'E' || c == '+')) {
                        i++;
                    }

                    int intValue;
                    if (i != 0 && int.TryParse(source.Substring(0, i).ToString(), out intValue)) {
                        value = intValue;
                    } else {
                        throw new ArgumentException("Could not parse json");
                    }
                    source = source.Substring(i).TrimLeft();
                }
                return value;
            }

            private char PeekChar() {
                return source.Length > 0 ? source[0] : '\0';
            }

            private bool TryChar(char c, bool andWhitespace) {
                if (source.Length > 0 && source[0] == c) {
                    source = source.Substring(1);
                    if (andWhitespace) source = source.TrimLeft();
                    return true;
                } else {
                    return false;
                }
            }
            private void EatChar(char c, bool andWhitespace) {
                if (source.Length > 0 && source[0] == c) {
                    source = source.Substring(1);
                    if (andWhitespace) source = source.TrimLeft();
                } else {
                    throw new ArgumentException("Could not parse json");
                }
            }

            private void EatChar(bool andWhitespace) {
                source = source.Substring(1);
                if (andWhitespace) source = source.TrimLeft();
            }
        }


    }

    interface ICerealBools
    {
        bool? this[string name] { get; set; }
    }
    interface ICerealInts
    {
        int? this[string name] { get; set; }
    }
    interface ICerealStrings
    {
        string this[string name] { get; set; }
    }
    interface ICerealGroups
    {
        Cereal this[string name] { get; set; }
    }
    interface ICerealArray
    {
        System.Collections.IList this[string name] { get; set; }
    }
}
