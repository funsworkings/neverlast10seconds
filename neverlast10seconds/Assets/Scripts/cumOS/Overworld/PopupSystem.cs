using System;
using cumOS.UIShit;
using UnityEngine;
using Random = UnityEngine.Random;

namespace cumOS.Overworld
{
    public class PopupSystem : UIWindowManager
    {
        // properties

        [SerializeField] Popup popupPrefab;
        
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
            AddWindow(popup);
        }

        Vector2 GetRandomPopupPosition()
        {
            return new Vector2(Random.Range(0, Screen.width), Random.Range(0, Screen.height));
        }
    }
}