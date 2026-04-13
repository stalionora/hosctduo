using System.Collections.Generic;
using UnityEditor;

//public class ResourceDataRepresentation {
//	//  representation of data received from server
//}

public class ResourceDataRepresentation {
	public ResourceDataRepresentation(Dictionary<int,string> resourceTable) {
		_resourceTable = resourceTable;
	}
	//public ResourceDataRepresentation() { }

	public Dictionary <int, string> ResourceTable { get { return _resourceTable; } }
	//public Dictionary<int,CardDataImpl> CardStatsTable { get { return _cardStatsTable; } }
	
	//
	private Dictionary<int, string> _resourceTable;
	private Dictionary<int, CardDataImpl> _cardStatsTable;

}