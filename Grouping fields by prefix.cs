class Player
{
    private Mover _mover;
    private Shooter _shooter;

    public Player(Mover mover, Shooter shooter, string name, int age) 
    {
        if (mover == null)
            throw new ArgumentNullException();

        if (shooter == null)
            throw new ArgumentNullException();

        if (age < 0)
            throw new ArgumentOutOfRangeException();

        _mover = mover;
        _shooter = shooter;
        Name = name;
        Age = age;
    }

    public string Name { get; private set; }

    public int Age { get; private set; }

    private void Move()
    {
        _mover.Move();
    }

    private void Attack()
    {
        _shooter.Attack();
    }
}

public class Mover
{
    public Mover(float movementDirectionX, float movementDirectionY, float movementSpeed)
    {
        MovementDirectionX = movementDirectionX;
        MovementDirectionY = movementDirectionY;
        MovementSpeed = movementSpeed;
    }

    public float MovementDirectionX { get; private set; }

    public float MovementDirectionY { get; private set; }

    public float MovementSpeed { get; private set; }

    public void Move()
    {
        //Do move
    }
}

public class Shooter
{
    public Shooter(float weaponCooldown, int weaponDamage)
    {
        if (weaponCooldown < 0)
            throw new ArgumentOutOfRangeException();

        if (weaponDamage < 0)
            throw new ArgumentOutOfRangeException();

        WeaponDamage = weaponDamage;
        WeaponCooldown = weaponCooldown;
    }

    public float WeaponCooldown { get; private set; }

    public int WeaponDamage { get; private set; }

    public void Attack()
    {
        //attack
    }

    public bool IsReloading()
    {
        throw new NotImplementedException();
    }
}
