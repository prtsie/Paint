using Paint.Figures;

namespace Paint
{
    public partial class PaintForm : Form
    {
        [Flags]
        private enum ResizeSquares
        {
            Top = 1,
            Left = 2,
            Bottom = 4,
            Right = 8
        }
        private readonly Dictionary<ResizeSquares, Rectangle> resizeSquares = new()
        {
            {ResizeSquares.Top | ResizeSquares.Left, new Rectangle() },
            {ResizeSquares.Top | ResizeSquares.Right, new Rectangle() },
            {ResizeSquares.Bottom | ResizeSquares.Left, new Rectangle() },
            {ResizeSquares.Bottom | ResizeSquares.Right, new Rectangle() }
        };
        private readonly Dictionary<ResizeSquares, Cursor> resizingCursors = new()
        {
            {ResizeSquares.Top | ResizeSquares.Left, Cursors.SizeNWSE },
            {ResizeSquares.Top | ResizeSquares.Right, Cursors.SizeNESW },
            {ResizeSquares.Bottom | ResizeSquares.Left, Cursors.SizeNESW },
            {ResizeSquares.Bottom | ResizeSquares.Right, Cursors.SizeNWSE }
        };
        private readonly Dictionary<string, IFigure> figuresNames = new()
        {
            { "Прямоугольник", new Rect() },
            {"Круг", new Circle()},
            {"Эллипс",  new Ellipse()}
        };
        private readonly Graphics graphics;
        private readonly List<IFigure> drawn = new();
        private readonly int selectionBoxPadding = 1;
        private readonly Pen selectionBoxPen = (Pens.Black.Clone() as Pen)!;
        private readonly Pen resizeSquarePen = Pens.Chocolate;
        private int selectedIndex = -1;
        private Point mouseDownCoords;
        private Pen pen = (Pens.Black.Clone() as Pen)!;
        private Brush brush = Brushes.Black;
        private bool toFill;
        private bool isDrawing;
        private bool isMoving;
        private bool isResizing;
        private const int ResizeSquareSize = 8;

        private readonly BufferedGraphicsContext currentContext = BufferedGraphicsManager.Current;
        private readonly BufferedGraphics buffer;

        public PaintForm()
        {
            InitializeComponent();
            pen.Width = 3;
            selectionBoxPadding += (int)pen.Width;
            selectionBoxPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            toolStripFigures.Items.AddRange(figuresNames.Keys.ToArray());
            toolStripFigures.SelectedIndex = 0;
            graphics = CreateGraphics();
            buffer = currentContext.Allocate(graphics, DisplayRectangle);
            buffer.Graphics.Clear(BackColor);
            buffer.Render();
        }

        private void SetResizeSquares(Rectangle selectionBox)
        {
            var offset = new Point(ResizeSquareSize / 2, ResizeSquareSize / 2);
            var size = new Size(ResizeSquareSize, ResizeSquareSize);
            var point = CalculateOffset(selectionBox.Location, offset);
            resizeSquares[ResizeSquares.Top | ResizeSquares.Left] = new Rectangle(point, size);
            point.X += selectionBox.Width;
            resizeSquares[ResizeSquares.Top | ResizeSquares.Right] = new Rectangle(point, size);
            point.Y += selectionBox.Height;
            resizeSquares[ResizeSquares.Bottom | ResizeSquares.Right] = new Rectangle(point, size);
            point.X -= selectionBox.Width;
            resizeSquares[ResizeSquares.Bottom | ResizeSquares.Left] = new Rectangle(point, size);
        }

        private Rectangle CalculateResizedArea(Point resizePosition)
        {
            var square = resizeSquares.Values.Last(square => square.Contains(mouseDownCoords));
            var key = resizeSquares.Keys.First(key => resizeSquares[key] == square);
            var area = drawn[selectedIndex].Area;
            if ((key & ResizeSquares.Left) != 0)
            {
                area.Width += area.X - resizePosition.X;
                area.X = resizePosition.X;
            }
            else
            {
                area.Width = resizePosition.X - area.X;
            }
            if ((key & ResizeSquares.Top) != 0)
            {
                area.Height += area.Y - resizePosition.Y;
                area.Y = resizePosition.Y;
            }
            else
            {
                area.Height = resizePosition.Y - area.Y;
            }
            if (area.Width <= 0 || area.Height <= 0)
            {
                area = Rectangle.Empty;
            }
            return area;
        }

        private Rectangle CalculateMovedArea(Point movedPosition)
        {
            var selectedArea = drawn[selectedIndex].Area;
            var selectedCursorOffset = CalculateOffset(mouseDownCoords, selectedArea.Location);
            var leftUp = CalculateOffset(movedPosition, selectedCursorOffset);
            return new Rectangle(leftUp, selectedArea.Size);
        }

        private static Point CalculateOffset(Point first, Point second)
        {
            return new Point(first.X - second.X, first.Y - second.Y);
        }

        private void VisualiseDrawing(Rectangle area)
        {
            Redraw();
            IFigure clone;
            if (isDrawing)
            {
                clone = (IFigure)figuresNames[toolStripFigures.SelectedItem!.ToString()!].Clone();
            }
            else
            {
                clone = (IFigure)drawn[selectedIndex].Clone();
            }
            clone.Pen = selectionBoxPen;
            clone.Area = area;
            clone.IsFilled = false;
            clone.Draw(buffer);
            buffer.Render();
        }

        private void DrawSelectionBox(Rectangle area)
        {
            var selectionBox = new Rectangle(
                    area.Left - selectionBoxPadding,
                    area.Top - selectionBoxPadding,
                    area.Width + selectionBoxPadding * 2,
                    area.Height + selectionBoxPadding * 2);
            graphics.DrawRectangle(selectionBoxPen, selectionBox);
            SetResizeSquares(selectionBox);
            graphics.DrawRectangles(resizeSquarePen, resizeSquares.Values.ToArray());
            graphics.FillRectangles(resizeSquarePen.Brush, resizeSquares.Values.ToArray());
        }

        private static Rectangle RectFromPoints(Point first, Point second)
        {
            var (LeftUp, RightBottom) = SortPoints(first, second);
            return new Rectangle(LeftUp, new(RightBottom.X - LeftUp.X, RightBottom.Y - LeftUp.Y));
        }

        private Color? RequestColor()
        {
            if (colorDialog.ShowDialog() == DialogResult.Cancel)
            {
                return null;
            }
            return colorDialog.Color;
        }

        private void Select(IFigure figure)
        {
            selectedIndex = drawn.LastIndexOf(figure);
            if (selectedIndex != -1)
            {
                toolStripDeleteButton.Enabled = true;
                toFill = figure.IsFilled;
                Redraw();
                DrawSelectionBox(drawn[selectedIndex].Area);
            }
        }

        private void Deselect()
        {
            selectedIndex = -1;
            toolStripDeleteButton.Enabled = false;
        }

        private void DrawAll()
        {
            foreach (var figure in drawn)
            {
                figure.Draw(buffer);
            }
            buffer.Render();
        }

        private void Redraw()
        {
            buffer.Graphics.Clear(BackColor);
            DrawAll();
        }

        private static (Point LeftUp, Point RightBottom) SortPoints(Point first, Point second)
        {
            Point leftUp = new();
            Point rightBottom = new();
            if (first.X < second.X)
            {
                leftUp.X = first.X;
                rightBottom.X = second.X;
            }
            else
            {
                leftUp.X = second.X;
                rightBottom.X = first.X;
            }
            if (first.Y < second.Y)
            {
                leftUp.Y = first.Y;
                rightBottom.Y = second.Y;
            }
            else
            {
                leftUp.Y = second.Y;
                rightBottom.Y = first.Y;
            }
            return (leftUp, rightBottom);
        }

        private void PaintForm_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDownCoords = e.Location;
            if (selectedIndex != -1
                && drawn.LastOrDefault(figure => figure.Contains(mouseDownCoords)) is IFigure toMove)
            {
                isMoving = true;
            }
            else if (resizeSquares.Values.LastOrDefault(square => square.Contains(mouseDownCoords)) != Rectangle.Empty)
            {
                isResizing = true;
            }
            else
            {
                isDrawing = true;
            }
        }

        private void PaintForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Location == mouseDownCoords
                && drawn.LastOrDefault(figure => figure.Contains(mouseDownCoords)) is IFigure toSelect)
            {
                Select(toSelect);
            }
            else if (isMoving)
            {
                drawn[selectedIndex].Area = CalculateMovedArea(e.Location);
                Select(drawn[selectedIndex]);
            }
            else if (isResizing)
            {
                drawn[selectedIndex].Area = CalculateResizedArea(e.Location);
                Select(drawn[selectedIndex]);
            }
            else if (isDrawing)
            {
                Deselect();
                var rect = RectFromPoints(e.Location, mouseDownCoords);
                var figure = (IFigure)figuresNames[toolStripFigures.SelectedItem!.ToString()!].Clone();
                figure.Area = rect;
                figure.IsFilled = toFill;
                figure.Pen = pen;
                figure.Brush = brush;
                drawn.Add((IFigure)figure);
                Redraw();
            }
            isDrawing = false;
            isMoving = false;
            isResizing = false;
        }

        private void toolStripClearButton_Click(object sender, EventArgs e)
        {
            drawn.Clear();
            graphics.Clear(BackColor);
        }

        private void PaintForm_Paint(object sender, PaintEventArgs e)
        {
            DrawAll();
        }

        private void toolStripDeleteButton_Click(object sender, EventArgs e)
        {
            drawn.RemoveAt(selectedIndex);
            toolStripDeleteButton.Enabled = false;
            selectedIndex = -1;
            Redraw();
        }

        private void toolStripButtontoFill_Click(object sender, EventArgs e)
        {
            toFill = !toFill;
            if (selectedIndex != -1 && drawn[selectedIndex].IsFilled != toFill)
            {
                drawn[selectedIndex].IsFilled = toFill;
                Redraw();
            }
        }

        private void toolStripFillColorButton_Click(object sender, EventArgs e)
        {
            var color = RequestColor();
            if (color is not null)
            {
                brush = new SolidBrush(color.Value);
                if (selectedIndex != -1)
                {
                    drawn[selectedIndex].Brush = brush;
                }
            }
        }

        private void toolStripDrawColorButton_Click(object sender, EventArgs e)
        {
            var color = RequestColor();
            if (color is not null)
            {
                pen = new Pen(color.Value) { Width = pen.Width };
                if (selectedIndex != -1)
                {
                    drawn[selectedIndex].Pen = pen;
                }
            }

        }

        private Cursor GetResizingCursor(Rectangle square)
        {
            var key = resizeSquares.Keys.First(key => resizeSquares[key] == square);
            return resizingCursors[key];
        }

        private void PaintForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (selectedIndex != -1 && drawn[selectedIndex].Contains(e.Location))
            {
                Cursor.Current = Cursors.SizeAll;
            }
            else if (resizeSquares.Values.LastOrDefault(square => square.Contains(e.Location)) != Rectangle.Empty)
            {
                var square = resizeSquares.Values.LastOrDefault(square => square.Contains(e.Location));
                Cursor.Current = GetResizingCursor(square);
            }
            else if (drawn.LastOrDefault(figure => figure.Contains(e.Location)) is not null
                     || resizeSquares.Values.LastOrDefault(square => square.Contains(e.Location)) != Rectangle.Empty)
            {
                Cursor.Current = Cursors.Hand;
            }
            else
            {
                Cursor.Current = Cursors.Default;
            }
            if (isDrawing)
            {
                VisualiseDrawing(RectFromPoints(mouseDownCoords, e.Location));
            }
            else if (isMoving)
            {
                Cursor.Current = Cursors.SizeAll;
                VisualiseDrawing(CalculateMovedArea(e.Location));
            }
            else if (isResizing)
            {
                var square = resizeSquares.Values.LastOrDefault(square => square.Contains(mouseDownCoords));
                Cursor.Current = GetResizingCursor(square);
                VisualiseDrawing(CalculateResizedArea(e.Location));
            }
        }
    }
}
