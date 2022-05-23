using TMPro;
using UnityEngine;

namespace Cubeman.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Screen Message Data", menuName = "Scriptable Object/Dialogue/Screen Message", order = 1)]
    public sealed class ScreenMessageData : ScriptableObject
    {
        #region Encapsulation
        public string Message { get => message; }
        public TMP_SpriteAsset SpriteAsset { get => hotkeysSpriteAsset; }
        #endregion

        [Header("Settings")]
        [SerializeField] private string message = "Put your screen message here.";

        [Space(12)]

        [SerializeField] private TMP_SpriteAsset hotkeysSpriteAsset;
        
        #region Editor Variables
    #if UNITY_EDITOR

        [Space(12)]

        [Header("Editor Settings")]
        [SerializeField] [Multiline(6)] private string devNotes = "Put your dev notes here.";
    #endif
        #endregion
    }
}