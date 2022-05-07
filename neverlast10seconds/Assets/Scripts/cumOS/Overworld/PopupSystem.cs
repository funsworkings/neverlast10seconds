using System;
using System.Collections.Generic;
using cumOS.Scriptables;
using cumOS.UIShit;
using UnityEngine;
using UnityEngine.Video;
using Random = UnityEngine.Random;

namespace cumOS.Overworld
{
    public class PopupSystem : UIWindowManager
    {
        // properties

        [SerializeField] Popup popupPrefab;
        [SerializeField] private PopupDatabase _assetDatabase;
        
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.P))
            {
                AddPopup(Instantiate(popupPrefab, itemsRoot));
            }
        }

        void AddPopup(Popup popup)
        {
            popup.transform.position = GetRandomPopupPosition();
            AttachPopupContent(popup);
            
            AddWindow(popup);
        }

        void AttachPopupContent(Popup popup)
        {
            if (Random.Range(0, 2) == 0) // Image
            {
                var img = _assetDatabase.images[Random.Range(0, _assetDatabase.images.Count)];
                popup.Initialize(img);
            }
            else // Video
            {
                var clip = _assetDatabase.clips[Random.Range(0, _assetDatabase.clips.Count)];
                popup.Initialize(clip);
            }
        }

        Vector2 GetRandomPopupPosition()
        {
            return new Vector2(Random.Range(0, Screen.width), Random.Range(0, Screen.height));
        }
    }
}