namespace Assets._scripts.Manager
{
    public enum BoardType
    {
        Rock,
        Water,
        Woods,
        Dirt,
        Desert,
        Grain,
        Pasture,
        PlayerBoard
    }

    public static class BitBoardManager
    {
        #region  Member

        public static long DesertBoard64 { get; private set; }
        public static long DirtBoard64 { get; private set; }
        public static long GrainBoard64 { get; private set; }
        public static long PastureBoard64 { get; private set; }
        public static long PlayerBoard64 { get; private set; }
        public static long RockBoard64 { get; private set; }
        public static long WaterBoard64 { get; private set; }
        public static long WoodBoard64 { get; private set; }

        #endregion

        public static int GetCellCount(long bitBoard)
        {
            var count = 0;
            for (var i = 0; i < sizeof(long) * 8; i++)
                if (((bitBoard >> i) & 1) == 1)
                    count++;
            return count;
        }

        public static long GetFreeDirtCellMask()
        {
            return DirtBoard64 & ~WoodBoard64 & ~PlayerBoard64;
        }

        public static void SetBitInBoard(BoardType type, int index)
        {
            switch (type)
            {
                case BoardType.Rock:
                    RockBoard64 |= 1L << index;
                    break;
                case BoardType.Water:
                    WaterBoard64 |= 1L << index;
                    break;
                case BoardType.Woods:
                    WoodBoard64 |= 1L << index;
                    break;
                case BoardType.Dirt:
                    DirtBoard64 |= 1L << index;
                    break;
                case BoardType.Desert:
                    DesertBoard64 |= 1L << index;
                    break;
                case BoardType.Grain:
                    GrainBoard64 |= 1L << index;
                    break;
                case BoardType.Pasture:
                    PastureBoard64 |= 1L << index;
                    break;
                case BoardType.PlayerBoard:
                    PlayerBoard64 |= 1L << index;
                    break;
            }
        }
    }
}