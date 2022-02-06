# Final Fantasy Pixel Remaster - Save Game Editor
**FFPRSaveEditor** is a prototype save game editor project for the *Final Fantasy Pixel Remaster* series. The initial release supports decrypting, exporting and re-encrypting save game files from the PC (Steam) version of Final Fantasy 1 through 5 (and should be compatible with 6 when it is released in the future).

## How to Use

To get started, copy your save game file(s) from the `%USERPROFILE%\Documents\My Games\FINAL FANTASY PR\Steam\XXX` directory to a safe location. Drag-and-drop the file onto the decrypt executable which will output a plain-text JSON file. After opening and making changes to the file, re-encrypt the save game using the encrypt executable and copy it back into your save game directory.

**Always remember to backup your save game files & directories!**

## Hacking Notes
All FFPR save game data is exported as a large JSON structure which is then DEFLATE compressed, encrypted with a Rijndael cipher and encoded as a Base64 string (in that order). While the raw JSON data is mostly human readable, Square-Enix unfortunately made the shockingly poor decision to stringify every nested JSON node which makes deserializing the structures difficult. The game's parser itself is also very sensitive: save game files which don't conform exactly can cause the game to hang at startup.

## Future

**Help wanted!** I am not a JSON expert. I would appreciate any support from individuals who are interested in working on the project and would like to help sort through the stringified nodes (or figure out how to better deserialize them).

### Roadmap

* Identify and deserialize all JSON nodes into classes
* Create a managed WPF GUI project to edit save games in an interactive way
* ???
