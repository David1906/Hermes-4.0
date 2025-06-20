using Common;
using Data.Sfc;
using Domain.Core.Types;
using Domain.Operations;
using ROP;
using System.Text.RegularExpressions;

namespace UseCases.Operations;

public class AddOperationToSfc(
    ISfcApi sfcApi
) : IUseCase<AddOperationToSfcRequest, AddOperationToSfcResponse>
{
    private const RegexOptions RgxOptions = RegexOptions.IgnoreCase | RegexOptions.Multiline;
    private static readonly Regex RegexWrongStation = new(@"^go-.+[\r\n]+", RgxOptions);
    private static readonly Regex RegexIsOk = new(@"^ok[\r\n]+", RgxOptions);
    private static readonly Regex RegexIsEndOfFileError = new(@"end-of-file", RgxOptions);

    public async Task<Result<AddOperationToSfcResponse>> ExecuteAsync(
        AddOperationToSfcRequest request,
        CancellationToken ct = default)
    {
        if (!this.EnsureSfcPathCommunication())
        {
            return new AddOperationToSfcResponse(UploadResultType.ConnectionError);
        }

        return await sfcApi.UploadOperationAsync(request, ct)
            .Map(textDocument => new AddOperationToSfcResponse(
                this.ExtractUploadResult(textDocument, request.OkResponses),
                textDocument.FileInfo));
    }

    private UploadResultType ExtractUploadResult(TextDocument textDocument, string okResponses)
    {
        if (RegexIsOk.Match(textDocument.Content).Success ||
            this.ContainsAdditionalOkSfcResponse(textDocument.Content, okResponses))
        {
            return UploadResultType.Ok;
        }

        if (RegexWrongStation.Match(textDocument.Content).Success)
        {
            return UploadResultType.WrongStation;
        }

        if (RegexIsEndOfFileError.Match(textDocument.Content).Success)
        {
            return UploadResultType.EndOfFile;
        }

        return UploadResultType.Fail;
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