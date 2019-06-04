/* Copyright (C) 2008  Miguel Rojas <miguelrojasch@yahoo.es>
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

namespace NCDK.Reactions.Mechanisms
{
    /// <summary>
    /// Tests for RemovingSEofNBMechanism implementations.
    /// </summary>
    // @cdk.module test-reaction
    [TestClass()]
    public class RemovingSEofNBMechanismTest : ReactionMechanismTest
    {
        public RemovingSEofNBMechanismTest()
            : base()
        {
            SetMechanism(typeof(RemovingSEofNBMechanism));
        }

        [TestMethod()]
        public void TestRemovingSEofNBMechanism()
        {
            var mechanism = new RemovingSEofNBMechanism();
            Assert.IsNotNull(mechanism);
        }

        [TestMethod()]
        public void TestInitiate_IAtomContainerSet_ArrayList_ArrayList()
        {
            var mechanism = new RemovingSEofNBMechanism();

            Assert.IsNotNull(mechanism);
        }
    }
}
