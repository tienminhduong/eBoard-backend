using eBoardAPI.Consts;
using eBoardAPI.Entities;
using eBoardAPI.Models.Student;

namespace eBoardAPI.Helpers;

public static class StringHelper
{
    public static string ParseAcademicYear(DateOnly startDate, DateOnly endDate) => $"{startDate.Year}-{endDate.Year}";
    public static string ParseAcademicYear(Class @class) => ParseAcademicYear(@class.StartDate, @class.EndDate);

    public static string ParseFullAddress(string baseAddress, string ward, string district, string province)
    {
        var result = "";
        if (!string.IsNullOrWhiteSpace(baseAddress))
            result += $"{baseAddress}, ";
        
        if (!string.IsNullOrWhiteSpace(ward))
            result += $"{ward}, ";
        
        if (!string.IsNullOrWhiteSpace(district))
            result += $"{district}, ";
        
        if (!string.IsNullOrWhiteSpace(province))
            result += $"{province}";
        
        return result;
    }
    public static string ParseFullAddress(Student student) => ParseFullAddress(student.Address, student.Ward, student.District, student.Province);
    public static string ParseFullAddress(CreateStudentDto createStudentDto)
        => ParseFullAddress(createStudentDto.Address, createStudentDto.Ward, createStudentDto.District, createStudentDto.Province);
    
    public static string ScoreToGrade(double score)
    {
        return score switch
        {
            >= 9 => "Xuất sắc",
            >= 8 => "Giỏi",
            >= 7 => "Khá",
            >= 5 => "Trung bình",
            _ => "Yếu"
        };
    }

    public static string ConductAndScoreGradeToFinalGrade(string conduct, string grade)
    {
        return conduct switch
        {
            EConduct.GOOD => grade,
            EConduct.AVERAGE => grade switch
            {
                "Xuất sắc" => "Giỏi",
                "Giỏi" => "Khá",
                "Khá" => "Trung bình",
                "Trung bình" => "Yếu",
                _ => "Yếu"
            },
            _ => grade switch
            {
                "Xuất sắc" => "Khá",
                "Giỏi" => "Trung bình",
                "Khá" => "Yếu",
                _ => "Yếu"
            }
        };
    }
}