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
    /// TestSuite that runs a test for the ElectronImpactNBEReactionTest.
    /// </summary>
    // @cdk.module test-reaction
    [TestClass()]
    public class ElectronImpactNBEReactionTest : ReactionProcessTest
    {
        private IChemObjectBuilder builder = CDK.Builder;

        public ElectronImpactNBEReactionTest()
        {
            SetReaction(typeof(ElectronImpactNBEReaction));
        }

        [TestMethod()]
        public void TestElectronImpactNBEReaction()
        {
            var type = new ElectronImpactNBEReaction();
            Assert.IsNotNull(type);
        }

        /// <summary>
        ///  A unit test with the compound 2_5_Hexen_3_one.
        /// </summary>
        [TestMethod()]
        public override void TestInitiate_IAtomContainerSet_IAtomContainerSet()
        {
            /* Ionize(>C=O): C=CCC(=O)CC -> C=CCC(=O*)CC , set the reactive center */

            var reactant = builder.NewAtomContainer();//CreateFromSmiles("C=CCC(=O)CC")
            reactant.Atoms.Add(builder.NewAtom("C"));
            reactant.Atoms.Add(builder.NewAtom("C"));
            reactant.Atoms.Add(builder.NewAtom("C"));
            reactant.Atoms.Add(builder.NewAtom("C"));
            reactant.Atoms.Add(builder.NewAtom("O"));
            reactant.Atoms.Add(builder.NewAtom("C"));
            reactant.Atoms.Add(builder.NewAtom("C"));
            reactant.AddBond(reactant.Atoms[0], reactant.Atoms[1], BondOrder.Double);
            reactant.AddBond(reactant.Atoms[1], reactant.Atoms[2], BondOrder.Single);
            reactant.AddBond(reactant.Atoms[2], reactant.Atoms[3], BondOrder.Single);
            reactant.AddBond(reactant.Atoms[3], reactant.Atoms[4], BondOrder.Double);
            reactant.AddBond(reactant.Atoms[3], reactant.Atoms[5], BondOrder.Single);
            reactant.AddBond(reactant.Atoms[5], reactant.Atoms[6], BondOrder.Single);
            AddExplicitHydrogens(reactant);
            AtomContainerManipulator.PercieveAtomTypesAndConfigureAtoms(reactant);
            CDK.LonePairElectronChecker.Saturate(reactant);

            foreach (var atom in reactant.Atoms)
            {
                if (reactant.GetConnectedLonePairs(atom).Count() > 0)
                {
                    atom.IsReactiveCenter = true;
                }
            }

            var setOfReactants = CDK.Builder.NewAtomContainerSet();
            setOfReactants.Add(reactant);

            /* initiate */
            MakeSureAtomTypesAreRecognized(reactant);

            var type = new ElectronImpactNBEReaction();
            var paramList = new List<IParameterReaction>();
            var param = new SetReactionCenter
            {
                IsSetParameter = true
            };
            paramList.Add(param);
            type.ParameterList = paramList;

            var setOfReactions = type.Initiate(setOfReactants, null);

            Assert.AreEqual(1, setOfReactions.Count);
            Assert.AreEqual(1, setOfReactions[0].Products.Count);

            var molecule = setOfReactions[0].Products[0];
            Assert.AreEqual(1, molecule.Atoms[4].FormalCharge.Value);
            Assert.AreEqual(1, molecule.GetConnectedSingleElectrons(molecule.Atoms[4]).Count());

            Assert.IsTrue(setOfReactions[0].Mappings.Any());
        }

        /// <summary>
        ///  A unit test with the compound 2_5_Hexen_3_one.
        /// </summary>
        [TestMethod()]
        public void TestAutomatic_Set_Active_Atom()
        {
            // Ionize(>C=O): C=CCC(=O)CC -> C=CCC(=O*)CC, without setting the
            // reactive center
            var reactant = builder.NewAtomContainer();//CreateFromSmiles("C=CCC(=O)CC")
            reactant.Atoms.Add(builder.NewAtom("C"));
            reactant.Atoms.Add(builder.NewAtom("C"));
            reactant.Atoms.Add(builder.NewAtom("C"));
            reactant.Atoms.Add(builder.NewAtom("C"));
            reactant.Atoms.Add(builder.NewAtom("O"));
            reactant.Atoms.Add(builder.NewAtom("C"));
            reactant.Atoms.Add(builder.NewAtom("C"));
            reactant.AddBond(reactant.Atoms[0], reactant.Atoms[1], BondOrder.Double);
            reactant.AddBond(reactant.Atoms[1], reactant.Atoms[2], BondOrder.Single);
            reactant.AddBond(reactant.Atoms[2], reactant.Atoms[3], BondOrder.Single);
            reactant.AddBond(reactant.Atoms[3], reactant.Atoms[4], BondOrder.Double);
            reactant.AddBond(reactant.Atoms[3], reactant.Atoms[5], BondOrder.Single);
            reactant.AddBond(reactant.Atoms[5], reactant.Atoms[6], BondOrder.Single);
            AddExplicitHydrogens(reactant);
            AtomContainerManipulator.PercieveAtomTypesAndConfigureAtoms(reactant);
            CDK.LonePairElectronChecker.Saturate(reactant);

            var setOfReactants = CDK.Builder.NewAtomContainerSet();
            setOfReactants.Add(reactant);

            /* initiate */
            MakeSureAtomTypesAreRecognized(reactant);

            var type = new ElectronImpactNBEReaction();
            var setOfReactions = type.Initiate(setOfReactants, null);

            Assert.AreEqual(1, setOfReactions.Count);
            Assert.AreEqual(1, setOfReactions[0].Products.Count);

            var molecule = setOfReactions[0].Products[0];
            Assert.AreEqual(1, molecule.Atoms[4].FormalCharge.Value);
            Assert.AreEqual(1, molecule.GetConnectedSingleElectrons(molecule.Atoms[4]).Count());
        }

        /// <summary>
        /// A unit test suite. Reaction: methanamine.
        /// C-!N! => C[N*+]
        /// </summary>
        // @cdk.inchi  InChI=1/CH5N/c1-2/h2H2,1H3
        [TestMethod()]
        public void TestNsp3SingleB()
        {
            //CreateFromSmiles("CN")
            var molecule = builder.NewAtomContainer();
            molecule.Atoms.Add(builder.NewAtom("C"));
            molecule.Atoms.Add(builder.NewAtom("N"));
            molecule.AddBond(molecule.Atoms[0], molecule.Atoms[1], BondOrder.Single);
            molecule.Atoms.Add(builder.NewAtom("H"));
            molecule.Atoms.Add(builder.NewAtom("H"));
            molecule.Atoms.Add(builder.NewAtom("H"));
            molecule.Atoms.Add(builder.NewAtom("H"));
            molecule.Atoms.Add(builder.NewAtom("H"));
            molecule.AddBond(molecule.Atoms[0], molecule.Atoms[2], BondOrder.Single);
            molecule.AddBond(molecule.Atoms[0], molecule.Atoms[3], BondOrder.Single);
            molecule.AddBond(molecule.Atoms[0], molecule.Atoms[4], BondOrder.Single);
            molecule.AddBond(molecule.Atoms[1], molecule.Atoms[5], BondOrder.Single);
            molecule.AddBond(molecule.Atoms[1], molecule.Atoms[6], BondOrder.Single);
            AtomContainerManipulator.PercieveAtomTypesAndConfigureAtoms(molecule);
            CDK.LonePairElectronChecker.Saturate(molecule);

            molecule.Atoms[1].IsReactiveCenter = true;

            var setOfReactants = CDK.Builder.NewAtomContainerSet();
            setOfReactants.Add(molecule);

            var type = new ElectronImpactNBEReaction();
            var paramList = new List<IParameterReaction>();
            var param = new SetReactionCenter
            {
                IsSetParameter = true
            };
            paramList.Add(param);
            type.ParameterList = paramList;

            /* initiate */
            var setOfReactions = type.Initiate(setOfReactants, null);

            Assert.AreEqual(1, setOfReactions.Count);

            // expected products

            //CreateFromSmiles("C[N*+]")
            var expected1 = builder.NewAtomContainer();
            expected1.Atoms.Add(builder.NewAtom("C"));
            expected1.Atoms.Add(builder.NewAtom("N"));
            expected1.Atoms[1].FormalCharge = +1;
            expected1.SingleElectrons.Add(builder.NewSingleElectron(expected1.Atoms[1]));
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
        }

        /// <summary>
        /// A unit test suite. Reaction: Methanimine.
        /// C=!N! => C=[N*+]
        /// </summary>
        // @cdk.inchi  InChI=1/CH3N/c1-2/h2H,1H2
        [TestMethod()]
        public void TestNsp2SingleB()
        {
            //CreateFromSmiles("C=N")
            var molecule = builder.NewAtomContainer();
            molecule.Atoms.Add(builder.NewAtom("N"));
            molecule.Atoms.Add(builder.NewAtom("C"));
            molecule.AddBond(molecule.Atoms[0], molecule.Atoms[1], BondOrder.Double);
            molecule.Atoms.Add(builder.NewAtom("H"));
            molecule.Atoms.Add(builder.NewAtom("H"));
            molecule.Atoms.Add(builder.NewAtom("H"));
            molecule.AddBond(molecule.Atoms[0], molecule.Atoms[2], BondOrder.Single);
            molecule.AddBond(molecule.Atoms[1], molecule.Atoms[3], BondOrder.Single);
            molecule.AddBond(molecule.Atoms[1], molecule.Atoms[4], BondOrder.Single);
            AtomContainerManipulator.PercieveAtomTypesAndConfigureAtoms(molecule);
            CDK.LonePairElectronChecker.Saturate(molecule);

            molecule.Atoms[0].IsReactiveCenter = true;

            var setOfReactants = CDK.Builder.NewAtomContainerSet();
            setOfReactants.Add(molecule);

            var type = new ElectronImpactNBEReaction();

            var paramList = new List<IParameterReaction>();
            var param = new SetReactionCenter
            {
                IsSetParameter = true
            };
            paramList.Add(param);
            type.ParameterList = paramList;

            /* initiate */
            var setOfReactions = type.Initiate(setOfReactants, null);

            Assert.AreEqual(1, setOfReactions.Count);

            // expected products

            //CreateFromSmiles("[N*+]=C")
            var expected1 = builder.NewAtomContainer();
            expected1.Atoms.Add(builder.NewAtom("N"));
            expected1.Atoms[0].FormalCharge = 1;
            expected1.SingleElectrons.Add(builder.NewSingleElectron(expected1.Atoms[0]));
            expected1.Atoms.Add(builder.NewAtom("C"));
            expected1.AddBond(expected1.Atoms[0], expected1.Atoms[1], BondOrder.Double);
            expected1.Atoms.Add(builder.NewAtom("H"));
            expected1.Atoms.Add(builder.NewAtom("H"));
            expected1.Atoms.Add(builder.NewAtom("H"));
            expected1.AddBond(expected1.Atoms[0], expected1.Atoms[2], BondOrder.Single);
            expected1.AddBond(expected1.Atoms[1], expected1.Atoms[3], BondOrder.Single);
            expected1.AddBond(expected1.Atoms[1], expected1.Atoms[4], BondOrder.Single);
            AtomContainerManipulator.PercieveAtomTypesAndConfigureAtoms(expected1);
            CDK.LonePairElectronChecker.Saturate(expected1);

            var product1 = setOfReactions[0].Products[0];
            var queryAtom = QueryAtomContainerCreator.CreateSymbolAndChargeQueryContainer(expected1);
            Assert.IsTrue(new UniversalIsomorphismTester().IsIsomorph(product1, queryAtom));
        }

        /// <summary>
        /// A unit test suite. Reaction: fluoromethane.
        /// F!-!C => [F*+]C
        /// </summary>
        // @cdk.inchi InChI=1/CH3F/c1-2/h1H3
        [TestMethod()]
        public void TestFspSingleB()
        {
            //CreateFromSmiles("FC")
            var molecule = builder.NewAtomContainer();
            molecule.Atoms.Add(builder.NewAtom("F"));
            molecule.Atoms.Add(builder.NewAtom("C"));
            molecule.AddBond(molecule.Atoms[0], molecule.Atoms[1], BondOrder.Single);
            molecule.Atoms.Add(builder.NewAtom("H"));
            molecule.Atoms.Add(builder.NewAtom("H"));
            molecule.Atoms.Add(builder.NewAtom("H"));
            molecule.AddBond(molecule.Atoms[1], molecule.Atoms[2], BondOrder.Single);
            molecule.AddBond(molecule.Atoms[1], molecule.Atoms[3], BondOrder.Single);
            molecule.AddBond(molecule.Atoms[1], molecule.Atoms[4], BondOrder.Single);
            AtomContainerManipulator.PercieveAtomTypesAndConfigureAtoms(molecule);
            CDK.LonePairElectronChecker.Saturate(molecule);

            molecule.Atoms[0].IsReactiveCenter = true;

            var setOfReactants = CDK.Builder.NewAtomContainerSet();
            setOfReactants.Add(molecule);

            var type = new ElectronImpactNBEReaction();

            var paramList = new List<IParameterReaction>();
            var param = new SetReactionCenter
            {
                IsSetParameter = true
            };
            paramList.Add(param);
            type.ParameterList = paramList;

            /* initiate */
            var setOfReactions = type.Initiate(setOfReactants, null);

            Assert.AreEqual(1, setOfReactions.Count);

            // expected products

            //CreateFromSmiles("[F*+]C")
            var expected1 = builder.NewAtomContainer();
            expected1.Atoms.Add(builder.NewAtom("F"));
            expected1.Atoms[0].FormalCharge = 1;
            expected1.SingleElectrons.Add(builder.NewSingleElectron(expected1.Atoms[0]));
            expected1.Atoms.Add(builder.NewAtom("C"));
            expected1.AddBond(expected1.Atoms[0], expected1.Atoms[1], BondOrder.Single);
            expected1.Atoms.Add(builder.NewAtom("H"));
            expected1.Atoms.Add(builder.NewAtom("H"));
            expected1.Atoms.Add(builder.NewAtom("H"));
            expected1.AddBond(expected1.Atoms[1], expected1.Atoms[2], BondOrder.Single);
            expected1.AddBond(expected1.Atoms[1], expected1.Atoms[3], BondOrder.Single);
            expected1.AddBond(expected1.Atoms[1], expected1.Atoms[4], BondOrder.Single);
            AtomContainerManipulator.PercieveAtomTypesAndConfigureAtoms(expected1);
            CDK.LonePairElectronChecker.Saturate(expected1);

            var product1 = setOfReactions[0].Products[0];
            var queryAtom = QueryAtomContainerCreator.CreateSymbolAndChargeQueryContainer(expected1);
            Assert.IsTrue(new UniversalIsomorphismTester().IsIsomorph(product1, queryAtom));
        }

        /// <summary>
        /// A unit test suite. Reaction: C=O => C=[O*+]
        /// Manually put of the reactive center.
        /// </summary>
        // @cdk.inchi InChI=1/CH2O/c1-2/h1H2
        [TestMethod()]
        public void TestCDKConstants_REACTIVE_CENTER()
        {
            var type = new ElectronImpactNBEReaction();

            var setOfReactants = GetExampleReactants();
            var molecule = setOfReactants[0];
            /* manually put the reactive center */
            molecule.Atoms[1].IsReactiveCenter = true;

            var paramList = new List<IParameterReaction>();
            var param = new SetReactionCenter
            {
                IsSetParameter = true
            };
            paramList.Add(param);
            type.ParameterList = paramList;

            /* initiate */
            var setOfReactions = type.Initiate(setOfReactants, null);

            Assert.AreEqual(1, setOfReactions.Count);
            Assert.AreEqual(1, setOfReactions[0].Products.Count);

            var reactant = setOfReactions[0].Reactants[0];
            Assert.IsTrue(molecule.Atoms[1].IsReactiveCenter);
            Assert.IsTrue(reactant.Atoms[1].IsReactiveCenter);
        }

        /// <summary>
        /// A unit test suite. Reaction: C=O => C=[O*+]
        /// Manually put of the reactive center.
        /// </summary>
        // @cdk.inchi InChI=1/CH2O/c1-2/h1H2
        [TestMethod()]
        public void TestMapping()
        {
            var type = new ElectronImpactNBEReaction();
            var setOfReactants = GetExampleReactants();
            var molecule = setOfReactants[0];

            /* automatic search of the center active */
            var paramList = new List<IParameterReaction>();
            var param = new SetReactionCenter
            {
                IsSetParameter = false
            };
            paramList.Add(param);
            type.ParameterList = paramList;

            var setOfReactions = type.Initiate(setOfReactants, null);

            var product = setOfReactions[0].Products[0];

            Assert.AreEqual(4, setOfReactions[0].Mappings.Count);
            var mappedProductA1 = (IAtom)ReactionManipulator.GetMappedChemObject(setOfReactions[0],
                    molecule.Atoms[1]);
            Assert.AreEqual(mappedProductA1, product.Atoms[1]);
        }

        /// <summary>
        /// Test to recognize if a IAtomContainer matcher correctly the CDKAtomTypes.
        /// </summary>
        /// <param name="molecule">The IAtomContainer to analyze</param>
        private void MakeSureAtomTypesAreRecognized(IAtomContainer molecule)
        {
            var matcher = CDK.AtomTypeMatcher;
            foreach (var nextAtom in molecule.Atoms)
            {
                Assert.IsNotNull(matcher.FindMatchingAtomType(molecule, nextAtom), $"Missing atom type for: {nextAtom}");
            }
        }

        /// <summary>
        /// Get the example set of molecules.
        /// </summary>
        private IChemObjectSet<IAtomContainer> GetExampleReactants()
        {
            var setOfReactants = CDK.Builder.NewAtomContainerSet();
            var molecule = builder.NewAtomContainer();//CreateFromSmiles("C=O")
            molecule.Atoms.Add(builder.NewAtom("C"));
            molecule.Atoms.Add(builder.NewAtom("O"));
            molecule.AddBond(molecule.Atoms[0], molecule.Atoms[1], BondOrder.Double);
            molecule.Atoms.Add(builder.NewAtom("H"));
            molecule.Atoms.Add(builder.NewAtom("H"));
            molecule.AddBond(molecule.Atoms[0], molecule.Atoms[2], BondOrder.Single);
            molecule.AddBond(molecule.Atoms[0], molecule.Atoms[3], BondOrder.Single);

            try
            {
                AtomContainerManipulator.PercieveAtomTypesAndConfigureAtoms(molecule);
                CDK.LonePairElectronChecker.Saturate(molecule);
                MakeSureAtomTypesAreRecognized(molecule);
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
        /// TODO:reaction. Set the products
        /// </summary>
        private IChemObjectSet<IAtomContainer> GetExpectedProducts()
        {
            var setOfProducts = builder.NewAtomContainerSet();

            setOfProducts.Add(null);
            return setOfProducts;
        }
    }
}
