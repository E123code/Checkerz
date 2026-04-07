namespace CheckerZ
{
    internal class BoardLocation
    {
        public int Row {  get; set; }

        public int Col { get; set; }

        public bool isReversed { get; set; } = false;

        public BoardLocation(int row, int col)
        {
            this.Row = row;
            this.Col = col;
        }
        public override string ToString()
        {
            return $"[{Row},{Col},{isReversed}]";
        }
    }
}
