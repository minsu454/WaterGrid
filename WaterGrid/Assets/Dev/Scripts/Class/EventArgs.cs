namespace Common.Event.Args
{
    public static class EventArgs
    {
        public class ButtonArgs
        {
            public UIButtonType type;
            public int count;

            public ButtonArgs(UIButtonType type, int count)
            {
                this.type = type;
                this.count = count;
            }
        }
    }
}