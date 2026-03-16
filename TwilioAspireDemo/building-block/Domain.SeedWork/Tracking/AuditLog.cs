namespace Domain.SeedWork.Tracking;

public class AuditLog
{
    public int Id { get; set; }
    public string Table_Name{ get; set; }
    public string Key_Values { get; set; }
    public string Old_Values { get; set; }
    public string New_Values { get; set; }
    public string Action { get; set; }
    public string User_Name { get; set; }
    public DateTime Created_At { get; set; }
}
