/*
 * Copyright (C) 2006-2010  Syed Asad Rahman <asad@ebi.ac.uk>
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

using System;
using System.Runtime.CompilerServices;

namespace NCDK.SMSD.Globals
{
    /// <summary>
    /// Class that manages MCS timeout.
    /// </summary>
    // @cdk.module smsd
    // @author Syed Asad Rahman <asad@ebi.ac.uk>
    [Obsolete("SMSD has been deprecated from the CDK with a newer, more recent version of SMSD is available at http://github.com/asad/smsd .")]
    public class TimeOut
    {
        private static TimeOut instance = null;

        /// <summary>
        /// Get Instance of the timeout. This starts the timeout counter.
        /// </summary>
        /// <returns>Instance</returns>
        public static TimeOut Instance
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get
            {
                if (instance == null)
                {
                    // it's ok, we can call this constructor
                    instance = new TimeOut();
                }
                return instance;
            }
        }

        protected internal TimeOut() { }

        /// <summary>
        /// cutoff value for time out.
        /// -1 for infinite and 0.23 for 23 seconds.
        /// </summary>
        public double Time { get; set; } = -1;

        /// <summary>
        /// true if timeout occures else false
        /// </summary>
        public bool Enabled { get; set; }
    }
}
