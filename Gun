class Weapon
{
    private int _damage;
    private int _countBullets;

    public void Fire(Player player)
    {
        if (_countBullets == 0)
            return;


        _countBullets--;
        player.TakeDamage(_damage);
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
    private Weapon _weapon;

    public void OnSeePlayer(Player player)
    {
        _weapon.Fire(player);
    }
}
