using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace cumOS.Overworld
{
    public class MinigameLoader : Singleton<MinigameLoader>
    {
        private Coroutine sceneLoadOp = null;
        private int activeScene = -1;
        
        private Action<Scene> onSceneLoadCompleted;
        private Action onSceneLoadFailure;

        public void LoadGame(int sceneIndex, Action<Scene> onCompleted, Action onFailure)
        {
            if (sceneIndex == activeScene)
            {
                onCompleted?.Invoke(SceneManager.GetSceneByBuildIndex(sceneIndex));
                return;
            }
            
            if (sceneLoadOp != null)
            {
                onSceneLoadFailure?.Invoke(); // Send failure callback to caller
                return;
            }
            
            onSceneLoadCompleted = onCompleted;
            onSceneLoadFailure = onFailure;

            sceneLoadOp = StartCoroutine(GameLoadAsync(sceneIndex));
        }

        IEnumerator GameLoadAsync(int sceneBuildIndex)
        {
            yield return GameUnloadAsync(); // Wait for unload op to complete
            yield return null;

            var asyncLoadOp = SceneManager.LoadSceneAsync(sceneBuildIndex, LoadSceneMode.Additive);
            asyncLoadOp.allowSceneActivation = true;

            while (!asyncLoadOp.isDone)
            {
                Debug.Log($"Scene load progress for {sceneBuildIndex}: {asyncLoadOp.progress}%");
                yield return null; // Wait for async scene load to finish
            }

            var sc = SceneManager.GetSceneByBuildIndex(sceneBuildIndex);
            if (sc.IsValid())
            {
                activeScene = sceneBuildIndex;
                onSceneLoadCompleted?.Invoke(sc);   
            }
            else
            {
                Debug.LogWarning($"Scene: {sceneBuildIndex} not found!");
            
                activeScene = -1;
                onSceneLoadFailure?.Invoke();
            }

            sceneLoadOp = null;
            onSceneLoadCompleted = null;
            onSceneLoadFailure = null;
        }
        
        IEnumerator GameUnloadAsync()
        {
            if (activeScene >= 0)
            {
                Debug.Log($"Unload scene: {activeScene}");
                
                var asyncUnloadOp = SceneManager.UnloadSceneAsync(activeScene);
                asyncUnloadOp.allowSceneActivation = true;
                
                while (!asyncUnloadOp.isDone) yield return null; // Wait for scene unload op to complete
            }

            activeScene = -1;

            yield return null;
        }
    }
}