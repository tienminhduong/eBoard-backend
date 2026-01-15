using eBoardAPI.Entities;

namespace eBoardAPI.Helpers;

public static class StringHelper
{
    public static string ParseAcademicYear(DateOnly startDate, DateOnly endDate) => $"{startDate.Year}-{endDate.Year}";
    public static string ParseAcademicYear(Class @class) => ParseAcademicYear(@class.StartDate, @class.EndDate);
}