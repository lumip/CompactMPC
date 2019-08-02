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

        public MultiplicativeGroup(BigInteger primeModulo, BigInteger order, BigInteger generator, int groupElementSize, int orderSize)
            : this(new MultiplicativeGroupAlgebra(primeModulo, order, generator, groupElementSize, orderSize)) { }

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
