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
        public int GameScene = -1;
        
        [Header("Minigame Settings")]
        [SerializeField] private RawImage minigameImage;

        protected override void Start()
        {
            base.Start();
            
            draggable = false;
        }

        public void Initialize(RenderTexture renderTexture)
        {
            minigameImage.texture = renderTexture;
        }

        public override void SetColor(Color color)
        {
            minigameImage.color = color;
            tab.SetColor(color);
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
                _tab.gameObject.SetActive(false);
            }
        }

        public override void Activate()
        {
            base.Activate();
            
            (manager as BrowserController).Window.Select();
        }

        public override void OnVisible()
        {
            base.OnVisible();
            
            (manager as BrowserController).LoadMinigameForBrowserWindow(this);
        }
    }
}