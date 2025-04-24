namespace Hospital.Application.Models.Doctors
{
    public class FilterDoctorRequest
{
    public List<int> Degrees { get; set; } = new();
    public List<int> Titles { get; set; } = new();
    public List<int> Ranks { get; set; } = new();
    public List<long> SpeIds { get; set; } = new();
    public List<int> Genders { get; set; } = new();
}
}
