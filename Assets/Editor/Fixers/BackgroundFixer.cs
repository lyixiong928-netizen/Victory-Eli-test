using UnityEngine;
using UnityEditor;

/// <summary>
/// Background 物件診斷與修復工具
/// 檢查並自動修復 DarkDescentDemo 場景中的 Background 物件
/// </summary>
public class BackgroundFixer : EditorWindow
{
    private Vector2 scrollPosition;
    private string diagnosticReport = "";
    
    [MenuItem("DD Debug/🔧 Background 診斷與修復")]
    public static void ShowWindow()
    {
        var window = GetWindow<BackgroundFixer>("Background 修復工具");
        window.minSize = new Vector2(600, 500);
        window.Show();
    }
    
    void OnGUI()
    {
        GUILayout.Space(10);
        
        // 標題
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.fontSize = 18;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.alignment = TextAnchor.MiddleCenter;
        
        GUILayout.Label("🔧 Background 物件診斷與修復", titleStyle);
        GUILayout.Space(20);
        
        // 按鈕區
        GUILayout.BeginHorizontal();
        
        GUI.backgroundColor = new Color(0.3f, 0.7f, 1f);
        if (GUILayout.Button("🔍 診斷 Background", GUILayout.Height(40)))
        {
            DiagnoseBackground();
        }
        
        GUI.backgroundColor = new Color(0.4f, 0.8f, 0.4f);
        if (GUILayout.Button("🔧 自動修復", GUILayout.Height(40)))
        {
            AutoFixBackground();
        }
        
        GUI.backgroundColor = new Color(1f, 0.6f, 0.3f);
        if (GUILayout.Button("✨ 完整重建", GUILayout.Height(40)))
        {
            RebuildBackground();
        }
        
        GUI.backgroundColor = Color.white;
        GUILayout.EndHorizontal();
        
        GUILayout.Space(20);
        
        // 診斷報告區
        GUILayout.Label("診斷報告：", EditorStyles.boldLabel);
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.ExpandHeight(true));
        GUILayout.TextArea(diagnosticReport, GUILayout.ExpandHeight(true));
        GUILayout.EndScrollView();
    }
    
    /// <summary>
    /// 診斷 Background 物件
    /// </summary>
    void DiagnoseBackground()
    {
        diagnosticReport = "========== Background 診斷報告 ==========\n\n";
        
        GameObject background = GameObject.Find("Background");
        GameObject clickableBackground = GameObject.Find("ClickableBackground");
        
        if (background == null && clickableBackground == null)
        {
            diagnosticReport += "❌ 錯誤：場景中找不到任何 Background 物件\n";
            diagnosticReport += "   建議：點擊「完整重建」按鈕創建新的背景\n\n";
            return;
        }
        
        GameObject target = background ?? clickableBackground;
        diagnosticReport += $"✅ 找到背景物件: {target.name}\n\n";
        
        // 檢查 SpriteRenderer
        SpriteRenderer sr = target.GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            diagnosticReport += "❌ 缺少 SpriteRenderer 元件\n";
            diagnosticReport += "   影響：背景無法顯示\n";
        }
        else
        {
            diagnosticReport += "✅ SpriteRenderer 元件存在\n";
            diagnosticReport += $"   - 顏色: {sr.color}\n";
            diagnosticReport += $"   - Sorting Order: {sr.sortingOrder}\n";
            diagnosticReport += $"   - Sprite: {(sr.sprite ? sr.sprite.name : "❌ 無")}\n";
            
            if (sr.sortingOrder >= 0)
            {
                diagnosticReport += "   ⚠️ 警告：Sorting Order 應為負數（例如 -100）以確保在最底層\n";
            }
        }
        
        diagnosticReport += "\n";
        
        // 檢查 Collider
        BoxCollider2D collider = target.GetComponent<BoxCollider2D>();
        if (collider == null)
        {
            diagnosticReport += "❌ 缺少 BoxCollider2D 元件\n";
            diagnosticReport += "   影響：無法檢測滑鼠點擊\n";
        }
        else
        {
            diagnosticReport += "✅ BoxCollider2D 元件存在\n";
            diagnosticReport += $"   - Size: {collider.size}\n";
        }
        
        diagnosticReport += "\n";
        
        // 檢查腳本
        ClickableBackground clickScript = target.GetComponent<ClickableBackground>();
        BackgroundManager bgManager = target.GetComponent<BackgroundManager>();
        
        if (clickScript == null && bgManager == null)
        {
            diagnosticReport += "❌ 缺少背景控制腳本\n";
            diagnosticReport += "   影響：背景沒有互動功能\n";
            diagnosticReport += "   建議：添加 ClickableBackground 或 BackgroundManager\n";
        }
        else
        {
            if (clickScript)
            {
                diagnosticReport += "✅ ClickableBackground 腳本已附加\n";
                diagnosticReport += $"   - 顏色數量: {clickScript.colors.Length}\n";
                diagnosticReport += $"   - 過渡速度: {clickScript.transitionSpeed}\n";
            }
            if (bgManager)
            {
                diagnosticReport += "✅ BackgroundManager 腳本已附加\n";
                diagnosticReport += $"   - 透明度: {bgManager.backgroundAlpha}\n";
            }
        }
        
        diagnosticReport += "\n";
        
        // 檢查位置與大小
        diagnosticReport += "📐 Transform 資訊：\n";
        diagnosticReport += $"   - Position: {target.transform.position}\n";
        diagnosticReport += $"   - Scale: {target.transform.localScale}\n";
        diagnosticReport += $"   - Rotation: {target.transform.rotation.eulerAngles}\n";
        
        // 檢查是否覆蓋畫面
        Camera mainCamera = Camera.main;
        if (mainCamera)
        {
            float screenHeight = mainCamera.orthographicSize * 2;
            float screenWidth = screenHeight * mainCamera.aspect;
            Vector3 scale = target.transform.localScale;
            
            diagnosticReport += $"\n📺 畫面覆蓋檢查：\n";
            diagnosticReport += $"   - 螢幕尺寸: {screenWidth:F2} x {screenHeight:F2}\n";
            diagnosticReport += $"   - 背景尺寸: {scale.x:F2} x {scale.y:F2}\n";
            
            if (scale.x < screenWidth * 0.9f || scale.y < screenHeight * 0.9f)
            {
                diagnosticReport += "   ⚠️ 警告：背景可能無法完全覆蓋畫面\n";
            }
            else
            {
                diagnosticReport += "   ✅ 背景尺寸足夠覆蓋畫面\n";
            }
        }
        
        diagnosticReport += "\n========== 診斷完成 ==========\n";
    }
    
    /// <summary>
    /// 自動修復 Background
    /// </summary>
    void AutoFixBackground()
    {
        diagnosticReport = "========== 開始自動修復 ==========\n\n";
        
        GameObject background = GameObject.Find("Background");
        GameObject clickableBackground = GameObject.Find("ClickableBackground");
        GameObject target = background ?? clickableBackground;
        
        if (target == null)
        {
            diagnosticReport += "❌ 找不到背景物件，請使用「完整重建」\n";
            return;
        }
        
        diagnosticReport += $"🔧 修復物件: {target.name}\n\n";
        
        // 確保有 SpriteRenderer
        SpriteRenderer sr = target.GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            sr = target.AddComponent<SpriteRenderer>();
            diagnosticReport += "✅ 已添加 SpriteRenderer\n";
        }
        
        // 創建基本精靈（如果沒有）
        if (sr.sprite == null)
        {
            Texture2D tex = new Texture2D(1, 1);
            tex.SetPixel(0, 0, Color.white);
            tex.Apply();
            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1);
            sr.sprite = sprite;
            diagnosticReport += "✅ 已創建基本精靈\n";
        }
        
        // 修正顏色（如果是粉紅色）
        if (Mathf.Approximately(sr.color.r, 1f) && Mathf.Approximately(sr.color.g, 0f) && Mathf.Approximately(sr.color.b, 1f))
        {
            sr.color = Color.black;
            diagnosticReport += "✅ 已修正粉紅色為黑色\n";
        }
        
        // 修正 Sorting Order
        if (sr.sortingOrder >= 0)
        {
            sr.sortingOrder = -100;
            diagnosticReport += "✅ 已設定 Sorting Order 為 -100\n";
        }
        
        // 確保有 Collider
        BoxCollider2D collider = target.GetComponent<BoxCollider2D>();
        if (collider == null)
        {
            collider = target.AddComponent<BoxCollider2D>();
            diagnosticReport += "✅ 已添加 BoxCollider2D\n";
        }
        
        // 確保有控制腳本
        if (target.GetComponent<ClickableBackground>() == null)
        {
            target.AddComponent<ClickableBackground>();
            diagnosticReport += "✅ 已添加 ClickableBackground 腳本\n";
        }
        
        // 調整尺寸以覆蓋畫面
        Camera mainCamera = Camera.main;
        if (mainCamera)
        {
            float height = mainCamera.orthographicSize * 2;
            float width = height * mainCamera.aspect;
            target.transform.localScale = new Vector3(width * 1.1f, height * 1.1f, 1);
            diagnosticReport += $"✅ 已調整尺寸: {width * 1.1f:F2} x {height * 1.1f:F2}\n";
        }
        
        // 重置位置
        target.transform.position = new Vector3(0, 0, 10);
        diagnosticReport += "✅ 已重置位置為中心 (0, 0, 10)\n";
        
        EditorUtility.SetDirty(target);
        diagnosticReport += "\n========== 修復完成 ==========\n";
        diagnosticReport += "💡 提示：按下 Play 測試點擊背景切換顏色功能\n";
        
        Debug.Log("✅ Background 自動修復完成！");
    }
    
    /// <summary>
    /// 完整重建 Background
    /// </summary>
    void RebuildBackground()
    {
        diagnosticReport = "========== 開始重建 Background ==========\n\n";
        
        // 刪除舊的背景物件
        GameObject oldBackground = GameObject.Find("Background");
        if (oldBackground) 
        {
            DestroyImmediate(oldBackground);
            diagnosticReport += "🗑️ 已刪除舊的 Background\n";
        }
        
        GameObject oldClickable = GameObject.Find("ClickableBackground");
        if (oldClickable) 
        {
            DestroyImmediate(oldClickable);
            diagnosticReport += "🗑️ 已刪除舊的 ClickableBackground\n";
        }
        
        diagnosticReport += "\n";
        
        // 創建新背景
        GameObject background = new GameObject("Create_Background");
        diagnosticReport += "✨ 已創建新的 Create_Background 物件\n";
        
        // 添加 SpriteRenderer
        SpriteRenderer sr = background.AddComponent<SpriteRenderer>();
        Texture2D tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, Color.white);
        tex.Apply();
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1);
        sr.sprite = sprite;
        sr.color = Color.black;
        sr.sortingOrder = -100;
        diagnosticReport += "✅ 已設定 SpriteRenderer (黑色, Order: -100)\n";
        
        // 設定大小
        Camera mainCamera = Camera.main;
        if (mainCamera)
        {
            float height = mainCamera.orthographicSize * 2;
            float width = height * mainCamera.aspect;
            background.transform.localScale = new Vector3(width * 1.1f, height * 1.1f, 1);
            diagnosticReport += $"✅ 已設定尺寸: {width * 1.1f:F2} x {height * 1.1f:F2}\n";
        }
        else
        {
            background.transform.localScale = new Vector3(25, 20, 1);
            diagnosticReport += "✅ 已設定預設尺寸: 25 x 20\n";
        }
        
        // 設定位置
        background.transform.position = new Vector3(0, 0, 10);
        diagnosticReport += "✅ 已設定位置: (0, 0, 10)\n";
        
        // 添加 Collider
        BoxCollider2D collider = background.AddComponent<BoxCollider2D>();
        diagnosticReport += "✅ 已添加 BoxCollider2D\n";
        
        // 添加互動腳本
        ClickableBackground clickScript = background.AddComponent<ClickableBackground>();
        diagnosticReport += "✅ 已添加 ClickableBackground 腳本\n";
        
        // 設定到場景根目錄
        GameObject demoRoot = GameObject.Find("DarkDescentDemo");
        if (demoRoot)
        {
            background.transform.SetParent(demoRoot.transform);
            diagnosticReport += "✅ 已設定為 DarkDescentDemo 的子物件\n";
        }
        
        Selection.activeGameObject = background;
        EditorGUIUtility.PingObject(background);
        
        diagnosticReport += "\n========== 重建完成 ==========\n";
        diagnosticReport += "✨ 新的 Background 已創建並選中\n";
        diagnosticReport += "💡 提示：\n";
        diagnosticReport += "   1. 在 Inspector 中可調整顏色列表\n";
        diagnosticReport += "   2. 按下 Play 測試點擊切換功能\n";
        diagnosticReport += "   3. 可以添加 BackgroundManager 以顯示圖片背景\n";
        
        Debug.Log("✨ Background 重建完成！");
    }
}
