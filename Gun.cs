class Weapon
{
    private List<Bullet> _bullets;

    public void Fire()
    {
        if (_bullets.Count == 0)
            return;


        _bullets[0].Move();
        _bullets.RemoveAt(0);
    }
}

class Bullet
{
    private readonly int _damage;

    public void Move() { }

    public void Collision(Player other)
    {
        other.TakeDamage(_damage);
    }
}

class Player
{
    private int _health;

    public void TakeDamage(int damage) 
    { 
        if (damage < 0)
            damage = 0;

        _health -= damage;

        if (_health < 0)
            _health = 0;
    }
}

class Bot
{
    private readonly Weapon _weapon;

    public void OnSeePlayer()
    {
        _weapon.Fire();
    }
}
