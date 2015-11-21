using System;

namespace Common
{
    public static class Check
    {
        public static void Argument(bool condition, string message)
        {
            if(!condition)
                throw new ArgumentException(message);
        }
    }
}