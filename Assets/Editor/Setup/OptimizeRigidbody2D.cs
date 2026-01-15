using UnityEngine;
using UnityEditor;

/// <summary>
/// 優化 Rigidbody2D 參數 - 讓自由落體更順暢
/// </summary>
public class OptimizeRigidbody2D : EditorWindow
{
    [MenuItem("DarkDescentDemo/修復工具/⚙️ 優化 Rigidbody2D 參數", false, 25)]
    public static void OptimizePhysics()
    {
        if (EditorApplication.isPlaying)
        {
            EditorUtility.DisplayDialog("錯誤", "❌ 請先停止播放", "確定");
            return;
        }

        GameObject fallingChar = GameObject.Find("FallingCharacter");
        if (fallingChar == null)
        {
            EditorUtility.DisplayDialog("錯誤", "❌ 找不到 FallingCharacter", "確定");
            return;
        }

        Rigidbody2D rb = fallingChar.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = fallingChar.AddComponent<Rigidbody2D>();
            Debug.Log("✅ 已添加 Rigidbody2D 組件");
        }

        // === 優化參數 ===
        SerializedObject so = new SerializedObject(rb);
        
        // Body Type: Dynamic（動態物理）
        so.FindProperty("m_BodyType").enumValueIndex = 0;
        
        // Material: None（無摩擦材質）
        so.FindProperty("m_Material").objectReferenceValue = null;
        
        // Simulated: true（啟用物理模擬）
        so.FindProperty("m_Simulated").boolValue = true;
        
        // Use Auto Mass: false（使用自訂質量）
        so.FindProperty("m_UseAutoMass").boolValue = false;
        
        // Mass: 1（質量 = 1）
        so.FindProperty("m_Mass").floatValue = 1f;
        
        // Linear Drag: 0（無線性阻力，讓腳本控制）
        so.FindProperty("m_LinearDrag").floatValue = 0f;
        
        // Angular Drag: 0（無旋轉阻力）
        so.FindProperty("m_AngularDrag").floatValue = 0f;
        
        // Gravity Scale: 0（關閉Unity內建重力，使用腳本自訂重力）
        so.FindProperty("m_GravityScale").floatValue = 0f;
        
        // Collision Detection: Continuous（連續碰撞偵測，避免穿透）
        so.FindProperty("m_CollisionDetection").enumValueIndex = 1;
        
        // Sleeping Mode: Never Sleep（永不休眠）
        so.FindProperty("m_SleepingMode").enumValueIndex = 2;
        
        // Interpolate: Interpolate（插值，讓移動更平滑）
        so.FindProperty("m_Interpolate").enumValueIndex = 1;
        
        // Constraints: Freeze Rotation（凍結旋轉，只允許直線下降）
        so.FindProperty("m_Constraints").intValue = 4; // FreezeRotation
        
        so.ApplyModifiedProperties();

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());

        string report = "=== Rigidbody2D 參數優化完成 ===\n\n";
        report += "✅ Body Type = Dynamic（動態）\n";
        report += "✅ Gravity Scale = 0（關閉內建重力）\n";
        report += "✅ Linear Drag = 0（無阻力）\n";
        report += "✅ Angular Drag = 0（無旋轉阻力）\n";
        report += "✅ Mass = 1（質量為1）\n";
        report += "✅ Collision Detection = Continuous（連續）\n";
        report += "✅ Interpolate = Interpolate（插值平滑）\n";
        report += "✅ Constraints = Freeze Rotation（凍結旋轉）\n";
        report += "✅ Sleeping Mode = Never Sleep（永不休眠）\n\n";
        report += "🎯 完美的自由落體設定！\n";
        report += "物理由 DarkDescentController 腳本完全控制\n";

        Debug.Log(report);
        EditorUtility.DisplayDialog("優化完成", report, "確定");

        Selection.activeGameObject = fallingChar;
    }

    [MenuItem("DarkDescentDemo/快速創建/📋 Rigidbody2D 手動調整指南", false, 26)]
    public static void ShowManualGuide()
    {
        string guide = @"=== Rigidbody2D 手動調整指南 ===

1️⃣ 選中 FallingCharacter
   在 Hierarchy 視窗點擊 FallingCharacter

2️⃣ 在 Inspector 視窗找到 Rigidbody 2D 組件

3️⃣ 手動設定以下參數：

📌 Body Type
   ▸ Dynamic（動態物理）

📌 Material
   ▸ None（無摩擦材質）

📌 Simulated
   ☑ 打勾（啟用物理模擬）

📌 Use Auto Mass
   ☐ 不勾（使用自訂質量）

📌 Mass（質量）
   ▸ 1

📌 Linear Drag（線性阻力）
   ▸ 0

📌 Angular Drag（旋轉阻力）
   ▸ 0

📌 Gravity Scale（重力縮放）
   ▸ 0（很重要！關閉Unity內建重力）

📌 Collision Detection
   ▸ Continuous（連續碰撞偵測）

📌 Sleeping Mode
   ▸ Never Sleep（永不休眠）

📌 Interpolate（插值）
   ▸ Interpolate（讓移動平滑）

📌 Constraints（約束）
   ☑ Freeze Rotation Z（凍結Z軸旋轉）
   讓角色只能直線下降，不會旋轉

============================

💡 為什麼這樣設定？

✓ Gravity Scale = 0
  因為 DarkDescentController 腳本
  會用自訂重力公式控制墜落

✓ Linear Drag = 0
  阻力由腳本的 airResistance 參數控制

✓ Interpolate = Interpolate
  讓每一幀之間的移動更平滑
  消除抖動感

✓ Freeze Rotation
  避免角色在墜落時旋轉
  保持直立姿態

============================

🎯 完成後按 Play 測試！";

        Debug.Log(guide);
        
        EditorUtility.DisplayDialog(
            "Rigidbody2D 手動調整指南", 
            "完整指南已輸出到 Console 視窗\n\n按 Console 標籤查看詳細步驟\n\n關鍵參數：\n• Gravity Scale = 0\n• Linear Drag = 0\n• Interpolate = Interpolate\n• Freeze Rotation Z = ✓", 
            "確定");
    }
}
