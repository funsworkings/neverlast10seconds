using UnityEngine;
using UnityEngine.UI;

namespace cumtypebeat
{
    public class CumForcePush : MonoBehaviour
    {
        // Properties

        [SerializeField] private Animator animator;
        [SerializeField] private Image image;
        
        // Attributes

        [SerializeField] private Sprite successImg, failImg;
        [SerializeField] private Color successColor, failColor;
        
        public void Win()
        {
            TriggerCondition(true);   
        }

        public void Lose()
        {
            TriggerCondition(false);
        }

        void TriggerCondition(bool success)
        {
            animator.SetTrigger("show");
            image.sprite = (success) ? successImg : failImg;
            image.color = (success) ? successColor : failColor;
        }
    }
}