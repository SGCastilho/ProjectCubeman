using UnityEngine;

namespace Cubeman.Enemies
{
    public sealed class EnemyAirShooterAttack : MonoBehaviour
    {
        public delegate void Shoot(string projectileKey, Vector3 instancePoint);
        public event Shoot OnShoot;

        [Header("Classes")]
        [SerializeField] private EnemyAirShooterBehaviour behaviour;

        [Header("Settings")]
        [SerializeField] private Transform[] shootingLeftPoints;
        [SerializeField] private Transform[] shootingRightPoints;
        private Transform _nextShootingPointTransform;
        private int _currentShootingPoint;
        private bool _usingRightPoints;
        private bool _backingCurrentFromPoints;
        private float _distanceFromShootingPoint;

        [Space(12)]

        [SerializeField] private Transform shootPoint;
        [Space(6)]
        [SerializeField] [Range(0.1f, 2f)] private float recoveryDelay = 1f;
        private float _currentRecoveryDelay;

        private bool _canShoot;
        private string _projectileKey;

        private void Awake() => LoadData();

        private void OnEnable() => SetupInitialPoints();

        private void OnDisable() => ResetGameObject();

        private void LoadData()
        {
            _projectileKey = behaviour.DataLoader.Data.Projectile.Key;
        }

        private void ResetGameObject()
        {
            _currentShootingPoint = 0;
            _distanceFromShootingPoint = 0;
            _backingCurrentFromPoints = false;
            SetupInitialPoints();
        }

        private void SetupInitialPoints()
        {
            _usingRightPoints = behaviour.Moviment.StarMoveRight;

            if (_usingRightPoints)
            {
                _nextShootingPointTransform = shootingRightPoints[_currentShootingPoint];
            }
            else
            {
                _nextShootingPointTransform = shootingLeftPoints[_currentShootingPoint];
            }

            _canShoot = true;
        }

        private float CalculateDistanceAbs(float pointA, float pointB)
        {
            return Mathf.Abs(pointA - pointB);
        }

        private void Update() => TravelBetweenShootingPoints();

        private void TravelBetweenShootingPoints()
        {
            if (_canShoot)
            {
                _distanceFromShootingPoint = CalculateDistanceAbs(behaviour.Moviment.EnemyTransform.position.x,
                    _nextShootingPointTransform.position.x);

                if (_distanceFromShootingPoint < 0.1f)
                {
                    behaviour.Moviment.IsMoving = false;

                    if (OnShoot != null) { OnShoot(_projectileKey, shootPoint.position); }

                    _canShoot = false;
                    _distanceFromShootingPoint = 0;
                }
            }
            else
            {
                _currentRecoveryDelay += Time.deltaTime;
                if (_currentRecoveryDelay >= recoveryDelay)
                {
                    behaviour.Moviment.IsMoving = true;
                    _currentRecoveryDelay = 0;
                    NextShootingPoint();
                }
            }
        }

        private void NextShootingPoint()
        {
            if(_backingCurrentFromPoints)
            {
                if (_usingRightPoints)
                {
                    _currentShootingPoint--;
                    if (_currentShootingPoint < 0)
                    {
                        _backingCurrentFromPoints = false;
                        _usingRightPoints = false;
                        return;
                    }
                    _nextShootingPointTransform = shootingRightPoints[_currentShootingPoint];
                }
                else
                {
                    _currentShootingPoint--;
                    if (_currentShootingPoint < 0)
                    {
                        _backingCurrentFromPoints = false;
                        _usingRightPoints = true;
                        return;
                    }
                    _nextShootingPointTransform = shootingLeftPoints[_currentShootingPoint];
                }
            }
            else
            {
                if (_usingRightPoints)
                {
                    _currentShootingPoint++;
                    if (_currentShootingPoint == shootingRightPoints.Length)
                    {
                        _backingCurrentFromPoints = true;
                        return;
                    }
                    _nextShootingPointTransform = shootingRightPoints[_currentShootingPoint];
                }
                else
                {
                    _currentShootingPoint++;
                    if(_currentShootingPoint == shootingLeftPoints.Length)
                    {
                        _backingCurrentFromPoints = true;
                        return;
                    }
                    _nextShootingPointTransform = shootingLeftPoints[_currentShootingPoint];
                }
            }

            _canShoot = true;
        }

        #region Editor Gizmos
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if(shootingLeftPoints != null && shootingRightPoints != null)
            {
                Gizmos.color = Color.yellow;

                for (int i = 0; i < shootingLeftPoints.Length; i++)
                {
                    Gizmos.DrawSphere(shootingLeftPoints[i].position, 0.2f);
                }

                for (int i = 0; i < shootingRightPoints.Length; i++)
                {
                    Gizmos.DrawSphere(shootingRightPoints[i].position, 0.2f);
                }
            }
        }
#endif
        #endregion
    }
}
