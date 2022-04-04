using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipCollection : MonoBehaviour
{
    public static AudioClipCollection instance;

    public AudioClip shuffleSFX;
    public AudioClip turnSFX;
    public AudioClip chipsDropSFX;
    public AudioClip checkSFX;
    public AudioClip foldSFX;
    public AudioClip betChipDraggingSFX;
    public AudioClip communityCardOpenSFX;
    public AudioClip alarmsound; 

    private void Awake()
    {
        instance = this;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        //SoundManager.instance.PlayPomeSound(AudioClipCollection.instance.checkSFX);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
