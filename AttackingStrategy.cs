public abstract class BaseAttackSetter {
    //public abstract void ChooseTarget(PlayerController ally, PlayerController enemy);
    public virtual void Attack(int damage) { 
        Target.ActionOfTakingDamage(damage);
    }
   
    public PlayerController Target { get; set; }
}


public class EnemyAttackSetter : BaseAttackSetter{
    
    public EnemyAttackSetter(PlayerController target) {
        Target = target;
    }
    //public override void ChooseTarget(PlayerController ally, PlayerController enemy){
    //    Target = ally;
    //}
    
}


public class AllyAttackSetter : BaseAttackSetter{
    public AllyAttackSetter(PlayerController target) {
        Target = target; 
    }
    //public override void ChooseTarget(PlayerController ally, PlayerController enemy){
    //    Target = enemy;
    //}
}