using UnityEngine;
using UnityEditor;

/// <summary>
/// 清理工具 - 移除組件
/// </summary>
public class CleanupTool : EditorWindow
{
    [MenuItem("設定/清理 FallingCharacter")]
    static void ShowWindow()
    {
        var window = GetWindow<CleanupTool>("清理工具");
        window.minSize = new Vector2(400, 200);
        window.Show();
    }
    
    void OnGUI()
    {
        GUILayout.Space(20);
        
        GUILayout.Label("清理 FallingCharacter 組件", EditorStyles.boldLabel);
        
        GUILayout.Space(20);
        
        if (GUILayout.Button("移除 Sprite Renderer", GUILayout.Height(40)))
        {
            RemoveSpriteRenderer();
        }
        
        GUILayout.Space(10);
        
        if (GUILayout.Button("移除所有粒子系統", GUILayout.Height(40)))
        {
            RemoveParticleSystems();
        }
        
        GUILayout.Space(10);
        
        if (GUILayout.Button("完全清理（保留腳本）", GUILayout.Height(40)))
        {
            FullCleanup();
        }
    }
    
    void RemoveSpriteRenderer()
    {
        GameObject fallingChar = GameObject.Find("FallingCharacter");
        
        if (fallingChar == null)
        {
            EditorUtility.DisplayDialog("找不到", "場景中沒有 FallingCharacter", "確定");
            return;
        }
        
        SpriteRenderer sr = fallingChar.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            DestroyImmediate(sr);
            EditorUtility.DisplayDialog("完成", "已移除 Sprite Renderer", "確定");
            Debug.Log("✓ 已移除 Sprite Renderer");
        }
        else
        {
            EditorUtility.DisplayDialog("提示", "沒有找到 Sprite Renderer", "確定");
        }
    }
    
    void RemoveParticleSystems()
    {
        GameObject fallingChar = GameObject.Find("FallingCharacter");
        
        if (fallingChar == null)
        {
            EditorUtility.DisplayDialog("找不到", "場景中沒有 FallingCharacter", "確定");
            return;
        }
        
        ParticleSystem[] particles = fallingChar.GetComponentsInChildren<ParticleSystem>();
        
        foreach (var ps in particles)
        {
            DestroyImmediate(ps.gameObject);
        }
        
        EditorUtility.DisplayDialog("完成", $"已移除 {particles.Length} 個粒子系統", "確定");
        Debug.Log($"✓ 已移除 {particles.Length} 個粒子系統");
    }
    
    void FullCleanup()
    {
        GameObject fallingChar = GameObject.Find("FallingCharacter");
        
        if (fallingChar == null)
        {
            EditorUtility.DisplayDialog("找不到", "場景中沒有 FallingCharacter", "確定");
            return;
        }
        
        // 移除 SpriteRenderer
        SpriteRenderer sr = fallingChar.GetComponent<SpriteRenderer>();
        if (sr != null) DestroyImmediate(sr);
        
        // 移除所有粒子系統
        ParticleSystem[] particles = fallingChar.GetComponentsInChildren<ParticleSystem>();
        foreach (var ps in particles)
        {
            DestroyImmediate(ps.gameObject);
        }
        
        EditorUtility.DisplayDialog("完成", 
            "已清理完成\n\n" +
            "✓ 移除 Sprite Renderer\n" +
            $"✓ 移除 {particles.Length} 個粒子系統\n\n" +
            "保留了腳本組件", "確定");
        
        Debug.Log("✓ 完全清理完成");
    }
}
