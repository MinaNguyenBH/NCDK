/* Copyright (C) 1997-2007  The Chemistry Development Kit (CDK) project
 *
 *  Contact: cdk-devel@lists.sourceforge.net
 *
 *  This program is free software; you can redistribute it and/or
 *  modify it under the terms of the GNU Lesser General Public License
 *  as published by the Free Software Foundation; either version 2.1
 *  of the License, or (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT Any WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 *
 *  You should have received a copy of the GNU Lesser General Public License
 *  along with this program; if not, write to the Free Software
 *  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
 */

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using NCDK.Tools.Manipulator;
using NCDK.Aromaticities;
using NCDK.Common.Primitives;
using NCDK.Templates;
using NCDK.Graphs;
using NCDK.Isomorphisms;
using NCDK.Stereo;
using NCDK.Silent;
using NCDK.Common.Base;

namespace NCDK.Smiles
{
    /// <summary>
    /// Please see the test.gui package for visual feedback on tests.
    /// </summary>
    /// <seealso cref="SmilesParserTest"/>
    // @author         steinbeck
    // @cdk.module     test-smiles
    // @cdk.created    2003-09-19
    [TestClass()]
    public class SmilesParserTest : CDKTestCase
    {
        private static SmilesParser sp = CDK.SmilesParser;

        [TestMethod()]
        [Timeout(1000)]
        public void TestSingleOrDoubleFlag()
        {
            var smiles = "c1cccn1c2cccn2";

            // need to load the exact representation - this is SMILES string is
            // invalid and cannot be correctly kekulised
            var mol = LoadExact(smiles);

            // single or double flags now assigned separately
            AtomContainerManipulator.SetSingleOrDoubleFlags(mol);

            // Let's check the atoms first...
            Assert.IsTrue(mol.Atoms[0].IsSingleOrDouble);
            Assert.IsTrue(mol.Atoms[1].IsSingleOrDouble);
            Assert.IsTrue(mol.Atoms[2].IsSingleOrDouble);
            Assert.IsTrue(mol.Atoms[3].IsSingleOrDouble);
            Assert.IsTrue(mol.Atoms[4].IsSingleOrDouble);
            Assert.IsTrue(mol.Atoms[5].IsSingleOrDouble);
            Assert.IsTrue(mol.Atoms[6].IsSingleOrDouble);
            Assert.IsTrue(mol.Atoms[7].IsSingleOrDouble);
            Assert.IsTrue(mol.Atoms[8].IsSingleOrDouble);
            Assert.IsTrue(mol.Atoms[9].IsSingleOrDouble);
            // ...and then the bonds...
            // ...in the first ring...
            Assert.IsTrue(mol.GetBond(mol.Atoms[0], mol.Atoms[1]).IsSingleOrDouble);
            Assert.IsTrue(mol.GetBond(mol.Atoms[1], mol.Atoms[2]).IsSingleOrDouble);
            Assert.IsTrue(mol.GetBond(mol.Atoms[2], mol.Atoms[3]).IsSingleOrDouble);
            Assert.IsTrue(mol.GetBond(mol.Atoms[3], mol.Atoms[4]).IsSingleOrDouble);
            Assert.IsTrue(mol.GetBond(mol.Atoms[4], mol.Atoms[0]).IsSingleOrDouble);
            // ...then the bond in between the rings...
            Assert.IsFalse(mol.GetBond(mol.Atoms[4], mol.Atoms[5]).IsSingleOrDouble);
            // ...and at last the bonds in the other ring.
            Assert.IsTrue(mol.GetBond(mol.Atoms[5], mol.Atoms[6]).IsSingleOrDouble);
            Assert.IsTrue(mol.GetBond(mol.Atoms[6], mol.Atoms[7]).IsSingleOrDouble);
            Assert.IsTrue(mol.GetBond(mol.Atoms[7], mol.Atoms[8]).IsSingleOrDouble);
            Assert.IsTrue(mol.GetBond(mol.Atoms[8], mol.Atoms[9]).IsSingleOrDouble);
            Assert.IsTrue(mol.GetBond(mol.Atoms[9], mol.Atoms[5]).IsSingleOrDouble);
        }

        /// <summary>
        /// 1-(1H-pyrrol-2-yl)pyrrole
        /// </summary>
        // @cdk.inchi InChI=1/C8H8N2/c1-2-7-10(6-1)8-4-3-5-9-8/h1-7,9H
        [TestMethod()]
        [ExpectedException(typeof(InvalidSmilesException))]
        public void Pyrrolylpyrrole_invalid()
        {
            Load("c1cccn1c2cccn2");
        }

        /// <summary>
        /// 1-(1H-pyrrol-2-yl)pyrrole
        /// </summary>
        // @cdk.inchi InChI=1/C8H8N2/c1-2-7-10(6-1)8-4-3-5-9-8/h1-7,9H
        [TestMethod()]
        public void Pyrrolylpyrrole_valid()
        {
            var mol = Load("c1cccn1c2ccc[nH]2");
            Assert.IsNotNull(mol);
        }

        // cdk.bug 1363882
        [TestMethod()]
        [Timeout(1000)]
        public void TestBug1363882()
        {
            var smiles = "[H]c2c([H])c(c1c(nc(n1([H]))C(F)(F)F)c2Cl)Cl";
            var mol = Load(smiles);
            MakeAtomType(mol);
            AssertAtomTypesPerceived(mol);
            Assert.AreEqual(18, mol.Atoms.Count);
            Assert.IsTrue(Aromaticity.CDKLegacy.Apply(mol));
        }

        // @cdk.bug 1535587
        [TestMethod()]
        [Timeout(1000)]
        public void TestBug1535587()
        {
            var smiles = "COC(=O)c2ccc3n([H])c1ccccc1c3(c2)";
            var mol = Load(smiles);
            MakeAtomType(mol);
            AssertAtomTypesPerceived(mol);
            Assert.AreEqual(18, mol.Atoms.Count);
            Assert.IsTrue(Aromaticity.CDKLegacy.Apply(mol));
            Assert.AreEqual("N", mol.Atoms[8].Symbol);
            Assert.IsTrue(mol.Atoms[8].IsAromatic);
        }

        // @cdk.bug 1579235
        [TestMethod()]
        [Timeout(1000)]
        public void TestBug1579235()
        {
            var smiles = "c2cc1cccn1cc2";
            var mol = Load(smiles);
            MakeAtomType(mol);
            AssertAtomTypesPerceived(mol);
            Assert.AreEqual(9, mol.Atoms.Count);
            Assert.IsTrue(Aromaticity.CDKLegacy.Apply(mol));
            Assert.AreEqual("N", mol.Atoms[6].Symbol);
            foreach (var atom in mol.Atoms)
            {
                if (atom.Symbol.Equals("C"))
                {
                    Assert.AreEqual(Hybridization.SP2, atom.Hybridization);
                }
                else
                {
                    Assert.AreEqual(Hybridization.Planar3, atom.Hybridization);
                }
            }
        }

        [TestMethod()]
        [Timeout(1000)]
        public void TestBug1579229()
        {
            var smiles = "c1c(c23)ccc(c34)ccc4ccc2c1";
            var mol = sp.ParseSmiles(smiles);
            MakeAtomType(mol);
            AssertAtomTypesPerceived(mol);
            Assert.AreEqual(14, mol.Atoms.Count);
            Assert.IsTrue(Aromaticity.CDKLegacy.Apply(mol));
            foreach (var atom in mol.Atoms)
            {
                Assert.AreEqual(Hybridization.SP2, atom.Hybridization);
            }
        }

        // @cdk.bug 1579230
        [TestMethod()]
        [Timeout(1000)]
        public void TestBug1579230()
        {
            var smiles = "Cc1cccc2sc3nncn3c12";
            var mol = Load(smiles);
            MakeAtomType(mol);
            AssertAtomTypesPerceived(mol);
            Assert.AreEqual(13, mol.Atoms.Count);
            Assert.IsTrue(Aromaticity.CDKLegacy.Apply(mol));
            for (int i = 1; i < 13; i++)
            {
                // first atom is not aromatic
                IAtom atom = mol.Atoms[i];
                if (atom.Symbol.Equals("C"))
                    Assert.AreEqual(Hybridization.SP2, atom.Hybridization);
                if (atom.Symbol.Equals("N") || atom.Symbol.Equals("S"))
                {
                    Assert.IsTrue(Hybridization.SP2 == atom.Hybridization
                            || Hybridization.Planar3 == atom.Hybridization);
                }
            }
        }

        [TestMethod()]
        [Timeout(1000)]
        public void TestPyridine_N_oxideUncharged()
        {
            var smiles = "O=n1ccccc1";
            var mol = LoadExact(smiles);
            MakeAtomType(mol);
            AssertAtomTypesPerceived(mol);
            Assert.AreEqual(7, mol.Atoms.Count);
        }

        [TestMethod()]
        [Timeout(1000)]
        public void TestPyridine_N_oxideCharged()
        {
            var smiles = "[O-][n+]1ccccc1";
            var mol = sp.ParseSmiles(smiles);
            AssertAtomTypesPerceived(mol);
            Assert.AreEqual(7, mol.Atoms.Count);
        }

        [TestMethod()]
        [Timeout(1000)]
        public void TestPositivePhosphor()
        {
            var smiles = "[Cl+3]([O-])([O-])([O-])[O-].[P+]([O-])(c1ccccc1)(c1ccccc1)c1cc([nH0+](C)c(c1)c1ccccc1)c1ccccc1";
            var mol = sp.ParseSmiles(smiles);
            Assert.AreEqual(0, mol.Atoms[22].ImplicitHydrogenCount.Value);
            AssertAtomTypesPerceived(mol);
            Assert.AreEqual(38, mol.Atoms.Count);
            Assert.AreEqual("P", mol.Atoms[5].Symbol);
            Assert.AreEqual(+1, mol.Atoms[5].FormalCharge.Value);
            Assert.AreEqual("Cl", mol.Atoms[0].Symbol);
            Assert.AreEqual(+3, mol.Atoms[0].FormalCharge.Value);
        }

        /// <summary>
        /// The next methods tests compounds with several conjugated rings These
        /// compounds would not fail if the Aromaticity Detection was changed so that
        /// a ring is aromatic if all the atoms in a ring have already been flagged
        /// as aromatic from the testing of other rings in the system.
        /// </summary>
        [TestMethod()]
        [Timeout(1000)]
        public void TestUnusualConjugatedRings()
        {
            //7090-41-7:
            var smiles = "c1(Cl)cc2c3cc(Cl)c(Cl)cc3c2cc1Cl";
            var mol = sp.ParseSmiles(smiles);
            AssertAtomTypesPerceived(mol);
            Assert.AreEqual(16, mol.Atoms.Count);
        }

        [TestMethod()]
        [Timeout(1000)]
        public void TestUnusualConjugatedRings_2()
        {
            //206-44-0:
            var smiles = "c(c(ccc1)ccc2)(c1c(c3ccc4)c4)c23";
            var mol = sp.ParseSmiles(smiles);
            AssertAtomTypesPerceived(mol);
            Assert.AreEqual(16, mol.Atoms.Count);
        }

        [TestMethod()]
        [Timeout(1000)]
        public void TestUnusualConjugatedRings_3()
        {
            //207-08-9:
            var smiles = "c2ccc1cc3c(cc1c2)c4cccc5cccc3c45";
            var mol = sp.ParseSmiles(smiles);
            AssertAtomTypesPerceived(mol);
            Assert.AreEqual(20, mol.Atoms.Count);
        }

        [TestMethod()]
        [Timeout(1000)]
        public void TestUnusualConjugatedRings_4()
        {
            //2693-46-1:
            var smiles = "Nc1c(c23)cccc3c4ccccc4c2cc1";
            var mol = sp.ParseSmiles(smiles);
            AssertAtomTypesPerceived(mol);
            Assert.AreEqual(17, mol.Atoms.Count);
        }

        [TestMethod()]
        [Timeout(1000)]
        public void TestUnusualConjugatedRings_5()
        {
            //205-99-2:
            var smiles = "c12ccccc1cc3c4ccccc4c5c3c2ccc5";
            var mol = sp.ParseSmiles(smiles);
            AssertAtomTypesPerceived(mol);
            Assert.AreEqual(20, mol.Atoms.Count);
        }

        [TestMethod()]
        [Timeout(1000)]
        public void Test187_78_0()
        {
            // are all 4 rings aromatic? Is smiles correct?
            var smiles = "c1c(c23)ccc(c34)ccc4ccc2c1";
            var mol = sp.ParseSmiles(smiles);
            AssertAtomTypesPerceived(mol);
            Assert.AreEqual(14, mol.Atoms.Count);
        }

        [TestMethod()]
        [Timeout(1000)]
        public void Test187_78_0_PubChem()
        {
            // are all 4 rings aromatic? Is smiles correct?
            var smiles = "C1=CC2=C3C(=CC=C4C3=C1C=C4)C=C2";
            var mol = sp.ParseSmiles(smiles);
            AssertAtomTypesPerceived(mol);
            Assert.AreEqual(14, mol.Atoms.Count);
        }

        [TestMethod()]
        [Timeout(1000)]
        public void Test41814_78_2()
        {
            var smiles = "Cc1cccc2sc3nncn3c12";
            var mol = sp.ParseSmiles(smiles);
            AssertAtomTypesPerceived(mol);
            Assert.AreEqual(13, mol.Atoms.Count);
        }

        [TestMethod()]
        [Timeout(1000)]
        public void Test239_64_5()
        {
            var smiles = "c1ccc4c(c1)ccc5c3ccc2ccccc2c3[nH]c45";
            var mol = sp.ParseSmiles(smiles);
            AssertAtomTypesPerceived(mol);
            Assert.AreEqual(21, mol.Atoms.Count);
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidSmilesException))]
        public void Test239_64_5_invalid()
        {
            Load("c1ccc4c(c1)ccc5c3ccc2ccccc2c3nc45");
        }

        /// <summary>
        /// Compounds like Indolizine (274-40-8) with a fused nitrogen as part of a 6
        /// membered ring and another ring do not parse
        /// </summary>
        [TestMethod()]
        [Timeout(1000)]
        public void TestIndolizine()
        {
            var smiles = "c2cc1cccn1cc2";
            var mol = sp.ParseSmiles(smiles);
            AssertAtomTypesPerceived(mol);
            Assert.AreEqual(9, mol.Atoms.Count);
        }

        [TestMethod()]
        [Timeout(1000)]
        public void TestSmiles1()
        {
            var smiles = "C1c2c(c3c(c(O)cnc3)cc2)CC(=O)C1";
            var molecule = sp.ParseSmiles(smiles);
            AssertAtomTypesPerceived(molecule);
            Assert.AreEqual(16, molecule.Atoms.Count);
        }

        [TestMethod()]
        [Timeout(1000)]
        public void TestSmiles2()
        {
            var smiles = "O=C(O3)C1=COC(OC4OC(CO)C(O)C(O)C4O)C2C1C3C=C2COC(C)=O";
            var molecule = sp.ParseSmiles(smiles);
            AssertAtomTypesPerceived(molecule);
            Assert.AreEqual(29, molecule.Atoms.Count);
        }

        [TestMethod()]
        [Timeout(1000)]
        public void TestSmiles3()
        {
            var smiles = "CN1C=NC2=C1C(N(C)C(N2C)=O)=O";
            var molecule = sp.ParseSmiles(smiles);
            AssertAtomTypesPerceived(molecule);
            Assert.AreEqual(14, molecule.Atoms.Count);
        }

        [TestMethod()]
        [Timeout(1000)]
        public void TestSmiles4()
        {
            var smiles = "CN(C)CCC2=CNC1=CC=CC(OP(O)(O)=O)=C12";
            var molecule = sp.ParseSmiles(smiles);
            AssertAtomTypesPerceived(molecule);
            Assert.AreEqual(19, molecule.Atoms.Count);
        }

        [TestMethod()]
        [Timeout(1000)]
        public void TestSmiles5()
        {
            var smiles = "O=C(O)C1C(OC(C3=CC=CC=C3)=O)CC2N(C)C1CC2";
            var molecule = sp.ParseSmiles(smiles);
            AssertAtomTypesPerceived(molecule);
            Assert.AreEqual(21, molecule.Atoms.Count);
        }

        [TestMethod()]
        [Timeout(1000)]
        public void TestSmiles6()
        {
            var smiles = "C1(C2(C)(C))C(C)=CCC2C1";
            var molecule = sp.ParseSmiles(smiles);
            AssertAtomTypesPerceived(molecule);
            Assert.AreEqual(10, molecule.Atoms.Count);
        }

        [TestMethod()]
        [Timeout(1000)]
        public void TestSmiles7()
        {
            var smiles = "C1(C=C(C=C(C=C(C=C(C=CC%35=C%36)C%31=C%35C%32=C%33C%36=C%34)C%22=C%31C%23=C%32C%24=C%25C%33=C%26C%34=CC%27=CC%28=CC=C%29)C%14=C%22C%15=C%23C%16=C%24C%17=C%18C%25=C%19C%26=C%27C%20=C%28C%29=C%21)C6=C%14C7=C%15C8=C%16C9=C%17C%12=C%11C%18=C%10C%19=C%20C%21=CC%10=CC%11=CC(C=C%30)=C%12%13)=C(C6=C(C7=C(C8=C(C9=C%13C%30=C5)C5=C4)C4=C3)C3=C2)C2=CC=C1";
            var molecule = sp.ParseSmiles(smiles);
            AssertAtomTypesPerceived(molecule);
            Assert.IsNotNull(molecule);
        }

        [TestMethod()]
        [Timeout(1000)]
        public void TestSmiles8()
        {
            var smiles = "CC1(C(=C(CC(C1)O)C)C=CC(=CC=CC(=CC=CC=C(C=CC=C(C=CC1=C(CC(CC1(C)C)O)C)C)C)C)C)C";
            var molecule = sp.ParseSmiles(smiles);
            AssertAtomTypesPerceived(molecule);
            Assert.IsNotNull(molecule);
        }

        [TestMethod()]
        [Timeout(1000)]
        public void TestSmiles9()
        {
            var smiles = "NC(C(C)C)C(NC(C(C)O)C(NC(C(C)C)C(NC(CCC(N)=O)C(NC(CC([O-])[O-])C(NCC(NC(CC(N)=O)C(NC(Cc1ccccc1)C(NC(CO)C(NC(Cc2ccccc2)C(NC(CO)C(NC(CC(C)C)C(NC(CCC([O-])[O-])C(NC(CO)C(NC(C(C)C)C(NC(CCCC[N+])C(NC(CCCC[N+])C(NC(CC(C)C)C(NC(CCCC[N+])C(NC(CC([O-])[O-])C(NC(CC(C)C)C(NC(CCC(N)=O)C(NC(CCC([O-])[O-])C(N3CCCC3C(NC(CCC(N)=O)C(NC(CCC([O-])[O-])C(N4CCCC4C(NC(CCCNC([N+])[N+])C(NC(C(C)C)C(NCC(NC(CCCC[N+])C(NC(CC(C)C)C(NC(CCCNC([N+])[N+])C(NC(CC(N)=O)C(NC(Cc5ccccc5)C(NC(C)C(N6CCCC6C(NC(C(C)CC)C(N7CCCC7C(NCC(NC(CCC([O-])[O-])C(N8CCCC8C(NC(C(C)C)C(NC(C(C)C)C(N9CCCC9C(NC(C(C)CC)C(NC(CC(C)C)C(NC%19C[S][S]CC(C(NC(CCCC[N+])C(NC(CCC([O-])[O-])C(N%10CCCC%10C(NC(CC(N)=O)C(NC(C)C(NC(CCC(N)=O)C(NC(CCC([O-])[O-])C(NC(C(C)CC)C(NC(CC(C)C)C(NC(CCC(N)=O)C(NC(CCCNC([N+])[N+])C(NC(CC(C)C)C(NC(CCC([O-])[O-])C(NC(CCC([O-])[O-])C(NC(C(C)CC)C(NC(C)C(NC(CCC([O-])[O-])C(NC(CC([O-])[O-])C(N%11CCCC%11C(NCC(NC(C(C)O)C(NC%14C[S][S]CC%13C(NC(C(C)O)C(NCC(NC(C[S][S]CC(C(NC(C)C(NC(Cc%12ccc(O)cc%12)C(NC(C)C(NC(C)C(N%13)=O)=O)=O)=O)=O)NC(=O)C(C(C)CC)NC(=O)C(CCC([O-])[O-])NC%14=O)C(O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)NC(=O)C(CC(C)C)NC(=O)C%15CCCN%15C(=O)C(CCCC[N+])NC(=O)C(CC(C)C)NC(=O)C(CCC([O-])[O-])NC(=O)C(CCC([O-])[O-])NC(=O)C%16CCCN%16C(=O)C(Cc%17ccccc%17)NC(=O)C(CC(N)=O)NC(=O)C%18CCCN%18C(=O)C(CC(N)=O)NC(=O)C(CO)NC%19=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O)=O";
            var molecule = sp.ParseSmiles(smiles);
            AssertAtomTypesPerceived(molecule);
            Assert.IsNotNull(molecule);
        }

        // @cdk.bug 1296113
        [TestMethod()]
        [Timeout(1000)]
        public void TestSFBug1296113()
        {
            var smiles = "S(=O)(=O)(-O)-c1c2c(c(ccc2-N-c2ccccc2)-N=N-c2c3c(c(cc2)-N=N-c2c4c(c(ccc4)-S(=O)(=O)-O)ccc2)cccc3)ccc1";
            var molecule = sp.ParseSmiles(smiles);
            AssertAtomTypesPerceived(molecule);
            Assert.IsNotNull(molecule);
        }

        // @cdk.bug 1324105
        [TestMethod()]
        [Timeout(1000)]
        public void TestAromaticSmiles2()
        {
            var smiles = "n12:n:n:n:c:2:c:c:c:c:1";
            IAtomContainer molecule = LoadExact(smiles);
            AssertAtomTypesPerceived(molecule);
            IEnumerator<IBond> bonds = molecule.Bonds.GetEnumerator();
            while (bonds.MoveNext())
                Assert.IsTrue(bonds.Current.IsAromatic);
        }

        /// <summary>
        /// It is currently ignored because the SMILES
        /// given is invalid: the negative has an implied zero hydrogen count,
        /// making it have an unfilled valency.
        /// </summary>
        [TestMethod(), Ignore()]
        [Timeout(1000)]
        public void TestAromaticSmilesWithCharge()
        {
            var smiles = "c1cc[c-]c1";
            var molecule = sp.ParseSmiles(smiles);
            AssertAtomTypesPerceived(molecule);
            Assert.IsTrue(molecule.Atoms[0].IsAromatic);
            Assert.IsTrue(molecule.Bonds[0].IsAromatic);
        }

        [TestMethod()]
        [Timeout(1000)]
        public void TestAromaticSmiles()
        {
            var smiles = "c1ccccc1";
            var molecule = sp.ParseSmiles(smiles);
            foreach (var bond in molecule.Bonds)
                Assert.IsTrue(bond.IsAromatic);
        }

        // @cdk.bug 630475
        [TestMethod()]
        [Timeout(1000)]
        public void TestSFBug630475()
        {
            var smiles = "CC1(C(=C(CC(C1)O)C)C=CC(=CC=CC(=CC=CC=C(C=CC=C(C=CC1=C(CC(CC1(C)C)O)C)C)C)C)C)C";
            var mol = sp.ParseSmiles(smiles);
            Assert.IsTrue(mol.Atoms.Count > 0);
        }

        // @cdk.bug 585811
        [TestMethod()]
        [Timeout(1000)]
        public void TestSFBug585811()
        {
            var smiles = "CC(C(C8CCC(CC8)=O)C3C4C(CC5(CCC(C9=CC(C=CN%10)=C%10C=C9)CCCC5)C4)C2CCC1CCC7(CCC7)C6(CC6)C1C2C3)=O";
            var mol = sp.ParseSmiles(smiles);
            Assert.IsTrue(mol.Atoms.Count > 0);
        }

        // @cdk.bug 593648
        [TestMethod()]
        [Timeout(1000)]
        public void TestSFBug593648()
        {
            var smiles = "CC1=CCC2CC1C(C)2C";
            var mol = sp.ParseSmiles(smiles);

            IAtomContainer apinene = mol.Builder.NewAtomContainer();
            apinene.Atoms.Add(mol.Builder.NewAtom("C"));
            // 1
            apinene.Atoms.Add(mol.Builder.NewAtom("C"));
            // 2
            apinene.Atoms.Add(mol.Builder.NewAtom("C"));
            // 3
            apinene.Atoms.Add(mol.Builder.NewAtom("C"));
            // 4
            apinene.Atoms.Add(mol.Builder.NewAtom("C"));
            // 5
            apinene.Atoms.Add(mol.Builder.NewAtom("C"));
            // 6
            apinene.Atoms.Add(mol.Builder.NewAtom("C"));
            // 7
            apinene.Atoms.Add(mol.Builder.NewAtom("C"));
            // 8
            apinene.Atoms.Add(mol.Builder.NewAtom("C"));
            // 9
            apinene.Atoms.Add(mol.Builder.NewAtom("C"));
            // 10

            apinene.AddBond(apinene.Atoms[0], apinene.Atoms[1], BondOrder.Double);
            // 1
            apinene.AddBond(apinene.Atoms[1], apinene.Atoms[2], BondOrder.Single);
            // 2
            apinene.AddBond(apinene.Atoms[2], apinene.Atoms[3], BondOrder.Single);
            // 3
            apinene.AddBond(apinene.Atoms[3], apinene.Atoms[4], BondOrder.Single);
            // 4
            apinene.AddBond(apinene.Atoms[4], apinene.Atoms[5], BondOrder.Single);
            // 5
            apinene.AddBond(apinene.Atoms[5], apinene.Atoms[0], BondOrder.Single);
            // 6
            apinene.AddBond(apinene.Atoms[0], apinene.Atoms[6], BondOrder.Single);
            // 7
            apinene.AddBond(apinene.Atoms[3], apinene.Atoms[7], BondOrder.Single);
            // 8
            apinene.AddBond(apinene.Atoms[5], apinene.Atoms[7], BondOrder.Single);
            // 9
            apinene.AddBond(apinene.Atoms[7], apinene.Atoms[8], BondOrder.Single);
            // 10
            apinene.AddBond(apinene.Atoms[7], apinene.Atoms[9], BondOrder.Single);
            // 11

            IsomorphismTester it = new IsomorphismTester(apinene);
            Assert.IsTrue(it.IsIsomorphic(mol.Builder.NewAtomContainer(mol)));
        }

        [TestMethod()]
        [Timeout(1000)]
        public void TestReadingOfTwoCharElements()
        {
            var smiles = "[Na+]";
            var mol = sp.ParseSmiles(smiles);
            Assert.AreEqual(1, mol.Atoms.Count);
            Assert.AreEqual("Na", mol.Atoms[0].Symbol);
        }

        [TestMethod()]
        [Timeout(1000)]
        public void TestReadingOfOneCharElements()
        {
            var smiles = "[K+]";
            var mol = sp.ParseSmiles(smiles);
            Assert.AreEqual(1, mol.Atoms.Count);
            Assert.AreEqual("K", mol.Atoms[0].Symbol);
        }

        [TestMethod()]
        [Timeout(1000)]
        public void TestOrganicSubSetUnderstanding()
        {
            var smiles = "[Ni+2]";
            var mol = sp.ParseSmiles(smiles);
            Assert.AreEqual(1, mol.Atoms.Count);
            Assert.AreEqual("Ni", mol.Atoms[0].Symbol);

            smiles = "Co";
            mol = LoadExact(smiles);
            Assert.AreEqual(2, mol.Atoms.Count);
            Assert.AreEqual("C", mol.Atoms[0].Symbol);
            Assert.AreEqual("O", mol.Atoms[1].Symbol);
        }

        // note we can't kekulise 'Co' (above) but we can kekulise 'Cocc'
        [TestMethod()]
        public void TestOrganicSubSetUnderstanding2()
        {
            var mol = Load("Cocc");
            Assert.AreEqual(BondOrder.Single, mol.Bonds[0].Order);
            Assert.AreEqual(BondOrder.Single, mol.Bonds[1].Order);
            Assert.AreEqual(BondOrder.Double, mol.Bonds[2].Order);
        }

        [TestMethod()]
        [Timeout(1000)]
        public void TestMassNumberReading()
        {
            var smiles = "[13C]";
            var mol = sp.ParseSmiles(smiles);
            Assert.AreEqual(1, mol.Atoms.Count);
            Assert.AreEqual("C", mol.Atoms[0].Symbol);
            Assert.AreEqual(13, mol.Atoms[0].MassNumber.Value);
        }

        [TestMethod()]
        [Timeout(1000)]
        public void TestFormalChargeReading()
        {
            var smiles = "[OH-]";
            var mol = sp.ParseSmiles(smiles);
            Assert.AreEqual(1, mol.Atoms.Count);
            Assert.AreEqual("O", mol.Atoms[0].Symbol);
            Assert.AreEqual(-1, mol.Atoms[0].FormalCharge.Value);
        }

        [TestMethod()]
        [Timeout(1000)]
        public void TestReadingPartionedMolecules()
        {
            var smiles = "[Na+].[OH-]";
            var mol = sp.ParseSmiles(smiles);
            Assert.AreEqual(2, mol.Atoms.Count);
            Assert.AreEqual(0, mol.Bonds.Count);
        }

        [TestMethod()]
        [Timeout(1000)]
        public void TestExplicitSingleBond()
        {
            var smiles = "C-C";
            var mol = sp.ParseSmiles(smiles);
            Assert.AreEqual(2, mol.Atoms.Count);
            Assert.AreEqual(1, mol.Bonds.Count);
            Assert.AreEqual(BondOrder.Single, mol.Bonds[0].Order);
        }

        // @cdk.bug 1175478
        [TestMethod()]
        [Timeout(1000)]
        public void TestSFBug1175478()
        {
            var smiles = "c1cc-2c(cc1)C(c3c4c2onc4c(cc3N5CCCC5)N6CCCC6)=O";
            var mol = sp.ParseSmiles(smiles);
            Assert.AreEqual(27, mol.Atoms.Count);
            Assert.AreEqual(32, mol.Bonds.Count);
        }

        [TestMethod()]
        [Timeout(1000)]
        public void TestUnkownAtomType()
        {
            var smiles = "*C";
            var mol = sp.ParseSmiles(smiles);
            Assert.AreEqual(2, mol.Atoms.Count);
            Assert.AreEqual(1, mol.Bonds.Count);
            Assert.IsTrue(mol.Atoms[0] is IPseudoAtom);
            Assert.IsFalse(mol.Atoms[1] is IPseudoAtom);

            smiles = "[*]C";
            mol = sp.ParseSmiles(smiles);
            Assert.AreEqual(2, mol.Atoms.Count);
            Assert.AreEqual(1, mol.Bonds.Count);
            Assert.IsTrue(mol.Atoms[0] is IPseudoAtom);
            Assert.IsFalse(mol.Atoms[1] is IPseudoAtom);
        }

        // @cdk.bug 2596061
        [TestMethod()]
        public void TestUnknownAtomType2()
        {
            var smiles = "[12*H2-]";
            var mol = sp.ParseSmiles(smiles);
            Assert.AreEqual(1, mol.Atoms.Count);
            Assert.AreEqual(0, mol.Bonds.Count);
            Assert.IsTrue(mol.Atoms[0] is IPseudoAtom);
            Assert.AreEqual(12, mol.Atoms[0].MassNumber.Value);
            Assert.AreEqual(2, mol.Atoms[0].ImplicitHydrogenCount.Value);
            Assert.AreEqual(-1, mol.Atoms[0].FormalCharge.Value);
        }

        [TestMethod()]
        [Timeout(1000)]
        public void TestBondCreation()
        {
            var smiles = "CC";
            var mol = sp.ParseSmiles(smiles);
            Assert.AreEqual(2, mol.Atoms.Count);
            Assert.AreEqual(1, mol.Bonds.Count);

            smiles = "cc";
            mol = sp.ParseSmiles(smiles);
            Assert.AreEqual(2, mol.Atoms.Count);
            Assert.AreEqual(1, mol.Bonds.Count);
        }

        // @cdk.bug 784433
        [TestMethod()]
        [Timeout(1000)]
        public void TestSFBug784433()
        {
            var smiles = "c1cScc1";
            var mol = sp.ParseSmiles(smiles);
            Assert.AreEqual(5, mol.Atoms.Count);
            Assert.AreEqual(5, mol.Bonds.Count);
        }

        // @cdk.bug 873783.
        [TestMethod()]
        [Timeout(1000)]
        public void TestProton()
        {
            var smiles = "[H+]";
            var mol = sp.ParseSmiles(smiles);
            Assert.AreEqual(1, mol.Atoms.Count);
            Assert.AreEqual(1, mol.Atoms[0].FormalCharge.Value);
        }

        // @cdk.bug 881330.
        [TestMethod()]
        [Timeout(1000)]
        public void TestSMILESFromXYZ()
        {
            var smiles = "C.C.N.[Co].C.C.C.[H].[He].[H].[H].[H].[H].C.C.[H].[H].[H].[H].[H]";
            var mol = sp.ParseSmiles(smiles);
            Assert.AreEqual(20, mol.Atoms.Count);
        }

        [TestMethod()]
        [Timeout(1000)]
        public void TestSingleBracketH()
        {
            var smiles = "[H]";
            var mol = sp.ParseSmiles(smiles);
            Assert.AreEqual(1, mol.Atoms.Count);
        }

        [TestMethod()]
        public void TestSingleH()
        {
            // Beam allows bare 'H' - this is a common typo for '[H]' - there is
            // a 'strict' option which won't allow these but this isn't exposed
            // in the public API yet
            var mol = Load("H");
            Assert.AreEqual(1, mol.Atoms[0].AtomicNumber);
            Assert.AreEqual(1, mol.Atoms.Count);
        }

        [TestMethod()]
        public void TestSingleD()
        {
            // Beam allows bare 'D' - this is a common typo for '[2H]' - there is
            // a 'strict' option which won't allow these but this isn't exposed
            // in the public API yet
            var mol = Load("D");
            Assert.AreEqual(1, mol.Atoms.Count);
            Assert.AreEqual(1, mol.Atoms[0].AtomicNumber);
            Assert.AreEqual(2, mol.Atoms[0].MassNumber);
        }

        [TestMethod()]
        public void TestSingleT()
        {
            // Beam allows bare 'T' - this is a common typo for '[3H]' - there is
            // a 'strict' option which won't allow these but this isn't exposed
            // in the public API yet
            var mol = Load("T");
            Assert.AreEqual(1, mol.Atoms.Count);
            Assert.AreEqual(1, mol.Atoms[0].AtomicNumber);
            Assert.AreEqual(3, mol.Atoms[0].MassNumber);
        }

        // @cdk.bug 862930.
        [TestMethod()]
        [Timeout(1000)]
        public void TestHydroxonium()
        {
            var smiles = "[H][O+]([H])[H]";
            var mol = sp.ParseSmiles(smiles);
            Assert.AreEqual(4, mol.Atoms.Count);
        }

        // @cdk.bug 809412
        [TestMethod()]
        [Timeout(1000)]
        public void TestSFBug809412()
        {
            var smiles = "Nc4cc3[n+](c2c(c1c(cccc1)cc2)nc3c5c4cccc5)c6c7c(ccc6)cccc7";
            var mol = sp.ParseSmiles(smiles);
            Assert.AreEqual(33, mol.Atoms.Count);
        }

        /// <summary>
        /// A bug found with JCP.
        /// </summary>
        // @cdk.bug 956926
        [TestMethod()]
        [Timeout(1000)]
        public void TestSFBug956926()
        {
            var smiles = "[c+]1ccccc1";
            // C6H5+, phenyl cation
            var mol = sp.ParseSmiles(smiles);
            AssertAtomTypesPerceived(mol);
            Assert.AreEqual(6, mol.Atoms.Count);
            Assert.AreEqual(1, mol.Atoms[0].FormalCharge.Value);

            // I can also check whether all carbons have exact two neighbors
            for (int i = 0; i < mol.Atoms.Count; i++)
            {
                Assert.AreEqual(2, mol.GetConnectedBonds(mol.Atoms[i]).Count());
            }
            // and the number of implicit hydrogens
            int hCount = 0;
            for (int i = 0; i < mol.Atoms.Count; i++)
            {
                hCount += mol.Atoms[i].ImplicitHydrogenCount.Value;
            }
            Assert.AreEqual(5, hCount);
        }

        /// <summary>
        /// A bug found with JCP.
        /// </summary>
        /// <seealso cref="TestPyrrole"/>
        // @cdk.bug   956929
        // @cdk.inchi InChI=1/C4H5N/c1-2-4-5-3-1/h1-5H
        [TestMethod()]
        [Timeout(1000)]
        public void TestPyrrole()
        {
            var smiles = "c1ccc[NH]1";
            var mol = sp.ParseSmiles(smiles);
            for (int i = 0; i < mol.Atoms.Count; i++)
            {
                if (mol.Atoms[i].Symbol.Equals("N"))
                {
                    Assert.AreEqual(BondOrder.Single,
                            ((IBond)mol.GetConnectedBonds(mol.Atoms[i]).ElementAt(0)).Order);
                    Assert.AreEqual(BondOrder.Single,
                            ((IBond)mol.GetConnectedBonds(mol.Atoms[i]).ElementAt(1)).Order);
                }
            }
        }

        // @cdk.bug 2679607
        [TestMethod()]
        [Timeout(1000)]
        public void TestHardCodedHydrogenCount()
        {
            var smiles = "c1ccc[NH]1";
            var mol = sp.ParseSmiles(smiles);
            Assert.AreEqual(1, mol.Atoms[4].ImplicitHydrogenCount.Value);

            smiles = "[n]1cc[nH]c1";
            mol = sp.ParseSmiles(smiles);
            Assert.AreEqual(1, mol.Atoms[4].ImplicitHydrogenCount.Value);
            Assert.AreEqual(0, mol.Atoms[0].ImplicitHydrogenCount.Value);

            smiles = "[nH]1cc[n]c1";
            mol = sp.ParseSmiles(smiles);
            Assert.AreEqual(1, mol.Atoms[0].ImplicitHydrogenCount.Value);
            Assert.AreEqual(0, mol.Atoms[3].ImplicitHydrogenCount.Value);
        }

        // @cdk.bug 2679607
        [TestMethod()]
        public void TestHardCodedHydrogenCount2()
        {
            var smiles = "[CH2]CNC";
            var mol = sp.ParseSmiles(smiles);
            Assert.AreEqual(2, mol.Atoms[0].ImplicitHydrogenCount.Value);
        }

        /// <summary>
        /// A bug found with JCP.
        /// </summary>
        // @cdk.bug 956929
        [TestMethod()]
        [Timeout(1000)]
        public void TestSFBug956929()
        {
            var smiles = "Cn1cccc1";
            var mol = sp.ParseSmiles(smiles);
            Assert.AreEqual(6, mol.Atoms.Count);
            // I can also check whether the total neighbor count around the
            // nitrogen is 3, all single bonded
            IAtom nitrogen = mol.Atoms[1];
            // the second atom
            Assert.AreEqual("N", nitrogen.Symbol);
            IEnumerable<IBond> bondsList = mol.GetConnectedBonds(nitrogen);
            Assert.AreEqual(3, bondsList.Count());
            int totalBondOrder = BondManipulator.GetSingleBondEquivalentSum(bondsList);
            Assert.AreEqual(3.0, totalBondOrder, 0.001);
        }

        /// <summary>
        /// A bug found with JCP.
        /// </summary>
        // @cdk.bug 956921
        [TestMethod()]
        [Timeout(1000)]
        public void TestSFBug956921()
        {
            var smiles = "[cH-]1cccc1";
            var mol = sp.ParseSmiles(smiles);
            Assert.AreEqual(5, mol.Atoms.Count);
            // each atom should have 1 implicit hydrogen, and two neighbors
            foreach (var atomi in mol.Atoms)
            {
                Assert.AreEqual(1, atomi.ImplicitHydrogenCount.Value);
                Assert.AreEqual(2, mol.GetConnectedBonds(atomi).Count());
            }
            // and the first atom should have a negative charge
            Assert.AreEqual(-1, mol.Atoms[0].FormalCharge.Value);
        }

        // @cdk.bug 1274464
        [TestMethod()]
        [Timeout(1000)]
        public void TestSFBug1274464()
        {
            IAtomContainer fromSmiles = CDK.SmilesParser.ParseSmiles("C1=CC=CC=C1");
            IAtomContainer fromFactory = TestMoleculeFactory.MakeBenzene();
            var hAdder = CDK.HydrogenAdder;
            var matcher = CDK.AtomTypeMatcher;
            foreach (var atom in fromFactory.Atoms)
            {
                IAtomType type = matcher.FindMatchingAtomType(fromFactory, atom);
                AtomTypeManipulator.Configure(atom, type);
                hAdder.AddImplicitHydrogens(fromFactory, atom);
            }

            MakeAtomType(fromSmiles);
            Aromaticity.CDKLegacy.Apply(fromSmiles);
            Aromaticity.CDKLegacy.Apply(fromFactory);
            bool result = new UniversalIsomorphismTester().IsIsomorph(fromFactory, fromSmiles);
            Assert.IsTrue(result);
        }

        // @cdk.bug 1095696
        [TestMethod()]
        [Timeout(1000)]
        public void TestSFBug1095696()
        {
            var smiles = "Nc1ncnc2[nH]cnc12";
            var mol = sp.ParseSmiles(smiles);
            Assert.AreEqual(10, mol.Atoms.Count);
            Assert.AreEqual("N", mol.Atoms[6].Symbol);
            Assert.AreEqual(1, mol.Atoms[6].ImplicitHydrogenCount.Value);
        }

        /// <summary>
        ///  Example taken from 'Handbook of Chemoinformatics', Gasteiger, 2003, page 89
        ///  (Part I).
        /// </summary>
        [TestMethod()]
        [Timeout(1000)]
        public void TestNonBond()
        {
            var sodiumPhenoxide = "c1cc([O-].[Na+])ccc1";
            var mol = sp.ParseSmiles(sodiumPhenoxide);
            Assert.AreEqual(8, mol.Atoms.Count);
            Assert.AreEqual(7, mol.Bonds.Count);

            var fragments = ConnectivityChecker.PartitionIntoMolecules(mol).ToReadOnlyList();
            int fragmentCount = fragments.Count();
            Assert.AreEqual(2, fragmentCount);
            var mol1 = fragments[0];
            var mol2 = fragments[1];
            // one should have one atom, the other seven atoms
            // in any order, so just test the difference
            Assert.AreEqual(6, Math.Abs(mol1.Atoms.Count - mol2.Atoms.Count));
        }

        /// <summary>
        ///  Example taken from 'Handbook of Chemoinformatics', Gasteiger, 2003, page 89
        ///  (Part I).
        /// </summary>
        [TestMethod()]
        [Timeout(1000)]
        public void TestConnectedByRingClosure()
        {
            var sodiumPhenoxide = "C1.O2.C12";
            var mol = sp.ParseSmiles(sodiumPhenoxide);
            Assert.AreEqual(3, mol.Atoms.Count);
            Assert.AreEqual(2, mol.Bonds.Count);

            var fragments = ConnectivityChecker.PartitionIntoMolecules(mol).ToReadOnlyList();
            int fragmentCount = fragments.Count();
            Assert.AreEqual(1, fragmentCount);
            var mol1 = fragments[0];
            Assert.AreEqual(3, mol1.Atoms.Count);
        }

        [TestMethod()]
        public void TestConnectedByRingClosure_TwoAtom()
        {
            var methanol = "C1.O1";
            var mol = sp.ParseSmiles(methanol);
            Assert.AreEqual(2, mol.Atoms.Count);
            Assert.AreEqual(1, mol.Bonds.Count);

            var fragments = ConnectivityChecker.PartitionIntoMolecules(mol).ToReadOnlyList();
            int fragmentCount = fragments.Count();
            Assert.AreEqual(1, fragmentCount);
            var mol1 = fragments[0];
            Assert.AreEqual(2, mol1.Atoms.Count);
        }

        /// <summary>
        ///  Example taken from 'Handbook of Chemoinformatics', Gasteiger, 2003, page 89
        ///  (Part I).
        /// </summary>
        [TestMethod()]
        [Timeout(1000)]
        public void TestReaction()
        {
            string reactionSmiles = "O>>[H+].[OH-]";
            IReaction reaction = sp.ParseReactionSmiles(reactionSmiles);
            Assert.AreEqual(1, reaction.Reactants.Count);
            Assert.AreEqual(2, reaction.Products.Count);
        }

        [TestMethod()]
        public void NoReactants()
        {
            IReaction reaction = sp.ParseReactionSmiles(">>C");
            Assert.AreEqual(0, reaction.Reactants.Count);
            Assert.AreEqual(1, reaction.Products.Count);
        }

        [TestMethod()]
        public void NoProducts()
        {
            IReaction reaction = sp.ParseReactionSmiles("C>>");
            Assert.AreEqual(1, reaction.Reactants.Count);
            Assert.AreEqual(0, reaction.Products.Count);
        }

        [TestMethod()]
        public void NoReaction()
        {
            IReaction reaction = sp.ParseReactionSmiles(">>");
            Assert.AreEqual(0, reaction.Reactants.Count);
            Assert.AreEqual(0, reaction.Products.Count);
        }

        [TestMethod()]
        public void OnlyAgents()
        {
            IReaction reaction = sp.ParseReactionSmiles(">C>");
            Assert.AreEqual(0, reaction.Reactants.Count);
            Assert.AreEqual(1, reaction.Agents.Count);
            Assert.AreEqual(0, reaction.Products.Count);
        }

        /// <summary>
        ///  Example taken from 'Handbook of Chemoinformatics', Gasteiger, 2003, page 90
        ///  (Part I).
        /// </summary>
        [TestMethod()]
        [Timeout(1000)]
        public void TestReactionWithAgents()
        {
            string reactionSmiles = "CCO.CC(=O)O>[H+]>CC(=O)OCC.O";
            IReaction reaction = sp.ParseReactionSmiles(reactionSmiles);
            Assert.AreEqual(2, reaction.Reactants.Count);
            Assert.AreEqual(2, reaction.Products.Count);
            Assert.AreEqual(1, reaction.Agents.Count);

            Assert.AreEqual(1, reaction.Agents[0].Atoms.Count);
        }

        [TestMethod()]
        [Timeout(1000)]
        public void TestImplicitHydrogenCount()
        {
            var smiles = "C";
            var mol = sp.ParseSmiles(smiles);
            Assert.AreEqual(1, mol.Atoms.Count);
            Assert.AreEqual(4, mol.Atoms[0].ImplicitHydrogenCount.Value);
        }

        // @cdk.bug 2028780
        [TestMethod()]
        [Timeout(1000)]
        public void TestTungsten()
        {
            var smiles = "[W]";
            var mol = sp.ParseSmiles(smiles);
            AssertAtomTypesPerceived(mol);
            Assert.AreEqual(1, mol.Atoms.Count);
            Assert.AreEqual("W", mol.Atoms[0].Symbol);
        }

        [TestMethod()]
        [Timeout(1000)]
        public void TestImplicitHydrogenCount2()
        {
            var smiles = "CC";
            var mol = sp.ParseSmiles(smiles);
            Assert.AreEqual(2, mol.Atoms.Count);
            Assert.AreEqual(3, mol.Atoms[0].ImplicitHydrogenCount.Value);
        }

        [TestMethod()]
        [Timeout(1000)]
        public void TestImplicitHydrogenCount2b()
        {
            var smiles = "C=C";
            var mol = sp.ParseSmiles(smiles);
            Assert.AreEqual(2, mol.Atoms.Count);
            Assert.AreEqual(2, mol.Atoms[0].ImplicitHydrogenCount.Value);
        }

        [TestMethod()]
        [Timeout(1000)]
        public void TestImplicitHydrogenCount2c()
        {
            var smiles = "C#C";
            var mol = sp.ParseSmiles(smiles);
            Assert.AreEqual(2, mol.Atoms.Count);
            Assert.AreEqual(1, mol.Atoms[0].ImplicitHydrogenCount.Value);
        }

        [TestMethod()]
        [Timeout(1000)]
        public void TestImplicitHydrogenCount3()
        {
            var smiles = "CCC";
            var mol = sp.ParseSmiles(smiles);
            Assert.AreEqual(3, mol.Atoms.Count);
            Assert.AreEqual(2, mol.Atoms[1].ImplicitHydrogenCount.Value);
        }

        [TestMethod()]
        [Timeout(1000)]
        public void TestImplicitHydrogenCount4()
        {
            var smiles = "C1CCCCC1";
            var mol = sp.ParseSmiles(smiles);
            Assert.AreEqual(6, mol.Atoms.Count);
            Assert.AreEqual(2, mol.Atoms[0].ImplicitHydrogenCount.Value);
        }

        [TestMethod()]
        [Timeout(1000)]
        public void TestImplicitHydrogenCount4a()
        {
            var smiles = "c1=cc=cc=c1";
            var mol = sp.ParseSmiles(smiles);
            Assert.AreEqual(6, mol.Atoms.Count);
            Assert.AreEqual(1, mol.Atoms[0].ImplicitHydrogenCount.Value);
        }

        [TestMethod()]
        [Timeout(1000)]
        public void TestImplicitHydrogenCount4b()
        {
            var smiles = "c1ccccc1";
            var mol = sp.ParseSmiles(smiles);
            Assert.AreEqual(6, mol.Atoms.Count);
            Assert.AreEqual(1, mol.Atoms[0].ImplicitHydrogenCount.Value);
        }

        [TestMethod()]
        [Timeout(1000)]
        public void TestHOSECodeProblem()
        {
            var smiles = "CC=CBr";
            var mol = sp.ParseSmiles(smiles);
            Assert.AreEqual(4, mol.Atoms.Count);
            Assert.AreEqual("Br", mol.Atoms[3].Symbol);
        }

        [TestMethod()]
        [Timeout(1000)]
        public void TestPyridine()
        {
            var mol = Load("c1ccncc1");
            MakeAtomType(mol);
            Assert.AreEqual(6, mol.Atoms.Count);
            // I can also check whether the total neighbor count around the
            // nitrogen is 3, all single bonded
            IAtom nitrogen = mol.Atoms[3];
            // the second atom
            Assert.AreEqual("N", nitrogen.Symbol);
            foreach (var atom in mol.Atoms)
                Assert.AreEqual(Hybridization.SP2, atom.Hybridization);
        }

        // @cdk.bug 1306780
        [TestMethod()]
        [Timeout(1000)]
        public void TestParseK()
        {
            var mol = sp.ParseSmiles("C=CCC(=NOS(=O)(=O)[O-])SC1OC(CO)C(O)C(O)C1(O).[Na+]");
            Assert.IsNotNull(mol);
            Assert.AreEqual(23, mol.Atoms.Count);
            mol = sp.ParseSmiles("C=CCC(=NOS(=O)(=O)[O-])SC1OC(CO)C(O)C(O)C1(O).[K]");
            Assert.IsNotNull(mol);
            AssertAtomTypesPerceived(mol);
            Assert.AreEqual(23, mol.Atoms.Count);
            mol = sp.ParseSmiles("C=CCC(=NOS(=O)(=O)[O-])SC1OC(CO)C(O)C(O)C1(O).[K+]");
            Assert.IsNotNull(mol);
            AssertAtomTypesPerceived(mol);
            Assert.AreEqual(23, mol.Atoms.Count);
        }

        // @cdk.bug 1459299
        [TestMethod()]
        [Timeout(1000)]
        public void TestBug1459299()
        {
            var mol = sp.ParseSmiles("Cc1nn(C)cc1[C@H]2[C@H](C(=O)N)C(=O)C[C@@](C)(O)[C@@H]2C(=O)N");
            Assert.IsNotNull(mol);
            Assert.AreEqual(22, mol.Atoms.Count);
        }

        /// <summary>
        // @cdk.bug 1365547
        /// </summary>
        [TestMethod()]
        [Timeout(1000)]
        public void TestBug1365547()
        {
            var mol = LoadExact("c2ccc1[nH]ccc1c2");
            Assert.IsNotNull(mol);
            Assert.AreEqual(9, mol.Atoms.Count);
            Assert.IsTrue(mol.Bonds[0].IsAromatic);
        }

        // @cdk.bug 1365547
        [TestMethod()]
        [Timeout(1000)]
        public void TestBug1365547_2()
        {
            var mol = LoadExact("[H]c1c([H])c(c([H])c2c([H])c([H])n([H])c12)Br");
            Assert.IsNotNull(mol);
            Assert.AreEqual(16, mol.Atoms.Count);
            Assert.AreEqual(17, mol.Bonds.Count);
            for (int i = 0; i < 17; i++)
            {
                IBond bond = mol.Bonds[i];
                if (bond.Begin.Symbol.Equals("H") || bond.Begin.Symbol.Equals("Br")
                        || bond.End.Symbol.Equals("H") || bond.End.Symbol.Equals("Br"))
                {
                    Assert.IsFalse(bond.IsAromatic);
                }
                else
                {
                    Assert.IsTrue(bond.IsAromatic);
                }
            }
        }

        // @cdk.bug 1235852
        [TestMethod()]
        [Timeout(1000)]
        public void TestBug1235852()
        {
            //                                   0 1 234 56 7 890 12 3456 78
            var mol = sp.ParseSmiles("O=C(CCS)CC(C)CCC2Cc1ccsc1CC2");
            Assert.IsNotNull(mol);
            Assert.AreEqual(19, mol.Atoms.Count);
            Assert.AreEqual(20, mol.Bonds.Count);
            // test only option for delocalized bond system
            Assert.AreEqual(4.0, mol.GetBondOrderSum(mol.Atoms[12]), 0.001);
            Assert.AreEqual(3.0, mol.GetBondOrderSum(mol.Atoms[13]), 0.001);
            Assert.AreEqual(3.0, mol.GetBondOrderSum(mol.Atoms[14]), 0.001);
            Assert.AreEqual(2.0, mol.GetBondOrderSum(mol.Atoms[15]), 0.001);
            Assert.AreEqual(4.0, mol.GetBondOrderSum(mol.Atoms[16]), 0.001);
        }

        // @cdk.bug 1519183
        [TestMethod()]
        [Timeout(1000)]
        public void TestBug1519183()
        {
            //                             0    12345  6
            var mol = sp.ParseSmiles("c%101ccccc1.O%10"); // phenol
            Assert.IsNotNull(mol);
            Assert.AreEqual(7, mol.Atoms.Count);
            Assert.AreEqual(7, mol.Bonds.Count);
        }

        // @cdk.bug 1530926
        [TestMethod()]
        [Timeout(1000)]
        public void TestBug1530926()
        {
            //                               0      12345   6
            var mol = LoadExact("[n+]%101ccccc1.[O-]%10");
            Assert.IsNotNull(mol);
            Assert.AreEqual(7, mol.Atoms.Count);
            Assert.AreEqual(7, mol.Bonds.Count);
            for (int i = 0; i < 7; i++)
            {
                IBond bond = mol.Bonds[i];
                if (bond.Begin.Symbol.Equals("O") || bond.End.Symbol.Equals("O"))
                {
                    Assert.IsFalse(bond.IsAromatic);
                }
                else
                {
                    Assert.IsTrue(bond.IsAromatic);
                }
            }
        }

        // @cdk.bug 1541333
        [TestMethod()]
        [Timeout(1000)]
        public void TestBug1541333()
        {
            //                              01  2 345  67  8 9 0 12 3 4  5 67 89  0  1 2
            var mol1 = sp.ParseSmiles("OC(=O)CSC1=NC=2C=C(C=CC2N1C=3C=CC=CC3)N(=O)O");
            Assert.IsNotNull(mol1);
            Assert.AreEqual(23, mol1.Atoms.Count);
            Assert.AreEqual(25, mol1.Bonds.Count);
            var mol2 = sp.ParseSmiles("OC(=O)CSc1nc2cc(ccc2n1c3ccccc3)N(=O)O");
            Assert.IsNotNull(mol2);
            Assert.AreEqual(23, mol2.Atoms.Count);
            Assert.AreEqual(25, mol2.Bonds.Count);
            // do some checking
            Assert.AreEqual(BondOrder.Double, mol1.GetBond(mol1.Atoms[1], mol1.Atoms[2]).Order);
            Assert.AreEqual(BondOrder.Double, mol2.GetBond(mol2.Atoms[1], mol2.Atoms[2]).Order);
            MakeAtomType(mol1);
            MakeAtomType(mol2);
            Aromaticity.CDKLegacy.Apply(mol1);
            Aromaticity.CDKLegacy.Apply(mol2);
            Assert.IsTrue(mol1.Bonds[7].IsAromatic);
            Assert.IsTrue(mol2.Bonds[7].IsAromatic);
        }

        // @cdk.bug 1719287
        [TestMethod()]
        [Timeout(1000)]
        public void TestBug1719287()
        {
            //                              01  2  3  4  5 67 8
            var mol = sp
                    .ParseSmiles("OC(=O)[C@@H](N)CC[S+1](C)C[C@@H](O1)[C@@H](O)[C@@H](O)[C@@H]1n(c3)c(n2)c(n3)c(N)nc2");
            Assert.IsNotNull(mol);
            Assert.AreEqual(27, mol.Atoms.Count);
            Assert.AreEqual(29, mol.Bonds.Count);
            Assert.AreEqual(1, mol.Atoms[7].FormalCharge.Value);
        }

        /// <summary>
        /// Test for bug #1503541 "Problem with SMILES parsing". All SMILES in the test
        /// should result in a benzene molecule. Sometimes only a Cyclohexa-dien was
        /// created.
        /// </summary>
        // @cdk.bug 1503541
        [TestMethod()]
        [Timeout(1000)]
        public void TestBug1503541()
        {
            //                              0  1 23 45
            var mol = sp.ParseSmiles("C=1C=CC=CC=1"); // benzene #1
            Assert.IsNotNull(mol);
            Assert.AreEqual(6, mol.Atoms.Count);
            Assert.AreEqual(6, mol.Bonds.Count);
            // test only option for delocalized bond system
            Assert.AreEqual(3.0, mol.GetBondOrderSum(mol.Atoms[0]), 0.001);
            Assert.AreEqual(3.0, mol.GetBondOrderSum(mol.Atoms[1]), 0.001);
            Assert.AreEqual(3.0, mol.GetBondOrderSum(mol.Atoms[2]), 0.001);
            Assert.AreEqual(3.0, mol.GetBondOrderSum(mol.Atoms[3]), 0.001);
            Assert.AreEqual(3.0, mol.GetBondOrderSum(mol.Atoms[4]), 0.001);
            Assert.AreEqual(3.0, mol.GetBondOrderSum(mol.Atoms[5]), 0.001);

            //                              0 1 23 45
            mol = sp.ParseSmiles("C1C=CC=CC=1"); // benzene #2
            Assert.IsNotNull(mol);
            Assert.AreEqual(6, mol.Atoms.Count);
            Assert.AreEqual(6, mol.Bonds.Count);
            // test only option for delocalized bond system
            Assert.AreEqual(3.0, mol.GetBondOrderSum(mol.Atoms[0]), 0.001);
            Assert.AreEqual(3.0, mol.GetBondOrderSum(mol.Atoms[1]), 0.001);
            Assert.AreEqual(3.0, mol.GetBondOrderSum(mol.Atoms[2]), 0.001);
            Assert.AreEqual(3.0, mol.GetBondOrderSum(mol.Atoms[3]), 0.001);
            Assert.AreEqual(3.0, mol.GetBondOrderSum(mol.Atoms[4]), 0.001);
            Assert.AreEqual(3.0, mol.GetBondOrderSum(mol.Atoms[5]), 0.001);

            //                              0  1 23 45
            mol = sp.ParseSmiles("C=1C=CC=CC1"); // benzene #3
            Assert.IsNotNull(mol);
            Assert.AreEqual(6, mol.Atoms.Count);
            Assert.AreEqual(6, mol.Bonds.Count);
            // test only option for delocalized bond system
            Assert.AreEqual(3.0, mol.GetBondOrderSum(mol.Atoms[0]), 0.001);
            Assert.AreEqual(3.0, mol.GetBondOrderSum(mol.Atoms[1]), 0.001);
            Assert.AreEqual(3.0, mol.GetBondOrderSum(mol.Atoms[2]), 0.001);
            Assert.AreEqual(3.0, mol.GetBondOrderSum(mol.Atoms[3]), 0.001);
            Assert.AreEqual(3.0, mol.GetBondOrderSum(mol.Atoms[4]), 0.001);
            Assert.AreEqual(3.0, mol.GetBondOrderSum(mol.Atoms[5]), 0.001);

            //                              0  12 34 5
            mol = sp.ParseSmiles("C1=CC=CC=C1"); // benzene #4
            Assert.IsNotNull(mol);
            Assert.AreEqual(6, mol.Atoms.Count);
            Assert.AreEqual(6, mol.Bonds.Count);
            // test only option for delocalized bond system
            Assert.AreEqual(3.0, mol.GetBondOrderSum(mol.Atoms[0]), 0.001);
            Assert.AreEqual(3.0, mol.GetBondOrderSum(mol.Atoms[1]), 0.001);
            Assert.AreEqual(3.0, mol.GetBondOrderSum(mol.Atoms[2]), 0.001);
            Assert.AreEqual(3.0, mol.GetBondOrderSum(mol.Atoms[3]), 0.001);
            Assert.AreEqual(3.0, mol.GetBondOrderSum(mol.Atoms[4]), 0.001);
            Assert.AreEqual(3.0, mol.GetBondOrderSum(mol.Atoms[5]), 0.001);
        }

        /// <summary>
        /// Test case for bug #1783367 "SmilesParser incorrectly assigns double bonds".
        /// "C=%10C=CC=C%02C=%10N(C)CCC%02" was parsed incorrectly whereas "C=1C=CC=C%02C=1N(C)CCC%02"
        /// was parsed correctly. There was a bug with parsing "C=%10".
        /// </summary>
        // Author: Andreas Schueller <a.schueller@chemie.uni-frankfurt.de>
        // @cdk.bug 1783367
        [TestMethod()]
        [Timeout(1000)]
        public void TestBug1783367()
        {
            var smiles = "C=%10C=CC=C%02C=%10N(C)CCC%02";
            var mol = sp.ParseSmiles(smiles);
            Assert.AreEqual(BondOrder.Single, mol.Bonds[0].Order);
        }

        // @cdk.bug 1783547
        [TestMethod()]
        [Timeout(1000)]
        public void TestBug1783547()
        {
            // easy case
            var smiles = "c1ccccc1C1=CC=CC=C1";
            var mol = LoadExact(smiles);
            Assert.IsTrue(mol.Bonds[0].IsAromatic);
            Assert.IsTrue(mol.Bonds[1].IsAromatic);
            Assert.IsTrue(mol.Bonds[2].IsAromatic);
            Assert.IsTrue(mol.Bonds[3].IsAromatic);

            // harder case
            string smiles2 = "C%21=%01C=CC=C%02C=%01N(C)CCC%02.C%21c%02ccccc%02";
            IAtomContainer mol2 = LoadExact(smiles2);
            Assert.IsTrue(mol2.Bonds[16].IsAromatic);
            Assert.IsTrue(mol2.Bonds[17].IsAromatic);
            Assert.IsTrue(mol2.Bonds[18].IsAromatic);
            Assert.IsTrue(mol2.Bonds[19].IsAromatic);
        }

        /// <summary>
        /// Test case for bug #1783546 "Lost aromaticity in SmilesParser with Benzene".
        /// SMILES like "C=1C=CC=CC=1" which end in "=1" were incorrectly parsed, the ring
        /// closure double bond got lost.
        /// </summary>
        // @cdk.bug 1783546
        [TestMethod()]
        [Timeout(1000)]
        public void TestBug1783546()
        {
            var smiles = "C=1C=CC=CC=1";
            var mol = sp.ParseSmiles(smiles);
            Assert.AreEqual(BondOrder.Single, mol.GetBond(mol.Atoms[0], mol.Atoms[1]).Order);
            Assert.AreEqual(BondOrder.Double, mol.GetBond(mol.Atoms[1], mol.Atoms[2]).Order);
            Assert.AreEqual(BondOrder.Single, mol.GetBond(mol.Atoms[2], mol.Atoms[3]).Order);
            Assert.AreEqual(BondOrder.Double, mol.GetBond(mol.Atoms[3], mol.Atoms[4]).Order);
            Assert.AreEqual(BondOrder.Single, mol.GetBond(mol.Atoms[4], mol.Atoms[5]).Order);
            Assert.AreEqual(BondOrder.Double, mol.GetBond(mol.Atoms[5], mol.Atoms[0]).Order);
        }

        [TestMethod()]
        public void TestChargedAtoms()
        {
            var smiles = "[C-]#[O+]";
            var mol = sp.ParseSmiles(smiles);
            Assert.AreEqual(2, mol.Atoms.Count);
            Assert.AreEqual(BondOrder.Triple, mol.Bonds[0].Order);
            Assert.AreEqual(-1, mol.Atoms[0].FormalCharge.Value);
            Assert.AreEqual(1, mol.Atoms[1].FormalCharge.Value);
        }

        // @cdk.bug 1872969
        [TestMethod()]
        public void Bug1872969()
        {
            var smiles = "CS(=O)(=O)[O-].[Na+]";
            var mol = sp.ParseSmiles(smiles);
            MakeAtomType(mol);
            for (int i = 0; i < 6; i++)
            {
                Assert.IsNotNull(mol.Atoms[i].AtomTypeName);
            }
        }

        // @cdk.bug 1875949
        [TestMethod()]
        public void TestResonanceStructure()
        {
            var smiles = "[F+]=C-[C-]";
            var mol = sp.ParseSmiles(smiles);
            Assert.AreEqual(3, mol.Atoms.Count);
            Assert.AreEqual(BondOrder.Double, mol.Bonds[0].Order);
            Assert.AreEqual(+1, mol.Atoms[0].FormalCharge.Value);
            Assert.AreEqual(-1, mol.Atoms[2].FormalCharge.Value);
        }

        // @cdk.bug 1879589
        [TestMethod()]
        public void TestSP2HybridizedSulphur()
        {
            var smiles = "[s+]1c2c(nc3c1cccc3)cccc2";
            var mol = Load(smiles);
            MakeAtomType(mol);
            Aromaticity.CDKLegacy.Apply(mol);
            AssertAtomTypesPerceived(mol);
            foreach (var atom in mol.Atoms)
            {
                Assert.AreEqual(Hybridization.SP2, atom.Hybridization);
                Assert.IsTrue(atom.IsAromatic);
            }
        }

        [TestMethod()]
        public void TestMercaptan()
        {
            var mol = sp.ParseSmiles("C=CCS");
            AssertAtomTypesPerceived(mol);
        }

        // @cdk.bug 1957958
        [TestMethod()]
        public void Test3amino4methylpyridine()
        {
            var mol = sp.ParseSmiles("c1c(C)c(N)cnc1");
            AssertAtomTypesPerceived(mol);

            AtomContainerManipulator.PercieveAtomTypesAndConfigureAtoms(mol);
            bool isaromatic = Aromaticity.CDKLegacy.Apply(mol);
            Assert.IsTrue(isaromatic);
        }

        // Tests for various aromatic hetero cycles follow:

        // @cdk.bug 1959516
        [TestMethod()]
        public void TestPyrrole1()
        {
            var smiles = "[nH]1cccc1";
            var mol = Load(smiles);
            MakeAtomType(mol);
            AssertAtomTypesPerceived(mol);

            Assert.AreEqual(5, mol.Atoms.Count);

            AssertAllSingleOrAromatic(mol);

            AssertAtomSymbols(new string[] { "N", "C", "C", "C", "C" }, mol);

            AssertHybridizations(new Hybridization[]{Hybridization.Planar3,
                    Hybridization.SP2, Hybridization.SP2, Hybridization.SP2,
                    Hybridization.SP2}, mol);

            AssertHydrogenCounts(new int[] { 1, 1, 1, 1, 1 }, mol);
        }

        // @cdk.bug 1959516
        [TestMethod()]
        public void TestPyrrole2()
        {
            var smiles = "n1([H])cccc1";
            var mol = Load(smiles);
            MakeAtomType(mol);
            AssertAtomTypesPerceived(mol);

            Assert.AreEqual(6, mol.Atoms.Count);

            AssertAllSingleOrAromatic(mol);

            AssertAtomSymbols(new string[] { "N", "H", "C", "C", "C", "C" }, mol);

            AssertHybridizations(new Hybridization[]{Hybridization.Planar3, Hybridization.S,
                    Hybridization.SP2, Hybridization.SP2, Hybridization.SP2,
                    Hybridization.SP2}, mol);

            AssertHydrogenCounts(new int[] { 0, 0, 1, 1, 1, 1 }, mol);
        }

        // @cdk.bug 1962419
        [TestMethod()]
        [ExpectedException(typeof(InvalidSmilesException))]
        public void TestPyrrole3()
        {
            var smiles = "n1cccc1";
            sp.ParseSmiles(smiles);
        }

        // @cdk.bug 1962398
        [TestMethod()]
        public void TestPyrroleAnion1()
        {
            var smiles = "[n-]1cccc1";
            var mol = Load(smiles);
            MakeAtomType(mol);
            AssertAtomTypesPerceived(mol);

            Assert.AreEqual(5, mol.Atoms.Count);

            AssertAllSingleOrAromatic(mol);

            AssertAtomSymbols(new string[] { "N", "C", "C", "C", "C" }, mol);

            AssertHybridizations(new Hybridization[]{Hybridization.Planar3,
                    Hybridization.SP2, Hybridization.SP2, Hybridization.SP2,
                    Hybridization.SP2}, mol);

            AssertHydrogenCounts(new int[] { 0, 1, 1, 1, 1 }, mol);
        }

        // @cdk.bug 1960990
        [TestMethod()]
        public void TestImidazole1()
        {
            var smiles = "[nH]1cncc1";
            var mol = Load(smiles);
            MakeAtomType(mol);
            AssertAtomTypesPerceived(mol);

            Assert.AreEqual(5, mol.Atoms.Count);

            AssertAllSingleOrAromatic(mol);

            AssertAtomSymbols(new string[] { "N", "C", "N", "C", "C" }, mol);

            AssertHybridizations(new Hybridization[]{Hybridization.Planar3,
                    Hybridization.SP2, Hybridization.SP2, Hybridization.SP2,
                    Hybridization.SP2}, mol);

            AssertHydrogenCounts(new int[] { 1, 1, 0, 1, 1 }, mol);
        }

        // @cdk.bug 1960990
        [TestMethod()]
        public void TestImidazole2()
        {
            var smiles = "n1([H])cncc1";
            var mol = Load(smiles);
            MakeAtomType(mol);
            AssertAtomTypesPerceived(mol);

            Assert.AreEqual(6, mol.Atoms.Count);

            AssertAllSingleOrAromatic(mol);

            AssertAtomSymbols(new string[] { "N", "H", "C", "N", "C", "C" }, mol);

            AssertHybridizations(new Hybridization[]{Hybridization.Planar3, Hybridization.S,
                    Hybridization.SP2, Hybridization.SP2, Hybridization.SP2,
                    Hybridization.SP2}, mol);

            AssertHydrogenCounts(new int[] { 0, 0, 1, 0, 1, 1 }, mol);
        }

        // @cdk.bug 1962419
        [TestMethod()]
        [ExpectedException(typeof(InvalidSmilesException))]
        public void TestImidazole3()
        {
            var smiles = "n1cncc1";
            sp.ParseSmiles(smiles);
        }

        // @cdk.bug 1960990
        [TestMethod()]
        public void TestImidazole4()
        {
            var smiles = "n1cc[nH]c1";
            var mol = Load(smiles);
            MakeAtomType(mol);
            AssertAtomTypesPerceived(mol);

            Assert.AreEqual(5, mol.Atoms.Count);

            AssertAllSingleOrAromatic(mol);

            AssertAtomSymbols(new string[] { "N", "C", "C", "N", "C" }, mol);

            AssertHybridizations(new Hybridization[]{Hybridization.SP2, Hybridization.SP2,
                    Hybridization.SP2, Hybridization.Planar3, Hybridization.SP2}, mol);

            AssertHydrogenCounts(new int[] { 0, 1, 1, 1, 1 }, mol);
        }

        // @cdk.bug 1959516
        [TestMethod()]
        public void TestPyridine1()
        {
            var smiles = "n1ccccc1";
            var mol = Load(smiles);
            MakeAtomType(mol);
            AssertAtomTypesPerceived(mol);

            Assert.AreEqual(6, mol.Atoms.Count);

            AssertAtomSymbols(new string[] { "N", "C", "C", "C", "C", "C" }, mol);

            AssertHybridizations(new Hybridization[]{Hybridization.SP2, Hybridization.SP2,
                    Hybridization.SP2, Hybridization.SP2, Hybridization.SP2,
                    Hybridization.SP2}, mol);

            AssertHydrogenCounts(new int[] { 0, 1, 1, 1, 1, 1 }, mol);
        }

        // @cdk.bug 1959516
        [TestMethod()]
        public void TestPyrimidine1()
        {
            var smiles = "n1cnccc1";
            var mol = Load(smiles);
            MakeAtomType(mol);
            AssertAtomTypesPerceived(mol);

            Assert.AreEqual(6, mol.Atoms.Count);

            AssertAllSingleOrAromatic(mol);

            AssertAtomSymbols(new string[] { "N", "C", "N", "C", "C", "C" }, mol);

            AssertHybridizations(new Hybridization[]{Hybridization.SP2, Hybridization.SP2,
                    Hybridization.SP2, Hybridization.SP2, Hybridization.SP2,
                    Hybridization.SP2}, mol);

            AssertHydrogenCounts(new int[] { 0, 1, 0, 1, 1, 1 }, mol);
        }

        // @cdk.bug 1967468
        [TestMethod()]
        public void TestIndole1()
        {
            var smiles1 = "c1ccc2cc[nH]c2(c1)";
            var mol = LoadExact(smiles1);
            AssertAtomTypesPerceived(mol);
            Assert.AreEqual(9, mol.Atoms.Count);

            foreach (var atom in mol.Atoms)
            {
                Assert.IsTrue(atom.IsAromatic);
            }
        }

        // @cdk.bug 1967468
        [TestMethod()]
        public void TestIndole2()
        {
            var smiles1 = "C1(NC=C2)=C2C=CC=C1";
            var mol = LoadExact(smiles1);
            MakeAtomType(mol);
            Aromaticity.CDKLegacy.Apply(mol);
            AssertAtomTypesPerceived(mol);
            Assert.AreEqual(9, mol.Atoms.Count);
            foreach (var atom in mol.Atoms)
            {
                Assert.IsTrue(atom.IsAromatic);
            }
        }

        // @cdk.bug 1963731
        [TestMethod()]
        public void TestBug1963731()
        {

            var sp = CDK.SmilesParser;
            var molecule = sp.ParseSmiles("C(C1C(C(C(C(O1)O)N)O)O)O");
            int hcount = 0;
            for (int i = 0; i < molecule.Bonds.Count; i++)
            {
                hcount += molecule.Atoms[i].ImplicitHydrogenCount.Value;
            }
            Assert.AreEqual(13, hcount);
        }

        [TestMethod()]
        public void TestONSSolubility1()
        {
            var sp = CDK.SmilesParser;
            var molecule = sp.ParseSmiles("Oc1ccc(cc1OC)C=O");
            Assert.AreEqual(11, molecule.Atoms.Count);
            Assert.AreEqual(11, molecule.Bonds.Count);
        }

        [TestMethod()]
        public void Test1456139()
        {
            SmilesParser p = CDK.SmilesParser;
            var mol = p.ParseSmiles("Cc1nn(C)cc1[C@H]2[C@H](C(=O)N)C(=O)C[C@@](C)(O)[C@@H]2C(=O)N");
            IAtomContainer mol2 = ChemObjectBuilder.Instance.NewAtomContainer(mol);
            Assert.IsNotNull(mol2);
            Assert.AreEqual(22, mol2.Atoms.Count);
        }

        [TestMethod()]
        public void TestExplicitH()
        {
            SmilesParser p = CDK.SmilesParser;
            IAtomContainer mol;

            mol = p.ParseSmiles("CO[H]");
            Assert.AreEqual(3, mol.Atoms.Count);

            mol = p.ParseSmiles("[CH3][OH]");
            Assert.AreEqual(2, mol.Atoms.Count);

            mol = p.ParseSmiles("C([H])([H])([H])O([H])");
            Assert.AreEqual(6, mol.Atoms.Count);
        }

        // @cdk.bug 2514200
        [TestMethod()]
        public void Testno937()
        {
            var sp = CDK.SmilesParser;
            var mol = sp.ParseSmiles("C[nH0]1c[nH0]cc1"); // xlogp training set molecule no937
            Assert.IsNotNull(mol.Atoms[1].ImplicitHydrogenCount);
            Assert.AreEqual(0, mol.Atoms[1].ImplicitHydrogenCount.Value);
            Assert.IsNotNull(mol.Atoms[3].ImplicitHydrogenCount);
            Assert.AreEqual(0, mol.Atoms[3].ImplicitHydrogenCount.Value);
        }

        // @cdk.bug 2514200
        [TestMethod()]
        public void TestHardcodedH()
        {
            var sp = CDK.SmilesParser;
            var mol = sp.ParseSmiles("C[CH1]NC");
            Assert.IsNotNull(mol.Atoms[1].ImplicitHydrogenCount);
            Assert.AreEqual(1, mol.Atoms[1].ImplicitHydrogenCount.Value);

            mol = sp.ParseSmiles("C[CH]NC");
            Assert.IsNotNull(mol.Atoms[1].ImplicitHydrogenCount);
            Assert.AreEqual(1, mol.Atoms[1].ImplicitHydrogenCount.Value);

            mol = sp.ParseSmiles("C[CH0]NC");
            Assert.IsNotNull(mol.Atoms[1].ImplicitHydrogenCount);
            Assert.AreEqual(0, mol.Atoms[1].ImplicitHydrogenCount.Value);
        }

        // @cdk.bug 2714283
        [TestMethod()]
        [ExpectedException(typeof(InvalidSmilesException))]
        public void TestBadRingClosure1()
        {
            SmilesParser p = CDK.SmilesParser;
            p.ParseSmiles("c1ccccc1Cc1ccccc");
        }

        // @cdk.bug 2714283
        [TestMethod()]
        [ExpectedException(typeof(InvalidSmilesException))]
        public void TestBadRingClosure2()
        {
            SmilesParser p = CDK.SmilesParser;
            p.ParseSmiles("NC1=CC=C(N)C=C");
        }

        /// <summary>
        /// <seealso cref="TestPyrrole"/>
        /// </summary>
        // @cdk.inchi InChI=1/C4H5N/c1-2-4-5-3-1/h1-5H
        [TestMethod()]
        public void TestPyrrole_2()
        {
            SmilesParser p = CDK.SmilesParser;
            var mol = p.ParseSmiles("c1c[nH]cc1");

            Assert.AreEqual(BondOrder.Double, mol.GetBond(mol.Atoms[0], mol.Atoms[1]).Order);
            Assert.AreEqual(BondOrder.Single, mol.GetBond(mol.Atoms[1], mol.Atoms[2]).Order);
            Assert.AreEqual(BondOrder.Single, mol.GetBond(mol.Atoms[2], mol.Atoms[3]).Order);
            Assert.AreEqual(BondOrder.Double, mol.GetBond(mol.Atoms[3], mol.Atoms[4]).Order);
            Assert.AreEqual(BondOrder.Single, mol.GetBond(mol.Atoms[4], mol.Atoms[0]).Order);

            MakeAtomType(mol);
            Aromaticity.CDKLegacy.Apply(mol);

            foreach (var atom in mol.Atoms)
            {
                Assert.IsTrue(atom.IsAromatic);
            }
        }

        // @cdk.bug 3048501
        [TestMethod()]
        public void TestAromaticSeParsing()
        {
            // The CDK aromaticity model does not recognise 'se' but we can still
            // parse it from the SMILES
            SmilesParser p = new SmilesParser(ChemObjectBuilder.Instance, false);
            var mol = p.ParseSmiles("c1cc2cccnc2[se]1");
            foreach (var atom in mol.Atoms)
            {
                Assert.IsTrue(atom.IsAromatic);
            }
        }

        // @cdk.bug 3048501
        [TestMethod()]
        public void TestCeParsing()
        {
            SmilesParser p = CDK.SmilesParser;
            var mol = p.ParseSmiles("Cl[Ce](Cl)Cl");
            Assert.AreEqual("Ce", mol.Atoms[1].Symbol);
        }

        // @cdk.bug 3048501
        [TestMethod()]
        public void TestErParsing()
        {
            SmilesParser p = CDK.SmilesParser;
            var mol = p.ParseSmiles("Cl[Er](Cl)Cl");
            Assert.AreEqual("Er", mol.Atoms[1].Symbol);
        }

        // @cdk.bug 3048501
        [TestMethod()]
        public void TestGdParsing()
        {
            SmilesParser p = CDK.SmilesParser;
            var mol = p.ParseSmiles("Cl[Gd](Cl)Cl");
            Assert.AreEqual("Gd", mol.Atoms[1].Symbol);
        }

        // @cdk.bug 3048501
        [TestMethod()]
        public void TestSmParsing()
        {
            SmilesParser p = CDK.SmilesParser;
            var mol = p.ParseSmiles("Cl[Sm](Cl)Cl");
            Assert.AreEqual("Sm", mol.Atoms[1].Symbol);
        }

        // @cdk.bug 3048501
        [TestMethod()]
        public void TestLaParsing()
        {
            SmilesParser p = CDK.SmilesParser;
            var mol = p.ParseSmiles("[Cl-].[Cl-].[Cl-].[La+3]");
            Assert.AreEqual("La", mol.Atoms[3].Symbol);
        }

        // @cdk.bug 3048501
        [TestMethod()]
        public void TestAcParsing()
        {
            SmilesParser p = CDK.SmilesParser;
            var mol = p.ParseSmiles("[255Ac]");
            Assert.AreEqual("Ac", mol.Atoms[0].Symbol);
        }

        // @cdk.bug 3048501
        [TestMethod()]
        public void TestPuParsing()
        {
            SmilesParser p = CDK.SmilesParser;
            var mol = p.ParseSmiles("[Pu]");
            Assert.AreEqual("Pu", mol.Atoms[0].Symbol);
        }

        // @cdk.bug 3048501
        [TestMethod()]
        public void TestPrParsing()
        {
            SmilesParser p = CDK.SmilesParser;
            var mol = p.ParseSmiles("[Pr]");
            Assert.AreEqual("Pr", mol.Atoms[0].Symbol);
        }

        // @cdk.bug 3048501
        [TestMethod()]
        public void TestPaParsing()
        {
            SmilesParser p = CDK.SmilesParser;
            var mol = p.ParseSmiles("[Pa]");
            Assert.AreEqual("Pa", mol.Atoms[0].Symbol);
        }

        // @cdk.bug 3048501
        [TestMethod()]
        public void TestTbParsing()
        {
            SmilesParser p = CDK.SmilesParser;
            var mol = p.ParseSmiles("[Tb]");
            Assert.AreEqual("Tb", mol.Atoms[0].Symbol);
        }

        // @cdk.bug 3048501
        [TestMethod()]
        public void TestAmParsing()
        {
            SmilesParser p = CDK.SmilesParser;
            var mol = p.ParseSmiles("[Am]");
            Assert.AreEqual("Am", mol.Atoms[0].Symbol);
        }

        // @cdk.bug 3048501
        [TestMethod()]
        public void TestPmParsing()
        {
            SmilesParser p = CDK.SmilesParser;
            var mol = p.ParseSmiles("[Pm]");
            Assert.AreEqual("Pm", mol.Atoms[0].Symbol);
        }

        // @cdk.bug 3048501
        [TestMethod()]
        public void TestHoParsing()
        {
            SmilesParser p = CDK.SmilesParser;
            var mol = p.ParseSmiles("[Ho]");
            Assert.AreEqual("Ho", mol.Atoms[0].Symbol);
        }

        // @cdk.bug 3048501
        [TestMethod()]
        public void TestCfParsing()
        {
            SmilesParser p = CDK.SmilesParser;
            var mol = p.ParseSmiles("[Cf]");
            Assert.AreEqual("Cf", mol.Atoms[0].Symbol);
        }

        // @cdk.bug 2976054
        [TestMethod()]
        public void TestAromaticity()
        {
            var mol = LoadExact("c1cnc2s[cH][cH]n12");
            foreach (var atom in mol.Atoms)
            {
                Assert.IsTrue(atom.IsAromatic);
            }
        }

        /// <summary>
        /// Tests reading stereochemistry from a SMILES with one of the four groups being an implicit hydrogen.
        /// </summary>
        [TestMethod()]
        public void TestAtAt()
        {
            var sp = CDK.SmilesParser;
            var mol = sp.ParseSmiles("Br[C@@H](Cl)I");
            var stereoElements = mol.StereoElements.GetEnumerator();
            Assert.IsTrue(stereoElements.MoveNext());
            var stereoElement = stereoElements.Current;
            Assert.IsNotNull(stereoElement);
            Assert.IsTrue(stereoElement is ITetrahedralChirality);
            ITetrahedralChirality l4Chiral = (ITetrahedralChirality)stereoElement;
            Assert.AreEqual("C", l4Chiral.ChiralAtom.Symbol);
            var ligands = l4Chiral.Ligands;
            foreach (var atom in ligands)
                Assert.IsNotNull(atom);
            // note: the tetrahedral centre holds atom '1' to refer to implicit
            // hydrogen
            Assert.AreEqual(mol.Atoms[0], ligands[0]);
            Assert.AreEqual(mol.Atoms[1], ligands[1]);
            Assert.AreEqual(mol.Atoms[2], ligands[2]);
            Assert.AreEqual(mol.Atoms[3], ligands[3]);
            Assert.AreEqual(TetrahedralStereo.Clockwise, l4Chiral.Stereo);
        }

        /// <summary>
        /// Tests reading stereochemistry from a SMILES with one of the four groups being an implicit hydrogen.
        /// Per SMILES specification, this hydrogen is the atom towards the viewer, and will therefore end up
        /// as first atom in the array.
        /// </summary>
        [TestMethod()]
        public void TestAt()
        {
            var sp = CDK.SmilesParser;
            var mol = sp.ParseSmiles("Br[C@H](Cl)I");
            var stereoElements = mol.StereoElements.GetEnumerator();
            Assert.IsTrue(stereoElements.MoveNext());
            var stereoElement = stereoElements.Current;
            Assert.IsNotNull(stereoElement);
            Assert.IsTrue(stereoElement is ITetrahedralChirality);
            ITetrahedralChirality l4Chiral = (ITetrahedralChirality)stereoElement;
            Assert.AreEqual("C", l4Chiral.ChiralAtom.Symbol);
            var ligands = l4Chiral.Ligands;
            foreach (var atom in ligands)
                Assert.IsNotNull(atom);
            // note: the tetrahedral centre holds atom '1' to refer to implicit
            // hydrogen
            Assert.AreEqual(mol.Atoms[0], ligands[0]);
            Assert.AreEqual(mol.Atoms[1], ligands[1]);
            Assert.AreEqual(mol.Atoms[2], ligands[2]);
            Assert.AreEqual(mol.Atoms[3], ligands[3]);
            Assert.AreEqual(TetrahedralStereo.AntiClockwise, l4Chiral.Stereo);
        }

        [TestMethod()]
        public void TestAtAt_ExplicitHydrogen()
        {
            var sp = CDK.SmilesParser;
            var mol = sp.ParseSmiles("Br[C@@]([H])(Cl)I");
            var stereoElements = mol.StereoElements.GetEnumerator();
            Assert.IsTrue(stereoElements.MoveNext());
            var stereoElement = stereoElements.Current;
            Assert.IsNotNull(stereoElement);
            Assert.IsTrue(stereoElement is ITetrahedralChirality);
            ITetrahedralChirality l4Chiral = (ITetrahedralChirality)stereoElement;
            Assert.AreEqual("C", l4Chiral.ChiralAtom.Symbol);
            var ligands = l4Chiral.Ligands;
            foreach (var atom in ligands)
                Assert.IsNotNull(atom);
            Assert.AreEqual("Br", ligands[0].Symbol);
            Assert.AreEqual("H", ligands[1].Symbol);
            Assert.AreEqual("Cl", ligands[2].Symbol);
            Assert.AreEqual("I", ligands[3].Symbol);
            Assert.AreEqual(TetrahedralStereo.Clockwise, l4Chiral.Stereo);
        }

        [TestMethod()]
        public void TestAt_ExplicitHydrogen()
        {
            var sp = CDK.SmilesParser;
            var mol = sp.ParseSmiles("Br[C@]([H])(Cl)I");
            var stereoElements = mol.StereoElements.GetEnumerator();
            Assert.IsTrue(stereoElements.MoveNext());
            var stereoElement = stereoElements.Current;
            Assert.IsNotNull(stereoElement);
            Assert.IsTrue(stereoElement is ITetrahedralChirality);
            ITetrahedralChirality l4Chiral = (ITetrahedralChirality)stereoElement;
            Assert.AreEqual("C", l4Chiral.ChiralAtom.Symbol);
            var ligands = l4Chiral.Ligands;
            foreach (var atom in ligands)
                Assert.IsNotNull(atom);
            Assert.AreEqual("Br", ligands[0].Symbol);
            Assert.AreEqual("H", ligands[1].Symbol);
            Assert.AreEqual("Cl", ligands[2].Symbol);
            Assert.AreEqual("I", ligands[3].Symbol);
            Assert.AreEqual(TetrahedralStereo.AntiClockwise, l4Chiral.Stereo);
        }

        [TestMethod()]
        public void TestRingClosure()
        {
            var sp = CDK.SmilesParser;
            var mol = sp.ParseSmiles("C12(OC1)CCC2");
            Assert.AreEqual(6, mol.Atoms.Count);
            Assert.AreEqual("C", mol.Atoms[0].Symbol);
            Assert.AreEqual("O", mol.Atoms[1].Symbol);
            Assert.AreEqual("C", mol.Atoms[2].Symbol);
            Assert.AreEqual(4, mol.GetConnectedBonds(mol.Atoms[0]).Count());
            Assert.AreEqual(2, mol.GetConnectedBonds(mol.Atoms[1]).Count());
            Assert.AreEqual(2, mol.GetConnectedBonds(mol.Atoms[2]).Count());
        }

        [TestMethod()]
        public void TestRingClosure_At()
        {
            var sp = CDK.SmilesParser;
            var mol = sp.ParseSmiles("[C@]12(OC1)NCN2");
            var stereoElements = mol.StereoElements.GetEnumerator();
            Assert.IsTrue(stereoElements.MoveNext());
            var stereoElement = stereoElements.Current;
            Assert.IsNotNull(stereoElement);
            Assert.IsTrue(stereoElement is ITetrahedralChirality);
            ITetrahedralChirality l4Chiral = (ITetrahedralChirality)stereoElement;
            Assert.AreEqual("C", l4Chiral.ChiralAtom.Symbol);
            var ligands = l4Chiral.Ligands;
            foreach (var atom in ligands)
                Assert.IsNotNull(atom);
            // note: ligands are given in the order they appear in (hence in this
            // case the winding (getStereo) has flipped
            Assert.AreEqual(mol.Atoms[1], ligands[0]);
            Assert.AreEqual(mol.Atoms[2], ligands[1]);
            Assert.AreEqual(mol.Atoms[3], ligands[2]);
            Assert.AreEqual(mol.Atoms[5], ligands[3]);
            Assert.AreEqual(TetrahedralStereo.Clockwise, l4Chiral.Stereo);
        }

        [TestMethod()]
        public void TestNeighboRingChirality()
        {
            var sp = CDK.SmilesParser;
            var mol = sp.ParseSmiles("C[C@H](O)[C@H](O)C");
            var stereoElements = new List<IStereoElement<IChemObject, IChemObject>>(mol.StereoElements);
            stereoElements.Sort((o1, o2) =>
                Ints.Compare(mol.Atoms.IndexOf(((ITetrahedralChirality)o1).ChiralAtom),
                                mol.Atoms.IndexOf(((ITetrahedralChirality)o2).ChiralAtom)));

            // first chiral center
            Assert.AreEqual(2, stereoElements.Count);
            var stereoElement = stereoElements[0];
            Assert.IsNotNull(stereoElement);
            Assert.IsTrue(stereoElement is ITetrahedralChirality);
            ITetrahedralChirality l4Chiral = (ITetrahedralChirality)stereoElement;
            Assert.AreEqual("C", l4Chiral.ChiralAtom.Symbol);
            var ligands = l4Chiral.Ligands;
            foreach (var atom in ligands)
                Assert.IsNotNull(atom);
            Assert.AreEqual(mol.Atoms[0], ligands[0]);
            Assert.AreEqual(mol.Atoms[1], ligands[1]);
            Assert.AreEqual(mol.Atoms[2], ligands[2]);
            Assert.AreEqual(mol.Atoms[3], ligands[3]);
            Assert.AreEqual(TetrahedralStereo.AntiClockwise, l4Chiral.Stereo);
            // second chiral center
            stereoElement = stereoElements[1];
            Assert.IsNotNull(stereoElement);
            Assert.IsTrue(stereoElement is ITetrahedralChirality);
            l4Chiral = (ITetrahedralChirality)stereoElement;
            Assert.AreEqual("C", l4Chiral.ChiralAtom.Symbol);
            ligands = l4Chiral.Ligands;
            foreach (var atom in ligands)
                Assert.IsNotNull(atom);
            Assert.AreEqual(mol.Atoms[1], ligands[0]);
            Assert.AreEqual(mol.Atoms[3], ligands[1]);
            Assert.AreEqual(mol.Atoms[4], ligands[2]);
            Assert.AreEqual(mol.Atoms[5], ligands[3]);
            Assert.AreEqual(TetrahedralStereo.AntiClockwise, l4Chiral.Stereo);
        }

        [TestMethod()]
        public void TestChiralityInBranch()
        {
            var sp = CDK.SmilesParser;
            var mol = sp.ParseSmiles("NC([C@H](O)C)Cl");
            var stereoElements = mol.StereoElements.GetEnumerator();
            // first chiral center
            Assert.IsTrue(stereoElements.MoveNext());
            var stereoElement = stereoElements.Current;
            Assert.IsNotNull(stereoElement);
            Assert.IsTrue(stereoElement is ITetrahedralChirality);
            ITetrahedralChirality l4Chiral = (ITetrahedralChirality)stereoElement;
            Assert.AreEqual("C", l4Chiral.ChiralAtom.Symbol);
            var ligands = l4Chiral.Ligands;
            foreach (var atom in ligands)
                Assert.IsNotNull(atom);
            Assert.AreEqual(mol.Atoms[1], ligands[0]);
            Assert.AreEqual(mol.Atoms[2], ligands[1]); // refers to implicit hydrogen
            Assert.AreEqual(mol.Atoms[3], ligands[2]);
            Assert.AreEqual(mol.Atoms[4], ligands[3]);
            Assert.AreEqual(TetrahedralStereo.AntiClockwise, l4Chiral.Stereo);
        }

        [TestMethod()]
        public void TestChiralityWithTonsOfDots()
        {
            var sp = CDK.SmilesParser;
            var mol = sp.ParseSmiles("I1.Cl2.Br3.[C@]123CCC");
            var stereoElements = mol.StereoElements.GetEnumerator();
            Assert.IsTrue(stereoElements.MoveNext());
            var stereoElement = stereoElements.Current;
            Assert.IsNotNull(stereoElement);
            Assert.IsTrue(stereoElement is ITetrahedralChirality);
            ITetrahedralChirality l4Chiral = (ITetrahedralChirality)stereoElement;
            Assert.AreEqual("C", l4Chiral.ChiralAtom.Symbol);
            var ligands = l4Chiral.Ligands;
            foreach (var atom in ligands)
                Assert.IsNotNull(atom);
            Assert.AreEqual("I", ligands[0].Symbol);
            Assert.AreEqual("Cl", ligands[1].Symbol);
            Assert.AreEqual("Br", ligands[2].Symbol);
            Assert.AreEqual("C", ligands[3].Symbol);
            Assert.AreEqual(TetrahedralStereo.AntiClockwise, l4Chiral.Stereo);
        }

        [TestMethod()]
        public void TestChiralAtomWithDisconnectedLastAtom()
        {
            var sp = CDK.SmilesParser;
            var mol = sp.ParseSmiles("Br1.[C@]1(Cl)(OC)CCC");
            var stereoElements = mol.StereoElements.GetEnumerator();
            Assert.IsTrue(stereoElements.MoveNext());
            var stereoElement = stereoElements.Current;
            Assert.IsNotNull(stereoElement);
            Assert.IsTrue(stereoElement is ITetrahedralChirality);
            ITetrahedralChirality l4Chiral = (ITetrahedralChirality)stereoElement;
            Assert.AreEqual("C", l4Chiral.ChiralAtom.Symbol);
            var ligands = l4Chiral.Ligands;
            foreach (var atom in ligands)
                Assert.IsNotNull(atom);
            Assert.AreEqual("Br", ligands[0].Symbol);
            Assert.AreEqual("Cl", ligands[1].Symbol);
            Assert.AreEqual("O", ligands[2].Symbol);
            Assert.AreEqual("C", ligands[3].Symbol);
            Assert.AreEqual(TetrahedralStereo.AntiClockwise, l4Chiral.Stereo);
        }

        [TestMethod()]
        public void TestFromBlog1()
        {
            var sp = CDK.SmilesParser;
            var mol = sp.ParseSmiles("[C@@H]231.C2.N1.F3");
            var stereoElements = mol.StereoElements.GetEnumerator();
            Assert.IsTrue(stereoElements.MoveNext());
            var stereoElement = stereoElements.Current;
            Assert.IsNotNull(stereoElement);
            Assert.IsTrue(stereoElement is ITetrahedralChirality);
            ITetrahedralChirality l4Chiral = (ITetrahedralChirality)stereoElement;
            Assert.AreEqual("C", l4Chiral.ChiralAtom.Symbol);
            var ligands = l4Chiral.Ligands;
            foreach (var atom in ligands)
                Assert.IsNotNull(atom);
            // note: ligands are given in the order they appear in (hence in this
            // case the winding (getStereo) has flipped (0,1,3,2) -> (0,1,2,3)
            Assert.AreEqual(mol.Atoms[0], ligands[0]);
            Assert.AreEqual(mol.Atoms[1], ligands[1]);
            Assert.AreEqual(mol.Atoms[2], ligands[2]);
            Assert.AreEqual(mol.Atoms[3], ligands[3]);
            Assert.AreEqual(TetrahedralStereo.AntiClockwise, l4Chiral.Stereo);
        }

        [TestMethod()]
        public void TestFromBlog2()
        {
            var sp = CDK.SmilesParser;
            var mol = sp.ParseSmiles("[C@@H](Cl)1[C@H](C)(F).Br1");
            var stereoElements = mol.StereoElements.GetEnumerator();
            for (int i = 0; i < 2; i++)
            {
                Assert.IsTrue(stereoElements.MoveNext());
                var stereoElement = stereoElements.Current;
                Assert.IsNotNull(stereoElement);
                Assert.IsTrue(stereoElement is ITetrahedralChirality);
                ITetrahedralChirality l4Chiral = (ITetrahedralChirality)stereoElement;
                Assert.AreEqual("C", l4Chiral.ChiralAtom.Symbol);
                if (l4Chiral.ChiralAtom.Equals(mol.Atoms[0]))
                {
                    var ligands = l4Chiral.Ligands;
                    foreach (var atom in ligands)
                        Assert.IsNotNull(atom);
                    // note: ligands are given in the order they appear, there is
                    // one inversion (0,1,5,2) -> (0,1,2,5) so winding flips
                    Assert.AreEqual(mol.Atoms[0], ligands[0]);
                    Assert.AreEqual(mol.Atoms[1], ligands[1]);
                    Assert.AreEqual(mol.Atoms[2], ligands[2]);
                    Assert.AreEqual(mol.Atoms[5], ligands[3]);
                    Assert.AreEqual(TetrahedralStereo.AntiClockwise, l4Chiral.Stereo);
                }
                else
                {
                    var ligands = l4Chiral.Ligands;
                    foreach (var atom in ligands)
                        Assert.IsNotNull(atom);
                    Assert.AreEqual(mol.Atoms[0], ligands[0]);
                    Assert.AreEqual(mol.Atoms[2], ligands[1]);
                    Assert.AreEqual(mol.Atoms[3], ligands[2]);
                    Assert.AreEqual(mol.Atoms[4], ligands[3]);
                    Assert.AreEqual(TetrahedralStereo.AntiClockwise, l4Chiral.Stereo);
                }
            }
        }

        [TestMethod()]
        public void TestPreserveAromaticity()
        {
            var sp = new SmilesParser(ChemObjectBuilder.Instance, false);
            var molecule = sp.ParseSmiles("Oc1ccc(Cl)c2C(=O)c3c(sc4nccn34)C(=O)c12");
            Assert.AreEqual(14, CountAromaticAtoms(molecule));
            Assert.AreEqual(15, CountAromaticBonds(molecule));

            molecule = sp.ParseSmiles("COc1ccc2[nH]c3c(cnn4c(C)nnc34)c2c1");
            Assert.AreEqual(16, CountAromaticAtoms(molecule));
            Assert.AreEqual(19, CountAromaticBonds(molecule));

            molecule = sp.ParseSmiles("C:1:C:C:C:C:C1"); // n.b see cyclohexaneWithAromaticBonds
            Assert.AreEqual(6, CountAromaticAtoms(molecule));
            Assert.AreEqual(6, CountAromaticBonds(molecule));

            molecule = sp.ParseSmiles("c1cc[se]cc1");
            Assert.AreEqual(6, CountAromaticAtoms(molecule));
            Assert.AreEqual(6, CountAromaticBonds(molecule));

        }

        /// <summary>
        ///  'C:1:C:C:C:C:C1' is actually cyclo-hexane not benzene. Beam will kekulise
        ///  this correctly and leave single bonds the aromaticity flags are preserved.
        /// </summary>
        [TestMethod()]
        public void CyclohexaneWithAromaticBonds()
        {
            var molecule = sp.ParseSmiles("C:1:C:C:C:C:C1");
            Assert.AreEqual(6, CountAromaticAtoms(molecule));
            Assert.AreEqual(6, CountAromaticBonds(molecule));
            foreach (var bond in molecule.Bonds)
            {
                Assert.AreEqual(BondOrder.Single, bond.Order);
                Assert.IsTrue(bond.IsAromatic);
            }
        }

        [TestMethod()]
        public void TestPreserveAromaticityAndPerceiveAtomTypes()
        {
            var sp = new SmilesParser(ChemObjectBuilder.Instance, false);
            var molecule = sp.ParseSmiles("c1ccccc1");
            MakeAtomType(molecule);
            Assert.IsNotNull(molecule.Atoms[0].AtomTypeName);
        }

        // @cdk.bug 3160514
        [TestMethod()]
        public void TestAromaticBoron()
        {
            var mol = LoadExact("c1cc2c3cc1.c1cb23cc1");
            Assert.IsNotNull(mol);
            AssertAllSingleOrAromatic(mol);
        }

        /// <summary>
        /// This molecule is actually invalid and there is no way to kekulise it.
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(InvalidSmilesException))]
        public void TestAromaticBoron_invalid()
        {
            Load("c1cc2c3cc1.c1cb23cc1");
        }

        /// <summary>
        /// A 'proper' aromatic boron example.
        /// </summary>
        [TestMethod()]
        public void Borinine()
        {
            var mol = Load("b1ccccc1");
            Assert.AreEqual(BondOrder.Double, mol.GetBond(mol.Atoms[0], mol.Atoms[1]).Order);
            Assert.AreEqual(BondOrder.Single, mol.GetBond(mol.Atoms[1], mol.Atoms[2]).Order);
            Assert.AreEqual(BondOrder.Double, mol.GetBond(mol.Atoms[2], mol.Atoms[3]).Order);
            Assert.AreEqual(BondOrder.Single, mol.GetBond(mol.Atoms[3], mol.Atoms[4]).Order);
            Assert.AreEqual(BondOrder.Double, mol.GetBond(mol.Atoms[4], mol.Atoms[5]).Order);
            Assert.AreEqual(BondOrder.Single, mol.GetBond(mol.Atoms[5], mol.Atoms[0]).Order);
        }

        // @cdk.bug 1234
        [TestMethod()]
        [ExpectedException(typeof(InvalidSmilesException))]
        public void TestBug1234()
        {
            sp.ParseSmiles("C1C1");
        }

        [TestMethod()]
        public void TestFormalNeighborBount()
        {
            var sp = CDK.SmilesParser;
            var mol = sp.ParseSmiles("Oc1ccc(O)cc1");
            MakeAtomType(mol);
            Assert.AreEqual("O.sp3", mol.Atoms[0].AtomTypeName);
            Assert.AreEqual(2, mol.Atoms[0].FormalNeighbourCount.Value);
            Assert.AreEqual("C.sp2", mol.Atoms[1].AtomTypeName);
            Assert.AreEqual(3, mol.Atoms[1].FormalNeighbourCount.Value);
            IAtomContainer clone = (IAtomContainer)mol.Clone();
            Assert.AreEqual("O.sp3", clone.Atoms[0].AtomTypeName);
            Assert.AreEqual(2, clone.Atoms[0].FormalNeighbourCount.Value);
            Assert.AreEqual("C.sp2", clone.Atoms[1].AtomTypeName);
            Assert.AreEqual(3, clone.Atoms[1].FormalNeighbourCount.Value);
        }

        // @cdk.bug 549
        [TestMethod()]
        public void TestDiBorane()
        {
            var smiles = "[H]B1([H])HB([H]1)([H])[H]";

            var mol = LoadExact(smiles);
            Assert.AreEqual(8, mol.Atoms.Count);
            Assert.AreEqual(4, mol.GetConnectedBonds(mol.Atoms[1]).Count());
            Assert.AreEqual(2, mol.GetConnectedBonds(mol.Atoms[3]).Count());
            Assert.AreEqual(4, mol.GetConnectedBonds(mol.Atoms[4]).Count());
            Assert.AreEqual(2, mol.GetConnectedBonds(mol.Atoms[5]).Count());
        }

        /// <summary>
        /// Okay exception for a non-SMILES string.
        /// </summary>
        // @cdk.bug 1375
        [TestMethod()]
        [ExpectedException(typeof(InvalidSmilesException))]
        public void IdNumber()
        {
            Load("50-00-0");
        }

        [TestMethod()]
        public void AtomBasedDbStereo()
        {
            Assert.AreEqual("F/C=C/F", SmilesGenerator.Isomeric.Create(Load("F[C@H]=[C@@H]F")));
            Assert.AreEqual("F/C=C\\F", SmilesGenerator.Isomeric.Create(Load("F[C@H]=[C@H]F")));
            Assert.AreEqual("F/C=C/F", SmilesGenerator.Isomeric.Create(Load("F[C@@H]=[C@H]F")));
            Assert.AreEqual("F/C=C\\F", SmilesGenerator.Isomeric.Create(Load("F[C@@H]=[C@@H]F")));
        }

        [TestMethod()]
        public void AtomBasedDbStereoReversing()
        {
            Assert.AreEqual("C(\\F)=C\\F", SmilesGenerator.Isomeric.Create(Load("[C@H](F)=[C@@H]F")));
        }

        [TestMethod()]
        public void AzuleneHasAllBondOrdersSet()
        {
            var mol = Load("c1ccc-2cccccc12");
            foreach (IBond bond in mol.Bonds)
            {
                if (bond.Order == BondOrder.Unset)
                    Assert.Fail("Unset bond order");
            }
        }

        [TestMethod()]
        public void Cisplatin()
        {
            var mol = Load("[NH3][Pt@SP1]([NH3])(Cl)Cl");
            var se = mol.StereoElements.First();
            Assert.IsInstanceOfType(se, typeof(SquarePlanar));
            Assert.AreEqual(1, ((SquarePlanar)se).Configure.Order());
        }

        [TestMethod()]
        public void Cisplatin_Z()
        {
            var mol = Load("[NH3][Pt@SP3]([NH3])(Cl)Cl");
            var se = mol.StereoElements.First();
            Assert.IsInstanceOfType(se, typeof(SquarePlanar));
            Assert.AreEqual(3, ((SquarePlanar)se).Configure.Order());
        }

        [TestMethod()]
        public void Transplatin()
        {
            var mol = Load("[NH3][Pt@SP2]([NH3])(Cl)Cl");
            var se = mol.StereoElements.First();
            Assert.IsInstanceOfType(se, typeof(SquarePlanar));
            Assert.AreEqual(2, ((SquarePlanar)se).Configure.Order());
        }

        [TestMethod()]
        public void Tbpy1()
        {
            var mol = Load("S[As@TB1](F)(Cl)(Br)N");
            var se = mol.StereoElements.First();
            Assert.IsInstanceOfType(se, typeof(TrigonalBipyramidal));
            Assert.AreEqual(1, ((TrigonalBipyramidal)se).Configure.Order());
        }

        [TestMethod()]
        public void Tbpy2()
        {
            var mol = Load("S[As@TB2](F)(Cl)(Br)N");
            var se = mol.StereoElements.First();
            Assert.IsInstanceOfType(se, typeof(TrigonalBipyramidal));
            Assert.AreEqual(2, ((TrigonalBipyramidal)se).Configure.Order());
        }

        [TestMethod()]
        public void Oh1()
        {
            var mol = Load("C[Co@](F)(Cl)(Br)(I)S");
            var se = mol.StereoElements.First();
            Assert.IsInstanceOfType(se, typeof(Octahedral));
            Assert.AreEqual(1, ((Octahedral)se).Configure.Order());
        }

        [TestMethod()]
        public void Oh8()
        {
            var mol = Load("C[Co@OH8](F)(Br)(Cl)(I)S");
            var se = mol.StereoElements.First();
            Assert.IsInstanceOfType(se, typeof(Octahedral));
            Assert.AreEqual(8, ((Octahedral)se).Configure.Order());
        }

        [TestMethod()]
        public void ExtendedExtendedTrans3()
        {
            var mol = Load("C/C=C=C=C/C");
            foreach (var se in mol.StereoElements)
            {
                if (se is ExtendedCisTrans ect)
                {
                    Assert.AreEqual(StereoConfigurations.Opposite, ect.Configure);
                    Assert.AreEqual(mol.Bonds[2], ect.Focus);
                    Assert.AreEqual(mol.Bonds[0], ect.Carriers[0]);
                    Assert.AreEqual(mol.Bonds[4], ect.Carriers[1]);
                }
            }
        }
        [TestMethod()]
        public void ExtendedExtendedCis3()
        {
            var mol = Load("C/C=C=C=C\\C");
            foreach (var se in mol.StereoElements)
            {
                if (se is ExtendedCisTrans ect)
                {
                    Assert.AreEqual(StereoConfigurations.Together, ect.Configure);
                    Assert.AreEqual(mol.Bonds[2], ect.Focus);
                    Assert.AreEqual(mol.Bonds[0], ect.Carriers[0]);
                    Assert.AreEqual(mol.Bonds[4], ect.Carriers[1]);
                }
            }
        }
        [TestMethod()]
        public void ExtendedExtendedCis5()
        {
            var mol = Load("C/C=C=C=C=C=C\\C");
            foreach (var se in mol.StereoElements)
            {
                if (se is ExtendedCisTrans ect)
                {
                    Assert.AreEqual(StereoConfigurations.Together, ect.Configure);
                    Assert.AreEqual(mol.Bonds[3], ect.Focus);
                    Assert.AreEqual(mol.Bonds[0], ect.Carriers[0]);
                    Assert.AreEqual(mol.Bonds[6], ect.Carriers[1]);
                }
            }
        }
        // an even number of double bonds is extended tetrahedral not
        // extended Cis/Trans
        [TestMethod()]
        public void NotExtendedCis()
        {
            var mol = Load("C/C=C=C=C=C\\C");
            Assert.IsFalse(mol.StereoElements.Any());
        }

        [TestMethod()]
        public void WarnOnDirectionalBonds() 
        {
            var smipar = new SmilesParser(Silent.ChemObjectBuilder.Instance);
            var mol = smipar.ParseSmiles("C/C=C/1.C/1");
    }

        [TestMethod()]
        [ExpectedException(typeof(InvalidSmilesException))]
        public void FailOnDirectionalBondsWhenStrict()
        {
            var smipar = new SmilesParser(Silent.ChemObjectBuilder.Instance, strict: true);
            var mol = smipar.ParseSmiles("C/C=C/1.C/1");
        }

        [TestMethod()]
        public void IgnoreDoubleBond()
        {
            var smipar = new SmilesParser(Silent.ChemObjectBuilder.Instance);
            var mol = smipar.ParseSmiles("C/C=C(/F)/C");
            Assert.IsTrue(mol.StereoElements.Count == 0);
        }

        [TestMethod()]
        public void ExtendedTetrahedral7()
        {
            var mol = Load("CC=C=C=[C@]=C=C=CC");
            foreach (var se in mol.StereoElements)
            {
                if (se is ExtendedTetrahedral et)
                {
                    Assert.AreEqual(StereoConfigurations.Left, et.Configure);
                    Assert.AreEqual(mol.Atoms[4], et.Focus);
                    Assert.IsTrue(Compares.AreDeepEqual(
                        new IAtom[]
                        {
                            mol.Atoms[0],
                            mol.Atoms[1],
                            mol.Atoms[7],
                            mol.Atoms[8],
                        },
                        et.Carriers));
                }
            }
        }

        /// <summary>
        /// Counts aromatic atoms in a molecule.
        /// </summary>
        /// <param name="mol">molecule for which to count aromatic atoms.</param>
        private int CountAromaticAtoms(IAtomContainer mol)
        {
            int aromCount = 0;
            foreach (var atom in mol.Atoms)
            {
                if (atom.IsAromatic)
                    aromCount++;
            }
            return aromCount;
        }

        /// <summary>
        /// Counts aromatic bonds in a molecule.
        /// </summary>
        /// <param name="mol">molecule for which to count aromatic bonds.</param>
        private int CountAromaticBonds(IAtomContainer mol)
        {
            int aromCount = 0;
            foreach (var bond in mol.Bonds)
            {
                if (bond.IsAromatic)
                    aromCount++;
            }
            return aromCount;
        }

        static void MakeAtomType(IAtomContainer container)
        {
            var aromatic = new HashSet<IAtom>();
            foreach (var atom in container.Atoms)
            {
                if (atom.IsAromatic)
                    aromatic.Add(atom);
            }
            // helpfully clears aromatic flags...
            AtomContainerManipulator.PercieveAtomTypesAndConfigureAtoms(container);
            foreach (var atom in aromatic)
                atom.IsAromatic = true;
        }

        static IAtomContainer Load(string smi)
        {
            return CDK.SmilesParser.ParseSmiles(smi);
        }

        static IAtomContainer LoadExact(string smi)
        {
            var parser = new SmilesParser(CDK.Builder, false);
            return parser.ParseSmiles(smi);
        }

        [TestMethod()]
        public void TestNoTitle()
        {
            var parser = CDK.SmilesParser;
            var mol = parser.ParseSmiles("CCC");
            Assert.IsNull(mol.GetProperty<object>("cdk:Title"));
        }
    }
}
