using UnityEngine;

public class FigureAnimatorController : MonoBehaviour{
    public Vector3 WorldTargetPosition { get; set; }
    public HitherThitherAnimationMoveController MoveController { get { return _moveController; } }

    void Awake(){
        _animator = GetComponent<Animator>();
        //_rect = GetComponent<RectTransform>();
    }
    public void SetTargetPosition(Vector3 TargetPosition){
        _moveController.Set(transform.gameObject, transform.position, WorldTargetPosition);
        _moveController.SetRect(transform.gameObject, GetComponent<RectTransform>().anchoredPosition, TargetPosition);
    }
    public void PlayAttackAnimation(){
        //Vector2 localPoint;
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(_rect, RectTransformUtility.WorldToScreenPoint(null, WorldTargetPosition), null, out localPoint);

        _moveController.Start = transform.position;
        _moveController.StartRect = GetComponent<RectTransform>().anchoredPosition;
        _animator.SetTrigger("Attack");
    }

    public void PlayReturnAnimation(){
        //Vector2 localPoint;
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(_rect, RectTransformUtility.WorldToScreenPoint(null, WorldTargetPosition), null, out localPoint);
        _animator.SetTrigger("Return");
    }
    public void PlayIdleAnimation(){
        //Vector2 localPoint;
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(_rect, RectTransformUtility.WorldToScreenPoint(null, WorldTargetPosition), null, out localPoint);
        _animator.SetTrigger("Idle");
    }

    public void MoveOnAttackingAnimation(Vector3 distance){
        transform.position += distance;
    }

    private Animator _animator;
    private HitherThitherAnimationMoveController _moveController = new();
    //private Vector3 _originalPosition;
    //private RectTransform _rect;
}

public class HitherThitherAnimationMoveController
{
    public void Set(GameObject figure, Vector3 start, Vector3 end){
        Figure = figure;
        Start = start;
        End = end;
    }

    public void SetRect(GameObject figure, Vector3 startRect, Vector3 endRect){
        Figure = figure;
        StartRect = startRect;
        EndRect = endRect;
    }

    public void ReturnInFraction(float timeFraction){
        Figure.transform.position = Vector3.Lerp(End, Start, timeFraction);
        //Figure.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(EndRect, StartRect, timeFraction);
    }

    public void MovementByDistanceInFraction(float timeFraction){
        Figure.transform.position = Vector3.Lerp(Start, End, timeFraction);
        //Figure.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(StartRect, EndRect, timeFraction);
    }

    public GameObject Figure { get; set; }
    public Vector3 Start { get; set; }
    public Vector3 StartRect { get; set; }
    public Vector3 End { get; set; }
    public Vector3 EndRect { get; set; }
}