using UnityEngine;

namespace Cubeman.Enemies
{
    public class GreaterBR_JumpBackRepositionState : State
    {
        [Header("Classes")]
        [SerializeField] private BossGreaterBusterRobotBehaviour behaviour;

        [Header("Settings")]
        [SerializeField] private State nextState;

        [Space(12)]

        [SerializeField] [Range(10f, 40f)] private float jumpForce = 20f;
        [SerializeField] [Range(0.1f, 1f)] private float jumpDelay = 0.4f;
        [SerializeField] [Range(0.1f, 1f)] private float groundCheckTick = 0.4f;

        [Space(6)]

        [SerializeField] [Range(0.1f, 6f)] private float minDistanceFromPlayer = 4f;

        private bool _isJumped;
        private bool _jumpedTriggered;
        private float _currentJumpDelay;
        private float _currentDistanceFromPlayer;
        private float _currentGroundCheckTick;

        #region Editor Variable
    #if UNITY_EDITOR
        [SerializeField] private bool _showMinDistanceGizmos;
    #endif
        #endregion

        public override State RunCurrentState()
        {
            if(!_isJumped)
            {
                if(behaviour.CheckWallInFront.HasWallInBackwords(behaviour.Moviment.MoveRight))
                {
                    EndState();

                    return nextState;
                }

                _currentDistanceFromPlayer = behaviour.CheckPlayerDistance.PlayerDistance();
                if(_currentDistanceFromPlayer > minDistanceFromPlayer)
                {
                    EndState();

                    return nextState;
                }
            }

            Jump();

            if (_isJumped)
            {
                _currentGroundCheckTick += Time.deltaTime;
                if(_currentGroundCheckTick >= groundCheckTick)
                {
                    if (behaviour.Moviment.Gravity.IsGrounded)
                    {
                        EndState();

                        return nextState;
                    }

                    _currentGroundCheckTick = 0;
                }
            }

            return this;
        }

        private void EndState()
        {
            behaviour.Moviment.IsMoving = false;
            
            _isJumped = false;
            _jumpedTriggered = false;
            _currentDistanceFromPlayer = 0;
            _currentGroundCheckTick = 0;

            behaviour.Sequencer.NextSequence();
        }

        private void Jump()
        {
            if (!_isJumped)
            {
                if (!_jumpedTriggered)
                {
                    behaviour.ExclusiveAnimator.CallAnimationTrigger("jump");
                    _jumpedTriggered = true;
                }

                _currentJumpDelay += Time.deltaTime;
                if(_currentJumpDelay >= jumpDelay)
                {
                    _isJumped = true;

                    behaviour.Moviment.IsMoving = true;
                    behaviour.Moviment.MoveRightNoFlipEnemy = !behaviour.Moviment.MoveRightNoFlipEnemy;
                    behaviour.Moviment.Gravity.Jump(jumpForce);

                    _currentJumpDelay = 0;
                }
            }
        }

        #region Editor Function
    #if UNITY_EDITOR
        private void OnDrawGizmosSelected() 
        {
            if(_showMinDistanceGizmos)
            {
                Gizmos.color = Color.red;

                Gizmos.DrawWireSphere(transform.position, minDistanceFromPlayer);
            }
        }
    #endif
        #endregion
    }
}
