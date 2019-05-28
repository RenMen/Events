using System.Threading.Tasks;

namespace WebApp_OpenIDConnect_DotNet.Services.GraphOperations
{
    public interface IGraphApiOperations
    {
        Task<dynamic> GetUserInformation();
        //Task<string> GetPhotoAsBase64Async();
    }
}