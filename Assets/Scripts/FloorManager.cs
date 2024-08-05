using TMPro;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class FloorManager : MonoBehaviour
{
    public GameObject floorPrefab; // Assign the floor prefab in the Inspector
    public Transform player; // Assign the player transform in the Inspector
    public float floorHeight; // Height of each floor
    private GameObject[] floors = new GameObject[3];
    private int currentFloorIndex;
    private int currentLevel;
    public GameObject enemyPrefab;
    public int enemy_spawn_rate;
    public Transform stairwell; // Assign the stairwell transform in the Inspector

    public int initialLevel = 0;

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Player with tag 'Player' not found.");
        }
        // Initialize two floors: one at the player's level and one above
        currentLevel = initialLevel;
        floors[0] = InstantiateFloor(0);
        floors[1] = InstantiateFloor(1);
        currentFloorIndex = 0;
    }

    void Update()
    {
        float playerY = player.position.y;
        float playerX = player.position.x;

        // Check if the player moved up a floor
        if (playerY > floors[currentFloorIndex].transform.position.y + floorHeight / 2 && playerX > 1.3f)
        {
            MoveFloorsUp();
        }
        // Check if the player moved down a floor
        else if (floors[0] != null && currentFloorIndex > 0 && playerY < floors[currentFloorIndex - 1].transform.position.y + floorHeight && playerX <= 1.3f)
        {
            MoveFloorsDown();
        }
    }

    void MoveFloorsUp()
    {
        // Destroy the bottom floor if it exists
        if (currentFloorIndex == 0)
        {
            floors[2] = InstantiateFloor(2);
            currentFloorIndex++;
        }
        else
        {
            Destroy(floors[0]);

            // Move the middle floor to the bottom position
            floors[0] = floors[1];
            floors[1] = floors[2];

            // Create a new floor above the current top floor
            floors[2] = InstantiateFloor(2);
        }
        SpawnEnemiesOnFloor(floors[2]);
        currentLevel += 1;
        //Debug.Log("moving up");
        //Debug.Log(currentLevel);
    }

    void MoveFloorsDown()
    {
        // Destroy the top floor if it exists
        Destroy(floors[2]);

        // Move the middle floor to the top position
        floors[2] = floors[1];
        floors[1] = floors[0];

        // Check if the new floor's Y position is non-negative before creating it
        Vector3 newPosition = floors[0].transform.position - new Vector3(0, floorHeight, 0);
        if (newPosition.y >= 0)
        {
            floors[0] = InstantiateFloor(-2);
            SpawnEnemiesOnFloor(floors[0]);
        }
        else
        {
            floors[0] = null; // No floor below
        }
        currentLevel -= 1;
        //Debug.Log("moving down");
        //Debug.Log(currentLevel);
    }

    GameObject InstantiateFloor(int levelOffset)
    {
        Vector3 position = new Vector3(0, (currentLevel + levelOffset) * floorHeight, 0);
        GameObject newFloor = Instantiate(floorPrefab, position, Quaternion.identity);
        newFloor.transform.parent = this.transform; // Set the parent to the GameObject with the FloorManager script (stairwell)
        SetLevelText(newFloor, currentLevel + levelOffset);
        BakeNavMesh(newFloor); // Baking NavMesh
        return newFloor;
    }

    void SetLevelText(GameObject floor, int level)
    {
        TextMeshPro levelText = floor.GetComponentInChildren<TextMeshPro>();
        if (levelText != null)
        {
            levelText.text = "" + level;
        }
    }

    void SpawnEnemiesOnFloor(GameObject floor)
    {
        Bounds floorBounds = floor.GetComponent<Renderer>().bounds;
        for (int i = 0; i < enemy_spawn_rate; i++)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(floorBounds.min.x, floorBounds.max.x),
                floor.transform.position.y + 1, // Adjust the Y position to be above the floor
                Random.Range(floorBounds.min.z, floorBounds.max.z)
            );
            GameObject enemy = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
            enemy.transform.parent = transform; // Parent the enemy to the stairwell instead of the floor
        }
    }

    void BakeNavMesh(GameObject floor)
    {
        NavMeshSurface navMeshSurface = floor.GetComponent<NavMeshSurface>();
        if (navMeshSurface != null)
        {
            navMeshSurface.BuildNavMesh();
            foreach (Transform child in floor.transform)
            {
                NavMeshAgent agent = child.GetComponent<NavMeshAgent>();
                if (agent != null)
                {
                    agent.enabled = true;
                }
            }
        }
    }
}