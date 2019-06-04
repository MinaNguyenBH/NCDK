/* Copyright (C) 2007  Miguel Rojasch <miguelrojasch@users.sf.net>
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
using NCDK.Silent;
using NCDK.Tools.Manipulator;
using System;

namespace NCDK.Formula.Rules
{
    // @cdk.module test-formula
    [TestClass()]
    public class RDBERuleTest : FormulaRuleTest
    {
        private static readonly IChemObjectBuilder builder = ChemObjectBuilder.Instance;
        protected override Type RuleClass => typeof(RDBERule);

        [TestMethod()]
        public void TestRDBERule()
        {
            IRule rule = new RDBERule();
            Assert.IsNotNull(rule);
        }

        [TestMethod()]
        public void TestDefault()
        {
            IRule rule = new RDBERule();
            var objects = rule.Parameters;
            Assert.AreEqual(2, objects.Count);

            double min = (double)objects[0];
            double max = (double)objects[1];
            Assert.AreEqual(-0.5, min, 0.00001);
            Assert.AreEqual(30, max, 0.00001);
        }

        [TestMethod()]
        public void TestSetParameters()
        {
            IRule rule = new RDBERule();
            object[] parameters = new object[2];

            parameters[0] = 0.0;
            parameters[1] = 10.0;
            rule.Parameters = parameters;

            var objects = rule.Parameters;
            Assert.AreEqual(2, objects.Count);

            double min = (double)objects[0];
            double max = (double)objects[1];
            Assert.AreEqual(0.0, min, 0.00001);
            Assert.AreEqual(10.0, max, 0.00001);
        }

        [TestMethod()]
        public void TestDefaultValidFalse()
        {
            IRule rule = new RDBERule();

            IMolecularFormula formula = MolecularFormulaManipulator.GetMajorIsotopeMolecularFormula("C2H4", builder);

            Assert.AreEqual(1.0, rule.Validate(formula), 0.0001);
        }

        /// <summary>
        /// A unit test suite.C2H11N4O4
        /// </summary>
        [TestMethod()]
        public void TestDefaultValidFalse_SetParam()
        {
            RDBERule rule = new RDBERule();

            IMolecularFormula formula = MolecularFormulaManipulator.GetMajorIsotopeMolecularFormula("CH2F10S2", builder);

            var value = rule.GetRDBEValue(formula);
            Assert.AreEqual(6, value.Count, 0.0001);
            Assert.AreEqual(-4.0, value[0], 0.0001);
            Assert.AreEqual(-3.0, value[1], 0.0001);
            Assert.AreEqual(-2.0, value[2], 0.0001);
            Assert.AreEqual(-2.0, value[3], 0.0001);
            Assert.AreEqual(-1.0, value[4], 0.0001);
            Assert.AreEqual(0.0, value[5], 0.0001);

            Assert.AreEqual(1.0, rule.Validate(formula), 0.0001);
        }

        [TestMethod()]
        public void TestDefaultValidTrue()
        {
            IRule rule = new RDBERule();

            IMolecularFormula formula = MolecularFormulaManipulator.GetMajorIsotopeMolecularFormula("C1H4", builder);
            formula.Charge = 0;

            Assert.AreEqual(1.0, rule.Validate(formula), 0.0001);
        }

        [TestMethod()]
        public void TestGetRDBEValue_IMolecularFormula()
        {
            RDBERule rule = new RDBERule();

            IMolecularFormula formula = MolecularFormulaManipulator.GetMajorIsotopeMolecularFormula("C2H4", builder);

            Assert.AreEqual(1.0, rule.GetRDBEValue(formula)[0], 0.0001);
        }

        [TestMethod()]
        public void TestValidate_IMolecularFormula_Double()
        {
            RDBERule rule = new RDBERule();

            IMolecularFormula formula = MolecularFormulaManipulator.GetMajorIsotopeMolecularFormula("C2H4", builder);

            Assert.IsTrue(rule.Validate(formula, 2.0));
        }

        /// <summary>
        /// A unit test suite.C3H8O3S2
        /// </summary>
        [TestMethod()]
        public void Test1()
        {
            RDBERule rule = new RDBERule();

            IMolecularFormula formula = MolecularFormulaManipulator.GetMajorIsotopeMolecularFormula("C3H8O3S2", builder);

            var value = rule.GetRDBEValue(formula);
            Assert.AreEqual(6, value.Count, 0.0001);
            Assert.AreEqual(0.0, value[0], 0.0001);
            Assert.AreEqual(1.0, rule.Validate(formula), 0.0001);
        }

        /// <summary>
        /// A unit test suite.C4H8O3S1
        /// </summary>
        [TestMethod()]
        public void Test2()
        {
            RDBERule rule = new RDBERule();

            IMolecularFormula formula = MolecularFormulaManipulator.GetMajorIsotopeMolecularFormula("C4H8O3S1", builder);

            Assert.AreEqual(1.0, rule.Validate(formula), 0.0001);
        }

        /// <summary>
        /// A unit test suite.NH4+
        /// </summary>
        [TestMethod()]
        public void TestAnticipatedIonState_1()
        {
            RDBERule rule = new RDBERule();

            IMolecularFormula formula = MolecularFormulaManipulator.GetMajorIsotopeMolecularFormula("NH4", builder);
            formula.Charge = 1;

            Assert.AreEqual(1.0, rule.Validate(formula), 0.0001);
        }

        /// <summary>
        /// A unit test suite.NH4+
        /// </summary>
        [TestMethod()]
        public void TestAnticipatedIonState_2()
        {
            RDBERule rule = new RDBERule();

            IMolecularFormula formula = MolecularFormulaManipulator.GetMajorIsotopeMolecularFormula("NH4", builder);

            Assert.AreEqual(0.0, rule.Validate(formula), 0.0001);
        }

        /// <summary>
        /// A unit test suite for lipid PC.
        /// </summary>
        // @cdk.bug 2322906
        [TestMethod()]
        public void TestPCCharged()
        {
            RDBERule rule = new RDBERule();

            IMolecularFormula formula = MolecularFormulaManipulator.GetMajorIsotopeMolecularFormula("C42H85NO8P", builder);
            formula.Charge = 1;

            Assert.AreEqual(1.0, rule.Validate(formula), 0.0001);
        }

        /// <summary>
        /// A unit test suite for B.
        /// </summary>
        [TestMethod()]
        public void TestB()
        {
            RDBERule rule = new RDBERule();

            IMolecularFormula formula = MolecularFormulaManipulator.GetMajorIsotopeMolecularFormula("C6H9BNO2", builder);
            formula.Charge = 1;

            Assert.AreEqual(1.0, rule.Validate(formula), 0.0001);
        }
    }
}
