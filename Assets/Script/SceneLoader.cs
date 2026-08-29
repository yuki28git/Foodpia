using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadLobbyScene()
    {
        SceneManager.LoadScene("LobbyScene");
    }
    public void LoadCollectionScene()
    {
        SceneManager.LoadScene("CollectionScene");
    }
    public void LoadGachaScene()
    {
        SceneManager.LoadScene("GachaScene");
    }
    public void LoadGacharesult()
    {
        SceneManager.LoadScene("GachaResult");
    }
    public void LoadGacharesult10()
    {
        SceneManager.LoadScene("GachaResult10");
    }
    public void LoadNewScene()
    {
        SceneManager.LoadScene("NewScene");
    }
}
