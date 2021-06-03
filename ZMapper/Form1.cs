﻿using System.Windows.Forms;
using ZMapper.Properties;
using System.Drawing;
using System.Collections.Generic;
using System;
using FasTrak;
using System.Text.RegularExpressions;
using System.IO;

namespace ZMapper
{
    public partial class Form1 : Form
    {
        //const int TileSize = 24;

        //readonly Bitmap srcVisited = Resources.visited;
        //readonly Bitmap srcUnvisited = Resources.unvisited;
        //readonly Bitmap srcMarkers = Resources.marks;
        //readonly Bitmap owImage;
        //readonly Graphics gOwImage;
        
        //OverworldData owData = new OverworldData();
        string currentFileName = null;

        MapRenderer owMap = new MapRenderer(false);
        MapRenderer dungeonMap = new MapRenderer(true);

        MapRenderer currentMap;
        GlobalInputs inputs = new GlobalInputs();

        bool cursorOn = false;
        int cursorX = 0;
        int cursorY = 0;
        List<Cereal> dungeonData = new List<Cereal>() { null, null, null, null, null, null, null, null, null, };
        /// <summary>
        /// Zero based! (0 is level 1, 1 is level 2, etc)
        /// </summary>
        int currentDungeonIndex = 0;
        Point owCursorPos = new Point(7, 7);
        List<Point?> dungeonCursorPos = new List<Point?> { null, null, null, null, null, null, null, null, null, };
        static readonly Point DefaultCursorPos = new Point(7,7);
        ActiveWinTracker activeWindowTracker;
        bool thumbnailUpdatePending = false;

        Bitmap[] LevelCaptions = { Resources.D0, Resources.D1, Resources.D2, Resources.D3, Resources.D4, Resources.D5, Resources.D6, Resources.D7, Resources.D8, Resources.D9, };
        Bitmap OwPoiImage = Resources.OWPOISelector;
        Bitmap DPoiImage = Resources.DPOISelector;

        bool poiMode = false;
        int? poiFirstDigit = null;

        public Form1() {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            InitializeComponent();

            MainToolbar.ForeColor = SystemColors.ControlText;

            currentMap = owMap;
            this.currentMap.MapData[cursorX, cursorY].Visited = true;
            //this.map.OverworldData[8, 7].Bombed = true;
            //this.map.OverworldData[8, 7].Burned = true;
            //this.map.OverworldData[7, 7].Visited = true;
            //this.map.OverworldData[9, 7].Visited = true;
            //this.map.OverworldData[9, 6].Visited = true;
            //this.map.OverworldData[8, 7].Walls = Direction.Right;
            //this.map.OverworldData[9, 7].Walls = Direction.Left;

            this.currentMap.RenderEntireMap();
            this.pnlMap.BackgroundImage = currentMap.MapImage;
            //owImage = srcUnvisited.Clone(); // verify actual clone
            //gOwImage = Graphics.FromImage(owImage);

            this.CursorTimer.Start();

            this.inputs.KeyPressed += new System.EventHandler<MapOperationEventArgs>(inputs_KeyPressed);

            this.activeWindowTracker = new ActiveWinTracker(this);
            this.activeWindowTracker.ActiveWindowChanged += new EventHandler<ActiveWindowEventArgs>(activeWindowTracker_ActiveWindowChanged);

            GenerateAllDungeonThumbs();
            SetInputMode(InputMode.AlwaysActive, null); // Todo: Persist!

            picCaption.Image = LevelCaptions[0];
        }

        #region Input Enabled Management

        enum InputMode
        {
            AlwaysActive,
            ActiveByCaption,
            ActiveByClass,
        }

        bool inputEnabled = true;
        InputMode inputMode = InputMode.AlwaysActive;
        string inputRegexString = null;
        Regex inputRegex = null;

        void SetInputMode(InputMode mode, string regex) {
            this.inputMode = mode;
            this.inputRegexString = regex;
            if (regex != null) inputRegex = new Regex(regex);

            if (ActiveForm != null) this.EnableInput(); // Probably redundant
        }

        void activeWindowTracker_ActiveWindowChanged(object sender, ActiveWindowEventArgs e) {
            if (IsDisposed) return;

            bool enable = true;

            if (e.HWnd != this.Handle) {
                if (inputMode == InputMode.ActiveByCaption) {
                    enable = inputRegex.IsMatch(e.WindowTitle ?? "");
                } else if (inputMode == InputMode.ActiveByClass) {
                    enable = inputRegex.IsMatch(e.WindowClass ?? "");
                }
            }

            if (enable) {
                EnableInput();
            } else {
                DisableInput();
            }
        }

        private void DisableInput() {
            if (!inputEnabled) return;

            inputEnabled = false;
            this.inputs.UnmapKeys();
            this.Text = "ZMapper [Inactive]";
        }

        private void EnableInput() {
            if (inputEnabled) return;

            inputEnabled = true;
            this.inputs.MapKeys();
            this.Text = "ZMapper [Active]";
        }


        private void btnInputAlways_Click(object sender, EventArgs e) {
            SetInputMode(InputMode.AlwaysActive, null);
        }

        private void btnInputCaption_Click(object sender, EventArgs e) {
            if (InputModeEditor.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                SetInputMode(InputMode.ActiveByCaption, InputModeEditor.MatchRegex);
            }
        }

        private void btnInputClass_Click(object sender, EventArgs e) {
            if (InputModeEditor.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                SetInputMode(InputMode.ActiveByClass, InputModeEditor.MatchRegex);
            }
        }

        #endregion


        private void GenerateAllDungeonThumbs() {

            for (var i = 0; i < 9; i++) {
                EnterDungeonMode(i);
                ExitDungeonMode();
            }
        }

        List<Undoable> undoQueue = new List<Undoable>();
        List<Undoable> redoQueue = new List<Undoable>();

        void EnterDungeonMode(int dungeonIndex) {
            if (currentMap == dungeonMap) {
                if (dungeonIndex != currentDungeonIndex) ExitDungeonMode();
            }

            undoQueue.Clear();
            redoQueue.Clear();

            owCursorPos = new Point(cursorX,cursorY);
            currentDungeonIndex = dungeonIndex;
            currentMap = dungeonMap;
            var mapData = dungeonData[dungeonIndex];
            if(mapData == null) {
                dungeonMap.MapData.Clear();
            }else{
                dungeonMap.MapData.Deserialize(mapData);
            }

            cursorOn = false;
            var cursorPos = dungeonCursorPos[dungeonIndex] ?? DefaultCursorPos;
            cursorX = cursorPos.X;
            cursorY = cursorPos.Y;
            currentMap.RenderEntireMap();
            pnlMap.BackgroundImage = currentMap.MapImage;
            pnlMap.Invalidate();

            inputs.EnableBombWallMarking = true;
            picCaption.Image = LevelCaptions[currentDungeonIndex + 1];
            picPOI.Image = DPoiImage;
        }

        private void ExitDungeonMode() {
            if (currentMap == owMap) return;

            undoQueue.Clear();
            redoQueue.Clear();

            // Save map data
            var mapData = currentMap.MapData.Serialize();
            dungeonData[currentDungeonIndex] = mapData;
            dungeonCursorPos[currentDungeonIndex] = new Point(cursorX, cursorY);
            if (cursorOn) ToggleCursor();
            this.minimap.UpdateThumb(currentMap.MapImage, currentDungeonIndex);

            cursorOn = false;
            var cursorPos = owCursorPos;
            cursorX = cursorPos.X;
            cursorY = cursorPos.Y;

            currentMap = owMap;
            currentMap.RenderEntireMap();
            pnlMap.BackgroundImage = currentMap.MapImage;
            pnlMap.Invalidate();

            inputs.EnableBombWallMarking = false;
            picCaption.Image = LevelCaptions[0];
            picPOI.Image = OwPoiImage;
        }


        private void QueueThumbnailUpdate() {
            this.thumbnailUpdatePending = true;
        }


        void Do(Action doAction, Action undoAction, bool minor) {
            redoQueue.Clear();

            undoQueue.Add(new Undoable(doAction, undoAction, minor));
            doAction();
            QueueThumbnailUpdate();
        }


        void Undo() {
            if (undoQueue.Count == 0) return;
            bool finished;

            do {
                finished = true;
                
                var action = undoQueue[undoQueue.Count - 1];
                undoQueue.RemoveAt(undoQueue.Count - 1);
                action.UndoAction();
                redoQueue.Add(action);
                if (action.MinorAction) finished = false;

            } while (!finished);
            QueueThumbnailUpdate();
        }
        void Redo() {
            if (redoQueue.Count == 0) return;
            bool finished;

            do {
                finished = true;

                var action = redoQueue[redoQueue.Count - 1];
                redoQueue.RemoveAt(redoQueue.Count - 1);
                action.UndoAction();
                undoQueue.Add(action);
                if (action.MinorAction) finished = false;

            } while (!finished);
            QueueThumbnailUpdate();
        }

        void inputs_KeyPressed(object sender, MapOperationEventArgs e) {
            if (poiMode) {
                int digitValue = (int)e.Key - (int)Keys.NumPad0;
                if (digitValue < 0 || digitValue > 9) {
                    ExitPoiMode();
                    return;
                } else {
                    if (poiFirstDigit == null) {
                        if (digitValue == 0 || digitValue > 5) {
                            ExitPoiMode();
                            return;
                        }
                        poiFirstDigit = digitValue - 1;
                    } else {
                        var newDigit = (digitValue + 9) % 10;
                        var poiValue = poiFirstDigit.Value * 10 + newDigit;
                        AddPoi(poiValue);
                        ExitPoiMode();
                    }
                }
            } else {
                var op = e.Op;
                HandleMoveOp(op);
                HandleWallMarkOp(op);
                HandleWallBombMarkOp(op);
                HandleDungeonOp(op);

                if (op == MapOperation.MarkPoi) {
                    EnterPoiMode();
                    //ModifyCurrentCell(
                    //    data => data.POI = !data.POI,
                    //    data => data.POI = !data.POI);
                } else if (op == MapOperation.MarkBombed) {
                    ModifyCurrentCell(
                        data => data.Bombed = !data.Bombed,
                        data => data.Bombed = !data.Bombed);
                } else if (op == MapOperation.MarkBurned) {
                    ModifyCurrentCell(
                        data => data.Burned = !data.Burned,
                        data => data.Burned = !data.Burned);
                } else if (op == MapOperation.MarkFluted) {
                    ModifyCurrentCell(
                        data => data.Fluted = !data.Fluted,
                        data => data.Fluted = !data.Fluted);
                } else if (op == MapOperation.MarkClear) {
                    ModifyCurrentCell(
                        data => data.Clear = !data.Clear,
                        data => data.Clear = !data.Clear);
                } else if (op == MapOperation.MarkUnvisited) {
                    ModifyCurrentCell(data => data.Visited = false, null);
                } else if (op == MapOperation.Undo) {
                    Undo();
                }
            }
        }

        private void AddPoi(int value) {
            MessageBox.Show(value.ToString());
        }

        private void EnterPoiMode() {
            if (!poiMode) {
                picPOI.Visible = true;
                poiMode = true;
                poiFirstDigit = null;
            }
        }
        private void ExitPoiMode() {
            if (poiMode) {
                picPOI.Visible = false;
                poiMode = false;
            }
        }

        private void HandleDungeonOp(MapOperation op) {
            switch (op) {
                case MapOperation.GotoOverworld:
                    this.ExitDungeonMode();
                    break;
                case MapOperation.GotoDungeon1:
                    this.EnterDungeonMode(0);
                    break;
                case MapOperation.GotoDungeon2:
                    this.EnterDungeonMode(1);
                    break;
                case MapOperation.GotoDungeon3:
                    this.EnterDungeonMode(2);
                    break;
                case MapOperation.GotoDungeon4:
                    this.EnterDungeonMode(3);
                    break;
                case MapOperation.GotoDungeon5:
                    this.EnterDungeonMode(4);
                    break;
                case MapOperation.GotoDungeon6:
                    this.EnterDungeonMode(5);
                    break;
                case MapOperation.GotoDungeon7:
                    this.EnterDungeonMode(6);
                    break;
                case MapOperation.GotoDungeon8:
                    this.EnterDungeonMode(7);
                    break;
                case MapOperation.GotoDungeon9:
                    this.EnterDungeonMode(8);
                    break;
            }
        }

        delegate void CellModifier(ScreenData data);

  

        void ModifyCurrentCell(CellModifier mod, CellModifier undo) {
            ModifyCell(cursorX, cursorY, mod, undo);
        }
        void ModifyCell(int x, int y, CellModifier mod, CellModifier undo) {
            if (undo == null) {
                ModifyCellInternal(x, y, mod);
            } else {
                Do(
                    () => ModifyCellInternal(x, y, mod),
                    () => ModifyCellInternal(x, y, undo),
                    false);
            }
        }

        private void ModifyCellInternal(int x, int y, CellModifier mod) {
            bool cursorOn = this.cursorOn && (x == this.cursorX) && (y == this.cursorY);

            var data = currentMap.MapData[x, y];
            mod(data);

            if (cursorOn) ToggleCursor();
            pnlMap.Invalidate(currentMap.RenderOverworldScreen(x, y));
        }

        private void HandleMoveOp(MapOperation op) {
            Point cursorMove = new Point(0, 0);

            if (op == MapOperation.NavEast) {
                cursorMove = new Point(1, 0);
            } else if (op == MapOperation.NavWest) {
                cursorMove = new Point(-1, 0);
            } else if (op == MapOperation.NavSouth) {
                cursorMove = new Point(0, 1);
            } else if (op == MapOperation.NavNorth) {
                cursorMove = new Point(0, -1);
            }

            if (cursorMove.X != 0 || cursorMove.Y != 0) {
                bool cursorOn = this.cursorOn;
                if (cursorOn) ToggleCursor();
                cursorX = (cursorX + cursorMove.X).Clamp(0, MapData.MapWidth - 1);
                cursorY = (cursorY + cursorMove.Y).Clamp(0, MapData.MapHeight - 1);
                var visited = this.currentMap.MapData[cursorX, cursorY].Visited;
                if (!visited) {
                    this.ModifyCurrentCell(data => data.Visited = true, null);
                    QueueThumbnailUpdate();
                }
                if (cursorOn) ToggleCursor();
            }

        }

        private void HandleWallMarkOp(MapOperation op) {
            Direction? thisWall = null;
            Direction? northWall = null;
            Direction? southWall = null;
            Direction? eastWall = null;
            Direction? westWall = null;

            if (op == MapOperation.MarkWest) {
                thisWall = Direction.Left;
                westWall = Direction.Right;
            } else if (op == MapOperation.MarkEast) {
                thisWall = Direction.Right;
                eastWall = Direction.Left;
            } else if (op == MapOperation.MarkSouth) {
                thisWall = Direction.Down;
                southWall = Direction.Up;
            } else if (op == MapOperation.MarkNorth) {
                thisWall = Direction.Up;
                northWall = Direction.Down;
            }



            if (thisWall != null) {
                this.ModifyCurrentCell(
                    data => data.Walls = data.Walls.Toggle(thisWall.Value),
                    data => data.Walls = data.Walls.Toggle(thisWall.Value));
            }
            HandleWallMarkOpAdjacent(-1, 0, westWall);
            HandleWallMarkOpAdjacent(0, -1, northWall);
            HandleWallMarkOpAdjacent(0, 1, southWall);
            HandleWallMarkOpAdjacent(1, 0, eastWall);
        }
         private void HandleWallBombMarkOp(MapOperation op) {
                    Direction? thisWall = null;
                    Direction? northWall = null;
                    Direction? southWall = null;
                    Direction? eastWall = null;
                    Direction? westWall = null;

                    if (op == MapOperation.MarkBombedWest) {
                        thisWall = Direction.Left;
                        westWall = Direction.Right;
                    } else if (op == MapOperation.MarkBombedEast) {
                        thisWall = Direction.Right;
                        eastWall = Direction.Left;
                    } else if (op == MapOperation.MarkBombedSouth) {
                        thisWall = Direction.Down;
                        southWall = Direction.Up;
                    } else if (op == MapOperation.MarkBombedNorth) {
                        thisWall = Direction.Up;
                        northWall = Direction.Down;
                    }

                    if (thisWall != null) {
                        this.ModifyCurrentCell(
                            data => data.BombedWalls = data.BombedWalls.Toggle(thisWall.Value),
                            data => data.BombedWalls = data.BombedWalls.Toggle(thisWall.Value));
                    }
                    HandleWallBombMarkOpAdjacent(-1, 0, westWall);
                    HandleWallBombMarkOpAdjacent(0, -1, northWall);
                    HandleWallBombMarkOpAdjacent(0, 1, southWall);
                    HandleWallBombMarkOpAdjacent(1, 0, eastWall);
                }
         private void HandleWallMarkOpAdjacent(int dx, int dy, Direction? toggle) {
             if (toggle != null) {
                 int x = cursorX + dx;
                 int y = cursorY + dy;
                 if (x >= 0 && x < MapData.MapWidth && y >= 0 && y < MapData.MapHeight) {
                     ModifyCell(x, y,
                         data => data.Walls = data.Walls.Toggle(toggle.Value),
                         data => data.Walls = data.Walls.Toggle(toggle.Value));
                 }
             }
         }
         private void HandleWallBombMarkOpAdjacent(int dx, int dy, Direction? toggle) {
             if (toggle != null) {
                 int x = cursorX + dx;
                 int y = cursorY + dy;
                 if (x >= 0 && x < MapData.MapWidth && y >= 0 && y < MapData.MapHeight) {
                     ModifyCell(x, y,
                         data => data.BombedWalls = data.BombedWalls.Toggle(toggle.Value),
                         data => data.BombedWalls = data.BombedWalls.Toggle(toggle.Value));
                 }
             }
         }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);

        }
        private void CursorTimer_Tick(object sender, System.EventArgs e) {
            TryUpdateThumb();
            ToggleCursor();
            TryUpdateThumb();
        }

        private void ToggleCursor() {
            cursorOn = !cursorOn;
            this.pnlMap.Invalidate(currentMap.InvertCell(cursorX, cursorY));
        }


        private void TryUpdateThumb() {
            if (!cursorOn && thumbnailUpdatePending == true) {
                thumbnailUpdatePending = false;
                if (currentMap != owMap) {
                    this.minimap.UpdateThumb(currentMap.MapImage, currentDungeonIndex);
                }
            }
        }

        private void pnlMap_MouseDown(object sender, MouseEventArgs e) {
            int tileX = (e.X / currentMap.CellSize.Width).Clamp(0, MapData.MapWidth);
            int tileY = (e.Y / currentMap.CellSize.Width).Clamp(0, MapData.MapHeight);
            if (e.Button == System.Windows.Forms.MouseButtons.Left) {
                var cursor = cursorOn;
                if (cursor) ToggleCursor();
                cursorX = tileX;
                cursorY = tileY;
                if (cursor) ToggleCursor();

                ModifyCell(tileX, tileY, data => data.Visited = true, null);
            } else if (e.Button == System.Windows.Forms.MouseButtons.Right) {
                ModifyCell(tileX, tileY, data => data.Visited = false, null);
            }
        }

        #region Saving/Loading
        internal Cereal SaveMap() {
            // If current map is a dungeon, we must serialize it now (normally they are serialized upon map swaps)
            if (currentMap == dungeonMap) {
                dungeonData[currentDungeonIndex] = currentMap.MapData.Serialize();
            }

            Cereal result = new Cereal();
            var maps = Cereal.List();
            maps.Add(owMap.MapData.Serialize());
            for (int i = 0; i < dungeonData.Count; i++) {
                maps.Add(dungeonData[i]);
            }
            result["maps"] = maps;

            Cereal itemData = this.itemPanel1.Serialize();
            result["items"] = itemData;

            return result;
        }

        internal void LoadMap(Cereal data) {
            ExitDungeonMode();

            owMap.MapData.Clear();
            dungeonMap.MapData.Clear();
            for (int i = 0; i < dungeonData.Count; i++) dungeonData[i] = null;
            for (int i = 0; i < dungeonCursorPos.Count; i++) dungeonCursorPos[i] = null;

            var maps = data.Array["maps"];
            var items = data.Group["items"];

            int mapCount = Math.Min(maps.Count, dungeonData.Count + 1);
            for (int i = 0; i < mapCount; i++) {
                if (i == 0) {
                    owMap.MapData.Deserialize((Cereal)maps[i]);
                } else {
                    dungeonData[i - 1] = (Cereal)maps[i];
                }
            }

            if (items != null) {
                this.itemPanel1.Deserialize(items);
            }

            GenerateAllDungeonThumbs();
            cursorOn = false;
        }

        private void ClearEntireTracker() {
            if (MessageBox.Show("Tracker data will be deleted.", "Delete Tracker Data", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK) {
                itemPanel1.ClearItemData();
                owMap.MapData.Clear();
                dungeonMap.MapData.Clear();
                for (int i = 0; i < dungeonData.Count; i++) {
                    dungeonData[i] = null;
                }
                GenerateAllDungeonThumbs();

                currentFileName = null;
            }
        }
        
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) {
            DoSaveAs();
        }

        private void DoSaveAs() {
            if (FileSaver.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                DoSave(FileSaver.FileName);
            }
        }
        /// <summary>
        /// Saves the tracker data to the specified filename.
        /// </summary>
        /// <param name="filename">The filename to save to. If save succeeds, `currentFilename` will be set to this filename.</param>
        /// <returns>The new value of `currentFilename`</returns>
        private string DoSave(string filename) {
            Cereal mapData;
            try {
                mapData = this.SaveMap();
            } catch (Exception ex) {
                MessageBox.Show(
                  "An error occurred creating save data.\n" + ex.GetType().ToString() + ": " + ex.Message,
                  "Error Saving File",
                  MessageBoxButtons.OK,
                  MessageBoxIcon.Error);
                return currentFileName;
            }

            try {
                File.WriteAllText(filename, mapData.Encode());
            } catch (Exception ex) {
                MessageBox.Show(
                  "An error occurred accessing the file.\n" + ex.GetType().ToString() + ": " + ex.Message,
                  "Error Saving File",
                  MessageBoxButtons.OK,
                  MessageBoxIcon.Error);

                return currentFileName;
            }

            currentFileName = FileSaver.FileName;
            return currentFileName;
        }


        private void btnNew_Click(object sender, EventArgs e) {
            ClearEntireTracker();
        }

        private void btnOpen_Click(object sender, EventArgs e) {
            if (FileOpener.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                string fileContents = null;
                Cereal mapData = null;

                try {
                    fileContents = File.ReadAllText(FileOpener.FileName);
                } catch (Exception ex) {
                    MessageBox.Show(
                        "An error occurred while trying to accesss the file.\n" + ex.GetType().ToString() + ": " + ex.Message,
                        "Error Opening File",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }

                if (fileContents != null) {
                    try {
                        mapData = (Cereal)Cereal.FromString(fileContents);
                    } catch (Exception ex) {
                        MessageBox.Show(
                            "An error occurred while trying to parse the file contents.\n" + ex.GetType().ToString() + ": " + ex.Message,
                            "Error Opening File",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }

                if (mapData != null) {
                    try {
                        this.LoadMap(mapData);
                    } catch (Exception ex) {
                        MessageBox.Show(
                            "An error occurred while processing the file.\n" + ex.GetType().ToString() + ": " + ex.Message,
                            "Error Opening File",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);

                        ClearEntireTracker(); // Sorry!
                    }

                    currentFileName = FileOpener.FileName;
                }
            }
        }

        private void btnSave_ButtonClick(object sender, EventArgs e) {
            if (currentFileName == null) {
                DoSaveAs();
            } else {
                DoSave(currentFileName);
            }
        }

        #endregion


    }

    class Undoable
    {
        public Undoable(Action doAction, Action undoAction, bool minor) {
            this.DoAction = doAction;
            this.UndoAction = undoAction;
            this.MinorAction = minor;
        }
        public Action DoAction { get; private set; }
        public Action UndoAction { get; private set; }
        /// <summary>
        /// Minor actions are done/undone along with adjacent non-minor actions. For example, cursor movements.
        /// </summary>
        public bool MinorAction { get; private set; }
    }

    class BufferedPanel : Panel
    {
        public BufferedPanel() {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        }
    }

}
