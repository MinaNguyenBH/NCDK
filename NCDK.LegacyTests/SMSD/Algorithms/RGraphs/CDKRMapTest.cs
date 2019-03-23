/* Copyright (C) 2007  Egon Willighagen <egonw@users.sf.net>
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

namespace NCDK.SMSD.Algorithms.RGraphs
{
    // @cdk.module test-smsd
    // @author     Syed Asad Rahman
    [TestClass()]
    public class CDKRMapTest : CDKTestCase
    {
        [TestMethod()]
        public void TestRMap_int_int()
        {
            CDKRMap node = new CDKRMap(1, 2);
            Assert.IsNotNull(node);
            Assert.AreEqual(1, node.Id1);
            Assert.AreEqual(2, node.Id2);
        }
    }
}
