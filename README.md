# W10MGameDevQuickie v3.0-alpha (MonoGame2D tutorial projects/samples)

url: https://github.com/mediaexplorer74/W10MGameDevQuickie

page_type: sample

description: "LubiiiCZ's Quick Monogame2D-based game development tutorials planned to 
port on/to W10M."

languages: csharp

products: windows 10, windows 10 mobile

## About 
This is my new experiment to run some simplest Desktop Monogame2D apps on/in W10M (Astoria build included)). I'm starting some W10MGameDevQuickie 3.0 remake. ;)

## Screenshots (Target: win 11 on desktop pc)
![Button](Images/Q01.png)
![JumpingAndGravity](Images/Q21.png)
![TopDownShooter](Images/P02.png)
![Land](Images/P03.png)
![Infart](Images/P10.png)
![NuPogodi](Images/P11.png)
![InfartonW10M](Images/W10M.png)
![StarfallW10M](Images/P12.png)
![8bitInviders](Images/P22.png)
![Contest](Images/S02.png)


## Features
Min. Win. OS Build descreased to 10240 (Astoria Compatibility)

## Progress
- VS 2017 compatibility partially realized (approx. 15 of 35 projects ready)
- Q01-Q21 ported to UWP, but a lite incomplete (because of no TouchPanel "handlers" yet)
- P01 and P02 ported to UWP too (keyboard/mouse control only at now... no touch, heh!)
- P03 added as *bonus* ( it's my "uwp fork" of https://github.com/semack/land )
- P05 project is only in draft state/mode (not complete at all!)
- P09 project is only some dirty experiments with TileSharp & tesserae for far future tile mapping deals :)
- P10 added as *astoria + uwp + orchestrators* technics demo ( my fork of https://github.com/FrancescoBonizzi/InfartGame )
- P11 added as *uwp + mvvmlight* technics rnd (my "fork/port" of https://github.com/martinsuchan/WP.NuPogodi)
- P12 added as *astoria + uwp + orchestrators* technics RnD (my fork of https://github.com/FrancescoBonizzi/StarfallGame)
- S01-02 added as "game synthez from some game parts" technics RnD (draft at now)
- P13-P21 added
- Q22 added

## Bugs / Errors
- SpriteFont mulfunction 
- Strange scene scaling (some error still here)
- Pixel Shaders (fx) incompatibility
- Map pathfinding seems to be broken ;(

## Requirements
This app requires Visual Studio 2017 or 2022 Community to run. [Click here to learn how to get set up with Visual Studio](https://docs.microsoft.com/windows/uwp/get-started/get-set-up).

## Install and run the app
1. Install **MonoGame 3.6 (3.8?)** for Visual Studio from [monogame.net](http://www.monogame.net/)
2. Clone or download this repository
3. Open **Src\W10MGameDevQuickie.sln** in Visual Studio.
4. Select the desired Qxx (or Pxx) project and set it as default project for run. 
5. To run the selected app, press **F5**. To quit, press **ESC**.

## Learn more about MonoGame UWP gamedev
For a step-by-step walkthrough of the code, check out the [MonoGame 2D tutorial on docs.microsoft.com](https://web.archive.org/web/20170907085024/https://docs.microsoft.com/en-us/windows/uwp/get-started/get-started-tutorial-game-mg2d).

## TODO
- Improve Monogame endine src code (fx, shaders, wav, ogg, etc.).
- Create Wiki.
- Refactor game control for better W10M support :)
- Refactor readme... and something else? 
- Create your own small and sweet retro-games for Windows 10 Mobile os! )))

## References
- https://www.youtube.com/@GameDevQuickie/videos LubiiiCZ's Monogame2D video-tutorials
- https://github.com/LubiiiCZ/DevQuickie The original GameDevQuickie
- https://github.com/LubiiiCZ LubiiiCZ, GameDevQuickie's creator/developer
- https://github.com/FrancescoBonizzi/ Francesco Bonizzi, InFart & StarFall game developer


## ..
As is. No support. For Learning purposes only / DIY :)

## .
[m][e] November, 14 2024
