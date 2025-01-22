    public static int ClampNumber(int speed, int time, int distance)
    {
        if (speed < time)
            return time;
        else if (speed > distance)
            return distance;
        else
            return speed;
    }
