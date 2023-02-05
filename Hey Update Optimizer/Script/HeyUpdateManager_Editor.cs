#if UNITY_EDITOR
using JahnStar.Optimization;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HeyUpdateManager))]
public class HeyUpdateManager_Editor : Editor
{
    HeyUpdateManager _target;
    private bool hideApplyButton = false;
    private void OnEnable()
    {
        _target = (HeyUpdateManager)target;
        if (_target.updatables == null) _target.Load();
    }

    public override void OnInspectorGUI()
    {
        GUI.backgroundColor = Color.black + Color.white * 0.25f;
        EditorGUILayout.HelpBox("\n Developed by Halil Emre Yildiz. Github: @JahnStar \n", MessageType.Info);
        Rect buttonRect = new Rect(4, 4, 46, GUILayoutUtility.GetLastRect().height);
        if (GUI.Button(buttonRect, "</>")) _target._editorInfo = !_target._editorInfo;
        GUI.backgroundColor = Color.black + Color.white * 0.75f;

        EditorGUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.Space();
        GUIStyle style = new GUIStyle(EditorStyles.boldLabel) { fontSize = 16, alignment = TextAnchor.MiddleCenter, normal = new GUIStyleState() { textColor = new Color(0.05f, 0.8f, 1f) } };
        GUILayout.Label("Hey Update Optimizer v1", style);
        EditorGUILayout.Space();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(10);
        GUI.color = new Color(0.05f, 0.8f, 1f);

        int _updatePerFrame = 1;
        float _processPerFrame = 1, _updatePoolingRatio = 1;
        bool _ignoreInterfaces = _target.ignoreInterfaces;
        if (_target.ignoreInterfaces = EditorGUILayout.Toggle("Ignore Interfaces", _ignoreInterfaces))
        {
            EditorGUILayout.LabelField("Update Frequency", EditorStyles.boldLabel);
            _updatePerFrame = EditorGUILayout.IntField("Update Per Frame", _target.updatePerFrame);
            _target.updatePerFrame = _updatePerFrame > 1 ? _updatePerFrame : 1;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Pooling Ratio", EditorStyles.boldLabel);
            _target.updatePoolingRatio = EditorGUILayout.Slider("Update Pooling Ratio", Mathf.Clamp(_target.updatePoolingRatio, 0, 0.999f), 0, 1);
            _updatePoolingRatio = 1f - _target.updatePoolingRatio;
            float _updatablesLength = _target._updatablesLength;
            _processPerFrame = (int)(_updatePoolingRatio * _updatablesLength);
        }

        EditorGUILayout.Space();
        float updateFrequency = (int)(_updatePerFrame / _updatePoolingRatio * 100f) / 100f;
        EditorGUILayout.LabelField("Skippes frames: " + (_ignoreInterfaces ? updateFrequency + "" :  "x"), EditorStyles.boldLabel);
        GUI.backgroundColor = Color.white;
        GUI.color = Color.white;
        EditorGUI.ProgressBar(new Rect(0, EditorGUILayout.GetControlRect().y + EditorGUIUtility.singleLineHeight * 0.5f, EditorGUILayout.GetControlRect().width + 22, 38),
            1f / updateFrequency, $"{(_processPerFrame > 1 ? _processPerFrame : 1)} process per {(_ignoreInterfaces ? _updatePerFrame + "" : "x")} frames");

        if (EditorApplication.isPlaying && _target.NeedApply())
        {
            if (GUI.Button(new Rect(2, EditorGUILayout.GetControlRect().y + EditorGUIUtility.singleLineHeight * 0.5f, EditorGUILayout.GetControlRect().width + 17, 36),
                !hideApplyButton ? "Apply Changes" : "Applying...")) _target.reload = hideApplyButton = true;
        }
        else if (hideApplyButton) hideApplyButton = false;

        if (_target._editorInfo)
        {
            EditorGUILayout.Space(14);
            GUI.backgroundColor = (Color.cyan + Color.blue) * Color.white * 0.5f; 
            EditorGUILayout.LabelField("Properties:", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("\n" +
                "* </>: Opens and closes help boxes.\n\n" +
                "* Ignore Interfaces: Allows modification of 'Update Per Frame' runtime by ignoring 'UpdatePerFrame' in IHeyUpdate Scripts. \n\n" +
                "* Update Per Frame: Determines how often the objects will be updated per frame.\n(Helps to optimize the update frequency) \n\n" +
                "* Update Pooling Ratio: Determines the maximum number of objects that will be updated per frame.\n(Helps to optimize the update load) \n\n" +
                "* Skippes frames: Shows how many updates will be saved. The larger this number, the less updates there will be. Also update frequency is represented in the progress bar.\n\n" +
                "* Apply Changes: Reloads with the new settings after existing in-game processes are finished. (It's no needed in the Editor Mode)\n\n" +
                "Note: Use 'Profiler' to analyze performance.\n", MessageType.Info);

            GUI.backgroundColor = Color.black;
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Scripting Reference:", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("\n" +
                "|   using UnityEngine;\n" +
                "|   using JahnStar.Optimization;\n\n" +
                "|   public class Example : MonoBehaviour, IHeyUpdate \n" +
                "|   {\n" +
                "|       public int UpdatePerFrame { get => 1; }\n" +
                "|       public void HeyUpdate(float deltaTime)\n" +
                "|       {\n" +
                "|           if (!gameObject.activeInHierarchy) return;\n" +
                "|           // Your update code here \n" +
                "|       }\n" +
                "|   }\n", MessageType.Info);
        }
        serializedObject.ApplyModifiedProperties();
    }
}
#endif