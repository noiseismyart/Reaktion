/*
* UniOSC
* Copyright © 2014-2015 Stefan Schlupek
* All rights reserved
* info@monoflow.org
*/
using UnityEngine;
using System.Collections;

using System;
using System.Collections.Generic;
using System.Net.Sockets;

using OSCsharp.Data;
using OSCsharp.Net;
using OSCsharp.Utils;

namespace UniOSC{

	//Based on: https://github.com/valyard/TUIOsharp/blob/master/TUIOsharp/TuioServer.cs
	//https://github.com/valyard/OSCsharp

	/// <summary>
	/// Uni OSC receiver.
	/// </summary>
	public class UniOSCReceiver  {

		#region Public 

		public int Port { get; private set; }
		public int FrameNumber { get; private set; }

		#endregion Public


		#region Private 
		
		private UDPReceiver udpReceiver;
		
		#endregion Private


		#region Events

		public event EventHandler<UniOSCEventArgs> OSCMessageReceived;
		public event EventHandler<ExceptionEventArgs> OSCErrorOccured;

		#endregion Events


		#region Constructors
		
		public UniOSCReceiver() : this(3333,null)
		{}

        public UniOSCReceiver(int port, string MulticastAddress)
		{
			Port = port;
            if (MulticastAddress == null) 
            {
                udpReceiver = new UDPReceiver(Port, false);
            } 
            else 
            {
                //IPAddress mAddress = IPAddress.Parse(MulticastAddress);
                //udpReceiver = new UDPReceiver(port, mAddress, false);
            }

			udpReceiver = new UDPReceiver(Port, false);
			udpReceiver.MessageReceived += handlerOscMessageReceived;
			udpReceiver.ErrorOccured += handlerOscErrorOccured;
			udpReceiver.BundleReceived += handlerOSCBundleReceived;
		}
		
		#endregion

		
		#region Public methods
		/// <summary>
		/// Connect this instance.
		/// </summary>
		public bool Connect()
		{
			//Debug.Log("OSCReceiver.Connect:"+Port);
			try{
				if (!udpReceiver.IsRunning)udpReceiver.Start();
				return true;
			}catch (SocketException e){
				Debug.LogWarning(String.Format("The connection on port: {0} could not be established!\n{1}",Port,e.ToString()));
				return false;
			}catch (ArgumentOutOfRangeException e){
				Debug.LogWarning(String.Format("The connection on port: {0} could not be established!Invalid Port Number.\n{1}",Port,e.ToString()));
				return false;
			}
		}

		/// <summary>
		/// Disconnect this instance.
		/// </summary>
		public void Disconnect()
		{
			//Debug.Log("UniOSCReceiver.Disconnect:"+udpReceiver.IsRunning);
			if (udpReceiver.IsRunning) {udpReceiver.Stop();}
		}
		
		#endregion


		#region Private functions
		
		private void parseOscMessage(OscMessage message)
		{

			if( OSCMessageReceived != null) OSCMessageReceived(this, new UniOSCEventArgs(Port,message )) ;

		}


		private void parseOscBundle(OscBundle bundle){

			foreach(var message in bundle.Messages){
				parseOscMessage(message);
			}

			if(bundle.Bundles.Count >0){
				foreach(var _bundle in bundle.Bundles){
					parseOscBundle(_bundle);
				}
			}
		}

		#endregion


		#region Event handlers
		
		private void handlerOscErrorOccured(object sender, ExceptionEventArgs exceptionEventArgs)
		{
			if( OSCErrorOccured != null) OSCErrorOccured(this, exceptionEventArgs);
		}
		
		private void handlerOscMessageReceived(object sender, OscMessageReceivedEventArgs oscMessageReceivedEventArgs)
		{
			parseOscMessage(oscMessageReceivedEventArgs.Message);
		}


		private void handlerOSCBundleReceived(object sender, OscBundleReceivedEventArgs  oscBundleReceivedEventArgs)
		{
			parseOscBundle(oscBundleReceivedEventArgs.Bundle);
		}

		#endregion
		

	}
}





