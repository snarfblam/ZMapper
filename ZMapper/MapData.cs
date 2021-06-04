using System;
using System.Collections.Generic;
using System.Text;
using FasTrak;

namespace ZMapper
{
    class MapData
    {
        public const int MapWidth = 16;
        public const int MapHeight = 8;
        readonly ScreenData[,] screens = new ScreenData[16, 8];
        public bool DungeonMap { get; private set; }

        public MapData(bool dungeon) {
            this.DungeonMap = dungeon;

            CreateScreenData();
        }

        public void Clear() { CreateScreenData(); }

        private void CreateScreenData() {
            for (var x = 0; x < 16; x++) {
                for (var y = 0; y < 8; y++) {
                    var screen = new ScreenData();
                    screens[x, y] = screen;

                    if (DungeonMap) {
                        screen.Bombed = screen.Burned = screen.Fluted = true;
                    }
                }
            }
        }

        public ScreenData this[int x, int y] {
            get { return screens[x, y]; }
        }

        public Cereal Serialize() {
            Cereal mapData = new Cereal();
            var notesData = Cereal.List();
            var poiData = Cereal.List();

            byte[] rawMap = new byte[MapWidth * MapHeight * 2];
            int ptr = 0;

            for (var y = 0; y < MapHeight; y++) {
                for (var x = 0; x < MapWidth; x++) {
                    var raw = this.screens[x, y].GetData();
                    rawMap[ptr] = (byte)(raw); 
                    ptr++;
                    rawMap[ptr] = (byte)(raw >> 8); 
                    ptr++;

                    var note = this.screens[x, y].Note;
                    if (note != null) {
                        var noteData = new Cereal();
                        noteData["x"] = x;
                        noteData["y"] = y;
                        noteData["note"] = note;
                        notesData.Add(noteData);
                    }
                    var poiList = this.screens[x, y].PoiMarkers;
                    if (poiList.Count > 0) {
                        var poi = new Cereal();
                        poi["x"] = x;
                        poi["y"] = y;
                        poi["poi"] = new List<int>(poiList);
                        poiData.Add(poi);
                    }
                }
            }

            var base64 = Convert.ToBase64String(rawMap);
            mapData["notes"] = notesData;
            mapData["poi"] = poiData;
            mapData["data"] = base64;

            return mapData;
        }
        public void Deserialize(Cereal data) {
            // Fresh start
            CreateScreenData();

            var rawMap = Convert.FromBase64String(data.String["data"]);
            int ptr = 0;

            for (var y = 0; y < MapHeight; y++) {
                for (var x = 0; x < MapWidth; x++) {
                    int raw = rawMap[ptr] | (rawMap[ptr + 1] << 8);
                    this.screens[x, y].SetData((ushort)raw);
                    ptr += 2;
                }
            }

            var notesData = data.Array["notes"];
            if (notesData != null) {
                foreach (Cereal note in notesData) {
                    var x = note.Int["x"] ?? 0;
                    var y = note.Int["y"] ?? 0;
                    var noteText = note.String["note"];
                    if (noteText != null) this.screens[x, y].Note = noteText;
                }
            }

            var poiData = data.Array["poi"];
            if (poiData != null) {
                foreach (Cereal poi in poiData) {
                    var x = poi.Int["x"] ?? 0;
                    var y = poi.Int["y"] ?? 0;
                    var items = poi.Array["poi"];
                    if (items != null) {
                        foreach (var item in items) {
                            if (item is int) this.screens[x, y].AddPOIMarker((int)item);
                        }
                    }
                }
            }
        }
    }

    class ScreenData
    {
        public bool Bombed { get; set; }
        public bool Burned { get; set; }
        public bool Fluted { get; set; }
        public bool Clear { get; set; }
        public bool Visited { get; set; }
        public bool POI { get; set; }
        /// <summary>Null = no note present. Empty string is considered an empty note.</summary>
        public string Note { get; set; }
        public Direction Walls { get; set; }
        public Direction BombedWalls { get; set; }
        List<int> poiMarkers = new List<int>();
        IList<int> poiPublic;

        public ScreenData() {
            this.poiPublic = poiMarkers.AsReadOnly();
        }
        /// <summary>
        /// Gets a read-only list of POI markers. This list is NOT immutable and may or may not change if POI markers are added or removed.
        /// </summary>
        public IList<int> PoiMarkers { get { return poiPublic; } }

        public void AddPOIMarker(int index) {
            this.poiMarkers.Add(index);
        }

        public void ClearPOIMarkers() {
            this.poiMarkers.Clear();
        }

        public ushort GetData() {
            var raw = new BitArray32();
            raw[0] = Bombed;
            raw[1] = Burned;
            raw[2] = Fluted;
            raw[3] = Clear;
            raw[4] = Visited;
            raw[5] = POI;
            ApplyDirection(ref raw, Walls, 6); // 6-9
            ApplyDirection(ref raw, BombedWalls, 10); //10-13

            return (ushort)raw.Bits;
        }

        public void SetData(ushort data) {
            var raw = new BitArray32();
            raw.Bits = data;
            Bombed = raw[0];
            Burned = raw[1];
            Fluted = raw[2];
            Clear = raw[3];
            Visited = raw[4];
            POI = raw[5];
            Walls = DecodeDirection(raw, 6); // 6-9
            BombedWalls = DecodeDirection(raw, 10); //10-13
        }
        private static void ApplyDirection(ref BitArray32 vect, Direction d, int baseOffset) {
            if (d.Test(Direction.Up)) vect[baseOffset] = true;
            if (d.Test(Direction.Right)) vect[baseOffset + 1] = true;
            if (d.Test(Direction.Down)) vect[baseOffset + 2] = true;
            if (d.Test(Direction.Left)) vect[baseOffset + 3] = true;
        }
        private static Direction DecodeDirection(BitArray32 vect, int baseOffset) {
            Direction d = 0;
            if (vect[baseOffset]) d = d.Set(Direction.Up);
            if (vect[baseOffset + 1] ) d = d.Set(Direction.Right);
            if (vect[baseOffset + 2] ) d = d.Set(Direction.Down);
            if (vect[baseOffset + 3] ) d = d.Set(Direction.Left);
            return d;
        }
    }
}
