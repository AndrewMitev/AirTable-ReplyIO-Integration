namespace AirReplyAPI.Data.Interfaces
{
    public interface IAirtableService
    {
        Task<bool> IsClientContactedAsync(string? baseId, string? clientId);

        Task<HttpResponseMessage> UpdateContact(object fields, string? baseId, string? airtableClientId);
    }
}
