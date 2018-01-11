using UnityEngine;
using GoogleMobileAds.Api;

public class WatchVideoToContinue : MonoBehaviour
{
    private RewardBasedVideoAd rewardBasedVideo;

    public void Start()
    {
        this.rewardBasedVideo = RewardBasedVideoAd.Instance;
        this.RequestRewardedVideo();
    }

    private void RequestRewardedVideo()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-4536201563869143~8735727650";
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-4536201563869143~2417731319";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded video ad with the request.
        this.rewardBasedVideo.LoadAd(request, adUnitId);
    }
}
