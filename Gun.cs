using System;

class Weapon
{
    private readonly int _damage;
    private int _countBullets;

    public void Fire(Player player)
    {
        player.TakeDamage(_damage);
        _countBullets--;
    }
}

class Player
{
    private int _health;

    public void TakeDamage(int damage)
    {
        damage = Math.Max(0, damage);

        _health -= damage;
        _health = Math.Max(0, _health);
    }
}

class Bot
{
    private readonly Weapon _weapon;

    public void OnSeePlayer(Player player)
    {
        _weapon.Fire(player);
    }
}
