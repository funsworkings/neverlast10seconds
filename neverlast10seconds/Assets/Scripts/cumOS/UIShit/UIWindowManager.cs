using System;
using System.Collections.Generic;
using UnityEngine;

namespace cumOS.UIShit
{
    public class UIWindowManager : MonoBehaviour
    {
        protected List<UIWindow> windows = new List<UIWindow>();
        protected UIWindow selectedWindow = null;
        
        [SerializeField] protected Transform itemsRoot;

        protected virtual void Start()
        {
            if(itemsRoot == null) itemsRoot = transform;
            
            var scrapeWindows = itemsRoot.GetComponentsInChildren<UIWindow>(true);
            foreach (UIWindow window in scrapeWindows)
            {
                if (window.gameObject.activeSelf)
                {
                    windows.Insert(0, window);
                }
                else
                {
                    windows.Add(window);
                }

                window.manager = this;
            }
            UpdateSiblingIndices();
            
            UIWindow.onSelectWindow += window =>
            {
                if(selectedWindow != window && selectedWindow != null) selectedWindow.Deactivate();
                selectedWindow = window;
                window.Activate();
                
                BringToFront(window); // Bring window to front
                window.Drag();
            };

            UIWindow.onReleaseWindow += window =>
            {
                window.Release();
            };

            UIWindow.onCloseWindow += window =>
            {
                DeactivateWindow(window);
            };
        }

        #region Active ops

        public void ActivateWindow(UIWindow window)
        {
            if (window == null) return;

            if (!window.gameObject.activeSelf)
            {
                window.gameObject.SetActive(true);
                window.Show();
            }
            BringToFront(window);
        }

        public void DeactivateWindow(UIWindow window)
        {
            if (window == null) return;

            if (selectedWindow == window && selectedWindow != null)
            {
                selectedWindow.Deactivate();
                selectedWindow = null;
            }
            
            SendToBack(window);
            if (window.gameObject.activeSelf)
            {
                window.Hide();
                window.gameObject.SetActive(false);
            }
        }

        public void AddWindow(UIWindow window)
        {
            if (!windows.Contains(window))
            {
                windows.Add(window);
                window.manager = this;

                window.transform.parent = itemsRoot;
                BringToFront(window);
            }
        }

        #endregion
        
        #region View ops

        public void BringToFront(UIWindow window)
        {
            if (window == null) return;
            if (!windows.Contains(window)) return;
            
            int index = windows.IndexOf(window);
            windows.RemoveAt(index);
            windows.Add(window);
            
            UpdateSiblingIndices();
        }
        
        public void BringForward(UIWindow window)
        {
            if (window == null) return;
            if (!windows.Contains(window)) return;
            
            int index = windows.IndexOf(window);
            if (index < windows.Count - 1) // Can bring forward
            {
                int nextIndex = index + 1;
                windows.RemoveAt(index);
                windows.Insert(nextIndex, window);
                
                UpdateSiblingIndices();
            }
        }

        public void SendToBack(UIWindow window)
        {
            if (window == null) return;
            if (!windows.Contains(window)) return;

            int index = windows.IndexOf(window);
            windows.RemoveAt(index);
            windows.Insert(0, window);
            
            UpdateSiblingIndices();
        }

        public void SendBack(UIWindow window)
        {
            if (window == null) return;
            if (!windows.Contains(window)) return;
            
            int index = windows.IndexOf(window);
            if (index > 0) // Can send back
            {
                int prevIndex = index - 1;
                var prevWindow = windows[prevIndex];
                windows.RemoveAt(prevIndex);
                windows.Insert(index, prevWindow);
                
                UpdateSiblingIndices();
            }
        }

        protected void UpdateSiblingIndices()
        {
            int i = 0;
            foreach (UIWindow window in windows)
            {
                window.transform.SetSiblingIndex(i++);
            }
        }
        
        #endregion

    }
}