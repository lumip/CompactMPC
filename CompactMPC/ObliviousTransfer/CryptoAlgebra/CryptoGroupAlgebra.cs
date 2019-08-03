using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Diagnostics;

namespace CompactMPC.ObliviousTransfer.CryptoAlgebra
{
    public abstract class CryptoGroupAlgebra
    {
        public BigInteger Order { get; }
        public BigInteger Generator { get; }
        public int GroupElementSize { get; }
        public int OrderSize { get; }

        public int GroupElementBitlen { get { return 8 * GroupElementSize; } }
        public int OrderBitlen { get { return 8 * OrderSize; } }

        public CryptoGroupAlgebra(BigInteger generator, BigInteger order, int groupElementSize, int orderSize)
        {
            if (generator == null)
                throw new ArgumentNullException(nameof(generator));
            Generator = generator;

            if (order == null)
                throw new ArgumentNullException(nameof(order));
            Order = order;

            GroupElementSize = groupElementSize;
            OrderSize = orderSize;
        }

        public BigInteger GenerateElement(BigInteger index)
        {
            return MultiplyScalar(Generator, index);
        }

        public virtual BigInteger Negate(BigInteger e)
        {
            return MultiplyScalar(e, Order - 1);
        }

        protected BigInteger Multiplex(BigInteger selection, BigInteger left, BigInteger right)
        {
            Debug.Assert(selection.IsOne || selection.IsZero);
            return right + selection * (left - right);
        }

        protected BigInteger Multiplex(bool selection, BigInteger left, BigInteger right)
        {
            var sel = new BigInteger(Convert.ToByte(selection));
            return Multiplex(sel, left, right);
        }

        public virtual BigInteger MultiplyScalar(BigInteger e, BigInteger k)
        {
            // note(lumip): double-and-add (in this case: square-and-multiply)
            //  implementation that issues the same amount of adds no matter
            //  the value of k and has no conditional control flow. It is thus
            //  safe(r) against timing/power/cache/branch prediction(?)
            //  side channel attacks.

            k = k % Order; // k * e is at least periodic in Order
            BigInteger r0 = IdentityElement;

            int i = OrderBitlen - 1;

            for (BigInteger mask = BigInteger.One << (OrderBitlen - 1); !mask.IsZero; mask = mask >> 1, --i)
            {
                BigInteger bitI = (k & mask) >> i;
                r0 = Add(r0, r0);
                BigInteger r1 = Add(r0, e);

                r0 = Multiplex(bitI, r1, r0);
            }
            Debug.Assert(i == -1);
            return r0;
        }

        public abstract BigInteger IdentityElement { get; }
        public abstract BigInteger Add(BigInteger left, BigInteger right);
        
    }
}
