using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace CompactMPC.ObliviousTransfer.CryptoAlgebra
{
    public class MultiplicativeGroup : CryptoGroup<BigInteger, BigInteger>
    {
        public MultiplicativeGroup(MultiplicativeGroupAlgebra groupAlgebra)
            : base(groupAlgebra) { }

        public MultiplicativeGroup(SecurityParameters parameters)
            : this(new MultiplicativeGroupAlgebra(parameters)) { }

        public MultiplicativeGroup(BigInteger primeModulo, BigInteger order, BigInteger generator)
            : this(new MultiplicativeGroupAlgebra(primeModulo, order, generator)) { }

        public MultiplicativeGroup(BigInteger primeModulo, BigInteger generator)
            : this(new MultiplicativeGroupAlgebra(primeModulo, generator)) { }

        protected override CryptoGroupElement<BigInteger, BigInteger> CreateGroupElement(BigInteger e, CryptoGroupAlgebra<BigInteger, BigInteger> groupAlgebra)
        {
            return new MultiplicativeGroupElement(e, groupAlgebra);
        }

        protected override CryptoGroupElement<BigInteger, BigInteger> CreateGroupElement(byte[] buffer, CryptoGroupAlgebra<BigInteger, BigInteger> groupAlgebra)
        {
            return new MultiplicativeGroupElement(new BigInteger(buffer), groupAlgebra);
        }
    }
}
