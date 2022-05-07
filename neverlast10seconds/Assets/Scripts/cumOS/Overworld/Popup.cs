using System;
using cumOS.UIShit;
using UnityEngine;

namespace cumOS.Overworld
{
    public class Popup  : UIWindow
    {
        protected override void Start()
        {
            base.Start();
            
            draggable = true;
        }
    }
}