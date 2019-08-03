using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Diagnostics;

namespace CompactMPC.ObliviousTransfer.CryptoAlgebra
{
    public class MultiplicativeGroupAlgebra : CryptoGroupAlgebra
    {
        public BigInteger Modulo { get; }

        public MultiplicativeGroupAlgebra(BigInteger primeModulo, BigInteger order, BigInteger generator, int groupElementSize, int orderSize)
            : base(generator, order, groupElementSize, orderSize)
        {
            // note(lumip): currently not verifying that module is indeed prime or that generator is indeed a generator!
            Modulo = primeModulo;
        }

        public MultiplicativeGroupAlgebra(SecurityParameters parameters)
            : this(parameters.P, parameters.Q, parameters.G, parameters.GroupElementSize, parameters.ExponentSize) { }

        public override BigInteger Add(BigInteger left, BigInteger right)
        {
            return (left * right) % Modulo;
        }

        public BigInteger MultiplyScalarUnsafe(BigInteger e, BigInteger k)
        {
            // note(lumip): faster, but might be susceptible to side channel timing/power attacks..
            return BigInteger.ModPow(e, k, Modulo);
        }

        private BigInteger Multiplex(BigInteger selection, BigInteger left, BigInteger right)
        {
            Debug.Assert(selection.IsOne || selection.IsZero);
            return right + selection * (left - right);
        }

        private BigInteger Multiplex(bool selection, BigInteger left, BigInteger right)
        {
            var sel = new BigInteger(Convert.ToByte(selection));
            return Multiplex(sel, left, right);
        }

        public override BigInteger MultiplyScalar(BigInteger e, BigInteger k)
        {
            // note(lumip): double-and-add (in this case: square-and-multiply)
            //  implementation that issues the same amount of adds no matter
            //  the value of k and has no conditional control flow. It is thus
            //  safe(r) against timing/power/cache/branch prediction(?)
            //  side channel attacks.

            k = k % Order; // k * e is at least periodic in Order
            BigInteger r0 = BigInteger.One;
            
            int i = OrderBitlen - 1;
            BigInteger currentExp = BigInteger.Zero;
            
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
    }
}
