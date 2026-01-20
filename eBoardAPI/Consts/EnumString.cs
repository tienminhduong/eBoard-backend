namespace eBoardAPI.Consts;

public static class ERelationshipWithParent
{
    public const string FATHER = "Cha";
    public const string MOTHER = "Mẹ";
    public const string GUARDIAN = "Người giám hộ";
}

public static class EGender
{
    public const string MALE = "Nam";
    public const string FEMALE = "Nữ";
}

public static class EContributionStatus
{
    public const string CONTRIBUTED = "Đã đóng";
    public const string PARTIALLY_CONTRIBUTED = "Đóng một phần";
    public const string NOT_CONTRIBUTED = "Chưa đóng";
}

public static class EAttendanceStatus
{
    public const string PRESENT = "Có mặt";
    public const string ABSENT = "Vắng không phép";
    public const string EXCUSED = "Vắng có phép";
}

public static class EAbsentRequestStatus
{
    public const string PENDING = "Chờ duyệt";
    public const string APPROVED = "Đã duyệt";
    public const string REJECTED = "Từ chối";
}

public static class EDayOfWeek
{
    public const string MONDAY = "Thứ Hai";
    public const string TUESDAY = "Thứ Ba";
    public const string WEDNESDAY = "Thứ Tư";
    public const string THURSDAY = "Thứ Năm";
    public const string FRIDAY = "Thứ Sáu";
    public const string SATURDAY = "Thứ Bảy";
    public const string SUNDAY = "Chủ Nhật";
}

public static class EExamFormat
{
    public const string MIDTERM = "Giữa kỳ";
    public const string FINAL = "Cuối kỳ";
}