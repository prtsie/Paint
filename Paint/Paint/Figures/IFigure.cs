using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paint.Figures
{
    internal interface IFigure : ICloneable
    {
        Pen Pen { get; set; }
        Brush Brush { get; set; }
        bool IsFilled { get; set; }
        Rectangle Area { get; set; }

        public void Draw(Graphics g);

        public bool Contains(Point point);
    }
}
