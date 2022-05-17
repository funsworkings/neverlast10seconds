using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace cumtypebeat
{
    public class CumBeatMachine : MonoBehaviour, ICumEndListener
    {
        public static System.Action<CumBeat, CumBeat> onSetupBeat;
        
        // Properties

        CumBeat _beat;
        public CumBeat CurrentBeat => _beat;

        [SerializeField] private Image beatUI;
        [SerializeField] private AnimationCurve beatAnimCurve;
        [SerializeField] private int bpm = 60;
        
        [SerializeField] private float matchBeatTimeAllowance = .1f;
        public float BeatAllowanceTime => matchBeatTimeAllowance;

        private int beatIndex = 0;
        public int BeatIndex => beatIndex;

        public bool Ended { get; set; } = false;

        // Helpers
        
        private float beatInterval => 60f / bpm;
        private bool initialised = false;

        private void Update()
        {
            if (!initialised) return; // Ignore till ready
            if (_beat == null) return;
            if (Ended) return;
            
            _beat.Tick(Time.deltaTime); // Tick beat 
            beatUI.transform.localScale = Vector3.one * beatAnimCurve.Evaluate(1f - Mathf.Clamp01(_beat.Progress));
        }

        public void Initialise()
        {
            SetupBeat();
            initialised = true;
        }

        void SetupBeat(float tDiff = 0f)
        {
            CumBeat lastBeat = _beat;
            
            _beat = new CumBeat(beatIndex, Time.time + beatInterval - (tDiff + matchBeatTimeAllowance), matchBeatTimeAllowance * 2f, beatInterval, beatSlippage =>
            {
                beatIndex++;
                SetupBeat(beatSlippage);
            });
            
            onSetupBeat?.Invoke(lastBeat, _beat);
        }

        public void SetBPM(int bpm)
        {
            this.bpm = bpm;
        }

        public void End()
        {
            Ended = true;
            beatUI.transform.localScale = Vector3.zero;
            _beat = null;
        }
    }
}