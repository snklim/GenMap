using System.Globalization;
using System.Linq;

namespace GenMap.UI
{
    public class MapCell
    {
        public MapCell(int cellX, int cellY)
        {
            X = cellX;
            Y = cellY;
            CellX = cellX;
            CellY = cellY;

            LetterIndex = 1;
            SectorIndex = 1;
            SegmentIndex = 71;
        }

        public string MapPageKey
        {
            get
            {
                return string.Format("{0}-{1}-{2}",
                    Letters[LetterIndex], Sectors[SectorIndex], Segments[SegmentIndex]);
            }
        }

        public int X { get; set; }

        public int Y { get; set; }

        public int CellX { get; set; }

        public int CellY { get; set; }

        public int LetterIndex { get; set; }

        public int SectorIndex { get; set; }

        public int SegmentIndex { get; set; }

        public void TurnUp()
        {
            if (CellY + 1 < MapPage.MaxCellHeight)
            {
                CellY++;
            }
            else
            {
                CellY = (CellY + 1) % MapPage.MaxCellHeight;

                if (SegmentIndex + 12 > 144)
                {
                    LetterIndex = (LetterIndex - 1 + Letters.Length) % Letters.Length;
                }
                SegmentIndex = (SegmentIndex + 12) % 144;
            }
            
        }

        public void TurnRight()
        {
            if (CellX - 1 >= 0)
            {
                CellX--;
            }
            else
            {
                CellX = (CellX - 1 + MapPage.MaxCellWidth) % MapPage.MaxCellWidth;

                if (SegmentIndex % 12 == 0)
                {
                    SegmentIndex += 11;
                    SectorIndex = (SectorIndex - 1 + Sectors.Length) % Sectors.Length;
                }
                else
                {
                    SegmentIndex -= 1;
                }
            }
        }

        public void TurnBottom()
        {
            if (CellY - 1 >= 0)
            {
                CellY--;
            }
            else
            {
                CellY = (CellY - 1 + MapPage.MaxCellHeight) % MapPage.MaxCellHeight;

                if (SegmentIndex < 12)
                {
                    SegmentIndex = 144 - SegmentIndex;
                    LetterIndex = (LetterIndex + 1) % Letters.Length;
                }
                else
                {
                    SegmentIndex -= 12;
                }
            }
        }

        public void TurnLeft()
        {
            if (CellX + 1 < MapPage.MaxCellWidth)
            {
                CellX++;
            }
            else
            {
                CellX = (CellX + 1) % MapPage.MaxCellWidth;

                if ((SegmentIndex + 1) % 12 == 0)
                {
                    SegmentIndex -= 11;
                    SectorIndex = SectorIndex + 1;
                }
                else
                {
                    SegmentIndex++;
                }
            }
        }

        public static char[] Letters = {'l', 'm', 'n'};

        public static string[] Sectors = {"34", "35", "36", "37"};

        public static string[] Segments = Enumerable.Range(1, 144)
            .Select(i =>
            {
                var ret = i.ToString(CultureInfo.InvariantCulture);

                if (ret.Length == 1)
                {
                    ret = "00" + ret;
                }
                else if (ret.Length == 2)
                {
                    ret = "0" + ret;
                }

                return ret;
            })
            .ToArray();
    }
}