using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Assets.SimpleAndroidNotifications
{
    public class NotifScript : MonoBehaviour
    {
        public void SendNotif()
        {
            NotificationManager.SendWithAppIcon(TimeSpan.FromSeconds(2),
                "AR Caring Info Plant Reminder",
                "Hey there ! Don't forget to water your plants !",
                Color.white,
                NotificationIcon.Bell);
        }
    }
}