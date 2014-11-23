using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace GenMap.UI
{
    public class Map
    {
        private Dictionary<string, MapPage> _mapPages;

        private List<MapCell> _mapCells;

        private Bitmap _mapViewBitmap;

        private readonly MapPageDownloader _mapPageDownloader = new MapPageDownloader();

        public Dictionary<string, MapPage> MapPages
        {
            get
            {
                return _mapPages ?? (_mapPages = new Dictionary<string, MapPage>());
            }
        }

        public List<MapCell> MapCells { get { return _mapCells ?? (_mapCells = new List<MapCell>()); } }

        public int MapViewWidth { get; private set; }

        public int MapViewHeight { get; private set; }

        public int MapViewCellWidth { get; private set; }

        public int MapViewCellHeight { get; private set; }

        public void SetViewPort(int mapViewWidth, int mapViewHeight)
        {
            MapViewWidth = mapViewWidth + 2 * MapPage.CellSize;
            MapViewHeight = mapViewHeight + 2 * MapPage.CellSize;

            MapViewCellWidth = MapViewWidth / MapPage.CellSize;
            MapViewCellHeight = MapViewHeight / MapPage.CellSize;

            MapCells.Clear();

            for (int y = 0; y < MapViewCellHeight; y++)
            {
                for (int x = 0; x < MapViewCellWidth; x++)
                {
                    MapCells.Add(new MapCell(x, y));
                }
            }

            _mapViewBitmap = new Bitmap(MapViewWidth, MapViewHeight);
        }

        private void ProcessCells(Graphics g, int shiftX, int shiftY, string mapPageKey, List<MapCell> cells)
        {
            var rectTarget = new Rectangle();
            var rectSource = new Rectangle();

            var flag = true;

            cells.ForEach(c =>
            {
                var x = shiftX + c.X * MapPage.CellSize;
                var y = shiftY + c.Y * MapPage.CellSize;

                var sourceX = MapPage.CellSize * c.CellX;
                var sourceY = MapPage.CellSize * c.CellY;

                if (flag)
                {
                    rectTarget = new Rectangle(x, y, MapPage.CellSize, MapPage.CellSize);
                    rectSource = new Rectangle(sourceX, sourceY, MapPage.CellSize, MapPage.CellSize);
                    flag = false;
                }

                rectTarget = Rectangle.Union(rectTarget, new Rectangle(x, y, MapPage.CellSize, MapPage.CellSize));
                rectSource = Rectangle.Union(rectSource, new Rectangle(sourceX, sourceY, MapPage.CellSize, MapPage.CellSize));
            });

            if (MapPages.ContainsKey(mapPageKey))
            {
                g.DrawImage(MapPages[mapPageKey].MapPageBitmap, rectTarget, rectSource, GraphicsUnit.Pixel);
            }
        }

        public void DrawMap(Graphics g1, int shiftX, int shiftY)
        {
            var groups = MapCells.GroupBy(c => c.MapPageKey).ToList();
            var existsMapPages = new HashSet<string>();

            using (var g = Graphics.FromImage(_mapViewBitmap))
            {
                g.Clear(Color.White);

                groups.ForEach(group =>
                {
                    existsMapPages.Add(group.Key);

                    if (!MapPages.ContainsKey(group.Key))
                    {
                        string mapPageFile = string.Format("{0}\\maps\\{1}.jpg",
                            Directory.GetCurrentDirectory(),
                            group.Key);

                        if (!_mapPageDownloader.Tasks.Contains(group.Key))
                        {
                            if (File.Exists(mapPageFile))
                            {
                                MapPages.Add(group.Key, new MapPage(group.Key, mapPageFile));
                            }
                            else
                            {
                                _mapPageDownloader.Download(group.Key);
                            }
                        }
                    }

                    ProcessCells(g, shiftX, shiftY, group.Key, group.ToList());
                });

                g1.DrawImage(_mapViewBitmap,
                    new Rectangle(-MapPage.CellSize, -MapPage.CellSize, MapViewWidth, MapViewHeight),
                    new Rectangle(0, 0, MapViewWidth, MapViewHeight),
                    GraphicsUnit.Pixel);
            }

            if (MapPages.Count > 4)
            {
                MapPages.Where(kv => !existsMapPages.Contains(kv.Key)).ToList().ForEach(kv => MapPages.Remove(kv.Key));
            }
        }

        public void TurnLeft()
        {
            MapCells.ForEach(c => c.TurnLeft());
        }

        public void TurnRight()
        {
            MapCells.ForEach(c => c.TurnRight());
        }

        public void TurnUp()
        {
            MapCells.ForEach(c => c.TurnUp());
        }

        public void TurnBottom()
        {
            MapCells.ForEach(c => c.TurnBottom());
        }
    }
}