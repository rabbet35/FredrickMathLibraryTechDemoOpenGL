﻿using OpenTK;

namespace Coictus
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

        public static double clamp(double val, double min, double max)
        {
            if (val < min) val = min;
            if (val > max) val = max;
            return val;
        }

        public static double lerp(double src, double dest, float factor)
        {
            return src + (dest - src) * factor;
        }
        public static float lerpF(float src, float dest, float factor)
        {
            return src + (dest - src) * factor;
        }

        public static double radians(double degrees)
        {
            return degrees * 0.01745329251994329576923690768489D;
        }
        public static float radiansF(float degrees)
        {
            return degrees * 0.01745329251994329576923690768489F;
        }
        public static Vector3 radians(Vector3 degrees)
        {
            return degrees * 0.01745329251994329576923690768489F;
        }
        public static Vector3d radians(Vector3d degrees)
        {
            return degrees * 0.01745329251994329576923690768489D;
        }

        /*gives the dot product of the two provided coordinates*/
        public static float dotFloats(float xA, float yA, float zA, float xB, float yB, float zB)
        {
            return xA * xB + yA * yB + zA * zB;
        }

        public static double dotDoubles(double xA, double yA, double zA, double xB, double yB, double zB)
        {
            return xA * xB + yA * yB + zA * zB;
        }

        public static double hypotenuse(double a, double b)
        {
            return System.Math.Sqrt(a * a + b * b);
        }

        /*Takes in 3 doubles and a smooth factor. Smooths the 3 values with strength of smooth factor. Smooth factor of 0 does nothing, and 1 makes each value the average.*/
        public static void smooth3(ref double x, ref double y, ref double z, float factor)
        {
            if (factor <= 0)
                return;
            if (factor > 1F)
                factor = 1F;

            double avg = (x + y + z) / 3D;
            x = lerp(x, avg, factor);
            y = lerp(y, avg, factor);
            z = lerp(z, avg, factor);
        }

         public static void smooth3F(ref float x, ref float y, ref float z, float factor)
        {
            if (factor <= 0)
                return;
            if (factor > 1F)
                factor = 1F;

            float avg = (x + y + z) / 3F;
            x = lerpF(x, avg, factor);
            y = lerpF(y, avg, factor);
            z = lerpF(z, avg, factor);
        }

        public static double max3(double a, double b, double c)
        {
            if (a >= b && a >= c)
            {
                return a;
            }
            if (b >= a && b >= c)
            {
                return b;
            }
            return c;
        }

        public static double min3(double a, double b, double c)
        {
            if (a <= b && a <= c)
            {
                return a;
            }
            if (b <= a && b <= c)
            {
                return b;
            }
            return c;
        }

        public static double max6(double a, double b, double c, double d, double e, double f)
        {
            if (a >= b && a >= c && a >= d && a >= e && a >= f)
            {
                return a;
            }
            if (b >= a && b >= c && b >= d && b >= e && b >= f)
            {
                return b;
            }
            if (c >= a && c >= b && c >= d && c >= e && c >= f)
            {
                return c;
            }
            if (d >= a && d >= b && d >= c && d >= e && d >= f)
            {
                return d;
            }
            if (e >= a && e >= b && e >= c && e >= d && e >= f)
            {
                return e;
            }
            return f;
        }

        

        

        /*Returns a vector 3 with the absolute values negated from one, only useful for axis aligned collisions*/
        public static Vector3 oneMinusAbsolute(Vector3 vec)
        {
            return new Vector3(1F - System.Math.Abs(vec.X), 1F - System.Math.Abs(vec.Y), 1F - System.Math.Abs(vec.Z));
        }

        /*Returns a vector 3d with the absolute values negated from one, only useful for axis aligned collisions*/
        public static Vector3d oneMinusAbsolute(Vector3d vec)
        {
            return new Vector3d(1D - System.Math.Abs(vec.X), 1D - System.Math.Abs(vec.Y), 1D - System.Math.Abs(vec.Z));
        }

        public static Vector3 convertVec(Vector3d vec)
        {
            return new Vector3((float)vec.X, (float)vec.Y, (float)vec.Z);
        }
        public static Matrix4 createRotation(Vector3 rot)
        {
            rot = radians(rot);
            return Matrix4.CreateRotationX(rot.X) * Matrix4.CreateRotationY(rot.Y) * Matrix4.CreateRotationZ(rot.Z);
        }
        public static Matrix4 createRotation(Vector3d rot)
        {
            rot = radians(rot);
            return Matrix4.CreateRotationX((float)rot.X) * Matrix4.CreateRotationY((float)rot.Y) * Matrix4.CreateRotationZ((float)rot.Z);
        }

        /*Scales the provided floats the same way a matrix would*/
        public static void scaleXYZFloats(Vector3 newScale, float x, float y, float z, out float newX, out float newY, out float newZ)
        {
            newX = x * newScale.X;
            newY = y * newScale.Y;
            newZ = z * newScale.Z;
        }

        /*Rotates the provided floats the same way a matrix would*/
        public static void rotateXYZFloats(Vector3 newRotation, float x, float y, float z, out float newX, out float newY, out float newZ)
        {
            Matrix4 rotMat = createRotation(newRotation);
            newX = MathUtil.dotFloats(rotMat.M11, rotMat.M12, rotMat.M13, x, y, z) + rotMat.M14;
            newY = MathUtil.dotFloats(rotMat.M21, rotMat.M22, rotMat.M23, x, y, z) + rotMat.M24;
            newZ = MathUtil.dotFloats(rotMat.M31, rotMat.M32, rotMat.M33, x, y, z) + rotMat.M34;
        }

        /*Translates the provided floats the same way a matrix would*/
        public static void translateXYZFloats(Vector3 translation, float x, float y, float z, out float newX, out float newY, out float newZ)
        {
            newX = x + translation.X;
            newY = y + translation.Y;
            newZ = z + translation.Z;
        }
    }
}
