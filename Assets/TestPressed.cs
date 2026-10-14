using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestPressed : MonoBehaviour
{
    SaveSystem saveSystem;

    int currentWorld = 1;

    [SerializeField] private TextMeshProUGUI t_worldSelection;
    void Start()
    {
        // Singelton
        saveSystem = SaveSystem.Instance;
        currentWorld = saveSystem.GetWorld();
        
        UpdateText();
    }
    public void Select()
    {
        saveSystem.SetLevel(1);
        
        if (currentWorld == 4) // << block the selection
            return;
        
        switch(currentWorld)
        {
            case 0:
                SceneManager.LoadScene(1);
                break;
            case 1:
                SceneManager.LoadScene(11);
                break;
            case 2:
                SceneManager.LoadScene(21);
                break;
            case 3:
                SceneManager.LoadScene(31);
                break;
            case 4:
                SceneManager.LoadScene(41);
                break;
        }
    }
    
    public void MoveLeft()
    {
        if (currentWorld > 0)
            ChangeWorld(false);
    }
    public void MoveRight()
    {
        if (currentWorld < saveSystem.GetWorld())
            ChangeWorld(true);
    }
    
    void ChangeWorld(bool isIncreasing)
    {
        int mod = 0;
        
        if(isIncreasing)
            mod = +1;
        else
            mod = -1;

        currentWorld += mod;
        UpdateText();
    }

    void UpdateText()
    {
        if (t_worldSelection == null) return;

        t_worldSelection.text = "World ";

        t_worldSelection.text += currentWorld + 1;
    }
}

