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
        private PopupSystem popupMgr;
        private Vector3 origScale;

        [SerializeField] private Image _image;
        private Sprite _sprite;
        
        [SerializeField] private RawImage _rawImage;
        [SerializeField] private VideoPlayer _videoPlayer;
        private VideoClip _clip;
        private RenderTexture _render;
        
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

        private void OnDestroy()
        {
            if (_render != null)
            {
                DestroyImmediate(_render);
                _render = null;
            }
        }

        protected override void Start()
        {
            base.Start();
            
            draggable = true;
            origScale = transform.localScale;
        }

        public override void Show()
        {
            base.Show();

            if (_sprite != null) Initialize(_sprite);
            else if (_clip != null) Initialize(_clip);
        }

        protected override Color GetColor()
        {
            return new Color(1f, 1f, 1f, .67f);
        }

        public override void SetColor(Color color)
        {
            _image.color = _rawImage.color = color;
        }

        public override void Close()
        {
            //play close sound 
            if (popupMgr)
            {
                popupMgr.PopupAudio.PlayRandomSoundRandomPitch(PopupAudio.popupCloseSounds, 1f);
            }
            
            base.Destroy();
        }

        public override void SetActive(bool active)
        {
            base.SetActive(active);

            if (_clip != null)
            {
                if(active) _videoPlayer.Play();
                else _videoPlayer.Pause();
            }
        }

        public void Initialize(Sprite sprite, PopupSystem popupSystem = null)
        {
            popupMgr = popupSystem;
            _videoPlayer.gameObject.SetActive(false);
            _rawImage.gameObject.SetActive(false);
            _image.gameObject.SetActive(true);
            
            //play show sound 
            if (PopupAudio)
            {
                PopupAudio.PlayRandomSoundRandomPitch(PopupAudio.popupOpenSounds, 1f);
            }

            _image.sprite = sprite;
            SetAspect(1f * sprite.rect.width / sprite.rect.height);
        }

        public void Initialize(VideoClip clip, PopupSystem popupSystem = null)
        {
            popupMgr = popupSystem;
            _videoPlayer.gameObject.SetActive(true);
            _rawImage.gameObject.SetActive(true);
            _image.gameObject.SetActive(false);
            
            //play show sound 
            if (PopupAudio)
            {
                PopupAudio.PlayRandomSoundRandomPitch(PopupAudio.popupOpenSounds, 1f);
            }

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