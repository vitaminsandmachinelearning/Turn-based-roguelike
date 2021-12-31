using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : OdinMenuEditorWindow
{
    [OnValueChanged("StateChange")]
    [LabelText("Manager View")]
    [LabelWidth(100f)]
    [EnumToggleButtons]
    [ShowInInspector]
    private ManagerState managerState;
    private int enumIndex = 0;
    private bool treeRebuild;

    private DrawSelected<BaseSpell> drawSpells = new DrawSelected<BaseSpell>();
    private DrawSelected<BaseUnit> drawUnits = new DrawSelected<BaseUnit>();

    private string spellPath = "Assets/Prefabs/Spells";
    private string unitPath = "Assets/Prefabs/Units";

    public enum ManagerState
    { 
        Player,
        Units,
        Items,
        Spells,
        Rooms
    }

    [MenuItem("Tools/Game Manager")]
    public static void OpenWindow()
    {
        GetWindow<GameManager>().Show();
    }

    private void StateChange()
    {
        treeRebuild = true;
    }

    protected override void Initialize()
    {
        drawSpells.SetPath(spellPath);
        drawUnits.SetPath(unitPath);
        base.Initialize();
    }

    protected override void OnGUI()
    {
        if (treeRebuild && Event.current.type == EventType.Layout)
        {
            ForceMenuTreeRebuild();
            treeRebuild = false;
        }

        SirenixEditorGUI.Title("Game Manager", "", TextAlignment.Center, true);

        switch (managerState)
        {
            case ManagerState.Player:
            case ManagerState.Units:
            case ManagerState.Items:
            case ManagerState.Spells:
            case ManagerState.Rooms:
                DrawEditor(enumIndex);
                break;
            default:
                break;
        }
        EditorGUILayout.Space();
        base.OnGUI();
    }

    protected override void DrawEditors()
    {
        switch (managerState)
        {
            case ManagerState.Player:
                break;
            case ManagerState.Units:
                drawUnits.SetSelected(this.MenuTree.Selection.SelectedValue);
                break;
            case ManagerState.Items:
                break;
            case ManagerState.Spells:
                drawSpells.SetSelected(this.MenuTree.Selection.SelectedValue);
                break;
            case ManagerState.Rooms:
                break;
        }
        DrawEditor((int)managerState);
    }

    protected override IEnumerable<object> GetTargets()
    {
        List<object> targets = new List<object>();

        targets.Add(null);
        targets.Add(drawUnits);
        targets.Add(null);
        targets.Add(drawSpells);
        targets.Add(null);

        targets.Add(base.GetTarget());

        enumIndex = targets.Count - 1;

        return targets;
    }

    protected override void DrawMenu()
    {
        switch (managerState)
        {
            case ManagerState.Player:
                break;
            case ManagerState.Units:
            case ManagerState.Items:
            case ManagerState.Spells:
            case ManagerState.Rooms:
                base.DrawMenu();
                break;
            default:
                break;
        }
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        OdinMenuTree tree = new OdinMenuTree();

        switch (managerState)
        {
            case ManagerState.Player:
                break;
            case ManagerState.Units:
                tree.AddAllAssetsAtPath("Units", unitPath, typeof(BaseUnit));
                break;
            case ManagerState.Items:
                break;
            case ManagerState.Spells:
                tree.AddAllAssetsAtPath("Spells", spellPath, typeof(BaseSpell));
                break;
            case ManagerState.Rooms:
                break;
        }

        return tree;
    }
}

public class DrawSelected<T> where T : ScriptableObject
{
    [InlineEditor(InlineEditorObjectFieldModes.CompletelyHidden)]
    public T selected;

    [LabelWidth(100f)]
    [PropertyOrder(-1)]
    [BoxGroup("CreateNew")]
    [HorizontalGroup("CreateNew/Horizontal")]
    public string nameForNew;

    private string path;

    [HorizontalGroup("CreateNew/Horizontal")]
    [GUIColor(0.7f,0.7f,1f)]
    [Button]
    public void CreateNew()
    {
        if (nameForNew == "")
            return;

        T newItem = ScriptableObject.CreateInstance<T>();
        newItem.name = "New " + typeof(T).ToString();

        if (path == "")
            path = "Assets/";

        AssetDatabase.CreateAsset(newItem, path + "\\" + nameForNew + ".asset");
        AssetDatabase.SaveAssets();

        nameForNew = "";
    }
    [HorizontalGroup("CreateNew/Horizontal")]
    [GUIColor(1f,0.7f,0.7f)]
    [Button]
    public void DeleteSelected()
    {
        if (selected != null)
        {
            string _path = AssetDatabase.GetAssetPath(selected);
            AssetDatabase.DeleteAsset(_path);
            AssetDatabase.SaveAssets();
        }
    }

    public void SetSelected(object item)
    {
        var attempt = item as T;
        if (attempt != null)
            this.selected = attempt;
    }

    public void SetPath(string path)
    {
        this.path = path;
    }
}