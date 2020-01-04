/* Copyright (C) 2012  Egon Willighagen <egonw@users.sf.net>
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
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program; if not, write to the Free Software Foundation, Inc.,
 * 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
 */

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;

namespace NCDK.Renderers.Generators
{
    // @cdk.module test-renderextra
    [TestClass()]
    public class AtomMassGeneratorTest : BasicAtomGeneratorTest
    {
        private AtomMassGenerator generator;

        public override Rect GetCustomCanvas()
        {
            return Rect.Empty;
        }

        public AtomMassGeneratorTest()
            : base()
        {
            this.generator = new AtomMassGenerator();
            base.SetTestedGenerator(generator);
        }

        [TestMethod()]
        public void TestEmptyContainer()
        {
            var emptyContainer = base.builder.NewAtomContainer();

            // nothing should be made
            var root = generator.Generate(emptyContainer, model);
            var elements = elementUtil.GetAllSimpleElements(root);
            Assert.AreEqual(0, elements.Count);
        }
    }
}
