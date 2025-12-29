# Willowbrook 🎮

**Willowbrook** is a 2D top‑down adventure game built with **MonoGame** and **C#**.  
Play as a traveler helping the frozen town of Willowbrook by solving puzzles, exploring maps, and reconnecting villagers.

---

## ✨ Features
- Top‑down exploration and quest progression
- Multiple maps with transitions
- NPC interactions & dialogue system
- Puzzle mechanics and minigames
- Built with **MonoGame (DesktopGL)**

---

## 🛠 Tech Stack
- **C#**
- **MonoGame 3.7 (DesktopGL)**
- **.NET Framework 4.6.1**
- **Visual Studio 2022**
- NuGet (**packages.config**)

---

## 🚀 Getting Started

### ⚠️ Platform Support
> **Running the game is supported on Windows only.**  
> The project targets **.NET Framework 4.6.1**, which requires Windows and Visual Studio.

macOS/Linux users may edit the code, but cannot build or run the game without porting the project to modern .NET.

---

### Prerequisites
- **Windows 10 / 11**
- **Visual Studio 2022 (Community or higher)**
  - Workload: **.NET desktop development**
  - Individual components:
    - **.NET Framework 4.6.1 SDK**
    - **.NET Framework 4.6.1 Targeting Pack**
- **Git**

> VS Code can be used for editing, but **Visual Studio is required to build and run**.

---

### Build & Run
1. **Clone the repository**
   ```bash
   git clone https://github.com/<your-username>/willowbrook-game.git
   cd willowbrook-game
   ```

2. **Open** `PASS3.sln` in **Visual Studio 2022**

3. **Restore NuGet packages**
   - Visual Studio usually prompts automatically  
   - If not:  
     **Right‑click Solution → Restore NuGet Packages**

4. **Set startup project**
   - Right‑click `PASS3` → **Set as Startup Project**

5. **Run the game**
   - Press **F5** or use *Debug → Start Debugging*

---

## 📦 Creating a Release Build
While this project cannot be published as a single self‑contained executable, you can create a **portable Windows build**.

1. In Visual Studio, switch the configuration to **Release**
2. Build the solution (**Build → Build Solution**)
3. Locate the output folder:
   ```
   PASS3/bin/Release/
   ```
   (or `PASS3/bin/x64/Release/` depending on configuration)

4. Run `PASS3.exe` from that folder to verify it works
5. Zip the **entire Release folder** and distribute it

Users can run the game by unzipping and double‑clicking `PASS3.exe`.

---

## 🎮 Controls (default)
- **WASD / Arrow Keys** — Move
- **Space / Enter** — Interact
- **Esc** — Pause / Menu

---

## 📁 Repository Notes
- Editor‑specific files such as `.vscode/` are intentionally ignored
- Build artifacts are excluded (`bin/`, `obj/`, `packages/`, `.vs/`)
- This is a legacy MonoGame project targeting .NET Framework

---

## 🔮 Future Improvements
- Port project to **modern .NET (net6+/net8)** for cross‑platform support
- Upgrade to **MonoGame 3.8+**
- Refactor content pipeline and asset loading
- Expanded quest lines and NPC behaviors

---

## 📜 License
This project is for educational and personal use.
