/* Copyright (C) 2006-2010  Syed Asad Rahman <asad@ebi.ac.uk>
 *
 * Contact: cdk-devel@lists.sourceforge.net
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public License
 * as published by the Free Software Foundation; either version 2.1
 * of the License, or (at your option) any later version.
 * All we ask is that proper credit is given for our work, which includes
 * - but is not limited to - adding the above copyright notice to the beginning
 * of your source code files, and to any copyright notice that you may distribute
 * with programs based on this work.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
 *
 * MX Cheminformatics Tools for Java
 *
 * Copyright (c) 2007-2009 Metamolecular, LLC
 *
 * http://metamolecular.com
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using NCDK.SMSD.Algorithms.VFLib.Builder;
using System;
using System.Collections.Generic;

namespace NCDK.SMSD.Algorithms.VFLib
{
    /// <summary>
    /// Interface for the mappings (mapped objects).
    /// </summary>
    // @cdk.module smsd
    // @author Syed Asad Rahman <asad@ebi.ac.uk>
    [Obsolete("SMSD has been deprecated from the CDK with a newer, more recent version of SMSD is available at http://github.com/asad/smsd . ")]
    public interface IMapper
    {
        /// <summary>
        /// checks if a map exits for a molecule.
        /// </summary>
        /// <param name="molecule">molecule</param>
        /// <returns>true/false.</returns>
        bool HasMap(IAtomContainer molecule);

        /// <summary>
        /// Returns solution map count.
        /// </summary>
        /// <param name="target">target molecule.</param>
        /// <returns>map count.</returns>
        int CountMaps(IAtomContainer target);

        /// <summary>
        /// Returns all solution map.
        /// </summary>
        /// <param name="target">molecule.</param>
        /// <returns>get maps.</returns>
        IReadOnlyList<IReadOnlyDictionary<INode, IAtom>> GetMaps(IAtomContainer target);

        /// <summary>
        /// Returns first solution map.
        /// </summary>
        /// <param name="target">molecule.</param>
        /// <returns>get first map.</returns>
        IReadOnlyDictionary<INode, IAtom> GetFirstMap(IAtomContainer target);

        /// <summary>
        /// checks if a map exits for a molecule.
        /// </summary>
        /// <param name="molecule">molecule</param>
        /// <returns>true/false.</returns>
        bool HasMap(TargetProperties molecule);

        /// <summary>
        /// Returns solution map count.
        /// </summary>
        /// <param name="target">target molecule.</param>
        /// <returns>map count.</returns>
        int CountMaps(TargetProperties target);

        /// <summary>
        /// Returns all solution map.
        /// </summary>
        /// <param name="target">molecule.</param>
        /// <returns>get maps.</returns>
        IReadOnlyList<IReadOnlyDictionary<INode, IAtom>> GetMaps(TargetProperties target);

        /// <summary>
        /// Returns first solution map.
        /// </summary>
        /// <param name="target">molecule.</param>
        /// <returns>get first map.</returns>
        IReadOnlyDictionary<INode, IAtom> GetFirstMap(TargetProperties target);
    }
}

