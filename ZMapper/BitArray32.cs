using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZMapper
{
    public struct BitArray32
    {
        public uint Bits;

        public bool this[int index] {
            get {
                uint mask = 1u << index;
                return (Bits & mask) == mask;
            }
            set {
                uint mask = 1u << index;
                if (value) {
                    Bits |= mask;
                } else {
                    Bits &= ~mask;
                }
            }
        }
    }
}
