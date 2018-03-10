using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.NativePlugins;

public class BTNShare : MonoBehaviour {

    public int lastScore;

    public void SetPreviousScore(int score) {
        lastScore = score;
    }

    public void ShareHighScore() {
        if (NPBinding.Sharing.IsMessagingServiceAvailable()) {
            MessageShareComposer _composer = new MessageShareComposer();
            _composer.Body = "Hey! Try to beat my highscore: " + Manager.highScore + " in BLOX! Available now, on both IOS and Android!";
            NPBinding.Sharing.ShowView(_composer);
        }
    }

    public void ShareLastScore()
    {
        if (NPBinding.Sharing.IsMessagingServiceAvailable())
        {
            MessageShareComposer _composer = new MessageShareComposer();
            _composer.Body = "Hey! Try to beat my highscore: " + Manager.highScore + " in BLOX! Available now, on both IOS and Android!";
            NPBinding.Sharing.ShowView(_composer);
        }
    }
}
