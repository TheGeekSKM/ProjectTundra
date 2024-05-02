using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EntityAnimationController : MonoBehaviour
{
    [SerializeField] Animator _animator;
    public Animator Animator => _animator;

    [SerializeField] EntityStatsContainer _entityStatsContainer;

    #region Player Animation Hashes
    private static readonly int RangerIdle = Animator.StringToHash("RangerIdle");
    private static readonly int RangerWalk = Animator.StringToHash("RangerWalk");
    private static readonly int RangerAttack = Animator.StringToHash("RangerAttack");

    private static readonly int ScoutIdle = Animator.StringToHash("ScoutIdle");
    private static readonly int ScoutAttack = Animator.StringToHash("ScoutAttack");

    private static readonly int MageIdle = Animator.StringToHash("MageIdle");
    private static readonly int MageAttack = Animator.StringToHash("MageAttack");
    #endregion

    #region Enemy Animation Hashes
    private static readonly int SlimeIdle = Animator.StringToHash("SlimeIdle");
    private static readonly int SlimeAttack = Animator.StringToHash("SlimeAttack");

    private static readonly int SkeletonIdle = Animator.StringToHash("SkeletonIdle");
    private static readonly int SkeletonAttack = Animator.StringToHash("SkeletonAttack");

    private static readonly int BatIdle = Animator.StringToHash("BatIdle");
    private static readonly int BatAttack = Animator.StringToHash("BatAttack");
    #endregion

    void Awake()
    {
        if (_animator == null) _animator = GetComponent<Animator>();
        // look for the EntityStatsContainer component in the parent GameObject
        if (_entityStatsContainer == null) _entityStatsContainer = GetComponentInParent<EntityStatsContainer>();
    }

    void Start()
    {
        
        
    }

    public void Initialize()
    {
        // set the animator's controller to the one in the EntityStatsContainer
        _animator.runtimeAnimatorController = _entityStatsContainer.PlayerStatsData.AnimatorController;
        HandleAnimation(AnimType.Idle);
    }

    void HandleAnimation(AnimType animType)
    {
        #region Idle Animation
        if (animType == AnimType.Idle)
        {
            if (_entityStatsContainer.PlayerStatsData.EntityType == EntityType.Player)
            {
                switch (_entityStatsContainer.PlayerStatsData.PlayerClass)
                {
                    case PlayerClass.Ranger:
                        _animator.CrossFade(RangerIdle, 0, 0);
                        break;
                    case PlayerClass.Scout:
                        _animator.CrossFade(ScoutIdle, 0, 0);
                        break;
                    case PlayerClass.Mage:
                        _animator.CrossFade(MageIdle, 0, 0);
                        break;
                }
            }
            else
            {
                switch (_entityStatsContainer.PlayerStatsData.EnemyClass)
                {
                    case EnemyClass.Slime:
                        _animator.CrossFade(SlimeIdle, 0, 0);
                        break;
                    case EnemyClass.Skeleton:
                        _animator.CrossFade(SkeletonIdle, 0, 0);
                        break;
                    case EnemyClass.Bat:
                        _animator.CrossFade(BatIdle, 0, 0);
                        break;
                }
            }

            return;
        }
        #endregion

        #region Walk Animation
        if (animType == AnimType.Walk)
        {
            if (_entityStatsContainer.PlayerStatsData.EntityType == EntityType.Player)
            {
                switch (_entityStatsContainer.PlayerStatsData.PlayerClass)
                {
                    case PlayerClass.Ranger:
                        _animator.CrossFade(RangerWalk, 0, 0);
                        break;
                    case PlayerClass.Scout:
                        _animator.CrossFade(ScoutIdle, 0, 0);
                        break;
                    case PlayerClass.Mage:
                        _animator.CrossFade(MageIdle, 0, 0);
                        break;
                }
            }
            else
            {
                switch (_entityStatsContainer.PlayerStatsData.EnemyClass)
                {
                    case EnemyClass.Slime:
                        _animator.CrossFade(SlimeIdle, 0, 0);
                        break;
                    case EnemyClass.Skeleton:
                        _animator.CrossFade(SkeletonIdle, 0, 0);
                        break;
                    case EnemyClass.Bat:
                        _animator.CrossFade(BatIdle, 0, 0);
                        break;
                }
            }

            return;
        }
        #endregion

        #region Attack Animation
        if (animType == AnimType.Attack)
        {
            if (_entityStatsContainer.PlayerStatsData.EntityType == EntityType.Player)
            {
                switch (_entityStatsContainer.PlayerStatsData.PlayerClass)
                {
                    case PlayerClass.Ranger:
                        _animator.CrossFade(RangerAttack, 0, 0);
                        break;
                    case PlayerClass.Scout:
                        _animator.CrossFade(ScoutAttack, 0, 0);
                        break;
                    case PlayerClass.Mage:
                        _animator.CrossFade(MageAttack, 0, 0);
                        break;
                }
            }
            else
            {
                switch (_entityStatsContainer.PlayerStatsData.EnemyClass)
                {
                    case EnemyClass.Slime:
                        _animator.CrossFade(SlimeAttack, 0, 0);
                        break;
                    case EnemyClass.Skeleton:
                        _animator.CrossFade(SkeletonAttack, 0, 0);
                        break;
                    case EnemyClass.Bat:
                        _animator.CrossFade(BatAttack, 0, 0);
                        break;
                }
            }

            return;
        }
        #endregion
        
    }

}
public enum AnimType
{
    Idle,
    Walk,
    Attack
}
