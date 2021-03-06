﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using Global;
using UnityEngine;

namespace Network
{
	public class KSIMEventArgs : EventArgs {
		public string Content { get; set; }

		public KSIMEventArgs(string content, bool macroEvent = false)
		{
			this.Content = content;
		}
	}

	public class KSIMSocket : SocketConnection
	{		
		public event EventHandler ConnectionLost;

		public void OnConnectionLost(object sender, EventArgs e)
		{
			if (ConnectionLost != null)
			{
				ConnectionLost(this, e);
			}
		}

        public void Write(byte[] content) {
			// Check to see if this NetworkStream is writable.
			if (_client.GetStream().CanWrite) {
                byte[] writeBuffer = content;
                if (!BitConverter.IsLittleEndian)
                {
                    Array.Reverse(writeBuffer);
                }

                //using (BinaryWriter w = new BinaryWriter(_client.GetStream(), Encoding.ASCII))
                //{
                //    w.Write(writeBuffer);
                    //w.Write(2);
                //}
                       
				_client.GetStream().Write (writeBuffer, 0, writeBuffer.Length);
                Debug.Log (string.Format("Written to this NetworkStream: {0} ({1})",writeBuffer.Length,Helper.PrintByteArray(writeBuffer)));  
			} 
			else {
				Debug.Log ("Sorry.  You cannot write to this NetworkStream.");  
			}
		}
	}
}