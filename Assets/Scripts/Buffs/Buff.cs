using System;

public abstract class Buff
{
    protected bool harmful = false;
    protected Random random = new Random();

    public bool IsHarmful()
    {
        return harmful;
    }

    public abstract void Update(UnitStats stats);
}