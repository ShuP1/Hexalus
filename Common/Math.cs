namespace Common
{
    public enum Direction { Center, LeftUp, Up, RightUp, LeftDown, Down, RightDown }

    public static class Math
    {
        public static double sqrt3 = System.Math.Sqrt(3);

        public static bool Odd(int x)
        {
            return x % 2 == 0;
        }

        public static float OddCorrect(int x)
        {
            return (Odd(x) ? 0.5f : 0);
        }
    }
}