using Atlas.Law.Application.CQRS.LegalDocuments.Commands.CreateLegalDocuments;
using Atlas.Law.Domain.Entities.LegalDocumentEntity;
using Atlas.Shared.Application.Abstractions;
using Atlas.Shared.Application.Abstractions.Messaging.Command;
using Atlas.Shared.Application.Queue;
using DocumentFormat.OpenXml.Packaging;
using System.Text;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace Atlas.Law.Application.CQRS.LegalDocuments.Commands.PrepareLegalDocumentSummary;

internal sealed class CreateLegalDocumentsCommandHandler(ILegalDocumentRepository legalDocumentRepository, IExecutionContextAccessor executionContext) : ICommandHandler<CreateLegalDocumentsCommand, IEnumerable<string>>
{
    public async Task<IEnumerable<string>> Handle(CreateLegalDocumentsCommand request, CancellationToken cancellationToken)
    {
        var failedFiles = new List<string>();
        var processedFileNames = new HashSet<string>();

        foreach (var document in request.Documents)
        {
            // Check for duplicate file names
            if (processedFileNames.Contains(document.FileName))
            {
                failedFiles.Add(document.FileName);
                continue;
            }

            processedFileNames.Add(document.FileName); // Add the file name to processed names

            byte[] bytes = Convert.FromBase64String(document.Base64Data);

            if (!IsByteArrayWithinLimit(bytes, 10 * 1024 * 1024)) // 10 MB limit
            {
                failedFiles.Add(document.FileName);
                continue;
            }

            string? text = TextExtractor(bytes, document.MimeType);
            if (string.IsNullOrEmpty(text))
            {
                failedFiles.Add(document.FileName);
                continue;
            }

            var legalDocument = await LegalDocument.CreateAsync(document.FileName, text, "en-GB", document.MimeType, executionContext.UserId, legalDocumentRepository);
            await legalDocumentRepository.AddAsync(legalDocument, cancellationToken);
        }

        return failedFiles;
    }

    /// <summary>
    /// Checks whether the length of the byte array is within the specified limit.
    /// </summary>
    /// <param name="byteArray">The byte array to check.</param>
    /// <param name="limitInBytes">The limit in bytes.</param>
    /// <returns>True if the byte array is within the limit, false otherwise.</returns>
    private static bool IsByteArrayWithinLimit(byte[] byteArray, long limitInBytes)
    {
        return byteArray.Length <= limitInBytes;
    }

    /// <summary>
    /// Extracts text from various file types.
    /// </summary>
    /// <param name="bytes">The bytes of the file from which the text should be extracted.</param>
    /// <param name="mimeType">The mime type of the <paramref name="bytes"/> file.</param>
    /// <returns>The extracted text.</returns>
    private static string? TextExtractor(byte[] bytes, string mimeType) 
    {
        switch (mimeType)
        {
            case "text/plain":
            {
                return Encoding.UTF8.GetString(bytes);
            }
            case "application/pdf":
            {
                return PdfTextExtractor(bytes);
            }
            case "application/vnd.openxmlformats-officedocument.wordprocessingml.document":
            {
                 return DocxTextExtractor(bytes);
            }
        }

        return null;
    }

    /// <summary>
    /// Extracts the text from a PDF file.
    /// </summary>
    /// <param name="bytes">The bytes of the PDf file.</param>
    /// <returns>The extracted text.</returns>
    private static string? PdfTextExtractor(byte[] bytes)
    {
        var stringBuilder = new StringBuilder();

        using var document = PdfDocument.Open(bytes);

        foreach (Page page in document.GetPages())
        {
            var wordsList = page.GetWords().GroupBy(x => x.BoundingBox.Bottom);

            foreach (var word in wordsList)
            {
                foreach (var item in word)
                {
                    stringBuilder.Append(item.Text + " ");
                }
                stringBuilder.Append('\n');
            }
        }

        return stringBuilder.ToString();
    }

    /// <summary>
    /// Extracts the text from a Docx file (Microsoft Word file).
    /// </summary>
    /// <param name="bytes">The bytes of the Docx file.</param>
    /// <returns>The extracted text.</returns>
    private static string? DocxTextExtractor(byte[] bytes)
    {
        using var fileStream = new MemoryStream(bytes);

        // open the document
        using var doc = WordprocessingDocument.Open(fileStream, false);

        var stringBuilder = new StringBuilder();

        foreach(var paragraph in doc.MainDocumentPart.Document.Body.ChildElements)
        {
            stringBuilder.AppendLine(paragraph.InnerText);   
        }

        return stringBuilder.ToString();
    }
}
