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

namespace NCDK.SMSD.Helper
{
    /// <summary>
    /// Unit testing for the <see cref="LabelContainer"/> class.
    /// </summary>
    // @author     Syed Asad Rahman
    // @cdk.module test-smsd
    [TestClass()]
    public class LabelContainerTest
    {
        [TestMethod()]
        public void TestInstance()
        {
            Assert.IsNotNull(LabelContainer.Instance);
        }

        /// <summary>
        /// Test of addLabel method, of class LabelContainer.
        /// </summary>
        [TestMethod()]
        public void TestAddLabel()
        {
            string label = "R3";
            LabelContainer instance = new LabelContainer();
            instance.Add(label);
            Assert.AreEqual(3, instance.Count);
            int expectedValue = 2;
            Assert.AreEqual(expectedValue, instance.GetLabelID("R3"));
        }

        /// <summary>
        /// Test of getLabelID method, of class LabelContainer.
        /// </summary>
        [TestMethod()]
        public void TestGetLabelID()
        {
            string label = "R3";
            LabelContainer instance = new LabelContainer();
            instance.Add(label);
            int expectedValue = 2;
            Assert.AreEqual(expectedValue, instance.GetLabelID("R3"));
        }

        /// <summary>
        /// Test of getLabel method, of class LabelContainer.
        /// </summary>
        [TestMethod()]
        public void TestGetLabel()
        {
            string label = "R3";
            LabelContainer instance = new LabelContainer();
            instance.Add(label);
            int index = 2;
            string result = instance.GetLabel(index);
            Assert.AreEqual(label, result);
        }

        /// <summary>
        /// Test of getSize method, of class LabelContainer.
        /// </summary>
        [TestMethod()]
        public void TestGetSize()
        {
            string label = "R3";
            LabelContainer instance = new LabelContainer();
            instance.Add(label);
            int expectedValue = 3;
            int result = instance.Count;
            Assert.AreEqual(expectedValue, result);
        }
    }
}
