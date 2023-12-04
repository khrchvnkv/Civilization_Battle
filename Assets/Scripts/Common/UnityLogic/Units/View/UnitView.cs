using UnityEngine;

namespace Common.UnityLogic.Units.View
{
    public sealed class UnitView : MonoBehaviour
    {
        private const string IsMoving_AnimatorBool = "IsMoving";
        private const string Die_AnimatorTrigger = "Die";
        private const string Attack_AnimatorTrigger = "Attack";
        private const string Hit_AnimatorTrigger = "Hit";

        private readonly int IsMoving_AnimatorBool_Hash = Animator.StringToHash(IsMoving_AnimatorBool);
        private readonly int Die_AnimatorTrigger_Hash = Animator.StringToHash(Die_AnimatorTrigger);
        private readonly int Attack_AnimatorTrigger_Hash = Animator.StringToHash(Attack_AnimatorTrigger);
        private readonly int Hit_AnimatorTrigger_Hash = Animator.StringToHash(Hit_AnimatorTrigger);
        
        [SerializeField] private Animator _animator;
        [SerializeField] private GameObject _availableRangeView;
        [SerializeField] private ParticleSystem _selectedUnitParticle;

        public void SetAvailableRangeActivity(in bool isActive) => _availableRangeView.SetActive(isActive);
        public void SetSelectedViewActivity(in bool isActive) => _selectedUnitParticle.gameObject.SetActive(isActive);
        public void PlayMoveAnimation() => _animator.SetBool(IsMoving_AnimatorBool_Hash, true);
        public void PlayIdleAnimation() => _animator.SetBool(IsMoving_AnimatorBool_Hash, false);
        public void PlayAttackAnimation() => _animator.SetTrigger(Attack_AnimatorTrigger_Hash);
        public void PlayHitAnimation() => _animator.SetTrigger(Hit_AnimatorTrigger_Hash);
        public void PlayDieAnimation() => _animator.SetTrigger(Die_AnimatorTrigger_Hash);
    }
}