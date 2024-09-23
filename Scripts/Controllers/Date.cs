using System.Collections.Generic;
using Godot;

namespace GameDevInc;

public class Date
{
    public int Week { get; private set; } = 1;
    public int Month { get; private set; } = 1;
    public int Year { get; private set; } = 1980;

    public Date(int week, int month, int year)
    {
        Week = week;
        Month = month;
        Year = year;
    }   

    public static string GetMonthName(int month)
    {
        return month switch
        {
            1 => "January",
            2 => "February",
            3 => "March",
            4 => "April",
            5 => "May",
            6 => "June",
            7 => "July",
            8 => "August",
            9 => "September",
            10 => "October",
            11 => "November",
            12 => "December",
            _ => "Error"
        };
    }

    public static string GetMonthNameShort(int month)
    {
        return month switch
        {
            1 => "Jan",
            2 => "Feb",
            3 => "Mar",
            4 => "Apr",
            5 => "May",
            6 => "Jun",
            7 => "Jul",
            8 => "Aug",
            9 => "Sept",
            10 => "Oct",
            11 => "Nov",
            12 => "Dec",
            _ => "Error"
        };
    }
}