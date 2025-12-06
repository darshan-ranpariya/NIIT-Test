# NIIT Unity Developer Test Submission

This project is a submission for the NIIT Unity Developer Test. It includes the complete implementation of Task 1 (Scalable Main Menu System) and Task 2 (Addressable Asset System Demo).

- **Unity Version:** 2022.3.x LTS (or latest LTS)
- **Key Focus:** The implementation prioritizes robust functionality, scalable architecture, and high code quality over visual aesthetics, as per the assignment guidelines. No third-party packages were used for core functionality.

---

## Core Architectural Principles

The entire project is built upon a foundation of SOLID principles and common design patterns to ensure scalability and maintainability.

- **Single Responsibility Principle (SRP):** Each class has a single, well-defined purpose. For example, the `UIManager` handles screen transitions, `LoginScreen` manages only the login UI, and `GameDataSO` is purely a data container. This separation makes the code easier to understand, debug, and extend.

- **State Machine Pattern & Service Locator:** The main menu is implemented as a state machine, where each screen is a "state". The `UIManager` acts as the context, managing the current state and the transitions between them. A `Stack` is used to manage the history of states, allowing for a robust back-navigation feature. A simple static instance (`UIManager.inst`) provides easy access to the manager, acting as a lightweight Service Locator.

- **Observer Pattern:** The `UIManager` uses a C# `event` (`OnNavigationStackChanged`) to broadcast changes to the navigation history. The `GlobalUIManager` subscribes to this event to dynamically show or hide the "Back" button. This decouples the global UI from any specific screen, allowing the system to be highly modular.

- **Data Decoupling via ScriptableObjects:** A `GameDataSO` ScriptableObject is used to store shared data (username, selected level). This is a powerful Unity-native approach that decouples data from scene-specific `MonoBehaviour` instances. It allows data to persist across scene loads (`MainMenu` to `Game`) without relying on cumbersome Singletons or `DontDestroyOnLoad`.

---

## Task 1: Scalable Main Menu System

### Objective
To develop a modular and scalable main menu system with inter-screen data communication and a persistent global options overlay.

### Implementation Details

- **`UIManager.cs`:** The core of the menu system. It holds a list of all screen prefabs and manages their instantiation. It uses a `Stack<BaseScreen>` to track navigation history, enabling the `GoBack()` functionality. The generic `ShowScreen<T>()` method provides a type-safe way to transition to any registered screen.

- **`BaseScreen.cs`:** An abstract base class that all screen controllers inherit from. It provides a common interface with virtual `Show()` and `Hide()` methods, which primarily manage the screen's `CanvasGroup`. This ensures that the `UIManager` can treat all screens polymorphically.

- **Screen Controllers (`WelcomeScreen.cs`, `LoginScreen.cs`, etc.):** Each screen has its own dedicated controller script responsible for its internal logic and UI elements. They communicate with the rest of the system via calls to the `UIManager`.

- **`GameDataSO.cs`:** This ScriptableObject acts as a central, non-scene-based container for the player's username and level selection. The `LoginScreen` writes the username to it, and the `GameSceneController` reads from it after the `Game` scene has loaded.

- **Global UI (`GlobalUIManager.cs`):** The "Options" button, "Back" button, and the volume overlay are managed by this single script on a separate, high-priority canvas. This keeps them independent of the individual screen canvases and ensures they are always accessible. The volume slider directly controls `AudioListener.volume`, affecting all audio sources in the project. The Back button's visibility is dynamically controlled by listening to the `UIManager.OnNavigationStackChanged` event.

### Design Justification

The chosen architecture is highly scalable. To add a new screen:
1.  Create a new screen script inheriting from `BaseScreen`.
2.  Create the UI prefab.
3.  Add the prefab to the `UIManager`'s list.
4.  Call `_uiManager.ShowScreen<NewScreen>()` from any other screen.

No existing screen scripts need to be modified, adhering to the Open/Closed Principle. The decoupling of data and UI logic makes the system robust and easy to maintain.

---

## Task 2: Addressable Asset System Demo

### Objective
To demonstrate proficiency with Unity’s Addressable Asset system, focusing on local asset bundles, inter-group dependencies, and asynchronous loading with progress tracking.

### Implementation Details

- **Asset Setup & Bundling:**
  - Two Addressable Asset groups were created: `EnvironmentAssets` and `CharacterAssets`.
  - An inter-dependency was created: A "Player" prefab in `CharacterAssets` was assigned a material from the `EnvironmentAssets` group.
  - Multiple Addressable Labels (`Environment`, `Character`, `CriticalAssets`) were created to demonstrate different loading strategies.
  - All assets are configured for local bundling and loading.

- **`AddressableManager.cs`:** This class centralizes all Addressables-related logic.
  - **Refactored Core Logic:** The script was optimized to use a single, private `LoadRoutine` coroutine. This removes code duplication and centralizes all UI progress-bar logic, following the DRY (Don't Repeat Yourself) principle.
  - **Generic Public API:** The manager exposes a clean and powerful public API:
    - `LoadSpecificAsset<T>(key, onLoaded)`: Loads a single asset by its unique addressable key and returns the result via a callback.
    - `LoadAssetsByLabel<T>(label)`: Loads a collection of assets that share a common label.
    - `UnloadAllAssets()`: Properly releases all loaded asset handles and destroys any instantiated objects to prevent memory leaks.
  - **Asynchronous Operations:** All loading is handled asynchronously within coroutines to prevent the application from freezing, with UI feedback provided via a slider and TextMeshPro text.

### How to Run the Demo
1.  If you haven't already, open the Addressables Groups window (`Window > Asset Management > Addressables > Groups`).
2.  Click `Build > New Build > Default Build Script` to build the local asset bundles.
3.  Open the `/_Project/Scenes/AddressablesDemo.unity` scene.
4.  Enter Play Mode. Use the UI buttons to load assets by specific key or by shared label, and to unload them.

### Challenges & Rationale
A key challenge during development was ensuring the UI logic in the `AddressableManager` was not duplicated across different loading methods. This was solved by refactoring the coroutines into a single, generic `LoadRoutine` that accepts a handle and a success callback. This approach centralizes the progress bar updates and makes the public-facing API cleaner and more maintainable, demonstrating a commitment to high-quality, reusable code.
