using System;
using cumOS.Scriptables;
using cumOS.UIShit;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
        public MinigameData GameScene { get; set; } = null;
        
        [Header("Minigame Settings")]
        [SerializeField] private RawImage minigameImage;
        public override Color color => minigameImage.color;


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

        protected override Color GetColor()
        {
            Color col = Random.ColorHSV(0f, 1f, 0f, 1f, .87f, 1f);
            return col;
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

        public override void Destroy()
        {
            base.Close();
        }

        public override void SetActive(bool active)
        {
            base.SetActive(active);
            
            if(active) (manager as BrowserController).Window.Select();
        }

        public override void OnVisible()
        {
            base.OnVisible();
            
            (manager as BrowserController).LoadMinigameForBrowserWindow(this);
        }
    }
}