using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventSystem
{
    public class EventNames
    {
        // Camera Related
        public const string CAMERA_CHANGE_FOCUS = "CAMERA_CHANGE_FOCUS";
        public const string CAMERA_CLEAR_FOCUS = "CAMERA_CLEAR_FOCUS";
        public const string CAMERA_MOUSE_SWITCH = "CAMERA_MOUSE_SWITCH";
        public const string CAMERA_VIEWMODE_MINIGAME = "CAMERA_VIEWMODE_MINIGAME";
        // User Interface
        public const string SET_UI_PLAYER_REFERENCE = "SET_UI_PLAYER_REFERENCE";
        public const string NOTIFY_PLAYER_INTERACTION = "NOTIFY_PLAYER_INTERACTION";
    }
}
