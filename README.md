# Among Us Launcher

A Windows desktop launcher that downloads Among Us from a configurable source and launches the game.

## Features

- **Download** — Fetches the latest game archive from a remote URL, extracts it, and cleans up
- **Play** — Launches the installed game executable
- **Setup Wizard** — First-run wizard for Start Menu shortcut, Desktop shortcut, and Apps & Features registration
- **Uninstall** — Remove game files only, or full launcher + game cleanup
- **Open Folder** — Opens the game install directory in Explorer
- **Copy Log** — Copy the entire log to clipboard for debugging
- **Version Display** — Shows the installed executable name and file version in the status bar

## Configuration

Edit `appsettings.json` (auto-created on first run):

- `RawUrl` — GitHub raw URL to a text file containing `link:` (download URL) and `exe:` (game executable name) lines
- `InstallPath` — Where the game is installed (default: `%LOCALAPPDATA%\AmongUsLauncher`)

## Download

Get the latest build from the [Releases](https://github.com/isam-ahmed0/AmongUs_Launcher/releases) page.
