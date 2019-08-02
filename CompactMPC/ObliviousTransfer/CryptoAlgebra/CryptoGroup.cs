using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace CompactMPC.ObliviousTransfer.CryptoAlgebra
{
    public abstract class CryptoGroup
    {
        private CryptoGroupAlgebra _groupAlgebra;

        public CryptoGroup(CryptoGroupAlgebra groupAlgebra)
        {
            if (groupAlgebra == null)
                throw new ArgumentNullException(nameof(groupAlgebra));
            _groupAlgebra = groupAlgebra;
        }

        public BigInteger Order { get { return _groupAlgebra.Order; } }

        public int GroupElementSize { get { return _groupAlgebra.GroupElementSize; } }
        public int OrderSize { get { return _groupAlgebra.OrderSize; } }

        public CryptoGroupElement Generator { get { return CreateGroupElement(_groupAlgebra.Generator, _groupAlgebra); } }

        public CryptoGroupElement GenerateElement(BigInteger index)
        {
            return CreateGroupElement(_groupAlgebra.GenerateElement(index), _groupAlgebra);
        }

        public CryptoGroupElement CreateElementFromBytes(byte[] buffer)
        {
            return CreateGroupElement(buffer, _groupAlgebra);
        }

        protected abstract CryptoGroupElement CreateGroupElement(BigInteger e, CryptoGroupAlgebra groupAlgebra);
        protected abstract CryptoGroupElement CreateGroupElement(byte[] buffer, CryptoGroupAlgebra groupAlgebra);
    }
}
