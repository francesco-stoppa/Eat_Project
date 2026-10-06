using UnityEngine;
using UnityEngine.SceneManagement;

public class TestPressed : MonoBehaviour
{
    public void OnPressed()
    {
        SceneManager.LoadScene(1);

    }
}
