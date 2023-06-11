using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace UnityToolbarExtender.Examples
{
    [InitializeOnLoad]
    public class TimeScaleBar
    {
        private static float timeScaleValue = 1;

        static TimeScaleBar()
        {
            ToolbarExtender.RightToolbarGUI.Add(OnToolbarGUI);
            EditorSceneManager.sceneClosed += OnSceneClosed;
        }

        static void OnToolbarGUI()
        {
            GUILayout.FlexibleSpace();

            GUIStyle labelStyle = new GUIStyle(EditorStyles.boldLabel);
            labelStyle.alignment = TextAnchor.MiddleCenter;
            labelStyle.fontSize = 12;

            GUILayout.Space(-5);

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Time", labelStyle, GUILayout.Height(16));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace(); 
            timeScaleValue = EditorGUILayout.Slider(timeScaleValue, 0, 10);
            GUILayout.Space(400);
            GUILayout.FlexibleSpace(); 
            GUILayout.EndHorizontal();

            GUILayout.FlexibleSpace();

            Time.timeScale = timeScaleValue;
        }

        static void OnSceneClosed(Scene scene)
        {
            timeScaleValue = 1f;
        }
    }
}
