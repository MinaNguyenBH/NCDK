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
using System.Linq;

namespace NCDK.Reactions.Types
{
    /// <summary>
    /// <para>IReactionProcess which participate in movement resonance.
    /// This reaction could be represented as [A-]-B=C => A=B-[C-]. Due to
    /// the negative charge of the atom A, the double bond in position 2 is
    /// displaced.</para>
    /// </summary>
    /// <example><code>
    /// var setOfReactants = CDK.Builder.NewAtomContainerSet();
    /// setOfReactants.Add(new AtomContainer());
    /// var type = new RearrangementAnion1Reaction();
    /// Dictionary<string,Object> params = new Dictionary<string,Object>();
    /// params.Put("hasActiveCenter",bool.FALSE);;
    /// type.Parameters = params;
    /// var setOfReactions = type.Initiate(setOfReactants, null);
    /// </code>
    /// We have the possibility to localize the reactive center. Good method if you
    /// want to localize the reaction in a fixed point
    /// <code>
    /// atoms[0].IsReactiveCenter = true;
    /// </code>
    /// <para>Moreover you must put the parameter bool.TRUE</para>
    /// <para>If the reactive center is not localized then the reaction process will
    /// try to find automatically the possible reactive center.</para>
    /// </code>
    /// </example>
    /// <remarks>
    /// TestSuite that runs a test for the RearrangementAnionReactionTest.
    /// Generalized Reaction: [A-]-B=C => A=B-[C-].
    /// </remarks>
    // @author         Miguel Rojas
    // @cdk.created    2006-05-05
    // @cdk.module     test-reaction
    [TestClass()]
    public class RearrangementAnionReactionTest : ReactionProcessTest
    {
        private IChemObjectBuilder builder = CDK.Builder;

        public RearrangementAnionReactionTest()
        {
            SetReaction(typeof(RearrangementAnionReaction));
        }

        [TestMethod()]
        public void TestRearrangementAnionReaction()
        {
            var type = new RearrangementAnionReaction();
            Assert.IsNotNull(type);
        }

        /// <summary>
        /// Reaction: [C-]-C=C-C => C=C-[C-]-C
        /// Automatic search of the center active.
        /// </summary>
        [TestMethod()]
        public override void TestInitiate_IAtomContainerSet_IAtomContainerSet()
        {
            var type = new RearrangementAnionReaction();

            /* [C-]-C=C-C */

            var setOfReactants = GetExampleReactants();
            var molecule = setOfReactants[0];
            /* initiate */

            var paramList = new List<IParameterReaction>();
            var param = new SetReactionCenter();
            param.IsSetParameter = false;
            paramList.Add(param);
            type.ParameterList = paramList;
            var setOfReactions = type.Initiate(setOfReactants, null);

            Assert.AreEqual(1, setOfReactions.Count);
            Assert.AreEqual(1, setOfReactions[0].Products.Count);

            var product = setOfReactions[0].Products[0];
            Assert.AreEqual(-1, product.Atoms[2].FormalCharge.Value);
            Assert.AreEqual(0, product.GetConnectedLonePairs(product.Atoms[1]).Count());

            /* C=C-[C-]-C */
            var molecule2 = GetExpectedProducts()[0];

            var queryAtom = QueryAtomContainerCreator.CreateSymbolAndChargeQueryContainer(product);
            Assert.IsTrue(new UniversalIsomorphismTester().IsIsomorph(molecule2, queryAtom));
        }

        /// <summary>
        /// Reaction: [C-]-C=C-C => C=C-[C-]-C
        /// Manually put of the centre active.
        /// </summary>
        [TestMethod()]
        public void TestManuallyCentreActive()
        {
            var type = new RearrangementAnionReaction();

            var setOfReactants = GetExampleReactants();
            var molecule = setOfReactants[0];

            /* manually put the center active */
            molecule.Atoms[0].IsReactiveCenter = true;
            molecule.Bonds[0].IsReactiveCenter = true;
            molecule.Atoms[1].IsReactiveCenter = true;
            molecule.Bonds[1].IsReactiveCenter = true;
            molecule.Atoms[2].IsReactiveCenter = true;
            molecule.Bonds[2].IsReactiveCenter = true;
            molecule.Atoms[3].IsReactiveCenter = true;

            var paramList = new List<IParameterReaction>();
            var param = new SetReactionCenter();
            param.IsSetParameter = true;
            paramList.Add(param);
            type.ParameterList = paramList;

            /* initiate */

            var setOfReactions = type.Initiate(setOfReactants, null);

            Assert.AreEqual(1, setOfReactions.Count);
            Assert.AreEqual(1, setOfReactions[0].Products.Count);

            var product = setOfReactions[0].Products[0];

            /* C=C-[C-]-C */
            var molecule2 = GetExpectedProducts()[0];

            var queryAtom = QueryAtomContainerCreator.CreateSymbolAndChargeQueryContainer(product);
            Assert.IsTrue(new UniversalIsomorphismTester().IsIsomorph(molecule2, queryAtom));
        }

        [TestMethod()]
        public void TestCDKConstants_REACTIVE_CENTER()
        {
            var type = new RearrangementAnionReaction();

            var setOfReactants = GetExampleReactants();
            var molecule = setOfReactants[0];

            /* manually put the reactive center */
            molecule.Atoms[0].IsReactiveCenter = true;
            molecule.Atoms[1].IsReactiveCenter = true;
            molecule.Atoms[2].IsReactiveCenter = true;
            molecule.Bonds[0].IsReactiveCenter = true;
            molecule.Bonds[1].IsReactiveCenter = true;

            var paramList = new List<IParameterReaction>();
            var param = new SetReactionCenter();
            param.IsSetParameter = true;
            paramList.Add(param);
            type.ParameterList = paramList;

            /* initiate */
            var setOfReactions = type.Initiate(setOfReactants, null);

            var reactant = setOfReactions[0].Reactants[0];
            Assert.IsTrue(molecule.Atoms[0].IsReactiveCenter);
            Assert.IsTrue(reactant.Atoms[0].IsReactiveCenter);
            Assert.IsTrue(molecule.Atoms[1].IsReactiveCenter);
            Assert.IsTrue(reactant.Atoms[1].IsReactiveCenter);
            Assert.IsTrue(molecule.Atoms[2].IsReactiveCenter);
            Assert.IsTrue(reactant.Atoms[2].IsReactiveCenter);
            Assert.IsTrue(molecule.Bonds[0].IsReactiveCenter);
            Assert.IsTrue(reactant.Bonds[0].IsReactiveCenter);
            Assert.IsTrue(molecule.Bonds[1].IsReactiveCenter);
            Assert.IsTrue(reactant.Bonds[1].IsReactiveCenter);
        }

        [TestMethod()]
        public void TestMapping()
        {
            var type = new RearrangementAnionReaction();

            var setOfReactants = GetExampleReactants();
            var molecule = setOfReactants[0];
            molecule.LonePairs.Add(CDK.Builder.NewLonePair(molecule.Atoms[0]));

            /* automatic search of the center active */
            var paramList = new List<IParameterReaction>();
            var param = new SetReactionCenter();
            param.IsSetParameter = false;
            paramList.Add(param);
            type.ParameterList = paramList;
            /* initiate */

            var setOfReactions = type.Initiate(setOfReactants, null);

            var product = setOfReactions[0].Products[0];

            Assert.AreEqual(11, setOfReactions[0].Mappings.Count);
            var mappedProductA1 = (IAtom)ReactionManipulator.GetMappedChemObject(setOfReactions[0], molecule.Atoms[0]);
            Assert.AreEqual(mappedProductA1, product.Atoms[0]);
            mappedProductA1 = (IAtom)ReactionManipulator.GetMappedChemObject(setOfReactions[0], molecule.Atoms[1]);
            Assert.AreEqual(mappedProductA1, product.Atoms[1]);
            mappedProductA1 = (IAtom)ReactionManipulator.GetMappedChemObject(setOfReactions[0], molecule.Atoms[2]);
            Assert.AreEqual(mappedProductA1, product.Atoms[2]);
        }

        /// <summary>
        /// Reaction: [F+]=C1-[C-]-C=C-C=C1 => [F+]=C1-[C=]-C-[C-]-C=C1
        /// Automatic search of the center active.
        /// </summary>
        [TestMethod()]
        public void TestAutomaticSearchCentreActiveExample3()
        {
            var type = new RearrangementAnionReaction();

            /* [F+]=C1-[C-]-C=C-C=C1 */
            var molecule = builder.NewAtomContainer();
            molecule.Atoms.Add(builder.NewAtom("F"));
            molecule.Atoms[0].FormalCharge = 1;
            molecule.Atoms.Add(builder.NewAtom("C"));
            molecule.AddBond(molecule.Atoms[0], molecule.Atoms[1], BondOrder.Double);
            molecule.Atoms.Add(builder.NewAtom("C"));
            molecule.Atoms[2].FormalCharge = -1;
            molecule.LonePairs.Add(CDK.Builder.NewLonePair(molecule.Atoms[0]));
            molecule.AddBond(molecule.Atoms[1], molecule.Atoms[2], BondOrder.Single);
            molecule.Atoms.Add(builder.NewAtom("C"));
            molecule.AddBond(molecule.Atoms[2], molecule.Atoms[3], BondOrder.Single);
            molecule.Atoms.Add(builder.NewAtom("C"));
            molecule.AddBond(molecule.Atoms[3], molecule.Atoms[4], BondOrder.Double);
            molecule.Atoms.Add(builder.NewAtom("C"));
            molecule.AddBond(molecule.Atoms[4], molecule.Atoms[5], BondOrder.Single);
            molecule.Atoms.Add(builder.NewAtom("C"));
            molecule.AddBond(molecule.Atoms[5], molecule.Atoms[6], BondOrder.Double);
            molecule.AddBond(molecule.Atoms[6], molecule.Atoms[1], BondOrder.Single);

            AddExplicitHydrogens(molecule);

            AtomContainerManipulator.PercieveAtomTypesAndConfigureAtoms(molecule);
            CDK.LonePairElectronChecker.Saturate(molecule);

            var setOfReactants = CDK.Builder.NewAtomContainerSet();
            setOfReactants.Add(molecule);

            /* initiate */
            MakeSureAtomTypesAreRecognized(molecule);

            var paramList = new List<IParameterReaction>();
            var param = new SetReactionCenter();
            param.IsSetParameter = false;
            paramList.Add(param);
            type.ParameterList = paramList;
            var setOfReactions = type.Initiate(setOfReactants, null);

            Assert.AreEqual(2, setOfReactions.Count);
            Assert.AreEqual(1, setOfReactions[0].Products.Count);

            var product1 = setOfReactions[1].Products[0];

            /* [F+]=C1-[C=]-C-[C-]-C=C1 */
            var molecule2 = builder.NewAtomContainer();
            molecule2.Atoms.Add(builder.NewAtom("F"));
            molecule2.Atoms[0].FormalCharge = 1;
            molecule2.Atoms.Add(builder.NewAtom("C"));
            molecule2.AddBond(molecule2.Atoms[0], molecule2.Atoms[1], BondOrder.Double);
            molecule2.Atoms.Add(builder.NewAtom("C"));
            molecule2.AddBond(molecule2.Atoms[1], molecule2.Atoms[2], BondOrder.Single);
            molecule2.Atoms.Add(builder.NewAtom("C"));
            molecule2.AddBond(molecule2.Atoms[2], molecule2.Atoms[3], BondOrder.Double);
            molecule2.Atoms.Add(builder.NewAtom("C"));
            molecule2.Atoms[4].FormalCharge = -1;
            molecule2.AddBond(molecule2.Atoms[3], molecule2.Atoms[4], BondOrder.Single);
            molecule2.Atoms.Add(builder.NewAtom("C"));
            molecule2.AddBond(molecule2.Atoms[4], molecule2.Atoms[5], BondOrder.Single);
            molecule2.Atoms.Add(builder.NewAtom("C"));
            molecule2.AddBond(molecule2.Atoms[5], molecule2.Atoms[6], BondOrder.Double);
            molecule2.AddBond(molecule2.Atoms[6], molecule2.Atoms[1], BondOrder.Single);

            AddExplicitHydrogens(molecule2);
            AtomContainerManipulator.PercieveAtomTypesAndConfigureAtoms(molecule2);
            CDK.LonePairElectronChecker.Saturate(molecule2);
            MakeSureAtomTypesAreRecognized(molecule2);
            var queryAtom = QueryAtomContainerCreator.CreateSymbolAndChargeQueryContainer(product1);
            Assert.IsTrue(new UniversalIsomorphismTester().IsIsomorph(molecule2, queryAtom));

            //////////////////////////////////////////////////

            var product2 = setOfReactions[0].Products[0];

            /* F=c1ccccc1 */
            var molecule3 = builder.NewAtomContainer();
            molecule3.Atoms.Add(builder.NewAtom("F"));
            molecule3.Atoms.Add(builder.NewAtom("C"));
            molecule3.AddBond(molecule3.Atoms[0], molecule3.Atoms[1], BondOrder.Single);
            molecule3.Atoms.Add(builder.NewAtom("C"));
            molecule3.AddBond(molecule3.Atoms[1], molecule3.Atoms[2], BondOrder.Double);
            molecule3.Atoms.Add(builder.NewAtom("C"));
            molecule3.AddBond(molecule3.Atoms[2], molecule3.Atoms[3], BondOrder.Single);
            molecule3.Atoms.Add(builder.NewAtom("C"));
            molecule3.AddBond(molecule3.Atoms[3], molecule3.Atoms[4], BondOrder.Double);
            molecule3.Atoms.Add(builder.NewAtom("C"));
            molecule3.AddBond(molecule3.Atoms[4], molecule3.Atoms[5], BondOrder.Single);
            molecule3.Atoms.Add(builder.NewAtom("C"));
            molecule3.AddBond(molecule3.Atoms[5], molecule3.Atoms[6], BondOrder.Double);
            molecule3.AddBond(molecule3.Atoms[6], molecule3.Atoms[1], BondOrder.Single);

            AddExplicitHydrogens(molecule3);
            AtomContainerManipulator.PercieveAtomTypesAndConfigureAtoms(molecule3);
            CDK.LonePairElectronChecker.Saturate(molecule3);
            MakeSureAtomTypesAreRecognized(molecule3);
            queryAtom = QueryAtomContainerCreator.CreateSymbolAndChargeQueryContainer(product2);
            Assert.IsTrue(new UniversalIsomorphismTester().IsIsomorph(molecule3, queryAtom));

            Assert.AreEqual(12, setOfReactions[0].Mappings.Count);
        }

        /// <summary>
        /// Test to recognize if this IAtomContainer_1 matches correctly into the CDKAtomTypes.
        /// </summary>
        [TestMethod()]
        public void TestAtomTypesAtomContainer1()
        {
            var moleculeTest = GetExampleReactants()[0];
            AtomContainerManipulator.PercieveAtomTypesAndConfigureAtoms(moleculeTest);
            MakeSureAtomTypesAreRecognized(moleculeTest);
        }

        /// <summary>
        /// Test to recognize if this IAtomContainer_2 matches correctly into the CDKAtomTypes.
        /// </summary>
        [TestMethod()]
        public void TestAtomTypesAtomContainer2()
        {
            var moleculeTest = GetExpectedProducts()[0];
            AtomContainerManipulator.PercieveAtomTypesAndConfigureAtoms(moleculeTest);
            MakeSureAtomTypesAreRecognized(moleculeTest);
        }

        /// <summary>
        /// Get the molecule 1: [C-]-C=C-C
        /// </summary>
        private IChemObjectSet<IAtomContainer> GetExampleReactants()
        {
            var setOfReactants = CDK.Builder.NewAtomContainerSet();

            var molecule = builder.NewAtomContainer();
            molecule.Atoms.Add(builder.NewAtom("C"));
            molecule.Atoms[0].FormalCharge = -1;
            molecule.LonePairs.Add(CDK.Builder.NewLonePair(molecule.Atoms[0]));
            molecule.Atoms.Add(builder.NewAtom("C"));
            molecule.AddBond(molecule.Atoms[0], molecule.Atoms[1], BondOrder.Single);
            molecule.Atoms.Add(builder.NewAtom("C"));
            molecule.AddBond(molecule.Atoms[1], molecule.Atoms[2], BondOrder.Double);
            molecule.Atoms.Add(builder.NewAtom("C"));
            molecule.AddBond(molecule.Atoms[2], molecule.Atoms[3], BondOrder.Single);

            try
            {
                AddExplicitHydrogens(molecule);
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e.StackTrace);
            }
            setOfReactants.Add(molecule);
            return setOfReactants;
        }

        /// <summary>
        /// Get the expected set of molecules.
        /// </summary>
        private IChemObjectSet<IAtomContainer> GetExpectedProducts()
        {
            var setOfProducts = builder.NewAtomContainerSet();

            var molecule = builder.NewAtomContainer();
            molecule.Atoms.Add(builder.NewAtom("C"));
            molecule.Atoms.Add(builder.NewAtom("C"));
            molecule.AddBond(molecule.Atoms[0], molecule.Atoms[1], BondOrder.Double);
            molecule.Atoms.Add(builder.NewAtom("C"));
            molecule.Atoms[2].FormalCharge = -1;
            molecule.LonePairs.Add(CDK.Builder.NewLonePair(molecule.Atoms[0]));
            molecule.AddBond(molecule.Atoms[1], molecule.Atoms[2], BondOrder.Single);
            molecule.Atoms.Add(builder.NewAtom("C"));
            molecule.AddBond(molecule.Atoms[2], molecule.Atoms[3], BondOrder.Single);

            try
            {
                AddExplicitHydrogens(molecule);
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e.StackTrace);
            }

            setOfProducts.Add(molecule);
            return setOfProducts;
        }

        /// <summary>
        /// Test to recognize if a IAtomContainer matcher correctly identifies the CDKAtomTypes.
        /// </summary>
        /// <param name="molecule">The IAtomContainer to analyze</param>
        private void MakeSureAtomTypesAreRecognized(IAtomContainer molecule)
        {
            var matcher = CDK.AtomTypeMatcher;
            foreach (var nextAtom in molecule.Atoms)
            {
                Assert.IsNotNull(matcher.FindMatchingAtomType(molecule, nextAtom), "Missing atom type for: " + nextAtom);
            }
        }
    }
}
