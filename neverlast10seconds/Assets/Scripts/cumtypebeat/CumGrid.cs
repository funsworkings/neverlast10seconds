using System;
using System.Collections.Generic;
using System.Linq;
using cumOS.Scriptables;
using UnityEngine;
using UnityEngine.UI;

namespace cumtypebeat
{
    public class CumGrid : MonoBehaviour, ICumEndListener
    {
        private RectTransform container;
        
        private CumGif[] gifs;
        
        public int GifCount => gifs.Length;
        public CumGif[] Gifs => gifs;
        
        private int activeGifIndex = -1;
        public int ActiveGiftIndex => activeGifIndex;

        public CumGif ActiveGif
        {
            get
            {
                if (activeGifIndex >= 0)
                {
                    return gifs[activeGifIndex];
                }

                return null;
            }
        }

        // Properties

        private Canvas _canvas;
        private GridLayoutGroup _layoutGroup;

        [SerializeField] private PopupDatabase _db;
        [SerializeField] private CumGif gifPrefab;
        [SerializeField, Min(1)] private int gridWidth, gridHeight;

        private int containerWidth => (int)(container.rect.width * _canvas.scaleFactor);
        private int containerHeight => (int)(container.rect.height * _canvas.scaleFactor);
        
        private int sw = 0, sh = 0;

        private void Awake()
        {
            _layoutGroup = GetComponent<GridLayoutGroup>();
        }

        private void Start()
        {
            _canvas = GetComponentInParent<Canvas>();
            container = GetComponentInParent<RectTransform>();
            
            _layoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            _layoutGroup.constraintCount = gridWidth;
            _layoutGroup.cellSize = new Vector2(1f * containerWidth / gridWidth, 1f * containerHeight / gridHeight) / _canvas.scaleFactor;

            PopulateGrid();
            BindGrid(); // Bind gifs to each other
            
            SetActiveGif(0);
        }

        void PopulateGrid()
        {
            List<CumGif> gifs = new List<CumGif>();

            Sprite[] frames = _db.cumGif.ToArray();
            List<int> framesPerLevel = new List<int>();

            int maxFramesPerLevel = Mathf.FloorToInt(1f * frames.Length / CumGlobalPrefs.NumberOfLevels);
            int totalFrames = (frames.Length - CumGif.DefaultFrameStackHeight);
            
            
            for (int i = 0; i < CumGlobalPrefs.NumberOfLevels-1; i++)
            {
                if (i < CumGlobalPrefs.NumberOfLevels - 2) // Use min value
                {
                    totalFrames -= maxFramesPerLevel;
                    framesPerLevel.Add(maxFramesPerLevel);
                }
                else // Use leftover value
                {
                    framesPerLevel.Add(totalFrames);
                }
            }

            var _framesPerLevelArr = framesPerLevel.ToArray();
            
            var items = gridWidth * gridHeight;
            for (int i = 0; i < items; i++)
            {
                var _gif = Instantiate(gifPrefab, transform);
                _gif.Initialise(frames, _framesPerLevelArr);
                
                gifs.Add(_gif);
            }

            this.gifs = gifs.ToArray();
        }
        
        /*
         
         x x x
         x x x
         
         */

        void BindGrid()
        {
            int row = 0, column = 0;

            for (int i = 0; i < gifs.Length; i++)
            {
                /*
                CumGif top = (row > 0) ? gifs[i - gridWidth] : null;
                CumGif bottom = (row < (gridHeight - 1)) ? gifs[i + gridWidth] : null;
                CumGif left = (column > 0) ? gifs[i - 1] : null;
                CumGif right = (column < (gridWidth - 1)) ? gifs[i + 1] : null;
                
                gifs[i].Bind(top, left, right, bottom);*/
                
                if (++column > gridWidth) // Move to next row!
                {
                    column = 0;
                    ++row;
                }
            }
        }

        private void Update()
        {
            int cw = containerWidth;
            int ch = containerHeight;
            
            // Resize functionality
            if (sw != cw || sh != ch)
            {
                sw = cw;
                sh = ch;
                
                _layoutGroup.cellSize = new Vector2(1f * cw / gridWidth, 1f * ch / gridHeight) / _canvas.scaleFactor;
            }
        }

        public void SetActiveGif(int index)
        {
            if (ActiveGif != null)
            {
                ActiveGif.SetActive(false);
            }

            activeGifIndex = index;
            if(ActiveGif != null) ActiveGif.SetActive(true);
        }
        
        public void End()
        {
            Ended = true;
            SetActiveGif(-1);
            foreach(CumGif gif in gifs) {gif.ResetColor(); gif.Finish();}
        }

        public bool Ended { get; set; } = false;
    }
}