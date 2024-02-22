using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paint.Figures
{
    internal struct Rect : IFigure
    {
        public System.Drawing.Rectangle Area { get; set; }
        public bool IsFilled { get; set; }
        public Pen Pen { get; set; }
        public Brush Brush { get; set; }

        public object Clone()
        {
            return new Rect() { Area = Area };
        }

        public bool Contains(Point point) => Area.Contains(point);

        public void Draw(BufferedGraphics buffer)
        {
            if (IsFilled)
            {
                buffer.Graphics.FillRectangle(Brush, Area);
            }
            buffer.Graphics.DrawRectangle(Pen, Area);
        }
    }
}
