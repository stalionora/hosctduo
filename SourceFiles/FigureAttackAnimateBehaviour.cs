using UnityEngine;

public class AttackAnimateBehaviour: StateMachineBehaviour{
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        _moveController = animator.GetComponentInParent<FigureAnimatorController>().MoveController;
        //_moveController.End = new Vector2(_moveController.Figure.transform.position.x, animator.GetFloat("TargetY"));
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex){
        float t = stateInfo.normalizedTime;
        t = Mathf.Clamp01(t);
        _moveController.MovementByDistanceInFraction(t);
    }


    HitherThitherAnimationMoveController _moveController;
}


