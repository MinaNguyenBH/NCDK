﻿/*
 * Copyright (c) 2016 John May <jwmay@users.sf.net>
 *
 * Contact: cdk-devel@lists.sourceforge.net
 *
 * This program is free software; you can redistribute it and/or modify it
 * under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation; either version 2.1 of the License, or (at
 * your option) any later version. All we ask is that proper credit is given
 * for our work, which includes - but is not limited to - adding the above
 * copyright notice to the beginning of your source code files, and to any
 * copyright notice that you may distribute with programs based on this work.
 *
 * This program is distributed in the hope that it will be useful, but WITHOUT
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
 * FITNESS FOR A PARTICULAR PURPOSE.  See the GNU Lesser General Public
 * License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 U
 */

using System;
using System.Text;

namespace NCDK.Isomorphisms.Matchers.SMARTS
{
    [Obsolete]
    [Flags]
    public enum ReactionRoles
    {
        Reactant = 0x1,
        Agent = 0x2,
        Product = 0x4,
        Any = Reactant | Agent | Product,
    }

    /// <summary>
    /// Matches atoms with a particular role in a reaction.
    /// </summary>
    [Obsolete]
    public class ReactionRoleQueryAtom : SMARTSAtom
    {
        private readonly ReactionRoles role;

        public readonly static ReactionRoleQueryAtom RoleReactant = new ReactionRoleQueryAtom(ReactionRoles.Reactant);
        public readonly static ReactionRoleQueryAtom RoleAgent = new ReactionRoleQueryAtom(ReactionRoles.Agent);
        public readonly static ReactionRoleQueryAtom RoleProduct = new ReactionRoleQueryAtom(ReactionRoles.Product);

        public ReactionRoleQueryAtom(ReactionRoles role)
            : base()
        {
            this.role = role;
        }

        public override bool Matches(IAtom atom)
        {
            ReactionRole? atomRole = atom.GetProperty<ReactionRole?>(CDKPropertyName.ReactionRole);
            if (atomRole == null)
                return this.role == ReactionRoles.Any;
            switch (atomRole.Value)
            {
                case ReactionRole.Reactant:
                    return (this.role & ReactionRoles.Reactant) != 0;
                case ReactionRole.Agent:
                    return (this.role & ReactionRoles.Agent) != 0;
                case ReactionRole.Product:
                    return (this.role & ReactionRoles.Product) != 0;
                default:
                    return false;
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            if ((role & ReactionRoles.Reactant) != 0)
                sb.Append("Reactant");
            if ((role & ReactionRoles.Agent) != 0)
                sb.Append("Agent");
            if ((role & ReactionRoles.Product) != 0)
                sb.Append("Product");
            return "ReactionRole(" + sb.ToString() + ")";
        }
    }
}
