using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SettingsData", menuName = "SizeRumble/SettingsData")]
public class SettingsData : ScriptableObject
{
    public bool enableWallJump;
    public bool enableDoubleJump = true;
}