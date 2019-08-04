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

        public MultiplicativeGroupAlgebra(BigInteger primeModulo, BigInteger order, BigInteger generator, int groupElementSize, int orderSize, int factorSize)
            : base(generator, order, groupElementSize, orderSize, factorSize)
        {
            // note(lumip): currently not verifying that module is indeed prime or that generator is indeed a generator!
            Modulo = primeModulo;
        }

        public MultiplicativeGroupAlgebra(BigInteger primeModulo, BigInteger order, BigInteger generator, int groupElementSize, int orderSize)
            : base(generator, order, groupElementSize, orderSize, orderSize) { }

        public MultiplicativeGroupAlgebra(SecurityParameters parameters)
            : this(parameters.P, parameters.Q, parameters.G, parameters.GroupElementSize, parameters.GroupElementSize, parameters.ExponentSize) { }

        public override BigInteger Add(BigInteger left, BigInteger right)
        {
            return (left * right) % Modulo;
        }

        public BigInteger MultiplyScalarUnsafe(BigInteger e, BigInteger k)
        {
            // note(lumip): faster, but might be susceptible to side channel timing/power attacks..
            return BigInteger.ModPow(e, k, Modulo);
        }

        public override BigInteger IdentityElement { get { return BigInteger.One; } }

    }
}
