/* Copyright (C) 2009  Egon Willighagen <egonw@users.sf.net>
 *
 * Contact: cdk-devel@slists.sourceforge.net
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
 * but WITHOUT Any WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
 */
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NCDK.Silent;
using System.IO;
using System;

namespace NCDK.IO.RDF
{
    /// <summary>
    /// TestCase for the <see cref="CDKOWLWriter"/>.
    /// </summary>
    // @cdk.module test-iordf
    [TestClass()]
    public class CDKOWLWriterTest : ChemObjectWriterTest
    {
        protected override Type ChemObjectIOToTestType => typeof(CDKOWLWriter);
        private static IChemObjectBuilder builder = ChemObjectBuilder.Instance;

        [TestMethod()]
        public void TestWriteMolecule()
        {
            StringWriter output = new StringWriter();
            CDKOWLWriter writer = new CDKOWLWriter(output);

            IAtomContainer mol = builder.NewAtomContainer();
            mol.Atoms.Add(new Atom("C"));
            mol.Atoms.Add(new Atom("C"));
            mol.AddBond(mol.Atoms[0], mol.Atoms[1], BondOrder.Double);
            writer.Write(mol);
            writer.Close();
            string outputString = output.ToString();
            Assert.IsTrue(outputString.Contains("http://cdk.sourceforge.net/model.owl#"));
        }
    }
}
