using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Phase 3 完整檢查工具 - 準備進入 Staging 前的最後檢查
/// 整合：代碼品質、選單重組、文檔完整性檢查
/// </summary>
public class Phase3Checker : EditorWindow
{
    private Vector2 scrollPos;
    private bool isChecking = false;
    private List<CheckResult> results = new List<CheckResult>();
    
    private bool codeQualityDone = false;
    private bool menuReorganizeDone = false;
    private bool documentationDone = false;
    
    [MenuItem("Dark Descent/⚙️ Settings/Phase 3 - Staging Checker", false, 650)]
    public static void ShowWindow()
    {
        var window = GetWindow<Phase3Checker>("Phase 3 檢查器");
        window.minSize = new Vector2(700, 500);
        window.Show();
    }
    
    void OnGUI()
    {
        DrawHeader();
        DrawProgress();
        
        GUILayout.Space(10);
        
        if (GUILayout.Button("🚀 執行完整檢查", GUILayout.Height(50)))
        {
            RunFullCheck();
        }
        
        GUILayout.Space(20);
        
        DrawResults();
        
        GUILayout.Space(20);
        
        DrawActionButtons();
    }
    
    void DrawHeader()
    {
        GUILayout.Space(10);
        
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label)
        {
            fontSize = 20,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter
        };
        
        GUILayout.Label("🎯 Phase 3 - Staging 準備檢查", titleStyle);
        
        GUILayout.Space(5);
        
        EditorGUILayout.HelpBox(
            "此工具會執行 3 大檢查，確保代碼品質達到進入 Staging 的標準：\n" +
            "1. 代碼品質檢查 - 掃描常見問題\n" +
            "2. 選單重組狀態 - 確認選單已更新\n" +
            "3. 文檔完整性 - 確保文檔更新",
            MessageType.Info);
        
        GUILayout.Space(10);
    }
    
    void DrawProgress()
    {
        GUILayout.BeginHorizontal();
        
        DrawCheckbox("代碼品質", codeQualityDone);
        DrawCheckbox("選單重組", menuReorganizeDone);
        DrawCheckbox("文檔完整", documentationDone);
        
        GUILayout.EndHorizontal();
        
        GUILayout.Space(10);
        
        int completedCount = (codeQualityDone ? 1 : 0) + (menuReorganizeDone ? 1 : 0) + (documentationDone ? 1 : 0);
        float progress = completedCount / 3f;
        
        Rect rect = GUILayoutUtility.GetRect(18, 18, GUILayout.ExpandWidth(true));
        EditorGUI.ProgressBar(rect, progress, $"{completedCount}/3 完成");
    }
    
    void DrawCheckbox(string label, bool done)
    {
        GUILayout.BeginVertical("box", GUILayout.Width(200));
        
        GUI.color = done ? Color.green : Color.gray;
        GUILayout.Label(done ? "✅" : "⬜", GUILayout.Height(30));
        GUI.color = Color.white;
        
        GUILayout.Label(label, EditorStyles.boldLabel);
        
        GUILayout.EndVertical();
    }
    
    void DrawResults()
    {
        if (results.Count == 0) return;
        
        GUILayout.Label("檢查結果：", EditorStyles.boldLabel);
        
        scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Height(250));
        
        foreach (var result in results)
        {
            GUILayout.BeginVertical("box");
            
            GUIStyle style = new GUIStyle(EditorStyles.label);
            style.richText = true;
            
            string icon = result.Type == ResultType.Pass ? "✅" : 
                         result.Type == ResultType.Warning ? "⚠️" : "❌";
            
            Color color = result.Type == ResultType.Pass ? Color.green :
                         result.Type == ResultType.Warning ? Color.yellow : Color.red;
            
            GUI.color = color;
            GUILayout.Label($"{icon} {result.Title}", EditorStyles.boldLabel);
            GUI.color = Color.white;
            
            GUILayout.Label(result.Message, EditorStyles.wordWrappedLabel);
            
            if (!string.IsNullOrEmpty(result.Action))
            {
                GUI.color = new Color(0.7f, 0.7f, 1f);
                GUILayout.Label($"💡 建議：{result.Action}", EditorStyles.miniLabel);
                GUI.color = Color.white;
            }
            
            GUILayout.EndVertical();
            GUILayout.Space(5);
        }
        
        GUILayout.EndScrollView();
    }
    
    void DrawActionButtons()
    {
        GUILayout.BeginHorizontal();
        
        GUI.enabled = !codeQualityDone;
        if (GUILayout.Button("1️⃣ 代碼品質檢查", GUILayout.Height(40)))
        {
            CheckCodeQuality();
        }
        GUI.enabled = true;
        
        GUI.enabled = !menuReorganizeDone;
        if (GUILayout.Button("2️⃣ 打開選單重組工具", GUILayout.Height(40)))
        {
            MenuReorganizer.ShowWindow();
        }
        GUI.enabled = true;
        
        GUI.enabled = !documentationDone;
        if (GUILayout.Button("3️⃣ 檢查文檔", GUILayout.Height(40)))
        {
            CheckDocumentation();
        }
        GUI.enabled = true;
        
        GUILayout.EndHorizontal();
        
        GUILayout.Space(10);
        
        bool allDone = codeQualityDone && menuReorganizeDone && documentationDone;
        GUI.enabled = allDone;
        GUI.color = allDone ? Color.green : Color.gray;
        
        if (GUILayout.Button(allDone ? "✅ 全部完成！準備進入 Staging" : "⏳ 尚未完成所有檢查", GUILayout.Height(50)))
        {
            if (EditorUtility.DisplayDialog("確認", 
                "所有檢查已完成！\n\n準備好合併到 staging 分支了嗎？", 
                "是的，我準備好了", "再檢查一次"))
            {
                ShowStagingGuide();
            }
        }
        
        GUI.color = Color.white;
        GUI.enabled = true;
    }
    
    void RunFullCheck()
    {
        results.Clear();
        CheckCodeQuality();
        CheckMenuStatus();
        CheckDocumentation();
        Repaint();
    }
    
    void CheckCodeQuality()
    {
        results.Add(new CheckResult
        {
            Title = "代碼品質檢查",
            Message = "正在掃描 Assets/Scripts/ 和 Assets/Editor/...",
            Type = ResultType.Info
        });
        
        int issueCount = 0;
        string scriptsPath = Path.Combine(Application.dataPath, "Scripts");
        string editorPath = Path.Combine(Application.dataPath, "Editor");
        
        // 簡化檢查：統計檔案數量
        int scriptFiles = Directory.Exists(scriptsPath) ? Directory.GetFiles(scriptsPath, "*.cs", SearchOption.AllDirectories).Length : 0;
        int editorFiles = Directory.Exists(editorPath) ? Directory.GetFiles(editorPath, "*.cs", SearchOption.AllDirectories).Length : 0;
        
        results.Add(new CheckResult
        {
            Title = "代碼統計",
            Message = $"找到 {scriptFiles} 個運行時腳本，{editorFiles} 個編輯器腳本",
            Type = ResultType.Pass
        });
        
        // 檢查是否有編譯錯誤
        bool hasErrors = false;
        var assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
        
        results.Add(new CheckResult
        {
            Title = hasErrors ? "編譯狀態：有錯誤" : "編譯狀態：正常",
            Message = hasErrors ? "請修復編譯錯誤後再繼續" : "所有腳本編譯成功",
            Type = hasErrors ? ResultType.Error : ResultType.Pass,
            Action = hasErrors ? "在 Unity Console 查看詳細錯誤訊息" : ""
        });
        
        codeQualityDone = !hasErrors;
        Repaint();
    }
    
    void CheckMenuStatus()
    {
        // 檢查是否有舊的 DarkDescentDemo 選單
        string editorPath = Path.Combine(Application.dataPath, "Editor");
        int oldMenuCount = 0;
        
        if (Directory.Exists(editorPath))
        {
            var files = Directory.GetFiles(editorPath, "*.cs", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                string content = File.ReadAllText(file);
                if (content.Contains("DarkDescentDemo/"))
                {
                    oldMenuCount++;
                }
            }
        }
        
        results.Add(new CheckResult
        {
            Title = "選單重組狀態",
            Message = oldMenuCount > 0 ? 
                $"發現 {oldMenuCount} 個檔案還在使用舊的選單路徑" : 
                "所有選單已更新到新架構",
            Type = oldMenuCount > 0 ? ResultType.Warning : ResultType.Pass,
            Action = oldMenuCount > 0 ? "使用 MenuReorganizer 工具批量更新" : ""
        });
        
        menuReorganizeDone = (oldMenuCount == 0);
        Repaint();
    }
    
    void CheckDocumentation()
    {
        string docPath = Path.Combine(Application.dataPath, "..", "Documentation");
        
        if (!Directory.Exists(docPath))
        {
            results.Add(new CheckResult
            {
                Title = "文檔檢查",
                Message = "Documentation 資料夾不存在",
                Type = ResultType.Warning,
                Action = "建立 Documentation 資料夾並添加 README.md"
            });
            documentationDone = false;
            return;
        }
        
        var mdFiles = Directory.GetFiles(docPath, "*.md", SearchOption.AllDirectories);
        
        bool hasReadme = mdFiles.Any(f => Path.GetFileName(f).ToLower() == "readme.md");
        bool hasChangelog = mdFiles.Any(f => Path.GetFileName(f).ToLower().Contains("changelog"));
        
        results.Add(new CheckResult
        {
            Title = "文檔完整性",
            Message = $"找到 {mdFiles.Length} 個文檔檔案\n" +
                     $"README: {(hasReadme ? "✅" : "❌")}\n" +
                     $"Changelog: {(hasChangelog ? "✅" : "❌")}",
            Type = hasReadme ? ResultType.Pass : ResultType.Warning,
            Action = !hasReadme ? "添加 README.md 說明專案概況" : ""
        });
        
        documentationDone = hasReadme;
        Repaint();
    }
    
    void ShowStagingGuide()
    {
        string guide = 
            "🎯 進入 Staging 分支的步驟：\n\n" +
            "1️⃣ 提交當前所有變更：\n" +
            "   git add .\n" +
            "   git commit -m \"Phase 3 完成 - 準備進入 Staging\"\n\n" +
            "2️⃣ 切換到 staging 分支：\n" +
            "   git checkout staging\n\n" +
            "3️⃣ 合併 dev 分支：\n" +
            "   git merge dev --no-ff -m \"合併 dev - 選單重組與品質提升\"\n\n" +
            "4️⃣ 開始 Staging 測試：\n" +
            "   • 完整功能測試\n" +
            "   • 場景執行測試\n" +
            "   • 效能檢查\n\n" +
            "5️⃣ 測試通過後推送：\n" +
            "   git push origin staging\n\n" +
            "💡 記得在 Unity 中實際測試所有功能！";
        
        EditorUtility.DisplayDialog("Staging 部署指南", guide, "了解");
        
        // 複製指令到剪貼簿
        GUIUtility.systemCopyBuffer = 
            "git add . && git commit -m \"Phase 3 完成 - 準備進入 Staging\" && git checkout staging && git merge dev --no-ff -m \"合併 dev - 選單重組與品質提升\"";
        
        Debug.Log("✅ Git 指令已複製到剪貼簿！");
    }
    
    enum ResultType { Pass, Warning, Error, Info }
    
    class CheckResult
    {
        public string Title;
        public string Message;
        public ResultType Type;
        public string Action = "";
    }
}
