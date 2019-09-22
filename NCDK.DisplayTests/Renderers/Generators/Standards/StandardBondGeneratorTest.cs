/*
 * Copyright (c) 2014 European Bioinformatics Institute (EMBL-EBI)
 *                    John May <jwmay@users.sf.net>
 *
 * Contact: cdk-devel@lists.sourceforge.net
 *
 * This program is free software; you can redistribute it and/or modify it
 * under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation; either version 2.1 of the License, or (at
 * your option) any later version. All we ask is that proper credit is given
 * for our work, which includes - but is not limited to - adding the above
 * copyright notice to the beginning of your source code files, and to any
 * copyright notice that you may distribute with programs based on this work.
 *
 * This program is distributed in the hope that it will be useful, but WITHOUT
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
 * FITNESS FOR A PARTICULAR PURPOSE.  See the GNU Lesser General Public
 * License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
 */

using Microsoft.VisualStudio.TestTools.UnitTesting;
using NCDK.Numerics;
using NCDK.Templates;
using System;
using System.Diagnostics;
using static NCDK.Renderers.Generators.Standards.StandardBondGenerator;

namespace NCDK.Renderers.Generators.Standards
{
    [TestClass()]
    public class StandardBondGeneratorTest
    {
        [TestMethod()]
        public void AdenineRingPreference()
        {
            var adenine = TestMoleculeFactory.MakeAdenine();
            var ringMap = StandardBondGenerator.RingPreferenceMap(adenine);

            int nSize5 = 0, nSize6 = 0;
            foreach (var bond in adenine.Bonds)
            {
                if (!ringMap.TryGetValue(bond, out IAtomContainer ring))
                    continue;// exocyclic bond
                switch (ring.Atoms.Count)
                {
                    case 5:
                        nSize5++;
                        break;
                    case 6:
                        nSize6++;
                        break;
                }
            }

            // 6 bonds should point to the six member ring
            // 4 bonds should point to the five member ring
            Assert.AreEqual(4, nSize5);
            Assert.AreEqual(6, nSize6);
        }

        [TestMethod()]
        public void MetalRingPreference()
        {
            var smipar = CDK.SmilesParser;
            var mol = smipar.ParseSmiles("C1[Fe]C=CC2=C1C=CN2");
            foreach (var atom in mol.Atoms)
                atom.Point2D = new Vector2(0, 0);
            var ringMap = StandardBondGenerator.RingPreferenceMap(mol);

            int nSize5 = 0, nSize6 = 0;
            foreach (var bond in mol.Bonds)
            {
                var ring = ringMap[bond];
                // exocyclic bond
                if (ring == null)
                    continue;
                switch (ring.Atoms.Count)
                {
                    case 5:
                        nSize5++;
                        break;
                    case 6:
                        nSize6++;
                        break;
                }
            }

            // 5 bonds should point to the six member ring
            // 5 bonds should point to the five member ring
            Assert.AreEqual(5, nSize5);
            Assert.AreEqual(5, nSize6);
        }

        [TestMethod()]
        public void RingSizePriority()
        {
            Assert.AreEqual(0, RingBondOffsetComparator.SizePreference(6));
            Assert.AreEqual(1, RingBondOffsetComparator.SizePreference(5));
            Assert.AreEqual(2, RingBondOffsetComparator.SizePreference(7));
            Assert.AreEqual(3, RingBondOffsetComparator.SizePreference(4));
            Assert.AreEqual(4, RingBondOffsetComparator.SizePreference(3));
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidRingSize()
        {
            RingBondOffsetComparator.SizePreference(2);
        }

        [TestMethod()]
        public void MacroCycle()
        {
            Assert.AreEqual(8, RingBondOffsetComparator.SizePreference(8));
            Assert.AreEqual(10, RingBondOffsetComparator.SizePreference(10));
            Assert.AreEqual(20, RingBondOffsetComparator.SizePreference(20));
        }

        [TestMethod()]
        public void BenzeneDoubleBondCount()
        {
            Assert.AreEqual(3, RingBondOffsetComparator.CountNumberOfDoubleBonds(TestMoleculeFactory.MakeBenzene()));
        }

        [TestMethod()]
        public void BenzeneElementCount()
        {
            var freq = RingBondOffsetComparator.CountLightElements(TestMoleculeFactory.MakeBenzene());
            Assert.AreEqual(6, freq[6]);
        }


        [TestMethod()]
        public void HighAtomicNoElementCount()
        {
            var container = TestMoleculeFactory.MakeBenzene();
            container.Atoms[0].AtomicNumber = 34;
            container.Atoms[0].Symbol = "Se";
            var freq = RingBondOffsetComparator.CountLightElements(container);
            Assert.AreEqual(5, freq[6]);
        }

        [TestMethod()]
        public void AdenineElementCount()
        {
            var freq = RingBondOffsetComparator.CountLightElements(TestMoleculeFactory.MakeAdenine());
            Assert.AreEqual(5, freq[6]);
            Assert.AreEqual(5, freq[7]);
        }

        [TestMethod()]
        public void BenzeneComparedToPyrrole()
        {
            var benzene = TestMoleculeFactory.MakeBenzene();
            var pyrrole = TestMoleculeFactory.MakePyrrole();

            Assert.AreEqual(-1, new RingBondOffsetComparator().Compare(benzene, pyrrole));
            Assert.AreEqual(+1, new RingBondOffsetComparator().Compare(pyrrole, benzene));
        }

        [TestMethod()]
        public void BenzeneComparedToCycloHexane()
        {
            var benzene = TestMoleculeFactory.MakeBenzene();
            var cyclohexane = TestMoleculeFactory.MakeCyclohexane();

            Assert.AreEqual(-1, new RingBondOffsetComparator().Compare(benzene, cyclohexane));
            Assert.AreEqual(+1, new RingBondOffsetComparator().Compare(cyclohexane, benzene));
        }

        [TestMethod()]
        public void BenzeneComparedToCycloHexene()
        {
            var benzene = TestMoleculeFactory.MakeBenzene();
            var cyclohexene = TestMoleculeFactory.MakeCyclohexene();

            Assert.AreEqual(-1, new RingBondOffsetComparator().Compare(benzene, cyclohexene));
            Assert.AreEqual(+1, new RingBondOffsetComparator().Compare(cyclohexene, benzene));
        }

        [TestMethod()]
        public void BenzeneComparedToBenzene()
        {
            var benzene1 = TestMoleculeFactory.MakeBenzene();
            var benzene2 = TestMoleculeFactory.MakeBenzene();

            Assert.AreEqual(0, new RingBondOffsetComparator().Compare(benzene1, benzene2));
            Assert.AreEqual(0, new RingBondOffsetComparator().Compare(benzene2, benzene1));
        }

        [TestMethod()]
        public void BenzeneComparedToPyridine()
        {
            var benzene = TestMoleculeFactory.MakeBenzene();
            var pyridine = TestMoleculeFactory.MakePyridine();

            Assert.AreEqual(-1, new RingBondOffsetComparator().Compare(benzene, pyridine));
            Assert.AreEqual(+1, new RingBondOffsetComparator().Compare(pyridine, benzene));
        }

        [TestMethod()]
        public void FuraneComparedToPyrrole()
        {
            var furane = TestMoleculeFactory.MakePyrrole();
            var pyrrole = TestMoleculeFactory.MakePyrrole();

            Debug.Assert(furane.Atoms[1].AtomicNumber == 7);
            furane.Atoms[1].AtomicNumber = 8;
            furane.Atoms[1].Symbol = "O";

            Assert.AreEqual(-1, new RingBondOffsetComparator().Compare(pyrrole, furane));
            Assert.AreEqual(+1, new RingBondOffsetComparator().Compare(furane, pyrrole));
        }

        [TestMethod()]
        public void FuraneComparedToThiophene()
        {
            var furane = TestMoleculeFactory.MakePyrrole();
            var thiophene = TestMoleculeFactory.MakePyrrole();

            Debug.Assert(furane.Atoms[1].AtomicNumber == 7);
            Debug.Assert(thiophene.Atoms[1].AtomicNumber == 7);
            furane.Atoms[1].AtomicNumber = 8;
            furane.Atoms[1].Symbol = "O";
            thiophene.Atoms[1].AtomicNumber = 16;
            thiophene.Atoms[1].Symbol = "S";

            Assert.AreEqual(-1, new RingBondOffsetComparator().Compare(furane, thiophene));
            Assert.AreEqual(+1, new RingBondOffsetComparator().Compare(thiophene, furane));
        }
    }
}
