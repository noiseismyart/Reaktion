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
using HutongGames.PlayMaker;
using Reaktion;


namespace UniOSC{

	/// <summary>
	/// Rotates (localRotation) the hosting game object.
	/// For every axis you have a separate OSC address to specify
	/// </summary>
	[AddComponentMenu("UniOSC/UniOSC to Reaktion")]
	public class UniOSCtoReaktion :  UniOSCEventTarget {

	
		OSCInjector injector;
		OSCInjector[] injectorArray;
		List<OSCInjector> injectorList;


		public string oscFader1;



		#region private

		private float tempFloat;
		private int tempInt;

		#endregion

		void Awake(){

			injectorArray = FindObjectsOfType (typeof(OSCInjector)) as OSCInjector[];
			injectorList = injectorArray.ToList();

			foreach (OSCInjector inject in injectorList) 
			{
				Debug.Log("Address: " + inject.Address + " Value: " + inject.Value);
			}
		}

		public override void OnEnable(){
			_Init();
			base.OnEnable();
		}

		private void _Init(){
			
			//receiveAllAddresses = false;
			_oscAddresses.Clear();
			if(!receiveAllAddresses){
				foreach (OSCInjector inject in injectorList) 
				{
					_oscAddresses.Add(inject.Address);
				}

			}

		}
	

		public override void OnOSCMessageReceived(UniOSCEventArgs args){
		
			if(args.Message.Data.Count <1)return;
			if(!( args.Message.Data[0]  is  System.Single))return;

			float value = (float)args.Message.Data[0] ;
			//Faders

			foreach (OSCInjector inject in injectorList) 
			{
				if (String.Equals (args.Address, inject.Address)) {
					inject.Value = value;
					//Debug to check working
					//Debug.Log("Address: " + inject.Address + " Value: " + inject.Value);
					}
			}

			/*if (String.Equals (args.Address, oscFader2)) {
				FsmVariables.GlobalVariables.GetFsmFloat("Fader2").Value = value;
			}
			if (String.Equals (args.Address, oscFader3)) {
				FsmVariables.GlobalVariables.GetFsmFloat("Fader3").Value = value;
			}
			if (String.Equals (args.Address, oscBounceModifier)) {
				FsmVariables.GlobalVariables.GetFsmFloat("BounceModifier").Value = (value-0.5f)*8.0f;
			}

			//Toggles
			if (String.Equals (args.Address, oscToggle1)) {
				FsmVariables.GlobalVariables.GetFsmBool("BrickWallAnimated").Value = Convert.ToBoolean(value);
			}
			if (String.Equals (args.Address, oscToggle2)) {
				FsmVariables.GlobalVariables.GetFsmBool("WallPunchActive").Value = Convert.ToBoolean(value);
			}
			if (String.Equals (args.Address, oscTurbLogo)) {
				FsmVariables.GlobalVariables.GetFsmBool("TurbLogo").Value = Convert.ToBoolean(value);
			}

			//Colors

			//AudioFilters		
			if (String.Equals (args.Address, oscFilter1)) {
				FsmVariables.GlobalVariables.GetFsmFloat("Filter1").Value = value;
			}
			if (String.Equals (args.Address, oscFilter2)) {
				FsmVariables.GlobalVariables.GetFsmFloat("Filter2").Value = value;
			}
			if (String.Equals (args.Address, oscFilter3)) {
				FsmVariables.GlobalVariables.GetFsmFloat("Filter3").Value = value;
			}
			if (String.Equals (args.Address, oscFilter4)) {
				FsmVariables.GlobalVariables.GetFsmFloat("Filter4").Value = value;
			}
			if (String.Equals (args.Address, oscFilter5)) {
				FsmVariables.GlobalVariables.GetFsmFloat("Filter5").Value = value;
			}
			if (String.Equals (args.Address, oscFilter6)) {
				FsmVariables.GlobalVariables.GetFsmFloat("Filter6").Value = value;
			}
			if (String.Equals (args.Address, oscFilter7)) {
				FsmVariables.GlobalVariables.GetFsmFloat("Filter7").Value = value;
			}
			if (String.Equals (args.Address, oscFilter8)) {
				FsmVariables.GlobalVariables.GetFsmFloat("Filter8").Value = value;
			}

			if (String.Equals (args.Address, oscColor1A)) {
				FsmVariables.GlobalVariables.GetFsmFloat("Color1A").Value = value;
			}
			if (String.Equals (args.Address, oscColor1R)) {
				FsmVariables.GlobalVariables.GetFsmFloat("Color1R").Value = value;
			}
			if (String.Equals (args.Address, oscColor1G)) {
				FsmVariables.GlobalVariables.GetFsmFloat("Color1G").Value = value;
			}
			if (String.Equals (args.Address, oscColor1B)) {
				FsmVariables.GlobalVariables.GetFsmFloat("Color1B").Value = value;
			}
			if (String.Equals (args.Address, oscColor2A)) {
				FsmVariables.GlobalVariables.GetFsmFloat("Color2A").Value = value;
			}
			if (String.Equals (args.Address, oscColor2R)) {
				FsmVariables.GlobalVariables.GetFsmFloat("Color2R").Value = value;
			}
			if (String.Equals (args.Address, oscColor2G)) {
				FsmVariables.GlobalVariables.GetFsmFloat("Color2G").Value = value;
			}
			if (String.Equals (args.Address, oscColor2B)) {
				FsmVariables.GlobalVariables.GetFsmFloat("Color2B").Value = value;
			}
			if (String.Equals (args.Address, oscColor3A)) {
				FsmVariables.GlobalVariables.GetFsmFloat("Color3A").Value = value;
			}
			if (String.Equals (args.Address, oscColor3R)) {
				FsmVariables.GlobalVariables.GetFsmFloat("Color3R").Value = value;
			}
			if (String.Equals (args.Address, oscColor3G)) {
				FsmVariables.GlobalVariables.GetFsmFloat("Color3G").Value = value;
			}
			if (String.Equals (args.Address, oscColor3B)) {
				FsmVariables.GlobalVariables.GetFsmFloat("Color3B").Value = value;
			}
			if (String.Equals (args.Address, oscColor4A)) {
				FsmVariables.GlobalVariables.GetFsmFloat("Color4A").Value = value;
			}
			if (String.Equals (args.Address, oscColor4R)) {
				FsmVariables.GlobalVariables.GetFsmFloat("Color4R").Value = value;
			}
			if (String.Equals (args.Address, oscColor4G)) {
				FsmVariables.GlobalVariables.GetFsmFloat("Color4G").Value = value;
			}
			if (String.Equals (args.Address, oscColor4B)) {
				FsmVariables.GlobalVariables.GetFsmFloat("Color4B").Value = value;
			}
			if (String.Equals (args.Address, oscColor5A)) {
				FsmVariables.GlobalVariables.GetFsmFloat("Color5A").Value = value;
			}
			if (String.Equals (args.Address, oscColor5R)) {
				FsmVariables.GlobalVariables.GetFsmFloat("Color5R").Value = value;
			}
			if (String.Equals (args.Address, oscColor5G)) {
				FsmVariables.GlobalVariables.GetFsmFloat("Color5G").Value = value;
			}
			if (String.Equals (args.Address, oscColor5B)) {
				FsmVariables.GlobalVariables.GetFsmFloat("Color5B").Value = value;
			}
			if (String.Equals (args.Address, oscColor6A)) {
				FsmVariables.GlobalVariables.GetFsmFloat("Color6A").Value = value;
			}
			if (String.Equals (args.Address, oscColor6R)) {
				FsmVariables.GlobalVariables.GetFsmFloat("Color6R").Value = value;
			}
			if (String.Equals (args.Address, oscColor6G)) {
				FsmVariables.GlobalVariables.GetFsmFloat("Color6G").Value = value;
			}
			if (String.Equals (args.Address, oscColor6B)) {
				FsmVariables.GlobalVariables.GetFsmFloat("Color6B").Value = value;
			}
			if (String.Equals (args.Address, oscColor7A)) {
				FsmVariables.GlobalVariables.GetFsmFloat("Color7A").Value = value;
			}
			if (String.Equals (args.Address, oscColor7R)) {
				FsmVariables.GlobalVariables.GetFsmFloat("Color7R").Value = value;
			}
			if (String.Equals (args.Address, oscColor7G)) {
				FsmVariables.GlobalVariables.GetFsmFloat("Color7G").Value = value;
			}
			if (String.Equals (args.Address, oscColor7B)) {
				FsmVariables.GlobalVariables.GetFsmFloat("Color7B").Value = value;
			}

			if (String.Equals (args.Address, oscFilterPeakFreq)) {
				FsmVariables.GlobalVariables.GetFsmFloat("FilterPeakFreq").Value = value;
			}
			if (String.Equals (args.Address, oscFilterPeakFreqMag)) {
				FsmVariables.GlobalVariables.GetFsmFloat("FilterPeakFreqMag").Value = value;
			}
			if (String.Equals (args.Address, oscMouseX)) {
				tempFloat = value * 2 - 1;
				FsmVariables.GlobalVariables.GetFsmFloat("MouseX").Value = tempFloat;
			}
			if (String.Equals (args.Address, oscMouseY)) {
				tempFloat = value * 2 - 1;
				FsmVariables.GlobalVariables.GetFsmFloat("MouseY").Value = tempFloat;
			}
			if (String.Equals (args.Address, oscX1)) {
				tempFloat = (value - 0.5f) * 14.0f;
				FsmVariables.GlobalVariables.GetFsmFloat("X1").Value = tempFloat;
			}
			if (String.Equals (args.Address, oscY1)) {
				tempFloat = value * 2 - 1;
				FsmVariables.GlobalVariables.GetFsmFloat("Y1").Value = tempFloat;
			}
			if (String.Equals (args.Address, oscX2)) {
				tempFloat = (value - 0.5f) * 14.0f;
				FsmVariables.GlobalVariables.GetFsmFloat("X2").Value = tempFloat;
			}
			if (String.Equals (args.Address, oscY2)) {
				tempFloat = value * 2 - 1;
				FsmVariables.GlobalVariables.GetFsmFloat("Y2").Value = tempFloat;
			}
			if (String.Equals (args.Address, oscX3)) {
				tempFloat = (value - 0.5f) * 14.0f;
				FsmVariables.GlobalVariables.GetFsmFloat("X3").Value = tempFloat;
			}
			if (String.Equals (args.Address, oscY3)) {
				tempFloat = value * 2 - 1;
				FsmVariables.GlobalVariables.GetFsmFloat("Y3").Value = tempFloat;
			}
			if (String.Equals (args.Address, oscX4)) {
				tempFloat = (value - 0.5f) * 14.0f;
				FsmVariables.GlobalVariables.GetFsmFloat("X4").Value = tempFloat;
			}
			if (String.Equals (args.Address, oscY4)) {
				tempFloat = value * 2 - 1;
				FsmVariables.GlobalVariables.GetFsmFloat("Y4").Value = tempFloat;
			}
			if (String.Equals (args.Address, oscTrigger1)) {
				tempFloat = value*100;
				tempInt = Mathf.RoundToInt(tempFloat);

				FsmVariables.GlobalVariables.GetFsmInt("ShaderTrigger").Value = tempInt;
			}
			if (String.Equals (args.Address, oscTrigger2)) {
				tempFloat = value*100;
				tempInt = Mathf.RoundToInt(tempFloat)+101;

				FsmVariables.GlobalVariables.GetFsmInt("ShaderTrigger").Value = tempInt;
			}*/


		}


	}

}