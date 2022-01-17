using AirReplyAPI.Data.Models;
using AirReplyAPI.Data.RequestModels;

namespace AirReplyAPI.Data.Interfaces
{
    public interface IReplyIOService
    {
        Task<UpdateSentStatus> OnSent_GetClientDataFromReplyAsync(EmailSentResponse sentEmailResponse);

        Task<UpdateRepliedStatus> OnReplied_GetClientDataFromReplyAsync(EmailRepliedResponse emailReplied);
    }
}
