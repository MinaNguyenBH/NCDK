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
    /// This matches a bond of any order.
    /// </summary>
    // @cdk.module  smarts
    // @cdk.keyword SMARTS
    [Obsolete]
    public class AnyOrderQueryBond : SMARTSBond
    {
        public AnyOrderQueryBond()
            : base()
        {
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="atom1"></param>
        /// <param name="atom2"></param>
        /// <param name="order"></param>
        public AnyOrderQueryBond(IQueryAtom atom1, IQueryAtom atom2, BondOrder order)
            : base(atom1, atom2, order)
        {
        }

        public override bool Matches(IBond bond)
        {
            return bond != null; // any bond order is fine
        }

        public override string ToString()
        {
            return nameof(AnyOrderQueryBond) + "()";
        }
    }
}
