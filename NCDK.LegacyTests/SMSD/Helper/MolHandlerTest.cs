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
using NCDK.Silent;
using NCDK.SMSD.Tools;

namespace NCDK.SMSD.Helper
{
    // @author Syed Asad Rahman <asad@ebi.ac.uk>
    // @cdk.module test-smsd
    [TestClass()]
    public class MolHandlerTest
    {
        public MolHandlerTest() { }

        /// <summary>
        /// Test of getMolecule method, of class MolHandler.
        /// </summary>
        [TestMethod()]
        public void TestGetMolecule()
        {
            MolHandler instance = new MolHandler(new AtomContainer(), true, true);
            IAtomContainer result = instance.Molecule;
            Assert.IsNotNull(result);
        }

        /// <summary>
        /// Test of getRemoveHydrogenFlag method, of class MolHandler.
        /// </summary>
        [TestMethod()]
        public void TestGetRemoveHydrogenFlag()
        {
            MolHandler instance = new MolHandler(new AtomContainer(), true, true);
            bool expResult = true;
            bool result = instance.RemoveHydrogenFlag;
            Assert.AreEqual(expResult, result);
        }
    }
}
