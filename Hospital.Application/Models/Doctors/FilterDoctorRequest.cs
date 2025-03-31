namespace Hospital.Application.Models.Doctors
{
    public class FilterDoctorRequest
    {
        public int Degree;

        public int Title;

        public int Rank;

        public List<long> SpeIds;
    }
}
