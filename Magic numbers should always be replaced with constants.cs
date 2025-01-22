class Weapon
{
    private const int MinCountBullets = 0;

    private int _countBulletsInShoot = 1;
    private int _countBullets;

    public Weapon(int countBullets)
    {
        _countBullets = countBullets;

        if (IsCorrectCountBullets)
            throw new ArgumentOutOfRangeException(nameof(countBullets));
    }

    public bool CanShoot => _countBullets > MinCountBullets;
    private bool IsCorrectCountBullets => _countBullets >= MinCountBullets;

    public void Shoot()
    {
        if (IsCorrectCountBullets)
            throw new InvalidOperationException();

        _countBullets -= _countBulletsInShoot;
    }
}
