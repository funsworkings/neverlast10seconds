using System;
using cumOS.UIShit;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace cumOS.Overworld
{
    public class Popup  : UIWindow
    {
        // Properties

        private Vector3 origScale;

        [SerializeField] private Image _image;
        private Sprite _sprite;
        
        [SerializeField] private RawImage _rawImage;
        [SerializeField] private VideoPlayer _videoPlayer;
        private VideoClip _clip;
        private RenderTexture _render;
        
        protected override void Start()
        {
            base.Start();
            
            draggable = true;
            origScale = transform.localScale;
        }

        public override void Show()
        {
            base.Show();
            
            if(_sprite != null) Initialize(_sprite);
            else if(_clip != null) Initialize(_clip);
        }

        public override void Hide()
        {
            base.Hide();

            if (_render != null)
            {
                DestroyImmediate(_render);
                _render = null;
            }
        }

        public override void Activate()
        {
            base.Activate();

            if (_clip != null)
            {
                _videoPlayer.Play();
            }
        }

        public override void Deactivate()
        {
            base.Deactivate();

            if (_clip != null)
            {
                _videoPlayer.Pause();
            }
        }

        public void Initialize(Sprite sprite)
        {
            _videoPlayer.gameObject.SetActive(false);
            _rawImage.gameObject.SetActive(false);
            _image.gameObject.SetActive(true);

            _image.sprite = sprite;
            SetAspect(1f * sprite.rect.width / sprite.rect.height);
        }

        public void Initialize(VideoClip clip)
        {
            _videoPlayer.gameObject.SetActive(true);
            _rawImage.gameObject.SetActive(true);
            _image.gameObject.SetActive(false);

            float aspect = 1f * clip.width / clip.height;

            _videoPlayer.clip = clip;
            _videoPlayer.renderMode = VideoRenderMode.RenderTexture;
            _videoPlayer.targetTexture = _render = new RenderTexture(Mathf.FloorToInt(aspect * 240), 240, 0);
            _rawImage.texture = _render;
            _videoPlayer.Play();
            
            SetAspect(aspect);
        }

        void SetAspect(float aspectRatio)
        {
            origScale = transform.localScale;
            transform.localScale = new Vector3(aspectRatio * origScale.x, origScale.y, origScale.z);
        }
    }
}