namespace eBoardAPI.Models.ScoreSheet;

public class UpdateIndividualStudentScoreSheetDto
{
    public IEnumerable<UpdateSubjectScoreDto> SubjectScores { get; set; } = [];
}