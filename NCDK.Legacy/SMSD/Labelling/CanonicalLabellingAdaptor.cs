using NCDK.Graphs.Invariant;
using NCDK.Smiles;
using System;

namespace NCDK.SMSD.Labelling
{
    // @cdk.module smsd
    [Obsolete]
    public class CanonicalLabellingAdaptor : ICanonicalMoleculeLabeller
    {
        public IAtomContainer GetCanonicalMolecule(IAtomContainer container)
        {
            return AtomContainerAtomPermutor.Permute(GetCanonicalPermutation(container), container);
        }

        public int[] GetCanonicalPermutation(IAtomContainer container)
        {
            CanonicalLabeler labeler = new CanonicalLabeler();
            labeler.CanonLabel(container);
            int n = container.Atoms.Count;
            int[] perm = new int[n];
            for (int i = 0; i < n; i++)
            {
                IAtom a = container.Atoms[i];
                int x = (int)a.GetProperty<long>(InvPair.CanonicalLabelPropertyKey);
                perm[i] = x - 1;
            }
            return perm;
        }
    }
}
