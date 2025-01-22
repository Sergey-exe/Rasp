class Player
{
    private readonly string _name;
    private readonly int _age;

    private Mover _mover;
    private Shooter _shooter;

    public Player(string name, int age)
    {
        _mover = new Mover(5, -5, 15);
        _shooter = new Shooter(0.5f, 25);

        _name = name;
        _age = age;
    }

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
        if (movementDirectionX <= 0)
            throw new ArgumentOutOfRangeException();

        if (movementDirectionY <= 0)
            throw new ArgumentOutOfRangeException();

        if (movementSpeed < 0)
            throw new ArgumentOutOfRangeException();

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
