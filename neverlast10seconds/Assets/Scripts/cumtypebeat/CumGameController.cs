using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

namespace cumtypebeat
{
    public class CumGameController : MonoBehaviour, ICumEndListener
    {
        [SerializeField] private CumBeatMachine _beatMachine;
        [SerializeField] private CumGrid _grid;
        [SerializeField] private CumForcePush _feedback;
        [SerializeField] private RectTransform _cursor;
        [SerializeField] private TMP_Text _levelUI;
        [SerializeField] private int _levelUICount = 10;
        [SerializeField] private CumLevel[] _levels;
        [SerializeField] private Animator endAnimator;
        
        private int _levelIndex = 0;
        
        private CumGif _activeBeatGridItem = null;

        private bool didInputBeat = false;
        public bool Ended { get; set; } = false;
        
        // UI

        [SerializeField] private TMP_Text instructionText;

        private void OnEnable()
        {
            CumBeatMachine.onSetupBeat += (aBeat, bBeat) =>
            {
                if (Ended) return; // Ignore beat behaviours on complete game
                
                if (aBeat != null) // had previous beat, check missed input
                {
                    if (!aBeat.DidInput)
                    {
                        onTryBeatInput(false);
                    }
                }
                
                int gridIndex = bBeat.index % (_grid.GifCount);
                _grid.SetActiveGif(gridIndex);
                Debug.Log("Switch to grid gif: " + gridIndex);
                
                _activeBeatGridItem = _grid.ActiveGif;
                ReAnchorCursor(true);

                if (gridIndex == 0) // Did loop!
                {
                    CheckForLevelChange();
                }
            };
        }

        void ReAnchorCursor(bool visible)
        {
            if (visible)
            {
                _cursor.gameObject.SetActive(true);
                _cursor.transform.parent = _activeBeatGridItem.transform;
                _cursor.sizeDelta = Vector2.zero;
                _cursor.localPosition = Vector2.zero;
            }
            else
            {
                _cursor.gameObject.SetActive(false);
            }
        }

        private IEnumerator Start()
        {
            instructionText.text = _levelUI.text =  "";
            yield return new WaitForEndOfFrame(); // Wait for scene elements to be f ready
            
            _grid.SetActiveGif(0);
            ReAnchorCursor(false);

            StartCoroutine(GetReadyRoutine()); // Trigger game
        }

        private void Update()
        {
            if (!initialised) return;
            if (Ended) return;
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TryMatchBeat();    
            }
        }

        IEnumerator GetReadyRoutine()
        {
            while (!Input.GetKeyUp(KeyCode.Space))
            {
                yield return null;
            }
            
            instructionText.text = "get ready";
            yield return new WaitForSecondsRealtime(1f);

            float countdown = 3f;
            while (countdown > 0f)
            {
                countdown -= Time.unscaledDeltaTime;
                instructionText.text = Mathf.CeilToInt(countdown).ToString();
                
                yield return null;
            }

            instructionText.text = "";
            Initialise();
        }

        bool initialised = false;
        void Initialise()
        {
            SetLevel(0);
            _beatMachine.Initialise();
            
            initialised = true;
        }

        void TryMatchBeat()
        {
            try
            {
                bool success = _beatMachine.CurrentBeat.CheckInput();
                onTryBeatInput(success);
            }
            catch (System.Exception err)
            {
                Debug.LogWarning($"Unable to enter input for beat: {err.Message}");
            }
        }

        void onTryBeatInput(bool success)
        {
            if (success)
            {
                _grid.ActiveGif.Advance();
                _feedback.Win();
            }
            else
            {
                _grid.ActiveGif.Fail();
                _feedback.Lose();
            }
        }

        void SetLevel(int index)
        {
            index = Mathf.Clamp(index, 0, _levels.Length - 1);
            _levelIndex = index;

            string _levelText = "";
            for (int i = 0; i < _levelUICount; i++) _levelText += (_levelIndex + 1).ToString();

            _levelText += "\n" + $"<i>{_levels[_levelIndex].name}"; // Attach level name
            _levelUI.text = _levelText;
            
            _beatMachine.SetBPM(_levels[_levelIndex].bpm);
        }

        void CheckForLevelChange()
        {
            int levelDir = 0;
            
            int currentLevel = _levelIndex;
            for (int i = 0; i < _grid.Gifs.Length; i++)
            {
                var gif = _grid.Gifs[i];
                var diff = gif.Count - currentLevel;
                
                if (diff > 0) levelDir++;
                else if (diff < 0) levelDir--;
            }
            Debug.Log($"Total level diff: {levelDir}");

            if (currentLevel == (CumGlobalPrefs.NumberOfLevels - 1) && levelDir >= 0)
            {
                CompleteGame();
            }
            else
            {
                if (levelDir > 0) levelDir = 1;
                else if (levelDir < 0) levelDir = -1;

                SetLevel(currentLevel + levelDir);   
            }
        }

        [ContextMenu("Debug complete game")]
        public void CompleteGame()
        {
            Ended = true;
            
            var listeners = FindObjectsOfType<MonoBehaviour>().OfType<ICumEndListener>().ToArray();
            foreach(ICumEndListener listener in listeners) listener.End();
        }

        public void End()
        {
            endAnimator.SetTrigger("finish");
        }
    }
}