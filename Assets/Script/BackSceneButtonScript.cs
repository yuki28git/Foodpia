using UnityEngine;
using UnityEngine.SceneManagement;

public class BackSceneButtonScript : MonoBehaviour
{
    public void OpenCollection()
    {
        SceneManager.LoadScene("GachaScene");
    }
}