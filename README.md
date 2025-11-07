## Architecture

The project is built with emphasis on clean component architecture rather than performance optimization. This allows for easy extension and behavior modification.

### Core Principles

**Component-based approach to pathfinding**

- Pathfinding is encapsulated in the IPathfindingProvider interface
- The provider implementation (NavMeshPathfindingProvider) can easily be replaced with another (A*, simple search, etc.)
- Drones are not dependent on a specific navigation system

**State Machine for behavior**

- Each drone uses a StateMachine to manage states
- States: SearchResource → MoveToResource → CollectResource → ReturnToBase → UnloadResource
- Transitions between states are defined within each state through ChangeState()
- New states can be easily added without modifying the logic of others

### Component Responsibilities

- **HarvesterController** — main component that manages the drone
- **IPathfindingProvider** — navigation (replaceable component)
- **PathVisualizer** — path visualization
- **BaseController** — faction and resource management
- **LootSpawner** — resource generation
- **Pool** — optimization of object creation/deletion

### Performance Optimization Notes

⚠️ Some solutions prioritize architecture over performance

- FindObjectsByType is called during resource search (cached via SearchInterval)
- Target validation checks every frame (but with interval-based polling)

