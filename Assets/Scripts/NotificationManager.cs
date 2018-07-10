using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using VoxelBusters.NativePlugins;

public class NotificationManager : MonoBehaviour
{
    string _message;
    private NotificationType m_notificationType;

    private void Start()
    {
        setPushNotif(28800);
    }

    public void setPushNotif(int timer)
    {
        _message = "Come back to BLOX! and try to beat your high score of: " + Manager.highScore + "!";

        CancelAllLocalNotifications();
        ClearNotifications();
        RegisterNotificationTypes(m_notificationType);
        EnabledNotificationTypes();
        ScheduleLocalNotification(CreateNotification(timer, eNotificationRepeatInterval.DAY));
    }

    private void RegisterNotificationTypes(NotificationType _notificationTypes)
    {
        NPBinding.NotificationService.RegisterNotificationTypes(_notificationTypes);
    }

    private NotificationType EnabledNotificationTypes()
    {
        return NPBinding.NotificationService.EnabledNotificationTypes();
    }

    private string ScheduleLocalNotification(CrossPlatformNotification _notification)
    {
        return NPBinding.NotificationService.ScheduleLocalNotification(_notification);
    }

    private void CancelAllLocalNotifications()
    {
        NPBinding.NotificationService.CancelAllLocalNotification();
    }

    private void ClearNotifications()
    {
        NPBinding.NotificationService.ClearNotifications();
    }

    private CrossPlatformNotification CreateNotification(long _fireAfterSec, eNotificationRepeatInterval _repeatInterval)
    {
        // User info
        IDictionary _userInfo = new Dictionary<string, string>();
        _userInfo["data"] = "custom data";

        CrossPlatformNotification.iOSSpecificProperties _iosProperties = new CrossPlatformNotification.iOSSpecificProperties();
        _iosProperties.HasAction = true;
        _iosProperties.AlertAction = "alert action";

        CrossPlatformNotification.AndroidSpecificProperties _androidProperties = new CrossPlatformNotification.AndroidSpecificProperties();
        _androidProperties.ContentTitle = "Beat your high score!";
        _androidProperties.TickerText = _message;
        _androidProperties.LargeIcon = "BLOX128.png"; //Keep the files in Assets/PluginResources/Android or Common folder.

        CrossPlatformNotification _notification = new CrossPlatformNotification();
        _notification.AlertBody = _message; //On Android, this is considered as ContentText
        _notification.FireDate = System.DateTime.Now.AddSeconds(_fireAfterSec);
        _notification.RepeatInterval = _repeatInterval;
        _notification.SoundName = "Notification.wav"; //Keep the files in Assets/PluginResources/Android or iOS or Common folder.
        _notification.UserInfo = _userInfo;
        _notification.iOSProperties = _iosProperties;
        _notification.AndroidProperties = _androidProperties;

        return _notification;
    }

}