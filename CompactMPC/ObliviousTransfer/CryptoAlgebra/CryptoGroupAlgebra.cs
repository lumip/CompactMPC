using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

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

        public BigInteger Invert(BigInteger e)
        {
            return MultiplyScalar(e, Order - 1);
        }

        public abstract BigInteger Add(BigInteger left, BigInteger right);
        public abstract BigInteger MultiplyScalar(BigInteger e, BigInteger scalar);
    }
}
