using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace cumOS.UIShit
{
    public class UIWindow : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public static System.Action<UIWindow> onSelectWindow, onReleaseWindow, onCloseWindow;
        
        // Properties

        public UIWindowManager manager;

        protected RectTransform _rectTransform;
        private Image background;
        public virtual Color color => background.color;
        
        [SerializeField] private RectTransform headerBar;
        [SerializeField] private Button closeButton;

        private bool dragging = false;
        Vector3 pointerDragOffset = Vector2.zero;

        private bool active = false;
        public bool IsActive => active;
        
        // Attributes

        public bool draggable = true;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            background = GetComponent<Image>();
        }

        protected virtual void Start()
        {
            if (closeButton)
            {
                closeButton.onClick.AddListener(() =>
                {
                     // Close window functionality
                     Debug.Log($"Did close window -> {gameObject.name}");
                     Close();
                });
            }
        }

        public virtual void Bind(UIWindowManager manager)
        {
            this.manager = manager;
            SetColor(Random.ColorHSV());
        }

        public virtual void SetColor(Color color)
        {
            background.color = color;
        }

        private void Update()
        {
            if (!dragging) return;

            if (Input.GetMouseButtonUp(0))
            {
                dragging = false; // Release input
            }
            else // Still active input
            {
                _rectTransform.position = Input.mousePosition - pointerDragOffset;
            }
        }

        #region UI interactions

        public virtual void Drag()
        {
            if (!draggable) return; // Ignore request to drag item
            
            dragging = true;
            pointerDragOffset = Input.mousePosition - _rectTransform.position;
        }

        public virtual void Release()
        {
            dragging = false;
        }

        #endregion
        
        #region Virtual methods

        public virtual void Select()
        {
            onSelectWindow?.Invoke(this);
        }

        public virtual void Deselect()
        {
            onReleaseWindow?.Invoke(this);
        }

        public virtual void Close()
        {
            onCloseWindow?.Invoke(this);
        }
        
        #endregion
        
        #region Sorting

        public virtual void Show(){}
        public virtual void Hide(){}

        public virtual void Activate()
        {
            active = true;
        }

        public virtual void Deactivate()
        {
            active = false;
        }
        
        public virtual void OnVisible(){}
        public virtual void OnHidden(){}

        #endregion
        
        #region Native UI callbacks

        public void OnPointerDown(PointerEventData eventData)
        {
            Select();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Deselect();
        }
        
        #endregion
    }
}