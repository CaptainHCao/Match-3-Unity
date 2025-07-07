# Project Summary and Improvements

### 1. Modifications Made to Improve Performance

- Instead of using `Resources.Load()` every time an item is created—which is not good practice during runtime—an **Item Manager** has been implemented. The Item Manager initializes all item prefabs before they are used. This will serve as the core foundation for item pooling later.

- `Update()` in `GameManager` calls `Update()` in `BoardController`. As a result, each `BoardController`'s `Update()` was being called twice every frame. `Update()` in `BoardController` has now been modified: it will only be called by `GameManager` and no longer executes on its own.

- Functions in `Cell.cs` are frequently called, but performance was not optimized. The main reason was an O(n²) complexity (a double loop) used to check all cells on the board. While creating new `Cell` objects often leads to a slight performance hit, this is a reasonable tradeoff to avoid overly complex code.

  However, in the `Fill()` method, a `List<NormalItem.eNormalType>` was being created during every entry into a double loop, which is **much worse** than instantiating a `Cell`. This performance issue has been resolved using a temporary holder list that is reused by multiple methods.

**Script(s) add**: `ItemManager.cs`
**Scrtip(s) modified**: `GameManager.cs`, `Cell.cs`, `BoardController.cs`,  `Board.cs`
  
---

### 2. Reskin Item Textures with ScriptableObject

An editor tool has been created under `Assets/Scripts/Editor`.

**To use the tool:**

1. In the top task bar, go to: `Tools -> Texture Set Assigner`.
2. Drag in the **ItemSet ScriptableObject** and **PrefabSet ScriptableObject**.
   - Ensure both lists have equal quantities.
3. Examples are provided in the `Resources` folder:
   - `Skins Set 1/2` for the `ItemSet` ScriptableObject
   - `Normal/Special Item Set` for the `PrefabSet` ScriptableObject
4. Click **"Assign Sprites from SO"** and the textures will be applied to the prefabs in the Editor.

**Script(s) add**: `ItemManager.cs`, `PrefabSet.cs`, `TextureSet.cs`

---

### 3. Restart Button

A **Restart** button has been added. Players can restart the current game mode, which will:
- Spawn a new board
- Reset the timer or move count

**Script(s) add**: `UIPanelRestart.cs`
**Script(s) add**: `UIMainManager.cs`

---

### 4. Smarter Item Spawning

`FillGapsWithNewItems()` has been modified to:
- Track current item types on the board
- Spawn new items **avoiding matches** with the 4 neighboring cells
- **Prioritize spawning** item types that are least present on the board

**Scrtip(s) modified**: `Board.cs`

---

### 5. Project Evaluation

#### ✅ Advantages:
- Interfaces and implementations are clearly defined
- Scripts and resources are logically organized and properly named
- The state machine and game flow are simple and easy to modify

#### ⚠️ Disadvantages:
- A lot of reused code can be separated for better abstraction  
  *(DRY Principle — Don't Repeat Yourself)*  
  For example: there are too many spots in the scripts that handle destruction of a `Cell` or an `Item` view. This makes item pooling—something that should be simple—too complicated, as unused GameObjects are scattered across multiple places.  
  **Suggestion:** Create separate methods/functions for logic that is reused often. Most importantly, abstract out item creation and destruction. Once that's done, item pooling will come naturally.

- Using **references instead of strings** is better for debugging and maintainability.  
  **Suggestion:** Some `public` variables in `Constants.cs` have already been replaced by the `ItemManager`. Continue this process to improve safety and clarity.

