﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;
using ZMapper.Properties;
using FasTrak;

namespace ZMapper
{
    class MapRenderer
    {
        #region static
        static ColorMatrix inversionMatrix = new ColorMatrix(new float[][]
         {
            new float[] {-1, 0, 0, 0, 0},
            new float[] {0, -1, 0, 0, 0},
            new float[] {0, 0, -1, 0, 0},
            new float[] {0, 0, 0, 1, 0},
            new float[] {1, 1, 1, 0, 1}
         });
        static ImageAttributes inversionAttributes = new ImageAttributes();
        static MapRenderer() {
            inversionAttributes.SetColorMatrix(inversionMatrix);
        }
        static void InvertRect(Bitmap image, Graphics gfx, Rectangle rect) {
            gfx.DrawImage(image, rect, rect.X, rect.Y, rect.Width, rect.Height, GraphicsUnit.Pixel, inversionAttributes);
        }
        #endregion

        const int TileSize = 24;
        public const int MapWidth = 16;
        public const int MapHeight = 8;

        public readonly Size CellSize = new Size(TileSize, TileSize);

        readonly Bitmap srcVisited;// = Resources.visitedDungeon;
        readonly Bitmap srcUnvisited; // = Resources.unvisitedDungeon;
        readonly Bitmap srcMarkers; // = Resources.marks;
        readonly Bitmap mapImage;
        readonly Graphics gMapImage;
        
        MapData mapData;// = new MapData();
        public bool DungeonMap { get; private set; }

        public MapRenderer(bool dungeon) {
            this.DungeonMap = dungeon;
            srcVisited = dungeon ? Resources.visitedDungeon : Resources.visited;
            srcUnvisited = dungeon ? Resources.unvisitedDungeon : Resources.unvisited;
            srcMarkers = dungeon ? Resources.marksDungeon : Resources.marks;
            mapData = new MapData(dungeon);

            mapImage = (Bitmap)srcUnvisited.Clone(); // verify actual clone
            gMapImage = Graphics.FromImage(mapImage);
        }

        public Bitmap MapImage { get { return this.mapImage; } }
        public MapData MapData { get { return this.mapData; } }

        public void RenderEntireMap() {
            for (var x = 0; x < 16; x++) for (var y = 0; y < MapHeight; y++) RenderOverworldScreen(x, y);
        }

        public Rectangle InvertCell(int x, int y){
            var rect = GetMapRect(x, y);
            rect = new Rectangle(rect.X + 6, rect.Y + 6, rect.Width - 12, rect.Height - 12);
            InvertRect(mapImage, gMapImage, rect);
            return rect;
        }
        public Rectangle GetMapRect(int x, int y) {
            return new Rectangle(x * TileSize, y * TileSize, TileSize, TileSize);
        }
        /// <summary>
        /// Redraws a cell on the overworld map grid
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>The bounds of the drawn screen</returns>
        public Rectangle RenderOverworldScreen(int x, int y) {
            var mapRect = GetMapRect(x, y);
            var markerSrcRect = mapRect; 

            var data = mapData[x, y];
            var baseImg = data.Visited ? srcVisited : srcUnvisited;
            gMapImage.DrawImage(baseImg, mapRect, mapRect, GraphicsUnit.Pixel);


            var markers = new List<MapMarkers>();

            if (data.Visited) {
                if (DungeonMap) {
                    if (data.Clear) markers.Add(MapMarkers.clear);
                    if (data.Note != null) markers.Add(MapMarkers.note);
                    if (data.POI) markers.Add(MapMarkers.poi);
                } else {
                    // Note, POI, and cleared markers are mutually exclusive.
                    if (data.Note != null) {
                        //markerSrcRect.Y = MapMarkers.note * TileSize;
                        //gOwImage.DrawImage(srcMarkers, mapRect, markerSrcRect, GraphicsUnit.Pixel);
                        markers.Add(MapMarkers.note);
                    } else if (data.POI) {
                        markers.Add(MapMarkers.poi);
                    } else if (data.Clear) {
                        markers.Add(MapMarkers.clear);
                    }
                }

                if (!data.Clear) {
                    if (!data.Bombed) markers.Add(MapMarkers.bomb);
                    if (!data.Burned) markers.Add(MapMarkers.fire);
                    if (!data.Fluted) markers.Add(MapMarkers.flute);
                }

                if (data.Walls.Test(Direction.Up)) markers.Add(MapMarkers.wallNorth);
                if (data.Walls.Test(Direction.Right)) markers.Add(MapMarkers.wallEast);
                if (data.Walls.Test(Direction.Down)) markers.Add(MapMarkers.wallSouth);
                if (data.Walls.Test(Direction.Left)) markers.Add(MapMarkers.wallWest);

                // Always shows (see below)
                //if (data.BombedWalls.Test(Direction.Up)) markers.Add(MapMarkers.bombedNorth);
                //if (data.BombedWalls.Test(Direction.Right)) markers.Add(MapMarkers.bombedEast);
                //if (data.BombedWalls.Test(Direction.Down)) markers.Add(MapMarkers.bombedSouth);
                //if (data.BombedWalls.Test(Direction.Left)) markers.Add(MapMarkers.bombedWest);

            }


            // Bombed-wall markers always show, regardless of visited status
            if (data.BombedWalls.Test(Direction.Up)) markers.Add(MapMarkers.bombedNorth);
            if (data.BombedWalls.Test(Direction.Right)) markers.Add(MapMarkers.bombedEast);
            if (data.BombedWalls.Test(Direction.Down)) markers.Add(MapMarkers.bombedSouth);
            if (data.BombedWalls.Test(Direction.Left)) markers.Add(MapMarkers.bombedWest);

            markerSrcRect.X = 0;
            foreach (var mark in markers) {
                markerSrcRect.Y = (int)mark * TileSize;
                gMapImage.DrawImage(srcMarkers, mapRect, markerSrcRect, GraphicsUnit.Pixel);
            }


            return mapRect;
        }

    }

    enum MapMarkers
    {
        bomb = 0,
        fire = 1,
        flute = 2,
        poi = 3,
        note = 4,
        clear = 5,
        wallNorth = 6,
        wallEast = 7,
        wallSouth = 8,
        wallWest = 9,
        bombedNorth = 10,
        bombedEast = 11,
        bombedSouth = 12,
        bombedWest = 13,
    }
}