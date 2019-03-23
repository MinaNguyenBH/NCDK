/* Copyright (C) 2004-2007  The Chemistry Development Kit (CDK) project
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

using System;

namespace NCDK.Isomorphisms.Matchers.SMARTS
{
    /// <summary>
    /// Abstract smarts atom.
    /// </summary>
    // @cdk.module smarts
    // @cdk.keyword SMARTS
    [Obsolete]
    public abstract class SMARTSAtom 
        : QueryAtom, IQueryAtom
    {
        protected SMARTSAtom()
            : base()
        { }

        /// <summary>
        /// Access the atom invariants for this atom. If the invariants have not been
        /// set an exception is thrown.
        /// </summary>
        /// <param name="atom">the atom to obtain the invariants of</param>
        /// <returns>the atom invariants for the atom</returns>
        /// <exception cref="NullReferenceException">thrown if the invariants were not set</exception>
        internal static SMARTSAtomInvariants Invariants(IAtom atom)
        {
            var inv = atom.GetProperty<SMARTSAtomInvariants>(SMARTSAtomInvariants.Key);
            if (inv == null)
                throw new NullReferenceException("Missing SMARTSAtomInvariants - please compute these values before matching.");
            return inv;
        }

        public override bool Matches(IAtom atom)
        {
            return false;
        }

        /// <summary>
        /// Check if the atom-based chirality of the target matches. This check is
        /// done post-matching and should only be checked on atoms which are know to
        /// have already been matched (<see cref="Matches(IAtom)"/>).
        ///
        /// Currently the only atom-based chirality allowed is tetrahedral stereo-chemistry. 
        /// </summary>
        /// <param name="target">the matched target (required to verify 'OR' conditions)</param>
        /// <param name="tParity">the parity (winding) of the target centre, 0=unspecified, 1=clockwise and -1=anticlockwise</param>
        /// <param name="permParity">permutation parity of the query neighbors (will be multiplied by the query parity)</param>
        public virtual bool ChiralityMatches(IAtom target, int tParity, int permParity)
        {
            return true; // no specification => chirality matches
        }
    }
}
