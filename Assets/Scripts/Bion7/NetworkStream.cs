using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkStream
{
	bool write = false;
	private List<object> writeData;
	private object[] readData;
	internal byte currentItem = 0; //Used to track the next item to receive.
	
	/// <summary>
	/// Creates a stream and initializes it. Used by PUN internally.
	/// </summary>
	public NetworkStream(bool write, object[] incomingData)
	{
		this.write = write;
		if (incomingData == null)
		{
			this.writeData = new List<object>(10);
		}
		else
		{
			this.readData = incomingData;
		}
	}
	
	/// <summary>If true, this client should add data to the stream to send it.</summary>
	public bool isWriting
	{
		get { return this.write; }
	}
	
	/// <summary>If true, this client should read data send by another client.</summary>
	public bool isReading
	{
		get { return !this.write; }
	}
	
	/// <summary>Count of items in the stream.</summary>
	public int Count
	{
		get
		{
			return (this.isWriting) ? this.writeData.Count : this.readData.Length;
		}
	}
	
	/// <summary>Read next piece of data from the stream when isReading is true.</summary>
	public object ReceiveNext()
	{
		if (this.write)
		{
			Debug.LogError("Error: you cannot read this stream that you are writing!");
			return null;
		}
		
		object obj = this.readData[this.currentItem];
		this.currentItem++;
		return obj;
	}
	
	/// <summary>Read next piece of data from the stream without advancing the "current" item.</summary>
	public object PeekNext()
	{
		if (this.write)
		{
			Debug.LogError("Error: you cannot read this stream that you are writing!");
			return null;
		}
		
		object obj = this.readData[this.currentItem];
		//this.currentItem++;
		return obj;
	}
	
	/// <summary>Add another piece of data to send it when isWriting is true.</summary>
	public void SendNext(object obj)
	{
		if (!this.write)
		{
			Debug.LogError("Error: you cannot write/send to this stream that you are reading!");
			return;
		}
		
		this.writeData.Add(obj);
	}
	
	/// <summary>Turns the stream into a new object[].</summary>
	public object[] ToArray()
	{
		return this.isWriting ? this.writeData.ToArray() : this.readData;
	}
	
	/// <summary>
	/// Will read or write the value, depending on the stream's isWriting value.
	/// </summary>
	public void Serialize(ref bool myBool)
	{
		if (this.write)
		{
			this.writeData.Add(myBool);
		}
		else
		{
			if (this.readData.Length > currentItem)
			{
				myBool = (bool)this.readData[currentItem];
				this.currentItem++;
			}
		}
	}
	
	/// <summary>
	/// Will read or write the value, depending on the stream's isWriting value.
	/// </summary>
	public void Serialize(ref int myInt)
	{
		if (write)
		{
			this.writeData.Add(myInt);
		}
		else
		{
			if (this.readData.Length > currentItem)
			{
				myInt = (int)this.readData[currentItem];
				currentItem++;
			}
		}
	}
	
	/// <summary>
	/// Will read or write the value, depending on the stream's isWriting value.
	/// </summary>
	public void Serialize(ref string value)
	{
		if (write)
		{
			this.writeData.Add(value);
		}
		else
		{
			if (this.readData.Length > currentItem)
			{
				value = (string)this.readData[currentItem];
				currentItem++;
			}
		}
	}
	
	/// <summary>
	/// Will read or write the value, depending on the stream's isWriting value.
	/// </summary>
	public void Serialize(ref char value)
	{
		if (write)
		{
			this.writeData.Add(value);
		}
		else
		{
			if (this.readData.Length > currentItem)
			{
				value = (char)this.readData[currentItem];
				currentItem++;
			}
		}
	}
	
	/// <summary>
	/// Will read or write the value, depending on the stream's isWriting value.
	/// </summary>
	public void Serialize(ref short value)
	{
		if (write)
		{
			this.writeData.Add(value);
		}
		else
		{
			if (this.readData.Length > currentItem)
			{
				value = (short)this.readData[currentItem];
				currentItem++;
			}
		}
	}
	
	/// <summary>
	/// Will read or write the value, depending on the stream's isWriting value.
	/// </summary>
	public void Serialize(ref float obj)
	{
		if (write)
		{
			this.writeData.Add(obj);
		}
		else
		{
			if (this.readData.Length > currentItem)
			{
				obj = (float)this.readData[currentItem];
				currentItem++;
			}
		}
	}
	
	/// <summary>
	/// Will read or write the value, depending on the stream's isWriting value.
	/// </summary>
	public void Serialize(ref Vector3 obj)
	{
		if (write)
		{
			this.writeData.Add(obj);
		}
		else
		{
			if (this.readData.Length > currentItem)
			{
				obj = (Vector3)this.readData[currentItem];
				currentItem++;
			}
		}
	}
	
	/// <summary>
	/// Will read or write the value, depending on the stream's isWriting value.
	/// </summary>
	public void Serialize(ref Vector2 obj)
	{
		if (write)
		{
			this.writeData.Add(obj);
		}
		else
		{
			if (this.readData.Length > currentItem)
			{
				obj = (Vector2)this.readData[currentItem];
				currentItem++;
			}
		}
	}
	
	/// <summary>
	/// Will read or write the value, depending on the stream's isWriting value.
	/// </summary>
	public void Serialize(ref Quaternion obj)
	{
		if (write)
		{
			this.writeData.Add(obj);
		}
		else
		{
			if (this.readData.Length > currentItem)
			{
				obj = (Quaternion)this.readData[currentItem];
				currentItem++;
			}
		}
	}
}
