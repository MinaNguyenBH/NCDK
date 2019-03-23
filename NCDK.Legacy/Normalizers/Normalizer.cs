/*  
 *  Copyright (C) 2004-2010  The Chemistry Development Kit (CDK) project
 *                     2014  Mark B Vine (orcid:0000-0002-7794-0426)
 *
 *  Contact: cdk-devel@lists.sourceforge.net
 *
 *  This program is free software; you can redistribute it and/or
 *  modify it under the terms of the GNU Lesser General Public License
 *  as published by the Free Software Foundation; either version 2.1
 *  of the License, or (at your option) any later version.
 *  All we ask is that proper credit is given for our work, which includes
 *  - but is not limited to - adding the above copyright notice to the beginning
 *  of your source code files, and to any copyright notice that you may distribute
 *  with programs based on this work.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT Any WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 *
 *  You should have received a copy of the GNU Lesser General Public License
 *  along with this program; if not, write to the Free Software
 *  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
 */

using NCDK.Isomorphisms;
using NCDK.Isomorphisms.MCSS;
using NCDK.Smiles;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace NCDK.Normalizers
{
    /// <summary>
    /// Adjusts parts of an AtomContainer to the configuration of a fragment.
    /// </summary>
    // @author        shk3
    // @cdk.created   2004-03-04
    // @cdk.module    smiles
    [Obsolete("The functionality provided by with class is better suited to SMIRKS")]
    public static class Normalizer
    {
        /// <summary>
        ///  The method takes an XML files like the following:
        ///  <pre>
        ///  &lt;replace-set&gt;
        ///  &lt;replace&gt;O=N=O&lt;/replace&gt;
        ///  &lt;replacement&gt;[O-][N+]=O&lt;/replacement&gt;
        ///  &lt;/replace-set&gt;
        ///  </pre>
        /// </summary>
        /// <remarks>
        ///  All parts in ac which are the same as replace will be changed according to replacement.
        ///  Currently the following changes are done: BondOrder, FormalCharge.
        ///  For detection of fragments like replace, we rely on <see cref="UniversalIsomorphismTester"/>.
        ///  doc may contain several replace-sets and a replace-set may contain several replace fragments, which will all be normalized according to replacement.
        ///  </remarks>
        /// <param name="ac">The atomcontainer to normalize.</param>
        /// <param name="doc">The configuration file.</param>
        /// <returns>Did a replacement take place?</returns>
        /// <exception cref="InvalidSmilesException"> doc contains an invalid smiles.</exception>
        public static bool Normalize(IAtomContainer ac, XDocument doc)
        {
            var nl = doc.Elements("replace-set");
            var sp = new SmilesParser();

            bool change = false;
            foreach (var child in nl)
            {
                var replaces = child.Elements("replace");
                var replacement = child.Elements("replacement");
                string replacementstring;
                {
                    var en = replacement.GetEnumerator();
                    en.MoveNext();
                    replacementstring = en.Current.Value;
                    if (replacementstring.IndexOf('\n') > -1 || replacementstring.Length < 1)
                    {
                        en.MoveNext();
                        replacementstring = en.Current.Value;
                    }
                }
                var replacementStructure = sp.ParseSmiles(replacementstring);
                foreach (var replace in replaces)
                {
                    string replacestring;
                    {
                        var en = replace.Nodes().GetEnumerator();
                        en.MoveNext();
                        replacestring = ((XText)en.Current).Value;
                        if (replacestring.IndexOf('\n') > -1 || replacestring.Length < 1)
                        {
                            en.MoveNext();
                            replacestring = ((XText)en.Current).Value;
                        }
                    }
                    var replaceStructure = sp.ParseSmiles(replacestring);
                    IReadOnlyList<RMap> l = null;
                    var universalIsomorphismTester = new UniversalIsomorphismTester();
                    while ((l = universalIsomorphismTester.GetSubgraphMap(ac, replaceStructure)) != null)
                    {
                        var l2 = UniversalIsomorphismTester.MakeAtomsMapOfBondsMap(l, ac, replaceStructure);
                        foreach (var rmap in l)
                        {
                            var acbond = ac.Bonds[rmap.Id1];
                            var replacebond = replacementStructure.Bonds[rmap.Id2];
                            acbond.Order = replacebond.Order;
                            change = true;
                        }
                        foreach (var rmap in l2)
                        {
                            var acatom = ac.Atoms[rmap.Id1];
                            var replaceatom = replacementStructure.Atoms[rmap.Id2];
                            acatom.FormalCharge = replaceatom.FormalCharge;
                            change = true;
                        }
                    }
                }
            }
            return change;
        }
    }
}
