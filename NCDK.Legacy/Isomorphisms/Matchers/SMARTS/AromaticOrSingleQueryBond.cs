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
    /// This matches an aromatic or a single bond, used when no bond is specified between an atom.
    /// </summary>
    // @cdk.module  smarts
    // @cdk.githash
    // @cdk.keyword SMARTS
    [Obsolete]
    public class AromaticOrSingleQueryBond : SMARTSBond
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        public AromaticOrSingleQueryBond()
            : base()
        {
            IsAromatic = true;
        }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public AromaticOrSingleQueryBond(IQueryAtom atom1, IQueryAtom atom2, BondOrder order)
           : base(atom1, atom2, order)
        {
            IsAromatic = true;
        }

        public override bool Matches(IBond bond)
        {
            return bond.IsAromatic || bond.Order == BondOrder.Single;
        }

        public override string ToString()
        {
            return nameof(AromaticOrSingleQueryBond) + "()";
        }
    }
}
