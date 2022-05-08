using System;
using System.Collections.Generic;
using cumOS.UIShit;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace cumOS.Overworld
{
    public class BrowserController : UIWindowManager
    {
        // Properties

        private UIWindow _window;
        public UIWindow Window => _window;
        
        [SerializeField] private Transform tabsRoot;
        [SerializeField] private BrowserUITab tabPrefab;
        [SerializeField] private BrowserWindow browserWindowPrefab;
        
        [Header("Spawning Settings")]
        public bool spawnBrowserTabs = true;
        public float spawnTimer;
        public float spawnTimerTotal;
        public Vector2 spawnRandomTimeRange = new Vector2(5f, 10f);

        [Header("Mini-game loading.")]
        [Tooltip("When the random roll is less than this value, it is a mini-game browser window.")]
        public float minigameChance;
        [Tooltip("Array of possible mini-game scenes to load.")]
        public string[] possibleGameScenesToLoad;
        public List< RenderTexture>  unusedRenderTextures = new List<RenderTexture>();
        public List< RenderTexture> usedRendertextures = new List<RenderTexture>();
        private void Awake()
        {
            _window = GetComponent<UIWindow>();
        }

        private void Update()
        {
            if (_window.IsActive)
            {
                SpawnBrowserWindow();
                
                if (Input.GetKeyUp(KeyCode.B))
                {
                    AddBrowserWindow(Instantiate(browserWindowPrefab, itemsRoot));
                }
            }
        }

        public void AddBrowserWindow(BrowserWindow browserWindow)
        {
            AddWindow(browserWindow);
            MinigameCheck(browserWindow);
        }

        public BrowserUITab RequestTab(BrowserWindow window)
        {
            var tab = Instantiate(tabPrefab, tabsRoot);

            tab.Initialize(window, window.thumbnail);
            
            tab.transform.SetSiblingIndex(window.transform.GetSiblingIndex()); // Match window

            return tab;
        }
        
        void SpawnBrowserWindow()
        {
            if (spawnBrowserTabs)
            {
                spawnTimer -= Time.deltaTime;

                if (spawnTimer < 0)
                {
                    //spawn browser window
                    AddBrowserWindow(Instantiate(browserWindowPrefab, itemsRoot));
                    SetRandomSpawnTimer();
                }
            }
        }

        void SetRandomSpawnTimer()
        {
            //random total
            spawnTimerTotal = Random.Range(spawnRandomTimeRange.x, spawnRandomTimeRange.y);
            //set popup timer
            spawnTimer = spawnTimerTotal;
        }

        /// <summary>
        /// Checks if this window should be a mini-game. 
        /// </summary>
        public void MinigameCheck(BrowserWindow browserWindow)
        {
            float randomChance = Random.Range(0f, 100f);

            if (randomChance < minigameChance)
            {
                LoadRandomMinigame(browserWindow);
            }
        }

        /// <summary>
        /// Loads a random mini-game scene additively. 
        /// </summary>
        public void LoadRandomMinigame(BrowserWindow browserWindow)
        {
            string sceneName = possibleGameScenesToLoad[Random.Range(0, possibleGameScenesToLoad.Length)];
            Scene minigame = SceneManager.GetSceneByName( sceneName);
            SceneManager.LoadScene(minigame.name, LoadSceneMode.Additive);
            browserWindow.SetMinigame(minigame);
        }
        
        //TODO will need an unload scene function when we close a window. 
        //https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.UnloadSceneAsync.html

        /// <summary>
        /// Assign the first unused render texture to the minigame in the browser window. 
        /// </summary>
        /// <param name="window"></param>
        public void AssignRenderTextureToMinigame(Scene scene, BrowserWindow window)
        {
            //get the render texture
            RenderTexture renderToUse = unusedRenderTextures[0];
            
            //get all objects in the scene we loaded
            GameObject[] minigameObjects = scene.GetRootGameObjects();
            //loop thru them
            foreach (var sceneObj in  minigameObjects)
            {
                //looking for the main cam
                if (sceneObj.CompareTag("MainCamera"))
                {
                    //get the camera component
                    Camera sceneCam = sceneObj.GetComponent<Camera>();
                    //assign the target render texture.
                    sceneCam.targetTexture = renderToUse;
                    break;
                }
            }
            
            //assign the render texture to our raw image in the window. 
            window.AssignRenderTexture(renderToUse);
            //remove from circulation. 
            usedRendertextures.Add(renderToUse);
            unusedRenderTextures.RemoveAt(0);
        }

        //Removes render texture from used and adds back to unused. 
        public void DeactivateRenderTexture(RenderTexture renderTexture)
        {
            usedRendertextures.Remove(renderTexture);
            unusedRenderTextures.Add(renderTexture);
        }
    }
}