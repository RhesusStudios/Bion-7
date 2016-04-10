using UnityEngine;
using System.Collections;

public class NetworkUtilities
{
	// Private Constructor
	private NetworkUtilities () {}

	public static void Serialize (ref BitStream stream, ref Vector2 obj)
	{
		stream.Serialize (ref obj.x);
		stream.Serialize (ref obj.y);
	}

	public static void Serialize (ref BitStream stream, ref Vector3 obj)
	{
		stream.Serialize (ref obj.x);
		stream.Serialize (ref obj.y);
		stream.Serialize (ref obj.z);
	}

	public static void Serialize (ref BitStream stream, ref Color obj)
	{
		stream.Serialize (ref obj.r);
		stream.Serialize (ref obj.g);
		stream.Serialize (ref obj.b);
		stream.Serialize (ref obj.a);
	}
}
