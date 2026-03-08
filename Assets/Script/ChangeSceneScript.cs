using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectionButtonScript : MonoBehaviour
{
    public void OpenCollection()
    {
        SceneManager.LoadScene("GachaResult");
    }
}