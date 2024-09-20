public interface IValidation
{
    bool IsValid();
    string ValidationMessage { get; }
}
