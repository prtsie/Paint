using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paint.Figures
{
    internal struct Ellipse : IFigure
    {
        public Rectangle Area { get; set; }

        public bool Contains(Point point)
        {
            // x^2 / a^2 + y^2 / b^2 = 1
            var a = Area.Width / 2;
            var b = Area.Height / 2;
            var relativeX = point.X - (Area.X + a);
            var relativeY = point.Y - (Area.Y + b);
            return Math.Pow(relativeX, 2) / Math.Pow(a, 2) + Math.Pow(relativeY, 2) / Math.Pow(b, 2) <= 1;
        }

        public bool IsFilled { get; set; }
        public Pen Pen { get; set; }
        public Brush Brush { get; set; }

        public void Draw(Graphics g)
        {
            if (IsFilled)
            {
                g.FillEllipse(Brush, Area);
            }
            g.DrawEllipse(Pen, Area);
        }

        public object Clone()
        {
            return new Ellipse() { Area = Area };
        }
    }
}
