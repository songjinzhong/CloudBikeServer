using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This class that represents a TCP connection with the client.
/// </summary>

public class ClientTCP : IClient
{
    private Socket clientSocket;

    private NetworkStream stream;
    private BinaryWriter writer;
    private BinaryReader reader;
	private byte[] speedFlag;

    public ClientTCP(Socket socket)
    {
        clientSocket = socket;
        init(clientSocket);
    }
	BikeInput bi;
    private void init(Socket clientSocket)
    {
		speedFlag = System.Text.Encoding.Default.GetBytes ("s");
        try
        {
            stream = new NetworkStream(clientSocket);
            writer = new BinaryWriter(stream);
            reader = new BinaryReader(stream);

            UnityEngine.Debug.Log("ClientConnection to: " + clientSocket.RemoteEndPoint.ToString() + " ready ");
        }
        catch (Exception)
        {
            Debug.Log("can't create streams!");
            throw new IOException("can't create streams!");
        }
		bi = new BikeInput ();
		bi.Speed = 0f;
		bi.Turn = 0f;
    }

    /// <summary>
    /// Read the screen resolution of the client.
    /// </summary>
    public int[] readScreenResolution()
    {
        try
        {
            int[] screenResolution = new int[2];
            screenResolution[0] = IPAddress.NetworkToHostOrder(reader.ReadInt32());
            screenResolution[1] = IPAddress.NetworkToHostOrder(reader.ReadInt32());

            return screenResolution;
        }
        catch (Exception)
        {
            disconnect();
            throw new IOException("Client disconnected");
        }
    }

    public void sendImage(byte[] data)
    {
        try
        {
            writer.Write(IPAddress.HostToNetworkOrder(data.Length));
            writer.Write(data);
        }
        catch (Exception)
        {
            disconnect();
            throw new IOException("Client disconnected");
        }
    }

	public void sendSpeed(byte[] data){
		try
		{
			//writer.Write(IPAddress.HostToNetworkOrder(data.Length));
//			Debug.Log(System.Text.Encoding.Default.GetString(speedFlag));
			Debug.Log(speedFlag.Length);
			Debug.Log(speedFlag[0]);
			writer.Write(IPAddress.HostToNetworkOrder(data.Length + 1));
			writer.Write(speedFlag);
			writer.Write(data);
		}
		catch (Exception)
		{
			disconnect();
			throw new IOException("Client disconnected");
		}
	}

    public g_Input readInput()
    {
        try
        {
            byte[] input = reader.ReadBytes(1 + (4 * 4));
//			if(input[0] == 0)
//				return IOUtils.handleInput(input);
//			Debug.Log("begin");
//			for(var i = 0; i < input.Length; i++){
//				Debug.Log(input[i]);
//			}
//			Debug.Log("end");
            return IOUtils.handleInput(input);
        }
        catch (Exception)
        {
            disconnect();
            throw new IOException("Client disconnected");
        }
    }
	public g_Input readInput3()
	{
		try
		{
			byte start = reader.ReadByte();
			while(start != 97){
				start = reader.ReadByte();
			}
			byte[] input = reader.ReadBytes(4);

			bi = (BikeInput)(IOUtils.handleInput(input));
			return bi;
		}
		catch (Exception)
		{
			disconnect();
			throw new IOException("Client disconnected");
		}
	}

	public g_Input readPeopleInput()
	{
		try
		{
			PeopleInput pi = new PeopleInput();
			byte start = reader.ReadByte();
			while(start != 47){
				start = reader.ReadByte();
			}
			List<byte> ls = new List<byte>();
			start = reader.ReadByte();
			while(start != 47){
				ls.Add(start);
				start = reader.ReadByte();
			}
			float heartRate = 0f;
			int len = ls.Count;
			for(int i = len - 1; i >= 0; i--){
				heartRate += (float)((ls[i] - 48) * Math.Pow(10, len - i - 1));
			}
			pi.HeartRate = heartRate;
			ls = new List<byte>();
			start = reader.ReadByte();
			while(start != 97){
				ls.Add(start);
				start = reader.ReadByte();
			}
			float oxygen = 0f;
			len = ls.Count;
			for(int i = len - 1; i >= 0; i--){
				oxygen += (float)((ls[i] - 48) * Math.Pow(10, len - i - 1));
			}
			pi.Oxygen = oxygen;
//			bi = (BikeInput)(IOUtils.handleInput(input));
			return pi;
		}
		catch (Exception)
		{
			disconnect();
			throw new IOException("Client disconnected");
		}
	}

    public void disconnect()
    {
        if (clientSocket.Connected)
        {
            clientSocket.Close();

            Debug.Log("Closed connection with client");
        }
    }
}