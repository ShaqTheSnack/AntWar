namespace AntEngine
{
    public abstract class AntBase
    {
        public int Index { set; get; }
        public int DX { set; get; }
        public int DY { set; get; }
        public bool WithFood { set; get; }

        public String Name { get; set; } = String.Empty;

        private void Move_Delta(int dx, int dy, bool with_food = false)
        {
            DX = dx;
            DY = dy;
            WithFood = with_food;
        }

        abstract public void Move(ScopeData scope, List<AntBase> mates);


        public virtual void North(bool with_food = false)
        {
            Move_Delta(0, -1, with_food);
        }
        public virtual void South(bool with_food = false)
        {
            Move_Delta(0, 1, with_food);
        }
        public virtual void West(bool with_food = false)
        {
            Move_Delta(-1, 0, with_food);
        }
        public virtual void East(bool with_food = false)
        {
            Move_Delta(1, 0, with_food);
        }
    }
}
