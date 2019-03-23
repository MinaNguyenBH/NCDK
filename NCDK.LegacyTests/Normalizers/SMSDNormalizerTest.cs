/* Copyright (C) 2010  Egon Willighagen <egonw@users.sf.net>
 *
 * Contact: cdk-devel@lists.sourceforge.net
 *
 * This program is free software; you can redistribute it and/or modify it under
 * the terms of the GNU Lesser General Public License as published by the Free
 * Software Foundation; either version 2.1 of the License, or (at your option)
 * any later version. All we ask is that proper credit is given for our work,
 * which includes - but is not limited to - adding the above copyright notice to
 * the beginning of your source code files, and to any copyright notice that you
 * may distribute with programs based on this work.
 *
 * This program is distributed in the hope that it will be useful, but WITHOUT
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
 * FOR A PARTICULAR PURPOSE.  See the GNU Lesser General Public License for more
 * details.
 *
 * You should have received rAtomCount copy of the GNU Lesser General Public
 * License along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
 */
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NCDK.Silent;
using NCDK.Smiles;
using System;

namespace NCDK.Normalizers
{
    // @author Syed Asad Rahman <asad@ebi.ac.uk>
    // @cdk.module test-smsd
    [TestClass()]
    public class SMSDNormalizerTest
    {
        public SMSDNormalizerTest() { }

        /// <summary>
        /// Test of makeDeepCopy method, of class SMSDNormalizer.
        /// </summary>
        [TestMethod()]
        public void TestMakeDeepCopy()
        {
            string rawMolSmiles = "[H]POOSC(Br)C(Cl)C(F)I";
            var sp = CDK.SmilesParser;
            var container = sp.ParseSmiles(rawMolSmiles);

            int counter = 0;
            foreach (var a in container.Atoms)
            {
                a.Id = (counter++).ToString();
            }

            IAtomContainer result = SMSDNormalizer.MakeDeepCopy(container);
            for (int i = 0; i < result.Atoms.Count; i++)
            {
                Assert.AreEqual(result.Atoms[i].Symbol, container.Atoms[i].Symbol);
                Assert.AreEqual(result.Atoms[i].Id, container.Atoms[i].Id);
            }
        }

        /// <summary>
        /// Test of aromatizeMolecule method, of class SMSDNormalizer.
        /// </summary>
        [TestMethod()]
        public void TestAromatizeMolecule()
        {
            string rawMolSmiles = "C1=CC2=C(C=C1)C=CC=C2";
            var sp = CDK.SmilesParser;
            var mol = sp.ParseSmiles(rawMolSmiles);
            SMSDNormalizer.AromatizeMolecule(mol);
            int count = 0;
            foreach (var b in mol.Bonds)
            {
                if (b.IsAromatic && b.Order.Equals(BondOrder.Double))
                {
                    count++;
                }
            }
            Assert.AreEqual(5, count);
        }

        /// <summary>
        /// Test of getExplicitHydrogenCount method, of class SMSDNormalizer.
        /// </summary>
        [TestMethod()]
        public void TestGetExplicitHydrogenCount()
        {
            string rawMolSmiles = "[H]POOSC(Br)C(Cl)C(F)I";
            var sp = CDK.SmilesParser;
            var atomContainer = sp.ParseSmiles(rawMolSmiles);
            IAtom atom = null;
            foreach (var a in atomContainer.Atoms)
            {
                if (string.Equals(a.Symbol, "P", StringComparison.OrdinalIgnoreCase))
                {
                    atom = a;
                    break;
                }
            }

            int expResult = 1;
            int result = SMSDNormalizer.GetExplicitHydrogenCount(atomContainer, atom);
            Assert.AreEqual(expResult, result);
        }

        /// <summary>
        /// Test of getImplicitHydrogenCount method, of class SMSDNormalizer.
        /// </summary>
        [TestMethod()]
        public void TestGetImplicitHydrogenCount()
        {
            string rawMolSmiles = "[H]POOSC(Br)C(Cl)C(F)I";
            var sp = CDK.SmilesParser;
            var atomContainer = sp.ParseSmiles(rawMolSmiles);
            IAtom atom = null;
            foreach (var a in atomContainer.Atoms)
            {
                if (string.Equals(a.Symbol, "P", StringComparison.OrdinalIgnoreCase))
                {
                    atom = a;
                    break;
                }
            }

            int expResult = 1;
            int result = SMSDNormalizer.GetImplicitHydrogenCount(atomContainer, atom);
            Assert.AreEqual(expResult, result);
        }

        /// <summary>
        /// Test of getHydrogenCount method, of class SMSDNormalizer.
        // @throws InvalidSmilesException
        /// </summary>
        [TestMethod()]
        public void TestGetHydrogenCount()
        {
            string rawMolSmiles = "[H]POOSC(Br)C(Cl)C(F)I";
            var sp = CDK.SmilesParser;
            var atomContainer = sp.ParseSmiles(rawMolSmiles);
            IAtom atom = null;
            foreach (var a in atomContainer.Atoms)
            {
                if (string.Equals(a.Symbol, "P", StringComparison.OrdinalIgnoreCase))
                {
                    atom = a;
                    break;
                }
            }
            int expResult = 2;
            int result = SMSDNormalizer.GetHydrogenCount(atomContainer, atom);
            Assert.AreEqual(expResult, result);
        }

        /// <summary>
        /// Test of removeHydrogensAndPreserveAtomID method, of class SMSDNormalizer.
        /// </summary>
        [TestMethod()]
        public void TestRemoveHydrogensAndPreserveAtomID()
        {
            string rawMolSmiles = "[H]POOSC(Br)C(Cl)C(F)I";
            var sp = CDK.SmilesParser;
            var atomContainer = sp.ParseSmiles(rawMolSmiles);
            IAtom beforeAtom = null;
            IAtom afterAtom = null;
            foreach (var a in atomContainer.Atoms)
            {
                if (string.Equals(a.Symbol, "P", StringComparison.OrdinalIgnoreCase))
                {
                    beforeAtom = a;
                    a.Id = "TEST";
                    break;
                }
            }
            IAtomContainer result = SMSDNormalizer.RemoveHydrogensAndPreserveAtomID(atomContainer);

            foreach (var a in result.Atoms)
            {
                if (string.Equals(a.Symbol, "P", StringComparison.OrdinalIgnoreCase))
                {
                    afterAtom = a;
                    break;
                }
            }

            Assert.AreEqual(afterAtom.Id, beforeAtom.Id);
        }

        /// <summary>
        /// Test of convertExplicitToImplicitHydrogens method, of class SMSDNormalizer.
        /// </summary>
        [TestMethod()]
        public void TestConvertExplicitToImplicitHydrogens()
        {
            string rawMolSmiles = "[H]POOSC(Br)C(Cl)C(F)I";
            var sp = CDK.SmilesParser;
            var atomContainer = sp.ParseSmiles(rawMolSmiles);
            int expResult = 11;
            IAtomContainer result = SMSDNormalizer.ConvertExplicitToImplicitHydrogens(atomContainer);
            Assert.AreEqual(expResult, result.Atoms.Count);
        }

        /// <summary>
        /// Test of percieveAtomTypesAndConfigureAtoms method, of class SMSDNormalizer.
        /// </summary>
        [TestMethod()]
        public void TestPercieveAtomTypesAndConfigureAtoms()
        {
            string rawMolSmiles = "[H]POOSC(Br)C(Cl)C(F)I";
            var sp = CDK.SmilesParser;
            var atomContainer = sp.ParseSmiles(rawMolSmiles);
            SMSDNormalizer.PercieveAtomTypesAndConfigureAtoms(atomContainer);
            Assert.IsNotNull(atomContainer);
        }
    }
}
