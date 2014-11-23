using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using Distort;

namespace GenMap.UI
{
    public class MapPage
    {
        public const int CellSize = 50;

        public const int MaxCellWidth = 42;

        public const int MaxCellHeight = 44;

        private static readonly Dictionary<string, List<Point>> MapPagePoints = new Dictionary<string, List<Point>>();

        static MapPage()
        {
            var doc = XDocument.Load(@"Maps/MapPagePoints.xml");
            foreach (var mapPage in doc.XPathSelectElements("//MapPage"))
            {
                var id = mapPage.Attribute("id").Value;
                var points = (from point in mapPage.Descendants("Point")
                              let x = Convert.ToInt32(point.Attribute("X").Value)
                              let y = Convert.ToInt32(point.Attribute("Y").Value)
                              select new Point(x, y)).ToList();
                MapPagePoints.Add(id, points);
            }
        }

        public MapPage(string mapPageId, string path)
        {
            MapPageId = mapPageId;

            var bitmap = new Bitmap(path);

            MapPageBitmap = new Bitmap(
                CellSize * MaxCellWidth,
                CellSize * MaxCellHeight);

            var rectSrc = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            var points = new[] {new Point(), new Point(), new Point(), new Point()};

            if (MapPagePoints.ContainsKey(mapPageId))
            {
                points = MapPagePoints[mapPageId].ToArray();

                var pointOuter = new Point(
                    Math.Min(points[0].X, points[3].X),
                    Math.Min(points[0].Y, points[1].Y));

                var sizeOuter = new Size(
                    Math.Max(points[1].X, points[2].X) - pointOuter.X,
                    Math.Max(points[2].Y, points[3].Y) - pointOuter.Y);

                rectSrc = new Rectangle(pointOuter, sizeOuter);
            }

            var srcBitmap = new Bitmap(MapPageBitmap.Width, MapPageBitmap.Height);

            using (var g = Graphics.FromImage(srcBitmap))
            {
                g.DrawImage(bitmap,
                    new Rectangle(0, 0, srcBitmap.Width, srcBitmap.Height),
                    rectSrc,
                    GraphicsUnit.Pixel);
            }

            var fastBitmapSrc = new FastBitmap(srcBitmap);

            var point0 = new Point(
                points[0].X > points[3].X ? points[3].X - points[0].X : 0,
                points[0].Y > points[1].Y ? points[1].Y - points[0].Y : 0);
            var point1 = new Point(
                MapPageBitmap.Width + (points[1].X < points[2].X ? points[2].X - points[1].X : 0),
                points[1].Y > points[0].Y ? points[0].Y - points[1].Y : 0);
            var point2 = new Point(
                MapPageBitmap.Width + (points[2].X < points[1].X ? points[1].X - points[2].X : 0),
                MapPageBitmap.Height + (points[2].Y < points[3].Y ? points[3].Y - points[2].Y : 0));
            var point3 = new Point(
                points[3].X > points[0].X ? points[0].X - points[3].X : 0,
                MapPageBitmap.Height + (points[3].Y < points[2].Y ? points[2].Y - points[3].Y : 0));

            QuadDistort.DrawBitmap(fastBitmapSrc,
                point0,
                point1,
                point2,
                point3,
                MapPageBitmap);
        }
        
        public FastBitmap MapPageBitmap { get; private set; }

        public string MapPageId { get; private set; }

    }
}