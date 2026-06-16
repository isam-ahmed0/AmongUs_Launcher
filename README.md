# Among Us Launcher

A Windows desktop launcher for Among Us that downloads and launches the game from a configurable source.

## Features

- **Download** — Fetches the latest game archive from a URL
- **Play** — Launches the installed game
- **Setup Wizard** — First-run wizard for Start Menu and shortcut registration
- **Uninstall** — Remove game files only, or full launcher cleanup
- **Open Folder** — Quickly open the game install directory

## Usage

```powershell
dotnet run -c Release
```

Or run `bin\Release\net10.0-windows\AmongUsLauncher.exe` directly.

## Configuration

Edit `appsettings.json`:
- `RawUrl` — GitHub raw URL to a text file containing `link:` and `exe:` lines
- `InstallPath` — Where the game is installed (default: `%LOCALAPPDATA%\AmongUsLauncher`)
