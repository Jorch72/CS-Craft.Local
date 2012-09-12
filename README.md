# Craft.Local

Craft.Net-powered local server for playing singleplayer games on Craft.Net.

## Usage

    Craft.Local.exe port path-to-level.dat

Starts up a local server on 127.0.0.1:port with the specified world.

## Plugin channels

### Craft.Local.Open

Used to open the server to the LAN.

**Payload**:

*byte*: Game mode (0 - survival, 1 - creative, 2 - adventure mode)

*bool*: Allow cheats

### Craft.Local.Exit

Exits the server.

**Payload**:

*none*

## Building from Source

On Windows, add "C:\Windows\Microsoft.NET\Framework\v4.0.30319" to your %PATH% and run this:

    msbuild.exe

From the root of the repository.  This will build it in DEBUG mode. If you want to build in RELEASE mode
(which uses an ILMerge-like tool to consolidate binary files into one), use this:

    msbuild.exe /p:Configuration=RELEASE

On other platforms, install Mono and run "xbuild" from the root of the repository. For RELEASE mode:

    xbuild /property:Configuration=RELEASE