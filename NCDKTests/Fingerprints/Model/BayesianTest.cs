/* $Revision$ $Author$ $Date$
 *
 * Copyright (c) 2015 Collaborative Drug Discovery, Inc. <alex@collaborativedrug.com>
 *
 * Implemented by Alex M. Clark, produced by Collaborative Drug Discovery, Inc.
 * Made available to the CDK community under the terms of the GNU LGPL.
 *
 *    http://collaborativedrug.com
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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using NCDK.IO.Iterator;
using NCDK.Silent;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;

namespace NCDK.Fingerprints.Model
{
    /// <summary>
    /// Validation test for the Bayesian model building & serialisation.
    /// </summary>
    // @cdk.module test-standard
    [TestClass()]
    public class BayesianTest
    {
        private const string REF_MOLECULE = "\n\n\n"
            + " 18 19  0  0  0  0  0  0  0  0999 V2000\n"
            + "   -2.5317   -1.1272    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0\n"
            + "   -1.5912    0.1672    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0\n"
            + "   -2.2420    1.6289    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0\n"
            + "    0.0000    0.0000    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0\n"
            + "    1.0706    1.1890    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0\n"
            + "    2.5323    0.5383    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0\n"
            + "    2.3650   -1.0530    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0\n"
            + "    0.8000   -1.3856    0.0000 S   0  0  0  0  0  0  0  0  0  0  0  0\n"
            + "    3.5541   -2.1236    0.0000 N   0  0  0  0  0  0  0  0  0  0  0  0\n"
            + "    5.0758   -1.6292    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0\n"
            + "    5.4084   -0.0641    0.0000 O   0  0  0  0  0  0  0  0  0  0  0  0\n"
            + "    6.2648   -2.6998    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0\n"
            + "    7.7865   -2.2053    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0\n"
            + "    8.1191   -0.6403    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0\n"
            + "    9.6408   -0.1459    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0\n"
            + "   10.8299   -1.2165    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0\n"
            + "   10.4972   -2.7815    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0\n"
            + "    8.9755   -3.2760    0.0000 C   0  0  0  0  0  0  0  0  0  0  0  0\n"
            + "  1  2  1  0  0  0  0\n" + "  2  3  1  0  0  0  0\n"
            + "  2  4  1  0  0  0  0\n" + "  4  8  1  0  0  0  0\n"
            + "  4  5  2  0  0  0  0\n" + "  5  6  1  0  0  0  0\n"
            + "  6  7  2  0  0  0  0\n" + "  7  8  1  0  0  0  0\n"
            + "  7  9  1  0  0  0  0\n" + "  9 10  1  0  0  0  0\n"
            + " 10 11  2  0  0  0  0\n" + " 10 12  1  0  0  0  0\n"
            + " 12 13  1  0  0  0  0\n" + " 13 18  2  0  0  0  0\n"
            + " 13 14  1  0  0  0  0\n" + " 14 15  2  0  0  0  0\n"
            + " 15 16  1  0  0  0  0\n" + " 16 17  2  0  0  0  0\n"
            + " 17 18  1  0  0  0  0\n" + "M  END";

        private static readonly int[] REF_ECFP6_0 = {-1951192287, -1876567787, -1685505461, -1594062081, -1494889718,
            -1469934531, -1064027736, -1006701866, -976660244, -964879417, -854951091, -836160636, -801752141,
            -790042671, -777607960, -636984940, -568302198, -563910794, -513573682, -289109509, -203612477, 22318543,
            86479455, 134489603, 229166175, 369386629, 423552486, 543172923, 598483088, 684703116, 747997863,
            772035298, 790592664, 887527738, 962328941, 1053690696, 1143774000, 1194907145, 1323701668, 1413433893,
            1444795951, 1627608083, 1777673917, 1932154898, 1987069734, 1994067521, 2078126852, 2147204365};
        private static readonly int[] REF_ECFP6_1024 = {18, 19, 61, 95, 133, 144, 152, 206, 232, 236, 269, 277, 314, 315,
            365, 394, 396, 404, 420, 424, 463, 486, 507, 515, 521, 549, 559, 577, 587, 607, 679, 701, 707, 726, 738,
            767, 772, 778, 801, 806, 816, 840, 845, 886, 900, 947, 967, 977};
        private static readonly int[] REF_ECFP6_32768 = {2555, 2727, 2815, 2888, 3535, 3649, 4703, 5181, 5540, 6458, 6960,
            7111, 7875, 8336, 9448, 9731, 9917, 10555, 11041, 13060, 13188, 14760, 14923, 15283, 15629, 15756, 18214,
            18981, 19210, 19551, 21218, 21523, 22025, 22063, 22546, 22764, 22805, 24980, 25733, 25994, 26086, 26486,
            26577, 29398, 31085, 31565, 31896, 31950};

        private static readonly int[] REF_FCFP6_0 = {-2128353587, -1853365819, -1764181020, -1625147000, -1589802267,
            -1589654580, -1571133932, -1555670640, -1475665446, -1377516953, -1369998514, -1226686118, -1114704338,
            -983437780, -674976432, -620757428, -454679744, -79956240, 0, 2, 3, 16, 32192941, 147050355, 193192566,
            205312945, 252180819, 346770359, 627637376, 785469695, 822686044, 824716024, 901194889, 960613971,
            994111779, 1018173271, 1481939742, 1629496255, 1992157502, 2101841914};
        private static readonly int[] REF_FCFP6_1024 = {0, 2, 3, 16, 128, 137, 255, 291, 318, 336, 339, 346, 348, 392, 400,
            429, 453, 474, 532, 556, 558, 588, 595, 615, 630, 717, 741, 752, 760, 798, 832, 846, 855, 883, 945, 951,
            959, 972, 996, 1018                 };
        private static readonly int[] REF_FCFP6_32768 = {0, 2, 3, 16, 2789, 4090, 5975, 6942, 8666, 9024, 9151, 9353, 11000,
            11600, 12636, 14728, 14765, 15332, 16730, 16999, 19383, 19404, 20051, 20339, 20735, 21425, 22928, 25029,
            25206, 26132, 26317, 26942, 28204, 28963, 30254, 30448, 31059, 31566, 31872, 32332};

        // ----------------- public methods -----------------

        [TestMethod()]
        public void TestFingerprints()
        {
            Trace.TraceInformation("Bayesian/Fingerprints test: verifying circular fingerprints for a single molecule");

            CheckFP(REF_MOLECULE, CircularFingerprinterClass.ECFP6, 0, REF_ECFP6_0);
            CheckFP(REF_MOLECULE, CircularFingerprinterClass.ECFP6, 1024, REF_ECFP6_1024);
        }

        [TestMethod()]
        public void TestAuxiliary()
        {
            Trace.TraceInformation("Bayesian/Fingerprints test: making sure auxiliary fields are preserved");

            CheckTextFields();
        }

        [TestMethod()]
        public void TestConfusion()
        {
            Trace.TraceInformation("Bayesian/Fingerprints test: ensuring expected truth table for canned data");

            ConfirmPredictions("Tiny.sdf", 8, 8, 0, 0);
            ConfirmPredictions("Small.sdf", 6, 12, 0, 6);
        }

        [TestMethod()]
        public void TestFolding()
        {
            Trace.TraceInformation("Bayesian/Fingerprints test: comparing folded fingerprints to reference set");

            CompareFolding("FoldedProbes.sdf", "ECFP6/0", CircularFingerprinterClass.ECFP6, 0);
            CompareFolding("FoldedProbes.sdf", "ECFP6/1024", CircularFingerprinterClass.ECFP6, 1024);
            CompareFolding("FoldedProbes.sdf", "ECFP6/32768", CircularFingerprinterClass.ECFP6, 32768);
            CompareFolding("FoldedProbes.sdf", "FCFP6/0", CircularFingerprinterClass.FCFP6, 0);
        }

        [TestMethod()]
        [TestCategory("SlowTest")]
        public void TestExample1()
        {
            Trace.TraceInformation("Bayesian/Fingerprints test: using dataset of binding data to compare to reference data");

            RunTest("Binders.sdf", "active", CircularFingerprinterClass.ECFP6, 1024, 0, "Binders-ECFP6-1024-loo.bayesian", true);
            RunTest("Binders.sdf", "active", CircularFingerprinterClass.ECFP6, 32768, 5, "Binders-ECFP6-32768-xv5.bayesian", true);
            RunTest("Binders.sdf", "active", CircularFingerprinterClass.FCFP6, 0, 0, "Binders-FCFP6-0-loo.bayesian", true);
        }

        [TestMethod()]
        [TestCategory("SlowTest")]
        public void TestExample2()
        {
            Trace.TraceInformation("Bayesian/Fingerprints test: using dataset of molecular probes to compare to reference data");

            RunTest("MLProbes.sdf", "Lipinski score", CircularFingerprinterClass.ECFP6, 1024, 0,
                "MLProbes-ECFP6-1024-loo.bayesian");
            RunTest("MLProbes.sdf", "Lipinski score", CircularFingerprinterClass.ECFP6, 32768, 5,
                "MLProbes-ECFP6-32768-xv5.bayesian");
            RunTest("MLProbes.sdf", "Lipinski score", CircularFingerprinterClass.FCFP6, 0, 0,
                "MLProbes-FCFP6-0-loo.bayesian");
            RunTest("MLProbes.sdf", "Lipinski score", CircularFingerprinterClass.FCFP6, 256, 3,
                "MLProbes-FCFP6-256-xv3.bayesian");
        }

        // ----------------- private methods -----------------

        // make sure that for a single molecule, the way that the hashes are created & folded is consistent with a reference
        private static void CheckFP(string molstr, CircularFingerprinterClass classType, int folding, int[] refHash)
        {
            var strType = classType == CircularFingerprinterClass.ECFP6 ? "ECFP6" : "FCFP6";
            WriteLine($"Comparing hash codes for {strType}/folding={folding}");

            var mol = new EnumerableSDFReader(new StringReader(molstr), ChemObjectBuilder.Instance).First();
            var model = new Bayesian(classType, folding);
            model.AddMolecule(mol, false);

            var calcHash = model.Training[0];
            var same = calcHash.Length == refHash.Length;
            if (same)
                for (int n = 0; n < calcHash.Length; n++)
                    if (calcHash[n] != refHash[n])
                    {
                        same = false;
                        break;
                    }
            if (!same)
            {
                WriteLine($"    ** calculated: {ArrayStr(calcHash)}");
                WriteLine($"    ** reference:  {ArrayStr(refHash)}");
                throw new CDKException("Hashes differ.");
            }
        }

        // make sure auxiliary fields like title & comments can serialise/deserialise
        private void CheckTextFields()
        {
            WriteLine("Checking integrity of text fields");

            var dummyTitle = "some title";
            var dummyOrigin = "some origin";
            var dummyComments = new string[] { "comment1", "comment2" };

            var model1 = new Bayesian(CircularFingerprinterClass.ECFP6)
            {
                NoteTitle = dummyTitle,
                NoteOrigin = dummyOrigin,
                NoteComments = dummyComments
            };

            Bayesian model2 = null;
            try
            {
                model2 = Bayesian.Deserialise(model1.Serialise());
            }
            catch (IOException ex)
            {
                throw new CDKException("Reserialisation failed", ex);
            }

            if (!dummyTitle.Equals(model1.NoteTitle, StringComparison.Ordinal)
             || !dummyTitle.Equals(model2.NoteTitle, StringComparison.Ordinal)
             || !dummyOrigin.Equals(model1.NoteOrigin, StringComparison.Ordinal)
             || !dummyOrigin.Equals(model2.NoteOrigin, StringComparison.Ordinal))
                throw new CDKException("Note integrity failure for origin");

            var comments1 = model1.NoteComments;
            var comments2 = model2.NoteComments;
            if (comments1.Count != dummyComments.Length
             || comments2.Count != dummyComments.Length
             || !comments1[0].Equals(dummyComments[0], StringComparison.Ordinal)
             || !comments2[0].Equals(dummyComments[0], StringComparison.Ordinal)
             || !comments1[1].Equals(dummyComments[1], StringComparison.Ordinal)
             || !comments2[1].Equals(dummyComments[1], StringComparison.Ordinal))
                throw new CDKException("Note integrity failure for origin");
        }

        // builds a model and uses the scaled predictions to rack up a confusion matrix, for comparison
        private static void ConfirmPredictions(string sdfile, int truePos, int trueNeg, int falsePos, int falseNeg)
        {
            WriteLine($"[{sdfile}] comparing confusion matrix");

            var molecules = new List<IAtomContainer>();
            var activities = new List<bool>();
            var model = new Bayesian(CircularFingerprinterClass.ECFP6, 1024);

            try
            {
                using (var ins = ResourceLoader.GetAsStream("NCDK.Data.CDD." + sdfile))
                {
                    var rdr = new EnumerableSDFReader(ins, ChemObjectBuilder.Instance);

                    foreach (var mol in rdr)
                    {
                        bool actv = "true" == (string)mol.GetProperties()["Active"];
                        molecules.Add(mol);
                        activities.Add(actv);
                        model.AddMolecule(mol, actv);
                    }
                }
            }
            catch (CDKException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new CDKException("Test failed", ex);
            }

            model.Build();
            model.ValidateLeaveOneOut();

            // build the confusion matrix
            int gotTP = 0, gotTN = 0, gotFP = 0, gotFN = 0;
            for (int n = 0; n < molecules.Count; n++)
            {
                double pred = model.ScalePredictor(model.Predict(molecules[n]));
                bool actv = activities[n];
                if (pred >= 0.5)
                {
                    if (actv)
                        gotTP++;
                    else
                        gotFP++;
                }
                else
                {
                    if (actv)
                        gotFN++;
                    else
                        gotTN++;
                }
            }

            WriteLine("    True Positives:  got=" + gotTP + " require=" + truePos);
            WriteLine("         Negatives:  got=" + gotTN + " require=" + trueNeg);
            WriteLine("    False Positives: got=" + gotFP + " require=" + falsePos);
            WriteLine("          Negatives: got=" + gotFN + " require=" + falseNeg);

            if (gotTP != truePos || gotTN != trueNeg || gotFP != falsePos || gotFN != falseNeg)
                throw new CDKException("Confusion matrix mismatch");
        }

        // compares a series of molecules for folding fingerprints being literally identical
        private static void CompareFolding(string sdfile, string fpField, CircularFingerprinterClass classType, int folding)
        {
            WriteLine($"[{sdfile}] calculation of: {fpField}");

            using (var ins = ResourceLoader.GetAsStream($"NCDK.Data.CDD.{sdfile}"))
            {
                var rdr = new EnumerableSDFReader(ins, ChemObjectBuilder.Instance);

                int row = 0;
                foreach (var mol in rdr)
                {
                    row++;
                    var model = new Bayesian(classType, folding);
                    model.AddMolecule(mol, false);
                    var hashes = model.Training[0];
                    var gotHashes = ArrayStr(hashes);
                    var reqHashes = (string)mol.GetProperties()[fpField];
                    Assert.AreEqual(reqHashes, gotHashes, $"Folded hashes do not match reference at {row}.");
                }
            }
        }
    
        // performs a bulk test: loads an SDfile, builds a model with the given parameters, and compares it to a reference model
        // that has been previously serialised
        private void RunTest(string sdfile, string actvField, CircularFingerprinterClass classType, int folding, int xval, string modelFN)
        { 
            RunTest(sdfile, actvField, classType, folding, xval, modelFN, false);
        }

        private void RunTest(string sdfile, string actvField, CircularFingerprinterClass classType, int folding, int xval, string modelFN, bool perceiveStereo)
        {
            WriteLine("[" + modelFN + "]");
            WriteLine("    Loading " + sdfile);

            try
            {
                var model = new Bayesian(classType, folding) { PerceiveStereo = perceiveStereo };

                int row = 0, numActives = 0;
                using (var rdr = new EnumerableSDFReader(ResourceLoader.GetAsStream($"NCDK.Data.CDD.{sdfile}"), ChemObjectBuilder.Instance))
                {
                    foreach (var mol in rdr)
                    {
                        row++;

                        var stractv = (string)mol.GetProperties()[actvField];
                        int active = stractv.Equals("true", StringComparison.Ordinal) ? 1 : stractv.Equals("false", StringComparison.Ordinal) ? 0 : int.Parse(stractv, NumberFormatInfo.InvariantInfo);
                        if (active != 0 && active != 1)
                            throw new CDKException("Activity field not found or invalid");

                        model.AddMolecule(mol, active == 1);
                        numActives += active;
                    }
                }

                WriteLine($"    Training with {row} rows, {numActives} actives, {(row - numActives)} inactives");

                model.Build();
                if (xval == 3)
                    model.ValidateThreeFold();
                else if (xval == 5)
                    model.ValidateFiveFold();
                else
                    model.ValidateLeaveOneOut();

                WriteLine($"    Validation: ROC AUC={model.RocAUC}");
                WriteLine($"    Parsing reference model");

                //FileReader frdr=new FileReader(modelFN);
                Bayesian reference;
                using (var mrdr = new StreamReader(ResourceLoader.GetAsStream($"NCDK.Data.CDD.{modelFN}")))
                {
                    reference = Bayesian.Deserialise(mrdr);
                }

                // start comparing the details...

                bool failed = false;
                if (model.Folding != reference.Folding)
                {
                    WriteLine($"    ** reference folding size={reference.Folding}");
                    failed = true;
                }
                if (model.TrainingSize != reference.TrainingSize)
                {
                    WriteLine($"    ** reference training size={reference.TrainingSize}");
                    failed = true;
                }
                if (model.TrainingActives != reference.TrainingActives)
                {
                    WriteLine($"    ** reference training actives={reference.TrainingActives}");
                    failed = true;
                }
                if (model.RocType != reference.RocType)
                {
                    WriteLine($"    ** reference ROC type={reference.RocType}");
                    failed = true;
                }
                if (!DblEqual(model.RocAUC, reference.RocAUC))
                {
                    WriteLine($"    ** reference ROC AUC={reference.RocAUC}");
                    failed = true;
                }
                if (Math.Abs(model.LowThreshold - reference.LowThreshold) > 0.00000000000001)
                {
                    WriteLine($"    ** reference lowThresh={reference.LowThreshold} different to calculated {model.LowThreshold}");
                    failed = true;
                }
                if (Math.Abs(model.HighThreshold - reference.HighThreshold) > 0.00000000000001)
                {
                    WriteLine($"    ** reference highThresh={reference.HighThreshold} different to calculated {model.HighThreshold}");
                    failed = true;
                }

                // make sure individual hash bit contributions match
                var mbits = model.Contributions;
                var rbits = reference.Contributions;
                if (mbits.Count != rbits.Count)
                {
                    WriteLine($"    ** model has {mbits.Count} contribution bits, reference has {rbits.Count}");
                    failed = true;
                }
                foreach (var h in mbits.Keys)
                    if (!rbits.ContainsKey(h))
                    {
                        WriteLine($"    ** model hash bit {h} not found in reference");
                        failed = true;
                        break; // one is enough
                    }
                foreach (var h in rbits.Keys)
                    if (!mbits.ContainsKey(h))
                    {
                        WriteLine($"    ** reference hash bit {h} not found in model");
                        failed = true;
                        break; // one is enough
                    }
                foreach (var h in mbits.Keys)
                    if (rbits.ContainsKey(h))
                    {
                        double c1 = mbits[h], c2 = rbits[h];
                        if (!DblEqual(c1, c2))
                        {
                            WriteLine($"    ** contribution for bit {h}: model={c1}, reference={c2}");
                            failed = true;
                            break; // one is enough
                        }
                    }

                if (failed)
                    throw new CDKException("Comparison to reference failed");
            }
            catch (CDKException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new CDKException("Test failed", ex);
            }
        }

        // convenience functions
        private static void WriteLine(string str)
        {
            Trace.TraceInformation(str);
        }

        private static bool DblEqual(double v1, double v2)
        {
            return v1 == v2 || Math.Abs(v1 - v2) <= 1E-7 * Math.Max(Math.Abs(v1), Math.Abs(v2));   
                // 1E-14 to 1E-7 because of precision difference between double and float.
        }

        private static string ArrayStr(int[] A)
        {
            if (A == null)
                return "{null}";
            string str = "";
            for (int n = 0; n < A.Length; n++)
                str += (n > 0 ? "," : "") + A[n];
            return str;
        }
    }
}
