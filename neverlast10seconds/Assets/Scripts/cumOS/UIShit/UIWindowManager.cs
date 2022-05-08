using System;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
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
                CreateWindow(window, reorder:false);
            }
            UpdateSiblingIndices();
        }
        
        #region Window ops

        public void DidSelectWindow(UIWindow window)
        {
            if (selectedWindow != window)
            {
                if(selectedWindow != null) selectedWindow.SetActive(false);
                selectedWindow = window;
                window.SetActive(true);
            }

            BringToFront(window); // Bring window to front
            window.Drag();
        }

        public void DidReleaseWindow(UIWindow window)
        {
            window.Release();
        }

        public void DidCloseWindow(UIWindow window)
        {
            DisableWindow(window);
        }

        public void DidDestroyWindow(UIWindow window)
        {
            DestroyWindow(window);
        }
        
        #endregion

        #region Active ops

        protected void EnableWindow(UIWindow window)
        {
            if (window == null) return;

            if (!window.gameObject.activeSelf)
            {
                window.gameObject.SetActive(true);
                window.Show();
            }
            
            BringToFront(window);
        }

        protected virtual void DisableWindow(UIWindow window)
        {
            if (window == null) return;

            if (selectedWindow == window && selectedWindow != null)
            {
                selectedWindow.SetActive(false);
                selectedWindow = null;
            }

            SendToBack(window);
            if (window.gameObject.activeSelf)
            {
                window.Hide();
                window.gameObject.SetActive(false);
            }
        }

        protected void CreateWindow(UIWindow window, bool reorder = true)
        {
            if (!windows.Contains(window))
            {
                windows.Add(window);
                window.Bind(this);

                window.transform.parent = itemsRoot;
                if(reorder) BringToFront(window);
            }
            else
            {
                EnableWindow(window); // Bring window to front
            }
        }

        protected void DestroyWindow(UIWindow window)
        {
            if (windows.Contains(window))
            {
                windows.Remove(window);
                Destroy(window.gameObject);
                
                UpdateSiblingIndices(); // Fire off callback
            }
        }

        #endregion
        
        #region View ops

        void BringToFront(UIWindow window)
        {
            if (window == null) return;
            if (!windows.Contains(window)) return;
            
            int index = windows.IndexOf(window);
            windows.RemoveAt(index);
            windows.Add(window);
            
            UpdateSiblingIndices();
        }
        
        void BringForward(UIWindow window)
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

        void SendToBack(UIWindow window)
        {
            if (window == null) return;
            if (!windows.Contains(window)) return;

            int index = windows.IndexOf(window);
            windows.RemoveAt(index);
            windows.Insert(0, window);
            
            UpdateSiblingIndices();
        }

        void SendBack(UIWindow window)
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

        void UpdateSiblingIndices()
        {
            int i = 0;
            int count = windows.Count;
            foreach (UIWindow window in windows)
            {
                window.transform.SetSiblingIndex(i++);
                
                if(i-1 == count-1) window.OnVisible();
                else window.OnHidden();
            }
        }
        
        #endregion

    }
}