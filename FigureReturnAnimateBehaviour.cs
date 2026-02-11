using UnityEngine;

class ReturnAnimateBehaviour : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _moveController = animator.GetComponentInParent<FigureAnimatorController>().MoveController;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float t = stateInfo.normalizedTime;
        t = Mathf.Clamp01(t);
        _moveController.ReturnInFraction(t);
    }

    private HitherThitherAnimationMoveController _moveController;
}