using cumOS.UIShit;
using UnityEngine;
using UnityEngine.UI;

namespace cumOS.Overworld
{
    public class BrowserUITab : MonoBehaviour
    {
        // Properties
        
        private BrowserWindow _window;

        [SerializeField] private Image thumbnail;
        [SerializeField] private Image background;

        public void Initialize(BrowserWindow window, Sprite thumbnail)
        {
            _window = window;
            
            this.thumbnail.sprite = thumbnail;
        }

        public void SetColor(Color color)
        {
            background.color = color;
        }

        public void Select()
        {
            _window.Select(); // Select window
        }

        public void Close()
        {
            _window.Close();
        }
    }
}