namespace VidHub.Core.Utilities.Internal
{
    internal class ProcessResult(int exitCode, string standardOutput, string standardError)
    {
        public int ExitCode { get; } = exitCode;
        public string StandardOutput { get; } = standardOutput;
        public string StandardError { get; } = standardError;
        public bool Successful => exitCode == 0 && string.IsNullOrEmpty(standardError);
        public bool Failure => exitCode != 0 || !string.IsNullOrEmpty(standardError);
    }
}
