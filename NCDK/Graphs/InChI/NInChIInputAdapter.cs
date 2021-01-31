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

using NCDK.Common.Primitives;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace NCDK.Graphs.InChI
{
    internal class NInChIInputAdapter : NInchiInput
    {
        /// <summary>
        /// Flag indicating windows or linux.
        /// </summary>
        private static bool IsWindows { get; } = Environment.OSVersion.Platform < PlatformID.Unix;

        /// <summary>
        /// Switch character for passing options. / in windows, - on other systems.
        /// </summary>
        private static readonly string FLAG_CHAR = IsWindows ? "/" : "-";

        public static readonly string FIVE_SECOND_TIMEOUT = FLAG_CHAR + "W5";

        public NInChIInputAdapter(string options)
        {
            this.Options = options == null ? "" : CheckOptions(options);
        }

        public NInChIInputAdapter(IList<InChIOption> options)
        {
            this.Options = options == null ? "" : CheckOptions(options);
        }

        private static bool IsTimeoutOptions(string op)
        {
            if (op == null || op.Length < 2)
                return false;
            return op[0] == 'W';
        }

        private static bool IsSubSecondTimeout(string op)
        {
            return op.IndexOf('.') >= 0 || op.IndexOf(',') >= 0;
        }

        private static string CheckOptions(string ops)
        {
            if (ops == null)
            {
                throw new ArgumentNullException(nameof(ops));
            }
            var sbOptions = new StringBuilder();

            bool hasUserSpecifiedTimeout = false;

            var tok = Strings.Tokenize(ops);
            string options = string.Join(" ", tok.Select(n =>
                {
                    string op = n;
                    if (op.StartsWithChar('-') || op.StartsWithChar('/'))
                    {
                        op = op.Substring(1);
                    }

                    var option = InChIOption.ValueOfIgnoreCase(op);
                    if (option != null)
                    {
                        return FLAG_CHAR + option.Name;
                    }
                    else if (IsTimeoutOptions(op))
                    {
                        var time = Math.Ceiling(double.Parse(op.Substring(1), NumberFormatInfo.InvariantInfo));
                        // fix #653: safer to use whole seconds, rounded to next bigger integer
                        if (time >= 0.0)
                        {
                            return $"{FLAG_CHAR}W{string.Format("F1", time)}";
                            hasUserSpecifiedTimeout = true;
                        }
                        return "";
                    }
                    // 1,5 tautomer option
                    else if (string.Equals("15T", op, StringComparison.Ordinal))
                    {
                        return FLAG_CHAR + "15T";
                    }
                    // keto-enol tautomer option
                    else if (string.Equals("KET", op, StringComparison.Ordinal))
                    {
                        return FLAG_CHAR + "KET";
                    }
                    else
                    {
                        throw new NInchiException("Unrecognised InChI option");
                    }
                }));

            if (!hasUserSpecifiedTimeout)
            {
                if (options.Length > 0)
                    options += " ";
                options += FIVE_SECOND_TIMEOUT;
            }
            return options;
        }

        private static string CheckOptions(IList<InChIOption> ops)
        {
            if (ops == null)
            {
                throw new ArgumentNullException(nameof(ops), "Null options");
            }
            string options = string.Join(" ", ops.Select(op => FLAG_CHAR + op.Name));
            if (options.Length > 0)
                options += " ";
            options += FIVE_SECOND_TIMEOUT;

            return options;
        }
    }
}
