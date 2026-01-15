/// <summary>
/// 統一的選單路徑常數
/// 避免選單路徑散亂、重複、不一致的問題
/// </summary>
public static class MenuPaths
{
    // 主選單根路徑
    public const string ROOT = "Dark Descent/";
    
    // ========== Quick Start (0-99) ==========
    public const string QUICK_START = ROOT + "🚀 Quick Start/";
    public const int PRIORITY_QUICK_START = 0;
    
    public const string QUICK_SETUP_ANIMATION = QUICK_START + "🎬 Setup Animation";
    public const string QUICK_CREATE_CHARACTER = QUICK_START + "🎭 Create Character";
    public const string QUICK_ADD_EFFECTS = QUICK_START + "✨ Add Effects";
    public const string QUICK_FIND_CHARACTER = QUICK_START + "🔍 Find Character";
    public const string QUICK_FIX_ALL = QUICK_START + "🔧 Fix All Issues";
    
    // ========== Scene Setup (100-199) ==========
    public const string SCENE_SETUP = ROOT + "🎨 Scene Setup/";
    public const int PRIORITY_SCENE_SETUP = 100;
    
    public const string SCENE_BACKGROUND = SCENE_SETUP + "Background/";
    public const string SCENE_BACKGROUND_CREATE = SCENE_BACKGROUND + "Create Background";
    public const string SCENE_BACKGROUND_COLOR = SCENE_BACKGROUND + "Set Color";
    public const string SCENE_BACKGROUND_CONTRAST = SCENE_BACKGROUND + "Adjust Contrast";
    
    public const string SCENE_COLOR = SCENE_SETUP + "Color Theme/";
    public const string SCENE_COLOR_PRESET = SCENE_COLOR + "Presets/";
    public const string SCENE_COLOR_HARMONY = SCENE_COLOR + "Color Harmony/";
    
    public const string SCENE_MATERIAL = SCENE_SETUP + "Material/";
    public const string SCENE_LIGHTING = SCENE_SETUP + "Lighting/";
    
    // ========== Animation (200-299) ==========
    public const string ANIMATION = ROOT + "🎬 Animation/";
    public const int PRIORITY_ANIMATION = 200;
    
    public const string ANIMATION_CREATE = ANIMATION + "Create/";
    public const string ANIMATION_CREATE_SIMPLE = ANIMATION_CREATE + "Simple Animation";
    public const string ANIMATION_CREATE_MULTI_FRAME = ANIMATION_CREATE + "Multi-Frame Animation";
    public const string ANIMATION_CREATE_SPRITE_SHEET = ANIMATION_CREATE + "From Sprite Sheet";
    
    public const string ANIMATION_CONTROL = ANIMATION + "Control/";
    public const string ANIMATION_CONTROL_PLAY = ANIMATION_CONTROL + "Play/Pause";
    public const string ANIMATION_CONTROL_STOP = ANIMATION_CONTROL + "Stop";
    public const string ANIMATION_CONTROL_SPEED = ANIMATION_CONTROL + "Speed Control";
    
    public const string ANIMATION_TEST = ANIMATION + "Test/";
    public const string ANIMATION_TEST_TWO_FRAME = ANIMATION_TEST + "Test 2-Frame Animation";
    public const string ANIMATION_TEST_THREE_FRAME = ANIMATION_TEST + "Test 3-Frame Animation";
    public const string ANIMATION_TEST_INFO = ANIMATION_TEST + "Show Animation Info";
    
    // ========== Effects (300-399) ==========
    public const string EFFECTS = ROOT + "✨ Effects/";
    public const int PRIORITY_EFFECTS = 300;
    
    public const string EFFECTS_PARTICLES = EFFECTS + "Particles/";
    public const string EFFECTS_PARTICLES_COLOR = EFFECTS_PARTICLES + "Color Presets/";
    public const string EFFECTS_PARTICLES_THEME = EFFECTS_PARTICLES + "Themes/";
    public const string EFFECTS_PARTICLES_EDITOR = EFFECTS_PARTICLES + "Color Editor";
    
    public const string EFFECTS_VISUAL = EFFECTS + "Visual/";
    public const string EFFECTS_VISUAL_FADE = EFFECTS_VISUAL + "Fade Effect";
    public const string EFFECTS_VISUAL_SHAKE = EFFECTS_VISUAL + "Shake Effect";
    public const string EFFECTS_VISUAL_TRAIL = EFFECTS_VISUAL + "Trail Effect";
    
    public const string EFFECTS_CAMERA = EFFECTS + "Camera/";
    public const string EFFECTS_CAMERA_SHAKE = EFFECTS_CAMERA + "Camera Shake";
    
    // ========== Tools (400-499) ==========
    public const string TOOLS = ROOT + "🔧 Tools/";
    public const int PRIORITY_TOOLS = 400;
    
    public const string TOOLS_SPRITE = TOOLS + "Sprite/";
    public const string TOOLS_SPRITE_SLICE = TOOLS_SPRITE + "Auto Slice";
    public const string TOOLS_SPRITE_INFO = TOOLS_SPRITE + "Show Info";
    public const string TOOLS_SPRITE_DIAGNOSTIC = TOOLS_SPRITE + "Diagnostic";
    
    public const string TOOLS_SCENE = TOOLS + "Scene/";
    public const string TOOLS_SCENE_VALIDATOR = TOOLS_SCENE + "Validate Scene";
    public const string TOOLS_SCENE_INSPECTOR = TOOLS_SCENE + "Scene Inspector";
    public const string TOOLS_SCENE_CLEANUP = TOOLS_SCENE + "Cleanup";
    
    public const string TOOLS_CODE = TOOLS + "Code Quality/";
    public const string TOOLS_CODE_CHECK = TOOLS_CODE + "Check Quality";
    public const string TOOLS_CODE_FORMAT = TOOLS_CODE + "Format Code";
    
    // ========== Debug (500-599) ==========
    public const string DEBUG = ROOT + "🐛 Debug/";
    public const int PRIORITY_DEBUG = 500;
    
    public const string DEBUG_INSPECTOR = DEBUG + "Inspector/";
    public const string DEBUG_INSPECTOR_LIST_ALL = DEBUG_INSPECTOR + "List All Objects";
    public const string DEBUG_INSPECTOR_FIND_PINK = DEBUG_INSPECTOR + "Find Pink Squares";
    public const string DEBUG_INSPECTOR_FIND_MISSING = DEBUG_INSPECTOR + "Find Missing Scripts";
    
    public const string DEBUG_COMPONENT = DEBUG + "Component Check/";
    public const string DEBUG_COMPONENT_ANIMATOR = DEBUG_COMPONENT + "Check Animator";
    public const string DEBUG_COMPONENT_RENDERER = DEBUG_COMPONENT + "Check Renderer";
    public const string DEBUG_COMPONENT_RIGIDBODY = DEBUG_COMPONENT + "Check Rigidbody";
    
    public const string DEBUG_FIX = DEBUG + "Quick Fix/";
    public const string DEBUG_FIX_PINK = DEBUG_FIX + "Fix Pink Squares";
    public const string DEBUG_FIX_MISSING = DEBUG_FIX + "Remove Missing Scripts";
    public const string DEBUG_FIX_SPRITE = DEBUG_FIX + "Fix Sprite References";
    
    // ========== Settings (600-699) ==========
    public const string SETTINGS = ROOT + "⚙️ Settings/";
    public const int PRIORITY_SETTINGS = 600;
    
    public const string SETTINGS_PROJECT = SETTINGS + "Project/";
    public const string SETTINGS_EDITOR = SETTINGS + "Editor/";
    public const string SETTINGS_PREFERENCES = SETTINGS + "Preferences/";
    
    // ========== GameObject Menu ==========
    public const string GAMEOBJECT_ROOT = "GameObject/Dark Descent/";
    public const string GAMEOBJECT_CREATE_CHARACTER = GAMEOBJECT_ROOT + "Create Character";
    public const string GAMEOBJECT_CREATE_EFFECT = GAMEOBJECT_ROOT + "Create Effect";
    
    // ========== 快捷鍵定義 ==========
    public const string HOTKEY_PLAY_ANIMATION = " %#p";      // Ctrl+Shift+P
    public const string HOTKEY_SETUP_VISUAL = " %#v";        // Ctrl+Shift+V
    public const string HOTKEY_SAVE_SCENE = " #&s";          // Shift+Alt+S
    public const string HOTKEY_QUICK_FIX = " %#f";          // Ctrl+Shift+F
}
