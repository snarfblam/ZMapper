using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ZMapper
{
    class GlobalInputs: IDisposable
    {
        public event EventHandler<MapOperationEventArgs> KeyPressed;
        bool keysMapped = false;

        static Dictionary<Keys, MapOperation> mappings = new Dictionary<Keys, MapOperation> {
            {Keys.NumPad7, MapOperation.MarkMode},
            {Keys.NumPad8, MapOperation.NavNorth},
            {Keys.NumPad9, MapOperation.MarkBurned},
            {Keys.NumPad4, MapOperation.NavWest},
            {Keys.NumPad5, MapOperation.MarkUnvisited},
            {Keys.NumPad6, MapOperation.NavEast},
            {Keys.NumPad1, MapOperation.MarkBombed},
            {Keys.NumPad2, MapOperation.NavSouth},
            {Keys.NumPad3, MapOperation.MarkFluted},
            {Keys.NumPad0, MapOperation.MarkPoi},
            {Keys.Decimal, MapOperation.AddNote},
            {Keys.Add, MapOperation.MarkClear},
            {Keys.Subtract, MapOperation.Undo},

            {Keys.Oemtilde, MapOperation.GotoOverworld},
            {Keys.D0, MapOperation.GotoOverworld},
            {Keys.D1, MapOperation.GotoDungeon1},
            {Keys.D2, MapOperation.GotoDungeon2},
            {Keys.D3, MapOperation.GotoDungeon3},
            {Keys.D4, MapOperation.GotoDungeon4},
            {Keys.D5, MapOperation.GotoDungeon5},
            {Keys.D6, MapOperation.GotoDungeon6},
            {Keys.D7, MapOperation.GotoDungeon7},
            {Keys.D8, MapOperation.GotoDungeon8},
            {Keys.D9, MapOperation.GotoDungeon9},
                                                          };
        KeyboardHook hook = new KeyboardHook();
        bool markMode;
        bool bombMarkMode;
        public bool EnableBombWallMarking { get; set; }

        public GlobalInputs() {
            MapKeys();

            hook.KeyPressed += new EventHandler<KeyPressedEventArgs>(hook_KeyPressed);
        }

        public void MapKeys() {
            if (keysMapped) return;
            foreach (KeyValuePair<Keys, MapOperation> entry in mappings) {
                hook.RegisterHotKey(0, entry.Key);
            }
            keysMapped = true;

        }
        public void UnmapKeys() {
            if (!keysMapped) return;
            hook.ClearHotkeys();
            keysMapped = false;
        }

        void hook_KeyPressed(object sender, KeyPressedEventArgs e) {
            MapOperation op = 0;
            if (mappings.TryGetValue(e.Key, out op)) {
                // In mark mode, convert navigation ops to marking ops
                if (markMode && op != MapOperation.MarkMode) { // Ignore markmode key (will be handled below)
                    if (op == MapOperation.NavNorth) {
                        op = MapOperation.MarkNorth;
                    } else if (op == MapOperation.NavEast) {
                        op = MapOperation.MarkEast;
                    } else if (op == MapOperation.NavSouth) {
                        op = MapOperation.MarkSouth;
                    } else if (op == MapOperation.NavWest) {
                        op = MapOperation.MarkWest;
                    } 
                    // Any other input cancels mark mode
                    markMode = false;
                } else if (EnableBombWallMarking && bombMarkMode && op != MapOperation.MarkBombed) {
                    if (op == MapOperation.NavNorth) {
                        op = MapOperation.MarkBombedNorth;
                    } else if (op == MapOperation.NavEast) {
                        op = MapOperation.MarkBombedEast;
                    } else if (op == MapOperation.NavSouth) {
                        op = MapOperation.MarkBombedSouth;
                    } else if (op == MapOperation.NavWest) {
                        op = MapOperation.MarkBombedWest;
                    }
                    // Any other input cancels mark mode
                    bombMarkMode = false;
                }

                if (op == MapOperation.MarkMode) {
                    markMode = !markMode; // Toggle mark mode
                    this.KeyPressed.Raise(this, new MapOperationEventArgs(MapOperation.None, e.Key, e.Modifier));
                } else if (EnableBombWallMarking && op == MapOperation.MarkBombed) {
                    bombMarkMode = !bombMarkMode; // Toggle mark mode
                    this.KeyPressed.Raise(this, new MapOperationEventArgs(MapOperation.None, e.Key, e.Modifier));
                } else {
                    markMode = false;
                    this.KeyPressed.Raise(this, new MapOperationEventArgs(op, e.Key, e.Modifier));
                }
            }
        }

        public void Dispose() {
            this.hook.Dispose();
        }

        /// <summary>
        /// Disables any input "modifiers", e.g. marker mode
        /// </summary>
        internal void CancelModifiers() {
            this.markMode = false;
        }
    }

    enum MapOperation
    {
        /// <summary>
        /// A key press was registered but does not correspond to a map operation
        /// </summary>
        None,
        /// <summary>
        /// Pseudo-operation that indicates that if the next operation is a 'Nav' operation it should be changed to a 'Mark' operation.
        /// </summary>
        MarkMode,
        MarkUnvisited,
        NavNorth,
        NavEast,
        NavSouth,
        NavWest,
        MarkNorth,
        MarkEast,
        MarkSouth,
        MarkWest,
        MarkBombed,
        MarkBurned,
        MarkFluted,
        MarkClear,
        MarkPoi,
        AddNote,
        Undo,
        GotoOverworld,
        GotoDungeon1,
        GotoDungeon2,
        GotoDungeon3,
        GotoDungeon4,
        GotoDungeon5,
        GotoDungeon6,
        GotoDungeon7,
        GotoDungeon8,
        GotoDungeon9,
        MarkBombedNorth,
        MarkBombedEast,
        MarkBombedSouth,
        MarkBombedWest,
    }

    class MapOperationEventArgs : EventArgs
    {
        public MapOperation Op{ get; private set; }
        public MapOperationEventArgs(MapOperation op, Keys key, ModifierKeys modifier) {
            this.Op = op;
            this.Key = key;
            this.Modifier = modifier;
        }

        public Keys Key { get; private set; }
        public ModifierKeys Modifier { get; private set; }
    }
}
