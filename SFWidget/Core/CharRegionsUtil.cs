using System;
using System.Collections.Generic;

namespace SFEditor
{
    static class CharRegionsUtil
    {
        public static CharacterRegion CombineCharacters(CharacterRegion ca, CharacterRegion cb)
        { 
            int start = Math.Min(ca.Start, cb.Start);
            int end = Math.Max(ca.End, cb.End);

            return new CharacterRegion(start, end);
        }

        public static bool CompareCharacters(CharacterRegion ca, CharacterRegion cb)
        {
            return (ca.Start < cb.Start && ca.End < cb.Start) || (ca.Start > cb.End && ca.End > cb.End);
        }

        public static bool CheckConflict(List<CharacterRegion> charRegions)
        {
            for (int i = 0; i < charRegions.Count; i++)
                for (int ii = i + 1; ii < charRegions.Count; ii++)
                    if (!CompareCharacters(charRegions[i], charRegions[ii]))
                        return false;

            return true;
        }

        public static List<CharacterRegion> ResolveConflicts(List<CharacterRegion> charRegions)
        {
            List<CharacterRegion> chars = new List<CharacterRegion>();

            for (int i = 0; i < charRegions.Count; i++)
                chars = InsertRegion(chars, charRegions[i]);

            return chars;
        }

        public static List<CharacterRegion> InsertRegion(List<CharacterRegion> charRegions, CharacterRegion charReg)
        {
            for (int i = 0; i < charRegions.Count; i++)
            {
                if (!CompareCharacters(charRegions[i], charReg))
                {
                    charRegions[i] = CombineCharacters(charRegions[i], charReg);
                    return ResolveConflicts(charRegions);
                }
            }

            charRegions.Add(charReg);
            return charRegions;
        }
    }
}
