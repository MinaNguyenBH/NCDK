/*
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
 *
 * Copyright (C) 2009-2010  Syed Asad Rahman <asad@ebi.ac.uk>
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
 */

using NCDK.SMSD.Algorithms.VFLib.Builder;

using NCDK.SMSD.Algorithms.VFLib.Query;
using NCDK.SMSD.Globals;
using NCDK.SMSD.Tools;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NCDK.SMSD.Algorithms.VFLib.Map
{
    /// <summary>
    /// This class finds MCS between query and target molecules
    /// using VF2 algorithm.
    /// </summary>
    // @cdk.module smsd
    // @author Syed Asad Rahman <asad@ebi.ac.uk>
    [Obsolete("SMSD has been deprecated from the CDK with a newer, more recent version of SMSD is available at http://github.com/asad/smsd . ")]
    public class VFMCSMapper : IMapper
    {
        private readonly IQuery query = null;
        private List<IReadOnlyDictionary<INode, IAtom>> maps = null;
        private int currentMCSSize = -1;
        private static TimeManager timeManager = null;

        /// <summary>
        /// <returns>the timeout</returns>
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        protected static double GetTimeOut() => TimeOut.Instance.Time;

        protected static TimeManager TimeManager
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                return timeManager;
            }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                TimeOut.Instance.Enabled = false;
                timeManager = value;
            }
        }

        public VFMCSMapper(IQuery query)
        {
            TimeManager = new TimeManager();
            this.query = query;
            this.maps = new List<IReadOnlyDictionary<INode, IAtom>>();
        }

        public VFMCSMapper(IAtomContainer queryMolecule, bool bondMatcher)
        {
            TimeManager = new TimeManager();
            this.query = new QueryCompiler(queryMolecule, bondMatcher).Compile();
            this.maps = new List<IReadOnlyDictionary<INode, IAtom>>();
        }

        /// <inheritdoc/>
        public bool HasMap(IAtomContainer targetMolecule)
        {
            IState state = new VFState(query, new TargetProperties(targetMolecule));
            maps.Clear();
            return MapFirst(state);
        }

        public IReadOnlyList<IReadOnlyDictionary<INode, IAtom>> GetMaps(IAtomContainer target)
        {
            IState state = new VFState(query, new TargetProperties(target));
            maps.Clear();
            MapAll(state);
            return maps;
        }

        public IReadOnlyDictionary<INode, IAtom> GetFirstMap(IAtomContainer target)
        {
            IState state = new VFState(query, new TargetProperties(target));
            maps.Clear();
            MapFirst(state);
            return maps.Count == 0 ? new Dictionary<INode, IAtom>() : maps[0];
        }

        public int CountMaps(IAtomContainer target)
        {
            IState state = new VFState(query, new TargetProperties(target));
            maps.Clear();
            MapAll(state);
            return maps.Count;
        }

        /// <inheritdoc/>
        public bool HasMap(TargetProperties targetMolecule)
        {
            IState state = new VFState(query, targetMolecule);
            maps.Clear();
            return MapFirst(state);
        }

        public IReadOnlyList<IReadOnlyDictionary<INode, IAtom>> GetMaps(TargetProperties targetMolecule)
        {
            IState state = new VFState(query, targetMolecule);
            maps.Clear();
            MapAll(state);
            return maps;
        }

        public IReadOnlyDictionary<INode, IAtom> GetFirstMap(TargetProperties targetMolecule)
        {
            IState state = new VFState(query, targetMolecule);
            maps.Clear();
            MapFirst(state);
            return maps.Count == 0 ? new Dictionary<INode, IAtom>() : maps[0];
        }

        public int CountMaps(TargetProperties targetMolecule)
        {
            IState state = new VFState(query, targetMolecule);
            maps.Clear();
            MapAll(state);
            return maps.Count;
        }

        private void AddMapping(IState state)
        {
            var map = state.GetMap();
            if (!HasMap(map) && map.Count > currentMCSSize)
            {
                maps.Add(map);
                currentMCSSize = map.Count;
            }
            else if (!HasMap(map) && map.Count == currentMCSSize)
            {
                maps.Add(map);
            }
        }

        private void MapAll(IState state)
        {
            if (state.IsDead)
            {
                return;
            }

            if (state.IsGoal)
            {
                var map = state.GetMap();
                if (!HasMap(map))
                {
                    maps.Add(state.GetMap());
                }
                else
                {
                    state.BackTrack();
                }
            }
            else
            {
                AddMapping(state);
            }

            while (state.HasNextCandidate())
            {
                Match candidate = state.NextCandidate();
                if (state.IsMatchFeasible(candidate))
                {
                    IState nextState = state.NextState(candidate);
                    MapAll(nextState);
                    nextState.BackTrack();
                }
            }
        }

        private bool MapFirst(IState state)
        {
            if (state.IsDead)
            {
                return false;
            }

            if (state.IsGoal)
            {
                maps.Add(state.GetMap());
                return true;
            }

            bool found = false;
            while (!found && state.HasNextCandidate())
            {
                Match candidate = state.NextCandidate();
                if (state.IsMatchFeasible(candidate))
                {
                    IState nextState = state.NextState(candidate);
                    found = MapFirst(nextState);
                    nextState.BackTrack();
                }
            }
            return found;
        }

        private bool HasMap(IReadOnlyDictionary<INode, IAtom> map)
        {
            foreach (var storedMap in maps)
            {
                if (Mapper.Comparer_INode_IAtom.Equals(storedMap, map))
                {
                    return true;
                }
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static bool IsTimeOut()
        {
            if (GetTimeOut() > -1 && TimeManager.GetElapsedTimeInMinutes() > GetTimeOut())
            {
                TimeOut.Instance.Enabled = true;
                return true;
            }
            return false;
        }
    }
}
