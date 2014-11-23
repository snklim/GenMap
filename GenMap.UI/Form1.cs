using System;
using System.Drawing;
using System.Windows.Forms;

namespace GenMap.UI
{
    public partial class Form1 : Form
    {
        private const int CellSize = 50;

        private readonly Map _map = new Map();

        public Form1()
        {
            InitializeComponent();

            SetMapBox();

            var shiftX = 0;
            var shiftY = 0;

            boxMap.Resize += (sender, args) => boxMap.Invalidate();

            boxMap.Paint += (sender, args) => _map.DrawMap(args.Graphics, shiftX, shiftY);

            var mouseDownPoint = new Point();
            var mouseDown = false;

            boxMap.MouseDown += (sender, args) =>
            {
                mouseDownPoint.X = args.X - shiftX;
                mouseDownPoint.Y = args.Y - shiftY;

                mouseDown = true;
            };

            boxMap.MouseUp += (sender, args) =>
            {
                mouseDown = false;
            };

            boxMap.MouseMove += (sender, args) =>
            {
                if (mouseDown)
                {
                    shiftX = args.X - mouseDownPoint.X;
                    shiftY = args.Y - mouseDownPoint.Y;

                    
                    while (shiftX <= -CellSize)
                    {
                        shiftX += CellSize;
                        mouseDownPoint.X = args.X - shiftX;

                        _map.TurnLeft();
                    }

                    
                    while (shiftY <= -CellSize)
                    {
                        shiftY += CellSize;
                        mouseDownPoint.Y = args.Y - shiftY;

                        _map.TurnUp();
                    }


                    while (shiftX >= CellSize)
                    {
                        shiftX -= CellSize;
                        mouseDownPoint.X = args.X - shiftX;

                        _map.TurnRight();
                    }

                    while (shiftY >= CellSize)
                    {
                        shiftY -= CellSize;
                        mouseDownPoint.Y = args.Y - shiftY;

                        _map.TurnBottom();
                    }

                    boxMap.Invalidate();
                }
            };
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e); 
            SetMapBox();
        }

        private void SetMapBox()
        {
            int boxWidth = CellSize*(panelMap.Width/CellSize);
            int boxHeight = CellSize*(panelMap.Height/CellSize);

            boxMap.Left = (panelMap.Width - boxWidth) / 2;
            boxMap.Top = (panelMap.Height - boxHeight) / 2;

            boxMap.Width = boxWidth;
            boxMap.Height = boxHeight;

            _map.SetViewPort(boxWidth, boxHeight);
        }
    }
}
