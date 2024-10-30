namespace AntwarConsoleProgram
{
    public abstract class AntBase
    {
        private Map Map;
        public int Index;

        public int DX { set; get; }
        public int DY { set; get; }
        public bool WithFood { set; get; }

        public void Attach(Map map, int index)
        {
            Map = map;
            Index = index;
        }

        public String Name { get; set; } = String.Empty;

        abstract public void Move(ScopeData scope, List<AntBase> mates);


        private void Move_Delta(int dx, int dy, bool with_food = false)
        {
            DX = dx;
            DY = dy;
            WithFood = with_food;
        }

        public void North(bool with_food = false)
        {
            Move_Delta(0, -1, with_food);
        }
        public void South(bool with_food = false)
        {
            Move_Delta(0, 1, with_food);
        }
        public void West(bool with_food = false)
        {
            Move_Delta(-1, 0, with_food);
        }
        public void East(bool with_food = false)
        {
            Move_Delta(1, 0, with_food);
        }
    }
}
