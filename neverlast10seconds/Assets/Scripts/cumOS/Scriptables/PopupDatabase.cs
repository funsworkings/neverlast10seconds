using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace cumOS.Scriptables
{
    [CreateAssetMenu(fileName = "New popup database", menuName = "Custom/Popups", order = 0)]
    public class PopupDatabase : ScriptableObject
    {
        public List<Sprite> images = new List<Sprite>();
        public List<VideoClip> clips = new List<VideoClip>();

        public List<MinigameData> minigames = new List<MinigameData>();

        public List<Sprite> cumGif = new List<Sprite>();
    }
}