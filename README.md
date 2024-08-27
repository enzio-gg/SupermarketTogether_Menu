# ENZIO - Supermarket Together Menu
## Prerequisites
1. Clone this repository with `git clone https://github.com/enzio-gg/SupermarketTogether_Menu`
2. Restore NuGet Packages
3. Create a `libs` folder inside the root directory of this project `SupermarketTogether_Menu\libs`
4. Copy all used Dynamic Linked Libraries (DLLs) from the games Managed folder  
   `Supermarket Together\Supermarket Together_Data\Managed` into the newly created `libs` folder `SupermarketTogether_Menu\libs`   
<sub>If you want, you can copy all DLLs too, except any System ones e.g. `System.*`, if you plan on using them</sub>  
5. Download BepInEx 6 via [Bleeding Edge (BE) build](https://builds.bepinex.dev/projects/bepinex_be) `BepInEx-Unity.Mono-win-x64-6.0.0-be.***+*******.zip`
6. Extract the contents of the BepInEx zip file into your games directory and launch it for the first time with BepInEx
7. Configure `BepInEx\config\BepInEx.cfg` to your liking

## Build Instructions
1. Build the project
3. Copy the generated `SupermarketTogether_Menu\Builds\Debug|Release\ENZIO_SupermarketTogetherMenu.dll` file into the `BepInEx\plugins` folder  
4. Enjoy

## Keybinds
- F1: Open Menu
