//
// Reaktion - An audio reactive animation toolkit for Unity.
//
// Copyright (C) 2013, 2014 Keijiro Takahashi
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
using UnityEngine;
using System.Collections;

namespace Reaktion {

[AddComponentMenu("Reaktion/Injector/UniOSC Injector")]
public class UniOSCInjector : InjectorBase
{
	public float OSCvalue = 0.0f;
	public string OSCAddress = "";
	public bool OSCenabled = false;
	public bool OSCSmoothing = false;
	public float OSCSmoothingAmt = 0.01f;
	public float SmoothVelocity = 0.0f;
	public bool OSCValueCurve = false;
	public string Address { get { return OSCAddress; } }

	public float Value { get { return OSCvalue; } set { OSCvalue = value; } }

	public bool On { get { return OSCenabled; } set { OSCenabled = value; } }

	public AnimationCurve curve = AnimationCurve.Linear(0, 1, 0.5f, 0);

		void OnEnable()
		{
			useRaw = true;
		}

    void Update()
    {
		//useRaw = true;

		Debug.Log (useRaw);
        if (OSCenabled) // checks to see if INC osc is enabled. IF it is will take any incoming data - DATA that is only passed when enabled to the OSCInjector.
		{

				if(OSCSmoothing && OSCValueCurve)	
				{
					dbLevel = Mathf.SmoothDamp(dbLevel, curve.Evaluate(OSCvalue), ref SmoothVelocity, OSCSmoothingAmt);
				}
				else if(OSCSmoothing)
				{
				if (dbLevel != OSCvalue)
				{
					dbLevel = Mathf.SmoothDamp(dbLevel, OSCvalue, ref SmoothVelocity, OSCSmoothingAmt);
				}
				}
				else if(OSCValueCurve)
				{
					dbLevel = curve.Evaluate(OSCvalue);
				}
				else if (dbLevel != OSCvalue)
					dbLevel = OSCvalue;
		}
		
	}

}

} // namespace Reaktion
