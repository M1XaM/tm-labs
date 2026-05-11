using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool isAlive = false;
    private GridManager gridManager;
    private int x, y;
    private int zone;
    private SpriteRenderer spriteRenderer;
    public int damageCounter = 1; // how many updates it can survive
    public const int maxDamageCounter = 2;


    public void Initialize(GridManager manager, int x, int y, int zone)
    {
        this.gridManager = manager;
        this.x = x;
        this.y = y;
        this.zone = zone;
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetRandomState();
        UpdateAppearance();
    }

    void SetRandomState()
    {
        isAlive = Random.value > 0.5f;
    }

    public void SetState(bool state)
    {
        isAlive = state;
        UpdateAppearance();
    }

    void UpdateAppearance()
    {
        if (gridManager.cellSprites == null || gridManager.cellSprites.Length < 5) return;

        GetComponent<SpriteRenderer>().sprite = isAlive 
            ? gridManager.cellSprites[zone] 
            : gridManager.cellSprites[4];
    }
    

    public int GetAliveNeighbors()
    {
        int aliveCount = 0;
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue; // Skip itself

                int nx = x + dx;
                int ny = y + dy;

                if (gridManager.IsValidCell(nx, ny) && gridManager.GetCell(nx, ny).isAlive)
                {
                    aliveCount++;
                }
            }
        }
        return aliveCount;
    }

    public void MakeAlive()
    {
        isAlive = true;
        UpdateAppearance(); // Update the appearance of the cell (color change, etc.)
    }
}