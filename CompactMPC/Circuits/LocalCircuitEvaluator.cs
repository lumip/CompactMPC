﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompactMPC.Circuits
{
    public class LocalCircuitEvaluator : ICircuitEvaluator<bool>
    {
        public bool EvaluateAndGate(bool leftValue, bool rightValue, GateContext gateContext, CircuitContext circuitContext)
        {
            return leftValue && rightValue;
        }

        public bool EvaluateXorGate(bool leftValue, bool rightValue, GateContext gateContext, CircuitContext circuitContext)
        {
            return leftValue ^ rightValue;
        }

        public bool EvaluateNotGate(bool value, GateContext gateContext, CircuitContext circuitContext)
        {
            return !value;
        }
    }
}