# ZMapper
A general purpose tracker for Legend of Zelda. Suitable for randomizers.

ZMapper lets you place and remove various markers indicating which screens 
you've visited, what secrets you've checked for, and more. ZMapper uses global 
hotkeys so that you don't need to switch between programs if playing in an 
emulator. Currently, inputs are hardcoded and use the number pad.

## Keyboard Input
Keyboard input is configurable. The default keyboard mapping is depicted. Functions are described below.
![Input map](ZMapper/Resources/Input2.png)
* **Directional** - Move the cursor on the map, marking screens as visited. Used in conjunction with other keys as well.
* **Bomb** - Toggle bomb marker for bombable caves and dungeon walls. In dungeons, press this key followed by a directional to mark a wall which can not be bombed through.
* **Candle** - Toggle candle marker for burnable bushes.
* **Recorder** - Toggle the recorder marker for secrets revealed by the recorder. 
* **Connections** - Press this key followed by a directional to toggle screen connections. Draw borders around screens in the overworld, and draw connections between rooms in dungeons.
* **Point of Interest** - Mark a screen as a point of interest. Press this key and then enter a 2-digit number corresponding to the desired POI marker.
* **Cleared** - Mark a screen as cleared. In the overworld, a cleared screen will not show any other markers (bomb, flute, or candle), but you can press this key again to unclear the screen and restore the markers.
* **Undo** - Undoes the last change made. This does not include marking screens as visited/unvisited.
* **Delete Screen** - Marks a screen as unvisited. Note that this does not actually remove any data and the screen will be restored to its previous state when marked as visited again.
* **Tilde (`)** - Switch to overworld map.
* **Digits 1-9** - (Non-number-pad digits.) Switch to a dungeon map. 

## Mouse Input
Mouse input is minimal.

### Item Icons
* Left click - Mark an item obtained. For progressive items (e.g. Sword, White Sword, Magic Sword) each successive click advances to the next item level.
* Right click - Mark an item unobtained. For progressive items, each successive click reverts the item to the previous level.

### Map
* Left click - Place the cursor on a screen and mark it visited.
* Right click - Mark a screen as unvisited. Note that the state of markers and screen connections is preserved and will be restored when the screen is marked as visited again.

### Dungeon Thumbnails
* Left click - Switch to the clicked map. If the clicked map is already open, the overworld will be shown instead.