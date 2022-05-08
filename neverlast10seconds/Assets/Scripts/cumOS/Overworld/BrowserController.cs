using System;
using System.Collections.Generic;
using cumOS.Scriptables;
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
        [SerializeField] private PopupDatabase database;
        
        [Header("Spawning Settings")]
        public bool spawnBrowserTabs = true;
        public float spawnTimer;
        public float spawnTimerTotal;
        public Vector2 spawnRandomTimeRange = new Vector2(5f, 10f);

        [Header("Mini-game loading.")]
        [Tooltip("When the random roll is less than this value, it is a mini-game browser window.")]
        public float minigameChance;
        [Tooltip("Array of possible mini-game scenes to load.")]
        public List<int> availableGameScenes, unavailableGameScenes = new List<int>();

        #region Browser texture

        private RenderTexture _browserWindowTexture;
        [SerializeField] int _browserTextureHeight = 600, _browserTextureWidth = 800;
        RenderTexture RequestRenderTexture()
        {
            bool flag_create = true;
            if (_browserWindowTexture != null)
            {
                if (_browserWindowTexture.width != _browserTextureWidth || _browserWindowTexture.height != _browserTextureHeight)
                {
                    DestroyImmediate(_browserWindowTexture);
                }
                else
                {
                    flag_create = false;
                }
            }

            if (flag_create)
            {
                _browserWindowTexture = new RenderTexture(_browserTextureWidth, _browserTextureHeight, 0);
            }

            return _browserWindowTexture;
        }
        
        #endregion
        
        private void Awake()
        {
            _window = GetComponent<UIWindow>();
        }

        private void OnDestroy()
        {
            if (_browserWindowTexture != null)
            {
                DestroyImmediate(_browserWindowTexture);
                _browserWindowTexture = null;
            }
        }

        private void Update()
        {
            if (_window.IsActive)
            {
                SpawnBrowserWindow();
                
                if (Input.GetKeyUp(KeyCode.B))
                {
                    RandomBrowserWindow();
                }
            }
        }

        void RandomBrowserWindow()
        {
            int gameScene = LoadRandomMinigame();
            if (gameScene >= 0)
            {
                AddBrowserWindow(Instantiate(browserWindowPrefab, itemsRoot), gameScene);
            }
        }

        void AddBrowserWindow(BrowserWindow browserWindow, int gameScene)
        {
            browserWindow.GameScene = gameScene;
            browserWindow.Initialize(null);
                
            AddWindow(browserWindow);
            LoadMinigameForBrowserWindow(browserWindow);
        }

        public override void DeactivateWindow(UIWindow window)
        {
            if (window != null)
            {
                var sceneIndex = (window as BrowserWindow).GameScene;
                if (sceneIndex >= 0)
                {
                    availableGameScenes.Add(sceneIndex); // Return to available scenes
                }
            }
            
            base.DeactivateWindow(window);
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
                    RandomBrowserWindow();
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
        /// Loads a random mini-game scene additively. 
        /// </summary>
        int LoadRandomMinigame()
        {
            if (availableGameScenes.Count > 0)
            {
                int index = Random.Range(0, availableGameScenes.Count);
                int sc = availableGameScenes[index];
                availableGameScenes.RemoveAt(index);

                return sc;
            }

            return -1;
        }

        public void LoadMinigameForBrowserWindow(BrowserWindow browserWindow)
        {
            int minigameIndex = browserWindow.GameScene;
            
            Debug.Log($"Trigger minigame load: {minigameIndex}");
            MinigameLoader.Instance.LoadGame(minigameIndex, scene => 
            {
                bool success = AssignRenderTextureToGameScene(scene);
                if (success)
                {
                    browserWindow.Initialize(RequestRenderTexture());
                }
                else
                {
                    Debug.LogWarning($"Failed to discover main camera in scene: {minigameIndex}");
                    browserWindow.Initialize(null);
                }
            }, () =>
            {
                Debug.LogWarning("Failed to load game scene, MISC!");
                browserWindow.Initialize(null);
            });
        }
        
        //TODO will need an unload scene function when we close a window. 
        //https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.UnloadSceneAsync.html

        /// <summary>
        /// Assign the first unused render texture to the minigame in the browser window. 
        /// </summary>
        /// <param name="window"></param>
        public bool AssignRenderTextureToGameScene(Scene scene)
        {
            //get the render texture
            RenderTexture renderToUse = RequestRenderTexture(); // Fetch render texture
            
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
                    return true;
                }
            }

            return false;
        }

        #region Old
        
        /*
         
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
         
         */
        
        #endregion
    }
}