using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetCommand : MonoBehaviour
{
    InputOnGrid input;

    void Start()
    {
        input = InputOnGrid.Instance;
        if (input == null)
        {
            Debug.LogError("The [input manager] is missing");
            return;
        }
        input.onCommands += ReStart;
    }

    void ReStart(E_Commands command)
    {
        if (command != E_Commands.ResetLevel) return;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDisable()
    {
        if (input == null) return;
        input.onCommands -= ReStart;
    }
}
