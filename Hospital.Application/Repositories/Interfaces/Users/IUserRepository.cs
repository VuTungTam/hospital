namespace Hospital.Application.Repositories.Interfaces.Users
{
    public interface IUserRepository
    {
        Task<bool> PhoneExistAsync(string phone, long exceptId = 0, CancellationToken cancellationToken = default);

        Task<bool> EmailExistAsync(string email, long exceptId = 0, CancellationToken cancellationToken = default);

        Task<bool> CodeExistAsync(string code, long exceptId = 0, CancellationToken cancellationToken = default);
    }
}
