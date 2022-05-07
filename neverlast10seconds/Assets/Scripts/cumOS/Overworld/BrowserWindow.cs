using System;
using cumOS.UIShit;
using UnityEngine;

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

        protected override void Start()
        {
            base.Start();
            
            draggable = false;
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
    }
}