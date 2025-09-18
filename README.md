# Willowbrook 🎮

A 2D top‑down adventure game built with **MonoGame**.  
Play as a traveler helping the frozen town of Willowbrook, solving puzzles and reuniting villagers.

---

## ✨ Features
- Top‑down exploration and quests
- Multiple maps with transitions
- NPC interactions & dialogue
- Puzzle mechanics
- C# + MonoGame (DesktopGL)

---

## 🚀 Getting Started

### Prerequisites
- **Windows or macOS**
- **Visual Studio 2022** (Community is fine) or **JetBrains Rider**
- **.NET SDK** appropriate for the project (your IDE will prompt to install required workloads)
- (Optional, if you edit content) **MonoGame Content Pipeline (MGCB Editor)**

### Build & Run
1. **Clone** the repo
   ```bash
   git clone https://github.com/<your-username>/willowbrook-game.git
   cd willowbrook-game
   ```

2. **Open** the solution file `PASS3.sln` in Visual Studio / Rider.

3. **Restore NuGet packages** (usually automatic). If needed:
   - Visual Studio: *Right‑click the solution → Restore NuGet Packages*

4. **Set startup project** to `PASS3` (if not already):
   - Right‑click `PASS3` → *Set as Startup Project*

5. **Run** the game:
   - Press **F5** or go to *Debug → Start Debugging*

> If you see errors about missing packages or content, restore NuGet again.  
> For content issues and if your project contains a `Content/Content.mgcb`, install MGCB Editor and rebuild content:
> ```bash
> dotnet tool update -g dotnet-mgcb-editor
> mgcb-editor --platform DesktopGL --build Content/Content.mgcb
> ```

---

## 🎮 Controls (default)
- **WASD / Arrow Keys** — Move
- **Space / Enter** — Interact
- **Esc** — Pause/Menu

---

## 📂 Project Structure
```
PASS3.sln          # Solution file
PASS3/             # Game source (C#), assets, content files
.gitignore         # Excludes build + OS files
README.md          # This file
```
> Build output folders like `bin/` and `obj/`, Visual Studio cache `.vs/`, NuGet `packages/`, and macOS `.DS_Store` are intentionally ignored.

---

## 🧭 Roadmap (optional)
- [ ] Quest log UI
- [ ] Save/Load system
- [ ] New map: Ice Caverns
- [ ] Boss encounter prototype

---

## 📸 Screenshots (optional)
Place images in `docs/images/` and reference them here:
```
![Village](docs/images/village.png)
![Cave](docs/images/cave.png)
```

---
