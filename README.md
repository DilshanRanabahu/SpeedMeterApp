# SpeedMeterApp

A sleek, lightweight, and modern cross-platform Network Speed Monitor built with Avalonia UI. It displays your current download and upload speeds in a small, unobtrusive floating widget and includes a beautiful real-time graph.

## Features
- **Real-Time Network Speed:** Accurately displays your active Upload and Download speeds.
- **Cross-Platform:** Built with Avalonia UI, supporting native execution on Windows, Linux, and macOS.
- **Real-Time Graph:** Right-click the widget and select "Show Graph" to see a smooth, real-time plotted graph of your network traffic over the last 60 seconds.
- **Auto-Startup (Windows):** Automatically registers itself in the Windows Registry to start on boot without manual configuration.
- **Standalone Executable:** Fully standalone and self-contained build, meaning you don't need .NET installed on your PC to run it!
- **Modern Dark Theme:** Aesthetically pleasing dark UI design out-of-the-box.

## How to Run

### Option 1: Standalone Build
Simply download the standalone executable (`SpeedMeterApp.exe`) and double-click to run!
- **On Windows:** It will automatically add itself to your startup programs on the first launch.

### Option 2: Build from Source
If you wish to run or modify the application from source, you must have the [.NET 10 SDK](https://dotnet.microsoft.com/) installed.

1. Clone the repository.
2. Navigate to the project directory:
   ```bash
   cd SpeedMeterApp
   ```
3. Run the application:
   ```bash
   dotnet run
   ```

## Controls
- **Move Widget:** Left-click and drag anywhere on the widget or graph to move it around your screen.
- **Open Menu:** Right-click the widget to open the Context Menu.
- **Show Graph:** Click "Show Graph" in the Context Menu to open the real-time visualizer.
- **Exit:** Click "Exit" in the Context Menu to completely close the app.
