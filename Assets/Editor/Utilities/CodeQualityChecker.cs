using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

/// <summary>
/// 程式碼品質檢查器 - 避免AI生成程式碼的常見問題
/// 使用方式: DarkDescentDemo → 開發工具 → 🔍 檢查程式碼品質
/// </summary>
public class CodeQualityChecker : EditorWindow
{
    private Vector2 scrollPosition;
    private string lastCheckResult = "";
    private int warningCount = 0;
    private int errorCount = 0;
    
    [MenuItem("Dark Descent/🔧 Tools/Code Quality/Check Quality")]
    public static void ShowWindow()
    {
        var window = GetWindow<CodeQualityChecker>("程式碼品質檢查");
        window.minSize = new Vector2(600, 400);
        window.Show();
    }
    
    void OnGUI()
    {
        GUILayout.Space(10);
        
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.fontSize = 18;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label("🔍 程式碼品質檢查器", titleStyle);
        
        GUILayout.Space(10);
        
        EditorGUILayout.HelpBox(
            "檢查常見的AI生成程式碼問題：\n" +
            "• 靜態變數濫用\n" +
            "• 缺少null檢查\n" +
            "• Unity生命週期誤用\n" +
            "• 缺少錯誤處理",
            MessageType.Info
        );
        
        GUILayout.Space(20);
        
        if (GUILayout.Button("▶ 開始檢查 Assets/Scripts/", GUILayout.Height(40)))
        {
            CheckCodeQuality();
        }
        
        GUILayout.Space(10);
        
        // 顯示統計
        if (!string.IsNullOrEmpty(lastCheckResult))
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label($"錯誤: {errorCount}", EditorStyles.boldLabel);
            GUILayout.Label($"警告: {warningCount}", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();
            
            GUILayout.Space(10);
            
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            EditorGUILayout.TextArea(lastCheckResult, GUILayout.ExpandHeight(true));
            EditorGUILayout.EndScrollView();
        }
    }
    
    void CheckCodeQuality()
    {
        lastCheckResult = "";
        warningCount = 0;
        errorCount = 0;
        
        string scriptsPath = Path.Combine(Application.dataPath, "Scripts");
        
        if (!Directory.Exists(scriptsPath))
        {
            lastCheckResult = "❌ 找不到 Assets/Scripts/ 目錄";
            return;
        }
        
        var csFiles = Directory.GetFiles(scriptsPath, "*.cs", SearchOption.AllDirectories);
        
        lastCheckResult += $"========== 檢查 {csFiles.Length} 個檔案 ==========\n\n";
        
        foreach (var file in csFiles)
        {
            CheckFile(file);
        }
        
        lastCheckResult += "\n========== 檢查完成 ==========\n";
        lastCheckResult += $"總計: {errorCount} 個錯誤, {warningCount} 個警告\n";
        
        if (errorCount == 0 && warningCount == 0)
        {
            lastCheckResult += "\n✅ 太棒了！沒有發現問題！";
        }
        
        Repaint();
    }
    
    void CheckFile(string filePath)
    {
        string fileName = Path.GetFileName(filePath);
        string content = File.ReadAllText(filePath);
        string[] lines = content.Split('\n');
        
        bool hasIssues = false;
        string fileReport = $"\n📄 {fileName}\n";
        
        // 檢查1: 靜態變數（Unity中的大忌）
        var staticFieldMatches = Regex.Matches(content, @"private static (?!readonly).*=");
        if (staticFieldMatches.Count > 0)
        {
            hasIssues = true;
            warningCount++;
            fileReport += $"  ⚠️  發現 {staticFieldMatches.Count} 個可變靜態變數\n";
            fileReport += "     → 建議：改用Manager管理，或改為const/readonly\n";
        }
        
        // 檢查2: FindObjectOfType 沒有null檢查
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (line.Contains("FindObjectOfType") && 
                !line.Contains("if") && 
                !line.Contains("==") &&
                !line.Contains("?."))
            {
                // 檢查接下來幾行是否有null檢查
                bool hasNullCheck = false;
                for (int j = i + 1; j < Mathf.Min(i + 3, lines.Length); j++)
                {
                    if (lines[j].Contains("== null") || lines[j].Contains("!= null"))
                    {
                        hasNullCheck = true;
                        break;
                    }
                }
                
                if (!hasNullCheck)
                {
                    hasIssues = true;
                    errorCount++;
                    fileReport += $"  ❌ 第 {i + 1} 行: FindObjectOfType 缺少null檢查\n";
                    fileReport += $"     {line}\n";
                    fileReport += "     → 必須加入: if (obj == null) { ... }\n";
                }
            }
        }
        
        // 檢查3: GetComponent 沒有null檢查
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (line.Contains(".GetComponent<") && 
                !line.Contains("if") && 
                !line.Contains("?.") &&
                !line.Contains("?? "))
            {
                bool hasNullCheck = false;
                for (int j = i + 1; j < Mathf.Min(i + 3, lines.Length); j++)
                {
                    if (lines[j].Contains("== null") || lines[j].Contains("!= null"))
                    {
                        hasNullCheck = true;
                        break;
                    }
                }
                
                if (!hasNullCheck && !line.Contains("AddComponent"))
                {
                    hasIssues = true;
                    warningCount++;
                    fileReport += $"  ⚠️  第 {i + 1} 行: GetComponent 建議加入null檢查\n";
                }
            }
        }
        
        // 檢查4: Update/FixedUpdate中有Find操作（性能殺手）
        bool inUpdate = false;
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            
            if (line.Contains("void Update()") || line.Contains("void FixedUpdate()"))
            {
                inUpdate = true;
            }
            
            if (inUpdate && line.Contains("}") && !line.Contains("{"))
            {
                inUpdate = false;
            }
            
            if (inUpdate && (line.Contains("Find") || line.Contains("GetComponent")))
            {
                hasIssues = true;
                errorCount++;
                fileReport += $"  ❌ 第 {i + 1} 行: Update中使用Find/GetComponent（性能問題）\n";
                fileReport += "     → 建議：在Start/Awake中快取引用\n";
            }
        }
        
        // 檢查5: 直接訪問陣列/列表沒有範圍檢查
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (Regex.IsMatch(line, @"\[\d+\]") && 
                !line.Contains("//") &&
                !line.Contains("if") &&
                !line.Contains("Length") &&
                !line.Contains("Count"))
            {
                hasIssues = true;
                warningCount++;
                fileReport += $"  ⚠️  第 {i + 1} 行: 直接訪問陣列索引，建議加入範圍檢查\n";
            }
        }
        
        // 檢查6: 缺少Unity Message的拼寫錯誤檢測
        var uncommonMethods = new[] {
            ("OnEnable", "確認拼寫正確"),
            ("OnDisable", "確認拼寫正確"),
            ("OnDestroy", "確認拼寫正確"),
            ("OnValidate", "只在Editor中運行")
        };
        
        foreach (var (method, note) in uncommonMethods)
        {
            if (content.Contains($"void {method}"))
            {
                // 檢查是否有拼寫錯誤
                var wrongSpellings = new[] {
                    $"{method.ToLower()}",
                    $"on{method.Substring(2).ToLower()}"
                };
                
                foreach (var wrong in wrongSpellings)
                {
                    if (content.Contains($"void {wrong}"))
                    {
                        hasIssues = true;
                        errorCount++;
                        fileReport += $"  ❌ Unity Message拼寫錯誤: {wrong} → 應為 {method}\n";
                    }
                }
            }
        }
        
        if (hasIssues)
        {
            lastCheckResult += fileReport;
        }
    }
}
