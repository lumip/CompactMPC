using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace CompactMPC.ObliviousTransfer.CryptoAlgebra
{
    public abstract class CryptoGroupElement
    {
        private CryptoGroupAlgebra _groupAlgebra;
        public BigInteger Value { get; private set; }

        public CryptoGroupElement(BigInteger value, CryptoGroupAlgebra groupAlgebra)
        {
            if (groupAlgebra == null)
                throw new ArgumentNullException(nameof(groupAlgebra));
            _groupAlgebra = groupAlgebra;

            if (value == null)
                throw new ArgumentNullException(nameof(value));
            Value = value;
        }

        public CryptoGroupElement Clone()
        {
            return Create(Value, _groupAlgebra);
        }

        public void Add(CryptoGroupElement e)
        {
            if (_groupAlgebra != e._groupAlgebra)
                throw new ArgumentException("Added group element must be from the same group!", nameof(e));
            Value = _groupAlgebra.Add(Value, e.Value);
        }

        public void MultiplyScalar(BigInteger k)
        {
            Value = _groupAlgebra.MultiplyScalar(Value, k);
        }

        public void Invert()
        {
            Value = _groupAlgebra.Negate(Value);
        }

        public static CryptoGroupElement operator +(CryptoGroupElement left, CryptoGroupElement right)
        {
            var result = left.Clone();
            result.Add(right);
            return result;
        }

        public static CryptoGroupElement operator -(CryptoGroupElement e)
        {
            var result = e.Clone();
            result.Invert();
            return result;
        }

        public static CryptoGroupElement operator -(CryptoGroupElement left, CryptoGroupElement right)
        {
            var result = right.Clone();
            result.Invert();
            result.Add(left);
            return result;
        }

        public static CryptoGroupElement operator *(CryptoGroupElement e, BigInteger k)
        {
            var result = e.Clone();
            result.MultiplyScalar(k);
            return result;
        }

        public static CryptoGroupElement operator *(BigInteger k, CryptoGroupElement e)
        {
            return e * k;
        }

        protected abstract CryptoGroupElement Create(BigInteger value, CryptoGroupAlgebra groupAlgebra);
        public abstract byte[] ToByteArray();
    }
}
