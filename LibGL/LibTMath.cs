using System;
using System.Collections.Generic;
using OpenTK;

namespace TAO.Engine.LibGL
{
    public static class LibTMath
    {
        public static float[] Vector3ToFloatArray(Vector3 vector)
        {
            return new float[3]
            {
                vector.X, vector.Y, vector.Z
            };
        }

        public static Vector3 ClampVector(Vector3 vector, Vector3 min, Vector3 max)
        {
            float x = Math.Clamp(vector.X, min.X, max.X);
            float y = Math.Clamp(vector.Y, min.Y, max.Y);
            float z = Math.Clamp(vector.Z, min.Z, max.Z);

            return new Vector3(x, y, z);
        }

        public static Vector3 AbsoluteVector(Vector3 vector)
        {
            return new Vector3(Math.Abs(vector.X), Math.Abs(vector.Y), Math.Abs(vector.Z));
        }

        public static bool IsLower(char @char)
        {
            if (@char == 'a')
            {
                return true;
            }
            else if (@char == 'b')
            {
                return true;
            }
            else if (@char == 'c')
            {
                return true;
            }
            else if (@char == 'd')
            {
                return true;
            }
            else if (@char == 'e')
            {
                return true;
            }
            else if (@char == 'f')
            {
                return true;
            }
            else if (@char == 'g')
            {
                return true;
            }
            else if (@char == 'h')
            {
                return true;
            }
            else if (@char == 'i')
            {
                return true;
            }
            else if (@char == 'j')
            {
                return true;
            }
            else if (@char == 'k')
            {
                return true;
            }
            else if (@char == 'l')
            {
                return true;
            }
            else if (@char == 'n')
            {
                return true;
            }
            else if (@char == 'm')
            {
                return true;
            }
            else if (@char == 'ñ')
            {
                return true;
            }
            else if (@char == 'o')
            {
                return true;
            }
            else if (@char == 'p')
            {
                return true;
            }
            else if (@char == 'q')
            {
                return true;
            }
            else if (@char == 'r')
            {
                return true;
            }
            else if (@char == 's')
            {
                return true;
            }
            else if (@char == 't')
            {
                return true;
            }
            else if (@char == 'u')
            {
                return true;
            }
            else if (@char == 'v')
            {
                return true;
            }
            else if (@char == 'w')
            {
                return true;
            }
            else if (@char == 'x')
            {
                return true;
            }
            else if (@char == 'y')
            {
                return true;
            }
            else if (@char == 'z')
            {
                return true;
            }
            else if (@char == 'ç')
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
