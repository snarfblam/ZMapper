using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZMapper
{
    [Flags]
    enum Direction
    {
        None = 0,
        All = 15,

        Up = 1,
        Right = 2,
        Down = 4, 
        Left = 8,
    }

    static class ExtDirection
    {
        public static Direction Toggle(this Direction d, Direction dir) {
            return d ^ dir;
        }
        public static Direction Clear(this Direction d, Direction dir) {
            return d & ~dir;
        }
        public static Direction Set(this Direction d, Direction dir) {
            return d | dir;
        }
        public static bool Test(this Direction d, Direction test) {
            return test == (d & test);
        }
        public static bool TestAny(this Direction d, Direction test) {
            return 0 != (d & test);
        }
    }
}
