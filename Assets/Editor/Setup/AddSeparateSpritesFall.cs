using UnityEngine;
using UnityEditor;

/// <summary>
/// 添加分離精靈墜落效果的編輯器工具
/// 使用方法：選單 → DarkDescentDemo → 快速創建 → ✨ 添加分離精靈墜落效果
/// </summary>
public class AddSeparateSpritesFall : EditorWindow
{
    [MenuItem(MenuPaths.QUICK_ADD_EFFECTS, false, MenuPaths.PRIORITY_QUICK_START + 30)]
    public static void AddEffect()
    {
        // 檢查是否在 Play 模式
        if (EditorApplication.isPlaying)
        {
            EditorUtility.DisplayDialog("錯誤", 
                "❌ 無法在 Play 模式中執行\n請先停止播放", 
                "確定");
            return;
        }
        
        // 尋找 FallingCharacter
        GameObject fallingChar = GameObject.Find("FallingCharacter");
        if (fallingChar == null)
        {
            EditorUtility.DisplayDialog("錯誤", 
                "❌ 找不到 FallingCharacter 物件\n請先創建場景", 
                "確定");
            return;
        }
        
        // 檢查是否已經有此組件
        SeparateSpritesFall existingScript = fallingChar.GetComponent<SeparateSpritesFall>();
        if (existingScript != null)
        {
            bool overwrite = EditorUtility.DisplayDialog("確認", 
                "FallingCharacter 已有 SeparateSpritesFall 組件\n是否要重新設定？", 
                "是", "否");
            
            if (!overwrite)
            {
                return;
            }
            
            DestroyImmediate(existingScript);
        }
        
        // 添加組件
        SeparateSpritesFall script = fallingChar.AddComponent<SeparateSpritesFall>();
        
        // 載入精靈
        string spritePath = "Assets/Sprites/建立影像 同命蠱.png";
        var allAssets = AssetDatabase.LoadAllAssetsAtPath(spritePath);
        
        System.Collections.Generic.List<Sprite> spriteList = new System.Collections.Generic.List<Sprite>();
        
        if (allAssets != null)
        {
            foreach (var asset in allAssets)
            {
                if (asset is Sprite)
                {
                    spriteList.Add(asset as Sprite);
                }
            }
        }
        
        // 設定參數
        SerializedObject so = new SerializedObject(script);
        
        if (spriteList.Count > 0)
        {
            SerializedProperty spritesArray = so.FindProperty("spritesToSeparate");
            spritesArray.arraySize = spriteList.Count;
            
            for (int i = 0; i < spriteList.Count; i++)
            {
                spritesArray.GetArrayElementAtIndex(i).objectReferenceValue = spriteList[i];
            }
        }
        
        // 設定預設參數
        so.FindProperty("gravityScale").floatValue = 1.5f;
        so.FindProperty("horizontalForceRange").floatValue = 3f;
        so.FindProperty("rotationSpeedRange").floatValue = 180f;
        so.FindProperty("separatedScale").floatValue = 1.5f;
        so.FindProperty("fadeOutDuration").floatValue = 3f;
        so.FindProperty("separatedColor").colorValue = Color.white;

        so.FindProperty("triggerKey").intValue = (int)KeyCode.Space;
        so.FindProperty("autoTriggerAtHalfway").boolValue = false;
        
        so.ApplyModifiedProperties();
        
        // 標記場景已修改
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
        
        // 建立報告
        string report = "✅ 分離精靈墜落效果設定完成！\n\n";
        report += "📋 設定內容：\n";
        report += $"• 載入精靈數量：{spriteList.Count}\n";
        report += "• 重力倍數：1.5x\n";
        report += "• 水平散開範圍：±3 單位\n";
        report += "• 旋轉速度：±180°/秒\n";
        report += "• 縮放：1.5 倍\n";
        report += "• 淡出時間：3 秒\n";
        report += "• 鬼影拖尾：已啟用\n\n";
        report += "🎮 使用方法：\n";
        report += "1. 按下 Play 開始遊戲\n";
        report += "2. 在墜落過程中按 Space 鍵\n";
        report += "3. 精靈會分離並各自墜落\n\n";
        report += "⚙️ 進階設定：\n";
        report += "• 在 Inspector 中調整 SeparateSpritesFall 的參數\n";
        report += "• 可設定 autoTriggerAtHalfway = true 自動觸發\n";
        report += "• 可更改 triggerKey 來使用其他按鍵\n";
        
        Debug.Log(report);
        EditorUtility.DisplayDialog("設定完成", report, "確定");
        
        // 選中物件
        Selection.activeGameObject = fallingChar;
    }
    
    [MenuItem("DarkDescentDemo/快速創建/🔄 重置分離精靈效果", false, 26)]
    public static void ResetEffect()
    {
        if (EditorApplication.isPlaying)
        {
            EditorUtility.DisplayDialog("錯誤", 
                "❌ 無法在 Play 模式中執行\n請先停止播放", 
                "確定");
            return;
        }
        
        GameObject fallingChar = GameObject.Find("FallingCharacter");
        if (fallingChar == null)
        {
            EditorUtility.DisplayDialog("錯誤", 
                "❌ 找不到 FallingCharacter 物件", 
                "確定");
            return;
        }
        
        SeparateSpritesFall script = fallingChar.GetComponent<SeparateSpritesFall>();
        if (script == null)
        {
            EditorUtility.DisplayDialog("提示", 
                "FallingCharacter 沒有 SeparateSpritesFall 組件", 
                "確定");
            return;
        }
        
        bool confirm = EditorUtility.DisplayDialog("確認", 
            "是否要移除 SeparateSpritesFall 組件？", 
            "是", "否");
        
        if (confirm)
        {
            DestroyImmediate(script);
            
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
                UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
            
            Debug.Log("✅ 已移除 SeparateSpritesFall 組件");
            EditorUtility.DisplayDialog("完成", 
                "已移除分離精靈墜落效果", 
                "確定");
        }
    }
}
