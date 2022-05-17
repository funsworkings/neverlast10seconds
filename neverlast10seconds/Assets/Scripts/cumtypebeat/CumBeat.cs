using System;
using System.Collections;
using UnityEngine;

namespace cumtypebeat
{
    [System.Serializable]
    public class CumBeat
    {
        System.Action<float> onComplete = null;
        
        float t = 0f;

        public int index;
        
        float timestamp;
        float duration;
        private float length;

        public float Progress => (timestamp - t) / length;

        private float startT, endT = 0f;

        bool didReceiveInput = false;
        
        public bool CanInput => !didReceiveInput;
        public bool DidInput => didReceiveInput;

        public CumBeat(int index, float time, float duration, float beatLength, System.Action<float> onComplete)
        {
            this.index = index;
            
            this.t = Time.time;
            
            this.timestamp = time;
            this.duration = duration;
            this.length = beatLength;

            startT = time - duration / 2f;
            endT = time + duration / 2f;

            didReceiveInput = false;

            this.onComplete = onComplete;
        }

        public void Tick(float dt)
        {
            t += dt;
            if (t > endT)
            {
                float diff = (t - endT);
                onComplete?.Invoke(diff);
            }
        }

        public bool CheckInput()
        {
            if (didReceiveInput) throw new SystemException("Cannot enter second input on beat!");
            didReceiveInput = true;
            
            return Mathf.Abs(Time.time - timestamp) <= (duration / 2f);
        }
    }
}