public void Activate()
{
    _enable = true;

    _effects.StartEnableAnimation();
}

public void Deactivate()
{
    _enable = false;

    _pool.Free(this);
}
