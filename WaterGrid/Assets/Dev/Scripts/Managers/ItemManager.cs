using Common.Event;
using static Common.Event.Args.EventArgs;

public sealed class ItemManager
{
    private int pump = 1;
    public int Pump
    {
        get { return pump; }
    }

    private int hammer = 2;
    public int Hammer
    {
        get { return hammer; }
    }

    public ItemManager(int pump, int hammer)
    {
        this.pump = pump;
        this.hammer = hammer;
    }

    public void Upgrade(ItemType type, int count)
    {
        ButtonArgs args = default;

        switch (type)
        {
            case ItemType.Hammer:
                hammer += count;
                args = new ButtonArgs(UIButtonType.HammerBtn, hammer);
                break;
            case ItemType.Pump:
                pump += count;
                args = new ButtonArgs(UIButtonType.PumpBtn, pump);
                break;
        }

        EventManager.Dispatch(GameEventType.ButtonEvent, args);
    }
}