using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace ColdSnap.Controls
{
	public partial class ProgressGlobe
	{
		/// <summary>
		/// class constructor
		/// </summary>
		public ProgressGlobe()
		{
			InitializeComponent();
			Value = 0;
		}

		/// <summary>
		/// Set the color for the total pie
		/// </summary>
		public SolidColorBrush ProgressBackground
		{
			set { BackgroundEllipse.Fill = value; }
		}

		/// <summary>
		/// Set the color for the hole
		/// </summary>
		public SolidColorBrush HoleBackground
		{
			set { CentralEllipse.Fill = value; }
		}

		/// <summary>
		/// Set the color for the fill for the sector
		/// </summary>
		public SolidColorBrush ProgressFill
		{
			set
			{
				ProgressPath.Fill = value;
				ProgressPath.Stroke = value;
			}
		}

		/// <summary>
		/// The value for this pie
		/// </summary>
		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(ProgressGlobe), null);
		public double Value
		{
			get { return (double)GetValue(ValueProperty); }
			set
			{
				value = Math.Truncate(value * 100) / 100;
				if (value > 1) value = 1; else if (value < 0) value = 0;

				SetValue(ValueProperty, value);

				DrawSector(value);
			}
		}

		/// <summary>
		/// this method draws the sector based on the value
		/// </summary>
		/// <param name="value"></param>
		private void DrawSector(double value)
		{
			ProgressPath.SetValue(Path.DataProperty, null);
			var pg = new PathGeometry();
			var fig = new PathFigure();
			if (Math.Abs(value) < 0.1) return;

			var height = ActualHeight - 2;
			var width = ActualWidth - 2;
			var radius = height / 2;
			var theta = (360 * value) - 90;
			var xC = radius;
			var yC = radius;

			if (Math.Abs(value - 1) < 0.1) theta += 1;

			var finalPointX = xC + (radius * Math.Cos(theta * 0.0174));
			var finalPointY = yC + (radius * Math.Sin(theta * 0.0174));

			fig.StartPoint = new Point(radius, radius);
			var firstLine = new LineSegment {Point = new Point(radius, 0)};
			fig.Segments.Add(firstLine);

			if (value > 0.25)
			{
				var firstQuart = new ArcSegment
				{
					Point = new Point(width, radius),
					SweepDirection = SweepDirection.Clockwise,
					Size = new Size(radius, radius)
				};
				fig.Segments.Add(firstQuart);
			}

			if (value > 0.5)
			{
				var secondQuart = new ArcSegment
				{
					Point = new Point(radius, height),
					SweepDirection = SweepDirection.Clockwise,
					Size = new Size(radius, radius)
				};
				fig.Segments.Add(secondQuart);
			}

			if (value > 0.75)
			{
				var thirdQuart = new ArcSegment
				{
					Point = new Point(0, radius),
					SweepDirection = SweepDirection.Clockwise,
					Size = new Size(radius, radius)
				};
				fig.Segments.Add(thirdQuart);
			}

			var finalQuart = new ArcSegment
			{
				Point = new Point(finalPointX, finalPointY),
				SweepDirection = SweepDirection.Clockwise,
				Size = new Size(radius, radius)
			};
			fig.Segments.Add(finalQuart);

			var lastLine = new LineSegment {Point = new Point(radius, radius)};
			fig.Segments.Add(lastLine);
			pg.Figures.Add(fig);
			ProgressPath.SetValue(Path.DataProperty, pg);
		}
	}
}
