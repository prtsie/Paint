using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paint.Figures
{
    internal struct Circle : IFigure
    {
        private Rectangle area;
        public Rectangle Area
        {
            get => area;
            set
            {
                if (value.Width != value.Height)
                {
                    var max = value.Width > value.Height ? value.Width : value.Height;
                    value.Width = max;
                    value.Height = max;
                }
                area = value;
            }
        }

        public bool IsFilled { get; set; }
        public Pen Pen { get; set; }
        public Brush Brush { get; set; }

        public void Draw(BufferedGraphics buffer)
        {
            if (IsFilled)
            {
                buffer.Graphics.FillEllipse(Brush, Area);
            }
            buffer.Graphics.DrawEllipse(Pen, Area);
        }

        public bool Contains(Point point)
        {
            var radius = Area.Height / 2;
            var center = new Point(Area.X + radius, Area.Y + radius);
            return Math.Pow(center.X - point.X, 2) + Math.Pow(center.Y - point.Y, 2) <= Math.Pow(radius, 2);
        }

        public object Clone()
        {
            return new Circle() { Area = Area };
        }
    }
}
