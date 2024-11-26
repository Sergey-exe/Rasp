using System;


class Weapon
{
    private readonly int _damage;
    private int _countBullets;

    public Weapon(int damage, int countBullets)
    {
        if (damage < 0 && countBullets < 0)
            throw new ArgumentOutOfRangeException();
            
        _damage = damage;
        _countBullets = countBullets;
    }

    public void Fire(Player player)
    {
        if (_countBullets == 0)
            throw new ArgumentOutOfRangeException();

        player.TakeDamage(_damage);
        _countBullets--;
    }
}

class Player
{
    private int _health;

    public Player(int health)
    {
        if (health <= 0)
            throw new ArgumentOutOfRangeException();

        _health = health;
    }
    
    public void TakeDamage(int damage)
    {
        if(damage < 0)
            throw new ArgumentOutOfRangeException();

        _health -= damage;

        if (_health < 0)
            throw new ArgumentOutOfRangeException();
    }
}

class Bot
{
    private readonly Weapon _weapon;

    public Bot(Weapon weapon)
    {
        if(weapon == null) 
            throw new ArgumentNullException();

        _weapon = weapon;
    }

    public void OnSeePlayer(Player player)
    {
        if (player == null)
            throw new ArgumentNullException();

        _weapon.Fire(player);
    }
}
