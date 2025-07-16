# GitHub Copilot Instructions for RimWorld Modding Project

## Mod Overview and Purpose
The "Crystal Ball" mod for RimWorld introduces an enchanting new feature to the game world, allowing players to engage in scrying activities using a mystical crystal ball. The pinnacle of this mod is the `Building_CrystalBallTable`, where pawns can predict future events, potentially altering the course of gameplay by providing forewarning of upcoming incidents.

## Key Features and Systems

- **Scrying Mechanics**: 
  - The `Building_CrystalBallTable` class handles the core scry functions with methods such as `GetCurrentProgress()`, `PerformScryWork()`, and `isReadyForScrying()`.
  - The `JobDriver_ScryCrystalBall` class facilitates the execution of the scrying job by pawns.

- **Incident Prediction and Management**:
  - `WarnedIncidentQueueWorldComponent` manages a queue of predicted events, offering methods for adding, removing, and ticking incidents.

- **User Interface**:
  - The `ITab_CrystalBallPredictions` class adds an interface tab that lets players see predicted incidents via `DrawKnownIncidentList()`.

## Coding Patterns and Conventions

- **Class Naming**: 
  - Classes are named with a clear purpose, directly indicating their role. For instance, `Building_CrystalBallTable` and `ITab_CrystalBallPredictions`.

- **Method Naming**:
  - Methods are verb-based, descriptive of their function: `AddKnownIncident()`, `PerformScryWork()`, `PredictEvents()`, etc.

- **Access Modifiers**:
  - Ensure correct usage of `public`, `internal`, and `private` to control access where necessary.

- **Event-driven Programming**:
  - Utilize specific methods in classes like `WarnedIncidentQueueWorldComponent` to respond to game world events efficiently.

## XML Integration

While there isn't explicit XML content summarized here, a typical process includes:

- **Defining new objects and jobs** in XML files using `<Defs>` sections.
- **Integrating C# code** with XML by matching method functionality to XML tags such as `<WorkGiver>`, `<Job>`, and defining related criteria.

## Harmony Patching

- **HarmonyPatches Class**: 
  - This class should host all Harmony patches necessary to modify the existing game logic non-destructively. Follow principles of being minimally invasive, focusing on `Postfix`, `Prefix`, `Transpilers`, and `Finalizer` as needed.
  
- **Best Practices**: 
  - Use Harmony to hook into methods where mod functionality needs to change or extend base game behavior without altering core files.

## Suggestions for Copilot

1. **Generate Code Stubs**:
   - Use Copilot to create initial stubs for methods and classes by entering descriptive comments.

2. **XML Code Generation**:
   - Prompt generation of XML snippets based on C# code components for seamless integration between XML definitions and C# methods.

3. **Harmony Integration**:
   - Suggest full Harmony hooks by starting patterns with `Harmony.PatchAll()` or targeting specific methods using `var harmonyInstance = new Harmony("modname.id");`.

4. **Fixing Common Bugs**:
   - Utilize Copilot to catch and suggest fixes for common issues like incorrect type casting or off-by-one errors in loops.

5. **Interface Elements**:
   - Draft new UI components and integrate these with game logic using class methods with Copilot’s assistance.

With adherence to these guidelines and strategic use of GitHub Copilot, the development and enhancement of the "Crystal Ball" mod should be a seamless and efficient process.
