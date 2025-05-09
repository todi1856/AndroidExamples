package com.unity3d.player;


public class Utilities
{
    public static void log(String message){
        UnityPlayer.UnitySendMessage("LogReceiver", "OnLogMessage", message);
    }
}