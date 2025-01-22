public static int ClampNumber(int countKings, int countArchers, int countVehicles)
{
    if (countKings < countArchers)
        return countArchers;
    else if (countKings > countVehicles)
        return countVehicles;
    else
        return countKings;
}
