using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;

namespace cumOS.Scriptables.Editor
{
    [CustomEditor(typeof(PopupDatabase))]
    public class PopupDatabaseEditor : UnityEditor.Editor
    {
        private const string searchPath = "Popups";
        private PopupDatabase _database;

        private SerializedProperty imageProp;
        private SerializedProperty videoProp;
        private SerializedProperty gifProp;

        private void OnEnable()
        {
            _database = serializedObject.targetObject as PopupDatabase;

            imageProp = serializedObject.FindProperty("images");
            videoProp = serializedObject.FindProperty("clips");
            gifProp = serializedObject.FindProperty("cumGif");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Load all popup assets"))
            {
                LoadAllAssets();
            }

            serializedObject.ApplyModifiedProperties();
        }

        void LoadAllAssets()
        {
            List<Sprite> sprites = FindAssetsByType<Sprite>( "Assets/Popups");
            List<VideoClip> clips = FindAssetsByType<VideoClip>( "Assets/Popups");
            List<Sprite> gif = FindAssetsByType<Sprite>("Assets/Sprites/cumgif");
            gif.Reverse(); // Reverse gif order

            Debug.Log($"Discovered sprites: {sprites.Count} clips: {clips.Count}");
            
            int i = 0;
            
            imageProp.arraySize = sprites.Count;
            for (i = 0; i < sprites.Count; i++)
            {
                imageProp.GetArrayElementAtIndex(i).objectReferenceValue = sprites[i];
            }

            videoProp.arraySize = clips.Count;
            for (i = 0; i < clips.Count; i++)
            {
                videoProp.GetArrayElementAtIndex(i).objectReferenceValue = clips[i];
            }
            
            gifProp.arraySize = gif.Count;
            for (i = 0; i < gif.Count; i++)
            {
                gifProp.GetArrayElementAtIndex(i).objectReferenceValue = gif[i];
            }
        }
        
        List<T> FindAssetsByType<T>(string folder) where T : UnityEngine.Object
        {
            List<T> assets = new List<T>();
            string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T).Name.ToLower()), new string[] { folder });
            Debug.Log($"{guids.Length} guids");
            for( int i = 0; i < guids.Length; i++ )
            {
                string assetPath = AssetDatabase.GUIDToAssetPath( guids[i] );
                T asset = AssetDatabase.LoadAssetAtPath<T>( assetPath );
                if( asset != null )
                {
                    assets.Add(asset);
                }
            }
            return assets;
        }
    }
}