/* Copyright (C) 2002-2007  The Chemistry Development Kit (CDK) project
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
 * but WITHOUT Any WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
 */

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Text;

namespace NCDK.IO
{
    /// <summary>
    /// Combined TestCase for the reading/writing of mdl and cml files.
    /// </summary>
    // @cdk.module test-libiocml
    [TestClass()]
    public class MDLCMLRoundtripTest
    {
        private readonly IChemObjectBuilder builder = CDK.Builder;

        public MDLCMLRoundtripTest()
            : base()
        { }

        // @cdk.bug 1649526
        [TestMethod()]
        public void TestBug1649526()
        {
            //Read the original
            var filename = "NCDK.Data.MDL.bug-1649526.mol";
            var ins = ResourceLoader.GetAsStream(filename);
            var reader = new MDLReader(ins);
            var mol = reader.Read(builder.NewAtomContainer());
            reader.Close();
            //Write it as cml
            var writer = new StringWriter();
            var cmlWriter = new CMLWriter(writer);
            cmlWriter.Write(mol);
            cmlWriter.Close();
            //Read this again
            var cmlreader = new CMLReader(new MemoryStream(Encoding.UTF8.GetBytes(writer.ToString())));
            var file = (IChemFile)cmlreader.Read(builder.NewChemFile());
            cmlreader.Close();
            //And finally write as mol
            var writermdl = new StringWriter();
            var mdlWriter = new MDLV2000Writer(writermdl);
            mdlWriter.Write(file);
            mdlWriter.Close();
            var output = writermdl.ToString().Replace("\r\n", "\n");
            //if there would be 3 instances (as in the bug), the only instance wouldnt't be right at the end
            Assert.AreEqual(2961, output.IndexOf("M  END"));
            //there would need some $$$$ to be in
            Assert.AreEqual(-1, output.IndexOf("$$$$"));
            //check atom/bond count
            Assert.AreEqual(25, output.IndexOf(" 31 33  0  0  0  0"));
        }
    }
}
