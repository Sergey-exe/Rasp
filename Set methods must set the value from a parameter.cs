public static void CreateObject()
{
    //Создание объекта на карте
}

public static void SetRandomChance()
{
    _chance = Random.Range(0, 100);
}

public static int CalculateSalary(int hoursWorked)
{
    return _hourlyRate * hoursWorked;
}
