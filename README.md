# Digital Logic Gates VR Lab

An interactive, high-fidelity 3D and VR-ready digital logic simulation laboratory built natively in Unity without external 3D models or third-party asset dependencies.

---

## Project Overview

**Digital Logic Gates VR Lab** provides educational simulation, physical circuit experimentation, and theoretical learning of fundamental digital logic gates and data routing circuits:
- **7 Simulated Circuits**: AND, OR, NOT, NAND (Universal), NOR (Universal), 2:1 MUX (Multiplexer), 1:2 DEMUX (Demultiplexer).
- **Physical 3D Workbench**: Procedural workbench with animated interactive lever switches, illuminated glowing sphere lamps, active signal wire rendering (LineRenderers with logic-high emission), and IEEE logic gate symbols.
- **Strict 4-Screen Architecture**:
  1. **Screen 1 (Main Menu)**: Welcome, system overview, How-To modal guide, navigation.
  2. **Screen 2 (Theory)**: Interactive cards for each circuit, Boolean expressions, truth tables, universal gate theory, and practical engineering applications.
  3. **Screen 3 (Practical Lab)**: Unobtrusive sidebar UI preserving full view of the central 3D laboratory, live truth table with active row highlighting, dynamic I/O summaries, and reset controls.
  4. **Screen 4 (Quiz & Results)**: 10-question master bank, Fisher-Yates shuffle selecting 5 distinct questions per attempt, randomized options with remapped answers, answer locking, score tracking, and results review cards.

---

## Folder Structure

```
Unity Logic gate/
├── Assets/
│   ├── Editor/
│   │   └── ProjectSetup.cs              # "Digital Logic Lab > Setup Project" tool
│   ├── Materials/                       # Procedural materials
│   ├── Prefabs/                         # Prefabs directory
│   ├── Resources/                       # Optional resources
│   ├── Scenes/
│   │   └── Main.unity                   # Primary lab scene (built via code)
│   ├── Scripts/
│   │   ├── Core/
│   │   │   └── AppBootstrap.cs          # Runtime initialization of UI, lab, and managers
│   │   ├── Input/
│   │   │   ├── IInteractable.cs         # VR-ready interactable interface
│   │   │   ├── IInputProvider.cs        # Input provider abstraction
│   │   │   └── DesktopInputProvider.cs  # Desktop mouse & keyboard implementation
│   │   ├── Lab/
│   │   │   ├── CircuitSymbolRenderer.cs # Procedural IEEE symbols & active wire illumination
│   │   │   ├── InputSwitch.cs           # Physical 3D toggle switch component
│   │   │   ├── LabController.cs         # Lab coordinator, selection, and truth table sync
│   │   │   ├── LabEnvironmentBuilder.cs # Procedural 3D room, bench, board, and props
│   │   │   └── OutputLamp.cs            # Emissive output lamp component
│   │   ├── Logic/
│   │   │   ├── LogicEngine.cs           # Pure static C# evaluation engine
│   │   │   └── LogicEngineTests.cs      # Automated unit tests for all 7 circuits
│   │   ├── Quiz/
│   │   │   ├── QuizManager.cs           # Screen 4 UI coordinator and results review
│   │   │   ├── QuizQuestion.cs          # Question models and pure C# QuizEngine
│   │   │   └── QuizSimulationTests.cs   # 200-attempt automated verification tests
│   │   ├── Theory/
│   │   │   └── TheoryPanel.cs           # Screen 2 theory cards and reference tabs
│   │   └── UI/
│   │       ├── MenuPanel.cs             # Screen 1 main menu and how-to modal
│   │       └── ScreenManager.cs         # Strict 4-screen navigation manager
│   ├── Textures/                        # Textures directory
│   └── UI/                              # UI elements
├── Packages/
│   └── manifest.json                    # Clean Unity dependencies (uGUI, physics, core)
├── ProjectSettings/
│   └── ProjectVersion.txt               # Unity version configuration
└── README.md
```

---

## How to Open and Run

### Requirements
- **Unity Version**: Unity 2022.3 LTS or newer (verified on Unity 6000.5.8f1).
- **Render Pipeline**: Built-in or Universal Render Pipeline (URP-agnostic fallback to Standard, Lit, Sprites/Default, or Unlit/Color).
- **Dependencies**: Uses only Unity built-in primitives, uGUI, TextMeshPro, and LineRenderer. No external assets required.

### Steps
1. **Open Project**:
   - Open Unity Hub.
   - Click **Add** -> **Add project from disk**.
   - Select the folder: `c:\Users\payal\Documents\GitHub\Unity Logic gate`.
   - Open the project in Unity Editor.
2. **Initial Setup (Automated)**:
   - In the top Unity menu, click:  
     `Digital Logic Lab` -> `Setup Project`
   - This ensures `Assets/Scenes/Main.unity` is generated, added to Build Settings, and TMP Essentials are imported.
3. **Open Scene & Play**:
   - In the Project browser, open `Assets/Scenes/Main.unity`.
   - Press the **Play** button in Unity Editor.
   - `AppBootstrap.cs` will procedurally instantiate the 3D lab environment, wire inputs/outputs, and initialize the UI.

---

## Controls & Interaction

- **Mouse Click**:
  - Click directly on 3D physical switches (`A`, `B`, `S`, `D`) on the workbench to toggle their states (0 ↔ 1).
  - Alternatively, click the sidebar input toggle buttons for quick accessibility.
  - Click UI buttons across all screens.
- **Keyboard Shortcuts**:
  - `1` through `7`: Quick-select circuit in Practical Lab:
    - `1`: AND Gate
    - `2`: OR Gate
    - `3`: NOT Gate
    - `4`: NAND Gate
    - `5`: NOR Gate
    - `6`: 2:1 Multiplexer (MUX)
    - `7`: 1:2 Demultiplexer (DEMUX)
  - `Escape`: Return to Main Menu.

---

## Automated Tests & Verification

The project includes two built-in test suites accessible directly via the Unity Editor menu:
1. **Logic Engine Unit Tests**:
   - Menu: `Digital Logic Lab` -> `Run Logic Engine Tests`
   - Verifies 45 test assertions covering truth tables for all 7 circuits, dynamic generation consistency, and asserts that DEMUX never outputs $Y0=1 \land Y1=1$.
2. **200 Quiz Simulation Attempts**:
   - Menu: `Digital Logic Lab` -> `Run 200 Quiz Simulation Tests`
   - Simulates 200 complete quiz attempts, asserting:
     - Exactly 5 distinct questions per attempt.
     - Option shuffling maintains valid correct-answer remapping.
     - Scores are bounded between 0 and 5.
     - Set diversification between consecutive attempts.

---

## VR Setup (Not Implemented)

### Implemented Target
The implemented target is **Desktop Keyboard & Mouse**. Immersive VR headsets, 6-DoF controllers, and hand-tracking are **NOT implemented or tested**.

### Prepared VR-Ready Architecture
The codebase is structured to allow seamless conversion to VR:
1. **`IInteractable` Interface**:
   [InputSwitch.cs](file:///c:/Users/payal/Documents/GitHub/Unity%20Logic%20gate/Assets/Scripts/Lab/InputSwitch.cs) implements `IInteractable` (`Interact()`, `GetInteractPrompt()`, `IsInteractable()`).
2. **Input Abstraction**:
   [DesktopInputProvider.cs](file:///c:/Users/payal/Documents/GitHub/Unity%20Logic%20gate/Assets/Scripts/Input/DesktopInputProvider.cs) implements `IInputProvider`, allowing drop-in replacement by an XR controller provider.
3. **World-Space UI Layout**:
   All UI elements are structured in discrete, anchored sidebar panels that can be converted from `RenderMode.ScreenSpaceOverlay` to `RenderMode.WorldSpace`.

### Steps Required for Future VR Implementation
To convert the lab to VR:
1. Open Unity Package Manager and install:
   - **XR Interaction Toolkit** (`com.unity.xr.interaction.toolkit`)
   - **OpenXR Plugin** (`com.unity.xr.openxr`)
2. In `Assets/Scenes/Main.unity`, replace the default `Main Camera` with an **XR Origin (VR)** rig.
3. Replace `DesktopInputProvider` with an XR Ray Interactor or Direct Interactor that invokes `IInteractable.Interact()` upon trigger press.
4. On the UI Canvas, switch `RenderMode` to `World Space`, position the panels around the workbench, and add a `TrackedDeviceGraphicRaycaster`.
