using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//Easing Library

public class Easing {
    //Linear
    public static float Linear(float t) {
        return t;
    }
    //Linear PingPong Overload
    public static float Linear(float t, bool x) {
        if (x) return Mathf.PingPong(Linear(t), 0.5f);
        return t;
    }

    //Quadratic Easings
    public class Quadratic {
        //Quadratic ease in equation
        public static float easeIn(float t) {
            return t * t;
        }
        //Quadratic ease in equation, PingPong Overload
        public static float easeIn(float t, bool x) {
            if (x) return Mathf.PingPong(easeIn(t), 0.5f);
            return t * t;
        }
        //Quadratic ease out equation
        public static float easeOut(float t) {
            return t * (2f - t);
        }
        //Quadratic ease out equation, PingPong Overload
        public static float easeOut(float t, bool x) {
            if (x) return Mathf.PingPong(easeOut(t), 0.5f);
            return t * (2f - t);
        }
        //Quadratic ease in/out equation
        public static float easeInOut(float t) {
            if ((t *= 2f) < 1f) return 0.5f * t * t;
            return -0.5f * ((t -= 1f) * (t - 2f) - 1f);
        }
        //Quadratic ease in/out equation, PingPong Overload
        public static float easeInOut(float t, bool x) {
            if (x) return Mathf.PingPong(easeInOut(t), 0.5f);
            else if ((t *= 2f) < 1f) return 0.5f * t * t;
            return -0.5f * ((t -= 1f) * (t - 2f) - 1f);
        }
    }

    //Sine Easings
    public class Sine
    {
        //Sine ease in equation
        public static float EaseIn(float t) {
            return 1f - Mathf.Cos(t * Mathf.PI * 0.5f);
        }
        //Sine ease in equation, PingPong Overload
        public static float EaseIn(float t, bool x) {
            if (x) return Mathf.PingPong(EaseIn(t), 0.5f);
            return 1f - Mathf.Cos(t * Mathf.PI * 0.5f);
        }
        //Sine ease out equation
        public static float EaseOut(float t) {
            return Mathf.Sin(t * Mathf.PI * 0.5f);
        }
        //Sine ease out equation, PingPong Overload
        public static float EaseOut(float t, bool x) {
            if (x) return Mathf.PingPong(EaseOut(t), 0.5f);
            return Mathf.Sin(t * Mathf.PI * 0.5f);
        }
        //Sine ease in/out equation
        public static float EaseInOut(float t) {
            return -(Mathf.Cos(Mathf.PI * t) - 1) / 2;
        }
        //Sine ease in/out equation, PingPong Overload
        public static float EaseInOut(float t, bool x) {
            if (x) return Mathf.PingPong(EaseInOut(t), 0.5f);
            return -(Mathf.Cos(Mathf.PI * t) - 1) / 2;
        }
        
    }
    //Elastic Easings
    public class Elastic
    {
        //Elastic ease in equation
        public static float EaseIn(float t) {
            const float c = (2 * Mathf.PI) / 3;
            if (t == 0) return 0;
            else if (t == 1) return 1;
            return -Mathf.Pow(2, 10 * t - 10) * Mathf.Sin((float)((t * 10 - 10.75) * c));
        }
        //Elastic ease in equation, PingPong Overload
        public static float EaseIn(float t, bool x) {
            const float c = (2 * Mathf.PI) / 3;
            if (x) return Mathf.PingPong(EaseIn(t), 0.5f);
            else if (t == 0) return 0;
            else if (t == 1) return 1;
            return -Mathf.Pow(2, 10 * t - 10) * Mathf.Sin((float)((t * 10 - 10.75) * c));
        }
    }
}

