using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace CompactMPC.ObliviousTransfer.CryptoAlgebra
{
    public class MultiplicativeGroupElement : CryptoGroupElement
    {
        public MultiplicativeGroupElement(BigInteger value, CryptoGroupAlgebra groupAlgebra)
            : base(value, groupAlgebra) { }

        public override byte[] ToByteArray()
        {
            return Value.ToByteArray();
        }

        protected override CryptoGroupElement Create(BigInteger value, CryptoGroupAlgebra groupAlgebra)
        {
            return new MultiplicativeGroupElement(Value, groupAlgebra);
        }
    }
}
