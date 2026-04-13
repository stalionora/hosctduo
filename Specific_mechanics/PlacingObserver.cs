
//	dependencies: PlacingTraits, CardFSM
//	Observer

// traits -> return fsm

public class PlacingObserver {
	//public DirectableCardFSM DirectableKit { get { return _directableFSM; } }
	//public StaticCardFSM StaticCardKit { get { return _staticCardFSM; } }

	public PlacingObserver(DirectableCardFSM directableFSM, StaticCardFSM staticCardFSM) { 
		_directableFSM = directableFSM;
		_staticCardFSM = staticCardFSM;
	}

	public CardFSM GetFSM(PlacingTraits placing) {
		//switch(placing) {
		//	case DirectableTrait trait: {
		//			return _directableFSM;
		//		}
		//	case StaticPlaceableWithAimingTrait trait: {
		//			return _staticCardFSM;
		//		}
		//	default:
		//		throw new System.Exception("No FSM for this placing trait");
		//}
		if(placing is DirectableTrait) {
			return _directableFSM;
		}
		else if(placing is StaticPlaceableTrait) {
			return _staticCardFSM;
		}
		else {
			throw new System.Exception($"No FSM for this placing trait {placing.GetType()}");
		}
	}

	//public CardFSM PlacingKit(StaticPlaceableWithAiming placing) { }
	//public CardFSM PlacingKit(NonPlaceableUsage placing) { }

	DirectableCardFSM _directableFSM;
	StaticCardFSM _staticCardFSM;
}
