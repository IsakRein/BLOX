using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.Utility;
using VoxelBusters.DesignPatterns;
using VoxelBusters.UASUtils;
using VoxelBusters.UnityEngineUtils;
using VoxelBusters.NativePlugins;
using System.Diagnostics.Contracts;

public class NotificationManager : MonoBehaviour {

	private CrossPlatformNotification CreateNotification(long _fireAfterSec, eNotificationRepeatInterval.DAY _repeatInterval)
    {
		Contract.Ensures(Contract.Result<CrossPlatformNotification>() != null);
		// Set iOS specific properties
        CrossPlatformNotification.iOSSpecificProperties _iosProperties = new CrossPlatformNotification.iOSSpecificProperties();
        _iosProperties.HasAction = true;
        _iosProperties.AlertAction = "alert action";

        // Set Android specific properties
        CrossPlatformNotification.AndroidSpecificProperties _androidProperties = new CrossPlatformNotification.AndroidSpecificProperties();
        _androidProperties.ContentTitle = "content title";
        _androidProperties.TickerText = "ticker ticks over here";
        _androidProperties.LargeIcon = "NativePlugins.png"; //Keep the files in Assets/PluginResources/VoxelBusters/NativePlugins/Android folder.

        // Create CrossPlatformNotification instance
        CrossPlatformNotification _notification = new CrossPlatformNotification();
        _notification.AlertBody = "alert body"; //On Android, this is considered as ContentText
        _notification.FireDate = System.DateTime.Now.AddSeconds(_fireAfterSec);
        _notification.RepeatInterval = _repeatInterval;
        _notification.UserInfo = _userInfo;
        _notification.SoundName = "Notification.mp3"; //Keep the files in Assets/PluginResources/NativePlugins/Android or iOS or Common folder.

        _notification.iOSProperties = _iosProperties;
        _notification.AndroidProperties = _androidProperties;

        return _notification;
    }
    Schedule the created notification.
    CrossPlatformNotification _notification = CreateNotification(FIRE_AT_SECS, REPEAT_INTERVAL);

    //Schedule this local notification.
    NPBinding.NotificationService.ScheduleLocalNotification(_notification);

}
