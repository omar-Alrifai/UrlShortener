public class UniqueCodeGeneratorService : IUniqueCodeGeneratorService
{
    private readonly IShortLinkRepository _shortLinkRepository;
    private readonly ICodeGeneratorService _codeGeneratorSevice;
    private const int MaxAttempts = 10;
    public UniqueCodeGeneratorService(IShortLinkRepository repository, ICodeGeneratorService service)
    {
        _shortLinkRepository = repository;
        _codeGeneratorSevice = service;
    }

    public async Task<string> GenerateUniqueCodeAsync()
    {
        for (int i = 0; i < MaxAttempts; i++)
        {
            string code = _codeGeneratorSevice.Generate();
            if (!await _shortLinkRepository.ExistsByCodeAsync(code))
                return code;
        }
        throw new InvalidOperationException("Could not generate unique short code after multiple attempts.");
    }
}