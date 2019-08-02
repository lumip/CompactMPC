using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace CompactMPC.ObliviousTransfer.CryptoAlgebra
{
    public class MultiplicativeGroupElement : CryptoGroupElement<BigInteger, BigInteger>
    {
        public MultiplicativeGroupElement(BigInteger value, CryptoGroupAlgebra<BigInteger, BigInteger> groupAlgebra)
            : base(value, groupAlgebra) { }

        public override byte[] ToByteArray()
        {
            return Value.ToByteArray();
        }

        protected override CryptoGroupElement<BigInteger, BigInteger> Create(BigInteger value, CryptoGroupAlgebra<BigInteger, BigInteger> groupAlgebra)
        {
            return new MultiplicativeGroupElement(Value, groupAlgebra);
        }
    }
}
