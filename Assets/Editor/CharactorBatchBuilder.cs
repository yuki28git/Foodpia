using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.IO;
using System.Linq;

public static class CharactorBatchBuilder
{
    [MenuItem("Tools/Generate/Charactor FBX->Mat->Prefab(+Animator)")]
    public static void BuildAll()
    {
        string srcDir = "Assets/Charactor";
        string animDir = "Assets/Animation";
        string outMatDir = "Assets/Charactor/Generated/Materials";
        string outPrefabDir = "Assets/Resources/Charactors/3D";

        Directory.CreateDirectory(animDir);
        Directory.CreateDirectory(outMatDir);
        Directory.CreateDirectory(outPrefabDir);

        var fbxGuids = AssetDatabase.FindAssets("t:Model", new[] { srcDir });
        int ok = 0, ng = 0;

        foreach (var guid in fbxGuids)
        {
            string fbxPath = AssetDatabase.GUIDToAssetPath(guid);
            string baseName = Path.GetFileNameWithoutExtension(fbxPath);

            var fbx = AssetDatabase.LoadAssetAtPath<GameObject>(fbxPath);
            if (fbx == null) { ng++; continue; }

            // Texture
            string texPath = FindTexturePath(srcDir, baseName);
            Texture2D tex = null;
            if (!string.IsNullOrEmpty(texPath))
            {
                ConfigureTextureImportSettings(texPath);
                tex = AssetDatabase.LoadAssetAtPath<Texture2D>(texPath);
            }

            // Material
            string matPath = $"{outMatDir}/{baseName}.mat";
            var mat = AssetDatabase.LoadAssetAtPath<Material>(matPath);
            if (mat == null)
            {
                mat = new Material(Shader.Find("Standard"));
                AssetDatabase.CreateAsset(mat, matPath);
            }
            if (tex != null)
            {
                mat.mainTexture = tex;
                if (mat.HasProperty("_MainTex")) mat.SetTexture("_MainTex", tex);
            }
            SetStandardCutout(mat);
            EditorUtility.SetDirty(mat);
            AssetDatabase.SaveAssets();

            // ★Importerの表示モードは触らず、Remapだけ設定
            RemapOnlyKeepOriginalImporterState(fbxPath, mat, texPath);

            // Animator Controller
            string controllerPath = $"{animDir}/{baseName}_Animation.controller";
            var controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(controllerPath);
            if (controller == null)
            {
                controller = AnimatorController.CreateAnimatorControllerAtPath(controllerPath);
                TryAddDefaultIdleState(controller, fbxPath);
                EditorUtility.SetDirty(controller);
                Debug.Log($"[CREATE] {controllerPath}");
            }

            // Prefab
            fbx = AssetDatabase.LoadAssetAtPath<GameObject>(fbxPath);
            if (fbx == null) { ng++; continue; }

            var inst = (GameObject)PrefabUtility.InstantiatePrefab(fbx);

            // 見た目保証としてPrefab側にも適用
            foreach (var r in inst.GetComponentsInChildren<Renderer>(true))
            {
                var mats = r.sharedMaterials;
                for (int i = 0; i < mats.Length; i++) mats[i] = mat;
                r.sharedMaterials = mats;
            }

            var animator = inst.GetComponent<Animator>();
            if (animator == null) animator = inst.AddComponent<Animator>();
            animator.runtimeAnimatorController = controller;

            inst.transform.rotation = Quaternion.Euler(0f, -139f, 0f);

            string prefabPath = $"{outPrefabDir}/{baseName}.prefab";
            PrefabUtility.SaveAsPrefabAsset(inst, prefabPath);
            Object.DestroyImmediate(inst);

            ok++;
            Debug.Log($"[OK] {baseName} -> {prefabPath}");
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"完了: OK={ok}, SKIP={ng}");
    }

    static void RemapOnlyKeepOriginalImporterState(string fbxPath, Material targetMat, string texPath)
    {
        var importer = AssetImporter.GetAtPath(fbxPath) as ModelImporter;
        if (importer == null) return;

        // 触らない：materialImportMode / materialLocation / naming / search など
        // Remapped Materials だけを追加

        bool mapped = false;

        // 1) FBX内Material名に対してRemap
        var subAssets = AssetDatabase.LoadAllAssetsAtPath(fbxPath);
        foreach (var a in subAssets)
        {
            if (a is Material srcMat)
            {
                var id = new AssetImporter.SourceAssetIdentifier(typeof(Material), srcMat.name);
                importer.AddRemap(id, targetMat);
                mapped = true;
            }
        }

        // 2) 画像の例のように "No3_texture.png" 名で来るケースにも対応
        if (!mapped && !string.IsNullOrEmpty(texPath))
        {
            string texFileName = Path.GetFileName(texPath); // 例: No3_texture.png
            var idByTextureFile = new AssetImporter.SourceAssetIdentifier(typeof(Material), texFileName);
            importer.AddRemap(idByTextureFile, targetMat);
            mapped = true;
        }

        // 3) さらに保険で拡張子なし
        if (!mapped && !string.IsNullOrEmpty(texPath))
        {
            string texNoExt = Path.GetFileNameWithoutExtension(texPath); // No3_texture
            var idByTextureName = new AssetImporter.SourceAssetIdentifier(typeof(Material), texNoExt);
            importer.AddRemap(idByTextureName, targetMat);
        }

        importer.SaveAndReimport();
    }

    static string FindTexturePath(string srcDir, string baseName)
    {
        var prefer = AssetDatabase.FindAssets($"{baseName}_texture t:Texture2D", new[] { srcDir });
        if (prefer.Length > 0) return AssetDatabase.GUIDToAssetPath(prefer[0]);

        var fallback = AssetDatabase.FindAssets($"{baseName} t:Texture2D", new[] { srcDir });
        if (fallback.Length > 0) return AssetDatabase.GUIDToAssetPath(fallback[0]);

        return null;
    }

    static void ConfigureTextureImportSettings(string texPath)
    {
        var importer = AssetImporter.GetAtPath(texPath) as TextureImporter;
        if (importer == null) return;

        bool changed = false;

        if (!importer.alphaIsTransparency) { importer.alphaIsTransparency = true; changed = true; }
        if (importer.filterMode != FilterMode.Point) { importer.filterMode = FilterMode.Point; changed = true; }
        if (importer.wrapMode != TextureWrapMode.Clamp) { importer.wrapMode = TextureWrapMode.Clamp; changed = true; }
        if (importer.textureCompression != TextureImporterCompression.Uncompressed) { importer.textureCompression = TextureImporterCompression.Uncompressed; changed = true; }
        if (importer.mipmapEnabled) { importer.mipmapEnabled = false; changed = true; }

        if (changed) importer.SaveAndReimport();
    }

    static void TryAddDefaultIdleState(AnimatorController controller, string fbxPath)
    {
        var clips = AssetDatabase.LoadAllAssetsAtPath(fbxPath)
            .OfType<AnimationClip>()
            .Where(c => !string.IsNullOrEmpty(c.name) && !c.name.StartsWith("__preview__"))
            .ToList();

        if (clips.Count == 0) return;

        var idle = clips.FirstOrDefault(c => c.name.ToLower().Contains("idle")) ?? clips[0];

        var sm = controller.layers[0].stateMachine;
        var state = sm.AddState(idle.name);
        state.motion = idle;
        sm.defaultState = state;
    }

    static void SetStandardCutout(Material mat)
    {
        mat.SetFloat("_Mode", 1);
        mat.SetOverrideTag("RenderType", "TransparentCutout");
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        mat.SetInt("_ZWrite", 1);
        mat.EnableKeyword("_ALPHATEST_ON");
        mat.DisableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.AlphaTest;
    }
}