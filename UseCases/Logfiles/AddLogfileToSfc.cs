using Common.ResultOf;
using Domain.Logfiles;
using System.Text.RegularExpressions;

namespace UseCases.Logfiles;

public class AddLogfileToSfc(
    ILogfileGateway logfileGateway,
    MoveLogfileToBackup moveLogfileToBackup
) : IUseCase<AddLogfileToSfcCommand, Logfile>
{
    private const RegexOptions RgxOptions = RegexOptions.IgnoreCase | RegexOptions.Multiline;
    private static readonly Regex RegexWrongStation = new(@"^go-.+[\r\n]+", RgxOptions);
    private static readonly Regex RegexIsOk = new(@"^ok[\r\n]+", RgxOptions);
    private static readonly Regex RegexIsEndOfFileError = new(@"end-of-file", RgxOptions);

    public async Task<ResultOf<Logfile>> ExecuteAsync(
        AddLogfileToSfcCommand command,
        CancellationToken ct = default)
    {
        if (!this.EnsureSfcPathCommunication())
        {
            return ResultOf<Logfile>.Failure(Error.ConnectionError);
        }

        return await logfileGateway.UploadOperationAsync(
                command.LogfileToUpload,
                command.MaxRetries,
                command.Timeout,
                ct)
            .Bind((responseLogfile, _) => MoveLogfileToBackup(responseLogfile, command.BackupDirectory, ct), ct)
            .Bind(responseLogfile => ExtractResult(responseLogfile, command.OkResponses));
    }

    private async Task<ResultOf<Logfile>> MoveLogfileToBackup(
        Logfile logfile,
        DirectoryInfo backupDirectory,
        CancellationToken ct)
    {
        return await moveLogfileToBackup.ExecuteAsync(
            new MoveLogfileToBackupCommand(
                Logfile: logfile,
                DestinationDirectory: backupDirectory),
            ct);
    }

    private ResultOf<Logfile> ExtractResult(Logfile responseLogfile, string okResponses)
    {
        if (RegexIsOk.Match(responseLogfile.Content).Success ||
            this.ContainsAdditionalOkSfcResponse(responseLogfile.Content, okResponses))
        {
            return responseLogfile;
        }

        if (RegexWrongStation.Match(responseLogfile.Content).Success)
        {
            return ResultOf<Logfile>.Failure(Error.WrongStation);
        }

        if (RegexIsEndOfFileError.Match(responseLogfile.Content).Success)
        {
            return ResultOf<Logfile>.Failure(Error.EndOfFile);
        }

        return ResultOf<Logfile>.Failure(Error.Fail);
    }

    private bool ContainsAdditionalOkSfcResponse(string content, string okResponses)
    {
        var responses = okResponses
            .Split(",", StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim().ToLower());
        var lowerContent = content.ToLower();

        foreach (var additionalResponse in responses)
        {
            if (lowerContent.Contains(additionalResponse))
            {
                return true;
            }
        }

        return false;
    }

    private bool EnsureSfcPathCommunication()
    {
        // TODO
        return true;
    }
}