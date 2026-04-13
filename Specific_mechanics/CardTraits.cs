
////////////////////////////////////////////////////////////////////
//	no dependencies
////////////////////////////////////////////////////////////////////
//	типы кард возвращаю action


//------------------------------------------------------------------
//	draggable, placeable, placable with aiming
//  +
//  
public interface PlacingTraits{}

public struct DirectableTrait : PlacingTraits {}

public struct StaticPlaceableTrait : PlacingTraits {}

public struct StaticPlaceableWithAimingTrait : PlacingTraits {}

public struct NonPlaceableUsageTrait : PlacingTraits {}

//------------------------------------------------------------------
public interface CardMechanicTraits {}

public class AttackingTrait: CardMechanicTraits {}

public class DeferredTrait: CardMechanicTraits {}

public class EffectTrait : CardMechanicTraits {}
//------------------------------------------------------------------
//	CARD TYPES
//------------------------------------------------------------------

public abstract class CardType {
	public abstract CardMechanicTraits[] GetMechanics();
	public abstract PlacingTraits GetPlacingTrait();	
}

//	with turn dependent effect or trigger
public class StaticMobCard: CardType {
	public override CardMechanicTraits[] GetMechanics() {
		return _mechanics;
	}

	public override PlacingTraits GetPlacingTrait() {
		return _placing;
	}

	//
	private CardMechanicTraits[] _mechanics = new CardMechanicTraits[] { new DeferredTrait() } ;
	private StaticPlaceableTrait _placing = new StaticPlaceableTrait();
}

//should be directed in enemy base
public class DirectableMobCard: CardType{
	public override CardMechanicTraits[] GetMechanics() {
		throw new System.NotImplementedException();
	}

	public override PlacingTraits GetPlacingTrait() {
		return _placing;
	}

	private CardMechanicTraits[] _mechanics;
	private DirectableTrait _placing;
}

public class BuildingCard : CardType
{
    public override CardMechanicTraits[] GetMechanics()
    {
        throw new System.NotImplementedException();
    }

    public override PlacingTraits GetPlacingTrait()
    {
        throw new System.NotImplementedException();
    }
}


//////////////////
public class IntersectionCard : CardType
{
	public override CardMechanicTraits[] GetMechanics()
	{
		throw new System.NotImplementedException();
	}

	public override PlacingTraits GetPlacingTrait()
	{
		throw new System.NotImplementedException();
	}
}

//////////////////
public class EnvironmentCard : CardType
{
    public override CardMechanicTraits[] GetMechanics()
    {
        throw new System.NotImplementedException();
    }

    public override PlacingTraits GetPlacingTrait()
    {
        throw new System.NotImplementedException();
    }
}

public class EffectCard : CardType
{
	public override CardMechanicTraits[] GetMechanics()
	{
		throw new System.NotImplementedException();
	}

	public override PlacingTraits GetPlacingTrait()
	{
		throw new System.NotImplementedException();
	}
}
