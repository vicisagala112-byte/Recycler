using UnityEngine;

namespace Anoa
{
    public static class TrashTypeMapper
    {
        public static TRASH_TYPE GetTrashTypeFromID(int id)
        {
            // 🟦 ANORGANIK
            if (id >= 0 && id <= 8)
                return TRASH_TYPE.ANORGANIK;

            // 🟧 B3 / MEDIS
            if (id >= 9 && id <= 13)
                return TRASH_TYPE.B3;

            // 🟩 ORGANIK
            if (id >= 14 && id <= 23)
                return TRASH_TYPE.ORGANIK;

            Debug.LogWarning("ID Sampah tidak dikenal: " + id);
            return TRASH_TYPE.ORGANIK; // default
        }
    }
}
