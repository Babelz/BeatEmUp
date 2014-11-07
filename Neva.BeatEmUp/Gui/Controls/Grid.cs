using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Neva.BeatEmUp.Gui.Controls.Components;

namespace Neva.BeatEmUp.Gui.Controls
{
    internal sealed class Grid : Container
    {
        #region Vars
        // Kontrollit joita ei voi lisätä gridiin koska solut puuttuvat.
        private readonly Dictionary<Point, List<Control>> cellControlLists;
        private readonly List<CellSize> rowSizeOverwrites;
        private readonly List<CellSize> columnSizeOverwrites;

        private Positioning positioning;
        private int currentRows;
        private int currentColumns;
        private GridCell[][] cells;

        private bool drawBorders;
        private int columns;
        private int rows;
        #endregion

        #region Properties
        public int Rows
        {
            get
            {
                return rows;
            }
            set
            {
                if (rows != value)
                {
                    rows = value;

                    UpdateCellLayout();
                }
            }
        }
        public int Columns
        {
            get
            {
                return columns;
            }
            set
            {
                if (columns != value)
                {
                    columns = value;

                    UpdateCellLayout();
                }
            }
        }
        /// <summary>
        /// Piirretäänkö solujen rajat.
        /// </summary>
        public bool DrawBorders
        {
            get
            {
                return drawBorders;
            }
            set
            {
                for (int i = 0; i < cells.Length; i++)
                {
                    for (int j = 0; j < cells[i].Length; j++)
                    {
                        cells[i][j].DrawBorders = value;
                    }
                }

                drawBorders = value;
            }
        }
        #endregion

        public Grid(Microsoft.Xna.Framework.Game game)
            : base(game)
        {
            positioning = Positioning.Absolute;

            cellControlLists = new Dictionary<Point, List<Control>>();
            rowSizeOverwrites = new List<CellSize>();
            columnSizeOverwrites = new List<CellSize>();

            rows = 2;
            columns = 2;

            UpdateCellLayout();
        }

        private float GetTotalOverwritePercent(List<CellSize> sizeOverwrites)
        {
            float value = 0.0f;

            for (int i = 0; i < sizeOverwrites.Count; i++)
            {
                value += sizeOverwrites[i].Percents;
            }

            return value;
        }
        private float GetTotalRowOverwritePercent()
        {
            return GetTotalOverwritePercent(rowSizeOverwrites);
        }
        private float GetTotalColumnOverwritePercent()
        {
            return GetTotalOverwritePercent(columnSizeOverwrites);
        }

        private CellSize GetRowOverwrite(int row)
        {
            return rowSizeOverwrites.FirstOrDefault(c => c.Index == row);
        }
        private CellSize GetColumnOverwrite(int column)
        {
            return columnSizeOverwrites.FirstOrDefault(c => c.Index == column);
        }

        // Palauttaa annetun rowin ja columin omaavan kontrolli listan.
        private List<Control> GetCellControlList(int row, int column)
        {
            Point key = new Point(column, row);
            List<Control> cellControlList = null;

            cellControlLists.TryGetValue(key, out cellControlList);

            if (cellControlList == null)
            {
                cellControlList = new List<Control>();
                cellControlLists.Add(key, cellControlList);
            }

            return cellControlList;
        }

        // Poistaa kontrollin solujen kontrolli listoista.
        private bool RemoveFromCellControlList(Control control)
        {
            List<Control> cellControlList = cellControlLists.FirstOrDefault(c => c.Value.Contains(control)).Value;

            return cellControlList.Remove(control);
        }

        // Palauttaa solun joka sisältää annetun kontrollin.
        private GridCell GetContainingCell(Control control)
        {
            for (int i = 0; i < cells.Length; i++)
            {
                for (int j = 0; j < cells[i].Length; j++)
                {
                    if (cells[i][j].Contains(control))
                    {
                        return cells[i][j];
                    }
                }
            }

            return null;
        }

        // Päivittää solujen layoutin.
        private void UpdateCellLayout()
        {
            positioning = Positioning.Absolute;

            // Suspendataan layout jotta vältytään rekursiiviselta ja 
            // jatkuvalta layoutin päivittämiseltä.
            SuspendLayout();

            UpdateCellStructure();
            UpdateCellSizes();
            UpdateCellPositions();

            ResumeLayout();

            positioning = Positioning.Relative;
        }

        // Päivittää solujen rakennetta.
        private void UpdateCellStructure()
        {
            if(cells == null)
            {
                InitializeCells();
            }
            
            if (rows < currentRows || columns < currentColumns)
            {
                DownScaleGrid();
            }
            else if (rows > currentRows || columns > currentColumns)
            {
                UpScaleGrid();
            }

            currentRows = cells.Length;
            currentColumns = cells.First().Length;
        }

        private void DownScaleGrid()
        {
            // Otetaan kaikki kontrollit talteen jotka eivät mahdu pienempään gridiin.
            for (int i = rows; i < currentRows; i--)
            {
                for (int j = columns; j < currentColumns; j--)
                {
                    GetCellControlList(i, j).AddRange(cells[i][j].Childs());
                }
            }
            
            // Luodaan uusi pienempi gridi ja kloonataan nykyisestä kaikki solut siihen.
            GridCell[][] newCells = new GridCell[rows][];

            for (int i = 0; i < rows; i++)
            {
                newCells[i] = new GridCell[columns];
            }
            for (int i = 0; i < rows; i++)
            {
                Array.Copy(cells[i], newCells[i], columns);
            }

            cells = newCells;
        }
        private void UpScaleGrid()
        {
            // Koko muuttunut, kopioidaan uuteen taulukkoon.
            GridCell[][] newCells = new GridCell[rows][];

            for (int i = 0; i < rows; i++)
            {
                newCells[i] = new GridCell[columns];
            }
            // Kopioidaan rivi kerrallaan.
            for (int i = 0; i < cells.Length; i++)
            {
                Array.Copy(cells[i], newCells[i], cells[i].Length);
            }

            int columnStart = 0;

            // Tehdään tyhjiin kohtiin uudet cellit.
            for (int i = 0; i < rows; i++)
            {
                columnStart = newCells[i][0] == null ? 0 : currentColumns;

                for (int j = columnStart; j < columns; j++)
                {
                    // Solun alustus.
                    newCells[i][j] = new GridCell(game, i, j);
                    newCells[i][j].DrawBorders = drawBorders;

                    // Haetaan kontrollilista ja lisätään kontrollit
                    // jotka se sisältää soluun.
                    List<Control> cellControlList = GetCellControlList(i, j);

                    if (cellControlList.Count > 0)
                    {
                        for (int h = 0; h < cellControlList.Count; h++)
                        {
                            cells[i][j].Add(cellControlList[h]);
                            cellControlList.RemoveAt(h);
                        }
                    }

                    base.Add(newCells[i][j]);
                }
            }

            cells = newCells;
        }

        private void InitializeCells()
        {
            if (cells == null)
            {
                cells = new GridCell[rows][];

                for (int i = 0; i < rows; i++)
                {
                    cells[i] = new GridCell[columns];
                }

                // Tehdään default cellit.
                for (int i = currentRows; i < rows; i++)
                {
                    for (int j = currentColumns; j < columns; j++)
                    {
                        cells[i][j] = new GridCell(game, i, j);
                        cells[i][j].DrawBorders = drawBorders;

                        base.Add(cells[i][j]);
                    }
                }
            }
        }

        private void UpdateCellSizes()
        {
            float width = 0.0f, height = 0.0f;
            float widthPercents = (100.0f - GetTotalColumnOverwritePercent()) / (currentColumns - columnSizeOverwrites.Count);
            float heightPercents = (100.0f - GetTotalRowOverwritePercent()) / (currentRows - rowSizeOverwrites.Count);

            for (int i = 0; i < cells.Length; i++)
            {
                CellSize rowOverwrite = GetRowOverwrite(i);

                for (int j = 0; j < cells[i].Length; j++)
                {
                    CellSize columnOverwrite = GetColumnOverwrite(j);

                    if (rowOverwrite != null)
                    {
                        height = rowOverwrite.Percents;
                    }
                    else
                    {
                        height = heightPercents;
                    }

                    if (columnOverwrite != null)
                    {
                        width = columnOverwrite.Percents;
                    }
                    else
                    {
                        width = widthPercents;
                    }

                    cells[i][j].Size = new Vector2(width, height);
                }
            }
        }
        private void UpdateCellPositions()
        {
            float x = 0.0f, y = 0.0f;

            for (int i = 0; i < cells.Length; i++)
            {
                for (int j = 0; j < cells[i].Length; j++)
                {
                    // Otetaan aloitus arvo (nurkka) tai lasketaan edellisestä rowista ja columista position.
                    x = j > 0 ? cells[i][j - 1].Area.Right : cells[i][j].Column * cells[i][j].SizeInPixels.X + Position.X;
                    y = i > 0 ? cells[i - 1][j].Area.Bottom : cells[i][j].Row * cells[i][j].SizeInPixels.Y + Position.Y;

                    cells[i][j].Position = new Vector2(x, y);
                }
            }
        }

        protected override Positioning GetChildPositioning()
        {
            return positioning;
        }

        /// <summary>
        /// Lisää uuden kontrollin gridiin. Asettaa sen default celliin (column 1, row 0)
        /// </summary>
        public override void Add(Control control)
        {
            if (control.GetType() == typeof(GridCell))
            {
                throw new InvalidGuiOperationException("Manually adding cells is not supported. Use column and row properties to add cells.");
            }

            Add(control, 0, 0);
        }
        /// <summary>
        /// Poistaa kontrollin gridistä.
        /// </summary>
        public override void Remove(Control control)
        {
            if (control.GetType() == typeof(GridCell))
            {
                throw new InvalidGuiOperationException("Manually removing cells is not supported. Use column and row properties to remove cells.");
            }

            if (!RemoveFromCellControlList(control))
            {
                GridCell gridCell = GetContainingCell(control);

                if (gridCell == null)
                {
                    throw new InvalidGuiOperationException("Grid dosent contain given control.");
                }

                gridCell.Remove(control);
            }
        }
        public override void UpdateLayout(GuiLayoutEventArgs guiLayoutEventArgs)
        {
            base.UpdateLayout(guiLayoutEventArgs);

            if (!LayoutSuspended)
            {
                UpdateCellLayout();
            }
        }

        /// <summary>
        /// Lisää kontrollin annetussa indeksissä sijaitsevaan soluun.
        /// </summary>
        public void Add(Control control, int row, int column)
        {
            GridCell gridCell = cells[row][column];
            
            if (gridCell == null)
            {
                List<Control> cellControlList = GetCellControlList(row, column);
                cellControlList.Add(control);
            }
            else 
            {
                gridCell.Add(control);
            }
        }
        /// <summary>
        /// Asettaa annetun kontrollin gridi sijainnin. Jos annettua solua ei ole olemassa,
        /// kontrolli lisätään gridiin vasta sittenkun solu löytyy. Kontrolli pitää lisätä add metodin kautta ennenkuin
        /// sen indeksi voidaan vaihtaa.
        /// </summary>
        /// <param name="control">Kontrolli jonka indeksi halutaan asettaa.</param>>
        public void ChangeCell(Control control, int row, int column)
        {
            List<Control> cellControlList = GetCellControlList(row, column);
            GridCell gridCell = cells[row][column];

            if (cellControlList.Contains(control))
            {
                cellControlList.Remove(control);
                gridCell.Add(control);
            }
            else if (gridCell != null)
            {
                GridCell currentCell = GetContainingCell(control);

                if(currentCell != null)
                {
                    currentCell.Remove(control);
                    gridCell.Add(control);
                }
            }
            else
            {
                throw new InvalidGuiOperationException("Control must be added before its index can be changed.");
            }
        }
        /// <summary>
        /// Asettaa halutun rowin koon.
        /// </summary>
        /// <param name="row">Rowi jonka koko halutaan muuttaa.</param>
        /// <param name="height">Prosentuaalinen korkeus.</param>
        public void SetRowHeight(int row, float height)
        {
            CellSize rowSize = GetRowOverwrite(row);

            if (rowSize != null)
            {
                throw new InvalidGuiOperationException("Grid already overwrites given row. Call reset before overwiriting row height.");
            }

            rowSizeOverwrites.Add(new CellSize(row, height));

            UpdateLayout(GuiLayoutEventArgs.Empty);
        }
        /// <summary>
        /// Resetoi halutun rowin korkeuden.
        /// </summary>
        /// <param name="row">Rowi jonka korkeus halutaan resetoida.</param>
        public void ResetRowHeight(int row)
        {
            CellSize rowSize = GetRowOverwrite(row);

            if (rowSize == null)
            {
                throw new InvalidGuiOperationException("Given row does not exist.");
            }

            rowSizeOverwrites.Remove(rowSize);

            UpdateLayout(GuiLayoutEventArgs.Empty);
        }
        /// <summary>
        /// Asettaa halutun columin koon
        /// </summary>
        /// <param name="column">Columi jonka koko halutaan muuttaa.</param>
        /// <param name="width">Prosentuaalinen leveys.</param>
        public void SetColumnWidth(int column, float width)
        {
            CellSize columnSize = GetColumnOverwrite(column);

            if (columnSize != null)
            {
                throw new InvalidGuiOperationException("Grid already overwrites given column. Call reset before overwriting column width.");
            }

            columnSizeOverwrites.Add(new CellSize(column, width));

            UpdateLayout(GuiLayoutEventArgs.Empty);
        }
        /// <summary>
        /// Resetoi halutun columin leveyden.
        /// </summary>
        /// <param name="column">Columi jonka leveys halutaan resetoida.</param>
        public void ResetColumnWidth(int column)
        {
            CellSize columnSize = GetColumnOverwrite(column);

            if (columnSize == null)
            {
                throw new InvalidGuiOperationException("Given column does not exist.");
            }

            columnSizeOverwrites.Remove(columnSize);

            UpdateLayout(GuiLayoutEventArgs.Empty);
        }
    }
}
