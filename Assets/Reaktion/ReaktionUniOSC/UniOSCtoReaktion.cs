/*
* UniOSC
* Copyright © 2014 Stefan Schlupek
* All rights reserved
* info@monoflow.org
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Reaktion;
using OSCsharp.Data;
using UniOSC;

[AddComponentMenu("UniOSC/UniOSC to Reaktion")]
public class UniOSCtoReaktion :  MonoBehaviour {

	
		UniOSCInjector injector;
		UniOSCInjector[] injectorArray;
		List<UniOSCInjector> injectorList;
		public UniOSCConnection OSCConnection;


		#region private
		private UniOSCEventTargetCBImplementation oscTarget;

		#endregion

		void Awake(){

			injectorArray = FindObjectsOfType (typeof(UniOSCInjector)) as UniOSCInjector[];
			injectorList = injectorArray.ToList();

			foreach (UniOSCInjector inject in injectorList) 
			{
				Debug.Log("Address: " + inject.Address + " Value: " + inject.Value + "Enabled" + inject.On);
			}

			oscTarget = new UniOSCEventTargetCBImplementation(OSCConnection);
			oscTarget.OSCMessageReceived+=OnOSCMessageReceived;
		}

		void OnEnable(){
		//Debug.Log("UniOSCCodeBasedDemo.OnEnable");
		//Just to create a OSCEventTarget isn't enough. We nedd to enable it:
		//oscTarget.Enable();

			oscTarget.Enable ();
		}

		void OnDisable()
		{
			oscTarget.Disable ();
		}

		void OnDestroy()
		{
			//Clean up things and release recources!!!!
			//Otherwise our callbacks can still respond even if our GameObject with this script is destroyed/removed from the scene

			oscTarget.Dispose ();
			oscTarget = null;
		}


	#region callbacks
		void OnOSCMessageReceived(object sender, UniOSCEventArgs args)
		{
			//Debug.Log("UniOSCCodeBasedDemo.OnOSCMessageReceived:"+ _GetAddressFromOscPacket(args));

			OscMessage msg = (OscMessage)args.Packet;
			if(msg.Data.Count <1)return;
			
			float _data = (float)msg.Data[0];

			foreach (UniOSCInjector inject in injectorList) 
			{
				if (String.Equals (args.Address, inject.Address)) {
									if(inject.On)
					{
						inject.Value = _data;
					}
					//Debug to check working
					//Debug.Log("Address: " + inject.Address + " Value: " + inject.Value);
					}
			}

		}

		private string _GetAddressFromOscPacket(UniOSCEventArgs args){
			return (args.Packet is OscMessage) ? ((OscMessage)args.Packet).Address : ((OscBundle)args.Packet).Address ;
		}

	#endregion


		
}