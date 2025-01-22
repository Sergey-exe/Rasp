class Weapon
{
    private const int MinCountBullets = 0;
    private const int CountBulletsInShoot = 1;

    private int _countBullets;

    public Weapon(int countBullets)
    {
        if (countBullets < MinCountBullets)
            throw new ArgumentOutOfRangeException(nameof(countBullets));

        _countBullets = countBullets;
    }

    public bool CanShoot => _countBullets > MinCountBullets;

    public void Shoot()
    {
        if (_countBullets < MinCountBullets)
            throw new InvalidOperationException();

        _countBullets -= CountBulletsInShoot;
    }
}
