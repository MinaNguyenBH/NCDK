/* Copyright (C) 2004-2007  The Chemistry Development Kit (CDK) project
 *
 * Contact: cdk-devel@lists.sourceforge.net
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public License
 * as published by the Free Software Foundation; either version 2.1
 * of the License, or (at your option) any later version.
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

namespace NCDK.QSAR.Descriptors.Moleculars
{
    // @cdk.module test-qsarmolecular
    [TestClass()]
    public class WeightDescriptorTest : MolecularDescriptorTest<WeightDescriptor>
    {
        [TestMethod()]
        public void TestWeightDescriptor()
        {
            var sp = CDK.SmilesParser;
            var mol = sp.ParseSmiles("CCC");
            Assert.AreEqual(44.095, CreateDescriptor().Calculate(mol, "*").Value, 0.001);
        }

        // @cdk.bug 2185475
        [TestMethod()]
        public void TestNoHydrogens()
        {
            var builder = CDK.Builder;
            var mol = builder.NewAtomContainer();
            mol.Atoms.Add(builder.NewAtom("C"));
            Assert.AreEqual(12.0107, CreateDescriptor().Calculate(mol, "*").Value, 0.001);
        }

        // @cdk.bug 2185475
        [TestMethod()]
        public void TestExplicitHydrogens()
        {
            var builder = CDK.Builder;
            var mol = builder.NewAtomContainer();
            mol.Atoms.Add(builder.NewAtom("C"));
            mol.Atoms.Add(builder.NewAtom("H"));
            mol.Atoms.Add(builder.NewAtom("H"));
            mol.Atoms.Add(builder.NewAtom("H"));
            mol.Atoms.Add(builder.NewAtom("H"));
            mol.AddBond(mol.Atoms[0], mol.Atoms[1], BondOrder.Single);
            mol.AddBond(mol.Atoms[0], mol.Atoms[2], BondOrder.Single);
            mol.AddBond(mol.Atoms[0], mol.Atoms[3], BondOrder.Single);
            mol.AddBond(mol.Atoms[0], mol.Atoms[4], BondOrder.Single);
            Assert.AreEqual(16.042, CreateDescriptor().Calculate(mol, "*").Value, 0.001);
        }

        // @cdk.bug 2185475
        [TestMethod()]
        public void TestImplicitHydrogens()
        {
            var builder = CDK.Builder;
            var mol = builder.NewAtomContainer();
            mol.Atoms.Add(builder.NewAtom("C"));
            mol.Atoms[0].ImplicitHydrogenCount = 4;
            Assert.AreEqual(16.042, CreateDescriptor().Calculate(mol, "*").Value, 0.001);
        }
    }
}
