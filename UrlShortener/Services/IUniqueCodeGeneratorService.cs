public interface IUniqueCodeGeneratorService
{
    Task<string> GenerateUniqueCodeAsync();
}