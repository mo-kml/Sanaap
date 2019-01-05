using Plugin.Media.Abstractions;
using System.Threading.Tasks;

namespace Sanaap.App.Helpers.Contracts
{
    public interface IPhotoHelper
    {
        Task<MediaFile> TakePhoto();
    }
}
