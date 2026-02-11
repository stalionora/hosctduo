using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Reporting;

//public class ResourceDataRepresentation {
//	//  representation of data received from server
//}

public class ResourceDataRepresentation {
	public ResourceDataRepresentation(Dictionary<int,string> resourceTable) {
		_resourceTable = resourceTable;
	}
	public ResourceDataRepresentation() { }

	public Dictionary <int, string> ResourceTable { get { return _resourceTable; } }
	public Dictionary<int,CardData> CardStatsTable { get { return _cardStatsTable; } }
	
	//
	private Dictionary<int, string> _resourceTable;
	private Dictionary<int, CardData> _cardStatsTable;

}