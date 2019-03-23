/* Copyright (C) 2009-2010 Syed Asad Rahman <asad@ebi.ac.uk>
 *
 * Contact: cdk-devel@lists.sourceforge.net
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public License
 * as published by the Free Software Foundation; either version 2.1
 * of the License, or (at your option) any later version.
 * All we ask is that proper credit is given for our work, which includes
 * - but is not limited to - adding the above copyright notice to the beginning
 * of your source code files, and to any copyright notice that you may distribute
 * with programs based on this work.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
 */

using Microsoft.VisualStudio.TestTools.UnitTesting;
using NCDK.Smiles;
using NCDK.SMSD.Helper;
using System.Collections.Generic;

namespace NCDK.SMSD.Algorithms.RGraphs
{
    // @cdk.module test-smsd
    // @author     Syed Asad Rahman
    [TestClass()]
    public class CDKRMapHandlerTest
    {
        private readonly IChemObjectBuilder builder = CDK.Builder;
        private readonly CDKRMapHandler handler = new CDKRMapHandler();

        public CDKRMapHandlerTest() { }

        [TestMethod()]
        public void TestGetSource()
        {
            IAtomContainer expResult = builder.NewAtomContainer();
            handler.Source = expResult;
            IAtomContainer result = handler.Source;
            Assert.AreEqual(expResult, result);
        }

        [TestMethod()]
        public void TestSetSource()
        {
            IAtomContainer expResult = builder.NewAtomContainer();
            handler.Source = expResult;
            IAtomContainer result = handler.Source;
            Assert.AreEqual(expResult, result);
        }

        [TestMethod()]
        public void TestGetTarget()
        {
            IAtomContainer expResult = builder.NewAtomContainer();
            handler.Target = expResult;
            IAtomContainer result = handler.Target;
            Assert.AreEqual(expResult, result);
        }

        [TestMethod()]
        public void TestSetTarget()
        {
            IAtomContainer expResult = builder.NewAtomContainer();
            handler.Target = expResult;
            IAtomContainer result = handler.Target;
            Assert.AreEqual(expResult, result);
        }

        [TestMethod()]
        public void TestCalculateOverlapsAndReduce()
        {
            var sp = new SmilesParser(builder);
            var Molecule1 = sp.ParseSmiles("O1C=CC=C1");
            var Molecule2 = sp.ParseSmiles("C1CCCC1");
            CDKRMapHandler instance = new CDKRMapHandler();
            instance.CalculateOverlapsAndReduce(Molecule1, Molecule2, true);
            Assert.IsNotNull(FinalMappings.Instance.Count);
        }

        [TestMethod()]
        public void TestCalculateOverlapsAndReduceExactMatch()
        {
            var sp = new SmilesParser(builder);
            var Molecule1 = sp.ParseSmiles("O1C=CC=C1");
            var Molecule2 = sp.ParseSmiles("O1C=CC=C1");
            CDKRMapHandler instance = new CDKRMapHandler();
            instance.CalculateOverlapsAndReduceExactMatch(Molecule1, Molecule2, true);
            // TODO review the generated test code and remove the default call to fail.
            Assert.IsNotNull(FinalMappings.Instance.Count);
        }

        [TestMethod()]
        public void TestGetMappings()
        {
            var sp = new SmilesParser(builder);
            var Molecule1 = sp.ParseSmiles("O1C=CC=C1");
            var Molecule2 = sp.ParseSmiles("O1C=CC=C1");
            CDKRMapHandler instance = new CDKRMapHandler();
            instance.CalculateOverlapsAndReduceExactMatch(Molecule1, Molecule2, true);
            var result = instance.Mappings;
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod()]
        public void TestSetMappings()
        {
            var map = new SortedDictionary<int, int>
            {
                [0] = 0,
                [1] = 1
            };
            var mappings = new List<IReadOnlyDictionary<int, int>> { map };

            CDKRMapHandler instance = new CDKRMapHandler
            {
                Mappings = mappings
            };
            Assert.IsNotNull(instance.Mappings);
        }

        [TestMethod()]
        public void TestIsTimeoutFlag()
        {
            CDKRMapHandler instance = new CDKRMapHandler();
            bool expResult = true;
            instance.IsTimedOut = true;
            bool result = instance.IsTimedOut;
            Assert.AreEqual(expResult, result);
        }

        [TestMethod()]
        public void TestSetTimeoutFlag()
        {
            bool timeoutFlag = false;
            var instance = new CDKRMapHandler();
            instance.IsTimedOut = timeoutFlag;
            Assert.AreNotEqual(true, instance.IsTimedOut);
        }
    }
}
