using Cubeman.Audio;
using Cubeman.Enemies;
using Cubeman.Manager;
using Cubeman.Projectile;
using UnityEngine;

namespace Cubeman.AnimationEvents
{
    public sealed class BossGreaterBusterRobotAnimationEvent : MonoBehaviour
    {
        [Header("Classes")]
        [SerializeField] private BossGreaterBusterRobotBehaviour behaviour;

        private AudioController audioController;
        private ObjectPoolingManager poolingManager;

        [Header("Settings")]
        [SerializeField] private Transform shootingPointTransform;
        [SerializeField] private Transform shockWavePointTransform;

        [Space(12)]

        [SerializeField] private ParticleSystem shockWaveSparksParticles;
        [SerializeField] private ParticleSystem deathLaserChargeParticles;

        [Space(12)]

        [SerializeField] private GameObject deathLaserGameObject;

        private const string AUDIO_JUMPIN_KEY = "audio_jumpIn";
        private const string AUDIO_JUMPOUT_KEY = "audio_jumpOut";
        private const string AUDIO_DEATHLASER_OUT = "audio_deathLaser_out";
        private const string AUDIO_DEATHLASER_CHARGE = "audio_deathLaser_charge";

        private string _laserProjectileKey;
        private AudioClip _laserAudioClip;
        private float _laserVolumeScale;

        private string _shockWaveProjectileKey;
        private AudioClip _shockWaveAudioClip;
        private float _shockWaveVolumeScale;

        private AudioClip _deathLaserOutAudioClip;
        private float _deathLaserOutVolumeScale;

        private AudioClip _deathLaserChargeAudioClip;
        private float _deathLaserChargeVolumeScale;

        private void Awake() => SetupObject();

        private void SetupObject()
        {
            CacheComponets();
            CacheData();
        }

        private void CacheComponets()
        {
            poolingManager = ObjectPoolingManager.Instance;
            audioController = AudioController.Instance;
        }

        private void CacheData()
        {
            CacheProjectilesSFX();
            CacheDeathLaserSFX();
        }

        private void CacheProjectilesSFX()
        {
            _laserProjectileKey = behaviour.DataLoader.Data.Projectiles[0].Key;
            _laserAudioClip = behaviour.DataLoader.Data.Projectiles[0].ProjectileSFX;
            _laserVolumeScale = behaviour.DataLoader.Data.Projectiles[0].VolumeScale;

            _shockWaveProjectileKey = behaviour.DataLoader.Data.Projectiles[1].Key;
            _shockWaveAudioClip = behaviour.DataLoader.Data.Projectiles[1].ProjectileSFX;
            _shockWaveVolumeScale = behaviour.DataLoader.Data.Projectiles[1].VolumeScale;
        }

        private void CacheDeathLaserSFX()
        {
            var deathLaserOutSFX = behaviour.SoundEffects.GetSoundEffect(AUDIO_DEATHLASER_OUT);

            _deathLaserOutAudioClip = deathLaserOutSFX._audioClip;
            _deathLaserOutVolumeScale = deathLaserOutSFX._audioVolumeScale;

            var deathLaserChargeSFX = behaviour.SoundEffects.GetSoundEffect(AUDIO_DEATHLASER_CHARGE);

            _deathLaserChargeAudioClip = deathLaserChargeSFX._audioClip;
            _deathLaserChargeVolumeScale = deathLaserChargeSFX._audioVolumeScale;
        }

        public void ShootEvent()
        {
            var projectile = poolingManager.SpawnPrefab(_laserProjectileKey, shootingPointTransform.position)
                .GetComponent<ProjectileBehaviour>();

            projectile.Moviment.MoveRight = behaviour.Movement.MoveRight;

            audioController.PlaySoundEffect(ref _laserAudioClip, _laserVolumeScale);
        }

        public void ShockWaveSparksEvent()
        {
            shockWaveSparksParticles.Play();
        }

        public void ShockWaveStartEvent()
        {
            behaviour.Movement.CrounchEnemy = true;
        }

        public void ShockWaveEvent()
        {
            var projectile = poolingManager.SpawnPrefab(_shockWaveProjectileKey, shockWavePointTransform.position)
                .GetComponent<ProjectileBehaviour>();

            projectile.Moviment.MoveRight = behaviour.Movement.MoveRight;

            audioController.PlaySoundEffect(ref _shockWaveAudioClip, _shockWaveVolumeScale);
        }

        public void ShockWaveEndEvent()
        {
            behaviour.Movement.CrounchEnemy = false;
        }

        public void JumpInEvent()
        {
            var jumpInSFX = behaviour.SoundEffects.GetSoundEffect(AUDIO_JUMPIN_KEY);
            audioController.PlaySoundEffect(ref jumpInSFX._audioClip, jumpInSFX._audioVolumeScale);
        }

        public void JumpOutEvent()
        {
            var jumpOutSFX = behaviour.SoundEffects.GetSoundEffect(AUDIO_JUMPOUT_KEY);
            audioController.PlaySoundEffect(ref jumpOutSFX._audioClip, jumpOutSFX._audioVolumeScale);
        }

        public void DeathLaserStartChargeEvent()
        {
            audioController.PlaySoundEffect(ref _deathLaserChargeAudioClip, _deathLaserChargeVolumeScale);
            deathLaserChargeParticles.Play();
        }

        public void DeathLaserEndChargeEvent()
        {
            deathLaserChargeParticles.Stop();
        }

        public void DeathLaserStartEvent()
        {
            audioController.PlaySoundEffect(ref _deathLaserOutAudioClip, _deathLaserOutVolumeScale);
            deathLaserGameObject.SetActive(true);
        }

        public void DeathLaserEndEvent()
        {
            deathLaserGameObject.SetActive(false);
        }
    }
}