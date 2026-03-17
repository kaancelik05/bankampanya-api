using Bankampanya.Domain.Common;

namespace Bankampanya.Domain.Entities;

public class AssistantPromptTemplate : PublishableEntity
{
    public string Text { get; set; } = string.Empty;
    public string Tone { get; set; } = string.Empty;
}
