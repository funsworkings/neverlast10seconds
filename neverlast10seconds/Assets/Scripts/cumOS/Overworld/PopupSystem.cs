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

        [Header("Spawning Settings")]
        public bool spawnPopups = true;
        public float popupTimer;
        public float popupTimerTotal;
        public Vector2 popupRandomTimeRange = new Vector2(5f, 10f);
        
         
        private PopupAudio popupAudio;
        public PopupAudio PopupAudio
        {
            get
            {
                if (popupAudio == null)
                {
                    popupAudio = GetComponent<PopupAudio>();
                }

                return popupAudio;
            }
         
        }

        private void OnEnable()
        {
            CumMeter.Instance.onBeginMasturbationEvent.AddListener(SetRandomPopupTimer);
        }

        private void OnDisable()
        {
            CumMeter.Instance.onBeginMasturbationEvent.RemoveListener(SetRandomPopupTimer);
        }

        private void Update()
        {
            SpawnPopups();

            //input key to spawn popups 
            if (Input.GetKeyUp(KeyCode.P))
            {
                AddPopup(Instantiate(popupPrefab, itemsRoot));
            }
        }

        void SpawnPopups()
        {
            if (spawnPopups)
            {
                popupTimer -= Time.deltaTime;

                if (popupTimer < 0)
                {
                    //spawn popup
                    AddPopup(Instantiate(popupPrefab, itemsRoot));
                    SetRandomPopupTimer();
                }
            }
        }

        void SetRandomPopupTimer()
        {
            //random total
            popupTimerTotal = Random.Range(popupRandomTimeRange.x, popupRandomTimeRange.y);
            //set popup timer
            popupTimer = popupTimerTotal;
        }

        /// <summary>
        /// Generates popups. 
        /// </summary>
        /// <param name="popup"></param>
        void AddPopup(Popup popup)
        {
            popup.transform.position = GetRandomPopupPosition();
            AttachPopupContent(popup);
            
            CreateWindow(popup);
        }

        void AttachPopupContent(Popup popup)
        {
            if (Random.Range(0, 2) == 0) // Image
            {
                var img = _assetDatabase.images[Random.Range(0, _assetDatabase.images.Count)];
                popup.Initialize(img, this);
            }
            else // Video
            {
                var clip = _assetDatabase.clips[Random.Range(0, _assetDatabase.clips.Count)];
                popup.Initialize(clip,this);
            }
        }

        Vector2 GetRandomPopupPosition()
        {
            return new Vector2(Random.Range(0, Screen.width), Random.Range(0, Screen.height));
        }
    }
}