using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Lerp Function Library
public class LerpFunctions
{
    //Function to create value used to interpolate object
    public static float Lerping(float startValue, float endValue, float time)
    {
        //Calculation takes current time, start & end points, and interpolates where object should be 
        return (startValue + (endValue - startValue) * time);
    }

    //Function calculates lerp percentage value based on the ease chosen
    public static float LerpPerc(int ease, float t, bool ping)
    {
        switch (ease)
        {
            case 0: return 0;

            case 1: return Easing.Linear(t, ping);

            case 2: return Easing.Quadratic.easeIn(t, ping);

            case 3: return Easing.Quadratic.easeOut(t, ping);

            case 4: return Easing.Quadratic.easeInOut(t, ping);

            case 5: return Easing.Sine.EaseIn(t, ping);

            case 6: return Easing.Sine.EaseOut(t, ping);

            case 7: return Easing.Sine.EaseInOut(t, ping);

            case 8: return Easing.Elastic.EaseIn(t, ping);

            default: return 0;
        }
    }
}
