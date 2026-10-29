# GitHub Copilot Instructions for RimWorld Modding - CrystalBall (Continued)

Welcome to the CrystalBall mod development project! This mod allows colonists to scry into the future, preparing for future incidents. Below are detailed instructions and guidelines to help you effectively use GitHub Copilot for developing this mod.

## Mod Overview and Purpose

**Mod Name:** CrystalBall (Continued)  
**Author:** The Pickle  
**Package ID:** Mlie.CrystalBall  
**Mod Description:** Update of The Pickles mod  
- Creates a crystal ball that pawns can use to scry into the future  
- Helps prepare for future incidents without delaying any quest-related incidents  

## Key Features and Systems

- **Crystal Ball Table:** Created via the Production tab using Jade resources  
- **Scry Work:** Colonists can scry into the future; influenced by Intellectual and Psychic Sensitivity  
- **Incident Delay:** Delays future incidents by a random interval, compatible with Storytellers  
- **Mod Settings:** Adjust incident delays and scry frequency  
- **Compatibility:** May have compatibility issues with other mods affecting incident triggers

## Coding Patterns and Conventions

- **C# File Structure:** Follow a clear and organized structure, separating logic into different classes as needed.
- **Naming Conventions:** Use PascalCase for class names and camelCase for method names.
- **Comments and Documentation:** Write clear comments and documentation to explain complex logic or important changes.
- **Error Handling:** Implement robust error handling to prevent crashes and unexpected behavior.

## XML Integration

- **XML Files:** Use XML files to define game objects, effects, job definitions, stats, and thing definitions.
- **Consistent Formatting:** Maintain consistent formatting for XML tags and attributes.
- **Validation:** Ensure XML files are validated against RimWorld's XML schema to prevent loading errors.

### Example XML File

**Building_CrystalBall.xml:**
xml
<ThingDef ParentName="Building" Name="CrystalBallTable">
    <label>Crystal Ball Table</label>
    <description>A table used by colonists to scry into the future.</description>
    <defName>CrystalBallTable</defName>
    <category>Building</category>
    <thingCategories>
        <li>Building</li>
        <li>Table</li>
    </thingCategories>
    <statBases>
        <WorkToBuild>100</WorkToBuild>
    </statBases>
    <costList>
        <li>Resource=Jade;Count=20</li>
    </costList>
    <workGiverDefs>
        <li>WorkGiver_CrystalBall</li>
    </workGiverDefs>
</ThingDef>


## Harmony Patching

- **Harmony Library:** Utilize the Harmony library to patch existing RimWorld methods without modifying the original codebase.
- **Patch Structure:** Follow a clear structure for Harmony patches, including prefix, postfix, transpiler, etc.
- **Naming Conventions:** Use descriptive names for patch methods to indicate their purpose.

### Example Harmony Patch

**CrystalBall.cs:**
csharp
using HarmonyLib;

public class CrystalBall
{
    [HarmonyPatch(typeof(IncidentWorker), nameof(IncidentWorker.TryExecute))]
    public static class IncidentWorker_TryExecute_Patch
    {
        public static void Prefix(IncidentWorker __instance, IncidentParms parms)
        {
            // Implement logic to delay incidents
        }
    }
}


## Suggestions for Copilot

- **Code Completion:** Use Copilot to complete code snippets, especially for repetitive tasks like defining XML elements or implementing Harmony patches.
- **Documentation:** Let Copilot generate documentation comments for your classes and methods to ensure clarity and maintainability.
- **Error Handling:** Use Copilot to suggest error handling code, such as try-catch blocks, to prevent unexpected behavior.
- **Refactoring:** Use Copilot to refactor existing code, such as improving method readability or optimizing logic.

## Conclusion

By following these guidelines and utilizing GitHub Copilot effectively, you can enhance your development process and create a robust and user-friendly mod for RimWorld. Happy coding!

## Project Solution Guidelines
- Relevant mod XML files are included as Solution Items under the solution folder named XML, these can be read and modified from within the solution.
- Use these in-solution XML files as the primary files for reference and modification.
- The `.github/copilot-instructions.md` file is included in the solution under the `.github` solution folder, so it should be read/modified from within the solution instead of using paths outside the solution. Update this file once only, as it and the parent-path solution reference point to the same file in this workspace.
- When making functional changes in this mod, ensure the documented features stay in sync with implementation; use the in-solution `.github` copy as the primary file.
- In the solution is also a project called Assembly-CSharp, containing a read-only version of the decompiled game source, for reference and debugging purposes.
- For any new documentation, update this copilot-instructions.md file rather than creating separate documentation files.


## Hard rules (must follow)
- Do NOT run commands that modify the repo (no git commit, git apply, dotnet format) unless explicitly asked.
- Prefer minimal reads: read only the smallest code region needed (around the suspicious lines).
- When mentioning SonarQube issues, automatically use the SonarQube MCP service to fetch and address issues instead of making inferred fixes without querying SonarQube first.
- When mentioning the rimworld log, automatically use the Rimworld MCP service to fetch the log.

