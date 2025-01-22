public static int FindElement(int[] array, int element)
{
    if (array == null)
        throw new ArgumentNullException();

    if (element < 0)
        throw new ArgumentOutOfRangeException();

    for (int i = 0; i < array.Length; i++)
        if (array[i] == element)
            return i;

    return -1;
}
