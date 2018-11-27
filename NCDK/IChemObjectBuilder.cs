/* Copyright (C) 2009-2010  Egon Willighagen <egonw@users.sf.net>
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

using System.Collections.Generic;
using NCDK.Numerics;

namespace NCDK
{
    /// <summary>
    /// A helper class to instantiate a <see cref="ICDKObject"/> instance for a specific implementation.
    /// </summary>
    // @author        egonw
    // @cdk.module    interfaces
    // @cdk.githash
    public interface IChemObjectBuilder
    {
        IAdductFormula NewAdductFormula();
        IAdductFormula NewAdductFormula(IMolecularFormula formula);
        IAminoAcid NewAminoAcid();
        IAtom NewAtom();
        IAtom NewAtom(IElement element);
        IAtom NewAtom(int elem);
        IAtom NewAtom(int elem, int hcnt);
        IAtom NewAtom(int elem, int hcnt, int fchg);
        IAtom NewAtom(string elementSymbol);
        IAtom NewAtom(string elementSymbol, Vector2 point2d);
        IAtom NewAtom(string elementSymbol, Vector3 point3d);
        IChemObjectSet<T> NewChemObjectSet<T>() where T : IAtomContainer;
        IAtomContainer NewAtomContainer();
        IAtomContainer NewAtomContainer(IAtomContainer container);
        IAtomContainer NewAtomContainer(IEnumerable<IAtom> atoms, IEnumerable<IBond> bonds);
        IAtomContainerSet NewAtomContainerSet();
        IAtomType NewAtomType(IElement element);
        IAtomType NewAtomType(string elementSymbol);
        IAtomType NewAtomType(string identifier, string elementSymbol);
        IBioPolymer NewBioPolymer();
        IBond NewBond();
        IBond NewBond(IAtom atom1, IAtom atom2);
        IBond NewBond(IAtom atom1, IAtom atom2, BondOrder order);
        IBond NewBond(IEnumerable<IAtom> atoms);
        IBond NewBond(IEnumerable<IAtom> atoms, BondOrder order);
        IBond NewBond(IAtom atom1, IAtom atom2, BondOrder order, BondStereo stereo);
        IBond NewBond(IEnumerable<IAtom> atoms, BondOrder order, BondStereo stereo);
        IChemFile NewChemFile();
        IChemModel NewChemModel();
        IChemObject NewChemObject();
        IChemObject NewChemObject(IChemObject chemObject);
        IChemSequence NewChemSequence();
        ICrystal NewCrystal();
        ICrystal NewCrystal(IAtomContainer container);
        IElectronContainer NewElectronContainer();
        IElement NewElement();
        IElement NewElement(IElement element);
        IElement NewElement(string symbol);
        IElement NewElement(string symbol, int atomicNumber);
        IFragmentAtom NewFragmentAtom();
        ILonePair NewLonePair();
        ILonePair NewLonePair(IAtom atom);
        IIsotope NewIsotope(string elementSymbol);
        IIsotope NewIsotope(int atomicNumber, string elementSymbol, int massNumber, double exactMass, double abundance);
        IIsotope NewIsotope(int atomicNumber, string elementSymbol, double exactMass, double abundance);
        IIsotope NewIsotope(string elementSymbol, int massNumber);
        IIsotope NewIsotope(IElement element);
        IMapping NewMapping(IChemObject objectOne, IChemObject objectTwo);
        IMolecularFormula NewMolecularFormula();
        IMolecularFormulaSet NewMolecularFormulaSet();
        IMolecularFormulaSet NewMolecularFormulaSet(IMolecularFormula formula);
        IMonomer NewMonomer();
        IPseudoAtom NewPseudoAtom();
        IPseudoAtom NewPseudoAtom(string label);
        IPseudoAtom NewPseudoAtom(IElement element);
        IPseudoAtom NewPseudoAtom(string label, Vector2 point2d);
        IPseudoAtom NewPseudoAtom(string label, Vector3 point3d);
        IReaction NewReaction();
        IReactionSet NewReactionSet();
        IReactionScheme NewReactionScheme();
        IPDBAtom NewPDBAtom(IElement element);
        IPDBAtom NewPDBAtom(string symbol);
        IPDBAtom NewPDBAtom(string symbol, Vector3 coordinate);
        IPDBMonomer NewPDBMonomer();
        IPDBPolymer NewPDBPolymer();
        IPDBStructure NewPDBStructure();
        IPolymer NewPolymer();
        IRing NewRing();
        IRing NewRing(int ringSize, string elementSymbol);
        IRing NewRing(IAtomContainer atomContainer);
        IRing NewRing(IEnumerable<IAtom> atoms, IEnumerable<IBond> bonds);
        IRingSet NewRingSet();
        ISubstance NewSubstance();
        ISingleElectron NewSingleElectron();
        ISingleElectron NewSingleElectron(IAtom atom);
        IStrand NewStrand();
        ITetrahedralChirality NewTetrahedralChirality(IAtom chiralAtom, IReadOnlyList<IAtom> ligandAtoms, TetrahedralStereo chirality);
    }
}
