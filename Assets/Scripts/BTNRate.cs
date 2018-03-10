using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.NativePlugins;

 

public class BTNRate : MonoBehaviour {
    public void Rate () {
        #if UNITY_ANDROID
        Application.OpenURL("market://details?id=BLOX!");
        #elif UNITY_IPHONE
        Application.OpenURL("itms-apps://itunes.apple.com/app/BLOX!");
        #endif    
    }
}
