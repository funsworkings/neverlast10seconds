using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace cumtypebeat
{
    public class CumGif : MonoBehaviour
    {
        [SerializeField] Image image;
        [SerializeField] Image outline;
        [SerializeField] private GameObject[] levelUi;

        private int m_count = 0;
        public int Count
        {
            get => m_count;
            private set
            {
                m_count = Mathf.Clamp(value, 0, CumGlobalPrefs.NumberOfLevels-1);
                UpdateCounterUI();
            }
        }

        private Sprite[] frames;
        private float t = 0f;
        
        private int frameStackHeight = 1;
        private int frameIndex = 0; // Baseline frame to count from
        private int frameLoop = 0;

        private const float refreshT = .1f;
        public static readonly int DefaultFrameStackHeight = 2;

        private int[] framesPerLevel;

        int GetCurrentFrame()
        {
            return (int)Mathf.Repeat(frameLoop + frameIndex, frames.Length-1);
        }

        private void Start()
        {
            SetActive(false);
            Count = 0;
        }

        private void Update()
        {
            if (!initialised) return; // Ignore updates until ready
            
            t += Time.unscaledDeltaTime;
            if (t > refreshT)
            {
                t -= refreshT;
                Tick();
            }
        }

        private bool initialised = false;
        public void Initialise(Sprite[] frames, int[] framesPerLevel)
        {
            this.frames = frames;
            frameStackHeight = DefaultFrameStackHeight;
            frameIndex = frameLoop = 0;

            this.framesPerLevel = framesPerLevel;
            
            AssignFrame(0); // Set default frame
            
            initialised = true;
        }

        void Tick()
        {
            if (++frameLoop > frameStackHeight-1)
            {
                frameLoop = 0;
            }

            var i = GetCurrentFrame();
            AssignFrame(i);
        }

        void AssignFrame(int index)
        {
            var spr = frames[index];
            image.sprite = spr;
        }

        public void SetActive(bool active)
        {
            outline.gameObject.SetActive(active);
        }

        public void Advance()
        {
            if (Count < (CumGlobalPrefs.NumberOfLevels - 1))
            {
                frameStackHeight += framesPerLevel[Count]; // Append previous count frames
                ++Count;
            }
            else
            {
                frameStackHeight = frames.Length; // Assign to maximum
                Count = CumGlobalPrefs.NumberOfLevels - 1;
            }
            image.color = Color.green;
        }

        public void Fail()
        {
            Count = 0;
            frameStackHeight = DefaultFrameStackHeight;

            frameIndex = GetCurrentFrame();
            frameLoop = 0;

            image.color = Color.red;
        }

        public void Finish()
        {
            Count = CumGlobalPrefs.NumberOfLevels - 1;
            frameStackHeight = frames.Length;
        }

        public void ResetColor()
        {
            image.color = Color.white;
        }

        void UpdateCounterUI()
        {
            for (int i = 0; i < levelUi.Length; i++)
            {
                levelUi[i].SetActive(i < Count);
            }
        }
    }
}