namespace Common
{
    public class Point
    {
        public int X;
        public int Y;

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(Point p)
        {
            if (p == null)
                return false;

            return X == p.X && Y == p.Y;
        }

        public static Point HexMove(Point origin, Direction dir)
        {
            if (origin == null)
                return null;

            Point res = origin;
            switch (dir)
            {
                case Direction.Up:
                    res.Y--;
                    break;

                case Direction.Down:
                    res.Y++;
                    break;

                case Direction.LeftUp:
                case Direction.RightUp:
                    if (!Math.Odd(res.X)) { res.Y--; }
                    break;

                case Direction.LeftDown:
                case Direction.RightDown:
                    if (Math.Odd(res.X)) { res.Y++; }
                    break;
            }

            if (dir == Direction.LeftUp || dir == Direction.LeftDown) { res.X--; }
            else if (dir == Direction.RightUp || dir == Direction.RightDown) { res.X++; }
            return res;
        }

        public void HexMove(Direction dir)
        {
            Point p = HexMove(this, dir);
            X = p.X;
            Y = p.Y;
        }

        public Point[] GetAdj()
        {
            Point[] res = new Point[6];
            for (int i = 0; i < 6; i++)
            {
                res[i] = HexMove(this, (Direction)(i + 1));
            }

            return res;
        }
    }
}