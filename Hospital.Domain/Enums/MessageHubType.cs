namespace Hospital.Domain.Enums
{
    public enum MessageHubType
    {
        OnlineUser = 0,
        Message = 1,
        Notification = 2,
        Login = 3,
        Logout = 4,
        FindLogout = 5,
        UpdateRole = 6,
        UpdateAccount = 7,
        Maintaince = 8,
        Booking = 9,
        Reload = 10,
        ConfirmBooking = 11,
        CustomerCancelBooking = 12,
        EmployeeCancelBooking = 13,
        NextBooking = 14,
        CompleteBooking = 15,
    }
}
