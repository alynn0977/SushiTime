using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SushiTime.Utilities
{
    /// <summary>
    /// Finds SpriteRenderers using an unwanted material (e.g. the URP
    /// "Sprite-Unlit-Default" left over from the Unity 2022 -> Unity 6
    /// conversion) and swaps them to a replacement material.
    /// Scans all open scenes plus every prefab under Assets/.
    /// </summary>
    public class SpriteMaterialFixerWindow : EditorWindow
    {
        private string sourceMaterialName = "Sprite-Unlit-Default";
        private Material replacementMaterial;
        private Vector2 scrollPosition;
        private bool hasScanned;
        private readonly List<Entry> results = new List<Entry>();

        private class Entry
        {
            public SpriteRenderer Renderer;
            public string Location;
            public bool IsPrefabAsset;
            public bool Updated;
        }

        [MenuItem("Tools/SushiTime/Sprite Material Fixer")]
        public static void Open()
        {
            var window = GetWindow<SpriteMaterialFixerWindow>("Sprite Material Fixer");
            window.minSize = new Vector2(520f, 340f);
        }

        private void OnEnable()
        {
            if (replacementMaterial == null)
            {
                replacementMaterial = AssetDatabase.GetBuiltinExtraResource<Material>("Sprites-Default.mat");
            }
        }

        private void OnGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox(
                "Finds every SpriteRenderer whose material matches the name below, " +
                "in all open scenes and all prefabs under Assets/. " +
                "Note: the URP material is named \"Sprite-Unlit-Default\" (no \"s\").",
                MessageType.Info);

            sourceMaterialName = EditorGUILayout.TextField("Material to replace", sourceMaterialName);
            replacementMaterial = (Material)EditorGUILayout.ObjectField(
                "Replacement material", replacementMaterial, typeof(Material), false);

            EditorGUILayout.Space();
            if (GUILayout.Button("Scan Open Scenes + All Prefabs", GUILayout.Height(28f)))
            {
                Scan();
            }

            if (!hasScanned)
            {
                return;
            }

            EditorGUILayout.Space();
            int remaining = results.Count(e => !e.Updated && e.Renderer != null);
            EditorGUILayout.LabelField(
                $"Found: {results.Count}   Remaining: {remaining}", EditorStyles.boldLabel);

            if (results.Count == 0)
            {
                EditorGUILayout.HelpBox("No SpriteRenderers found using that material.", MessageType.None);
                return;
            }

            using (new EditorGUI.DisabledScope(replacementMaterial == null || remaining == 0))
            {
                if (GUILayout.Button($"Update All ({remaining})", GUILayout.Height(28f)))
                {
                    UpdateAll();
                }
            }

            if (replacementMaterial == null)
            {
                EditorGUILayout.HelpBox("Assign a replacement material to enable updating.", MessageType.Warning);
            }

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            foreach (Entry entry in results)
            {
                DrawRow(entry);
            }
            EditorGUILayout.EndScrollView();
        }

        private void DrawRow(Entry entry)
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
            {
                if (entry.Renderer == null)
                {
                    EditorGUILayout.LabelField("(object no longer exists — rescan)", EditorStyles.miniLabel);
                    return;
                }

                string kind = entry.IsPrefabAsset ? "[Prefab]" : "[Scene]";
                EditorGUILayout.LabelField($"{kind} {entry.Renderer.gameObject.name}", GUILayout.Width(220f));
                EditorGUILayout.LabelField(entry.Location, EditorStyles.miniLabel);

                if (GUILayout.Button("Select", GUILayout.Width(60f)))
                {
                    Selection.activeObject = entry.Renderer.gameObject;
                    EditorGUIUtility.PingObject(entry.Renderer.gameObject);
                }

                if (entry.Updated)
                {
                    EditorGUILayout.LabelField("✓ Updated", GUILayout.Width(70f));
                }
                else
                {
                    using (new EditorGUI.DisabledScope(replacementMaterial == null))
                    {
                        if (GUILayout.Button("Update", GUILayout.Width(70f)))
                        {
                            Apply(entry);
                            AssetDatabase.SaveAssets();
                        }
                    }
                }
            }
        }

        private void Scan()
        {
            results.Clear();
            hasScanned = true;

            // Open scenes.
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (!scene.isLoaded)
                {
                    continue;
                }

                foreach (GameObject root in scene.GetRootGameObjects())
                {
                    foreach (SpriteRenderer renderer in root.GetComponentsInChildren<SpriteRenderer>(true))
                    {
                        if (IsMatch(renderer.sharedMaterial))
                        {
                            results.Add(new Entry
                            {
                                Renderer = renderer,
                                Location = $"Scene: {scene.name} — {GetHierarchyPath(renderer.transform)}",
                                IsPrefabAsset = false,
                            });
                        }
                    }
                }
            }

            // All prefabs under Assets/.
            string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets" });
            try
            {
                for (int i = 0; i < guids.Length; i++)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                    EditorUtility.DisplayProgressBar("Scanning prefabs", path, (float)i / guids.Length);

                    var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                    if (prefab == null)
                    {
                        continue;
                    }

                    foreach (SpriteRenderer renderer in prefab.GetComponentsInChildren<SpriteRenderer>(true))
                    {
                        if (IsMatch(renderer.sharedMaterial))
                        {
                            results.Add(new Entry
                            {
                                Renderer = renderer,
                                Location = path,
                                IsPrefabAsset = true,
                            });
                        }
                    }
                }
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }

        private bool IsMatch(Material material)
        {
            return material != null
                && string.Equals(material.name.Trim(), sourceMaterialName.Trim(), StringComparison.OrdinalIgnoreCase);
        }

        private void UpdateAll()
        {
            foreach (Entry entry in results)
            {
                if (!entry.Updated && entry.Renderer != null)
                {
                    Apply(entry);
                }
            }

            AssetDatabase.SaveAssets();
        }

        private void Apply(Entry entry)
        {
            if (entry.Renderer == null || replacementMaterial == null)
            {
                return;
            }

            if (entry.IsPrefabAsset)
            {
                entry.Renderer.sharedMaterial = replacementMaterial;
                EditorUtility.SetDirty(entry.Renderer);
            }
            else
            {
                Undo.RecordObject(entry.Renderer, "Replace Sprite Material");
                entry.Renderer.sharedMaterial = replacementMaterial;
                EditorSceneManager.MarkSceneDirty(entry.Renderer.gameObject.scene);
            }

            entry.Updated = true;
        }

        private static string GetHierarchyPath(Transform transform)
        {
            string path = transform.name;
            while (transform.parent != null)
            {
                transform = transform.parent;
                path = transform.parent == null ? path : $"{transform.name}/{path}";
            }

            return path;
        }
    }
}
