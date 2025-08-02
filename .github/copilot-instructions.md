# GitHub Copilot Instructions for RimWorld Modding: Swords (Continued)

## Mod Overview and Purpose
**Swords (Continued)** is a RimWorld mod that revives and updates Trunken's original mod to enhance the medieval combat experience. Players can immerse themselves in a world where legendary swords, crafted by master smiths, hold incredible power and variety. This mod introduces a collection of meticulously designed swords, each offering unique capabilities and challenges on the battlefield. 

## Key Features and Systems
- **Legendary Swords**: Adds five distinct legendary swords — Cloud Sword (Storm), Nightfall (Poison), Executioner (Frost), Deathbringer (Bleed), and Red Queen (Flame).
- **Varied Attack Mechanics**: Each sword provides eight different attacks, encompassing a spectrum of tactical options such as stabs, slashes, and hidden effects.
- **Enhanced Crafting Requirements**: Swords necessitate high crafting skills, appropriate technology levels, and research, emphasizing the player's progression.
- **Compatibility and Integration**: Compatible with several mods including Combat Extended and JecsTools. The precise load order supports seamless gameplay integration.

## Coding Patterns and Conventions
- Follow established C# coding standards such as PascalCase for public fields and methods, camelCase for private fields.
- Adherence to RimWorld's modding conventions, emphasizing readability and maintainability.
- Utilize appropriate namespaces to prevent conflicts and ensure modular code design.

## XML Integration
- Define new swords and their associated properties using XML Defs, ensuring integration with the game's item generation system.
- Use XML patching for compatibility with existing mods and game mechanics, maintaining game balance and functionality.

## Harmony Patching
- Intermingle Harmony patches to override or extend RimWorld's core functionalities where necessary.
- Structure Harmony classes as `internal static` and ensure patches comply with best practices to prevent conflicts and improve performance.

## Suggestions for Copilot
- **Smart Code Suggestions**: Leverage Copilot to generate boilerplate code for new classes, methods, or XML configurations, reducing development time.
- **Complex Logic Assistance**: Use Copilot for integrating complex attack mechanics or harmony patches, increasing efficiency and accuracy.
- **Error Handling and Debugging**: Encourage Copilot to provide alerts or suggestions for potential pitfalls, ensuring robust and stable mod performance.
- **Documentation**: Request Copilot to add or suggest inline comments and documentation for methods and classes, improving codebase clarity.

Encourage contributions and modifications through clear code structuring and documentation, allowing other developers to easily interact with and expand upon the existing framework. Join us on Discord for community support and collaboration!
