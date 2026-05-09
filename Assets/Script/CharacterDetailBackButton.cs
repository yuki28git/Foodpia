using UnityEngine;
using UnityEngine.SceneManagement;
public class CharacterDetailBackButton : MonoBehaviour
{
    public void OnClickBack()
    {
        // 非同期 Unload をリクエスト
        AsyncOperation op = SceneManager.UnloadSceneAsync("FieldGuideDetailScene");
        op.completed += (asyncOp) =>
        {
            // 完全にアンロードされた時だけCanvasを再表示
            if (CollectionRootController.Instance != null)
                CollectionRootController.Instance.ShowCanvas();

            // Debug
            Debug.Log("FieldGuideDetailScene Unloaded.");
        };
    }
}
