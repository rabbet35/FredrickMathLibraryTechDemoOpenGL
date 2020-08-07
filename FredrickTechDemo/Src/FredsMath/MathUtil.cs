﻿namespace FredrickTechDemo.FredsMath
{
    static class MathUtil
    {
        public static float normalize(float min, float max, float val)//returns a float in between 0 and 1 representing the percentage that VAL is from Min to Max e.g: min of 0 and max of 100 with value 50 will return 0.5
        {
            return (val - min) / (max - min);
        }

        public static float normalizeCustom(float mapMin, float mapMax, float min, float max, float val)//returns a float in between mapMin and mapMax representing the percentage that VAL is from Min to Max e.g: min of 0 and max of 100 with value 50 will return 0
        {
            return (mapMax - mapMin) * normalize(min, max, val) + mapMin;
        }

        public static double radians(double degrees)
        {
            return degrees * 0.01745329251994329576923690768489D;
        }

        /*gives the dot product of the two provided coordinates*/
        public static float dotFloats(float xA, float yA, float zA, float xB, float yB, float zB)
        {
            return xA * xB + yA * yB + zA * zB;
        }
    }
}