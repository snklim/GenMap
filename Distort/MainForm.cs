using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace Distort
{
	public class MainForm : Form
	{
		#region The main entry point for the application.
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
		#endregion

		FastBitmap oTexture;
		Point[] Points;
		Rectangle oRect = new Rectangle();
		int nPoint = -1;
		public MainForm()
		{
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			LoadImage("..\\..\\CP.gif");
		}

		private void LoadImage(String sPath) 
		{
			oTexture = new FastBitmap(sPath);
			Points = new Point[]
			{
				new Point(5,5),
				new Point(oTexture.Width+5, 5),
				new Point(oTexture.Width+5, oTexture.Height+5),
				new Point(5, oTexture.Height+5)
			};
		}
		protected override void OnPaint(PaintEventArgs e)
		{
			FastBitmap oOut = new FastBitmap(this.ClientSize);
			
			Stopwatch oClock1 = new Stopwatch();
			oClock1.Start();
			QuadDistort.DrawBitmap(oTexture, Points[0], Points[1], Points[2], Points[3], oOut);
			oClock1.Stop();
			
			Trace.WriteLine("Time:" + oClock1.ElapsedMilliseconds);

			e.Graphics.Clear(Color.White);
			e.Graphics.DrawImageUnscaled(oOut,Point.Empty);
			if (nPoint >= 0)
			{
				e.Graphics.DrawRectangle(Pens.Green, oRect);
			}
		}

		#region mouse handling and point dragging
		bool bDraging;
		protected override void OnMouseDown(MouseEventArgs e)
		{
			if (nPoint >= 0)
			{
				bDraging = true;
			}
		}
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			bDraging = false;
		}


		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			Point mouse = e.Location;
			if (bDraging)
			{
				Points[nPoint] = mouse;
				Point p = Points[nPoint];
				oRect = new Rectangle(p.X - 5, p.Y - 5, 10, 10);
			}
			else
			{
				nPoint = -1;
				for (int i = 0; i < Points.Length; i++)
				{
					Point p = Points[i];
					oRect = new Rectangle(p.X - 5, p.Y - 5, 10, 10);
					if (oRect.Contains(mouse))
					{
						nPoint = i;
						break;
					}
				}
			}
			if (nPoint >= 0)
				this.Refresh();
		}
		#endregion


	}
}
