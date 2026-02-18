using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class CardResourceService : IService {
	//  ISERVICE INTERFACE
	public void Initialize() {
	}
	//  MAJOR FUNCTIONALITY
	//public void LoadCashFunc(Func<string, int> hashFunc, Vector<int> deck) {
	//	//  SIMULATE RECEIVING DATA FROM SERVER
	//	_hashFunc = hashFunc; 
	//}

	public void SetResourceTable(ResourceDataRepresentation dataFromServer) { 
		_tableData = dataFromServer;
	}

	public void LoadCardsResource(GameObject obj){ 
		//_hashFunc.Invoke();
		//return Resources.Load<Sprite>(_hashFunc.Invoke());
		obj.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>(obj.GetComponent<Card>().Stats.Hash);

	}
	///////////////////////////////
	//  receiving card data from server
	private Func<string, int> _hashFunc;
	private Vector<CardData> _currentCards;
	private ResourceDataRepresentation _tableData;
}

public class CustomResource { 
	
}

public enum CardHash { 
	
}

