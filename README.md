# Proposal

This game is going to be an implementation of the well-known game *Minesweeper*, which is likely familiar to anyone who has used any version of windows older than Windows 8 for a while.

The game will have a grid of tiles, some of which may be mines. If a tile is left-clicked, the following will happen: if it is a mine, it will explode and the game will end, otherwise it and surrounding (TODO: which?) non-mine tiles will be cleared away. When a tile is right-clicked, if it is not flagged as a mine it will be flagged, otherwise it will be unflagged.

When a tile is cleared, it is replaced with a number showing how many mines are adjacent to it (including diagonally adjacent ones).

The game will likely include a simple high score tracker, different difficulties, and custom parameters, similarly to the Windows version of the game. Implementing these will require the creation of simple user interface elements such as dropdowns, text fields, and buttons.

NOTE: This will have no main menu, as is usual for the game. The game will immediately begin when opened.

# Assets

The assets will probably be made using *mspaint.exe* on a *Windows* machine, either physical or virtual.

# Timeline

1. Create the images used in game
2. Create the layout for the game in Unity's editor
3. Implement the game's logic in C#