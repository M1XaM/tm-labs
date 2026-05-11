using Godot;
using System.Collections.Generic;

public partial class Farm : TileMapLayer
{
	public Dictionary<Vector2I, int> _cellTimes = new Dictionary<Vector2I, int>();
	
	public override void _Ready()
	{
		// Initialize dictionary with all existing cells
		foreach (Vector2I cellPosition in GetUsedCells())
		{
			_cellTimes[cellPosition] = 0;
		}
	}
	
	public override void _Process(double delta)
	{
		foreach (Vector2I cellPosition in GetUsedCells())
		{
			Vector2I atlasCoords = GetCellAtlasCoords(cellPosition);
			CheckIfCellIsPlanted(cellPosition, atlasCoords);
		}
	}
	
	private void CheckIfCellIsPlanted(Vector2I cellPosition, Vector2I atlasCoords)
	{
		// Empty cell (soil)
		if(atlasCoords == new Vector2I(63, 11))  
		{
			return;
		}
		
		// First growth phase - initial planting
		else if (atlasCoords == new Vector2I(51, 12))  
		{
			if (TimeManager.Instance.Days > _cellTimes[cellPosition])
			{
				// Advance to phase 2
				SetCell(cellPosition, GetCellSourceId(cellPosition), new Vector2I(51, 13));
				_cellTimes[cellPosition] = TimeManager.Instance.Days;
			}
		}
		
		// Second growth phase
		else if (atlasCoords == new Vector2I(51, 13))  
		{
			if (TimeManager.Instance.Days > _cellTimes[cellPosition])
			{
				// Advance to phase 3
				SetCell(cellPosition, GetCellSourceId(cellPosition), new Vector2I(51, 15));
				_cellTimes[cellPosition] = TimeManager.Instance.Days;
			}
		}
		
		// Third growth phase
		else if (atlasCoords == new Vector2I(51, 15))  
		{
			if (TimeManager.Instance.Days > _cellTimes[cellPosition])
			{
				// Advance to final phase
				SetCell(cellPosition, GetCellSourceId(cellPosition), new Vector2I(51, 17));
				_cellTimes[cellPosition] = TimeManager.Instance.Days;
			}
		}
		
		// Final phase - do nothing
		else if (atlasCoords == new Vector2I(51, 17))  
		{
			// Fully grown, no changes needed
		}
	}
	
	public void PlantSeedAtCell(Vector2I cellPosition)
	{
		Vector2I currentCoords = GetCellAtlasCoords(cellPosition);
		
		// Only plant if cell is empty soil
		if (currentCoords == new Vector2I(63, 11))
		{
			SetCell(cellPosition, GetCellSourceId(cellPosition), new Vector2I(51, 12));
			_cellTimes[cellPosition] = TimeManager.Instance.Days;
		}
	}
}
