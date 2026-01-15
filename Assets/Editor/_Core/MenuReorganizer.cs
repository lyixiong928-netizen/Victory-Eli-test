using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 自動化選單重組工具
/// 批量掃描並更新所有 MenuItem 屬性
/// </summary>
public class MenuReorganizer : EditorWindow
{
    private Vector2 scrollPos;
    private List<MenuItemInfo> menuItems = new List<MenuItemInfo>();
    private bool isScanned = false;
    private int updatedCount = 0;
    
    [MenuItem("DD Setup/⚙️ Settings/Menu Reorganizer", false, 600)]
    public static void ShowWindow()
    {
        var window = GetWindow<MenuReorganizer>("選單重組工具");
        window.minSize = new Vector2(600, 400);
        window.Show();
    }
    
    void OnGUI()
    {
        EditorGUILayout.LabelField("🔧 選單自動重組工具", EditorStyles.boldLabel);
        EditorGUILayout.Space();
        
        EditorGUILayout.HelpBox(
            "此工具會自動掃描所有 Editor 腳本中的 MenuItem，\n" +
            "並建議新的選單路徑。您可以批量套用變更。", 
            MessageType.Info);
        
        EditorGUILayout.Space();
        
        if (GUILayout.Button("🔍 掃描所有選單項目", GUILayout.Height(30)))
        {
            ScanAllMenuItems();
        }
        
        if (!isScanned)
        {
            EditorGUILayout.HelpBox("點擊上方按鈕開始掃描", MessageType.None);
            return;
        }
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField($"找到 {menuItems.Count} 個選單項目", EditorStyles.boldLabel);
        
        if (GUILayout.Button("✅ 套用所有建議變更", GUILayout.Height(30)))
        {
            ApplyAllChanges();
        }
        
        EditorGUILayout.Space();
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        
        foreach (var item in menuItems)
        {
            DrawMenuItem(item);
        }
        
        EditorGUILayout.EndScrollView();
        
        if (updatedCount > 0)
        {
            EditorGUILayout.HelpBox($"✅ 已更新 {updatedCount} 個檔案", MessageType.Info);
        }
    }
    
    void DrawMenuItem(MenuItemInfo item)
    {
        EditorGUILayout.BeginVertical("box");
        
        EditorGUILayout.LabelField("檔案：" + Path.GetFileName(item.FilePath), EditorStyles.boldLabel);
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("舊路徑：", GUILayout.Width(60));
        EditorGUILayout.SelectableLabel(item.OldPath, GUILayout.Height(18));
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("新路徑：", GUILayout.Width(60));
        GUI.color = Color.green;
        EditorGUILayout.SelectableLabel(item.NewPath, GUILayout.Height(18));
        GUI.color = Color.white;
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space(5);
    }
    
    void ScanAllMenuItems()
    {
        menuItems.Clear();
        updatedCount = 0;
        
        string editorPath = Path.Combine(Application.dataPath, "Editor");
        if (!Directory.Exists(editorPath))
        {
            EditorUtility.DisplayDialog("錯誤", "找不到 Editor 資料夾", "確定");
            return;
        }
        
        var csFiles = Directory.GetFiles(editorPath, "*.cs", SearchOption.AllDirectories);
        
        foreach (var file in csFiles)
        {
            ScanFile(file);
        }
        
        isScanned = true;
        Debug.Log($"✅ 掃描完成，找到 {menuItems.Count} 個選單項目");
    }
    
    void ScanFile(string filePath)
    {
        string content = File.ReadAllText(filePath);
        
        // 匹配 MenuItem 屬性 - 完整保留所有參數
        var pattern = @"\[MenuItem\s*\(\s*""([^""]+)""([^\]]*)\)\]";
        var matches = Regex.Matches(content, pattern);
        
        foreach (Match match in matches)
        {
            string oldPath = match.Groups[1].Value;
            string parameters = match.Groups[2].Value; // 保留所有其他參數
            
            // 只處理 DarkDescentDemo 開頭的
            if (!oldPath.StartsWith("DarkDescentDemo/") && !oldPath.StartsWith("GameObject/DarkDescent"))
                continue;
            
            string newPath = ConvertToNewPath(oldPath);
            
            if (newPath != oldPath)
            {
                menuItems.Add(new MenuItemInfo
                {
                    FilePath = filePath,
                    OldPath = oldPath,
                    NewPath = newPath,
                    Parameters = parameters,
                    FullMatch = match.Value
                });
            }
        }
    }
    
    string ConvertToNewPath(string oldPath)
    {
        // 對應表
        var mappings = new Dictionary<string, string>
        {
            // Quick Start
            {"DarkDescentDemo/🎬 一鍵動畫設定", MenuPaths.QUICK_SETUP_ANIMATION},
            {"DarkDescentDemo/快速創建/🎭 創建 FallingCharacter", MenuPaths.QUICK_CREATE_CHARACTER},
            {"DarkDescentDemo/快速創建/🔍 尋找 FallingCharacter", MenuPaths.QUICK_FIND_CHARACTER},
            {"DarkDescentDemo/快速創建/✨ 添加分離精靈墜落效果", MenuPaths.QUICK_ADD_EFFECTS},
            {"DarkDescentDemo/🔧 一鍵完整修復（解決桃色方塊）", MenuPaths.QUICK_FIX_ALL},
            
            // Animation
            {"DarkDescentDemo/動畫測試/🎬 建立雙幀動畫測試 (1秒間隔)", MenuPaths.ANIMATION_TEST_TWO_FRAME},
            {"DarkDescentDemo/動畫測試/🚀 建立三幀動畫測試 (0.5秒間隔)", MenuPaths.ANIMATION_TEST_THREE_FRAME},
            {"DarkDescentDemo/動畫測試/📊 顯示當前動畫資訊", MenuPaths.ANIMATION_TEST_INFO},
            {"DarkDescentDemo/動畫控制/為選中物件設定並播放動畫 %#p", MenuPaths.ANIMATION_CONTROL_PLAY + MenuPaths.HOTKEY_PLAY_ANIMATION},
            
            // Tools - Sprite
            {"DarkDescentDemo/修復工具/✂️ 自動切片精靈圖片", MenuPaths.TOOLS_SPRITE_SLICE},
            {"DarkDescentDemo/修復工具/📊 顯示精靈圖片資訊", MenuPaths.TOOLS_SPRITE_INFO},
            {"DarkDescentDemo/診斷工具/🔍 精靈切片診斷器", MenuPaths.TOOLS_SPRITE_DIAGNOSTIC},
            
            // Debug
            {"DarkDescentDemo/除錯工具/分析粉紅色方形", MenuPaths.DEBUG_INSPECTOR_FIND_PINK},
            {"DarkDescentDemo/除錯工具/為粉紅色物件設定精靈", MenuPaths.DEBUG_FIX_SPRITE},
            {"DarkDescentDemo/除錯工具/查找遺失的腳本", MenuPaths.DEBUG_INSPECTOR_FIND_MISSING},
            {"DarkDescentDemo/除錯工具/移除所有遺失的腳本", MenuPaths.DEBUG_FIX_MISSING},
            {"DarkDescentDemo/除錯工具/列出場景中所有物件", MenuPaths.DEBUG_INSPECTOR_LIST_ALL},
            {"DarkDescentDemo/快速修復/刪除所有粉紅色方形", MenuPaths.DEBUG_FIX_PINK},
            
            // Effects - Particles
            {"DarkDescentDemo/粒子系統/🎨 粒子顏色編輯器", MenuPaths.EFFECTS_PARTICLES_EDITOR},
            
            // Tools - Code
            {"DarkDescentDemo/開發工具/🔍 檢查程式碼品質", MenuPaths.TOOLS_CODE_CHECK},
            
            // GameObject
            {"GameObject/DarkDescent/創建 FallingCharacter", MenuPaths.GAMEOBJECT_CREATE_CHARACTER},
        };
        
        // 精確匹配
        if (mappings.ContainsKey(oldPath))
        {
            return mappings[oldPath];
        }
        
        // 模糊匹配 - 根據關鍵字判斷
        if (oldPath.Contains("粒子系統/顏色設定/"))
            return oldPath.Replace("DarkDescentDemo/粒子系統/顏色設定/", MenuPaths.EFFECTS_PARTICLES_COLOR);
        
        if (oldPath.Contains("粒子系統/主題設定/"))
            return oldPath.Replace("DarkDescentDemo/粒子系統/主題設定/", MenuPaths.EFFECTS_PARTICLES_THEME);
        
        if (oldPath.Contains("視覺效果/背景顏色/"))
            return oldPath.Replace("DarkDescentDemo/視覺效果/背景顏色/", MenuPaths.SCENE_BACKGROUND_COLOR);
        
        if (oldPath.Contains("背景設定/"))
            return oldPath.Replace("DarkDescentDemo/背景設定/", MenuPaths.SCENE_BACKGROUND);
        
        if (oldPath.Contains("顏色設定/"))
            return oldPath.Replace("DarkDescentDemo/顏色設定/", MenuPaths.SCENE_COLOR_PRESET);
        
        if (oldPath.Contains("顏色協調/"))
            return oldPath.Replace("DarkDescentDemo/顏色協調/", MenuPaths.SCENE_COLOR_HARMONY);
        
        // 預設：保持原樣
        return oldPath;
    }
    
    void ApplyAllChanges()
    {
        if (menuItems.Count == 0)
        {
            EditorUtility.DisplayDialog("提示", "沒有需要更新的項目", "確定");
            return;
        }
        
        bool confirm = EditorUtility.DisplayDialog(
            "確認", 
            $"即將更新 {menuItems.Count} 個選單項目\n\n建議先備份或提交當前變更！", 
            "確定更新", "取消");
        
        if (!confirm) return;
        
        updatedCount = 0;
        var fileGroups = menuItems.GroupBy(m => m.FilePath);
        
        foreach (var group in fileGroups)
        {
            UpdateFile(group.Key, group.ToList());
        }
        
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("完成", $"✅ 已更新 {updatedCount} 個檔案", "確定");
    }
    
    void UpdateFile(string filePath, List<MenuItemInfo> items)
    {
        string content = File.ReadAllText(filePath);
        bool changed = false;
        
        foreach (var item in items)
        {
            // 保留所有原始參數（validate, priority 等）
            string newMenuItem = $"[MenuItem(\"{item.NewPath}\"{item.Parameters})]";
            
            if (content.Contains(item.FullMatch))
            {
                content = content.Replace(item.FullMatch, newMenuItem);
                changed = true;
            }
        }
        
        if (changed)
        {
            File.WriteAllText(filePath, content);
            updatedCount++;
            Debug.Log($"✅ 已更新：{Path.GetFileName(filePath)}");
        }
    }
    
    class MenuItemInfo
    {
        public string FilePath;
        public string OldPath;
        public string NewPath;
        public string Parameters;  // 保留所有參數：validate, priority 等
        public string FullMatch;
    }
}
