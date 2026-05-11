using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    public int gridSize = 1000; // Grid size (100x100)

    public int gridWidth = 95;
    public int gridHeight = 53;
    public int cellSize = 20; // Each cell is 10x10 pixels
    private Cell[,] grid; // 2D array to store cells

    float updateInterval;
    private float timer;
    private int generations;
    private int genCount = 0;

    public Button PlayButton;
    public Button PauseButton;
    public bool isRunning = false;

    public Slider GenerationsSlider;
    public Text GenerationText; 
    public Text currentGen; 

    public Sprite[] cellSprites; // [0-3] = Zone alive sprites, [4] = Dead sprite
    public float pixelsPerUnit = 100f; // Match this with your sprite's PPU setting
    
    private int minGenerations = 1;
    private int maxGenerations = 5000;
    
    private float maxSpeed = 0.01f;
    private float minSpeed = 1.5f;

    public Button ClearButton;
    public Slider SpeedSlider;


    private void Awake() {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Initiate()
    { 
        CreateGrid();

        PlayButton.onClick.AddListener(PlayGame);
        PauseButton.onClick.AddListener(PauseGame);

        GenerationsSlider.onValueChanged.AddListener(UpdateGenerations);

        GenerationsSlider.value = 0.0f; 
        UpdateGenerations(GenerationsSlider.value);

        ClearButton.onClick.AddListener(ClearGrid);

        SpeedSlider.onValueChanged.AddListener(UpdateSpeed);
        SpeedSlider.value = 0.5f; 
        UpdateSpeed(SpeedSlider.value); 
    }

    public void ClearGrid()
    {
        if (grid == null) return;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (grid[x, y] != null)
                {
                    grid[x, y].SetState(false); // Reset cell to dead
                }
            }
        }

        PauseGame();
        genCount = 0;
    }

    public void UpdateSpeed(float value)
    {
        updateInterval = Mathf.Lerp(minSpeed, maxSpeed, value); 
    }

    public void UpdateGenerations(float value)
    {
        generations = Mathf.RoundToInt(Mathf.Lerp(minGenerations, maxGenerations, value));
        if (generations == maxGenerations) 
        {
            generations = int.MaxValue;
            GenerationText.text = "Number of generations is infinite";
        }
        else 
        {
            GenerationText.text = $"Number of generations: {generations}";
        }
        
        genCount = 0;
        currentGen.text = "Current generation: 0";

        ResetCells();
    }

    void ResetCells()
    {
        if (grid == null) return;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                if (grid[x, y] != null)
                {
                    grid[x, y].SetState(false); // Reset cell to dead
                    grid[x, y].Initialize(this, x, y, GetZone(x, y));
                }
            }
        }
    }

    void ResetGrid()
    {
        PauseGame();
        ResetCells();
    }

    public void PlayGame()
    {
        isRunning = true;
    }

    public void PauseGame()
    {
        isRunning = false;
        timer = 0f;
    }

    void Update()
    {
        if (isRunning)
        {
            if (genCount < generations)
            {
                timer += Time.deltaTime;
                if (timer >= updateInterval)
                {
                    timer = 0f;
                    UpdateGrid();
                    genCount++; 
                    currentGen.text = $"Current generation: {genCount}";
                }
            }
            else
            {
                PauseGame();
                // Stop updating the grid if we've reached the maximum generations
                Debug.Log("Maximum generations reached.");
            }
        }
    }

    public void CreateGrid()
    {
        grid = new Cell[gridWidth, gridHeight];

        // Add camera setup
        float gridWidthTotal = gridWidth * cellSize - 10;
        float gridHeightTotal = gridHeight * cellSize - 10;

        // Set camera to center of grid
        Camera.main.orthographicSize = gridHeightTotal / 2;
        Camera.main.transform.position = new Vector3(
            gridWidthTotal / 2,
            gridHeightTotal / 2,
            Camera.main.transform.position.z
        );
        
        for(int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 position = new Vector3(x * cellSize, y * cellSize, 0);
                GameObject cellObj = new GameObject("Cell");
                cellObj.transform.position = position;
                
                // Add SpriteRenderer component
                SpriteRenderer renderer = cellObj.AddComponent<SpriteRenderer>();
                renderer.sprite = cellSprites[4]; // Default dead sprite
                
                // Set scale based on PPU
                float scale = cellSize / (renderer.sprite.rect.width / pixelsPerUnit);
                cellObj.transform.localScale = new Vector3(scale, scale, 1);

                Cell cell = cellObj.AddComponent<Cell>();
                grid[x,y] = cell;
                
                int zone = GetZone(x, y);
                cell.Initialize(this, x, y, zone);
                cellObj.transform.SetParent(transform);
            }
        }
    }

    void UpdateGrid()
    {
        bool[,] newStates = new bool[gridWidth, gridHeight];
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {

                if (grid[x, y] == null)
            {
                Debug.LogError($"Cell at ({x}, {y}) is null in UpdateGrid.");
                continue; // Skip this cell
            }

                int zone = GetZone(x, y);

                int aliveNeighbors = grid[x, y].GetAliveNeighbors();
                bool isAlive = grid[x, y].isAlive;
                
                if (zone == 1 || zone == 2)
                {
                    if (isAlive && (aliveNeighbors < 2 || aliveNeighbors > 3))
                        newStates[x, y] = false; // Cell dies
                    else if (!isAlive && aliveNeighbors == 3)
                        newStates[x, y] = true; // Cell becomes alive
                    else
                        newStates[x, y] = isAlive; // Remains the same
                }
                else if (zone == 0)
                {
                    //damage zone
                    if (isAlive && aliveNeighbors != 2)
                    {
                        // Decrease damage counter
                        grid[x, y].damageCounter--;

                        if (grid[x, y].damageCounter <= 0)
                            newStates[x, y] = false; // Finally dies
                        else
                            newStates[x, y] = true; // Still alive
                    }
                    else if (!isAlive && aliveNeighbors > 3)
                    {
                        newStates[x, y] = true; // Becomes alive
                        grid[x, y].damageCounter = Cell.maxDamageCounter; // Reset counter
                    }
                    else
                    {
                        newStates[x, y] = isAlive;
                    }

                }
                else
                {
                    //sensory cortex zone
                    if (isAlive && (aliveNeighbors > 3 || aliveNeighbors < 1)) {
                        newStates[x, y] = false; // dies
                    } else if (!isAlive && aliveNeighbors > 1){
                        newStates[x, y] = true; // becomes alive
                    } else {
                        newStates[x, y] = isAlive;
                    }
                }


            }
        }

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                // Check if cell is dead and triggers the 0.3% chance
                if (!newStates[x, y] && Random.value < 0.003f)
                {
                    newStates[x, y] = true; // Revive center cell
                    
                    // Reset damage counter for center cell if in zone 0
                    if (GetZone(x, y) == 0)
                    {
                        grid[x, y].damageCounter = Cell.maxDamageCounter;
                    }

                    // Revive cross-shaped neighbors
                    ReviveCrossNeighbors(x, y, newStates);
                }
            }
        }

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                grid[x, y].SetState(newStates[x, y]);
            }
        }
    }

    private void ReviveCrossNeighbors(int x, int y, bool[,] newStates)
    {
        int[] dx = { 0, 0, -1, 1 }; // Left, Right
        int[] dy = { 1, -1, 0, 0 }; // Up, Down

        for (int i = 0; i < 4; i++)
        {
            int nx = x + dx[i];
            int ny = y + dy[i];

            if (IsValidCell(nx, ny))
            {
                newStates[nx, ny] = true; // Revive neighbor
                
                // Reset damage counter if neighbor is in zone 0
                if (GetZone(nx, ny) == 0)
                {
                    grid[nx, ny].damageCounter = Cell.maxDamageCounter;
                }
            }
        }
    }    

    public bool IsValidCell(int x, int y)
    {
        return x >= 0 && x < gridWidth && y >= 0 && y < gridHeight;
    }

    public Cell GetCell(int x, int y)
    {
        return grid[x, y];
    }   

    int GetZone(int x, int y)
    {
        if (x < gridWidth / 2 && y < gridHeight / 2) return 0; // Bottom-left
        if (x >= gridWidth / 2 && y < gridHeight / 2) return 1; // Bottom-right
        if (x < gridWidth / 2 && y >= gridHeight / 2) return 2; // Top-left
        return 3; // Top-right
    }

    public void ActivateCellsAtMousePosition(Vector3 mousePosition)
    {
        // Convert world position to grid position
        Vector2 worldPoint = new Vector2(mousePosition.x, mousePosition.y);
        
        // Convert world coordinates to grid cell coordinates
        int x = Mathf.FloorToInt((worldPoint.x + cellSize / 2f) / cellSize);
        int y = Mathf.FloorToInt((worldPoint.y + cellSize / 2f) / cellSize);

        
        // Ensure we're within the grid bounds
        if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
        {
            Cell cell = grid[x, y];
            if (cell != null)
            {
                cell.MakeAlive(); // Activate the cell
            }
        }
    }
}