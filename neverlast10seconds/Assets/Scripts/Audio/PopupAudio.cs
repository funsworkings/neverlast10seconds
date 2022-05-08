using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupAudio : AudioHandler
{
    [Header("Popup Audio Settings")]
    public AudioClip[] popupOpenSounds;
    public AudioClip[] popupCloseSounds;
    public AudioClip[] popupClickSounds;
}
