using Cubeman.Audio;
using UnityEngine;

namespace Cubeman.Player
{
    public sealed class PlayerBehaviour : MonoBehaviour
    {
        #region Encapsulation
        public PlayerStatus Status { get => status; }
        public PlayerInput Input { get => input; }
        public PlayerMoviment Moviment { get => moviment; }
        public PlayerShoot Shoot { get => shoot; }
        internal PlayerAnimation Animation { get => anim; }
        public AudioSoundEffects SoundEffect { get => soundEffects; }

        public Transform Transform { get => _transform; }
        #endregion

        [Header("Classes")]
        [SerializeField] private PlayerStatus status;
        [SerializeField] private PlayerInput input;
        [SerializeField] private PlayerMoviment moviment;
        [SerializeField] private PlayerShoot shoot;
        [SerializeField] private PlayerAnimation anim;

        [Space(12)]

        [SerializeField] private AudioSoundEffects soundEffects;

        private Transform _transform;

        private void Awake() => _transform = transform;

        private void Start() => CursorState(true);

        public void CursorState(bool lockCursor)
        {
            if(lockCursor)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
            }
        }
    }
}
