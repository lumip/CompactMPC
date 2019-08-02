﻿using System;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Net;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using CompactMPC.ObliviousTransfer;
using CompactMPC.ObliviousTransfer.CryptoAlgebra;
using CompactMPC.Networking;
using CompactMPC.UnitTests.Util;

namespace CompactMPC.UnitTests
{
    [TestClass]
    public class ObliviousTransferTest
    {
        private static readonly string[] TestOptions = { "Alicia", "Briann", "Charly", "Dennis" };

        [TestMethod]
        public void TestNaorPinkasObliviousTransfer()
        {
            Task.WhenAll(
                Task.Factory.StartNew(RunObliviousTransferParty, TaskCreationOptions.LongRunning),
                Task.Factory.StartNew(RunObliviousTransferParty, TaskCreationOptions.LongRunning)
            ).Wait();
        }

        private void RunObliviousTransferParty()
        {
            Quadruple<byte[]>[] options = new Quadruple<byte[]>[3];
            options = new Quadruple<byte[]>[]
            {
                new Quadruple<byte[]>(TestOptions.Select(s => Encoding.ASCII.GetBytes(s)).ToArray()),
                new Quadruple<byte[]>(TestOptions.Select(s => Encoding.ASCII.GetBytes(s.ToLower())).ToArray()),
                new Quadruple<byte[]>(TestOptions.Select(s => Encoding.ASCII.GetBytes(s.ToUpper())).ToArray()),
            };

            using (CryptoContext cryptoContext = CryptoContext.CreateDefault())
            {
                IGeneralizedObliviousTransfer obliviousTransfer = new NaorPinkasObliviousTransfer(
                    new MultiplicativeGroup(SecurityParameters.CreateDefault768Bit()),
                    cryptoContext
                );

                using (ITwoPartyNetworkSession session = TestNetworkSession.EstablishTwoParty())
                {
                    if (session.LocalParty.Id == 0)
                    {
                        obliviousTransfer.SendAsync(session.Channel, options, 3, 6).Wait();
                    }
                    else
                    {
                        QuadrupleIndexArray indices = new QuadrupleIndexArray(new[] { 0, 3, 2 });
                        byte[][] results = obliviousTransfer.ReceiveAsync(session.Channel, indices, 3, 6).Result;

                        Assert.IsNotNull(results, "Result is null.");
                        Assert.AreEqual(3, results.Length, "Result does not match the correct number of invocations.");

                        for (int j = 0; j < 3; ++j)
                        {
                            CollectionAssert.AreEqual(
                                results[j],
                                options[j][indices[j]],
                                "Incorrect message content {0} (should be {1}).",
                                Encoding.ASCII.GetString(results[j]),
                                Encoding.ASCII.GetString(options[j][indices[j]])
                            );
                        }
                    }
                }
            }
        }
    }
}
