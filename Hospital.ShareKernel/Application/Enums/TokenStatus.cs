namespace Hospital.SharedKernel.Application.Enums
{
    public enum TokenStatus
    {
        None = 0,
        Ok = 1,
        LoggedOut = 2,
        Banned = 3,
        ShouldRefresh = 4,
        ForceLogout = 5,
    }
}
