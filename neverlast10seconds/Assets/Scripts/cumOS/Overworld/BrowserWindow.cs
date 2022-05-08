using System;
using cumOS.UIShit;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace cumOS.Overworld
{
    public class BrowserWindow : UIWindow
    {
        // Properties
        BrowserUITab _tab;
        BrowserUITab tab
        {
            get
            {
                if (_tab == null)
                {
                    _tab = (manager as BrowserController).RequestTab(this);
                }

                return _tab;
            }
        }
        
        public Sprite thumbnail;

        [Header("Minigame Settings")]
        public bool isMinigame;
        public Scene loadedMinigame;
        private RawImage minigameImage;
        private RenderTexture renderTexture;

        public RawImage MinigameImage
        {
            get
            {
                return minigameImage;
            }
        }

        protected override void Start()
        {
            base.Start();
            
            draggable = false;
            tab.SetColor(color);
        }

        public void SetMinigame(Scene loadedGame)
        {
            isMinigame = true;
            loadedMinigame = loadedGame;
            //destroy image
            Image imageComp = GetComponent<Image>();
            Destroy(imageComp);
            //add raw image instead
            minigameImage = gameObject.AddComponent<RawImage>();
            //assign the render texture :)
            (manager as BrowserController).AssignRenderTextureToMinigame(loadedGame, this);
        }

        public void AssignRenderTexture(RenderTexture texture)
        {
            renderTexture = texture;
            MinigameImage.texture = texture;
        }

        public override void Show()
        {
            base.Show();
            
            tab.transform.SetSiblingIndex(transform.GetSiblingIndex());
        }

        public override void Hide()
        {
            base.Hide();
            
            if (_tab != null)
            {
                if (MinigameImage != null)
                {
                    //tell BrowserController take my render texture out of circulation. 
                    (manager as BrowserController).DeactivateRenderTexture(renderTexture);
                }
                
                _tab.gameObject.SetActive(false);
            }
        }

        public override void Activate()
        {
            base.Activate();
            
            (manager as BrowserController).Window.Select();
        }
    }
}