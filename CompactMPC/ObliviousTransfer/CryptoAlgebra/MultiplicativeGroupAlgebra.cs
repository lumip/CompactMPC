using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace CompactMPC.ObliviousTransfer.CryptoAlgebra
{
    public class MultiplicativeGroupAlgebra : CryptoGroupAlgebra<BigInteger, BigInteger>
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

        public override BigInteger MultiplyScalar(BigInteger e, BigInteger k)
        {
            return BigInteger.ModPow(e, k, Modulo);
        }

        public override BigInteger Invert(BigInteger e)
        {
            return MultiplyScalar(e, Order - 1);
        }
    }
}
