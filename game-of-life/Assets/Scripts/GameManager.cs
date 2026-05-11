using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GridManager Grid;
    // public List<Button> buttonList;
    // public Button InfoButton, PauseButton, StartButton, InfiniteButton;
    // public ButtonManager Buttons;
    // public GameObject InfoPanel;
    // public InputField NumberOfIterationsField;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start() {
        Grid = FindFirstObjectByType<GridManager>(); // Ensure Grid is assigned
        if (Grid == null) {
            Debug.LogError("GridManager not found!");
            return;
        }
        Grid.Initiate();
        //Buttons.Initiate();
        //buttonList = new List<Button>;
    }

    void Update() {
        ButtonsUpdate();

        // if (InfoButton != null && InfoButton.gameObject.activeSelf) {
        //     InfoPanel.SetActive(true);
        // }

        HandleMouseInput();

        // if (PauseButton != null && PauseButton.gameObject.activeSelf) {
        //     return;
        // } else if (StartButton != null && InfiniteButton.gameObject.activeSelf) {
        //     Grid.Update();
        // } else {
        //     int iterations = int.Parse(NumberOfIterationsField.text);
        //     while (iterations > 0) {
        //         iterations--;
        //         Grid.Update();
        //     }
        // }
    }

    private void ButtonsUpdate() {
        // foreach (Button button in buttonList) {

        // }
    }

    private void HandleMouseInput() {
        if (Mouse.current.rightButton.isPressed){
            ActivateCellsAtMousePosition();
        }
    }

    private void ActivateCellsAtMousePosition()
    {
        Vector3 mouseScreenPosition = Mouse.current.position.ReadValue(); // Get mouse position in screen coordinates
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, Camera.main.nearClipPlane)); // Convert to world position
        
        GridManager.Instance.ActivateCellsAtMousePosition(mouseWorldPosition);
    }
}