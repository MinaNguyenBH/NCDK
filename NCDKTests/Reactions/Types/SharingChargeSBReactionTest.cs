/* Copyright (C) 2004-2007  Miguel Rojas <miguel.rojas@uni-koeln.de>
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
using NCDK.Isomorphisms;
using NCDK.Isomorphisms.Matchers;
using NCDK.Reactions.Types.Parameters;
using NCDK.Tools.Manipulator;
using System;
using System.Collections.Generic;

namespace NCDK.Reactions.Types
{
    /// <summary>
    /// TestSuite that runs a test for the SharingChargeDBReactionTest.
    /// Generalized Reaction: [A+]=B => A| + [B+].
    /// </summary>
    // @cdk.module test-reaction
    [TestClass()]
    public class SharingChargeSBReactionTest : ReactionProcessTest
    {
        private IChemObjectBuilder builder = CDK.Builder;

        public SharingChargeSBReactionTest()
        {
            SetReaction(typeof(SharingChargeSBReaction));
        }

        [TestMethod()]
        public void TestSharingChargeSBReaction()
        {
            var type = new SharingChargeSBReaction();
            Assert.IsNotNull(type);
        }

        /// <summary>
        /// A unit test suite. Reaction:  methoxymethane.
        /// C[O+]!-!C =>  CO + [C+]
        /// </summary>
        [TestMethod()]
        public override void TestInitiate_IAtomContainerSet_IAtomContainerSet()
        {
            var type = new SharingChargeSBReaction();

            var setOfReactants = GetExampleReactants();

            /* initiate */

            var paramList = new List<IParameterReaction>();
            var param = new SetReactionCenter();
            param.IsSetParameter = false;
            paramList.Add(param);
            type.ParameterList = paramList;
            var setOfReactions = type.Initiate(setOfReactants, null);

            Assert.AreEqual(3, setOfReactions.Count);
            Assert.AreEqual(2, setOfReactions[0].Products.Count);

            var product1 = setOfReactions[1].Products[0];

            var molecule1 = GetExpectedProducts()[0];
            var queryAtom = QueryAtomContainerCreator.CreateSymbolAndChargeQueryContainer(product1);
            Assert.IsTrue(new UniversalIsomorphismTester().IsIsomorph(molecule1, queryAtom));

            var product2 = setOfReactions[0].Products[1];
            var expected2 = GetExpectedProducts()[0];
            queryAtom = QueryAtomContainerCreator.CreateSymbolAndChargeQueryContainer(expected2);
            Assert.IsTrue(new UniversalIsomorphismTester().IsIsomorph(product2, queryAtom));
        }

        /// <summary>
        /// A unit test suite. Reaction:  methoxymethane.
        /// C[O+]!-!C =>  CO + [C+]
        /// Manually put of the center active.
        /// </summary>
        [TestMethod()]
        public void TestManuallyCentreActive()
        {
            var type = new SharingChargeSBReaction();

            var setOfReactants = GetExampleReactants();
            var molecule = setOfReactants[0];

            /* manually put the center active */
            molecule.Atoms[1].IsReactiveCenter = true;
            molecule.Atoms[2].IsReactiveCenter = true;
            molecule.Bonds[1].IsReactiveCenter = true;

            var paramList = new List<IParameterReaction>();
            var param = new SetReactionCenter();
            param.IsSetParameter = true;
            paramList.Add(param);
            type.ParameterList = paramList;
            /* initiate */

            var setOfReactions = type.Initiate(setOfReactants, null);

            Assert.AreEqual(1, setOfReactions.Count);
            Assert.AreEqual(2, setOfReactions[0].Products.Count);

            var product1 = setOfReactions[0].Products[0];
            var molecule1 = GetExpectedProducts()[0];
            var queryAtom = QueryAtomContainerCreator.CreateSymbolAndChargeQueryContainer(product1);
            Assert.IsTrue(new UniversalIsomorphismTester().IsIsomorph(molecule1, queryAtom));

            var product2 = setOfReactions[0].Products[1];
            var expected2 = GetExpectedProducts()[1];
            queryAtom = QueryAtomContainerCreator.CreateSymbolAndChargeQueryContainer(expected2);
            Assert.IsTrue(new UniversalIsomorphismTester().IsIsomorph(product2, queryAtom));
        }

        [TestMethod()]
        public void TestCDKConstants_REACTIVE_CENTER()
        {
            var type = new SharingChargeSBReaction();

            var setOfReactants = GetExampleReactants();
            var molecule = setOfReactants[0];

            /* manually put the reactive center */
            molecule.Atoms[1].IsReactiveCenter = true;
            molecule.Atoms[2].IsReactiveCenter = true;
            molecule.Bonds[1].IsReactiveCenter = true;

            var paramList = new List<IParameterReaction>();
            var param = new SetReactionCenter();
            param.IsSetParameter = true;
            paramList.Add(param);
            type.ParameterList = paramList;

            /* initiate */
            var setOfReactions = type.Initiate(setOfReactants, null);

            var reactant = setOfReactions[0].Reactants[0];
            Assert.IsTrue(molecule.Atoms[1].IsReactiveCenter);
            Assert.IsTrue(reactant.Atoms[1].IsReactiveCenter);
            Assert.IsTrue(molecule.Atoms[2].IsReactiveCenter);
            Assert.IsTrue(reactant.Atoms[2].IsReactiveCenter);
            Assert.IsTrue(molecule.Bonds[1].IsReactiveCenter);
            Assert.IsTrue(reactant.Bonds[1].IsReactiveCenter);
        }

        [TestMethod()]
        public void TestMapping()
        {
            var type = new SharingChargeSBReaction();

            var setOfReactants = GetExampleReactants();
            var molecule = setOfReactants[0];

            /* manually put the reactive center */
            molecule.Atoms[1].IsReactiveCenter = true;
            molecule.Atoms[2].IsReactiveCenter = true;
            molecule.Bonds[1].IsReactiveCenter = true;

            var paramList = new List<IParameterReaction>();
            var param = new SetReactionCenter();
            param.IsSetParameter = true;
            paramList.Add(param);
            type.ParameterList = paramList;
            /* initiate */

            var setOfReactions = type.Initiate(setOfReactants, null);

            var product1 = setOfReactions[0].Products[0];
            var product2 = setOfReactions[0].Products[1];

            Assert.AreEqual(10, setOfReactions[0].Mappings.Count);

            var mappedProductA1 = (IAtom)ReactionManipulator.GetMappedChemObject(setOfReactions[0], molecule.Atoms[1]);
            Assert.AreEqual(mappedProductA1, product1.Atoms[1]);
            mappedProductA1 = (IAtom)ReactionManipulator.GetMappedChemObject(setOfReactions[0], molecule.Atoms[2]);
            Assert.AreEqual(mappedProductA1, product2.Atoms[0]);
        }

        /// <summary>
        /// Test to recognize if this IAtomContainer_1 matches correctly into the CDKAtomTypes.
        /// </summary>
        [TestMethod()]
        public void TestAtomTypesAtomContainer1()
        {
            var moleculeTest = GetExampleReactants()[0];
            MakeSureAtomTypesAreRecognized(moleculeTest);
        }

        /// <summary>
        /// Test to recognize if this IAtomContainer_2 matches correctly into the CDKAtomTypes.
        /// </summary>
        [TestMethod()]
        public void TestAtomTypesAtomContainer2()
        {
            var moleculeTest = GetExpectedProducts()[0];
            MakeSureAtomTypesAreRecognized(moleculeTest);
        }

        /// <summary>
        /// get the molecule 1: C[O+]!-!C
        /// </summary>
        /// <returns>The IAtomContainerSet</returns>
        private IChemObjectSet<IAtomContainer> GetExampleReactants()
        {
            var setOfReactants = CDK.Builder.NewAtomContainerSet();

            var molecule = builder.NewAtomContainer();
            molecule.Atoms.Add(builder.NewAtom("C"));
            molecule.Atoms.Add(builder.NewAtom("O"));
            molecule.Atoms[1].FormalCharge = +1;
            molecule.Atoms.Add(builder.NewAtom("C"));
            molecule.AddBond(molecule.Atoms[0], molecule.Atoms[1], BondOrder.Single);
            molecule.AddBond(molecule.Atoms[1], molecule.Atoms[2], BondOrder.Single);
            molecule.Atoms.Add(builder.NewAtom("H"));
            molecule.Atoms.Add(builder.NewAtom("H"));
            molecule.Atoms.Add(builder.NewAtom("H"));
            molecule.Atoms.Add(builder.NewAtom("H"));
            molecule.Atoms.Add(builder.NewAtom("H"));
            molecule.Atoms.Add(builder.NewAtom("H"));
            molecule.Atoms.Add(builder.NewAtom("H"));
            molecule.AddBond(molecule.Atoms[0], molecule.Atoms[3], BondOrder.Single);
            molecule.AddBond(molecule.Atoms[0], molecule.Atoms[4], BondOrder.Single);
            molecule.AddBond(molecule.Atoms[0], molecule.Atoms[5], BondOrder.Single);
            molecule.AddBond(molecule.Atoms[1], molecule.Atoms[6], BondOrder.Single);
            molecule.AddBond(molecule.Atoms[2], molecule.Atoms[7], BondOrder.Single);
            molecule.AddBond(molecule.Atoms[2], molecule.Atoms[8], BondOrder.Single);
            molecule.AddBond(molecule.Atoms[2], molecule.Atoms[9], BondOrder.Single);
            try
            {
                AtomContainerManipulator.PercieveAtomTypesAndConfigureAtoms(molecule);
                CDK.LonePairElectronChecker.Saturate(molecule);
            }
            catch (CDKException e)
            {
                Console.Out.WriteLine(e.StackTrace);
            }

            setOfReactants.Add(molecule);
            return setOfReactants;
        }

        /// <summary>
        /// Get the expected set of molecules.
        /// </summary>
        /// <returns>The IAtomContainerSet</returns>
        private IChemObjectSet<IAtomContainer> GetExpectedProducts()
        {
            var setOfProducts = builder.NewAtomContainerSet();

            var expected1 = builder.NewAtomContainer();
            expected1.Atoms.Add(builder.NewAtom("C"));
            expected1.Atoms.Add(builder.NewAtom("O"));
            expected1.AddBond(expected1.Atoms[0], expected1.Atoms[1], BondOrder.Single);
            expected1.Atoms.Add(builder.NewAtom("H"));
            expected1.Atoms.Add(builder.NewAtom("H"));
            expected1.Atoms.Add(builder.NewAtom("H"));
            expected1.Atoms.Add(builder.NewAtom("H"));
            expected1.AddBond(expected1.Atoms[0], expected1.Atoms[2], BondOrder.Single);
            expected1.AddBond(expected1.Atoms[0], expected1.Atoms[3], BondOrder.Single);
            expected1.AddBond(expected1.Atoms[0], expected1.Atoms[4], BondOrder.Single);
            expected1.AddBond(expected1.Atoms[1], expected1.Atoms[5], BondOrder.Single);
            try
            {
                AtomContainerManipulator.PercieveAtomTypesAndConfigureAtoms(expected1);
                CDK.LonePairElectronChecker.Saturate(expected1);
            }
            catch (CDKException e)
            {
                Console.Out.WriteLine(e.StackTrace);
            }

            var expected2 = builder.NewAtomContainer();
            expected2.Atoms.Add(builder.NewAtom("C"));
            expected2.Atoms[0].FormalCharge = +1;
            expected2.Atoms.Add(builder.NewAtom("H"));
            expected2.Atoms.Add(builder.NewAtom("H"));
            expected2.Atoms.Add(builder.NewAtom("H"));
            expected2.AddBond(expected2.Atoms[0], expected2.Atoms[1], BondOrder.Single);
            expected2.AddBond(expected2.Atoms[0], expected2.Atoms[2], BondOrder.Single);
            expected2.AddBond(expected2.Atoms[0], expected2.Atoms[3], BondOrder.Single);
            try
            {
                AtomContainerManipulator.PercieveAtomTypesAndConfigureAtoms(expected2);
            }
            catch (CDKException e)
            {
                Console.Out.WriteLine(e.StackTrace);
            }

            setOfProducts.Add(expected1);
            setOfProducts.Add(expected2);
            return setOfProducts;
        }

        /// <summary>
        /// Test to recognize if a IAtomContainer matcher correctly identifies the CDKAtomTypes.
        /// </summary>
        /// <param name="molecule">The IAtomContainer to analyze</param>
        /// <exception cref="CDKException"></exception>
        private void MakeSureAtomTypesAreRecognized(IAtomContainer molecule)
        {
            var matcher = CDK.AtomTypeMatcher;
            foreach (var nextAtom in molecule.Atoms)
            {
                Assert.IsNotNull(matcher.FindMatchingAtomType(molecule, nextAtom), "Missing atom type for: " + nextAtom);
            }
        }

        /// <summary>
        /// A unit test suite. Reaction:.
        /// C[N+]!-!C => CN + [C+]
        /// </summary>
        [TestMethod()]
        public void TestNsp3ChargeSingleB()
        {
            //CreateFromSmiles("C[N+]C")
            var molecule = builder.NewAtomContainer();
            molecule.Atoms.Add(builder.NewAtom("C"));
            molecule.Atoms.Add(builder.NewAtom("N"));
            molecule.Atoms[1].FormalCharge = +1;
            molecule.Atoms.Add(builder.NewAtom("C"));
            molecule.AddBond(molecule.Atoms[0], molecule.Atoms[1], BondOrder.Single);
            molecule.AddBond(molecule.Atoms[1], molecule.Atoms[2], BondOrder.Single);
            molecule.Atoms.Add(builder.NewAtom("H"));
            molecule.Atoms.Add(builder.NewAtom("H"));
            molecule.Atoms.Add(builder.NewAtom("H"));
            molecule.Atoms.Add(builder.NewAtom("H"));
            molecule.Atoms.Add(builder.NewAtom("H"));
            molecule.Atoms.Add(builder.NewAtom("H"));
            molecule.Atoms.Add(builder.NewAtom("H"));
            molecule.Atoms.Add(builder.NewAtom("H"));
            molecule.AddBond(molecule.Atoms[0], molecule.Atoms[3], BondOrder.Single);
            molecule.AddBond(molecule.Atoms[0], molecule.Atoms[4], BondOrder.Single);
            molecule.AddBond(molecule.Atoms[0], molecule.Atoms[5], BondOrder.Single);
            molecule.AddBond(molecule.Atoms[1], molecule.Atoms[6], BondOrder.Single);
            molecule.AddBond(molecule.Atoms[1], molecule.Atoms[7], BondOrder.Single);
            molecule.AddBond(molecule.Atoms[2], molecule.Atoms[8], BondOrder.Single);
            molecule.AddBond(molecule.Atoms[2], molecule.Atoms[9], BondOrder.Single);
            molecule.AddBond(molecule.Atoms[2], molecule.Atoms[10], BondOrder.Single);
            AtomContainerManipulator.PercieveAtomTypesAndConfigureAtoms(molecule);
            CDK.LonePairElectronChecker.Saturate(molecule);

            molecule.Atoms[1].IsReactiveCenter = true;
            molecule.Atoms[2].IsReactiveCenter = true;
            molecule.Bonds[1].IsReactiveCenter = true;

            var setOfReactants = CDK.Builder.NewAtomContainerSet();
            setOfReactants.Add(molecule);

            var type = new SharingChargeSBReaction();
            var paramList = new List<IParameterReaction>();
            var param = new SetReactionCenter();
            param.IsSetParameter = true;
            paramList.Add(param);
            type.ParameterList = paramList;

            /* initiate */
            var setOfReactions = type.Initiate(setOfReactants, null);

            Assert.AreEqual(1, setOfReactions.Count);

            // expected products

            //CreateFromSmiles("CN")
            var expected1 = builder.NewAtomContainer();
            expected1.Atoms.Add(builder.NewAtom("C"));
            expected1.Atoms.Add(builder.NewAtom("N"));
            expected1.AddBond(expected1.Atoms[0], expected1.Atoms[1], BondOrder.Single);
            expected1.Atoms.Add(builder.NewAtom("H"));
            expected1.Atoms.Add(builder.NewAtom("H"));
            expected1.Atoms.Add(builder.NewAtom("H"));
            expected1.Atoms.Add(builder.NewAtom("H"));
            expected1.Atoms.Add(builder.NewAtom("H"));
            expected1.AddBond(expected1.Atoms[0], expected1.Atoms[2], BondOrder.Single);
            expected1.AddBond(expected1.Atoms[0], expected1.Atoms[3], BondOrder.Single);
            expected1.AddBond(expected1.Atoms[0], expected1.Atoms[4], BondOrder.Single);
            expected1.AddBond(expected1.Atoms[1], expected1.Atoms[5], BondOrder.Single);
            expected1.AddBond(expected1.Atoms[1], expected1.Atoms[6], BondOrder.Single);
            AtomContainerManipulator.PercieveAtomTypesAndConfigureAtoms(expected1);
            CDK.LonePairElectronChecker.Saturate(expected1);
            var product1 = setOfReactions[0].Products[0];
            var queryAtom = QueryAtomContainerCreator.CreateSymbolAndChargeQueryContainer(expected1);
            Assert.IsTrue(new UniversalIsomorphismTester().IsIsomorph(product1, queryAtom));

            //CreateFromSmiles("[C+]")
            var expected2 = builder.NewAtomContainer();
            expected2.Atoms.Add(builder.NewAtom("C"));
            expected2.Atoms[0].FormalCharge = +1;
            expected2.Atoms.Add(builder.NewAtom("H"));
            expected2.Atoms.Add(builder.NewAtom("H"));
            expected2.Atoms.Add(builder.NewAtom("H"));
            expected2.AddBond(expected2.Atoms[0], expected2.Atoms[1], BondOrder.Single);
            expected2.AddBond(expected2.Atoms[0], expected2.Atoms[2], BondOrder.Single);
            expected2.AddBond(expected2.Atoms[0], expected2.Atoms[3], BondOrder.Single);
            AtomContainerManipulator.PercieveAtomTypesAndConfigureAtoms(expected2);
            var product2 = setOfReactions[0].Products[1];
            queryAtom = QueryAtomContainerCreator.CreateSymbolAndChargeQueryContainer(expected2);
            Assert.IsTrue(new UniversalIsomorphismTester().IsIsomorph(product2, queryAtom));
        }

        /// <summary>
        /// Reaction: C=[N+]!-!C => C=N + [C+]
        /// </summary>
        [TestMethod()]
        public void TestNsp2ChargeSingleB()
        {
            //CreateFromSmiles("C=[N+]C")
            var molecule = builder.NewAtomContainer();
            molecule.Atoms.Add(builder.NewAtom("C"));
            molecule.Atoms.Add(builder.NewAtom("N"));
            molecule.Atoms[1].FormalCharge = 1;
            molecule.Atoms.Add(builder.NewAtom("C"));
            molecule.AddBond(molecule.Atoms[0], molecule.Atoms[1], BondOrder.Double);
            molecule.AddBond(molecule.Atoms[1], molecule.Atoms[2], BondOrder.Single);
            molecule.Atoms.Add(builder.NewAtom("H"));
            molecule.Atoms.Add(builder.NewAtom("H"));
            molecule.Atoms.Add(builder.NewAtom("H"));
            molecule.Atoms.Add(builder.NewAtom("H"));
            molecule.Atoms.Add(builder.NewAtom("H"));
            molecule.Atoms.Add(builder.NewAtom("H"));
            molecule.AddBond(molecule.Atoms[0], molecule.Atoms[3], BondOrder.Single);
            molecule.AddBond(molecule.Atoms[0], molecule.Atoms[4], BondOrder.Single);
            molecule.AddBond(molecule.Atoms[1], molecule.Atoms[5], BondOrder.Single);
            molecule.AddBond(molecule.Atoms[2], molecule.Atoms[6], BondOrder.Single);
            molecule.AddBond(molecule.Atoms[2], molecule.Atoms[7], BondOrder.Single);
            molecule.AddBond(molecule.Atoms[2], molecule.Atoms[8], BondOrder.Single);
            AtomContainerManipulator.PercieveAtomTypesAndConfigureAtoms(molecule);
            CDK.LonePairElectronChecker.Saturate(molecule);

            molecule.Atoms[1].IsReactiveCenter = true;
            molecule.Atoms[2].IsReactiveCenter = true;
            molecule.Bonds[1].IsReactiveCenter = true;

            var setOfReactants = CDK.Builder.NewAtomContainerSet();
            setOfReactants.Add(molecule);

            var type = new SharingChargeSBReaction();
            var paramList = new List<IParameterReaction>();
            var param = new SetReactionCenter();
            param.IsSetParameter = true;
            paramList.Add(param);
            type.ParameterList = paramList;

            /* initiate */
            var setOfReactions = type.Initiate(setOfReactants, null);

            Assert.AreEqual(1, setOfReactions.Count);

            // expected products

            //CreateFromSmiles("C=N")
            var expected1 = builder.NewAtomContainer();
            expected1.Atoms.Add(builder.NewAtom("C"));
            expected1.Atoms.Add(builder.NewAtom("N"));
            expected1.AddBond(expected1.Atoms[0], expected1.Atoms[1], BondOrder.Double);
            expected1.Atoms.Add(builder.NewAtom("H"));
            expected1.Atoms.Add(builder.NewAtom("H"));
            expected1.Atoms.Add(builder.NewAtom("H"));
            expected1.AddBond(expected1.Atoms[0], expected1.Atoms[2], BondOrder.Single);
            expected1.AddBond(expected1.Atoms[0], expected1.Atoms[3], BondOrder.Single);
            expected1.AddBond(expected1.Atoms[1], expected1.Atoms[4], BondOrder.Single);
            AtomContainerManipulator.PercieveAtomTypesAndConfigureAtoms(expected1);
            CDK.LonePairElectronChecker.Saturate(expected1);
            var product1 = setOfReactions[0].Products[0];
            var queryAtom = QueryAtomContainerCreator.CreateSymbolAndChargeQueryContainer(expected1);
            Assert.IsTrue(new UniversalIsomorphismTester().IsIsomorph(product1, queryAtom));

            //CreateFromSmiles("[C+]")
            var expected2 = builder.NewAtomContainer();
            expected2.Atoms.Add(builder.NewAtom("C"));
            expected2.Atoms[0].FormalCharge = +1;
            expected2.Atoms.Add(builder.NewAtom("H"));
            expected2.Atoms.Add(builder.NewAtom("H"));
            expected2.Atoms.Add(builder.NewAtom("H"));
            expected2.AddBond(expected2.Atoms[0], expected2.Atoms[1], BondOrder.Single);
            expected2.AddBond(expected2.Atoms[0], expected2.Atoms[2], BondOrder.Single);
            expected2.AddBond(expected2.Atoms[0], expected2.Atoms[3], BondOrder.Single);
            AtomContainerManipulator.PercieveAtomTypesAndConfigureAtoms(expected2);
            var product2 = setOfReactions[0].Products[1];
            queryAtom = QueryAtomContainerCreator.CreateSymbolAndChargeQueryContainer(expected2);
            Assert.IsTrue(new UniversalIsomorphismTester().IsIsomorph(product2, queryAtom));
        }

        /// <summary>
        /// Reaction: [F+]!-!C => F + [C+]
        /// </summary>
        [TestMethod()]
        public void TestFspChargeSingleB()
        {
            //CreateFromSmiles("[F+]C")
            var molecule = builder.NewAtomContainer();
            molecule.Atoms.Add(builder.NewAtom("F"));
            molecule.Atoms[0].FormalCharge = +1;
            molecule.Atoms.Add(builder.NewAtom("C"));
            molecule.AddBond(molecule.Atoms[0], molecule.Atoms[1], BondOrder.Single);
            molecule.Atoms.Add(builder.NewAtom("H"));
            molecule.Atoms.Add(builder.NewAtom("H"));
            molecule.Atoms.Add(builder.NewAtom("H"));
            molecule.Atoms.Add(builder.NewAtom("H"));
            molecule.AddBond(molecule.Atoms[0], molecule.Atoms[2], BondOrder.Single);
            molecule.AddBond(molecule.Atoms[1], molecule.Atoms[3], BondOrder.Single);
            molecule.AddBond(molecule.Atoms[1], molecule.Atoms[4], BondOrder.Single);
            molecule.AddBond(molecule.Atoms[1], molecule.Atoms[5], BondOrder.Single);
            AtomContainerManipulator.PercieveAtomTypesAndConfigureAtoms(molecule);
            CDK.LonePairElectronChecker.Saturate(molecule);

            molecule.Atoms[0].IsReactiveCenter = true;
            molecule.Atoms[1].IsReactiveCenter = true;
            molecule.Bonds[0].IsReactiveCenter = true;

            var setOfReactants = CDK.Builder.NewAtomContainerSet();
            setOfReactants.Add(molecule);

            var type = new SharingChargeSBReaction();
            var paramList = new List<IParameterReaction>();
            var param = new SetReactionCenter();
            param.IsSetParameter = true;
            paramList.Add(param);
            type.ParameterList = paramList;

            /* initiate */
            var setOfReactions = type.Initiate(setOfReactants, null);

            Assert.AreEqual(1, setOfReactions.Count);

            //CreateFromSmiles("FH")
            var expected1 = builder.NewAtomContainer();
            expected1.Atoms.Add(builder.NewAtom("F"));
            expected1.Atoms.Add(builder.NewAtom("H"));
            expected1.AddBond(expected1.Atoms[0], expected1.Atoms[1], BondOrder.Single);
            AtomContainerManipulator.PercieveAtomTypesAndConfigureAtoms(expected1);
            CDK.LonePairElectronChecker.Saturate(expected1);
            var product1 = setOfReactions[0].Products[0];
            var queryAtom = QueryAtomContainerCreator.CreateSymbolAndChargeQueryContainer(expected1);
            Assert.IsTrue(new UniversalIsomorphismTester().IsIsomorph(product1, queryAtom));

            //CreateFromSmiles("[C+]")
            var expected2 = builder.NewAtomContainer();
            expected2.Atoms.Add(builder.NewAtom("C"));
            expected2.Atoms[0].FormalCharge = +1;
            expected2.Atoms.Add(builder.NewAtom("H"));
            expected2.Atoms.Add(builder.NewAtom("H"));
            expected2.Atoms.Add(builder.NewAtom("H"));
            expected2.AddBond(expected2.Atoms[0], expected2.Atoms[1], BondOrder.Single);
            expected2.AddBond(expected2.Atoms[0], expected2.Atoms[2], BondOrder.Single);
            expected2.AddBond(expected2.Atoms[0], expected2.Atoms[3], BondOrder.Single);
            AtomContainerManipulator.PercieveAtomTypesAndConfigureAtoms(expected2);
            var product2 = setOfReactions[0].Products[1];
            queryAtom = QueryAtomContainerCreator.CreateSymbolAndChargeQueryContainer(expected2);
            Assert.IsTrue(new UniversalIsomorphismTester().IsIsomorph(product2, queryAtom));
        }
    }
}
