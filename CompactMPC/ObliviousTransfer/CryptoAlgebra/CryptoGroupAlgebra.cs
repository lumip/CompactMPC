﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CompactMPC.ObliviousTransfer.CryptoAlgebra
{
    public abstract class CryptoGroupAlgebra<E, S>
    {
        public S Order { get; }
        public E Generator { get; }

        public CryptoGroupAlgebra(E generator, S order)
        {
            if (generator == null)
                throw new ArgumentNullException(nameof(generator));
            Generator = generator;

            if (order == null)
                throw new ArgumentNullException(nameof(order));
            Order = order;
        }

        public E GenerateElement(S index)
        {
            return MultiplyScalar(Generator, index);
        }

        public abstract E Add(E left, E right);
        public abstract E MultiplyScalar(E e, S scalar);

        public abstract E Invert(E e);
        // todo(lumip): can implement as below if I can constrain type S to have operator-
        //{
        //    return MultiplyScalar(e, Order - 1);
        //}
    }
}
