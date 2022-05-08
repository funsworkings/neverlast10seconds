using System;
using System.Collections.Generic;
using cumOS.UIShit;
using UnityEngine;

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

        private void Awake()
        {
            _window = GetComponent<UIWindow>();
        }

        private void Update()
        {
            if (_window.IsActive)
            {
                if (Input.GetKeyUp(KeyCode.B))
                {
                    AddBrowserWindow(Instantiate(browserWindowPrefab, itemsRoot));
                }
            }
        }

        public void AddBrowserWindow(BrowserWindow browserWindow)
        {
            AddWindow(browserWindow);
        }

        public BrowserUITab RequestTab(BrowserWindow window)
        {
            var tab = Instantiate(tabPrefab, tabsRoot);
            tab.Initialize(window, window.thumbnail);
            
            tab.transform.SetSiblingIndex(window.transform.GetSiblingIndex()); // Match window

            return tab;
        }
    }
}