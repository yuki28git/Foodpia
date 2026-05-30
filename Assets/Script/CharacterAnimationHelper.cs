using UnityEngine;

public static class CharacterAnimationHelper
{
    public static void PlayIdle(GameObject modelInstance)
    {
        if (modelInstance == null) return;

        var animator = modelInstance.GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("Animator が見つかりません");
            return;
        }

        animator.Play("idle", 0, 0f);
    }
}