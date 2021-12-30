using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : OdinMenuEditorWindow
{
    [LabelText("Manager View")]
    [LabelWidth(100f)]
    [EnumToggleButtons]
    [ShowInInspector]
    private ManagerState managerState;
    private int enumIndex = 0;

    public enum ManagerState
    { 
        Player,
        Enemies,
        Items,
        Spells,
        Rooms
    }

    [MenuItem("Tools/Game Manager")]
    public static void OpenWindow()
    {
        GetWindow<GameManager>().Show();
    }

    protected override void OnGUI()
    {
        SirenixEditorGUI.Title("Game Manager", "", TextAlignment.Center, true);
        EditorGUILayout.Space();

        switch (managerState)
        {
            case ManagerState.Player:
            case ManagerState.Enemies:
            case ManagerState.Items:
            case ManagerState.Spells:
            case ManagerState.Rooms:
                break;
            default:
                break;
        }
        base.OnGUI();
    }

    protected override void DrawEditors()
    {
        switch (managerState)
        {
            case ManagerState.Player:
                break;
            case ManagerState.Enemies:
                break;
            case ManagerState.Items:
                break;
            case ManagerState.Spells:
                break;
            case ManagerState.Rooms:
                break;
        }
        base.DrawEditors();
    }

    protected override IEnumerable<object> GetTargets()
    {
        List<object> targets = new List<object>();
        targets.Add(base.GetTarget());

        enumIndex = targets.Count - 1;

        return targets;
    }

    protected override void DrawMenu()
    {
        switch (managerState)
        {
            case ManagerState.Player:
            case ManagerState.Enemies:
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

        return tree;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
